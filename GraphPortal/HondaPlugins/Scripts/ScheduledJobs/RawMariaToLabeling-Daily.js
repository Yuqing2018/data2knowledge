db.ScheduledJobs.update({ "_id": "76def8ec-1a30-4890-ab72-c8d56f27dde8" }, {
    "_id": "76def8ec-1a30-4890-ab72-c8d56f27dde8",
    "Name": "RawMariaToLabeling-Daily",
    "JobType": "DataTranslateToLabelingTool",
    "Description": "Translate raw data from raw mariadb to labeling tool.",
    "Schedule": "0 0 16 * * ?",
    "Actions": [{
        "ActionId": "DataCollection",
        "Description": "Collect data from raw database (mariadb).",
        "IsDefault": true,
        "Options": {
            "BatchSize": 500,
            "ExecutorName": "DefaultMySqlDataCollector",
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
            "Ignore": true
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