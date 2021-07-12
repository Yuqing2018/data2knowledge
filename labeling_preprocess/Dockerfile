FROM python:3

EXPOSE 35002

WORKDIR /MusicKG.labeling_preprocess/

COPY . /MusicKG.labeling_preprocess/

RUN pip install -r requirements.txt

CMD [ "python", "./api_server.py" ]