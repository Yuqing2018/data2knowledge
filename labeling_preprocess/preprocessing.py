#coding:utf-8
import collections
import hashlib
import time
import random
import re
import importlib
jieba = importlib.import_module("jieba")
jieba.initialize()
class PreProcessor(object):
    def __init__(self,labeling_task,paragraphs,tokenization_dict=None):
        # tokenizer = jieba.Tokenizer()
        #tokenizer.initialized = True
        self.labeling_task = labeling_task
        self.paragraphs = paragraphs
        self.tokenization_dict = tokenization_dict
        if self.tokenization_dict and self.tokenization_dict is not None:
            for word in sorted(self.tokenization_dict):
                jieba.add_word(word,freq=100000)
        # self.tokenizer = tokenizer

    def cut_sent(self,para):
        para = re.sub('([。！？\?])([^”’])', r"\1\n\2", para)  # 单字符断句符
        para = re.sub('(\.{6})([^”’])', r"\1\n\2", para)  # 英文省略号
        para = re.sub('(\…{2})([^”’])', r"\1\n\2", para)  # 中文省略号
        para = re.sub('([。！？\?][”’])([^，。！？\?])', r'\1\n\2', para) # 如果双引号前有终止符，那么双引号才是句子的终点，把分句符\n放到双引号后，注意前面的几句都小心保留了双引号
        para = para.rstrip()  # 段尾如果有多余的\n就去掉 # 很多规则中会考虑分号;这里忽略不计，破折号、英文双引号等同样忽略，需要的再做些简单调整即可。
        return para.split("\n")

    def tokenization(self,sentence):
        #return self.tokenizer.lcut(sentence)
        tokens = jieba.lcut(sentence)
        if self.tokenization_dict and self.tokenization_dict is not None:
            for word in sorted(self.tokenization_dict):
                jieba.del_word(word)
        return tokens

    def pre_processing(self):
        if self.labeling_task == 'tokenization':
            results = []
            for para in self.paragraphs:
                para_result = {}
                para_result['Items'] = []
                sentences = self.cut_sent(para)
                for sent in sentences:
                    sent_result = collections.OrderedDict()

                    md5 = hashlib.md5()
                    md5.update(str(time.time()).encode('utf-8'))
                    sent_id = md5.hexdigest() if md5 else str(random.random())
                    sent_result['Id'] = sent_id
                    sent_result['Text'] = sent
                    sent_result['SpanItems'] = []
                    token_list = self.tokenization(sent)
                    token_begin_index = 0
                    print("token_list:",token_list)
                    for token in token_list:
                        token_end_index = token_begin_index + len(token)
                        token_result = {}
                        token_result['Start'] = token_begin_index
                        token_result['End'] = token_end_index
                        sent_result['SpanItems'].append(token_result)
                        token_begin_index = token_end_index
                    para_result['Items'].append(sent_result)
                results.append(para_result)
            return results
        elif self.labeling_task == 'ner':
            results = []
            for para in self.paragraphs:
                para_result = {}
                para_result['Items'] = []
                sentences = self.cut_sent(para)
                for sent in sentences:
                    sent_result = collections.OrderedDict()
                    md5 = hashlib.md5()
                    md5.update(str(time.time()).encode('utf-8'))
                    sent_id = md5.hexdigest() if md5 else str(random.random())
                    sent_result['Id'] = sent_id
                    sent_result['Text'] = sent
                    sent_result['SpanItems'] = []
                    para_result['Items'].append(sent_result)
                results.append(para_result)
            return results
        elif self.labeling_task == 'text_similarity': #分类只做分句处理
            results = []
            for para in self.paragraphs:
                para_result = {}
                para_result['Items'] = []
                sentences = self.cut_sent(para)
                for sent in sentences:
                    sent_result = collections.OrderedDict()
                    md5 = hashlib.md5()
                    md5.update(str(time.time()).encode('utf-8'))
                    sent_id = md5.hexdigest() if md5 else str(random.random())
                    sent_result['Id'] = sent_id
                    sent_result['TextSource'] = sent
                    sent_result['TextTargets'] = []
                    para_result['Items'].append(sent_result)
                results.append(para_result)
            return results
        elif self.labeling_task == 'nlu':  #Dialogue Engine intent and entity labeling
            results = []
            for para in self.paragraphs:
                para_result = {}
                para_result['Items'] = []
                sentences = self.cut_sent(para)
                for sent in sentences:
                    sent_result = collections.OrderedDict()
                    md5 = hashlib.md5()
                    md5.update(str(time.time()).encode('utf-8'))
                    sent_id = md5.hexdigest() if md5 else str(random.random())
                    sent_result['Id'] = sent_id
                    sent_result['Text'] = sent
                    para_result['Items'].append(sent_result)
                results.append(para_result)
            return results
        else:
            raise Exception('Not support yet!')


if __name__ == '__main__':
    labeling_task = 'nlu'
    paragraphs = ['我们中将来了。“他怎么来了？”她说。']
    dict = ['中将']
    pre_processor = PreProcessor(labeling_task=labeling_task,paragraphs=paragraphs,tokenization_dict=dict)
    sentence = '我们中将来了。“他怎么来了？”她说。'
    tokens = pre_processor.tokenization(sentence)
    result = pre_processor.pre_processing()
    print(result)
    print(tokens)


