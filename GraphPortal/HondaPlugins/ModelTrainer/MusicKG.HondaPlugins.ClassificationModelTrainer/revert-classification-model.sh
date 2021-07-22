#!/bin/bash
echo "This is the script to revert honda classification model to version $1.";
echo "-------------------------------------------------------";
echo "Parameters:";
echo "Classification model serving location: $1";
echo "Classification model backup location: $2";
echo "Model version: $3";

echo "Reverting model to version $3...";
mv -f $2/all_part_model/$3 $1/all_part_model/1
mv -f $2/all_syndrome_model/$3 $1/all_syndrome_model/1
echo "Reverting model to version $3 finished...";