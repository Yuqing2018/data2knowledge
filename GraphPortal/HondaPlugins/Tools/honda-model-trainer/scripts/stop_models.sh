#!/bin/bash
echo "Stopping classification model using gpu..."

docker rm -f classification-gpu

echo "Classification model using gpu stopped..."

echo "Stopping risk warning model..."

docker rm -f risk-warning

echo "Risk warning model stopped..."
