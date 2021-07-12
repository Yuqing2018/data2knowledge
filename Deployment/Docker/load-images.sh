echo "Loading images for version $1 from folder $2."

if [ -f $2/tensorflow.serving.2.3.0.tar ]
then
  echo "Loading image for tensorflow serving."
  docker load -i $2/tensorflow.serving.2.3.0.tar -q
else
  echo "Tensorflow serving image does not exist."
fi

if [ -f $2/mongodb.tar ]
then
  echo "Loading image for mongodb."
  docker load -i $2/mongodb.tar -q
else
  echo "Mongodb image does not exist."
fi

if [ -f $2/$1.tar.gz ]
then
  echo "Unzipping image files."
  tar zxvf $2/$1.tar.gz
else
  echo "There is no image file to be updated."
fi

echo "loading image for labeling webapi."
docker load -i honda_images/$1/musickg.webapi.$1.tar -q

echo "loading image for business webapi."
docker load -i honda_images/$1/honda.webapi.$1.tar -q

echo "loading image for model trainer."
docker load -i honda_images/$1/honda.modeltrainer.$1.tar -q

echo "loading image for offline worker."
docker load -i honda_images/$1/honda.offlineworker.$1.tar -q

echo "loading image for ui."
docker load -i honda_images/$1/musickg.webui.$1.tar -q
