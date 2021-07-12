#!/bin/bash
echo "This is the script to serve honda classification model.";
echo "-------------------------------------------------------";
echo "Parameters:";
echo "Classification model serving location: $1";
echo "New model file location: $2";
echo "New model version: $3";

echo "Start serving model...";

echo "Updating symbol link for classification model...";
cp -r $2/all_part_model $1/all_part_model/$3
cp -r $2/all_syndrome_model $1/all_syndrome_model/$3
echo "synmol link for classification model is updated.";

echo "Updating model config..."

sed -i "s/versions:.*$/versions: $3/" $1/all_models.config

#echo "Model config updated..."

echo "Finish serving model...";
