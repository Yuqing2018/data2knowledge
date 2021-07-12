FROM ubuntu

EXPOSE 8800

WORKDIR /MusicKG.GraphDataIngestion/

CMD ["python3", "start.py"]

RUN apt update \
  && apt install -y wget gnupg2 \
  && echo 'deb [arch=amd64] http://dl.google.com/linux/chrome/deb/ stable main' > /etc/apt/sources.list.d/google-chrome.list \
  && wget -qO - https://dl.google.com/linux/linux_signing_key.pub | apt-key add - \
  && apt update \
  && apt install -y python3.6 python3-pip xvfb google-chrome-stable \
  && apt clean 

COPY . /MusicKG.GraphDataIngestion/

RUN pip3 install -r requirements.txt

RUN chmod a+x /MusicKG.GraphDataIngestion/app/drivers/posix/chromedriver