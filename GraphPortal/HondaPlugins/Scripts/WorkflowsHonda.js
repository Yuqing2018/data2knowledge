db.Workflows.update({ "_id": ObjectId("5faba9fe2f2fb1c4d65e3cc4") }, {
    "_id": ObjectId("5faba9fe2f2fb1c4d65e3cc4"),
    "Name": "VehicleFailureClassification",
    "Steps": [
        {
            "StepId": ObjectId("5fabaa8d2f2fb1c4d65e3cc5"),
            "Name": "Uploading",
            "ProcessorAssembly": "",
            "ProcessorClass": "",
            "ResultDocumentStatus": 3,
            "AutoDoNext": false
        },
        {
            "StepId": ObjectId("5fb4a9d7c2da9ecfd6ce7b21"),
            "Name": "Annotation",
            "ProcessorAssembly": "",
            "ProcessorClass": "",
            "ResultDocumentStatus": 1,
            "AutoDoNext": false
        }
    ]
}, { upsert: true });