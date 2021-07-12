db.ScheduledJobTasks.update({ "_id": "4e27e221-acf1-401b-9d5a-0315d3ed7da4" }, {
    "_id": "4e27e221-acf1-401b-9d5a-0315d3ed7da4",
    "JobId": "76def8ec-1a30-4890-ab72-c8d56f27dde8",
    "Name": "MQIToLabeling",
    "Description": "Translate MQI data from raw database (mariadb) to labeling tool.",
    "IsRunOnce": false,
    "StartTime": null,
    "EndTime": null,
    "Executor": {
        "UserName": "honda_manager",
        "Password": "honda123",
        "Domain": null
    },
    "MerchantCreationTime": ISODate("2021-03-07T16:00:00Z"),
    "ManagedAt": ISODate(),
    "ManagedBy": null,
    "IsDeleted": false,
    "TaskDefine": {
        "DataSourceName": "MQI",
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