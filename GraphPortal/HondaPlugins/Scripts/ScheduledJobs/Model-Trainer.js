db.ScheduledJobs.update({ "_id": "3dece151-c25d-4d0e-b85c-5cd01d3ca05c" }, {
    "_id": "3dece151-c25d-4d0e-b85c-5cd01d3ca05c",
    "Name": "classification-model-trainer",
    "JobType": "ModelTrainer",
    "Description": "Train classification model online.",
    "Schedule": null,
    "Actions": [{
        "ActionId": "TrainingDataCollection",
        "Description": "Collect training data from master db.",
        "IsDefault": true,
        "Options": {
            "ExecutorName": "ClassificationModelTrainingDataCollector",
            "Ignore": false
        }
    }, {
        "ActionId": "ModelTraining",
        "Description": "Train model.",
        "IsDefault": false,
        "Options": {
            "ExecutorName": "ClassificationModelTrainer",
            "Ignore": false
        }
    }, {
        "ActionId": "ModelServing",
        "Description": "Serve the trained model.",
        "IsDefault": false,
        "Options": {
            "ExecutorName": "ClassificationModelServer",
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