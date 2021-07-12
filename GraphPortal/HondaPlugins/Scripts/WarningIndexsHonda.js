db.Honda_Warning_WarningIndex.update({ "_id": ObjectId("5fe19d6a6dd820000181014d") }, {
    "_id": ObjectId("5fe19d6a6dd820000181014d"),
    "WarningType": "多发预警",
    "IndexName": "近三个月相对上升率",
    "Value": "200",
    "Unit": "%"
}, { upsert: true });
db.Honda_Warning_WarningIndex.update({ "_id": ObjectId("5fe19d6a6dd820000181014e") }, {
    "_id": ObjectId("5fe19d6a6dd820000181014e"),
    "WarningType": "多发预警",
    "IndexName": "近三个月发生件数",
    "Value": "4"
}, { upsert: true });
db.Honda_Warning_WarningIndex.update({ "_id": ObjectId("5fe19d6a6dd820000181014f") }, {
    "_id": ObjectId("5fe19d6a6dd820000181014f"),
    "WarningType": "多发预警",
    "IndexName": "年款不良率",
    "Value": "2019",
    "Unit": "%"
}, { upsert: true });
db.Honda_Warning_WarningIndex.update({ "_id": ObjectId("5fe19d6a6dd8200001810150") }, {
    "_id": ObjectId("5fe19d6a6dd8200001810150"),
    "WarningType": "风险预警",
    "IndexName": "风险等级",
    "Value": "重点关注,高风险,潜在高风险,中风险,其他关注,低风险,一般"
}, { upsert: true });
db.Honda_Warning_WarningIndex.update({ "_id": ObjectId("5fe19d6a6dd8200001810151") }, {
    "_id": ObjectId("5fe19d6a6dd8200001810151"),
    "WarningType": "再发预警",
    "IndexName": "对策后生产车辆的MQI件数",
    "Value": "4"
}, { upsert: true });
db.Honda_Warning_WarningIndex.update({ "_id": ObjectId("5fe19d6a6dd8200001810152") }, {
    "_id": ObjectId("5fe19d6a6dd8200001810152"),
    "WarningType": "再发预警",
    "IndexName": "对策后再发不良率",
    "Value": "对策前不良率",
    "Unit": "%"
}, { upsert: true });