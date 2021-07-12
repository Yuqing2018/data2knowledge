# -*- coding:utf-8 -*-
from pymongo import MongoClient


class MongoOperator(object):
    def __init__(self, host=None, port=None, db=None):
        self.host = host
        self.port = port
        self.db = db
        self.client = self.__create_client()
        self.DB = self.client[self.db]

    def __create_client(self):
        client = MongoClient(self.host, self.port)
        return client

    def upinsert_one(self, collectionname=None, data=None, field_name=None):
        res = dict()
        collection = self.client[self.db][collectionname]
        if collection.find_one({field_name: data[field_name]}):
            res['existing'] = True
            res['data'] = collection.find_one({field_name: data[field_name]})
        else:
            collection.insert_one(data)
            res['existing'] = False
            res['data'] = collection.find_one({field_name: data[field_name]})
        return res

    def insert_one(self, collectionname=None, data=None):
        res = dict()
        collection = self.client[self.db][collectionname]
        res = collection.insert_one(data)
        return res

    def get_one(self, collectionname=None, field_name=None, field_value=None):
        res = dict()
        collection = self.client[self.db][collectionname]
        res = collection.find_one({field_name: field_value})
        return res

    def find(self, collectionname=None, field_name=None, field_value=None):
        res = dict()
        collection = self.client[self.db][collectionname]
        res = collection.find({field_name: field_value})
        return res

    def get_all_data(self, collectionname=None):
        res = dict()
        collection = self.client[self.db][collectionname]
        res = collection.find()
        return res

    def edit_one(self, collectionname=None, data=None, field_name=None):
        res = dict()
        collection = self.client[self.db][collectionname]
        res = collection.update_one({field_name: data[field_name]}, {'$set': data})
        return res

    def delete_one(self, collectionname=None, field_name=None, field_value=None):
        collection = self.client[self.db][collectionname]
        res = collection.delete_one({field_name: field_value})
        return res

    def remove_duplicates(self, collectionname=None):
        collection = self.client[self.db][collectionname]
        group1 = {'$group': {'_id': {'title': '$title', 'jobname': '$job_name'},
                             'count': {'$sum': 1},
                             'dups': {'$addToSet': '$_id'}}}
        group2 = {'$group': {'_id': {'img': '$img', 'title': '$title'},
                             'count': {'$sum': 1},
                             'dups': {'$addToSet': '$_id'}}}
        group3 = {'$group': {'_id': {'img': '$img', 'title': '$title', 'link': '$link'},
                             'count': {'$sum': 1},
                             'dups': {'$addToSet': '$_id'}}}
        match = {'$match': {'count': {"$gt": 1}}}
        sort = {'$sort': {'_id': 1}}
        dup_data = collection.aggregate([sort, group1, match])
        dup_data = list(dup_data)
        for d in dup_data:
            for k, v in d.items():
                if k == 'dups' and len(v) >= 2:
                    for i in range(len(v) - 1):
                        collection.remove({'_id': v[i]})
                        # print('{} removed'.format(v[i]))

    def set_status_old(self, collectionname=None):
        collection = self.client[self.db][collectionname]
        for i in collection.find():
            if i['status'] == 'new':
                _id = i['_id']
                collection.update_one({'_id': _id}, {'$set': {'status': 'old'}})

    def bulkinsert(self, collectionname=None, data=None, orderd=True, bypass_document_validation=False, session=None):
        collection = self.client[self.db][collectionname]
        im_result = collection.insert_many(data, ordered=orderd, bypass_document_validation=bypass_document_validation, session=session)
        return im_result

    def rm_all(self, collectionname=None):
        collection = self.client[self.db][collectionname]
        res = collection.remove()
        return res

    def replace(self, collectionname=None, data=None, field_name=None):
        collection = self.client[self.db][collectionname]
        if collection.find_one({field_name: data[field_name]}):
            collection.remove({field_name: data[field_name]})
            collection.insert_one(data)
        else:
            collection.insert_one(data)

    def get_sorted_n(self, collectionname=None, field_name=None, field_value=None, sort_field=None, sort=-1, n=1):
        collection = self.client[self.db][collectionname]
        res = collection.find({field_name: field_value}).sort(sort_field, sort).skip(0).limit(n)
        return list(res)

    def close(self):
        self.client.close()
