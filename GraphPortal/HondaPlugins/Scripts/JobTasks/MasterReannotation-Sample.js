db.ScheduledJobTasks.update({ "_id": "1a4f56fb-412d-4dde-a649-7aff73462116" }, {
    "_id": "1a4f56fb-412d-4dde-a649-7aff73462116",
    "JobId": "14f999fb-3378-46fe-81ec-19d510202ae9",
    "Name": "MasterReannotation-Sample",
    "Description": "Reannotate data from master database.",
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
        "DataSourceName": null,
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
