from flask import Flask
#from gevent import monkey
#from gevent.wsgi import WSGIServer
from flask_cors import CORS
from waitress import serve
import logging
import os
from api_router import api
#monkey.patch_all()
app = Flask(__name__)

app.config['JSON_AS_ASCII'] = False
app_log_file = os.getcwd() + os.sep + 'app.log'
logging.basicConfig(level=logging.INFO)

#app.register_blueprint(api, url_prefix='/MusicPreProcess')
app.register_blueprint(api)
CORS(app)

if __name__ == "__main__":

    #WSGIServer(('0.0.0.0', 35002), app).serve_forever()
    #app.run('0.0.0.0', port = 35002)
    serve(app,host='0.0.0.0',port=35002)