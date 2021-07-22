FROM python:3.7

RUN apt-get install -y gpg && \
  wget -O - https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor -o microsoft.asc.gpg && \
  mv microsoft.asc.gpg /etc/apt/trusted.gpg.d/ && \
  wget https://packages.microsoft.com/config/ubuntu/18.04/prod.list && \
  mv prod.list /etc/apt/sources.list.d/microsoft-prod.list && \
  chown root:root /etc/apt/trusted.gpg.d/microsoft.asc.gpg && \
  chown root:root /etc/apt/sources.list.d/microsoft-prod.list && \
  apt-get update && \
  apt-get install -y apt-transport-https && \
  apt-get update && \
  apt-get install -y dotnet-runtime-5.0

EXPOSE 80

COPY bin/Release/net5.0/publish/ /app/

WORKDIR /app

RUN pip install -r /app/Scripts/requirements.txt

CMD ["dotnet", "MusicKG.DataManager.ModelTrainingWorker.dll"]