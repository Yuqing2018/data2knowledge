db.AutoTaskCreationRules.update({ "_id": ObjectId("5fb7129c83cafe02f68d7442") }, {
    "_id": ObjectId("5fb7129c83cafe02f68d7442"),
    "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"),
    "CreatedUser": ObjectId("5fb5d373f03c2600017e34d0"),
    "OnlyCreateWhanMatchDocumentCount": false,
    "Rules": [
        {
            "Name": "Default",
            "DocumentTags": [],
            "Annotators": [ObjectId("5fb5d39af03c2600017e34d1")],
            "DocumentCount": 1,
            "Overlap": 0,
            "IsAutoApproved": true,
            "IsAutoMerged": true,
            "MaxFinishDays": 30
        }
    ]
}, { upsert: true });