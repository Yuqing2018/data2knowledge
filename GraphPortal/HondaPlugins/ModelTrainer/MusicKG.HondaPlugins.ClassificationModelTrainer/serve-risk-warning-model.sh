#!/bin/bash
echo "This is the script to serve honda risk-warning model.";
echo "-------------------------------------------------------";
echo "Parameters:";
echo "Risk model serving location: $1";
echo "Risk model backup location: $2";
echo "Last model version: $3";
echo "New model version: $4";
echo "New model file location: $5";
echo "Risk model init url: $6"

echo "Start serving model...";

echo "Backup model for version $3...";
mv -b $1/risk_prediction_model.pkl $2/$3/risk_prediction_model.pkl
echo "Backup model for version $3 finished...";

echo "Copy new model for version $4...";
mv $5/risk_prediction_model.pkl $1/risk_prediction_model.pkl
echo "Copy new model for version $4 finished...";

echo "Initialize risk-warning service...";
curl -v -X POST $6;
echo "Initialize risk-warning service finished...";

echo "Finish serving model...";