from flask import Flask
from flask_cors import CORS
import logging
import os
from api_router import api
from config import FLAGS

app = Flask(__name__)

app.config['JSON_AS_ASCII'] = False
app_log_file = os.getcwd() + os.sep + 'app.log'
logging.basicConfig(level=logging.INFO)

#app.register_blueprint(api, url_prefix='/MusicPreProcess')
app.register_blueprint(api)
CORS(app)

if __name__ == "__main__":
    app.run('0.0.0.0', port = FLAGS.port)