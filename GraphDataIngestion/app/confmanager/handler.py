# -*- coding:utf-8 -*-
import datetime
import pytz
from collections import OrderedDict
from bson.objectid import ObjectId
from app.utils.mongo_operator import MongoOperator


def ensure_now_rfc822():
    time_zone = pytz.timezone('Asia/Shanghai')
    # time_zone = tz.tzlocal()
    dt = datetime.datetime.now(tz=time_zone)
    # dt = dt.strftime('%a, %d %b %Y %H:%M:%S %z')
    dt = dt.strftime('%a, %d %b %Y %H:%M:%S')
    return dt


class ConfHandler(object):
    def __init__(self, data=None, app_config=None):
        self.data = data
        self.conf_name = data.get("conf_name")
        self.items = data.get("items")
        # mongo
        self.mongo_host = app_config['MONGODB_HOST']
        self.mongo_port = app_config['MONGODB_PORT']
        self.conf_db = app_config['MONGODB_CONF_DATABASE']
        self.conf_collection = app_config['JOB_CONF_COLL_NAME']
        self.mongo_operator = MongoOperator(host=self.mongo_host, port=self.mongo_port, db=self.conf_db)

    def save_conf(self, field_name=None):
        self.data["create_time"] = ensure_now_rfc822()
        self.data["usages"] = []
        res = self.mongo_operator.upinsert_one(collectionname=self.conf_collection, data=self.data, field_name=field_name)
        self.mongo_operator.close()
        return res

    def get_all_confs(self):
        res = self.mongo_operator.get_all_data(collectionname=self.conf_collection)
        all_jobs = list()
        if res:
            for entry in res:
                entry['_id'] = str(entry['_id'])
                all_jobs.append(entry)
        self.mongo_operator.close()
        return all_jobs

    def get_conf(self, conf_id=None):
        res = self.mongo_operator.get_one(collectionname=self.conf_collection, field_name='_id', field_value=ObjectId(conf_id))
        if res:
            res['_id'] = str(res['_id'])
        self.mongo_operator.close()
        return res

    def del_conf(self, conf_id=None):
        result = dict()
        conf = self.mongo_operator.get_one(collectionname=self.conf_collection, field_name='_id', field_value=ObjectId(conf_id))
        if not conf:
            result['conf'] = None
            return result
        # if the job is not used
        elif not conf['usages']:
            result['usages'] = conf['usages']
            result['conf'] = conf
            res = self.mongo_operator.delete_one(collectionname=self.conf_collection, field_name='_id', field_value=ObjectId(conf_id))
            result['res'] = res
        else:
            result['usages'] = conf['usages']
            result['conf'] = conf
        self.mongo_operator.close()
        return result

    def edit_conf(self, conf_id=None, data=None):
        _id = ObjectId(conf_id)
        res = None
        conf = self.mongo_operator.get_one(collectionname=self.conf_collection, field_name='_id', field_value=_id)
        if conf:
            data['_id'] = _id
            res = self.mongo_operator.edit_one(collectionname=self.conf_collection, data=data, field_name='_id')
        self.mongo_operator.close()
        return res

    def clear_all_usages(self):
        res = self.mongo_operator.get_all_data(collectionname=self.conf_collection)
        for entry in res:
            data = {'_id': entry['_id'], 'usages': []}
            self.mongo_operator.edit_one(collectionname=self.conf_collection, field_name='_id', data=data)
        self.mongo_operator.close()


class SchedHandler(object):
    def __init__(self, sched_name=None, conf=None, start_date=None, end_date=None, app_config=None, **kwargs):
        # required
        self.sched_name = sched_name
        self.conf = conf
        # cron fields
        self.year = kwargs.get("year")
        self.month = kwargs.get("month")
        self.week = kwargs.get("week")
        self.day = kwargs.get("day")
        self.day_of_week = kwargs.get("day_of_week")
        # self.hour = kwargs.get("hour")
        # self.minute = kwargs.get("minute")
        self.hm = kwargs.get("hm")
        self.second = kwargs.get("second")
        self.timezone = kwargs.get("timezone")
        self.jitter = kwargs.get("jitter")
        # time span
        self.start_date = start_date
        self.end_date = end_date
        # mongo conf
        self.mongo_host = app_config['MONGODB_HOST']
        self.mongo_port = app_config['MONGODB_PORT']
        self.conf_db = app_config['MONGODB_CONF_DATABASE']
        self.conf_collection = app_config['JOB_CONF_COLL_NAME']
        self.sched_collection = app_config['SCHED_CONF_COLL_NAME']
        self.mongo_operator = MongoOperator(host=self.mongo_host, port=self.mongo_port, db=self.conf_db)

    def save_sched(self, field_name=None):
        # gen sched data
        sched_data = OrderedDict()
        sched_data['sched_name'] = self.sched_name
        sched_data['conf'] = self.conf
        sched_data['start_date'] = self.start_date
        sched_data['end_date'] = self.end_date
        sched_data['year'] = self.year
        sched_data['month'] = self.month
        sched_data['week'] = self.week
        sched_data['day'] = self.day
        sched_data['day_of_week'] = self.day_of_week
        # sched_data['hour'] = self.hour
        # sched_data['minute'] = self.minute
        sched_data['second'] = self.second
        sched_data['hm'] = self.hm
        sched_data['timezone'] = self.timezone
        sched_data['jitter'] = self.jitter
        sched_data['create_time'] = ensure_now_rfc822()
        # validate job list
        conf_id_required = self.conf
        conf_ids_existed = [str(conf['_id']) for conf in self.mongo_operator.get_all_data(collectionname=self.conf_collection)]
        res = dict()
        # if some jobs do not exist
        if conf_id_required not in conf_ids_existed:
            res['status'] = "False"
            res['data'] = conf_id_required
        else:
            # insert sched conf
            result = self.mongo_operator.upinsert_one(collectionname=self.sched_collection, data=sched_data, field_name=field_name)
            if not result['existing']:
                res = result
                # update job usages
                conf = self.mongo_operator.get_one(collectionname=self.conf_collection, field_name='_id', field_value=ObjectId(conf_id_required))
                if conf:
                    tmp_list = conf['usages']
                    tmp_list.append(str(result["data"]["_id"]))
                    data = {'_id': conf['_id'], 'usages': tmp_list}
                    update_result = self.mongo_operator.edit_one(collectionname=self.conf_collection, field_name='_id', data=data)
            elif result['existing']:
                res = result
        self.mongo_operator.close()
        return res

    def get_all_scheds(self):
        res = self.mongo_operator.get_all_data(collectionname=self.sched_collection)
        all_groups = list()
        if res:
            for entry in res:
                entry['_id'] = str(entry['_id'])
                all_groups.append(entry)
        self.mongo_operator.close()
        return all_groups

    def get_sched(self, sched_id=None):
        res = self.mongo_operator.get_one(collectionname=self.sched_collection, field_name='_id', field_value=ObjectId(sched_id))
        self.mongo_operator.close()
        return res

    def del_sched(self, sched_id=None):
        result = dict()
        sched = self.mongo_operator.get_one(collectionname=self.sched_collection, field_name='_id', field_value=ObjectId(sched_id))
        if not sched:
            result['sched_id'] = None
            return result
        else:
            # update job usages before deleting, like transaction
            conf = sched['conf']
            entry = self.mongo_operator.get_one(collectionname=self.conf_collection, field_name='_id', field_value=ObjectId(conf))
            if entry:
                tmp_list = entry['usages']
                if sched_id in tmp_list:
                    tmp_list.remove(sched_id)
                data = {'_id': ObjectId(conf), 'usages': tmp_list}
                self.mongo_operator.edit_one(collectionname=self.conf_collection, field_name='_id', data=data)
            # delete sched
            res = self.mongo_operator.delete_one(collectionname=self.sched_collection, field_name='_id', field_value=ObjectId(sched_id))
            result['res'] = res
            result['sched_id'] = sched_id
        self.mongo_operator.close()
        return result

    def edit_sched(self, _id=None, data=None):
        o_id = ObjectId(_id)
        res = None
        result = dict()
        tmp_data = dict()
        conf = data.get("conf", "")
        sched_old = self.mongo_operator.get_one(collectionname=self.sched_collection, field_name='_id', field_value=ObjectId(_id))
        if not sched_old:
            result['sched_id'] = None
            return result
        if sched_old:
            if conf:
                if sched_old["conf"] == conf:
                    # do not need to update job confs
                    data['_id'] = o_id
                    res = self.mongo_operator.edit_one(collectionname=self.sched_collection, data=data, field_name='_id')
                else:
                    # update job usages before editing, like transaction
                    # 1st delete the sched from all the usages of jobs uesed by the sched
                    entry_old = self.mongo_operator.get_one(collectionname=self.conf_collection, field_name='_id', field_value=ObjectId(sched_old['conf']))
                    if entry_old:
                        tmp_list = entry_old['usages']
                        if _id in tmp_list:
                            tmp_list.remove(_id)
                        tmp_data = {'_id': ObjectId(sched_old['conf']), 'usages': tmp_list}
                        self.mongo_operator.edit_one(collectionname=self.conf_collection, field_name='_id', data=tmp_data)
                    # 2nd
                    entry_new = self.mongo_operator.get_one(collectionname=self.conf_collection, field_name='_id', field_value=ObjectId(data['conf']))
                    if entry_new:
                        print(entry_new)
                        tmp_list = entry_old['usages']
                        tmp_list.append(_id)
                        tmp_data = {'_id': ObjectId(data['conf']), 'usages': tmp_list}
                        self.mongo_operator.edit_one(collectionname=self.conf_collection, field_name='_id', data=tmp_data)

                    # update
                    data['_id'] = o_id
                    res = self.mongo_operator.edit_one(collectionname=self.sched_collection, data=data, field_name='_id')
            elif not conf:
                # if job_list is [], do not need to update job confs
                data['_id'] = o_id
                res = self.mongo_operator.edit_one(collectionname=self.sched_collection, data=data, field_name='_id')
        self.mongo_operator.close()
        return res

