#!/bin/bash
echo "This is the script to revert honda risk-warning model to version $1.";
echo "-------------------------------------------------------";
echo "Parameters:";
echo "Risk-warning model serving location: $1";
echo "Risk-warning trained model location: $2";
echo "Model version: $3";
echo "Risk model init url: $4";

echo "Reverting model to version $3...";
rm -f $1/risk_prediction_model.pkl
cp $2/$3/risk_prediction_model.pkl $1/risk_prediction_model.pkl
echo "Reverting model to version $3 finished...";

echo "Initialize risk-warning service...";
curl -v -X POST $4;
echo "Initialize risk-warning service finished...";

echo "Reverting model to version $3 finished...";
