db.ScheduledJobs.update({ "_id": "2a8b8a89-8e2f-4895-b5df-25379b24696d" }, {
    "_id": "2a8b8a89-8e2f-4895-b5df-25379b24696d",
    "Name": "Warning-Calculator",
    "JobType": "WarningCalculator",
    "Description": "Calculate warning task.",
    "Schedule": "0 0 16 * * ?",
    "Actions": [{
        "ActionId": "CollectVehicleFaultDataAction",
        "Description": "Collect vehicle fault data from master db.",
        "IsDefault": true,
        "Options": {
            "ExecutorName": "VehicleFaultDataCollector",
            "Ignore": false
        }
    }, {
        "ActionId": "LinkRelatedDataAction",
        "Description": "Link related data to vehicle fault data, like QIC/QIS.",
        "IsDefault": false,
        "Options": {
            "ExecutorName": "RelatedDataLinker",
            "Ignore": false
        }
    }, {
        "ActionId": "CalculateWarningAction",
        "Description": "Calculate warning from vehicle fault data and related data.",
        "IsDefault": false,
        "Options": {
            "ExecutorName": "WarningRecordCalculator",
            "Ignore": false
        }
    }, {
        "ActionId": "SaveCalculationResultAction",
        "Description": "Save warning records to master database.",
        "IsDefault": false,
        "Options": {
            "ExecutorName": "CalculationResultSaver",
            "Ignore": false
        }
    }, {
        "ActionId": "AnnounceWarningAction",
        "Description": "Send warning emails for this task.",
        "IsDefault": false,
        "Options": {
            "ExecutorName": "WarningAnnouncer",
            "Ignore": false
        }
    }],
    "LastRunAt": null
}, { upsert: true });