db.ScheduledJobs.update({ "_id": "51682d04-bbcc-4c35-84d6-375d56f6fa5e" }, {
    "_id": "51682d04-bbcc-4c35-84d6-375d56f6fa5e",
    "Name": "Result-Handler",
    "JobType": "DataHandling",
    "Description": "Handle labeling result.",
    "Schedule": null,
    "Actions": [{
        "ActionId": "DocumentCollection",
        "Description": "Collect result documents for finished tasks from labeling tool",
        "IsDefault": true,
        "Options": {
            "ExecutorName": "HondaDocumentCollector",
            "Ignore": false
        }
    }, {
        "ActionId": "DataHandling",
        "Description": "Save labeling result data to master db and model training db.",
        "IsDefault": false,
        "Options": {
            "ExecutorName": "HondaDataHandler",
            "Ignore": false,
            "DataConsumers": [2, 1]
        }
    }, {
        "ActionId": "TaskStatusUpdating",
        "Description": "Update task status to knowledge merged.",
        "IsDefault": false,
        "Options": {
            "ExecutorName": "DefaultTaskStatusUpdater",
            "Ignore": false
        }
    }],
    "LastRunAt": null
}, { upsert: true });