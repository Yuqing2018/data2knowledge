#coding:utf-8
from pre_model_predictor import NerPredictor
from pre_processor import PreProcessor
from config import FLAGS

class EnsembleProcessor(object):
    def __init__(self):
        self.model_predictor = NerPredictor()
        self.pre_processor = PreProcessor()
        # self.dict_predictor = MultiDictPredictor()
        # self.regex_predictor = MultiRegexPredictor()

    def merge_dict(self,json_obj):
        sentences = json_obj['SentenceList']['Sentences']
        for sentence in sentences:
            if 'Entities' in sentence:
                pre_set = set()
                merged_entities = []
                orig_entities = []
                by_dict = sentence['Entities']['ByDict'] if 'ByDict' in sentence['Entities'] else []
                by_regex = sentence['Entities']['ByRegex'] if 'ByRegex' in sentence['Entities'] else []
                by_model = sentence['Entities']['ByModel'] if 'ByModel' in sentence['Entities'] else []
                orig_entities.extend(by_dict)
                orig_entities.extend(by_regex)
                orig_entities.extend(by_model)
                for entity in orig_entities:
                    start = int(entity['Start'])
                    end = int(entity['End'])
                    if FLAGS.allow_cross:
                        flag = self.not_in_allow_cross(start,end,pre_set)
                    else:
                        flag = self.not_in_pre_set(start,end,pre_set)
                    if flag:
                        merged_entities.append(entity)
                        pre_set.add((start,end))
                sentence["Entities"] = merged_entities

    def not_in_allow_cross(self,start,end,pre_set):
        if len(pre_set)==0:
            return True
        if (start,end) not in pre_set:
            return True
        return False

    def not_in_pre_set(self,start,end,pre_set):
        if len(pre_set)==0:
            return True
        for pair in pre_set:
            pair_start = pair[0]
            pair_end = pair[1]
            if (end<=pair_start or start>=pair_end) and (start,end) not in pre_set:
                return True
        return False

    def merge(self,json_objs):
        if type(json_objs)==list:
            for json_obj in json_objs:
                self.merge_dict(json_obj)
        elif type(json_objs)==dict:
            self.merge_dict(json_objs)
        return json_objs


Processor = EnsembleProcessor()






































