db.ScheduledJobTasks.update({ "_id": "aa82a260-3117-4bb6-b0b1-5eff5198a338" }, {
    "_id": "aa82a260-3117-4bb6-b0b1-5eff5198a338",
    "JobId": "07270eb6-aeaa-490d-82b2-b565ae6c80e4",
    "Name": "GOV-Realtime",
    "Description": "Translate GOV data from mongo db to labeling tool.",
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
        "DataSourceName": "GOV",
        "WorkspaceId": "5fb3660530b3090001e0c6d8",
        "DBSettings": null,
        "DataConsumerSettings": {
            "TagFeatures": [],
            "DateTimeFormat": "yyyyMMddHHmmss",
            "ContentType": "application/json",
            "ItemsPerDocument": 50
        },
        "DataAnnotatorSettings": {
            "AnnotationServiceEndpoint": "NotUsed",
            "ModelVersion": "",
            "ItemsCountPerRequest": 20
        }
    },
    "Status": {
        "LastStatus": "准备中",
        "LastRunAt": null,
        "LastFinishedAt": null,
        "LastSucceedAt": null,
        "TryTimes": 0
    }
}, { upsert: true });