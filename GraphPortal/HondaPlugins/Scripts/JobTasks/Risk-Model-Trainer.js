db.ScheduledJobTasks.update({ "_id": "7b7e19b1-1052-49aa-aab4-8f146b68287c" }, {
    "_id": "7b7e19b1-1052-49aa-aab4-8f146b68287c",
    "JobId": "6243bbb8-00e2-4d98-9cb2-cc014e7bfa00",
    "Name": "Risk model trainer.",
    "Description": "Train and serve model online.",
    "IsRunOnce": true,
    "StartTime": ISODate(),
    "EndTime": ISODate(),
    "Executor": {},
    "MerchantCreationTime": ISODate(),
    "ManagedAt": ISODate(),
    "ManagedBy": "honda_manager",
    "IsDeleted": false,
    "TaskDefine": {
        "TrainAt": ISODate("2021-03-19T02:10:28.769Z")
    },
    "Status": {
        "LastStatus": "准备中",
        "LastRunAt": null,
        "LastFinishedAt": null,
        "LastSucceedAt": null,
        "TryTimes": 0
    }
}, { upsert: true });
