db.Honda_AIRiskWarningMapping.update({ "_id": ObjectId("60a1d56d9e45841319ac35aa") }, {
    "_id": ObjectId("60a1d56d9e45841319ac35aa"),
    "RiskLevel": "低风险",
    "RiskLevelInModel": 0
}, { upsert: true });

db.Honda_AIRiskWarningMapping.update({ "_id": ObjectId("60a1d65f9e45841319ac35ab") }, {
    "_id": ObjectId("60a1d65f9e45841319ac35ab"),
    "RiskLevel": "中风险",
    "RiskLevelInModel": 1
}, { upsert: true });

db.Honda_AIRiskWarningMapping.update({ "_id": ObjectId("60a1d67a9e45841319ac35ac") }, {
    "_id": ObjectId("60a1d67a9e45841319ac35ac"),
    "RiskLevel": "高风险",
    "RiskLevelInModel": 2
}, { upsert: true });