db.ScheduledJobs.update({ "_id": "07270eb6-aeaa-490d-82b2-b565ae6c80e4" }, {
    "_id": "07270eb6-aeaa-490d-82b2-b565ae6c80e4",
    "Name": "RawMongoToLabeling-Realtime",
    "JobType": "DataTranslateToLabelingTool",
    "Description": "Translate raw data from raw database (mongodb) to labeling tool.",
    "Schedule": null,
    "Actions": [{
        "ActionId": "DataCollection",
        "Description": "Collect data from raw database (mongo).",
        "IsDefault": true,
        "Options": {
            "BatchSize": 500,
            "ExecutorName": "DefaultMongoDataCollector",
            "Ignore": false
        }
    }, {
        "ActionId": "DataNormalization",
        "Description": "Normalize raw data to labeling tool format.",
        "IsDefault": false,
        "Options": {
            "ExecutorName": "HondaDataNormalizer",
            "Ignore": false
        }
    }, {
        "ActionId": "DataAnnotation",
        "Description": "Pre-annotate raw data by ai model.",
        "IsDefault": false,
        "Options": {
            "BatchSize": 100,
            "ExecutorName": "HondaDataAnnotator",
            "Ignore": false
        }
    }, {
        "ActionId": "DataConsumption",
        "Description": "Consume data by labeling tool.",
        "IsDefault": false,
        "Options": {
            "ExecutorName": "HondaLabelingToolDataConsumer",
            "Ignore": false,
            "Consumers": 0
        }
    }, {
        "ActionId": "TaskCreation",
        "Description": "Create task for translated data.",
        "IsDefault": false,
        "Options": {
            "ExecutorName": "DefaultTaskCreator",
            "Ignore": false
        }
    }],
    "LastRunAt": null
}, { upsert: true });