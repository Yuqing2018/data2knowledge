db.ScheduledJobs.update({ "_id": "3292baa1-8e67-493c-a06d-1e1e1f5b77b4" }, {
    "_id": "3292baa1-8e67-493c-a06d-1e1e1f5b77b4",
    "Name": "RawMariaToMaster-Bootstrap",
    "JobType": "DataTranslateToMasterDB",
    "Description": "Translate MQI data from raw database (mariadb) to master database when system warm up.",
    "Schedule": "0 0 16 * * ?",
    "Actions": [{
        "ActionId": "DataCollection",
        "Description": "Collect data from raw database (mariadb).",
        "IsDefault": true,
        "Options": {
            "BatchSize": 200,
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
            "Ignore": false
        }
    }, {
        "ActionId": "DataConsumption",
        "Description": "Consume data by business.",
        "IsDefault": false,
        "Options": {
            "ExecutorName": "HondaBusinessDataConsumer",
            "Ignore": false,
            "Consumers": 2
        }
    }],
    "LastRunAt": null
}, { upsert: true });