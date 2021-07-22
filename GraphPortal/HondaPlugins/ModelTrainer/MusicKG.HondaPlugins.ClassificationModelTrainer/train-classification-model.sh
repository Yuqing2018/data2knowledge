#!/bin/bash
echo "This is the script to train honda classification model.";
echo "-------------------------------------------------------";
echo "Parameters:";
echo "Part name training data file: $1";
echo "Part name validation data file: $2";
echo "Part name test data file: $3";
echo "Part name list file: $4";
echo "Syndrome training data file: $5";
echo "Syndrome validation data file: $6";
echo "Syndrome test data file: $7";
echo "Syndrome list file: $8";
echo "Syndrome mapping file: $9"
echo "model output folder: ${10}"

echo "Start training model...";

source ~/miniconda3/etc/profile.d/conda.sh;

conda activate modeltraining;

python /root/model-training/honda/train.py --part_train_path $1 --part_dev_path $2 --part_test_path $3 --part_label_path $4 --part_model_save_path $10 --syndrome_train_path $5 --syndrome_dev_path $6 --syndrome_test_path $7 --syndrome_label_path $8 --all_syndrome_path $9 --syndrome_model_save_path ${10}/syndrome --part_model_save_path ${10}/part --train_part_flag True --train_syndrome_flag True

sleep 1m;

echo "Finish training model...";
