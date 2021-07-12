db.ScheduledJobTasks.update({ "_id": "1bcd98c4-3711-4de8-818e-63593cf37fe6" }, {
    "_id": "1bcd98c4-3711-4de8-818e-63593cf37fe6",
    "JobId": "3292baa1-8e67-493c-a06d-1e1e1f5b77b4",
    "Name": "TechSupport-Bootstrap",
    "Description": "Translate Tech support data from raw database (mariadb) to master DB.",
    "IsRunOnce": true,
    "StartTime": ISODate("2021-05-05T16:00:00Z"),
    "EndTime": ISODate("2021-05-06T16:00:00Z"),
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
        "DataSourceName": "TECH_CONSULTING",
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