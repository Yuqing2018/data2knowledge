import pandas as pd
import xgboost as xgb
import pickle
import os
import numpy as np
from sklearn.model_selection import KFold, train_test_split, GridSearchCV
from sklearn.metrics import confusion_matrix, mean_squared_error, accuracy_score
from sklearn.datasets import load_iris, load_digits, load_boston
from sklearn.preprocessing import OneHotEncoder,LabelEncoder
import argparse
parser = argparse.ArgumentParser()

#parser.add_argument('--checkpoint_path', type=str, help='Path to the pretrained model',default='./models/pretrained_lm/chinese_L-12_H-768_A-12/')
parser.add_argument('--train_file_path',type=str, help='train file path',default='./data/风险台账processed.xlsx')
parser.add_argument('--test_set_size',type=float,help='test set size',default=0.1)
parser.add_argument('--model_save_path',type=str,help='risk model weights save path',default='./model/risk_prediction_model.pkl')
args = parser.parse_args()

rng = np.random.RandomState(31337)

#df = pd.read_excel('data/风险台账processed.xls')
df = pd.read_excel(args.train_file_path,engine='openpyxl')
df = df.drop(columns=['风险特征-超标影响', '信息系初判等级'])

label_column = '信息系初判等级new'
feature_columns = df.columns.to_list()
feature_columns.remove(label_column)
print(f'all features: {feature_columns}')
print(f'label column: {label_column}')
X = df[feature_columns].to_numpy()
y = df[label_column].to_numpy()

print(f'label values: {y}')

X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=args.test_set_size, random_state=42)
xgb_model = xgb.XGBClassifier(n_jobs=1, use_label_encoder=False).fit(X_train, y_train)
y_test_hat = xgb_model.predict(X_test)
print(accuracy_score(y_test,y_test_hat))
pickle.dump(xgb_model, open(args.model_save_path, 'wb'))
