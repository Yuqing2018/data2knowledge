#!/bin/bash
echo "This is the script to revert honda classification model to version $3.";
echo "-------------------------------------------------------";
echo "Parameters:";
echo "Classification model serving location: $1";
echo "Model version: $2";

echo "Reverting model to version $2...";

sed -i "s/versions:.*$/versions: $2/" $1/all_models.config

echo "Reverting model to version $2 finished...";
