db.ScheduledJobs.update({ "_id": "14f999fb-3378-46fe-81ec-19d510202ae9" }, {
    "_id": "14f999fb-3378-46fe-81ec-19d510202ae9",
    "Name": "Master-Reannotation",
    "JobType": "DataReannotation",
    "Description": "Reannotate data from master database.",
    "Schedule": "0 0 16 * * ?",
    "Actions": [{
        "ActionId": "DataCollection",
        "Description": "Collect vehicle fault data from master database.",
        "IsDefault": true,
        "Options": {
            "BatchSize": 500,
            "ExecutorName": "HondaVehicleFaultDataCollector",
            "Ignore": false
        }
    }, {
        "ActionId": "DataNormalization",
        "Description": "Normalize vehicle fault data to labeling tool format.",
        "IsDefault": false,
        "Options": {
            "ExecutorName": "HondaVehicleFaultDataNormalizer",
            "Ignore": false
        }
    }, {
        "ActionId": "DataAnnotation",
        "Description": "Reannotate vehicle fault data by ai model.",
        "IsDefault": false,
        "Options": {
            "BatchSize": 100,
            "ExecutorName": "HondaDataAnnotator",
            "Ignore": false
        }
    }, {
        "ActionId": "DataConsumption",
        "Description": "Save reannotation result back to master database.",
        "IsDefault": false,
        "Options": {
            "ExecutorName": "HondaVehicleFaultDataConsumer",
            "Ignore": false,
            "Consumers": 2
        }
    }],
    "LastRunAt": null
}, { upsert: true });