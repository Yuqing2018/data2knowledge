FROM ubuntu

EXPOSE 5002

WORKDIR /MusicKG.KEPreProcessor/

CMD ["python3", "api_server.py"]

RUN apt update \
  && apt install -y python3.6 python3-pip \
  && apt clean

COPY . /MusicKG.KEPreProcessor/

RUN pip3 install -r requirements.txt