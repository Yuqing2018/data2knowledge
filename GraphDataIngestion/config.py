# -*- coding: utf-8 -*-
import os
import sys
import json
from bson import ObjectId
from datetime import datetime
import logging
from logging.handlers import RotatingFileHandler

import requests
from apscheduler.schedulers.background import BackgroundScheduler
from flask_apscheduler import APScheduler
from flask_apscheduler.scheduler import EVENT_ALL
from apscheduler.jobstores.memory import MemoryJobStore
from apscheduler.executors.pool import ThreadPoolExecutor

from app.utils.mongo_operator import MongoOperator
from app.utils.utils import login_music_kg

basedir = os.path.abspath(os.path.dirname(__file__))


class InfoFilter(logging.Filter):
    def filter(self, record):
        if logging.INFO <= record.levelno < logging.ERROR:
            return super().filter(record)
        else:
            return 0


class Config:
    AUTHOR = 'shipeiyu'
    SECRET_KEY = 'hard to guess'
    # mongo
    MONGODB_HOST = "localhost"
    # MONGODB_HOST = ""
    MONGODB_PORT = 27017
    MONGODB_CONF_DATABASE = "InfoCollectorConfs"
    JOB_CONF_COLL_NAME = "jobs"
    GROUP_CONF_COLL_NAME = "groups"
    SCHED_CONF_COLL_NAME = "scheds"
    MONGODB_INFO_DATABASE = "Infos"
    ARTICLE_COLL_NAME = "articles"
    LOG_COLL_NAME = "logs"
    # mysql
    MYSQL_HOST = ""
    MYSQL_PORT = 3306
    MYSQL_USERNAME = ""
    MYSQL_PASSWORD = ""
    MYSQL_DATABASE = ""
    # redis
    REDIS_HOST = "localhost"
    # REDIS_HOST = ""
    REDIS_PORT = 6379
    REDIS_DATABASE = "0"
    # file path
    BACKUP_PATH = os.path.join(basedir, 'db', 'backups')
    LOG_PATH = os.path.join(basedir, 'logs')
    ARTICLE_PATH = os.path.join(basedir, 'articles')
    ZIPFILE_PATH = os.path.join(basedir, 'zipfiles')
    LOG_PATH_ERROR = os.path.join(LOG_PATH, 'error.log')
    LOG_PATH_INFO = os.path.join(LOG_PATH, 'info.log')
    LOG_FILE_MAX_BYTES = 2 * 1024 * 1024
    LOG_FILE_BACKUP_COUNT = 20
    # scheduler
    SCHEDULER_EXECUTORS = {
        'default': {'type': 'threadpool', 'max_workers': 20}
    }
    SCHEDULER_JOB_DEFAULTS = {
        'coalesce': False,
        'max_instances': 80,
        'misfire_grace_time': 800
    }
    SCHEDULER_API_ENABLED = True
    # post url
    KG_BASE_URL = "https://labeltool.eastasia.cloudapp.azure.com:35000"
    PATH_LOGIN = "/api/User/Login"
    PATH_DOCUMENT = "/api/Workspace/workspaceid/Document/Content"
    LOGIN_DATA = {
        "name": "manager",
        "password": "More$4*"
    }

    @classmethod
    def init_scheduler(cls, app=None):
        # listen and log job status
        def sche_listener(event):
            logger = app.logger
            if event.code == 2 ** 0:
                logger.info("scheduler started")
            if event.code == 2 ** 1:
                logger.info('scheduler shutdown')
            if event.code == 2 ** 2:
                logger.info('scheduler paused')
            if event.code == 2 ** 3:
                logger.info('scheduler resumed')
            if event.code == 2 ** 4:
                logger.info('event executor added')
            if event.code == 2 ** 5:
                logger.info('event executor removed')
            if event.code == 2 ** 6:
                logger.info('job store added')
            if event.code == 2 ** 7:
                logger.info('job store removed')
            if event.code == 2 ** 8:
                logger.info('all jobs removed')
            if event.code == 2 ** 9:
                logger.info('event job added, job id: {}'.format(event.job_id))
                # mo = MongoOperator(host=cls.MONGODB_HOST, port=cls.MONGODB_PORT, db=cls.MONGODB_INFO_DATABASE)
                # data = {'time': datetime.now().strftime('%Y-%m-%d %H:%M:%S'), 'event': 'added', 'id': event.job_id}
                # mo.replace(collectionname=cls.LOG_COLL_NAME, data=data, field_name='event')
                # mo.close()
            if event.code == 2 ** 10:
                logger.info('event job removed, job id: {}'.format(event.job_id))
            if event.code == 2 ** 11:
                logger.info('event job modified, job id: {}'.format(event.job_id))
            if event.code == 2 ** 12:
                logger.info('event job executed, job id: {}'.format(event.job_id))
                e = event
                # if the task executed then post the txts to /api/Workspace/{workspaceId}/Document/Content
                mo = MongoOperator(host=cls.MONGODB_HOST, port=cls.MONGODB_PORT, db=cls.MONGODB_INFO_DATABASE)
                # get latest crawl result log
                log = mo.get_sorted_n(collectionname=cls.LOG_COLL_NAME, field_name="task_id", field_value=event.job_id, sort_field="end_time", sort=-1, n=1)
                articles_dir = cls.ARTICLE_PATH
                if log:
                    inserted_urls = log[0]["inserted_urls"]
                    articles_names = []
                    for i in inserted_urls:
                        article = mo.get_one(collectionname=cls.ARTICLE_COLL_NAME, field_name="url", field_value=i)
                        if article:
                            # if article exisoted
                            # step1 login music KG
                            token = ""
                            try:
                                token = login_music_kg(url_login=cls.KG_BASE_URL + cls.PATH_LOGIN, login_data=cls.LOGIN_DATA)
                            except Exception as err:
                                logger.error( 'cannot get token from: {}'.format(cls.KG_BASE_URL + cls.PATH_LOGIN))
                            # step2 construct  workspaceid  tag txtfile dirs to upload crawl results
                            article_dir = articles_dir + os.sep + str(article["_id"]) + ".txt"
                            articles_names.append(article_dir)
                            article_sched_id = article["sched_id"]
                            conf_mo = MongoOperator(host=cls.MONGODB_HOST, port=cls.MONGODB_PORT, db=cls.MONGODB_CONF_DATABASE)
                            sched = conf_mo.get_one(collectionname=cls.SCHED_CONF_COLL_NAME, field_name="_id", field_value=ObjectId(article_sched_id))
                            conf = conf_mo.get_one(collectionname=cls.JOB_CONF_COLL_NAME, field_name="_id", field_value=ObjectId(sched["conf"]))
                            conf_mo.close()
                            workspaceid = conf.get("workspace_id","1")
                            tag = str(conf.get("_id")) + " " + log[0].get("end_time").split(" ")[0]
                            url = cls.KG_BASE_URL + cls.PATH_DOCUMENT
                            data = {"workspaceid": workspaceid, "tags": tag}
                            headers = {'Authorization': "Bearer " + token if token else ""}
                            files = dict()
                            r = "response"
                            try:
                                with open(article_dir, "rb") as f:
                                    files = {"files": (str(article["_id"]) + ".txt", f)}
                                    r = requests.post(url.replace("workspaceid", workspaceid), data, files=files, headers=headers)
                                if r.status_code == 200:
                                    logger.info('posted the article: {0} to {1}'.format(str(article["_id"]) + ".txt", url))
                                else:
                                    logger.info('posted article: {0} failed, status code: {1}, response content: {2}'.format(str(article["_id"]) + ".txt", r.status_code, r.content.decode('utf-8')))
                            except Exception as err:
                                logger.error(err + 'posting article error while request url {}'.format(url))
                            # step3 remove temp txt that has been posted
                            try:
                                os.remove(article_dir)
                            except Exception as err:
                                logger.error(err)
                mo.close()
            if event.code == 2 ** 13:
                logger.error('event job error' + event.traceback)
            if event.code == 2 ** 14:
                logger.error('missed job, job_id: {0}, schedule_run_time: {1}'.format(event.job_id, event.scheduled_run_time))
            if event.code == 2 ** 15:
                logger.info('event job submitted, job id: {}'.format(event.job_id))
            if event.code == 2 ** 16:
                logger.error('reached maximum of running instances, job_id: {}, took params long time to crawl'.format(event.job_id))
        scheduler = APScheduler()
        scheduler._logger = app.logger
        scheduler.init_app(app)
        scheduler.add_listener(sche_listener, EVENT_ALL)
        scheduler.start()

    @classmethod
    def init_app(cls, app):
        app.logger.level = logging.INFO
        formater = logging.Formatter('%(asctime)s %(levelname)s %(process)d %(thread)d %(pathname)s %(lineno)s --> %(message)s')
        # StreamHandler Info
        stream_handler_info = logging.StreamHandler(stream=sys.stdout)
        stream_handler_info.setFormatter(formater)
        stream_handler_info.setLevel(logging.INFO)
        stream_handler_info.addFilter(InfoFilter())
        app.logger.addHandler(stream_handler_info)
        # StreamHandler Error
        stream_handler_error = logging.StreamHandler(stream=sys.stdout)
        stream_handler_error.setFormatter(formater)
        stream_handler_error.setLevel(logging.ERROR)
        app.logger.addHandler(stream_handler_error)
        cls.init_scheduler(app)


class DevConfig(Config):
    DEBUG = True


class ProdConfig(Config):
    DEBUG = False
    MONGODB_HOST = 'mongodb'
    KG_BASE_URL = "http://webapi:5000"


class TestConfig(Config):
    pass


config = {
    'dev': DevConfig,
    'test': TestConfig,
    'prod': ProdConfig,
    'default': DevConfig,
}
