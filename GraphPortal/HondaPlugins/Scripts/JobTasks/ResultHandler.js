db.ScheduledJobTasks.update({ "_id": "44ad3e66-fcba-41c1-97b7-f36ca8424d52" }, {
    "_id": "44ad3e66-fcba-41c1-97b7-f36ca8424d52",
    "JobId": "51682d04-bbcc-4c35-84d6-375d56f6fa5e",
    "Name": "ResultHandler",
    "Description": "Handle labeling result.",
    "IsRunOnce": false,
    "StartTime": null,
    "EndTime": null,
    "Executor": {
        "UserName": "honda_manager",
        "Password": "honda123",
        "Domain": null
    },
    "MerchantCreationTime": ISODate("2021-03-08T03:54:48.487Z"),
    "ManagedAt": ISODate("2021-03-08T03:57:58.466Z"),
    "ManagedBy": null,
    "IsDeleted": false,
    "TaskDefine": {
        "WorkspaceId": "5fb3660530b3090001e0c6d8",
        "UpdatedTaskStatus": "KnowledgeMerged"
    },
    "Status": {
        "LastStatus": "准备中",
        "LastRunAt": null,
        "LastFinishedAt": null,
        "LastSucceedAt": null,
        "TryTimes": 0
    }
}, { upsert: true });