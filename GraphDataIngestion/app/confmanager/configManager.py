# -*- coding:utf-8 -*-
import json
import datetime
from copy import deepcopy

from flask import request
from flask import current_app
from flask import send_from_directory
from flask import make_response

from . import configManager
from app.confmanager.handler import *
from app.confmanager.task import *
from app.confmanager.task import crawl_task
from app.utils.utils import *


@configManager.before_request
def br():
    pass


@configManager.route('/createconf', methods=['POST'])
def create_conf():
    cur_env = request.__dict__['environ']
    current_app.logger.info(' '.join([request.remote_addr + ':', str(cur_env['REMOTE_PORT']), cur_env['REQUEST_METHOD'], cur_env['PATH_INFO'], cur_env['SERVER_PROTOCOL']]))
    config = dict(current_app.config)
    response = {'message': None, 'code': None}
    request_json = request.get_json()
    """
        the request_json data may like this:
        {
            "conf_name":"test1",
            "workspace_id":"1",
            "desc": "腾讯娱乐滚动新闻",
            "items":
                [
                    {
                      "areaName":"infinite stroll",
                      "page_url": "https://new.qq.com/ch/ent/",
                      "target_xpath": "//div[@id='List']/div/ul[@class='list' and @data-seq]",
                      "article_title_xpath": "//div[@class='LEFT']/h1",
                      "article_content_xpath": "//div[@class='content-article']",
                      "is_stroll": 1
                    }
                ]
        }
        {
            "conf_name":"test2",
            "workspace_id":"1",
            "desc": "腾讯音乐热门、原创新闻",
            "items":
                [
                    {
                      "areaName":"focus",
                      "page_url": "https://new.qq.com/ch2/music",
                      "target_xpath": "//div[@class='focus3']",
                      "article_title_xpath": "//div[@class='LEFT']/h1",
                      "article_content_xpath": "//div[@class='content-article']",
                      "is_stroll": 0
                    },
                    {
                      "areaName":"Original",
                      "page_url": "https://new.qq.com/ch2/music",
                      "target_xpath": "//div[@class='ori-bar ori-ent-bar']",
                      "article_title_xpath": "//div[@class='LEFT']/h1",
                      "article_content_xpath": "//div[@class='content-article']",
                      "is_stroll": 0
                    }
                ]
        }
    """
    if not request.json:
        return json.dumps({"message": "cannot get any config data", "code": 400})
    conf_name = request_json.get("conf_name", "")
    # job_name is required
    if not conf_name:
        return json.dumps({"message": "conf name is required", "code": 400})
    wsid = request_json.get("workspace_id", "")
    if not conf_name:
        return json.dumps({"message": "workspace id required", "code": 400})
    items = request_json.get("items", [])
    # items is required
    if not items:
        return json.dumps({"message": "items is required", "code": 400})
    # validate items
    if items:
        for item in items:
            page_url = item.get("page_url", "")
            if not page_url:
                return json.dumps({"message": "for item {}, page page_url is required".format(str(items.index(item) + 1)), "code": 400})
            target_xpath = item.get("target_xpath", "")
            if not target_xpath:
                return json.dumps({"message": "for item {}, target xpath is required".format(str(items.index(item) + 1)), "code": 400})
            article_title_xpath = item.get("article_title_xpath", None)
            if not article_title_xpath:
                return json.dumps({"message": "for item {}, article title xpath is required".format(str(items.index(item) + 1)), "code": 400})
            article_content_xpath = item.get("article_content_xpath", None)
            if not article_content_xpath:
                return json.dumps({"message": "for item {}, article content xpath is required".format(str(items.index(item) + 1)), "code": 400})
    conf_handler = ConfHandler(data=request_json, app_config=config)
    res = conf_handler.save_conf(field_name="conf_name")
    if res['existing']:
        current_app.logger.info("{} existed, save conf failed".format(conf_name))
        response["message"] = "{} existed, save job failed".format(conf_name)
        response["code"] = 202
    else:
        current_app.logger.info("conf: {0} created successfully".format(conf_name))
        response["message"] = "conf: {0} created successfully".format(res['data']['conf_name'])
        response["code"] = 201
    return json.dumps(response, ensure_ascii=False)


@configManager.route('/confs', methods=['get'])
def get_all_confs():
    cur_env = request.__dict__['environ']
    current_app.logger.info(' '.join([request.remote_addr + ':', str(cur_env['REMOTE_PORT']), cur_env['REQUEST_METHOD'], cur_env['PATH_INFO'], cur_env['SERVER_PROTOCOL']]))
    app_config = dict(current_app.config)
    conf_handler = ConfHandler(data={}, app_config=app_config)
    res_list = conf_handler.get_all_confs()
    res = {'message': res_list, 'code': 200}
    return json.dumps(res, ensure_ascii=False)


@configManager.route('/conf', methods=['get'])
def get_conf():
    conf_id = request.args.get('conf_id')
    if not conf_id:
        return json.dumps({"message": "conf id is needed", "code": 400})
    if len(conf_id) != 24:
        return json.dumps({"message": "InvalidId: {} is not params valid ObjectId, it must be params 12-byte input or params 24-character hex string".format(conf_id), "code": 400})
    cur_env = request.__dict__['environ']
    current_app.logger.info(' '.join([request.remote_addr + ':', str(cur_env['REMOTE_PORT']), cur_env['REQUEST_METHOD'], cur_env['PATH_INFO'], conf_id, cur_env['SERVER_PROTOCOL']]))
    app_config = dict(current_app.config)
    conf_handler = ConfHandler(data={}, app_config=app_config)
    conf = conf_handler.get_conf(conf_id=conf_id)
    if conf:
        res = {'message': conf, 'code': 200}
    else:
        res = {'message': 'conf {} do not existed'.format(conf_id), 'code': 202}
    return json.dumps(res, ensure_ascii=False)


@configManager.route('/deleteconf', methods=['GET', 'DELETE'])
def delete_conf():
    """
    A job can be deleted while it is not used by any group
    """
    res = {'message': 'deleted', 'code': 200}
    conf_id = request.args['conf_id']
    cur_env = request.__dict__['environ']
    current_app.logger.info(' '.join([request.remote_addr + ':', str(cur_env['REMOTE_PORT']), cur_env['REQUEST_METHOD'], cur_env['PATH_INFO'], conf_id, cur_env['SERVER_PROTOCOL']]))
    app_config = dict(current_app.config)
    conf_handler = ConfHandler(data={}, app_config=app_config)
    res = conf_handler.del_conf(conf_id=conf_id)
    if not res.get("conf"):
        res = {'message': 'no such conf {}'.format(conf_id), 'code': 202}
    else:
        if not res['usages']:
            current_app.logger.info('conf {} deleted'.format(conf_id))
            res = {'message': 'conf {} deleted'.format(conf_id), 'code': 200}
        else:
            current_app.logger.info('delete failed, conf {} is being used by sched(s): {}'.format(conf_id, ','.join(res['usages'])))
            res = {'message': 'delete failed, conf {} is being used by sched(s): {}'.format(conf_id, ','.join(res['usages'])), 'code': 202}
    return json.dumps(res, ensure_ascii=False)


@configManager.route('/editconf', methods=['POST'])
def edit_conf():
    config = dict(current_app.config)
    response = {'message': None, 'code': None}
    conf_id = request.args['conf_id']
    data = request.get_json()
    cur_env = request.__dict__['environ']
    current_app.logger.info(' '.join([request.remote_addr + ':', str(cur_env['REMOTE_PORT']), cur_env['REQUEST_METHOD'], cur_env['PATH_INFO'], "edit", conf_id, cur_env['SERVER_PROTOCOL']]))
    app_scheduler = current_app.apscheduler
    conf_handler = ConfHandler(data=data, app_config=config)
    conf = conf_handler.get_conf(conf_id=conf_id)
    all_conf_names = [conf["conf_name"] for conf in conf_handler.get_all_confs()]
    if conf["conf_name"] != data.get("conf_name", ""):
        if data.get("conf_name", "") in all_conf_names:
            response = {'message': "conf name {} existed".format(data["conf_name"]), 'code': 202}
            return json.dumps(response, ensure_ascii=False)
    if conf:
        keys_canbe_edited = list(conf.keys())
        keys_canbe_edited.remove("_id")
        keys_canbe_edited.remove("create_time")
        keys_canbe_edited.remove("usages")
        keys_needtobe_edited = data.keys()
        for kn in keys_needtobe_edited:
            if kn not in keys_canbe_edited:
                response = {'message': "some fields cannot be edited", 'code': 202}
                return json.dumps(response, ensure_ascii=False)
            else:
                pass
        # pause the job before editing
        for sched in conf['usages']:
            if app_scheduler.get_job(sched):
                try:
                    app_scheduler.pause_job(sched)
                    current_app.logger.info('sched {0} paused before editing job {1} in it'.format(sched, conf['conf_name']))
                except Exception as e:
                    current_app.logger.info('pause sched {0} failed before editing job {1} in it'.format(sched, conf['con_name']))
        # edit job conf
        res = conf_handler.edit_conf(conf_id=conf_id, data=data)
        if res.modified_count == 1:
            response['message'] = "successfully updated"
            response['code'] = 200
            current_app.logger.info("successfully updated conf：id={}".format(conf_id))
        elif res.modified_count == 0:
            response['message'] = "successfully updated, but nothing changed"
            response['code'] = 202
            current_app.logger.info("successfully updated conf：id={}, but nothing changed".format(conf_id))
        # resume all the tasks related to
        for sched in conf['usages']:
            if app_scheduler.get_job(sched):
                try:
                    app_scheduler.resume_job(sched)
                    current_app.logger.info('sched {0} resumed after editing conf {1}'.format(sched, conf['job_name']))
                except Exception as e:
                    current_app.logger.info('resume sched {0} failed after editing conf {1} in it'.format(sched, conf['job_name']))
    else:
        response['message'] = "no such conf：conf id={}".format(conf_id)
        response['code'] = 202
        current_app.logger.info("no such conf：conf id={}".format(conf_id))
    return json.dumps(response, ensure_ascii=False)


@configManager.route('/createsched', methods=['POST'])
def create_sched():
    cur_env = request.__dict__['environ']
    current_app.logger.info(' '.join([request.remote_addr + ':', str(cur_env['REMOTE_PORT']), cur_env['REQUEST_METHOD'], cur_env['PATH_INFO'], cur_env['SERVER_PROTOCOL']]))
    config = dict(current_app.config)
    response = {'message': None, 'code': None}
    request_json = request.get_json()
    """
        {
            "sched_name": "sched1",
            "conf": "",
            "start_date": "2019-01-1 00:00:00",
            "end_date": "2020-01-1 00:00:00",
            "day_of_week": "0,1,2,3,4,5,6",
            "hm":["9:0","10:0","11:0","12:0","13:0","14:0","15:0","16:0","17:0","18:0","19:0","20:0","21:0","22:0","23:0","0:0","1:0","2:0","3:0","4:0","5:0","6:0","7:0","8:0"]
        }
    """
    if not request.json:
        return json.dumps({"message": "cannot get any config data", "code": 400})
    # sched name and jobconf list are required
    sched_name = request_json.get("sched_name", "")
    if not sched_name:
        return json.dumps({"message": "sched name is required", "code": 400})
    conf = request_json.get("conf", [])
    if not conf:
        return json.dumps({"message": "conf is required", "code": 400})
    # time span, default 2019-01-1~2029-01-01
    start_date = request_json.get('start_date', '2019-01-1 00:00:00')
    end_date = request_json.get('end_date', '2090-01-02 00:00:00')
    trigger = dict()
    # cron fields
    # 0 15 10 ? * 0-4: execute at 10:15 am every Monday to Friday
    trigger["year"] = request_json.get("year", "*")
    trigger["month"] = request_json.get("month", "*")
    trigger["week"] = request_json.get("week", "*")
    trigger["day"] = request_json.get("day_of_month", "")
    trigger["day_of_week"] = request_json.get("day_of_week", "*")
    # trigger["hour"] = request_json.get("hour", "*")
    # trigger["minute"] = request_json.get("minute", "*")
    trigger["hm"] = request_json.get("hm", ["19:00", "20:00", "21:00", "22:00", "23:00", "24:00", "1:00", "2:00", "3:00", "4:00", "5:00"])
    trigger["second"] = request_json.get("second", "*")
    trigger["timezone"] = request_json.get("timezone", "")
    trigger["jitter"] = request_json.get("jitter", "")
    group_handler = SchedHandler(sched_name=sched_name, conf=conf, start_date=start_date, end_date=end_date, app_config=config, **trigger)
    res = group_handler.save_sched(field_name="sched_name")
    if res.get("status"):
        current_app.logger.info("ERROR: conf(s) {} not exist".format(res["data"]))
        response["message"] = "ERROR: conf(s) {} not exist".format(res["data"])
        response["code"] = 202
    elif res['existing']:
        current_app.logger.info("sched {} existed, save sched failed".format(sched_name))
        response["message"] = "sched {} existed, save sched failed".format(sched_name)
        response["code"] = 202
    elif not res["existing"]:
        # gen task from schedconf and add to scheduler
        # crawl_task(app_config=config, sched_id=str(res.get("data").get("_id")))
        current_app.logger.info("sched conf: {0} created successfully".format(sched_name))
        response["message"] = "sched conf: {0} created successfully".format(res['data']['sched_name'] + "(" + str(res['data']['_id']) + ")")
        response["code"] = 201
    return json.dumps(response, ensure_ascii=False)


@configManager.route('/scheds', methods=['get'])
def get_all_scheds():
    cur_env = request.__dict__['environ']
    current_app.logger.info(' '.join([request.remote_addr + ':', str(cur_env['REMOTE_PORT']), cur_env['REQUEST_METHOD'], cur_env['PATH_INFO'], cur_env['SERVER_PROTOCOL']]))
    app_config = dict(current_app.config)
    sched_handler = SchedHandler(app_config=app_config)
    res_list = sched_handler.get_all_scheds()
    for item in res_list:
        now = datetime.datetime.now()
        start_date = datetime.datetime.strptime(item["start_date"], "%Y-%m-%d %H:%M:%S")
        end_date = datetime.datetime.strptime(item["end_date"], "%Y-%m-%d %H:%M:%S")
        if start_date < now < end_date:
            item['status'] = "running"
        elif now > end_date:
            item['status'] = "done"
        elif start_date > now:
            item['status'] = "ready"
    res = {'message': res_list, 'code': 200}
    return json.dumps(res, ensure_ascii=False)


@configManager.route('/sched', methods=['get'])
def get_sched():
    cur_env = request.__dict__['environ']
    current_app.logger.info(' '.join([request.remote_addr + ':', str(cur_env['REMOTE_PORT']), cur_env['REQUEST_METHOD'], cur_env['PATH_INFO'], cur_env['SERVER_PROTOCOL']]))
    sched_id = request.args['sched_id']
    app_config = dict(current_app.config)
    sched_handler = SchedHandler(app_config=app_config)
    sched = sched_handler.get_sched(sched_id=sched_id)
    if sched:
        sched['_id'] = str(sched['_id'])
        res = {'message': sched, 'code': 200}
    else:
        res = {'message': 'no sched {}'.format(sched_id), 'code': 202}
    return json.dumps(res, ensure_ascii=False)


@configManager.route('/deletesched', methods=['GET', 'DELETE'])
def delete_sched():
    sched_id = request.args['sched_id']
    app_config = dict(current_app.config)
    cur_env = request.__dict__['environ']
    current_app.logger.info(' '.join([request.remote_addr + ':', str(cur_env['REMOTE_PORT']), cur_env['REQUEST_METHOD'], cur_env['PATH_INFO'], cur_env['SERVER_PROTOCOL']]))
    sched_handler = SchedHandler(app_config=app_config)
    res = sched_handler.del_sched(sched_id=sched_id)
    if not res.get('sched_id'):
        current_app.logger.info('no sched {}'.format(sched_id))
        res = {'message': 'no sched {}'.format(sched_id), 'code': 202}
    else:
        current_app.logger.info('sched {} deleted'.format(sched_id))
        res = {'message': 'sched config {} deleted'.format(sched_id), 'code': 200}
    return json.dumps(res, ensure_ascii=False)


@configManager.route('/editsched', methods=['POST'])
def edit_sched():
    config = dict(current_app.config)
    response = {'message': None, 'code': None}
    sched_id = request.args['sched_id']
    data = request.get_json()
    sched_handler = SchedHandler(app_config=config)
    sched = sched_handler.get_sched(sched_id=sched_id)
    if data.get("conf"):
        conf_handler = ConfHandler(data={}, app_config=config)
        conf = conf_handler.get_conf(conf_id=data.get("conf"))
        if not conf:
            current_app.logger.info('no conf {}'.format(data.get("conf")))
            res = {'message': 'no conf {}'.format(data.get("conf")), 'code': 202}
            return json.dumps(res, ensure_ascii=False)
    if not sched:
        current_app.logger.info('no sched {}'.format(sched_id))
        res = {'message': 'no sched {}'.format(sched_id), 'code': 202}
        return json.dumps(res, ensure_ascii=False)
    # validate whether keys needed to be edited are allowed
    keys_canbe_edited = list(sched.keys())
    keys_canbe_edited.remove("_id")
    keys_canbe_edited.remove("create_time")
    keys_needtobe_edited = data.keys()
    for kn in keys_needtobe_edited:
        if kn not in keys_canbe_edited:
            response = {'message': "fields {} cannot be edited".format(kn), 'code': 202}
            return json.dumps(response, ensure_ascii=False)
        else:
            pass
    res = sched_handler.edit_sched(_id=sched_id, data=data)
    if res.modified_count == 1:
        response['message'] = "successfully updated"
        response['code'] = 200
    else:
        response['message'] = "successfully updated, but nothing changed"
        response['code'] = 200
    return json.dumps(response, ensure_ascii=False)


@configManager.route('/activatesched', methods=['POST', 'GET'])
def start_sched():
    cur_env = request.__dict__['environ']
    current_app.logger.info(' '.join([request.remote_addr + ':', str(cur_env['REMOTE_PORT']), cur_env['REQUEST_METHOD'], cur_env['PATH_INFO'], cur_env['SERVER_PROTOCOL']]))
    sched_id = request.args.get('sched_id', "")
    app_config = dict(current_app.config)
    required_items = dict()
    required_items['MONGODB_HOST'] = app_config['MONGODB_HOST']
    required_items['MONGODB_PORT'] = app_config['MONGODB_PORT']
    # InfoCollectorConfs
    required_items['MONGODB_CONF_DATABASE'] = app_config['MONGODB_CONF_DATABASE']
    # jobs
    required_items['JOB_CONF_COLL_NAME'] = app_config['JOB_CONF_COLL_NAME']
    # groups
    required_items['JOB_CONF_COLL_NAME'] = app_config['JOB_CONF_COLL_NAME']
    # scheds
    required_items['SCHED_CONF_COLL_NAME'] = app_config['SCHED_CONF_COLL_NAME']
    # Infos
    required_items['MONGODB_INFO_DATABASE'] = app_config['MONGODB_INFO_DATABASE']
    # articles
    required_items['ARTICLE_COLL_NAME'] = app_config['ARTICLE_COLL_NAME']
    # logs
    required_items['LOG_COLL_NAME'] = app_config['LOG_COLL_NAME']
    sched_handler = SchedHandler(app_config=app_config)
    sched = sched_handler.get_sched(sched_id=sched_id)
    if sched:
        task_id = None
        sched_id = str(sched["_id"])
        start_date = sched["start_date"]
        end_date = sched["end_date"]
        # cron fields
        # 0 15 10 ? * 0-4: execute at 10:15 am every Monday to Friday
        year = sched.get("year", "*")
        month = sched.get("month", "*")
        week = sched.get("week", "*")
        day_of_week = sched.get("day_of_week", "*")
        # hour = sched.get("hour", "*")
        # minute = sched.get("minute", "/30")
        hm = sched.get("hm", ["0:0", "1:0", "2:0", "3:0", "4:0", "5:00"])
        timezone = sched.get("timezone", "")
        jitter = sched.get("jitter", "")
        scheduler = current_app.apscheduler
        task = None
        res = dict()
        task_list = list()
        for h_m in hm:
            hour = h_m.split(":")[0]
            minute = h_m.split(":")[1]
            task_id = sched_id + "-" + datetime.datetime.now().strftime("%Y:%m:%d_") + h_m
            try:
                task = scheduler.add_job(func=crawl_task,
                                         kwargs={'app_config': required_items, 'sched_id': sched_id, 'task_id': task_id},
                                         id=task_id, trigger='cron', start_date=start_date, end_date=end_date,
                                         year=year, month=month, week=week, day_of_week=day_of_week, hour=hour, minute=minute,
                                         next_run_time=datetime.datetime.now()
                                         )
            except Exception as e:
                res = {'message': str(e), 'code': 200}
                return json.dumps(res, ensure_ascii=False)
            if task:
                task_list.append(task_id)
        if task_list:
            res = {'message': "added tasks {} into scheduer".format(" ".join(task_list)), 'code': 200}
        else:
            res = {'message': "added task {} into scheduer failed".format(task_id), 'code': 200}
    else:
        res = {'message': 'no sched {}'.format(sched_id), 'code': 202}
    return json.dumps(res, ensure_ascii=False)


@configManager.route('/taskdetails', methods=['POST', 'GET'])
def get_exec_details():
    cur_env = request.__dict__['environ']
    app_config = dict(current_app.config)
    current_app.logger.info(' '.join([request.remote_addr + ':', str(cur_env['REMOTE_PORT']), cur_env['REQUEST_METHOD'], cur_env['PATH_INFO'], cur_env['SERVER_PROTOCOL']]))
    sched_id = request.args.get("sched_id", "")
    if not sched_id:
        res = {'message': "ERROR:sched_id is null", 'code': 202}
        return json.dumps(res, ensure_ascii=False)
    else:
        host = app_config['MONGODB_HOST']
        port = app_config['MONGODB_PORT']
        info_db = app_config['MONGODB_INFO_DATABASE']
        log_col = app_config['LOG_COLL_NAME']
        mongo_operator = MongoOperator(host=host, port=port, db=info_db)
        exec_logs = mongo_operator.find(collectionname=log_col, field_name="sched_id", field_value=sched_id)
        data = list()
        for log in exec_logs:
            tmp = dict()
            tmp["start_time"] = log["start_time"]
            tmp["end_time"] = log["end_time"]
            tmp["articles_number"] = log["articles_number"]
            tmp["inserted_number"] = log["inserted_number"]
            tmp["status"] = "success" if tmp["articles_number"] > 0 else "failed"
            data.append(tmp)
        mongo_operator.close()
        res = {'message': data, 'code': 202}
        return json.dumps(res, ensure_ascii=False)


#  scheds in the scheduler
@configManager.route('/tasks', methods=['POST', 'GET'])
def get_tasks_in_sched():
    cur_env = request.__dict__['environ']
    current_app.logger.info(' '.join([request.remote_addr + ':', str(cur_env['REMOTE_PORT']), cur_env['REQUEST_METHOD'], cur_env['PATH_INFO'], cur_env['SERVER_PROTOCOL']]))
    app_config = dict(current_app.config)
    scheduler = current_app.apscheduler
    tasks = scheduler.get_jobs()
    task_ids = list()
    for task in tasks:
        task_ids.append(task.id)
    if task_ids:
        task_list = list()
        sched_handler = SchedHandler(app_config=app_config)
        scheds = sched_handler.get_all_scheds()
        for sched in scheds:
            if str(sched["_id"]) in task_ids:
                sched["status"] = "running"
                # tmp["next_run_at"] = scheduler.get_job(str(sched["_id"])).next_run_time
                task_list.append(sched)
        res = {'message': task_list, 'code': 202}
        return json.dumps(res, ensure_ascii=False)
    else:
        res = {'message': [], 'code': 202}
        return json.dumps(res, ensure_ascii=False)


# get sched results
@configManager.route('/schedres', methods=['GET'])
def get_sched_results():
    cur_env = request.__dict__['environ']
    app_config = dict(current_app.config)
    sched_id = request.args.get("sched_id", "")
    current_app.logger.info(' '.join([request.remote_addr + ':', str(cur_env['REMOTE_PORT']), cur_env['REQUEST_METHOD'], cur_env['PATH_INFO'], cur_env['SERVER_PROTOCOL']]) + "downloading text files generated by sched_id: {}".format(sched_id))
    files_dir = os.path.dirname(os.path.dirname(os.path.dirname(os.path.realpath(__file__)))) + os.path.sep + 'articles'
    article_names = list()
    host = app_config['MONGODB_HOST']
    port = app_config['MONGODB_PORT']
    info_db = app_config['MONGODB_INFO_DATABASE']
    article_col = app_config['ARTICLE_COLL_NAME']
    mongo_operator = MongoOperator(host=host, port=port, db=info_db)
    articles = mongo_operator.find(collectionname=article_col, field_name="sched_id", field_value=sched_id)
    if articles:
        out_dir = os.path.dirname(os.path.dirname(os.path.dirname(os.path.realpath(__file__)))) + os.path.sep + 'zipfiles'
        if not os.path.exists(out_dir):
            try:
                os.mkdir(out_dir)
            except Exception as err:
                current_app.logger.error(err)
        for article in articles:
            article_names.append(str(article["_id"]) + ".txt")
        zip_dir = make_zip(source_dir=files_dir, filenames=article_names, output_filename=sched_id + ".zip", out_dir=out_dir)
        response = make_response(send_from_directory(directory=out_dir, filename=sched_id + ".zip", as_attachment=True))
        response.headers["Content-Disposition"] = "attachment; filename={}".format((sched_id + ".zip").encode('utf-8').decode('utf-8'))
        return response
    else:
        res = {'message': [], 'code': 202}
        return json.dumps(res, ensure_ascii=False)


# just for test, be careful
@configManager.route('/clearusages222222', methods=['GET'])
def clear_all_usages():
    app_config = dict(current_app.config)
    conf_handler = ConfHandler(data={}, app_config=app_config)
    conf_handler.clear_all_usages()
    return json.dumps("ok", ensure_ascii=False)
