#!/bin/bash
echo "This is the script to serve honda risk-warning model.";
echo "-------------------------------------------------------";
echo "Parameters:";
echo "Risk model serving location: $1";
echo "New model file location: $2";
echo "Risk model init url: $3"

echo "Start serving model...";

echo "Updating symbol link for risk-warning model...";
rm -f $1/risk_prediction_model.pkl
cp $2/risk_prediction_model.pkl $1/risk_prediction_model.pkl
echo "synmol link for risk-warning model is updated.";

echo "Initialize risk-warning service...";
curl -v -X POST $3;
echo "Initialize risk-warning service finished...";

echo "Finish serving model...";
