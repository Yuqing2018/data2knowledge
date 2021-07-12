#coding:utf-8
import numpy as np
import collections
from config import FLAGS


def kappa(matrix, k,theoretical_agreement_is_zero=True):  # matrix表示要计算的数据，k表示数据矩阵的是k*k的
    P0 = 0.0
    for i in range(k):
        P0 += matrix[i, i] * 1.0
    xsum = np.sum(matrix, axis=1)
    ysum = np.sum(matrix, axis=0)
    total = np.sum(matrix)
    # xsum是个k行1列的向量，ysum是个1行k列的向量
    P0 = float(P0 / total * 1.0)
    if theoretical_agreement_is_zero:
        if P0 == 0:
            return 0.0
        else:
            return round(P0, 2)
    Pe = float(ysum * xsum) / total ** 2
    if Pe >= 1:
        return 1.0
    cohens_coefficient = float((P0 - Pe) / (1 - Pe))
    if cohens_coefficient == 0:
        return 0.0
    else:
        return round(cohens_coefficient, 2)


def fleiss_kappa(matrix, N, k, n,theoretical_agreement_is_zero=True):  # matrix表示要计算的数据，（N,k）表示矩阵的形状，说明数据是N行k列的，一共有n个标注人员
    annotation_result_sum = 0.0
    P0 = 0.0
    for i in range(N):
        temp = 0.0
        for j in range(k):
            annotation_result_sum += matrix[i, j]
            temp += 1.0 * matrix[i, j] ** 2
        temp -= n
        temp /= (n - 1) * n
        P0 += temp
    P0 = 1.0 * P0 / N
    if theoretical_agreement_is_zero:
        if P0 == 0:
            return 0.0
        else:
            return round(P0, 2)
    ysum = np.sum(matrix, axis=0)
    Pe = np.sum(np.square(ysum)) / (n * (n - 1))
    if Pe >= 1:
        return 1.0
    ans = (P0 - Pe) / (1 - Pe)
    if ans == 0:
        return 0.0
    else:
        return round(ans, 2)

def calculate_score(entity_type_stat,annotator_num):
    results = collections.OrderedDict()
    if annotator_num == 2:
        for type,stat in entity_type_stat.items():
            same_num = stat['same_num']
            if FLAGS.repeat_identical_annotation:
                same_num = same_num*2
                total_entity_num = stat['first_num'] + stat['second_num']
                matrix = np.mat([[same_num,stat['first_num']-same_num/2],[stat['second_num']-same_num/2,0]])
            else:
                total_entity_num = stat['first_num'] + stat['second_num'] - same_num
                matrix = np.mat([[same_num, stat['first_num'] - same_num],
                                 [stat['second_num'] - same_num, 0]])
            if total_entity_num < FLAGS.entity_num_thread:
                theoretical_agreement_is_zero = True
            else:
                theoretical_agreement_is_zero = False
            results[type] = kappa(matrix, 2, theoretical_agreement_is_zero)
    elif annotator_num>2:
        for type,stat in entity_type_stat.items():
            matrix = []
            for num in stat.values():
                matrix.append([num, annotator_num - num])
            matrix = np.mat(matrix)
            task_num = matrix.shape[0]
            annotate_result_category = matrix.shape[1] # yes or no
            if task_num<FLAGS.entity_num_thread:
                theoretical_agreement_is_zero = True
            else:
                theoretical_agreement_is_zero = False
            results[type] = fleiss_kappa(matrix,task_num,annotate_result_category,annotator_num,theoretical_agreement_is_zero)
        total_score = 0.0
        entity_type_num = 0
        for score in results.values():
            total_score += score
            entity_type_num += 1
        results['all'] = round(total_score / entity_type_num,2)
    return results

class KEIAAOneDoc(object):
    def __init__(self,annotate_results):
        self.annotate_results = annotate_results
        self.annotator_num = len(annotate_results)
    def statistics_entity(self,annotate_results):
        if len(annotate_results)==2:
            first_annotator_sentences = annotate_results[0]['SentenceList']['Sentences']
            second_annotator_sentences = annotate_results[1]['SentenceList']['Sentences']
            assert len(first_annotator_sentences)==len(second_annotator_sentences)
            total_same_num = 0
            total_first_num = 0
            total_second_num = 0
            entity_type_stat = collections.OrderedDict()
            entity_set = set()
            for i in range(len(first_annotator_sentences)): #i表示第几句话,要求sentence有序。
                entities_1 = first_annotator_sentences[i]['Entities']
                entities_2 = second_annotator_sentences[i]['Entities']
                for entity in entities_1:
                    start = str(entity['Start'])
                    end = str(entity['End'])
                    type = str(entity['Type'])
                    is_placeholder = '1' if entity['IsPlaceHolder'] else '0'
                    entity_name = '-'.join([str(i),start,end,type,is_placeholder])
                    total_first_num += 1
                    if type not in entity_type_stat:
                        entity_type_stat[type] = collections.OrderedDict()
                        entity_type_stat[type]['same_num'] = 0
                        entity_type_stat[type]['first_num'] = 1
                        entity_type_stat[type]['second_num'] = 0
                    else:
                        entity_type_stat[type]['first_num'] += 1
                    if entity_name not in entity_set:
                        entity_set.add(entity_name)
                    else:
                        entity_type_stat[type]['same_num'] += 1
                        total_same_num += 1
                for entity in entities_2:
                    start = str(entity['Start'])
                    end = str(entity['End'])
                    type = str(entity['Type'])
                    is_placeholder = '1' if entity['IsPlaceHolder'] else '0'
                    entity_name = '-'.join([str(i), start, end, type, is_placeholder])
                    total_second_num += 1
                    if type not in entity_type_stat:
                        entity_type_stat[type] = collections.OrderedDict()
                        entity_type_stat[type]['same_num'] = 0
                        entity_type_stat[type]['first_num'] = 0
                        entity_type_stat[type]['second_num'] = 1
                    else:
                        entity_type_stat[type]['second_num'] += 1
                    if entity_name not in entity_set:
                        entity_set.add(entity_name)
                    else:
                        entity_type_stat[type]['same_num'] += 1
                        total_same_num += 1

            entity_type_stat['all'] = collections.OrderedDict()
            entity_type_stat['all']['first_num'] = total_first_num
            entity_type_stat['all']['second_num'] = total_second_num
            entity_type_stat['all']['same_num'] = total_same_num

            return entity_type_stat

        elif len(annotate_results)>=3:
            entity_type_stat = collections.OrderedDict()
            for annotate_result in annotate_results:
                sentences = annotate_result['SentenceList']['Sentences']
                for i,sentence in enumerate(sentences):
                    entities = sentence['Entities']
                    for entity in entities:
                        start = str(entity['Start'])
                        end = str(entity['End'])
                        type = str(entity['Type'])
                        is_placeholder = '1' if entity['IsPlaceHolder'] else '0'
                        entity_name = '-'.join([str(i), start, end, type, is_placeholder])
                        if type not in entity_type_stat:
                            entity_type_stat[type] = collections.OrderedDict()
                            entity_type_stat[type][entity_name] = 1
                        else:
                            if entity_name not in entity_type_stat[type]:
                                entity_type_stat[type][entity_name] = 1
                            else:
                                entity_type_stat[type][entity_name] += 1
            return entity_type_stat

    def statistics_relation(self,annotate_results):
        if len(annotate_results)==2:
            first_annotator_sentences = annotate_results[0]['SentenceList']['Sentences']
            second_annotator_sentences = annotate_results[1]['SentenceList']['Sentences']
            assert len(first_annotator_sentences)==len(second_annotator_sentences)
            total_same_num = 0
            total_first_num = 0
            total_second_num = 0
            relation_type_stat = collections.OrderedDict()
            relation_set = set()
            for i in range(len(first_annotator_sentences)): #i表示第几句话,要求sentence有序。
                relations_1 = first_annotator_sentences[i]['Relations']
                relations_2 = second_annotator_sentences[i]['Relations']
                for relation in relations_1:
                    start = str(relation['Args'][0])
                    end = str(relation['Args'][1])
                    type = str(relation['Type'])
                    relation_name = '-'.join([str(i),start,end,type])
                    total_first_num += 1
                    if type not in relation_type_stat:
                        relation_type_stat[type] = collections.OrderedDict()
                        relation_type_stat[type]['same_num'] = 0
                        relation_type_stat[type]['first_num'] = 1
                        relation_type_stat[type]['second_num'] = 0
                    else:
                        relation_type_stat[type]['first_num'] += 1
                    if relation_name not in relation_set:
                        relation_set.add(relation_name)
                    else:
                        relation_type_stat[type]['same_num'] += 1
                        total_same_num += 1
                for relation in relations_1:
                    start = str(relation['Args'][0])
                    end = str(relation['Args'][1])
                    type = str(relation['Type'])
                    relation_name = '-'.join([str(i),start,end,type])
                    total_second_num += 1
                    if type not in relation_type_stat:
                        relation_type_stat[type] = collections.OrderedDict()
                        relation_type_stat[type]['same_num'] = 0
                        relation_type_stat[type]['first_num'] = 0
                        relation_type_stat[type]['second_num'] = 1
                    else:
                        relation_type_stat[type]['second_num'] += 1
                    if relation_name not in relation_set:
                        relation_set.add(relation_name)
                    else:
                        relation_type_stat[type]['same_num'] += 1
                        total_same_num += 1

            relation_type_stat['all'] = collections.OrderedDict()
            relation_type_stat['all']['first_num'] = total_first_num
            relation_type_stat['all']['second_num'] = total_second_num
            relation_type_stat['all']['same_num'] = total_same_num

            return relation_type_stat

        elif len(annotate_results)>=3:
            relation_type_stat = collections.OrderedDict()
            for annotate_result in annotate_results:
                sentences = annotate_result['SentenceList']['Sentences']
                for i,sentence in enumerate(sentences):
                    relations = sentence['Relations']
                    for relation in relations:
                        start = str(relation['Args'][0])
                        end = str(relation['Args'][1])
                        type = str(relation['Type'])
                        relation_name = '-'.join([str(i), start, end, type])
                        if type not in relation_type_stat:
                            relation_type_stat[type] = collections.OrderedDict()
                            relation_type_stat[type][relation_name] = 1
                        else:
                            if relation_name not in relation_type_stat[type]:
                                relation_type_stat[type][relation_name] = 1
                            else:
                                relation_type_stat[type][relation_name] += 1
            return relation_type_stat

    def statistics_coref(self,annotate_results):
        if len(annotate_results)==2:
            first_annotator_corefs = annotate_results[0]['CorefChains']
            second_annotator_corefs = annotate_results[1]['CorefChains']
            total_same_num = 0
            total_first_num = 0
            total_second_num = 0
            ref_type_stat = collections.OrderedDict()
            ref_set = set()

            for ref in first_annotator_corefs:
                ref_str = ''
                type = ref['Type']
                ref_str = ref_str + str(type)
                for entity in sorted(ref['Entities']):
                    ref_str = ref_str + str(entity)
                total_first_num += 1
                if type not in ref_type_stat:
                    ref_type_stat[type] = collections.OrderedDict()
                    ref_type_stat[type]['same_num'] = 0
                    ref_type_stat[type]['first_num'] = 1
                    ref_type_stat[type]['second_num'] = 0
                else:
                    ref_type_stat[type]['first_num'] += 1
                if ref_str not in ref_set:
                    ref_set.add(ref_str)
                else:
                    ref_type_stat[type]['same_num'] += 1
                    total_same_num += 1
            for ref in second_annotator_corefs:
                ref_str = ''
                type = ref['Type']
                ref_str = ref_str + str(type)
                for entity in sorted(ref['Entities']):
                    ref_str = ref_str + str(entity)
                total_first_num += 1
                if type not in ref_type_stat:
                    ref_type_stat[type] = collections.OrderedDict()
                    ref_type_stat[type]['same_num'] = 0
                    ref_type_stat[type]['first_num'] = 1
                    ref_type_stat[type]['second_num'] = 0
                else:
                    ref_type_stat[type]['first_num'] += 1
                if ref_str not in ref_set:
                    ref_set.add(ref_str)
                else:
                    ref_type_stat[type]['same_num'] += 1
                    total_same_num += 1

            ref_type_stat['all'] = collections.OrderedDict()
            ref_type_stat['all']['first_num'] = total_first_num
            ref_type_stat['all']['second_num'] = total_second_num
            ref_type_stat['all']['same_num'] = total_same_num

            return ref_type_stat

        elif len(annotate_results)>=3:
            ref_type_stat = collections.OrderedDict()
            for annotate_result in annotate_results:
                annotator_corefs = annotate_result['CorefChains']
                for ref in annotator_corefs:
                    type = ref['Type']
                    ref_set_str = ''
                    ref_set_str = ref_set_str+str(type)
                    for entity in ref['Entities']:
                        ref_set_str = ref_set_str + str(entity)
                    if type not in ref_type_stat:
                        ref_type_stat[type] = collections.OrderedDict()
                        ref_type_stat[type][ref_set_str] = 1
                    else:
                        if ref_set_str not in ref_type_stat[type]:
                            ref_type_stat[type][ref_set_str] = 1
                        else:
                            ref_type_stat[type][ref_set_str] += 1
            return ref_type_stat

    def calculate_entity_score(self):
        entity_type_stat = self.statistics_entity(self.annotate_results)
        print("entity_type_stat:",entity_type_stat)
        return calculate_score(entity_type_stat,self.annotator_num)

    def calculate_relation_score(self):
        relation_type_stat = self.statistics_relation(self.annotate_results)
        return calculate_score(relation_type_stat,self.annotator_num)

    def calculate_coref_score(self):
        ref_type_stat = self.statistics_coref(self.annotate_results)
        return calculate_score(ref_type_stat,self.annotator_num)

class KEIAAOneTask(object):
    def __init__(self,annotate_results):
        self.annotate_results = annotate_results
        self.annotator_num = len(annotate_results)
    def statistics_entity(self,annotate_results):
        if len(annotate_results)==2:
            first_annotator_docs = annotate_results[0]
            second_annotator_docs = annotate_results[1]
            assert len(first_annotator_docs)==len(second_annotator_docs)
            total_same_num = 0
            total_first_num = 0
            total_second_num = 0
            entity_type_stat = collections.OrderedDict()
            entity_set = set()
            for k in range(len(first_annotator_docs)): #第k篇文档
                first_annotator_doc = first_annotator_docs[k]
                second_annotator_doc = second_annotator_docs[k]
                first_annotator_sentences = first_annotator_doc['SentenceList']['Sentences']
                second_annotator_sentences = second_annotator_doc['SentenceList']['Sentences']
                assert len(first_annotator_sentences)==len(second_annotator_sentences)
                for i in range(len(first_annotator_sentences)):  # i表示第几句话,要求sentence有序。
                    entities_1 = first_annotator_sentences[i]['Entities']
                    entities_2 = second_annotator_sentences[i]['Entities']
                    for entity in entities_1:
                        start = str(entity['Start'])
                        end = str(entity['End'])
                        type = str(entity['Type'])
                        is_placeholder = '1' if entity['IsPlaceHolder'] else '0'
                        entity_name = '-'.join([str(k),str(i), start, end, type, is_placeholder])
                        total_first_num += 1
                        if type not in entity_type_stat:
                            entity_type_stat[type] = collections.OrderedDict()
                            entity_type_stat[type]['same_num'] = 0
                            entity_type_stat[type]['first_num'] = 1
                            entity_type_stat[type]['second_num'] = 0
                        else:
                            entity_type_stat[type]['first_num'] += 1
                        if entity_name not in entity_set:
                            entity_set.add(entity_name)
                        else:
                            entity_type_stat[type]['same_num'] += 1
                            total_same_num += 1
                    for entity in entities_2:
                        start = str(entity['Start'])
                        end = str(entity['End'])
                        type = str(entity['Type'])
                        is_placeholder = '1' if entity['IsPlaceHolder'] else '0'
                        entity_name = '-'.join([str(k),str(i), start, end, type, is_placeholder])
                        total_second_num += 1
                        if type not in entity_type_stat:
                            entity_type_stat[type] = collections.OrderedDict()
                            entity_type_stat[type]['same_num'] = 0
                            entity_type_stat[type]['first_num'] = 0
                            entity_type_stat[type]['second_num'] = 1
                        else:
                            entity_type_stat[type]['second_num'] += 1
                        if entity_name not in entity_set:
                            entity_set.add(entity_name)
                        else:
                            entity_type_stat[type]['same_num'] += 1
                            total_same_num += 1


            entity_type_stat['all'] = collections.OrderedDict()
            entity_type_stat['all']['first_num'] = total_first_num
            entity_type_stat['all']['second_num'] = total_second_num
            entity_type_stat['all']['same_num'] = total_same_num

            return entity_type_stat

        elif len(annotate_results)>=3:
            #entity_type_stat = collections.OrderedDict()
            entity_type_stat = {}
            for annotator_docs in annotate_results:
                for k,doc in enumerate(annotator_docs):#第j篇文档
                    sentences = doc['SentenceList']['Sentences']
                    for i, sentence in enumerate(sentences):
                        entities = sentence['Entities']
                        for entity in entities:
                            start = str(entity['Start'])
                            end = str(entity['End'])
                            type = str(entity['Type'])
                            is_placeholder = '1' if entity['IsPlaceHolder'] else '0'
                            entity_name = '-'.join([str(k),str(i), start, end, type, is_placeholder])
                            if type not in entity_type_stat:
                                entity_type_stat[type] = collections.OrderedDict()
                                entity_type_stat[type][entity_name] = 1
                            else:
                                if entity_name not in entity_type_stat[type]:
                                    entity_type_stat[type][entity_name] = 1
                                else:
                                    entity_type_stat[type][entity_name] += 1
            return entity_type_stat

    def statistics_relation(self,annotate_results):
        if len(annotate_results)==2:
            first_annotator_docs = annotate_results[0]
            second_annotator_docs = annotate_results[1]
            assert len(first_annotator_docs)==len(second_annotator_docs)
            total_same_num = 0
            total_first_num = 0
            total_second_num = 0
            relation_type_stat = collections.OrderedDict()
            relation_set = set()
            for k in range(len(first_annotator_docs)): #第k篇文档
                first_annotator_doc = first_annotator_docs[k]
                second_annotator_doc = second_annotator_docs[k]
                first_annotator_sentences = first_annotator_doc['SentenceList']['Sentences']
                second_annotator_sentences = second_annotator_doc['SentenceList']['Sentences']
                assert len(first_annotator_sentences)==len(second_annotator_sentences)
                for i in range(len(first_annotator_sentences)):  # i表示第几句话,要求sentence有序。
                    relations_1 = first_annotator_sentences[i]['Relations']
                    relations_2 = second_annotator_sentences[i]['Relations']
                    for relation in relations_1:
                        start = str(relation['Args'][0])
                        end = str(relation['Args'][1])
                        type = str(relation['Type'])
                        relation_name = '-'.join([str(k),str(i), start, end, type])
                        total_first_num += 1
                        if type not in relation_type_stat:
                            relation_type_stat[type] = collections.OrderedDict()
                            relation_type_stat[type]['same_num'] = 0
                            relation_type_stat[type]['first_num'] = 1
                            relation_type_stat[type]['second_num'] = 0
                        else:
                            relation_type_stat[type]['first_num'] += 1
                        if relation_name not in relation_set:
                            relation_set.add(relation_name)
                        else:
                            relation_type_stat[type]['same_num'] += 1
                            total_same_num += 1
                    for relation in relations_2:
                        start = str(relation['Args'][0])
                        end = str(relation['Args'][1])
                        type = str(relation['Type'])
                        relation_name = '-'.join([str(k), str(i), start, end, type])
                        total_second_num += 1
                        if type not in relation_type_stat:
                            relation_type_stat[type] = collections.OrderedDict()
                            relation_type_stat[type]['same_num'] = 0
                            relation_type_stat[type]['first_num'] = 0
                            relation_type_stat[type]['second_num'] = 1
                        else:
                            relation_type_stat[type]['second_num'] += 1
                        if relation_name not in relation_set:
                            relation_set.add(relation_name)
                        else:
                            relation_type_stat[type]['same_num'] += 1
                            total_same_num += 1


            relation_type_stat['all'] = collections.OrderedDict()
            relation_type_stat['all']['first_num'] = total_first_num
            relation_type_stat['all']['second_num'] = total_second_num
            relation_type_stat['all']['same_num'] = total_same_num

            return relation_type_stat

        elif len(annotate_results)>=3:
            relation_type_stat = collections.OrderedDict()
            for annotator_docs in annotate_results:
                for k,doc in enumerate(annotator_docs):#第j篇文档
                    sentences = doc['SentenceList']['Sentences']
                    for i, sentence in enumerate(sentences):
                        relations = sentence['relations']
                        for relation in relations:
                            start = str(relation['Args'][0])
                            end = str(relation['Args'][1])
                            type = str(relation['Type'])
                            relation_name = '-'.join([str(k), str(i), start, end, type])
                            if type not in relation_type_stat:
                                relation_type_stat[type] = collections.OrderedDict()
                                relation_type_stat[type][relation_name] = 1
                            else:
                                if relation_name not in relation_type_stat[type]:
                                    relation_type_stat[type][relation_name] = 1
                                else:
                                    relation_type_stat[type][relation_name] += 1
            return relation_type_stat

    def statistics_coref(self,annotate_results):
        if len(annotate_results)==2:
            first_annotator_docs = annotate_results[0]
            second_annotator_docs = annotate_results[1]
            assert len(first_annotator_docs) == len(second_annotator_docs)
            total_same_num = 0
            total_first_num = 0
            total_second_num = 0
            ref_type_stat = collections.OrderedDict()
            ref_set = set()
            for k in range(len(first_annotator_docs)):  # 第k篇文档
                first_annotator_doc = first_annotator_docs[k]
                second_annotator_doc = second_annotator_docs[k]

                first_annotator_corefs = first_annotator_doc['CorefChains']
                second_annotator_corefs = second_annotator_doc['CorefChains']

                for ref in first_annotator_corefs:
                    ref_str = str(k)
                    type = ref['Type']
                    ref_set.add(type)
                    ref_str = ref_str + str(type)
                    for entity in sorted(ref['Entities']):
                        ref_str = ref_str + str(entity)
                    total_first_num += 1
                    if type not in ref_type_stat:
                        ref_type_stat[type] = collections.OrderedDict()
                        ref_type_stat[type]['same_num'] = 0
                        ref_type_stat[type]['first_num'] = 1
                        ref_type_stat[type]['second_num'] = 0
                    else:
                        ref_type_stat[type]['first_num'] += 1
                    if ref_str not in ref_set:
                        ref_set.add(ref_str)
                    else:
                        ref_type_stat[type]['same_num'] += 1
                        total_same_num += 1
                for ref in second_annotator_corefs:
                    ref_str = str(k)
                    type = ref['Type']
                    ref_set.add(type)
                    ref_str = ref_str + str(type)
                    for entity in sorted(ref['Entities']):
                        ref_str = ref_str + str(entity)
                    total_first_num += 1
                    if type not in ref_type_stat:
                        ref_type_stat[type] = collections.OrderedDict()
                        ref_type_stat[type]['same_num'] = 0
                        ref_type_stat[type]['first_num'] = 1
                        ref_type_stat[type]['second_num'] = 0
                    else:
                        ref_type_stat[type]['first_num'] += 1
                    if ref_str not in ref_set:
                        ref_set.add(ref_str)
                    else:
                        ref_type_stat[type]['same_num'] += 1
                        total_same_num += 1

                ref_type_stat['all'] = collections.OrderedDict()
                ref_type_stat['all']['first_num'] = total_first_num
                ref_type_stat['all']['second_num'] = total_second_num
                ref_type_stat['all']['same_num'] = total_same_num

            return ref_type_stat
        elif len(annotate_results)>=3:
            ref_type_stat = collections.OrderedDict()
            for annotator_docs in annotate_results:
                for k, doc in enumerate(annotator_docs):  # 第k篇文档
                    annotator_corefs = doc['CorefChains']
                    for ref in annotator_corefs:
                        type = ref['Type']
                        ref_set_str = str(k)
                        ref_set_str = ref_set_str+str(type)
                        for entity in sorted(ref['Entities']):
                            ref_set_str = ref_set_str + str(entity)
                        if type not in ref_type_stat:
                            ref_type_stat[type] = collections.OrderedDict()
                            ref_type_stat[type][ref_set_str] = 1
                        else:
                            if ref_set_str not in ref_type_stat[type]:
                                ref_type_stat[type][ref_set_str] = 1
                            else:
                                ref_type_stat[type][ref_set_str] += 1
            return ref_type_stat

    def calculate_entity_score(self):
        entity_type_stat = self.statistics_entity(self.annotate_results)
        return calculate_score(entity_type_stat,self.annotator_num)

    def calculate_relation_score(self):
        relation_type_stat = self.statistics_relation(self.annotate_results)
        return calculate_score(relation_type_stat,self.annotator_num)

    def calculate_coref_score(self):
        ref_type_stat = self.statistics_coref(self.annotate_results)
        return calculate_score(ref_type_stat,self.annotator_num)




