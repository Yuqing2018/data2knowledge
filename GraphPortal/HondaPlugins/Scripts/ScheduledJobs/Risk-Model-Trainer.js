db.ScheduledJobs.update({ "_id": "6243bbb8-00e2-4d98-9cb2-cc014e7bfa00" }, {
    "_id": "6243bbb8-00e2-4d98-9cb2-cc014e7bfa00",
    "Name": "riskwarning-model-trainer",
    "JobType": "ModelTrainer",
    "Description": "Train risk warning model online.",
    "Schedule": null,
    "Actions": [{
        "ActionId": "TrainingDataCollection",
        "Description": "Collect training data from master db.",
        "IsDefault": true,
        "Options": {
            "ExecutorName": "RiskModelTrainingDataCollector",
            "Ignore": false
        }
    }, {
        "ActionId": "ModelTraining",
        "Description": "Train model.",
        "IsDefault": false,
        "Options": {
            "ExecutorName": "RiskModelTrainer",
            "Ignore": false
        }
    }, {
        "ActionId": "ModelServing",
        "Description": "Serve the trained model.",
        "IsDefault": false,
        "Options": {
            "ExecutorName": "RiskModelServer",
            "Ignore": false
        }
    }, {
        "ActionId": "TrainingHistoryUpdateing",
        "Description": "Update training history.",
        "IsDefault": false,
        "Options": {
            "ExecutorName": "HondaTrainingHistoryUpdater",
            "Ignore": false
        }
    }],
    "LastRunAt": null
}, { upsert: true });