db.Workflows.update({ "_id": ObjectId("5c4a759820b5d45946662d48") }, {
    "_id": ObjectId("5c4a759820b5d45946662d48"),
    "Name": "General",
    "Steps": [
        {
            "StepId": ObjectId("5c4a75f320b5d45946662d49"),
            "Name": "Uploading",
            "ProcessorAssembly": "",
            "ProcessorClass": "",
            "ResultDocumentStatus": 3,
            "AutoDoNext": false
        },
        {
            "StepId": ObjectId("5c612f1d6598ef65ae53e0a5"),
            "Name": "Annotation",
            "ProcessorAssembly": "",
            "ProcessorClass": "",
            "ResultDocumentStatus": 1,
            "AutoDoNext": false
        }
    ]
}, { upsert: true });

db.Workflows.update({ "_id": ObjectId("5d5a5e11abf6ce767b2fc006") }, {
    "_id": ObjectId("5d5a5e11abf6ce767b2fc006"),
    "Name": "Tokenization",
    "Steps": [
        {
            "StepId": ObjectId("5d5a5e32abf6ce767b2fc007"),
            "Name": "Uploading",
            "ProcessorAssembly": "",
            "ProcessorClass": "",
            "ResultDocumentStatus": 1,
            "AutoDoNext": true
        },
        {
            "StepId": ObjectId("5d5a5e63abf6ce767b2fc008"),
            "Name": "Preannotation",
            "ProcessorAssembly": "MusicKG.Service",
            "ProcessorClass": "MusicKG.Service.SynchronizedWorkflow.Implementations.Processors.TokenizationProcessor",
            "ResultDocumentStatus": 3,
            "AutoDoNext": false
        },
        {
            "StepId": ObjectId("5d5a5f13abf6ce767b2fc009"),
            "Name": "Annotation",
            "ProcessorAssembly": "",
            "ProcessorClass": "",
            "ResultDocumentStatus": 1,
            "AutoDoNext": false
        }
    ]
}, { upsert: true });

db.Workflows.update({ "_id": ObjectId("5d898bcd1415dc2e40ebb601") }, {
    "_id": ObjectId("5d898bcd1415dc2e40ebb601"),
    "Name": "TextClassify",
    "Steps": [
        {
            "StepId": ObjectId("5d898c8d1415dc2e40ebb604"),
            "Name": "Uploading",
            "ProcessorAssembly": "",
            "ProcessorClass": "",
            "ResultDocumentStatus": 1,
            "AutoDoNext": true
        },
        {
            "StepId": ObjectId("5d898c9a1415dc2e40ebb605"),
            "Name": "Paragraphing",
            "ProcessorAssembly": "MusicKG.Service",
            "ProcessorClass": "MusicKG.Service.SynchronizedWorkflow.Implementations.Processors.ParagraphingProcessor",
            "ResultDocumentStatus": 3,
            "AutoDoNext": false
        },
        {
            "StepId": ObjectId("5d898ca01415dc2e40ebb606"),
            "Name": "Annotation",
            "ProcessorAssembly": "",
            "ProcessorClass": "",
            "ResultDocumentStatus": 1,
            "AutoDoNext": false
        }
    ]
}, { upsert: true });

db.Workflows.update({ "_id": ObjectId("5ddcd78f0d603b4a5d0a9fa5") }, {
    "_id": ObjectId("5ddcd78f0d603b4a5d0a9fa5"),
    "Name": "TextPhrasing",
    "Steps": [
        {
            "StepId": ObjectId("5ddcd78f0d603b4a5d0a9fa6"),
            "Name": "Uploading",
            "ProcessorAssembly": "",
            "ProcessorClass": "",
            "ResultDocumentStatus": 1,
            "AutoDoNext": true
        },
        {
            "StepId": ObjectId("5ddcd78f0d603b4a5d0a9fa7"),
            "Name": "Phrasing",
            "ProcessorAssembly": "MusicKG.Service",
            "ProcessorClass": "MusicKG.Service.SynchronizedWorkflow.Implementations.Processors.PhrasingProcessor",
            "ResultDocumentStatus": 3,
            "AutoDoNext": false
        },
        {
            "StepId": ObjectId("5ddcd78f0d603b4a5d0a9fa8"),
            "Name": "Annotation",
            "ProcessorAssembly": "",
            "ProcessorClass": "",
            "ResultDocumentStatus": 1,
            "AutoDoNext": false
        }
    ]
}, { upsert: true });

db.Workflows.update({ "_id": ObjectId("5de07a8de5e79b52e89c3d1c") }, {
    "_id": ObjectId("5de07a8de5e79b52e89c3d1c"),
    "Name": "TextSimilarity",
    "Steps": [
        {
            "StepId": ObjectId("5de07ad4e5e79b52e89c3d1d"),
            "Name": "Uploading",
            "ProcessorAssembly": "",
            "ProcessorClass": "",
            "ResultDocumentStatus": 1,
            "AutoDoNext": true
        },
        {
            "StepId": ObjectId("5de07329e5e79b52e89c3d1b"),
            "Name": "SimilarityText",
            "ProcessorAssembly": "MusicKG.Service",
            "ProcessorClass": "MusicKG.Service.SynchronizedWorkflow.Implementations.Processors.TextSimilarityProcessor",
            "ResultDocumentStatus": 3,
            "AutoDoNext": false
        },
        {
            "StepId": ObjectId("5de07b24e5e79b52e89c3d1e"),
            "Name": "Annotation",
            "ProcessorAssembly": "",
            "ProcessorClass": "",
            "ResultDocumentStatus": 1,
            "AutoDoNext": false
        }
    ]
}, { upsert: true });