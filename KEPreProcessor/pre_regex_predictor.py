#coding:utf-8
import collections
import requests
import hashlib
import re
from config import FLAGS
from api_error import ApiError

class MultiRegexPredictor(object):
    def __init__(self):
        self.regexp_dict = self.get_regexp()

    def get_regexp(self):
        # resp = requests.get(FLAGS.regex_url, timeout=(3, 30))
        # if (resp.status_code != requests.codes.ok):
        #     raise ApiError('Failed to get vocabularies', 500)
        # resp = resp.json()
        resp = {}
        date_regex = [r'\d{4}-\d{2}-\d{2}']
        resp['date'] = date_regex
        regexp_dict = collections.OrderedDict()
        for entity_type,regexp_list in resp.items():
            pattern_list = []
            for regexp in regexp_list:
                pattern = re.compile(regexp,re.I)
                pattern_list.append(pattern)
            regexp_dict[entity_type] = pattern_list
        return regexp_dict

    def predict_sentence(self,sentence):
        entity_list = []
        for entity_type,regexp_list in self.regexp_dict.items():
            for pattern in regexp_list:
                match_obj=pattern.search(sentence)
                if match_obj:
                    start_index = match_obj.span()[0]
                    end_index = match_obj.span()[1]
                    matched_text = match_obj.group()
                    entity = collections.OrderedDict()
                    md5 = hashlib.md5()
                    md5.update((str(sentence) + str(matched_text)+str(start_index)+str(end_index)).encode(encoding='UTF-8', errors='ignore'))
                    entity["Id"] = md5.hexdigest() if md5 else ''
                    entity["User"] = None
                    entity['Type'] = entity_type
                    entity['Start'] = start_index
                    entity['End'] = end_index
                    entity['Text'] = matched_text
                    entity["IsPlaceHolder"] = False
                    entity["PlaceHolderId"] = None
                    entity["InCoref"] = False
                    entity_list.append(entity)
        return entity_list

    def predict_paragraph_dict(self,json_obj):
        sentences = json_obj["SentenceList"]["Sentences"]
        for sentence in sentences:
            orig_text = sentence["Text"]
            entity_list = self.predict_sentence(orig_text)
            if 'Entities' not in sentence:
                sentence['Entities'] = {}
            sentence["Entities"]["ByRegex"] = entity_list


    def predict_paragraph(self,json_objs):
        if type(json_objs)==list:
            for json_obj in json_objs:
                self.predict_paragraph_dict(json_obj)
        elif type(json_objs)==dict:
            self.predict_paragraph_dict(json_objs)
        return json_objs
















