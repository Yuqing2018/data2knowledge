db.ScheduledJobTasks.update({ "_id": "1afa267a-d22a-4841-899d-ae35ef8d399c" }, {
        "_id": "1afa267a-d22a-4841-899d-ae35ef8d399c",
        "JobId": "76def8ec-1a30-4890-ab72-c8d56f27dde8",
        "Name": "TechSupportToLabeling",
        "Description": "Translate Tech support data from raw database (mariadb) to labeling tool.",
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
            "DataSourceName": "TECH_CONSULTING",
            "WorkspaceId": "5fb3660530b3090001e0c6d8",
            "DBSettings": {
                "ConnectionString": "Server=10.110.101.52;Port=3306;User ID=mqpaiopr;Password=mqpaighac;Database=mqpai_db",
                "Database": "",
                "TableName": "rawtechsupportdata",
                "TimestampFieldName": "Timestamp"
            },
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