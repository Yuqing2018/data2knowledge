# -*- coding:utf-8 -*-
import collections
import os
import modeling
import tokenization
import tensorflow as tf
import numpy as np
import hashlib
import logging

from config import FLAGS

CUDA_VISIBLE_DEVICES=0
logger = logging.getLogger(__name__)

def get_labels():

    # from ontology import ontology
    # entities = ontology['entities']
    # labels = []
    # # X是一个英文单词被split之后，#xxx的label,PAD是input长度小于max_length时，Label_id append 'PAD'对应的id即0
    # labels.extend(["PAD", "O", "X", "[CLS]", "[SEP]"])
    # for entity_type in entities:
    #     labels.append("B-" + entity_type.lower())
    #     labels.append("I-" + entity_type.lower())
    #
    # return labels
    return ["PAD", "O", "X", "[CLS]", "[SEP]", 'B-LOC', 'I-LOC', 'B-PER', 'I-PER', 'B-ORG', 'I-ORG']


class InputExample(object):
    """A single training/test example for simple sequence classification."""

    def __init__(self, guid, text, label=None):
        """Constructs a InputExample.

        Args:
          guid: Unique id for the example.
          text_a: string. The untokenized text of the first sequence. For single
            sequence tasks, only this sequence must be specified.
          label: (Optional) string. The label of the example. This should be
            specified for train and dev examples, but not for test examples.
        """
        self.guid = guid
        self.text = text
        self.label = label


class InputFeatures(object):
    """A single set of features of data."""

    def __init__(self, input_ids, input_mask, segment_ids, label_ids):
        self.input_ids = input_ids
        self.input_mask = input_mask
        self.segment_ids = segment_ids
        self.label_ids = label_ids
        #self.label_mask = label_mask


def convert_single_example(ex_index, example, label_list, max_seq_length, tokenizer,mode=None):
    label_map = {}
    for (i, label) in enumerate(label_list):
        label_map[label] = i
    textlist = example.text.split(' ')
    labellist = example.label.split(' ')
    tokens = []
    labels = []
    if len(textlist)!=len(labellist):
        logger.warning("len(textlist) doesn't equal len(labellist)")
        logger.warning("text:"+ str(textlist))
        logger.warning("lables:"+ str(labellist))
    for i, word in enumerate(textlist):
        token = tokenizer.tokenize(word)
        tokens.extend(token)
        label_1 = labellist[i]
        for m in range(len(token)):
            if m == 0:
                labels.append(label_1)
            else:
                labels.append("X")
    # tokens = tokenizer.tokenize(example.text)
    if len(tokens) >= max_seq_length - 1:
        tokens = tokens[0:(max_seq_length - 2)]
        labels = labels[0:(max_seq_length - 2)]
    ntokens = []
    segment_ids = []
    label_ids = []
    ntokens.append("[CLS]")
    segment_ids.append(0)
    label_ids.append(label_map["[CLS]"])
    for i, token in enumerate(tokens):
        ntokens.append(token)
        segment_ids.append(0)
        label_ids.append(label_map[labels[i]])
    ntokens.append("[SEP]")
    segment_ids.append(0)
    label_ids.append(label_map["[SEP]"])
    input_ids = tokenizer.convert_tokens_to_ids(ntokens)
    input_mask = [1] * len(input_ids)
    while len(input_ids) < max_seq_length:
        input_ids.append(0)
        input_mask.append(0)
        segment_ids.append(0)
        label_ids.append(0)
        ntokens.append("PAD")
    assert len(input_ids) == max_seq_length
    assert len(input_mask) == max_seq_length
    assert len(segment_ids) == max_seq_length
    assert len(label_ids) == max_seq_length

    # if ex_index < 1:
    #     tf.logging.info("*** Example ***")
    #     tf.logging.info("guid: %s" % (example.guid))
    #     tf.logging.info("tokens: %s" % " ".join(
    #         [tokenization.printable_text(x) for x in tokens]))
    #     tf.logging.info("input_ids: %s" % " ".join([str(x) for x in input_ids]))
    #     tf.logging.info("input_mask: %s" % " ".join([str(x) for x in input_mask]))
    #     tf.logging.info("segment_ids: %s" % " ".join([str(x) for x in segment_ids]))
    #     tf.logging.info("label_ids: %s" % " ".join([str(x) for x in label_ids]))

    feature = InputFeatures(
        input_ids=input_ids,
        input_mask=input_mask,
        segment_ids=segment_ids,
        label_ids=label_ids,
    )
    return feature


def create_model(bert_config, is_training, input_ids, input_mask,
                 segment_ids, labels, num_labels, use_one_hot_embeddings):
    model = modeling.BertModel(
        config=bert_config,
        is_training=is_training,
        input_ids=input_ids,
        input_mask=input_mask,
        token_type_ids=segment_ids,
        use_one_hot_embeddings=use_one_hot_embeddings
    )

    output_layer = model.get_sequence_output()

    hidden_size = output_layer.shape[-1].value

    output_weight = tf.get_variable(
        "output_weights", [num_labels, hidden_size],
        initializer=tf.truncated_normal_initializer(stddev=0.02)
    )
    output_bias = tf.get_variable(
        "output_bias", [num_labels], initializer=tf.zeros_initializer()
    )
    with tf.variable_scope("loss"):
        if is_training:
            output_layer = tf.nn.dropout(output_layer, keep_prob=0.9)
        output_layer = tf.reshape(output_layer, [-1, hidden_size])
        logits = tf.matmul(output_layer, output_weight, transpose_b=True)
        logits = tf.nn.bias_add(logits, output_bias)
        logits = tf.reshape(logits, [-1, FLAGS.max_seq_length, num_labels])
        # mask = tf.cast(input_mask,tf.float32)
        # loss = tf.contrib.seq2seq.sequence_loss(logits,labels,mask)
        # return (loss, logits, predict)
        ##########################################################################
        log_probs = tf.nn.log_softmax(logits, axis=-1)
        one_hot_labels = tf.one_hot(labels, depth=num_labels, dtype=tf.float32)
        per_example_loss = -tf.reduce_sum(one_hot_labels * log_probs, axis=-1)
        loss = tf.reduce_sum(per_example_loss)
        probabilities = tf.nn.softmax(logits, axis=-1)
        predict = tf.argmax(probabilities,axis=-1)
        return (loss, per_example_loss, logits,predict)
        ##########################################################################


class NerPredictor(object):
    def __init__(self):
        self.bert_config = modeling.BertConfig.from_json_file(FLAGS.bert_config_file)
        if FLAGS.max_seq_length > self.bert_config.max_position_embeddings:
            raise ValueError(
                "Cannot use sequence length %d because the BERT model "
                "was only trained up to sequence length %d" %
                (FLAGS.max_seq_length, self.bert_config.max_position_embeddings))

        self.label_list = get_labels()
        self.label2id = {v : k for k,v in enumerate(self.label_list)}
        self.id2label = {k : v for k,v in enumerate(self.label_list)}
        self.tokenizer = tokenization.FullTokenizer(vocab_file=FLAGS.vocab_file,do_lower_case=FLAGS.do_lower_case)
        is_training = False
        use_one_hot_embeddings = False
        batch_size = 1
        num_labels = len(self.label_list)
        gpu_config = tf.ConfigProto()
        gpu_config.gpu_options.allow_growth = True
        self.sess = tf.Session(config=gpu_config)
        if not os.path.exists(FLAGS.init_checkpoint + "checkpoint"):
            raise Exception("failed to get checkpoint. going to return ")
        self.input_ids = tf.placeholder(tf.int32, [batch_size, FLAGS.max_seq_length], name="input_ids")
        self.input_mask = tf.placeholder(tf.int32, [batch_size, FLAGS.max_seq_length], name="input_mask")
        self.label_ids = tf.placeholder(tf.int32, [batch_size, FLAGS.max_seq_length], name="label_ids")
        self.segment_ids = tf.placeholder(tf.int32, [batch_size, FLAGS.max_seq_length], name="segment_ids")
        (loss, per_example_loss, logits, self.predict) = create_model(self.bert_config,
                                                                 is_training,
                                                                 self.input_ids,
                                                                 self.input_mask,
                                                                 self.segment_ids,
                                                                 self.label_ids,
                                                                 num_labels,
                                                                 use_one_hot_embeddings)
        saver = tf.train.Saver()
        saver.restore(self.sess, tf.train.latest_checkpoint(FLAGS.init_checkpoint))
        self.predict_sentence("周杰伦唱过七里香。")

    def predict_paragraph_dict(self,json_obj):
        total_count = json_obj["SentenceList"]["TotalCount"]
        sentences = json_obj["SentenceList"]["Sentences"]
        assert len(sentences) == total_count
        for sentence in sentences:
            orig_text = sentence["Text"]
            entity_list = self.predict_sentence(orig_text)
            if 'Entities' not in sentence:
                sentence['Entities'] = {}
            sentence["Entities"]["ByModel"] = entity_list

    def predict_paragraph(self,json_objs):
        if type(json_objs)==list:
            for json_obj in json_objs:
                self.predict_paragraph_dict(json_obj)
        elif type(json_objs)==dict:
            self.predict_paragraph_dict(json_objs)
        return json_objs

    def merge_tag(self,predicts, sentence):
        entity_list = []
        for i in range(len(predicts)):
            if predicts[i].startswith("B"):
                begin_index = i
                entity_type = predicts[i][2:]
                end_index = i + 1
                for j in range(i + 1, len(predicts)):
                    if predicts[j].startswith("I"):
                        end_index += 1
                        i += 1
                    else:
                        break
                text = sentence[begin_index:end_index]
                entity = collections.OrderedDict()
                md5 = hashlib.md5()
                md5.update((str(sentence)+str(text)+str(begin_index)+str(end_index)).encode(encoding='UTF-8',errors='ignore'))
                entity["Id"] = md5.hexdigest() if md5 else ''
                entity["User"] = None
                entity["Type"] = entity_type
                entity["Text"] = text
                entity["Start"] = int(begin_index)
                entity["End"] = int(end_index)
                entity["IsPlaceHolder"] = False
                entity["PlaceHolderId"] = None
                entity["InCoref"] = False
                entity_list.append(entity)

        return entity_list

    def predict_sentence(self,sentence):
        text_length = len(list(sentence))
        label = ['O'] * text_length
        text = ' '.join(list(sentence))
        label = ' '.join(label)
        text = tokenization.convert_to_unicode(text)
        label = tokenization.convert_to_unicode(label)
        example = InputExample(guid="0", text=text, label=label)
        feature = convert_single_example(0, example, self.label_list, FLAGS.max_seq_length, self.tokenizer)
        input_ids = np.reshape([feature.input_ids], (1, FLAGS.max_seq_length))
        input_mask = np.reshape([feature.input_mask], (1, FLAGS.max_seq_length))
        label_ids = np.reshape([feature.label_ids], (1, FLAGS.max_seq_length))
        segment_ids = np.reshape([feature.segment_ids], (1, FLAGS.max_seq_length))
        feed_dict = {self.input_ids: input_ids, self.input_mask: input_mask, self.segment_ids: segment_ids,
                     self.label_ids: label_ids}
        predicts = self.sess.run([self.predict], feed_dict=feed_dict)
        # list of ndarray predicts[0]表示第一个样本预测结果，是个ndarray  batch_size=1
        # ndarray 是1*128维度
        predicts = predicts[0][0].tolist()
        predicts = [self.id2label[predicts[i]] for i in range(1,text_length+1)]
        entity_list = self.merge_tag(predicts, sentence)
        return entity_list








