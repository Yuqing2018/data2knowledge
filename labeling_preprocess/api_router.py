# coding=utf-8
from flask import Blueprint
from flask import request,render_template
from flask import request,flash,redirect
from werkzeug.utils import secure_filename

import collections
import json
from preprocessing import PreProcessor
import codecs
import os

api = Blueprint('api', __name__)

@api.route("/labeling/preprocess",methods=["POST"])
def preprocess():
    if request.method == 'POST':
        post_objects = json.loads(request.get_data(as_text=True))
        labeling_task = post_objects['labeling_task']
        paragraphs = post_objects['paragraphs']
        tokenization_dict = post_objects['tokenization_dict']
        pre_processor = PreProcessor(labeling_task=labeling_task, paragraphs=paragraphs, tokenization_dict=tokenization_dict)
        result = pre_processor.pre_processing()
        return json.dumps(result, ensure_ascii=False)
    else:
        return "Please use POST method!"

