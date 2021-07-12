db.ScheduledJobTasks.update({ "_id": "de9e043d-de1f-4780-b4ae-01025eb89fe8" }, {
    "_id": "de9e043d-de1f-4780-b4ae-01025eb89fe8",
    "JobId": "3292baa1-8e67-493c-a06d-1e1e1f5b77b4",
    "Name": "800-Bootstrap",
    "Description": "Translate 800 data from raw database (mariadb) to master DB.",
    "IsRunOnce": true,
    "StartTime": ISODate("2021-04-01T16:00:00Z"),
    "EndTime": ISODate("2021-04-03T16:00:00Z"),
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
        "DataSourceName": "HOTLINE",
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