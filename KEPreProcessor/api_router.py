# coding=utf-8
from flask import Blueprint
from flask import request,render_template
from flask import request,flash,redirect
from werkzeug.utils import secure_filename
from ensemble_processor import Processor
from pre_dict_predictor import MultiDictPredictor
from pre_regex_predictor import MultiRegexPredictor
from inter_annotator_agreement import KEIAAOneDoc,KEIAAOneTask
import collections
import json
import codecs
import os

api = Blueprint('api', __name__)

ALLOWED_EXTENSIONS = ['json']

def allowed_file(filename):
    return '.' in filename and \
           filename.rsplit('.', 1)[1].lower() in ALLOWED_EXTENSIONS

@api.route("/",methods=["GET"])
def index():
    return render_template('home.html')
@api.route("/KnowledgeExtraction/preprocess",methods=["POST"])
def preprocess():
    if request.method == 'POST':
        paragraph = request.get_data(as_text=True)
        json_objects = Processor.pre_processor.preprocess(paragraph)
        return json.dumps(json_objects,ensure_ascii=False)
    else:
        return "Please use POST method!"

@api.route("/KnowledgeExtraction/predict",methods=["POST"])
def predict():
    if request.method == 'POST':
        dict_predictor = MultiDictPredictor()
        regex_predictor = MultiRegexPredictor()
        json_objects = json.loads(request.get_data(as_text=True))
        json_objects = dict_predictor.predict_paragraph(json_objects)
        json_objects = Processor.model_predictor.predict_paragraph(json_objects)
        json_objects = regex_predictor.predict_paragraph(json_objects)
        return json.dumps(json_objects,ensure_ascii=False)
    else:
        return "Please use POST method!"

@api.route("/KnowledgeExtraction/merge",methods=["POST"])
def merge():
    if request.method == "POST":
        json_objects = json.loads(request.get_data(as_text=True))
        json_objects = Processor.merge(json_objects)
        return json.dumps(json_objects,ensure_ascii=False)
    else:
        return "Please use POST method"

@api.route("/KnowledgeExtraction/predict/sentence",methods=["POST"])
def predict_sentence():
    if request.method == "POST":
        dict_predictor = MultiDictPredictor()
        regex_predictor = MultiRegexPredictor()
        results = collections.OrderedDict()
        sentence = request.get_data(as_text=True)
        results["ByModel"] = Processor.model_predictor.predict_sentence(sentence)
        results["ByRegex"] = regex_predictor.predict_sentence(sentence)
        results["ByDict"] = dict_predictor.predict_sentence(sentence)
        return json.dumps(results,ensure_ascii=False)
    else:
        return "Please use POST method!"

@api.route("/predict_file", methods=["POST"])
def predict_file():
    if request.method == 'POST':
        if 'file' not in request.files:
            flash('No file part')
            return redirect(request.url)
        file = request.files['file']
        if file.filename == '':
            flash('No selected file')
            return redirect(request.url)
        if file and allowed_file(file.filename):
            file_path = r'./uploaded/'+'after_annotate_'+secure_filename(file.filename)
            file.save(file_path)
            dict_predictor = MultiDictPredictor()
            regex_predictor = MultiRegexPredictor()
            with codecs.open(file_path,'r',encoding='utf-8') as reader:
                json_objects = json.load(reader)
                json_objects = Processor.model_predictor.predict_paragraph(json_objects)
                json_objects = regex_predictor.predict_paragraph(json_objects)
                json_objects = dict_predictor.predict_paragraph(json_objects)
            return json.dumps(json_objects,ensure_ascii=False)

    return "Please use POST method!"



@api.route("/KnowledgeExtraction/predict/model",methods=["POST"])
def model_predict():
    if request.method == 'POST':
        json_objects = json.loads(request.get_data(as_text=True))
        json_objects = Processor.model_predictor.predict_paragraph(json_objects)
        return json.dumps(json_objects,ensure_ascii=False)
    else:
        return "Please use POST method!"

@api.route("/KnowledgeExtraction/predict/dict",methods=["POST"])
def dict_predict():
    if request.method == 'POST':
        dict_predictor = MultiDictPredictor()
        json_objects = json.loads(request.get_data(as_text=True))
        json_objects = dict_predictor.predict_paragraph(json_objects)
        return json.dumps(json_objects,ensure_ascii=False)
    else:
        return "Please use POST method!"

@api.route("/KnowledgeExtraction/predict/regex",methods=["POST"])
def regex_predict():
    if request.method == 'POST':
        regex_predictor = MultiRegexPredictor()
        json_objects = json.loads(request.get_data(as_text=True))
        json_objects = regex_predictor.predict_paragraph(json_objects)
        return json.dumps(json_objects,ensure_ascii=False)
    else:
        return "Please use POST method!"

@api.route("/KnowledgeExtraction/IAA/entity/doc",methods=["POST"])
def cal_entity_doc_score():
    if request.method == "POST":
        json_objects = json.loads(request.get_data(as_text=True))
        iaa = KEIAAOneDoc(json_objects)
        results = iaa.calculate_entity_score()
        return json.dumps(results,ensure_ascii=False)
    else:
        return "Please use POST method"

@api.route("/KnowledgeExtraction/IAA/relation/doc",methods=["POST"])
def cal_relation_doc_score():
    if request.method == "POST":
        json_objects = json.loads(request.get_data(as_text=True))
        iaa = KEIAAOneDoc(json_objects)
        results = iaa.calculate_relation_score()
        return json.dumps(results,ensure_ascii=False)
    else:
        return "Please use POST method"

@api.route("/KnowledgeExtraction/IAA/coref/doc",methods=["POST"])
def cal_coref_doc_score():
    if request.method == "POST":
        json_objects = json.loads(request.get_data(as_text=True))
        iaa = KEIAAOneDoc(json_objects)
        results = iaa.calculate_coref_score()
        return json.dumps(results,ensure_ascii=False)
    else:
        return "Please use POST method"

@api.route("/KnowledgeExtraction/IAA/entity/task",methods=["POST"])
def cal_entity_task_score():
    if request.method == "POST":
        json_objects = json.loads(request.get_data(as_text=True))
        iaa = KEIAAOneTask(json_objects)
        results = iaa.calculate_entity_score()
        return json.dumps(results,ensure_ascii=False)
    else:
        return "Please use POST method"

@api.route("/KnowledgeExtraction/IAA/relation/task",methods=["POST"])
def cal_relation_task_score():
    if request.method == "POST":
        json_objects = json.loads(request.get_data(as_text=True))
        iaa = KEIAAOneTask(json_objects)
        results = iaa.calculate_relation_score()
        return json.dumps(results,ensure_ascii=False)
    else:
        return "Please use POST method"

@api.route("/KnowledgeExtraction/IAA/coref/task",methods=["POST"])
def cal_coref_task_score():
    if request.method == "POST":
        json_objects = json.loads(request.get_data(as_text=True))
        iaa = KEIAAOneTask(json_objects)
        results = iaa.calculate_coref_score()
        return json.dumps(results,ensure_ascii=False)
    else:
        return "Please use POST method"

