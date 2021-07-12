# --*-- coding: utf-8 --*--
import os
from flask_cors import *

from app import create_app


if __name__ == '__main__':
    config_name = os.environ.get('MUSICKG_ENVIRONMENT') or 'dev'
    app = create_app(config_name=config_name)
    CORS(app, supports_credentials=True)
    app.run(host='0.0.0.0', port=8800)
