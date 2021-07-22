#!/bin/bash
echo "This is the script to train honda risk warning model.";
echo "-------------------------------------------------------";
echo "Parameters:";
echo "Training data file: $1";
echo "Risk warning model output folder: $2"

echo "Start training model...";

source ~/miniconda3/etc/profile.d/conda.sh;

conda activate modeltraining;

python /root/model-training/risk-warning/train.py --train_file_path $1 --model_save_path $2;

echo "Finish training model...";