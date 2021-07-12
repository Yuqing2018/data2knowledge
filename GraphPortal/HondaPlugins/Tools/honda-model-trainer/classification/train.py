# -*- coding: utf-8 -*-
##  1. part prediction
import sys, os
os.environ["CUDA_VISIBLE_DEVICES"]="0"  ##请根据实际GPU情况进行调整
os.environ["TF_KERAS"]="1"

import numpy as np
from keras_bert import load_vocabulary, load_trained_model_from_checkpoint, Tokenizer, get_checkpoint_paths
from tqdm import tqdm
from tensorflow import keras
import tensorflow as tf
from keras_adamw import AdamW
#from tensorflow.keras.optimizers import Adam
import json

import argparse
parser = argparse.ArgumentParser()

parser.add_argument('--checkpoint_path', type=str, help='Path to the pretrained model',default='./models/pretrained_lm/chinese_L-12_H-768_A-12/')
#parser.add_argument('--checkpoint_path', type=str, help='Path to the pretrained model',default='/home/j-jingz/work/pre_trained_models/chinese_L-12_H-768_A-12/')
parser.add_argument('--part_train_path',type=str, help='train file path',default='./data/train')
parser.add_argument('--part_dev_path',type=str, help='dev file path',default='./data/val')
parser.add_argument('--part_test_path',type=str, help='train file path',default='./data/test')
parser.add_argument('--part_label_path',type=str, help='label file path',default='./data/new_partsname.txt')
parser.add_argument('--part_padding_mode',type=str, help='padding mode: pre or post',default='pre',choices=['pre','post'])
parser.add_argument('--part_model_save_path',type=str,help='part model weights save path',default='./models/trained_models/part_model')
parser.add_argument('--part_learning_rate',type=float,help='learning rate',default=2e-5)
parser.add_argument('--part_batch_size',type=float,help='batch size',default=16)
parser.add_argument('--part_epochs',type=int,help='training epoch',default=7)
parser.add_argument('--part_maxlen',type=int,help='max length of input',default=256)
parser.add_argument('--train_part_flag',type=bool,help='train part model or not',default=False)
parser.add_argument('--part_droupout_rate',type=float,help='droupout rate',default=0.1)


parser.add_argument('--syndrome_train_path',type=str, help='train file path',default='./data/train')
parser.add_argument('--syndrome_dev_path',type=str, help='dev file path',default='./data/val')
parser.add_argument('--syndrome_test_path',type=str, help='train file path',default='./data/test')
parser.add_argument('--syndrome_label_path',type=str, help='label file path',default='./data/new_syndromelist.txt')
parser.add_argument('--all_syndrome_path',type=str, help='label file path',default='./data/AllSyndrome.json')
parser.add_argument('--syndrome_padding_mode',type=str, help='padding mode: pre or post',default='pre',choices=['pre','post'])
parser.add_argument('--syndrome_model_save_path',type=str,help='syndrome model weights save path',default='./models/trained_models/syndrome_model')
parser.add_argument('--syndrome_learning_rate',type=float,help='learning rate',default=2e-5)
parser.add_argument('--syndrome_batch_size',type=float,help='batch size',default=24)
parser.add_argument('--syndrome_epochs',type=int,help='training epoch',default=10)
parser.add_argument('--syndrome_maxlen',type=int,help='max length of input',default=256)
parser.add_argument('--train_syndrome_flag',type=bool,help='train syndrome model or not',default=True)
parser.add_argument('--syndrome_droupout_rate',type=float,help='droupout rate',default=0.3)
args = parser.parse_args()

train_syndrome_flag = args.train_syndrome_flag
if train_syndrome_flag:
    MAX_LEN=args.syndrome_maxlen  # sentence max length,used to tunc long sentence
    ##可能要更改地址 取决于'chinese_L-12_H-768_A-12/' 保存在哪
    paths = get_checkpoint_paths(args.checkpoint_path)
    bert_model = load_trained_model_from_checkpoint(paths.config, paths.checkpoint, training=False,trainable=True,seq_len=MAX_LEN)
    # model.summary(line_length=120)
    token_dict = load_vocabulary(paths.vocab)
    tokenizer = Tokenizer(token_dict)


    ##2. syndrome——prediction
    TRAIN_FILE = args.syndrome_train_path
    DEV_FILE= args.syndrome_dev_path
    TEST_FILE= args.syndrome_test_path

    LABEL_FILE = args.syndrome_label_path
    MAX_LEN= args.syndrome_maxlen  # sentence max length,used to tunc long sentence
    PADDING_MODE = args.syndrome_padding_mode # 'pre' or 'post'
    BATCH_SIZE= args.syndrome_batch_size
    RANDOM_SEED= 15
    #model_path = '1st_part_saved_model'
    # random.seed(RANDOM_SEED)
    np.random.seed(RANDOM_SEED)
    tf.random.set_seed(RANDOM_SEED)

    with open(LABEL_FILE,encoding='utf-8') as f:
        l = f.readlines()
        label_list = [i.rstrip('\n') for i in l]
    label_num = len(label_list)
    label_dict = {k:v for v,k in enumerate(label_list)}

    #data_list=[]
    #with open(TRAIN_FILE,encoding='utf-8') as f:
    #    for line in f.readlines():
    #        line = line.strip()
    #        if len(line.split('@@@@')) == 4:
    #            data_list.append(line.split('@@@@'))


    def load_dataset_from_file(file_path):
        """
            输入数据格式为：id@@@@训练文本@@@@不良症状@@@@主零件中文名
        """
        data_list = []
        with open(file_path, encoding='utf-8') as f:
            for line in f.readlines():
                line = line.strip()
                if len(line.split('@@@@')) == 4:
                    data_list.append(line.split('@@@@'))

        i_list = []
        s_list = []
        labels = []

        for guid, feature_text, syndrome, part in tqdm(data_list):
            if not syndrome or syndrome not in label_dict:continue
            indices, segments = tokenizer.encode(first=feature_text, max_len=MAX_LEN)

            i_list.append(indices)
            s_list.append(segments)
            labels.append(label_dict[syndrome])

        # convert to numpy array
        query_i_np = np.array(i_list)
        query_s_np = np.array(s_list)
        labels_onehot = tf.one_hot(labels, depth=label_num)
        return query_i_np, query_s_np, labels_onehot


    train_i_np, train_s_np, train_labels_onehot = load_dataset_from_file(TRAIN_FILE)
    dev_i_np, dev_s_np, dev_labels_onehot = load_dataset_from_file(DEV_FILE)
    test_i_np, test_s_np, test_labels_onehot = load_dataset_from_file(TEST_FILE)

    query_i = keras.layers.Input(shape =(MAX_LEN,),name='query_indice')
    query_s = keras.layers.Input(shape =(MAX_LEN,),name='query_seg')
    sent_word_embedding = bert_model([query_i,query_s])

    first_token_tensor = tf.squeeze(sent_word_embedding[:, 0:1, :], axis=1)
    x = keras.layers.Dense(768,activation='tanh',kernel_initializer=keras.initializers.TruncatedNormal(stddev=0.02))(first_token_tensor)
    x = keras.layers.Dropout(args.syndrome_droupout_rate)(x)
    prop = keras.layers.Dense(label_num,activation='softmax',kernel_initializer=keras.initializers.TruncatedNormal(stddev=0.02),name='probability')(x)

    train_model = keras.Model([query_i,query_s],prop,name='classify_prob')
    adam_opt = AdamW(learning_rate=args.syndrome_learning_rate)
    #adam_opt = Adam(args.syndrome_learning_rate)
    train_model.compile(loss=tf.keras.losses.CategoricalCrossentropy(), optimizer=adam_opt, metrics=['accuracy'])
    checkpoint_callback = tf.keras.callbacks.ModelCheckpoint(
        'checkpoints/weights.{epoch:02d}-{val_loss:.2f}', monitor='val_acc', verbose=0, save_best_only=True,
        save_weights_only=True, mode='auto', save_freq='epoch')
    print("Begin to train syndrome classification model")
    history = train_model.fit((train_i_np,train_s_np),train_labels_onehot,epochs=args.syndrome_epochs,validation_data=((dev_i_np,dev_s_np),dev_labels_onehot),callbacks=[checkpoint_callback], batch_size = BATCH_SIZE)
    print("Syndrome classification model training finished!")
    ##evaluate the syndrome model
    print("Start to evaluate...")
    train_model.evaluate((test_i_np,test_s_np),test_labels_onehot)

    label_tensor = tf.constant(label_list)
    ##"AllSyndrome_20201214.json" 是包含syndrome_id的映射文件
    with open(args.all_syndrome_path, encoding="utf8") as f:
        data = json.load(f)

    ##1:
    id_name = {}
    for i in data:
        index = i["id"]
        name = i["Name"]
        id_name[name] = index
    ##2:
    syndrome_ids = []
    for i in label_list:
        if i not in id_name:continue
        syndrome_ids.append(id_name[i])
    syndr_tensor = tf.constant(syndrome_ids)

    i = keras.layers.Input(shape =(MAX_LEN,),name='text_indice',dtype='int64')
    s = keras.layers.Input(shape =(MAX_LEN,),name='text_seg',dtype='int64')
    prob = train_model((i,s))
    max_prob = keras.layers.Lambda( lambda x: tf.reduce_max(x,axis=1),name='max_prob')(prob)
    pred = keras.layers.Lambda( lambda x: tf.argmax(x,axis=1))(prob)    ##pred: numpy array (ind)

    labels = keras.layers.Lambda( lambda x : tf.gather(label_tensor,x),name='pred_label')(pred)
    # syndr_ind = keras.layers.Lambda(lambda x: tf.gather(syndr_list_tensor,x),name='syndr_id')(pred)
    syndr_ind = keras.layers.Lambda(lambda x: tf.gather(syndr_tensor,x),name='syndr_id')(pred)

    model_to_store = keras.Model(inputs=[i,s],outputs=[prob,max_prob,labels, syndr_ind])

    #model_to_store.summary()

    #set optimizer to a empty one ,this is a workaroud of [this bug](https://github.com/tensorflow/tensorflow/issues/40380)
    train_model.optimizer = tf.keras.optimizers.SGD()
    # model_save_path = 'alldata_syndrome_bert_saved_model'
    model_save_path = args.syndrome_model_save_path

    model_to_store.save(model_save_path)
    del train_model


train_part_flag = args.train_part_flag
if train_part_flag:
    MAX_LEN=args.part_maxlen  # sentence max length,used to tunc long sentence
    ##可能要更改地址 取决于'chinese_L-12_H-768_A-12/' 保存在哪
    paths = get_checkpoint_paths(args.checkpoint_path)
    bert_model = load_trained_model_from_checkpoint(paths.config, paths.checkpoint, training=False,trainable=True,seq_len=MAX_LEN)
    # model.summary(line_length=120)
    token_dict = load_vocabulary(paths.vocab)
    tokenizer = Tokenizer(token_dict)

    TRAIN_FILE = args.part_train_path
    DEV_FILE= args.part_dev_path
    TEST_FILE= args.part_test_path

    LABEL_FILE = args.part_label_path

    PADDING_MODE = args.part_padding_mode
    BATCH_SIZE= args.part_batch_size
    RANDOM_SEED= 15
    #model_path = '1st_part_saved_model'
    # random.seed(RANDOM_SEED)
    np.random.seed(RANDOM_SEED)
    tf.random.set_seed(RANDOM_SEED)

    with open(LABEL_FILE,encoding='utf-8') as f:
        l = f.readlines()
        label_list = [i.rstrip('\n') for i in l]
    label_num = len(label_list)
    label_dict = {k:v for v,k in enumerate(label_list)}
    # assert len(label_dict) ==label_num


    def load_dataset_from_file(file_path,total_num):
        """
            输入数据格式为：id@@@@训练文本@@@@不良症状@@@@主零件中文名
        """
        data_list = []
        with open(file_path, encoding='utf-8') as f:
            for line in f.readlines():
                line = line.strip()
                if len(line.split('@@@@')) == 4:
                    data_list.append(line.split('@@@@'))

        #     return data_list

        i_list = []
        s_list = []
        labels = []
        data_list = data_list[:total_num]
        for guid, feature_text, syndrome, part in tqdm(data_list):
            if not part or part not in label_dict:continue
            indices, segments = tokenizer.encode(first=feature_text, max_len=MAX_LEN)
            i_list.append(indices)
            s_list.append(segments)
            labels.append(label_dict[part])

        # convert to numpy array
        query_i_np = np.array(i_list)
        query_s_np = np.array(s_list)
        labels_onehot = tf.one_hot(labels, depth=label_num)
        return query_i_np, query_s_np, labels_onehot


    train_i_np, train_s_np, train_labels_onehot = load_dataset_from_file(TRAIN_FILE,370000)
    dev_i_np, dev_s_np, dev_labels_onehot = load_dataset_from_file(DEV_FILE,15000)
    test_i_np, test_s_np, test_labels_onehot = load_dataset_from_file(TEST_FILE,15000)

    ##build the model
    query_i = keras.layers.Input(shape =(MAX_LEN,),name='query_indice')
    query_s = keras.layers.Input(shape =(MAX_LEN,),name='query_seg')
    sent_word_embedding = bert_model([query_i,query_s])

    first_token_tensor = tf.squeeze(sent_word_embedding[:, 0:1, :], axis=1)
    x = keras.layers.Dense(768,activation='tanh',kernel_initializer=keras.initializers.TruncatedNormal(stddev=0.02))(first_token_tensor)
    x = keras.layers.Dropout(args.part_droupout_rate)(x)
    prop = keras.layers.Dense(label_num,activation='softmax',kernel_initializer=keras.initializers.TruncatedNormal(stddev=0.02),name='probability')(x)

    train_model = keras.Model([query_i,query_s],prop,name='classify_prob')

    adam_opt = AdamW(learning_rate=args.part_learning_rate)
    #adam_opt = Adam(args.part_learning_rate)
    train_model.compile(loss=tf.keras.losses.CategoricalCrossentropy(), optimizer=adam_opt, metrics=['acc'])

    checkpoint_callback = tf.keras.callbacks.ModelCheckpoint(
        'checkpoints/weights.{epoch:02d}-{val_loss:.2f}', monitor='val_acc', verbose=0, save_best_only=True,
        save_weights_only=True, mode='auto', save_freq='epoch')
    print("Begin to train part classification model")
    history = train_model.fit((train_i_np,train_s_np),train_labels_onehot,epochs=args.part_epochs,validation_data=((dev_i_np,dev_s_np),dev_labels_onehot),callbacks=[checkpoint_callback], batch_size = BATCH_SIZE)
    print("Part classification model training finished!")
    train_model.evaluate((test_i_np,test_s_np),test_labels_onehot)
    ##将预测part模型导出成saved_model
    label_tensor = tf.constant(label_list)

    i = keras.layers.Input(shape =(MAX_LEN,),name='text_indice',dtype='int64')
    s = keras.layers.Input(shape =(MAX_LEN,),name='text_seg',dtype='int64')
    prob = train_model((i,s))

    max_prob = keras.layers.Lambda( lambda x: tf.reduce_max(x,axis=1),name='max_prob')(prob)
    pred = keras.layers.Lambda( lambda x: tf.argmax(x,axis=1),)(prob)
    labels = keras.layers.Lambda( lambda x : tf.gather(label_tensor,x),name='pred_label')(pred)
    model_to_store = keras.Model(inputs=[i,s],outputs=[prob,max_prob,labels])
    model_to_store.summary()
    train_model.optimizer = tf.keras.optimizers.SGD()

    model_to_store.save(args.part_model_save_path)
    del train_model



