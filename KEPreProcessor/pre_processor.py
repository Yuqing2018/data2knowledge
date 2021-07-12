#coding:utf-8
import collections
import jieba
import hashlib

user_dict_path = r'./data/artists.txt'
jieba.load_userdict(user_dict_path)

class PreProcessor(object):
    def __init__(self):
        self.cut_list = ['。','？','！']

    def find_token(self,char):
        if char in self.cut_list:
            return True
        else:
            return False

    def cut_paragraph(self,paragraph):
        total_count = 0
        sentences = []
        one_sentence = []
        for word in paragraph:
            one_sentence.append(word)
            if self.find_token(word):
                total_count += 1
                sentences.append("".join(one_sentence))
                one_sentence = []
        return (total_count,sentences)

    def preprocess(self,paragraph):
        paragraph = paragraph.strip().replace(' ','').replace('\r','').replace('\n','')
        json_objects = collections.OrderedDict()
        json_objects["Text"] = paragraph
        sentence_list = collections.OrderedDict()
        sentences = []
        (total_count,split_sentences) = self.cut_paragraph(paragraph)
        sentence_begin_index = 0
        for sentence in split_sentences:
            sentence_md5 = hashlib.md5()
            tokens = []
            token_list = self.tokenization(sentence)
            token_begin_index = 0
            for token in token_list:
                md5 = hashlib.md5()
                token_dict = collections.OrderedDict()
                token_end_index = token_begin_index+len(token)
                md5.update((str(token)+str(sentence)+str(token_begin_index)+str(token_end_index)).encode(encoding='UTF-8',errors='ignore'))
                token_dict["Id"] = md5.hexdigest() if md5 else ''
                token_dict["Text"] = token
                token_dict["Start"] = token_begin_index
                token_dict["End"] = token_end_index
                token_begin_index = token_end_index
                tokens.append(token_dict)

            sentence_end_index = sentence_begin_index + len(list(sentence))
            sentence_dict = collections.OrderedDict()
            sentence_md5.update(sentence.encode(encoding='UTF-8',errors='ignore'))
            sentence_dict["Id"] = sentence_md5.hexdigest() if sentence_md5 else ''
            sentence_dict["Text"] = sentence
            sentence_dict["Start"] = sentence_begin_index
            sentence_dict["End"] = sentence_end_index
            sentence_begin_index = sentence_end_index
            sentence_dict["Tokens"] = tokens
            sentences.append(sentence_dict)

        sentence_list["TotalCount"] = total_count
        sentence_list["Sentences"] = sentences
        json_objects["SentenceList"] = sentence_list

        return json_objects

    def tokenization(self,sentence):
        return jieba.lcut(sentence)