echo "Pulling images for version $1"
docker pull labeltool.eastasia.cloudapp.azure.com:5443/musickg.webapi:$1
docker pull labeltool.eastasia.cloudapp.azure.com:5443/honda.webapi:$1
docker pull labeltool.eastasia.cloudapp.azure.com:5443/honda.modeltrainer:$1
docker pull labeltool.eastasia.cloudapp.azure.com:5443/honda.offlineworker:$1
docker pull labeltool.eastasia.cloudapp.azure.com:5443/musickg.webui:$1
docker pull labeltool.eastasia.cloudapp.azure.com:5443/honda-risk-prediction:15340
docker pull tensorflow/serving:2.3.0
docker pull mongo
mkdir -p /honda_images/$1/
echo "Saving image for version $1"
echo "Saving image for labeling webapi."
docker save -o /honda_images/$1/musickg.webapi.$1.tar labeltool.eastasia.cloudapp.azure.com:5443/musickg.webapi:$1
echo "Saving image for business webapi."
docker save -o /honda_images/$1/honda.webapi.$1.tar labeltool.eastasia.cloudapp.azure.com:5443/honda.webapi:$1
echo "Saving image for model trainer."
docker save -o /honda_images/$1/honda.modeltrainer.$1.tar labeltool.eastasia.cloudapp.azure.com:5443/honda.modeltrainer:$1
echo "Saving image for offline worker."
docker save -o  /honda_images/$1/honda.offlineworker.$1.tar labeltool.eastasia.cloudapp.azure.com:5443/honda.offlineworker:$1
echo "Saving image for ui."
docker save -o  /honda_images/$1/musickg.webui.$1.tar labeltool.eastasia.cloudapp.azure.com:5443/musickg.webui:$1
if [ -f /honda_images/tensorflow.serving.2.3.0.tar ]
then
  echo "Tensorflow serving already exists."
else
  echo "Saving image for tensorflow serving."
  docker save -o /honda_images/tensorflow.serving.2.3.0.tar tensorflow/serving:2.3.0
fi
if [ -f /honda_images/mongodb.tar ]
then
  echo "Mongodb already exists."
else
  echo "Saving image for mongodb."
  docker save -o /honda_images/mongodb.tar mongo
fi
if [ -f /honda_images/riskpredictor.tar]
then
  echo "Riskpredictor already exists. "
else
  echo "Saving image for riskpredictor."
  docker save -o /honda_images/riskpredictor.tar labeltool.eastasia.cloudapp.azure.com:5443/honda-risk-prediction:15340

echo "Zipping image files."
zip -r /honda_images/$1.zip /honda_images/$1