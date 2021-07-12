# -*- coding: utf-8 -*-
from flask import Flask

from config import config
from .confmanager.configManager import configManager
from apscheduler.jobstores.mongodb import MongoDBJobStore
from pymongo import MongoClient


def create_app(config_name='dev'):
    app = Flask(__name__)

    app.config.from_object(config[config_name])
    app.config['SCHEDULER_JOBSTORES'] = {
      'default': MongoDBJobStore(collection='aps_jobstore',
                                 database='music_jobs',
                                 client=MongoClient(host=config[config_name].MONGODB_HOST, port=config[config_name].MONGODB_PORT),
                                 pickle_protocol=1.0)
    }

    config[config_name].init_app(app)
    app.register_blueprint(configManager)
    return app
