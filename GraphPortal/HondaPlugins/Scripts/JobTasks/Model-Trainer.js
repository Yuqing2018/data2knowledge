db.ScheduledJobTasks.update({ "_id": "93c3d55e-2c25-45e2-8eb1-25fce053daec" }, {
    "_id": "93c3d55e-2c25-45e2-8eb1-25fce053daec",
    "JobId": "3dece151-c25d-4d0e-b85c-5cd01d3ca05c",
    "Name": "Classification model trainer.",
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
