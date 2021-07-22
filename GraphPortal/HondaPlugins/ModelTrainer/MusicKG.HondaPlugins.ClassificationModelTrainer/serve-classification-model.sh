#!/bin/bash
echo "This is the script to serve honda classification model.";
echo "-------------------------------------------------------";
echo "Parameters:";
echo "Classification model serving location: $1";
echo "Classification model backup location: $2";
echo "Last model version: $3";
echo "New model version: $4";
echo "New model file location: $5";

echo "Start serving model...";

echo "Backup model for version $3...";
mv -b $1/all_part_model/1 $2/all_part_model/$3
mv -b $1/all_syndrome_model/1 $2/all_syndrome_model/$3
echo "Backup model for version $3 finished...";

echo "Copy new model for version $4...";
mv $5/all_part_model $1/all_part_model/1
mv $5/all_syndrome_model $1/all_syndrome_model/1
echo "Copy new model for version $4 finished...";

echo "Finish serving model...";