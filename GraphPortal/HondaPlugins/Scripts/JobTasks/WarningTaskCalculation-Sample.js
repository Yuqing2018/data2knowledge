db.ScheduledJobTasks.update({ "_id": "2d60a77b-6cc4-4255-857b-57290ba9abc9" }, {
    "_id": "2d60a77b-6cc4-4255-857b-57290ba9abc9",
    "JobId": "2a8b8a89-8e2f-4895-b5df-25379b24696d",
    "Name": "WarningTaskCalculation-Sample",
    "Description": "Reannotate data from master database.",
    "IsRunOnce": false,
    "StartTime": null,
    "EndTime": null,
    "Executor": {
        "UserName": "honda_manager",
        "Password": "honda123",
        "Domain": null
    },
    "MerchantCreationTime": ISODate(),
    "ManagedAt": ISODate(),
    "ManagedBy": null,
    "IsDeleted": false,
    "TaskDefine": {
        "_id": ObjectId("603775e9e193990001f11b61"),
        "Name": "Test",
        "WarningUnit": "零件_不良症状",
        "WarningStatus": "预警中",
        "CarModels": ["ACC", "ACCH"],
        "CarTypes": null,
        "YearModels": ["2017", "2018", "2019", "2020", "2021"],
        "WarningIndex": [{
            "_id": ObjectId("5fe19d6a6dd820000181014e"),
            "WarningType": "多发预警",
            "IndexName": "近三个月发生件数",
            "Value": "4",
            "Unit": null
        }],
        "CreateTime": ISODate(),
        "LastModifyTime": ISODate(),
        "CreateBy": "honda_user1"
    },
    "Status": {
        "LastStatus": "准备中",
        "LastRunAt": null,
        "LastFinishedAt": null,
        "LastSucceedAt": null,
        "TryTimes": 0
    }
}, { upsert: true });
