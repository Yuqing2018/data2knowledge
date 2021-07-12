#!/bin/bash
echo "Starting classification model using gpu..."

docker run --runtime=nvidia --restart always -d -p 48500:8500 -p 48501:8501 -v /models-test/classification:/all_models --name classification-gpu -t tensorflow/serving:latest-gpu --model_config_file=/all_models/all_models.config --model_config_file_poll_wait_seconds=60 --file_system_poll_wait_seconds=60

echo "Classification model using gpu started..."

echo "Starting risk warning model..."

docker run --restart=always -d -p 45000:5000 -v /models-test/risk-warning:/app/risk_prediction/model_file --name risk-warning -t labeltool.eastasia.cloudapp.azure.com:5443/honda-risk-prediction:15883
