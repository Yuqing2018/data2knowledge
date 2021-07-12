import os
import zipfile
import requests
import json
from app.confmanager.handler import MongoOperator


def make_zip(source_dir=None, filenames=None, output_filename=None, out_dir=None):
    zipf = zipfile.ZipFile(out_dir + os.sep + output_filename, 'w')
    # zipf = zipfile.ZipFile(output_filename, 'w')
    zipf_dir = out_dir + os.sep + output_filename
    for filename in filenames:
        pathfile = os.path.join(source_dir, filename)
        if os.path.exists(pathfile):
            zipf.write(pathfile, arcname=filename)
    zipf.close()
    return zipf_dir


def login_music_kg(url_login=None, login_data=None):
    headers = {
        'Content-Type': 'application/problem+json; charset=utf-8',
        'accept': 'application/json'
    }
    login_res = requests.post(url_login, data=json.dumps(login_data), headers=headers)
    content_str = login_res.content.decode()
    token = json.loads(content_str).get("token")
    return token

