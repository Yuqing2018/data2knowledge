import tensorflow as tf

flags = tf.flags

#bert related parameters
flags.DEFINE_string("bert_config_file", './tuned_models/bert_config.json',"The config json file corresponding to the pre-trained BERT model.")

flags.DEFINE_string("init_checkpoint", './tuned_models/',"Initial checkpoint (usually from a pre-trained BERT model).")

flags.DEFINE_bool("do_lower_case", True,"Whether to lower case the input text.")

flags.DEFINE_integer("max_seq_length", 128,"The maximum total input sequence length after WordPiece tokenization.")

flags.DEFINE_string("vocab_file", './tuned_models/vocab.txt',"The vocabulary file that the BERT model was trained on.")

#server related parameters
flags.DEFINE_integer("port",35002,"Port of the app")
flags.DEFINE_string("vocab_url", 'http://localhost:5003/vocab',"Get vocabularies from this url through get method")
flags.DEFINE_string("regex_url", 'http://localhost:5003/regex',"Get regular expressions dict from this url through get method")
flags.DEFINE_integer("vocab_thread", 500000 ,"If vocab_len of MultiDictPredictor bigger than vocab_thread, multiprocessing predictor will be used")

#pre-annotation results merge strategy
flags.DEFINE_bool("allow_cross", False,"Whether allow cross when merge three kinds of annotation results")

#IAA related parameters
flags.DEFINE_integer("entity_num_thread",10,"If entity num smaller than this thread,then set theoretical agreement to zero")
flags.DEFINE_bool("repeat_identical_annotation",False,"Whether set theoretical agreement is zero")

FLAGS = flags.FLAGS