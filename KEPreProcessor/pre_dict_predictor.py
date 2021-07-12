#coding:utf-8
import collections
import hashlib
import requests
import multiprocessing as mp
import logging
from api_error import ApiError

from config import FLAGS
logger = logging.getLogger(__name__)


class DictPredictor(object):
    def __init__(self,dict_type='song',vocab=None,file_path=r'./data/songname.txt',process_num=4):
        self.vocab_type = dict_type
        self.file_path = file_path if file_path is not None else None

        self.process_num = process_num
        self.vocabulary = vocab if vocab is not None else self.load_vocab()
        per_voc_size = len(self.vocabulary) // self.process_num
        self.vocabulary_list = []
        for i in range(self.process_num):
            if i == self.process_num - 1:
                self.vocabulary_list.append(self.vocabulary[i * per_voc_size:])
            else:
                self.vocabulary_list.append(self.vocabulary[i * per_voc_size:(i + 1) * per_voc_size])
        self.vocab_len = len(self.vocabulary)
        logger.info("vocab_len:" + str(self.vocab_len))
    @classmethod
    def is_chinese_char(cls,cp):
        """Checks whether CP is the codepoint of a CJK character."""
        # This defines a "chinese character" as anything in the CJK Unicode block:
        #   https://en.wikipedia.org/wiki/CJK_Unified_Ideographs_(Unicode_block)
        #
        # Note that the CJK Unicode block is NOT all Japanese and Korean characters,
        # despite its name. The modern Korean Hangul alphabet is a different block,
        # as is Japanese Hiragana and Katakana. Those alphabets are used to write
        # space-separated words, so they are not treated specially and handled
        # like the all of the other languages.
        if ((cp >= 0x4E00 and cp <= 0x9FFF) or  #
                (cp >= 0x3400 and cp <= 0x4DBF) or  #
                (cp >= 0x20000 and cp <= 0x2A6DF) or  #
                (cp >= 0x2A700 and cp <= 0x2B73F) or  #
                (cp >= 0x2B740 and cp <= 0x2B81F) or  #
                (cp >= 0x2B820 and cp <= 0x2CEAF) or
                (cp >= 0xF900 and cp <= 0xFAFF) or  #
                (cp >= 0x2F800 and cp <= 0x2FA1F)):  #
            return True

        return False

    def load_vocab(self):
        vocabulary = set()
        if self.file_path is None:
            return []
        with open(self.file_path, 'r', encoding='utf-8') as f:
            for line in f:
                line = line.strip()
                index = line.find('（')
                if index != -1:
                    line = line[:index]
                if len(line) == 0 or len(line)==1:
                    continue
                foreign_flag = 0
                for char in line:
                    cp = ord(char)
                    if not self.is_chinese_char(cp):
                        foreign_flag = 1
                        break
                if foreign_flag == 1:
                    continue
                vocabulary.add(line)
        return list(vocabulary)

    @classmethod
    def get_vocab(cls):
        vocabs = {}
        person_file_path = r'./data/artists.txt'
        person_vocab = set()
        if person_file_path is None:
            return []
        with open(person_file_path, 'r', encoding='utf-8') as f:
            for line in f:
                line = line.strip()
                index = line.find('（')
                if index != -1:
                    line = line[:index]
                if len(line) == 0 or len(line) == 1:
                    continue
                foreign_flag = 0
                for char in line:
                    cp = ord(char)
                    if not cls.is_chinese_char(cp):
                        foreign_flag = 1
                        break
                if foreign_flag == 1:
                    continue
                person_vocab.add(line)

        vocabs['person'] = list(person_vocab)

        song_file_path = r'./data/songname.txt'
        song_vocab = set()
        if song_file_path is None:
            return []
        with open(song_file_path, 'r', encoding='utf-8') as f:
            for line in f:
                line = line.strip()
                index = line.find('（')
                if index != -1:
                    line = line[:index]
                if len(line) == 0 or len(line) == 1:
                    continue
                foreign_flag = 0
                for char in line:
                    cp = ord(char)
                    if not cls.is_chinese_char(cp):
                        foreign_flag = 1
                        break
                if foreign_flag == 1:
                    continue
                song_vocab.add(line)
        song_vocab = list(song_vocab)
        vocabs["song"] = song_vocab
        return vocabs

    def job(self,vocab, sentence, q):
        entity_list = []
        for word in vocab:
            begin_index = sentence.find(word)
            if begin_index != -1:
                end_index = begin_index + len(word)
                entity = collections.OrderedDict()
                md5 = hashlib.md5()
                md5.update((str(sentence)+str(word)+str(begin_index)+str(end_index)).encode(encoding='UTF-8', errors='ignore'))
                entity["Id"] = md5.hexdigest() if md5 else ''
                entity["User"] = None
                entity['Type'] = self.vocab_type
                entity['Begin'] = begin_index
                entity['End'] = end_index
                entity['Text'] = word
                entity["IsPlaceHolder"] = False
                entity["PlaceHolderId"] = None
                entity["InCoref"] = False
                entity_list.append(entity)
        q.put(entity_list)

    def predict_sentence_multiprocessing(self,sentence):
        q = mp.Queue()
        processes = []
        for i in range(self.process_num):
            p = mp.Process(target=self.job, args=(self.vocabulary_list[i], sentence, q))
            processes.append(p)
            p.start()
        for p in processes:
            p.join()

        results = []
        for _ in range(self.process_num):
            results.extend(q.get())
        return results

    def predict_sentence(self,sentence):
        entity_list = []
        for word in self.vocabulary:
            begin_index = sentence.find(word)
            if begin_index != -1:
                end_index = begin_index + len(word)
                logger.info("sentence:" + str(sentence))
                logger.info("word:" + str(word))
                logger.info("begin_index:" + str(begin_index))
                entity = collections.OrderedDict()
                md5 = hashlib.md5()
                md5.update((str(sentence)+str(word)+str(begin_index)+str(end_index)).encode(encoding='UTF-8', errors='ignore'))
                entity["Id"] = md5.hexdigest() if md5 else ''
                entity["User"] = None
                entity['Type'] = self.vocab_type
                entity['Start'] = begin_index
                entity['End'] = end_index
                entity['Text'] = word
                entity["IsPlaceHolder"] = False
                entity["PlaceHolderId"] = None
                entity["InCoref"] = False
                entity_list.append(entity)
        return entity_list

class MultiDictPredictor():
    def __init__(self):
        self.dict_predictor_list = self.get_predictor_list()
        self.vocab_len = 0
        for predictor in self.dict_predictor_list:
            self.vocab_len += predictor.vocab_len
        logger.info("Category number of MultiDictPredictor:" + str(len(self.dict_predictor_list)))
        logger.info("Size of MultiDictPredictor:" + str(self.vocab_len))
    def get_predictor_list(self):
        # resp = requests.get(FLAGS.vocab_url,timeout=(3,30))
        # if (resp.status_code != requests.codes.ok):
        #     raise ApiError('Failed to get vocabularies', 500)
        # resp = resp.json()
        resp = {}
        resp['person'] = ["周杰伦","王菲"]
        resp['song'] = ["传奇","七里香"]
        dict_predictor_list = []
        for k,v in resp.items():
            dict_predictor_list.append(DictPredictor(dict_type=k,vocab=v))
        return dict_predictor_list

    def predict_sentence(self,sentence):
        entity_list = []
        for predictor in self.dict_predictor_list:
            entity_list.extend(predictor.predict_sentence(sentence))
        return entity_list

    def predict_sentence_multiprocessing(self,sentence):
        entity_list = []
        for predictor in self.dict_predictor_list:
            entity_list.extend(predictor.predict_sentence_multiprocessing(sentence))

        return entity_list

    def predict_paragraph_dict(self,json_obj):
        sentences = json_obj["SentenceList"]["Sentences"]
        for sentence in sentences:
            orig_text = sentence["Text"]
            entity_list = self.predict_sentence_multiprocessing(
                orig_text) if self.vocab_len > FLAGS.vocab_thread else self.predict_sentence(orig_text)
            if 'Entities' not in sentence:
                sentence['Entities'] = {}
            sentence["Entities"]["ByDict"] = entity_list

    def predict_paragraph(self,json_objs):
        if type(json_objs)==list:
            for json_obj in json_objs:
                self.predict_paragraph_dict(json_obj)
        elif type(json_objs)==dict:
            self.predict_paragraph_dict(json_objs)

        return json_objs