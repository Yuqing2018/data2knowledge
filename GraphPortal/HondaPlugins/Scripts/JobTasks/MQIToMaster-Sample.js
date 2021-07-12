db.ScheduledJobTasks.update({ "_id": "4a1f4b29-9ab4-4e29-8e21-4c5e98fcf92a" }, {
    "_id": "4a1f4b29-9ab4-4e29-8e21-4c5e98fcf92a",
    "JobId": "3292baa1-8e67-493c-a06d-1e1e1f5b77b4",
    "Name": "MQIToMaster-Bootstrap",
    "Description": "Translate MQI data from raw database (mariadb) to master database when system warm up.",
    "IsRunOnce": true,
    "StartTime": ISODate("2021-03-14T16:00:00Z"),
    "EndTime": ISODate(),
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
        "DataSourceName": "MQI",
        "WorkspaceId": null,
        "DBSettings": null
    },
    "Status": {
        "LastStatus": "准备中",
        "LastRunAt": null,
        "LastFinishedAt": null,
        "LastSucceedAt": null,
        "TryTimes": 0
    }
}, { upsert: true });
