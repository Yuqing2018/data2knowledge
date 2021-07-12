db.ScheduledJobTasks.update({ "_id": "4cf9609f-31b5-4878-b905-776ad1029755" }, {
    "_id": "4cf9609f-31b5-4878-b905-776ad1029755",
    "JobId": "3292baa1-8e67-493c-a06d-1e1e1f5b77b4",
    "Name": "MQI-Bootstrap",
    "Description": "Translate MQI data from raw database (mariadb) to master DB.",
    "IsRunOnce": true,
    "StartTime": ISODate("2021-04-06T16:00:00Z"),
    "EndTime": ISODate("2021-04-19T16:00:00Z"),
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