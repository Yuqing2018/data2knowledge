# -*- coding:utf-8 -*-
import os
import datetime
from bson.objectid import ObjectId

from app.utils.mongo_operator import *
from app.confmanager.spiders import ArticlesSpider


def crawl_task(app_config=None, sched_id=None, task_id=None):
    # get corresponding configs from app_config
    host = app_config['MONGODB_HOST']
    port = app_config['MONGODB_PORT']
    # InfoCollectorConfs
    all_conf_db = app_config['MONGODB_CONF_DATABASE']
    # jobs
    conf_col = app_config['JOB_CONF_COLL_NAME']
    # groups
    group_col = app_config['JOB_CONF_COLL_NAME']
    # scheds
    sched_col = app_config['SCHED_CONF_COLL_NAME']
    # Infos
    info_db = app_config['MONGODB_INFO_DATABASE']
    # articles
    article_col = app_config['ARTICLE_COLL_NAME']
    # logs
    log_col = app_config['LOG_COLL_NAME']
    # find the sched conf
    mongo_operator = MongoOperator(host=host, port=port, db=all_conf_db)
    sched = mongo_operator.get_one(collectionname=sched_col, field_name="_id", field_value=ObjectId(sched_id))
    if sched:
        sched_conf = sched["conf"]
        all_confs = [conf for conf in mongo_operator.get_all_data(collectionname=conf_col)]
        required_conf = dict()
        for conf in all_confs:
            if str(conf["_id"]) == sched_conf:
                required_conf = conf
        conf_name = required_conf["conf_name"]
        # crawl data accoreding to conf and log the task infos
        articles = list()
        inserted_articles = list()
        start_time = datetime.datetime.now()
        for item in required_conf["items"]:
            page_url = item["page_url"]
            target_xpath = item["target_xpath"]
            is_stroll = item["is_stroll"]
            article_title_xpath = item["article_title_xpath"]
            article_content_xpath = item["article_content_xpath"]
            article = ArticlesSpider(page_url=page_url, target_xpath=target_xpath, stroll=is_stroll,
                                     article_title_xpath=article_title_xpath, article_content_xpath=article_content_xpath,
                                     app_config=app_config, sched_id=sched_id)
            a, ia = article.pipeline_articles()
            articles.extend(a)
            inserted_articles.extend(ia)
        end_time = datetime.datetime.now()
        articles_number = len(articles)
        inserted_number = len(inserted_articles)
        status = 0 if articles_number > 0 else 1
        data = dict()
        data["sched_id"] = sched_id
        data["task_id"] = task_id
        data["conf_id"] = sched["conf"]
        data["conf_name"] = conf_name
        data["start_time"] = start_time.strftime("%Y-%m-%d %H:%M:%S")
        data["end_time"] = end_time.strftime("%Y-%m-%d %H:%M:%S")
        data["articles"] = [article["url"] for article in articles]
        data["inserted_urls"] = [article["url"] for article in inserted_articles]
        data["articles_number"] = articles_number
        data["inserted_number"] = inserted_number
        # store the exec log
        mongo_operator = MongoOperator(host=host, port=port, db=info_db)
        mongo_operator.insert_one(collectionname=log_col, data=data)
        mongo_operator.close()

