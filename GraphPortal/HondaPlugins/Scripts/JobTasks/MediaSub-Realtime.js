db.ScheduledJobTasks.update({ "_id": "fab33e76-ab60-4f67-8eee-44112c4e69a6" }, {
    "_id": "fab33e76-ab60-4f67-8eee-44112c4e69a6",
    "JobId": "07270eb6-aeaa-490d-82b2-b565ae6c80e4",
    "Name": "MediaSub-Realtime",
    "Description": "Translate Media Sub data from mongo db to labeling tool.",
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
        "DataSourceName": "MEDIA_SUB",
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