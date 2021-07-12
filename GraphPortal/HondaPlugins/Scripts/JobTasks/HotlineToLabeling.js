db.ScheduledJobTasks.update({ "_id": "1964e109-67f9-4d97-accc-3e50db4cd862" }, {
        "_id": "1964e109-67f9-4d97-accc-3e50db4cd862",
        "JobId": "76def8ec-1a30-4890-ab72-c8d56f27dde8",
        "Name": "HotlineToLabeling",
        "Description": "Translate 800 data from raw database (mariadb) to labeling tool.",
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