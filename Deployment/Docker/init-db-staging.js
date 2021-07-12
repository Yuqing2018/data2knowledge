db.Users.update({ "_id": ObjectId("5fa900c0d412d30001ae1f9c") }, {
    "_id": ObjectId("5fa900c0d412d30001ae1f9c"),
    "Name": "admin",
    "Status": 0,
    "Salt": "86EIlDTkjeBe5at4yTJZLzJ5aZ0wjXLs43llBNqsRaqhqyk72p7Z0kNIxIIwMwbsCijcZhDUCriLjBPLO2E40WCTpMY2f4Pv97OhoiiUVmwIccy626RT1jCQQAXmtKPILXMlWHwGUTiNz3Qmn29in6D2F8BYu6BlHY6pAnjvKk9i8ZqWTK8BKGvPtUQXjIlYs6jL6BWaSOIc6v3QSOeT79KaCq5QHvob9lZbSP0sY8S68tT819fCDB0twfiPFjDx",
    "Password": "407D05067A3E5CC8BA0ED7876AA1CD859059C670E6D36733743CDB8473631B1F",
    "Roles": [0],
    "Tokens": [],
    "CreatedBy": "admin",
    "CreatedAt": ISODate()
}, { upsert: true });

db.Users.update({ "_id": ObjectId("5fade7ee2ef3a83b042e6972") }, {
    "_id": ObjectId("5fade7ee2ef3a83b042e6972"),
    "Name": "honda_admin",
    "Status": 0,
    "Salt": "7PAsbkKBWZiTbJiY8jEyIw7JeKqZxaRLNpiWKxLdEmjw7w79iMkOhtlKS0nA4VdWhGfWBwoKIB3vpKLRNqRUZcEudHdTjkjTiuoBqkHWyKjQzZvhoGWNdt6fcVS4sFAxsjn5vkBCT4wp6Y4nm5ENxFPZxjyFCME3kTxpMzWdft3j7Rio0FSwaW9WEyHlW5ajACfmAOLWjAIdMG1QbkP74i2xTKVo2LrjtXTEM5Xl36mJo5k8G1kLpdmsJRDZ3w4I",
    "Password": "15DEEB2D1BBDD6C5F4DAA08555F787CBDC672C8A8CBA76EEECDA7DAFC5B5B0A7",
    "Roles": [0],
    "Tokens": [],
    "CreatedBy": "admin",
    "CreatedAt": ISODate()
}, { upsert: true });

db.Users.update({ "_id": ObjectId("5fb5d373f03c2600017e34d0") }, {
    "_id": ObjectId("5fb5d373f03c2600017e34d0"),
    "Name": "honda_manager",
    "Status": 0,
    "Salt": "Cti27TAvjDq4UEywwE6yT6BQB1bAFGRbJilfrrdEoiOj1EktWzSAIM6F2CBjS90JWouLssffACjLrrZ6QRNfYrC6lPmYZ7WosexMkJaDuM9jCvnLMEzawUVFWxzMjqpDP5sn3mWhQkVlrwD0CKmvHV2Hs20WjxxdwGAHcaSARsul9Knz4JkFe454cV4bwa8sXRZU55XB2VOoAQHSOA1Jn19CLFFqjRdQucyrUDE8VTMsqQEvnlw5iyf5SxjAAGvl",
    "Password": "4BFF340AEE0D519D53FED3FF408B25294CC7918BA242B26753EDAB6FC3032C06",
    "Roles": [1],
    "Tokens": [],
    "CreatedBy": "honda_admin",
    "CreatedAt": ISODate()
}, { upsert: true });

db.Users.update({ "_id": ObjectId("5fb5d39af03c2600017e34d1") }, {
    "_id": ObjectId("5fb5d39af03c2600017e34d1"),
    "Name": "honda_user1",
    "Status": 0,
    "Salt": "R74ysp8l63XDpQl4bjd4uiI56DJxtdHpxAKtWfd8nk9pLLQbloyqDB1Jz8BYQL12vIjYf2i5ffQkisuvXHlfPqV6Jb4XNorMtj3cF16tjClyURAjNjbizWneWjoe8ur0Xe1bysZ5L28mfxl2KXkHnjmG3ZqvM1uWkuLQZyRvrjGHXb8z7wElkUNN1uiJbDsTHmbVWFclfYqjMPS200yqxjLAQ7ZFqitXYWaL9nNCtsfvEHamLpSlfcwxKLfYQH46",
    "Password": "1184F87E02A53F3E8FCC347BA4EC9B04D3AD489F450A90AAB486BA7AC36F8659",
    "Roles": [2],
    "Tokens": [],
    "CreatedBy": "honda_admin",
    "CreatedAt": ISODate()
}, { upsert: true });

db.Users.update({ "_id": ObjectId("5fb5d3a8f03c2600017e34d2") }, {
    "_id": ObjectId("5fb5d3a8f03c2600017e34d2"),
    "Name": "honda_user2",
    "Status": 0,
    "Salt": "ECsZPh2Nn56fdq5j6M3aAiRVAMayhYWWxi0bokHExe7kfquuP61dN4GwVGzcjEFyjjR4z34VfzNvlHQjsQSl6SjDMEqSVEx3JIa5Jz5Vbj7oqlln0pUAVtCAans4Nl9LbUVMGUK2U6j0V4mBtc2GT7hpjK4vjaiZjwnGzIRfO1PYkLOjb0LKtD2dMGG60oWUYtUzRPcVvX21rtfF5YuqzHGMEj42bUpzf5a00pzvY0pGLaWsBbab4GvqXi5HUjTK",
    "Password": "1ECADDB0447C46894D8D53FAEFA479FD11BE4F84EA09F0EDF6DC39DB0EC9BF2D",
    "Roles": [2],
    "Tokens": [],
    "CreatedBy": "honda_admin",
    "CreatedAt": ISODate()
}, { upsert: true });

db.Users.update({ "_id": ObjectId("5fb5d3bff03c2600017e34d3") }, {
    "_id": ObjectId("5fb5d3bff03c2600017e34d3"),
    "Name": "honda_user3",
    "Status": 0,
    "Salt": "HCVwtoOPz8BJs9bDxMBaUnFJKTcSu1KOjP8joA9mwvM5eV8XjBkiST0u6mVJm6vHyXCMb2p7NoNyKKFYVoL3RC5wZOmbOzaz9eHrsvWomnFDjBTwR7Y7vC051jXxPnsnoozGdnFzCwRD6Bqy40uQLcdHoLzmIzJ84sp1cntDCyF9Vw9z0zCZ2MLlk1rBYt1cXSJOmyvQG1onHeW1Mawj6TfsQdRk61PjHCmcXumBdVsbxyNjfzZX6RfdMr8iRlUP",
    "Password": "F08F5D4AA0D32AE69898F210BE225A8C8049A1C265E03C4987C1506B3B6D62B2",
    "Roles": [2],
    "Tokens": [],
    "CreatedBy": "honda_admin",
    "CreatedAt": ISODate()
}, { upsert: true });

db.Users.update({ "_id": ObjectId("5fb5d3cef03c2600017e34d4") }, {
    "_id": ObjectId("5fb5d3cef03c2600017e34d4"),
    "Name": "honda_user4",
    "Status": 0,
    "Salt": "mfQf2TerX4flBe4szKfGb1mZwizFwdT1aNZsSIovvIabM4MIxPTjfoNu30STuJ5TlnkXoMK1wUkD1knCG1iQuNWW9Ey1ZEbWMq0yUYtUf2UvdkDSNqnCAoyHByYTnTb1Ty0KtYSRHxLC2ASZbQnJrIvy1s6waKHw02LOt2yYIF5cM6ObP8SHFCJldCSa2vQ70pLaYdGirzaVMtV5O5Qb190ysAL0kx6mY8lxvOmLCJqjBXJURXw7PPf1zmRyBP83",
    "Password": "3FEC614B8CDDB27AEB63936816CB79D7FE8152A913DD64A64B4D5CF7B418936A",
    "Roles": [2],
    "Tokens": [],
    "CreatedBy": "honda_admin",
    "CreatedAt": ISODate()
}, { upsert: true });

db.Users.update({ "_id": ObjectId("5fb5d3dbf03c2600017e34d5") }, {
    "_id": ObjectId("5fb5d3dbf03c2600017e34d5"),
    "Name": "honda_user5",
    "Status": 0,
    "Salt": "CY6NhyK9TeIrTU7E11ixb9Hj2u8i7YkXLxNN15MtmxoLhXiOpmGKGK2DqXUWzBc54VQvjuN5oZkNV6cfmt3VrJqaFjwdk03UvsQvjWH5IKvRAoSuVPJjIqNuEQ077KVy5AsCpQ8zV8mQoIKrBbsFqXlePw1A6K77Z7Wa6qfeQjuKe7j5fJF5sphionX3ZHDkao0rWKuk6nYjCLaRp5vxebPJZOWSDlpjQkJZjYiFov9fBaPMnfN3mjf9JrBiYMRU",
    "Password": "8073636B80CE78A7D9EFC2D8EB941C729A892329916FF03426CF9FD246A575F5",
    "Roles": [2],
    "Tokens": [],
    "CreatedBy": "honda_admin",
    "CreatedAt": ISODate()
}, { upsert: true });

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

db.WorkspaceTypes.update({ "_id": ObjectId("5faba9e42f2fb1c4d65e3cc3") }, { "_id": ObjectId("5faba9e42f2fb1c4d65e3cc3"), "Name": "VehicleFailureClassification", "WorkflowId": ObjectId("5faba9fe2f2fb1c4d65e3cc4"), "Status": 0 }, { upsert: true });

db.Workspaces.update({ "_id": ObjectId("5fb3660530b3090001e0c6d8") }, { "_id": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "Honda", "Description": "Honda Labeling Platform", "Type": ObjectId("5faba9e42f2fb1c4d65e3cc3"), "Language": 0, "IsAutoMerging": true, "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate(), "IsDeleted": false, "DeletedAt": null, "ReadOnlyUserIds": null, "IsAutoCreateTask": true, "ResultHandlerAssembly": "MusicKG.HondaPlugins.ResultHandler", "ResultHandlerClass": "MusicKG.HondaPlugins.ResultHandler.HondaResultHandler" }, { upsert: true });

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

db.Dictionaries.update({ "_id": ObjectId("5fb48af230b3090001e0c6f1") }, {
        "_id": ObjectId("5fb48af230b3090001e0c6f1"),
        "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"),
        "Name": "零件名",
        "EntityId": null,
        "Vocabularies": [
            "ＡＮＣ麦克风总成",
            "BCM 单元组件",
            "BCM单元组件",
            "BCM控制单元组件",
            "ＥＶＣＣ总成",
            "PCU总成",
            "保险丝盒总成",
            "变速箱总成",
            "玻璃升降器",
            "车门遥控器总成",
            "乘客侧温控马达",
            "乘员侧电动车窗开关",
            "乘员侧电动车窗开关总成",
            "乘员侧温控马达",
            "传感器组件",
            "大灯（左　连角灯）",
            "大灯高低调节前传感器",
            "倒车雷达传感器",
            "倒车雷达控制单元",
            "地板线束",
            "低音喇叭总成",
            "电动车窗主开关总成",
            "电机总成",
            "多功能摄像头模块总成",
            "方向机拉杆",
            "方向机总成",
            "方向机组件",
            "方向盘",
            "方向盘组合开关",
            "缸盖罩",
            "缸盖罩衬垫",
            "缸体总成",
            "功率放大器",
            "鼓风机壳体分总成",
            "鼓风机马达",
            "横拉杆右球头",
            "后横梁总成",
            "后摄像头",
            "后视摄像总成",
            "后轴梁总成",
            "环境传感器",
            "驾驶员侧车门线束",
            "驾驶员侧窗开关总成",
            "减震器芯",
            "节能开关",
            "空调压缩机",
            "空调硬管组件",
            "拉杆密封件",
            "轮胎",
            "门锁驱动组件",
            "汽油泵",
            "前轮鼓轴承",
            "前门左侧升降器",
            "前显示器",
            "前右减震器芯",
            "前雨刮连杆",
            "收放机总成",
            "天线",
            "微型继电器总成",
            "温度控制面板",
            "显示屏",
            "线束卷盘总成",
            "巡航开关",
            "扬声器总成",
            "仪表盘线束",
            "仪表线束",
            "油压调节阀",
            "右后视镜",
            "右前大灯总成",
            "右前减震器单元",
            "右前减震器芯",
            "右前门窗框胶带",
            "右前下臂组件",
            "远程连接控制单元",
            "照明开关总成",
            "智能控制单元组件",
            "智能显示屏总成",
            "主阀体总成",
            "主零件中文名",
            "主气囊",
            "主气囊控制单元",
            "驻车传感器总成",
            "转向管柱总成",
            "转向柱总成",
            "自动空调处理器",
            "组合仪表",
            "组合仪表总成",
            "左后尾灯",
            "左后尾灯(厢灯)",
            "左后尾盖灯",
            "左机舱线束",
            "左前大灯总成",
            "左前门窗框胶带",
            "左前门电动玻璃升降器",
            "左前下臂组件",
            "左前座椅骨架",
            "左前座椅靠背骨架"
        ],
        "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"),
        "CreatedAt": ISODate("2020-11-19T04:49:11.855Z"),
        "IsDeleted": false,
        "DeletedAt": null
    }, { upsert: true });

    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7d3") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7d3"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "4档冒烟", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7d4") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7d4"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "ACCH不良", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7d5") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7d5"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "AC开关无电", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7d6") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7d6"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "DA屏常亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7d7") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7d7"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "DVD无法启动", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7d8") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7d8"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "EPB作动音", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7d9") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7d9"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "EPB异响", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7da") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7da"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "EPB作动音", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7db") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7db"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "H标开裂", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7dc") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7dc"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "LED灯脱落", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7dd") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7dd"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "USB无法充电", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7de") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7de"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "U盘无法读取", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7df") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7df"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "故障灯亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7e0") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7e0"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "故障灯亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7e1") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7e1"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "acuralink无法使用", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7e2") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7e2"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "acuralink无法正常使用", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7e3") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7e3"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "nan", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7e4") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7e4"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "上下无法调节", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7e5") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7e5"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "上下调节异响", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7e6") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7e6"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "上升反弹", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7e7") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7e7"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "上升回弹", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7e8") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7e8"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "上升缓慢", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7e9") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7e9"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "下垂", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7ea") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7ea"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "下调异响", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7eb") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7eb"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "下降异响", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7ec") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7ec"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "不亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7ed") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7ed"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "不作动", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7ee") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7ee"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "不保修", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7ef") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7ef"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "不充电", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7f0") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7f0"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "不出风", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7f1") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7f1"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "不制冷", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7f2") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7f2"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "不发电", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7f3") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7f3"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "不响", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7f4") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7f4"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "不回位", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7f5") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7f5"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "不回正", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7f6") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7f6"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "不对称", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7f7") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7f7"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "不对齐", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7f8") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7f8"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "不工作", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7f9") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7f9"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "不平", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7fa") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7fa"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "不报警", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7fb") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7fb"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "不正", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7fc") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7fc"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "不聚光", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7fd") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7fd"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "不能停止", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7fe") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7fe"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "不能前后移动", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7ff") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7ff"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "不能复位", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa800") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa800"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "不舒适", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa801") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa801"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "不良", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa802") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa802"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "不贴", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa803") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa803"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "不走车", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa804") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa804"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "与官网图不一致", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa805") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa805"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "与车身间隙大", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa806") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa806"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "中央显示屏不工作", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa807") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa807"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "中央显示屏不显示", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa808") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa808"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "中央显示屏按键失灵", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa809") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa809"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "中央显示屏无电", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa80a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa80a"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "中央显示屏黑屏", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa80b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa80b"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "中控台异响", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa80c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa80c"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "中控异响", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa80d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa80d"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "中部凹陷", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa80e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa80e"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "中间位置凹陷", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa80f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa80f"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "主控开关背景灯闪烁", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa810") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa810"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "主驾座椅开关失灵", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa811") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa811"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "乱码", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa812") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa812"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "亏电", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa813") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa813"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa814") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa814"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "亮度不一样", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa815") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa815"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "仪表", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa816") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa816"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "仪表不显示", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa817") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa817"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "仪表亮度无法调节", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa818") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa818"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "仪表异常", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa819") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa819"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "仪表提示机油压力低故障", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa81a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa81a"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "仪表提示音异常", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa81b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa81b"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "仪表故障亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa81c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa81c"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "仪表故障灯", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa81d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa81d"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "仪表故障灯亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa81e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa81e"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "仪表无提示音", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa81f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa81f"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "仪表无显示", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa820") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa820"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "仪表显示不全", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa821") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa821"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "仪表氛围灯不亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa822") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa822"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "仪表温度显示不准", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa823") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa823"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "仪表温度显示异常", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa824") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa824"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "仪表灯不亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa825") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa825"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "仪表盘背景灯不亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa826") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa826"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "仪表胎压灯亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa827") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa827"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "仪表花屏", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa828") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa828"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "仪表里程不显示", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa829") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa829"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "仪表黑屏", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa82a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa82a"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "伤口", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa82b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa82b"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "位置设计不合理", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa82c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa82c"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "低", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa82d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa82d"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "低速抖动", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa82e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa82e"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "低速顿挫", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa82f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa82f"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "低频共振", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa830") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa830"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "作动不畅", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa831") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa831"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "作动不良", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa832") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa832"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "作动时有电流声", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa833") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa833"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "作动音", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa834") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa834"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "信号差", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa835") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa835"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "倒挡抖动", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa836") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa836"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "倒车影像左右线偏", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa837") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa837"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "倒车影像异常", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa838") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa838"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "倒车无影像", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa839") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa839"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "倒车时中控无反应", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa83a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa83a"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "倒车时刹车异响", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa83b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa83b"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "倒车灯不亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa83c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa83c"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "倒车灯常亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa83d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa83d"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "倒车雷达故障", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa83e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa83e"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "倾斜", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa83f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa83f"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "偏磨", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa840") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa840"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "偏移", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa841") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa841"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "做工差", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa842") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa842"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "充电不足", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa843") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa843"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "光晕", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa844") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa844"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "光线不聚焦", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa845") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa845"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "关不上", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa846") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa846"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "关不紧", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa847") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa847"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "关门力大", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa848") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa848"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "关闭时翘起", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa849") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa849"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "其他", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa84a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa84a"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "内侧生锈", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa84b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa84b"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "内侧裂开", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa84c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa84c"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "内部变形", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa84d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa84d"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "减配", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa84e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa84e"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "减震差", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa84f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa84f"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "凹凸不平", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa850") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa850"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "凹陷", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa851") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa851"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "出风异常", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa852") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa852"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "划伤", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa853") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa853"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "划痕", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa854") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa854"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "划痕多", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa855") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa855"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "刮不干净", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa856") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa856"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "制动异响", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa857") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa857"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "制动抖动", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa858") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa858"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "制动灯亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa859") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa859"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "刹车失灵", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa85a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa85a"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "刹车异响", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa85b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa85b"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "刹车抖动", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa85c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa85c"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "刹车灯不亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa85d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa85d"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "刹车灯常亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa85e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa85e"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "刹车硬", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa85f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa85f"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "前后异响", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa860") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa860"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "前后调节异响", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa861") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa861"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "前后调节时声音大", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa862") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa862"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "加改装导致不加热", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa863") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa863"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "加油不走", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa864") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa864"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "加油跳枪", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa865") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa865"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "加热异常", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa866") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa866"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "加装导航不显示", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa867") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa867"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "加速不良", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa868") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa868"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "加速受限", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa869") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa869"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "加速受限、机油增多", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa86a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa86a"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "加速延迟", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa86b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa86b"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "加速异响", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa86c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa86c"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "加速慢", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa86d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa86d"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "加速抖动", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa86e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa86e"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "加速无力", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa86f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa86f"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "加速顿挫", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa870") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa870"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "动力不足", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa871") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa871"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "升到顶反弹", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa872") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa872"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "升级后车辆无法启动", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa873") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa873"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "升起后挤压门饰板", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa874") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa874"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "升降不良", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa875") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa875"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "升降反弹", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa876") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa876"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "升降异常", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa877") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa877"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "升降慢", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa878") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa878"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "升降缓慢", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa879") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa879"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "单音", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa87a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa87a"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "卡扣损坏", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa87b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa87b"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "卡扣断裂", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa87c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa87c"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "卡扣脱落", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa87d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa87d"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "卡滞", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa87e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa87e"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "卡滞、机油增多", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa87f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa87f"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "卡滞、漏油", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa880") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa880"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "卡滞、顿挫", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa881") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa881"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "卡滞升级", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa882") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa882"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "卡滞软件升级", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa883") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa883"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "卡滞（服务投诉）", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa884") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa884"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "卡顿", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa885") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa885"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "印痕", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa886") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa886"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "印迹", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa887") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa887"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "参数异常", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa888") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa888"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "发动机故障灯亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa889") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa889"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "发卡", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa88a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa88a"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "发热", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa88b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa88b"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "发白", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa88c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa88c"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "发罩弹起", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa88d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa88d"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "发花", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa88e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa88e"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "发霉/泛白", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa88f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa88f"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "发黑", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa890") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa890"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "变形", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa891") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa891"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "变暗", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa892") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa892"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "变色", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa893") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa893"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "变色过保索赔", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa894") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa894"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "变速箱", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa895") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa895"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "口哨声", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa896") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa896"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "口哨音", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa897") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa897"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "召回", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa898") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa898"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "召回前费用", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa899") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa899"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "召回后二次不良", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa89a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa89a"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "召回后动力不足", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa89b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa89b"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "召回后抖动", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa89c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa89c"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "召回后油耗高", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa89d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa89d"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "召回后油表显示不正常", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa89e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa89e"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "召回后漏油", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa89f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa89f"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "召回后问题", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8a0") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8a0"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "召回安排", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8a1") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8a1"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "召回态度差", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8a2") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8a2"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "召回抱怨", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8a3") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8a3"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "召回更换新油泵后车辆无法启动", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8a4") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8a4"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "召回疑问", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8a5") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8a5"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "召回相关", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8a6") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8a6"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "召回等待时间长", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8a7") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8a7"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "召回维修后漏油", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8a8") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8a8"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "召回维修问题", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8a9") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8a9"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "召回补偿", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8aa") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8aa"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "召回费用", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ab") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ab"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "召回路程长", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ac") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ac"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "召回通知不及时", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ad") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ad"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "召回零件安排不当", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ae") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ae"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "右前油漆不良", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8af") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8af"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "吃胎", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8b0") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8b0"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "后备箱自动开启", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8b1") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8b1"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "后排温度高", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8b2") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8b2"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "后视镜折叠开关失灵", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8b3") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8b3"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "后视镜无法折叠", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8b4") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8b4"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "后视镜片无法调整", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8b5") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8b5"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "后视镜自动折叠", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8b6") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8b6"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "后视镜镜片无法调节", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8b7") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8b7"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "后轮抱死", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8b8") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8b8"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "后轮磨损", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8b9") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8b9"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "后雨刮自动工作", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ba") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ba"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "后雾灯常亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8bb") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8bb"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "后雾灯指示不良", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8bc") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8bc"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "吐槽", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8bd") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8bd"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "启停不工作", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8be") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8be"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "启停作动时异响", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8bf") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8bf"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "启动困难", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8c0") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8c0"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "启动异响", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8c1") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8c1"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "启动抖动", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8c2") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8c2"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "响声", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8c3") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8c3"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "响声大", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8c4") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8c4"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "啃胎", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8c5") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8c5"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "喇叭不响", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8c6") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8c6"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "喇叭长响", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8c7") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8c7"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "噪声大", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8c8") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8c8"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "噪音", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8c9") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8c9"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "噪音大", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ca") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ca"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "回位不良", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8cb") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8cb"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "地图不更新", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8cc") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8cc"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "地毯积水", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8cd") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8cd"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "坐起来不舒服", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ce") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ce"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "堵塞", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8cf") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8cf"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "塌陷", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8d0") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8d0"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "增多", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8d1") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8d1"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "声音大", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8d2") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8d2"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "声音小", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8d3") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8d3"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "壳体熔损", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8d4") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8d4"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "外伤", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8d5") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8d5"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "外壳变色", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8d6") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8d6"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "外观不良", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8d7") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8d7"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "多功能按键失灵", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8d8") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8d8"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "多功能按键开裂", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8d9") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8d9"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "多功能按键断裂", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8da") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8da"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "多功能按键破损", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8db") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8db"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "多功能按键脱漆", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8dc") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8dc"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "大灯不亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8dd") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8dd"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "大灯异常", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8de") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8de"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "大灯熔损", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8df") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8df"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "大灯闪烁", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8e0") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8e0"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "大灯高度无法调节", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8e1") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8e1"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "天窗无法打开", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8e2") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8e2"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "天窗无法正常开合", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8e3") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8e3"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "失去动力", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8e4") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8e4"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "失效", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8e5") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8e5"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "失灵", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8e6") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8e6"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "失速", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8e7") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8e7"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "安全性差", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8e8") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8e8"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "安全性质疑", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8e9") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8e9"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "安装不良", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ea") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ea"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "定速巡航无法使用", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8eb") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8eb"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "对象内", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ec") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ec"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "对象内，服务", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ed") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ed"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "对象内，补偿", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ee") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ee"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "对象内，费用", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ef") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ef"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "对象内，退车", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8f0") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8f0"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "对象内，通知错误", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8f1") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8f1"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "导航不准", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8f2") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8f2"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "导航定位不准", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8f3") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8f3"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "导航屏幕不亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8f4") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8f4"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "导航无法使用", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8f5") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8f5"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "导航闪屏", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8f6") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8f6"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "导轨饰板扣破损", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8f7") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8f7"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "小灯常亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8f8") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8f8"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "尾灯常亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8f9") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8f9"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "尾箱不灵敏", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8fa") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8fa"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "尾箱打不开", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8fb") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8fb"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "尾箱无法关闭", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8fc") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8fc"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "尾门一脚踢功能异常", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8fd") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8fd"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "尾门打不开", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8fe") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8fe"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "尾门无法一键上锁", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ff") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ff"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "尾门无法开启", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa900") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa900"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "尾门无法打开", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa901") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa901"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "尾门电机不作动", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa902") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa902"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "尾门电机异响", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa903") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa903"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "局部凹陷", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa904") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa904"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "屏幕有白点", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa905") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa905"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "屏幕漏光", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa906") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa906"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "工作音", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa907") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa907"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "左前扬声器不响", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa908") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa908"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "左前扬声器杂音", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa909") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa909"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "左右大灯亮度不一样", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa90a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa90a"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "左右大灯颜色不一样", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa90b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa90b"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "左右大灯颜色不一致", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa90c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa90c"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "左右差", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa90d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa90d"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "左右间隙不一致", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa90e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa90e"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "左转向指示灯常亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa90f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa90f"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "左转向灯常亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa910") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa910"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "差", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa911") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa911"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "常亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa912") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa912"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "座椅加热故障灯亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa913") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa913"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "座椅靠背无法调节", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa914") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa914"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "开关不灵敏", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa915") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa915"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "开关不畅", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa916") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa916"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "开关不良", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa917") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa917"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "开关失灵", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa918") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa918"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "开关拉手断裂", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa919") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa919"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "开关无法回位", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa91a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa91a"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "开县", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa91b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa91b"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "开启卡顿", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa91c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa91c"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "开启困难", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa91d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa91d"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "开启异响", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa91e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa91e"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "开启范围小", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa91f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa91f"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "开小灯天窗打开", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa920") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa920"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "开机慢", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa921") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa921"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "开空调有异物吹出", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa922") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa922"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "开线", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa923") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa923"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "开裂", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa924") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa924"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "开裂抖动", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa925") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa925"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "开门时灯不亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa926") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa926"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "异味", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa927") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa927"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "异响", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa928") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa928"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "异响、抖动", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa929") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa929"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "异响、顿挫", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa92a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa92a"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "异响（上铰链）", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa92b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa92b"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "异常", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa92c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa92c"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "异常作动", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa92d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa92d"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "异常磨损", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa92e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa92e"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "异物", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa92f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa92f"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "弹起", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa930") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa930"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "强度不足", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa931") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa931"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "怠速不稳", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa932") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa932"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "怠速中控异响", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa933") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa933"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "怠速启停失效", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa934") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa934"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "怠速异常", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa935") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa935"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "怠速抖动", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa936") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa936"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "怠速显示高", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa937") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa937"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "怠速起停不工作", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa938") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa938"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "恢复初始值", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa939") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa939"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "感应不良", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa93a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa93a"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "感应式雨刮失灵", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa93b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa93b"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "手刹放不了", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa93c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa93c"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "手刹无法解开", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa93d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa93d"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "手刹无法解除", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa93e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa93e"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "扎钉", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa93f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa93f"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "打不开", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa940") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa940"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "打不开门", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa941") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa941"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "打方向异响", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa942") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa942"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "打方向重", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa943") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa943"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "打滑", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa944") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa944"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "扬声器不响", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa945") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa945"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "扬声器无声音", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa946") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa946"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "扬声器有杂音", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa947") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa947"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "扬声器杂音", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa948") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa948"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "扬声器没声音", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa949") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa949"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "扶手卡不住", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa94a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa94a"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "扶手调整不良", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa94b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa94b"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "技术帖", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa94c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa94c"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "抓地力不足", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa94d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa94d"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "抖动", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa94e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa94e"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "抖动、熄火", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa94f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa94f"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "抖动异响", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa950") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa950"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "抖动故障灯亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa951") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa951"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "折叠过度", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa952") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa952"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "折皱", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa953") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa953"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "抛光蜡残留", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa954") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa954"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "拔不下来", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa955") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa955"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "拧不动", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa956") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa956"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "挂R挡抖动", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa957") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa957"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "挂不了档", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa958") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa958"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "挂倒挡异响", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa959") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa959"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "挂挡不走", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa95a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa95a"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "挂挡不走车", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa95b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa95b"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "挂挡异响", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa95c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa95c"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "挂挡无法走车", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa95d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa95d"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "挂挡熄火", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa95e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa95e"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "指示灯错误", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa95f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa95f"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "指针不回位", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa960") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa960"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "指针不良", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa961") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa961"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "指针异常复位", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa962") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa962"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "按不下去", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa963") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa963"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "按喇叭异响", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa964") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa964"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "按钮断裂", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa965") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa965"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "按钮无法调节", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa966") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa966"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "按钮脱落", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa967") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa967"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "按键不亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa968") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa968"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "按键卡滞", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa969") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa969"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "按键回位不良", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa96a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa96a"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "按键失灵", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa96b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa96b"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "按键开裂", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa96c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa96c"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "按键损坏", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa96d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa96d"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "按键断裂", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa96e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa96e"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "按键无反应", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa96f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa96f"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "按键无背景光", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa970") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa970"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "按键松动", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa971") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa971"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "按键灯不亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa972") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa972"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "按键破损", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa973") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa973"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "按键背光灯不亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa974") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa974"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "按键背景灯闪烁", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa975") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa975"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "按键脱漆", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa976") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa976"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "损坏", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa977") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa977"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "换挡抖动", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa978") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa978"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "换挡顿挫", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa979") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa979"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "掉漆", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa97a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa97a"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "掉皮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa97b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa97b"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "掉色", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa97c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa97c"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "接口处生锈", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa97d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa97d"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "播放音乐死机", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa97e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa97e"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "操作力大", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa97f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa97f"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "操作疑问", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa980") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa980"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "支撑问题", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa981") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa981"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "收放机不工作", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa982") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa982"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "收音机无法使用", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa983") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa983"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "收音机有杂音", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa984") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa984"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "改装导致失去动力", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa985") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa985"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "故障", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa986") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa986"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "故障灯亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa987") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa987"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "故障灯亮加速无力", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa988") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa988"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "故障灯亮，加速异常", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa989") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa989"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "故障灯异常", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa98a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa98a"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "故障码", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa98b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa98b"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "故障闪烁", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa98c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa98c"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "敏感", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa98d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa98d"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "断裂", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa98e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa98e"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "方向不正", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa98f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa98f"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "方向卡滞", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa990") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa990"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "方向卡滞升级", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa991") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa991"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "方向卡滞软件升级", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa992") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa992"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "方向发卡", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa993") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa993"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "方向盘多功能按键失灵", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa994") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa994"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "方向盘按键失灵", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa995") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa995"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "旋钮松动", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa996") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa996"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "无助力", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa997") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa997"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "无声音", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa998") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa998"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "无影像", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa999") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa999"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "无提示", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa99a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa99a"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "无提示音", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa99b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa99b"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "无显示", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa99c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa99c"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "无法上锁", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa99d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa99d"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "无法使用", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa99e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa99e"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "无法关闭", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa99f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa99f"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "无法升级", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9a0") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9a0"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "无法升降", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9a1") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9a1"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "无法启动", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9a2") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9a2"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "无法开启", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9a3") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9a3"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "无法开机", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9a4") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9a4"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "无法开闭", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9a5") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9a5"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "无法打开", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9a6") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9a6"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "无法折叠", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9a7") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9a7"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "无法拉出", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9a8") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9a8"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "无法换挡", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9a9") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9a9"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "无法断电", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9aa") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9aa"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "无法显示里程", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ab") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ab"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "无法熄火", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ac") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ac"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "无法自动升降", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ad") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ad"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "无法自动折叠", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ae") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ae"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "无法行驶", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9af") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9af"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "无法解锁", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9b0") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9b0"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "无法调节", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9b1") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9b1"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "无法转向", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9b2") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9b2"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "无法连接", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9b3") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9b3"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "无法锁车", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9b4") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9b4"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "无网络信号", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9b5") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9b5"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "无防夹功能", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9b6") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9b6"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "无阻尼", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9b7") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9b7"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "日行灯不亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9b8") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9b8"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "日行灯损坏", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9b9") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9b9"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "日行灯间隙性不亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ba") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ba"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "早期磨损", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9bb") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9bb"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "时钟不准", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9bc") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9bc"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "时间显示错误", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9bd") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9bd"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "显示不全", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9be") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9be"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "显示不准", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9bf") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9bf"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "显示不良", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9c0") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9c0"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "显示内容颠倒", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9c1") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9c1"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "显示屏不工作", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9c2") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9c2"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "显示屏外伤", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9c3") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9c3"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "显示屏支架断裂", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9c4") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9c4"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "显示屏黑屏", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9c5") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9c5"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "显示异常", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9c6") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9c6"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "显示颠倒", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9c7") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9c7"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "晃动", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9c8") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9c8"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "晃动音", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9c9") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9c9"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "晒伤", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ca") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ca"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "智能进入失效", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9cb") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9cb"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "智能进入失灵", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9cc") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9cc"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "智能钥匙失灵", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9cd") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9cd"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "暗电流大", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ce") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ce"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "曲轴磨损卡死", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9cf") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9cf"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "有凸点", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9d0") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9d0"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "有印迹", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9d1") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9d1"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "有反光点", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9d2") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9d2"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "有异物", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9d3") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9d3"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "有毛刺", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9d4") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9d4"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "有水渍", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9d5") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9d5"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "有水痕", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9d6") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9d6"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "有褶皱", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9d7") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9d7"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "有雪花状异物", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9d8") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9d8"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "有黄色污迹", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9d9") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9d9"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "有黄色污迹流出", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9da") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9da"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "有黄色油迹流出", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9db") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9db"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "未作动", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9dc") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9dc"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "未启动", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9dd") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9dd"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "未展开", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9de") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9de"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "未弹起", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9df") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9df"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "机油压力低", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9e0") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9e0"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "机油增多", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9e1") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9e1"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "机油增多、加速受限", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9e2") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9e2"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "机盖弹起", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9e3") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9e3"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "杂音", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9e4") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9e4"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "松动", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9e5") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9e5"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "松脱", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9e6") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9e6"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "档位抖动", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9e7") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9e7"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "档位无显示", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9e8") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9e8"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "模糊", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9e9") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9e9"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "正常", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ea") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ea"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "正常磨损", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9eb") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9eb"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "死机", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ec") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ec"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "段差", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ed") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ed"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "段差大", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ee") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ee"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "气囊壳破损", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ef") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ef"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "气囊灯亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9f0") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9f0"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "气囊皮磨损", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9f1") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9f1"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "气囊皮纹理不均匀", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9f2") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9f2"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "气囊皮起泡", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9f3") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9f3"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "气泡", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9f4") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9f4"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "氛围灯不亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9f5") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9f5"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "氧化", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9f6") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9f6"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "水温高", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9f7") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9f7"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "水纹", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9f8") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9f8"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "污迹", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9f9") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9f9"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "沙眼", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9fa") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9fa"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "油漆不良", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9fb") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9fb"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "油漆开裂", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9fc") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9fc"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "油漆污染", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9fd") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9fd"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "油漆腐蚀", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9fe") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9fe"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "油漆黄点", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ff") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ff"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "油漆龟裂", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa00") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa00"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "油耗高", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa01") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa01"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "油表不准", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa02") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa02"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "油表显示不准", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa03") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa03"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "油表显示错误", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa04") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa04"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "油量确认", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa05") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa05"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "泄露", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa06") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa06"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "波浪纹", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa07") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa07"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "波纹", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa08") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa08"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "流痕", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa09") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa09"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "海绵塌陷", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa0a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa0a"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "消声块脱落", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa0b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa0b"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "清洗后故障", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa0c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa0c"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "渗油", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa0d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa0d"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "温度显示异常", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa0e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa0e"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "温度高", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa0f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa0f"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "温控制开关损坏", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa10") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa10"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "溢出", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa11") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa11"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "滑动门框掉漆", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa12") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa12"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "滑牙", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa13") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa13"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "滴水", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa14") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa14"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "滴水痕", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa15") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa15"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "漆斑", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa16") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa16"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "漆面不良", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa17") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa17"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "漆面腐蚀", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa18") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa18"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "漏光", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa19") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa19"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "漏冷却液", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa1a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa1a"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "漏冷媒", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa1b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa1b"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "漏气", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa1c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa1c"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "漏水", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa1d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa1d"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "漏汽油", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa1e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa1e"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "漏油", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa1f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa1f"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "漏油、顿挫", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa20") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa20"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "漏液", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa21") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa21"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "漏电", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa22") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa22"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "漏装", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa23") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa23"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "漏雪种", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa24") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa24"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "漏风", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa25") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa25"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "灯不亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa26") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa26"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "灯光不亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa27") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa27"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "灯光不聚焦", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa28") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa28"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "灯光亮度不一致", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa29") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa29"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "灯光偏暗", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa2a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa2a"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "灯光偏黄", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa2b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa2b"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "灯光发蓝", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa2c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa2c"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "灯光发黄", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa2d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa2d"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "灯光开关失灵", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa2e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa2e"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "灯光异常", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa2f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa2f"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "灯光暗", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa30") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa30"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "灯光泛黄", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa31") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa31"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "灯光颜色不一致", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa32") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa32"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "灯光高度不一样", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa33") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa33"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "灯光高度不一致", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa34") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa34"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "灯光高度无法调节", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa35") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa35"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "灯壳开裂", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa36") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa36"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "灯带脱落", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa37") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa37"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "灯泡破损", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa38") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa38"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "灯罩模糊", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa39") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa39"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "灰尘", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa3a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa3a"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "灼伤", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa3b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa3b"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "点火开关失灵", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa3c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa3c"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "点烟器不工作", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa3d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa3d"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "烧损", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa3e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa3e"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "烧机油", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa3f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa3f"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "照度", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa40") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa40"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "煮水音", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa41") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa41"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "熄火", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa42") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa42"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "熄火后中央显示屏常亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa43") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa43"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "熔损", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa44") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa44"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "熔断", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa45") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa45"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "爆瓦", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa46") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa46"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "爆缸", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa47") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa47"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "爆胎", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa48") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa48"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "爆裂", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa49") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa49"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "版本低", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa4a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa4a"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "玻璃升降不良", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa4b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa4b"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "玻璃升降器倾斜", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa4c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa4c"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "玻璃升降器异响", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa4d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa4d"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "玻璃升降异常", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa4e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa4e"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "玻璃无法升降", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa4f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa4f"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "瑕疵", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa50") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa50"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "生产日期不一致", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa51") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa51"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "生锈", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa52") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa52"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "生锈卡滞", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa53") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa53"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "电压低", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa54") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa54"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "电池头氧化", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa55") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa55"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "电流音", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa56") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa56"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "画面模糊", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa57") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa57"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "痕迹", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa58") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa58"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "白屏", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa59") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa59"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "盲区显示不清晰", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa5a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa5a"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "盲区显示失灵", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa5b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa5b"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "看物体不清晰", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa5c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa5c"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "真皮发白", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa5d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa5d"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "真皮发黄", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa5e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa5e"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "真皮起皱异常", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa5f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa5f"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "石子敲击音", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa60") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa60"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "破损", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa61") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa61"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "破漆", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa62") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa62"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "破裂", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa63") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa63"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "破音", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa64") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa64"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "硬", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa65") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa65"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "磨损", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa66") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa66"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "磨损变形", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa67") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa67"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "磨损异常", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa68") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa68"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "磨损车漆", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa69") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa69"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "离地高度不等", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa6a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa6a"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "积水", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa6b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa6b"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "积碳", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa6c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa6c"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "积碳无响应", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa6d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa6d"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "空", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa6e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa6e"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "空调不制冷", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa6f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa6f"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "空调不工作", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa70") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa70"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "空调不良", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa71") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa71"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "空调出风口不出风", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa72") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa72"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "空调失灵", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa73") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa73"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "空调异常", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa74") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa74"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "空调控制面板失灵", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa75") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa75"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "空调控制面板无显示", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa76") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa76"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "空调控制面板背景灯不亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa77") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa77"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "空调无法启动", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa78") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa78"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "空调温度无法调节", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa79") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa79"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "空调自动开启", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa7a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa7a"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "空调面板背景灯不亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa7b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa7b"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "维修补偿", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa7c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa7c"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "维修质疑", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa7d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa7d"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "维修费用索赔", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa7e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa7e"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "缝线断裂", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa7f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa7f"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "缝隙不良", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa80") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa80"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "缝隙大", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa81") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa81"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "缺火", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa82") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa82"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "翘起", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa83") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa83"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "老化磨损", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa84") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa84"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "背景灯不亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa85") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa85"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "胎压不报警", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa86") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa86"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "胎压不显示", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa87") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa87"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "胎压灯亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa88") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa88"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "胎压灯点亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa89") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa89"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "胎压监测异常", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa8a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa8a"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "胎噪", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa8b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa8b"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "胎噪大", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa8c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa8c"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "胎噪音", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa8d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa8d"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "脚部无法调节", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa8e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa8e"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "脱出", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa8f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa8f"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "脱漆", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa90") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa90"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "脱皮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa91") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa91"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "脱线", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa92") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa92"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "脱落", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa93") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa93"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "腐蚀", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa94") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa94"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "自动下降", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa95") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa95"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "自动保存系统故障", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa96") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa96"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "自动关闭", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa97") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa97"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "自动刹车", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa98") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa98"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "自动开启", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa99") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa99"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "自动弹起", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa9a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa9a"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "自动感应失灵", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa9b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa9b"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "自动报警", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa9c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa9c"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "自动熄屏", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa9d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa9d"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "自动调节灵敏", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa9e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa9e"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "自动重启", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa9f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa9f"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "自展开", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaa0") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaa0"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "自燃", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaa1") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaa1"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "自爆", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaa2") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaa2"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "色差", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaa3") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaa3"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "艾力绅加速无力", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaa4") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaa4"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "节气门积碳", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaa5") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaa5"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "花屏", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaa6") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaa6"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "蓄电池亏电", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaa7") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaa7"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "蓝牙播放音乐有杂音", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaa8") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaa8"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "蓝牙播放音乐没有声音", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaa9") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaa9"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "蓝牙无法连接", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaaa") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaaa"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "蓝牙无法通话", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaab") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaab"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "蓝牙电话有杂音", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaac") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaac"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "蓝牙连接卡顿", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaad") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaad"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "蓝牙连接异常", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaae") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaae"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "蓝牙连接无声音", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaaf") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaaf"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "蓝牙连接时屏幕无字幕", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaab0") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaab0"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "蓝牙连接没有声音", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaab1") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaab1"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "蓝牙连接维修无法使用", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaab2") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaab2"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "薄", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaab3") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaab3"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "螺栓滑牙", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaab4") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaab4"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "行人保护系统作动", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaab5") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaab5"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "行李箱盖自动开启", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaab6") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaab6"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "行驶中熄火", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaab7") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaab7"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "行驶中熄火（缸内有缺火）", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaab8") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaab8"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "行驶异响", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaab9") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaab9"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "行驶熄火", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaba") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaba"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "表皮脱漆", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaabb") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaabb"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "表面不平", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaabc") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaabc"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "表面不平整", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaabd") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaabd"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "表面外伤", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaabe") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaabe"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "表面有裂口", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaabf") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaabf"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "衬套生锈", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaac0") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaac0"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "裂痕", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaac1") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaac1"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "裂纹", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaac2") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaac2"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "裂缝", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaac3") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaac3"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "褪色", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaac4") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaac4"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "褶皱", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaac5") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaac5"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "要求换单品", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaac6") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaac6"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "要求换总成", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaac7") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaac7"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "视线弯曲", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaac8") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaac8"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "解锁无反应", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaac9") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaac9"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "触摸屏无反应", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaca") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaca"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "误作动", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaacb") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaacb"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "误报警", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaacc") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaacc"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "调节不良", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaacd") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaacd"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "调节异响", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaace") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaace"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "调节异常", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaacf") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaacf"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "调节按钮失灵", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaad0") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaad0"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "调节故障", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaad1") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaad1"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "谐振器脱落", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaad2") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaad2"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "负极氧化", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaad3") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaad3"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "质疑", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaad4") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaad4"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "质量差", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaad5") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaad5"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "费用", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaad6") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaad6"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "起步慢", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaad7") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaad7"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "起步抖动", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaad8") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaad8"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "起步熄火", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaad9") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaad9"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "起步顿挫", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaada") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaada"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "起毛", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaadb") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaadb"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "起泡", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaadc") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaadc"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "起火", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaadd") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaadd"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "起皮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaade") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaade"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "起皱", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaadf") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaadf"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "起雾", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaae0") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaae0"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "跑偏", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaae1") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaae1"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "跳动", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaae2") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaae2"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "跳档", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaae3") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaae3"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "踩刹车异响", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaae4") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaae4"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "踩刹车抖动", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaae5") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaae5"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "踩油门发动机故障灯亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaae6") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaae6"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "踩油门异响", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaae7") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaae7"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "车内阅读灯不亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaae8") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaae8"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "车漆薄", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaae9") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaae9"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "车窗升降困难", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaea") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaea"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "车窗无法升降", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaeb") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaeb"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "车身薄", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaec") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaec"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "车辆启动困难", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaed") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaed"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "车辆无法启动", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaee") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaee"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "车辆无法行驶", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaef") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaef"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "车辆进老鼠咬断线束", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaf0") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaf0"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "车门不自动落锁", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaf1") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaf1"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "车门无法上锁", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaf2") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaf2"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "车门无法开启", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaf3") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaf3"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "车门无法解锁", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaf4") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaf4"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "车门自动解锁", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaf5") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaf5"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "车门锁止不良", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaf6") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaf6"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "车门锁止键失灵", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaf7") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaf7"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "车顶灯常亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaf8") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaf8"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "转不动", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaf9") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaf9"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "转向指示灯常亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaafa") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaafa"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "转向灯不亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaafb") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaafb"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "转向灯常亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaafc") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaafc"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "转向灯异常", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaafd") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaafd"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "转向灯闪", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaafe") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaafe"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "转向灯闪动频率快", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaff") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaff"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "转向灯闪烁", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab00") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab00"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "转过位", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab01") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab01"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "转速高", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab02") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab02"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "轮胎偏磨", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab03") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab03"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "轮胎异常磨损", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab04") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab04"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "轮胎磨损", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab05") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab05"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "软件升级", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab06") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab06"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "辅助驻车失效", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab07") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab07"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "边缘开裂", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab08") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab08"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "边缘腐蚀", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab09") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab09"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "过保损坏", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab0a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab0a"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "近光灯不亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab0b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab0b"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "近光灯漏光", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab0c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab0c"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "进昆虫", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab0d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab0d"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "进水", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab0e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab0e"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "进水爆缸", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab0f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab0f"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "进水维修", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab10") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab10"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "进液体", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab11") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab11"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "进虫子", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab12") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab12"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "远光无法关闭", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab13") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab13"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "远光灯不亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab14") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab14"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "远程启动异常", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab15") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab15"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "远近光无法关闭", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab16") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab16"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "遥控失灵", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab17") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab17"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "遥控钥匙失灵", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab18") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab18"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "部分功能失灵", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab19") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab19"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "配置少", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab1a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab1a"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "里程不显示", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab1b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab1b"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "里程数被锁死", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab1c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab1c"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "重复启动", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab1d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab1d"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "钢带断裂", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab1e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab1e"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "钥匙在车内自锁", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab1f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab1f"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "钥匙失灵", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab20") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab20"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "钥匙无法扣紧", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab21") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab21"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "锁不上", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab22") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab22"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "锁不住", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab23") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab23"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "锁死", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab24") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab24"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "锁车后屏幕常亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab25") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab25"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "锈点", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab26") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab26"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "锈蚀", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab27") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab27"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "锈迹", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab28") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab28"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "错位", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab29") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab29"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "错装", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab2a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab2a"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "镜片变色", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab2b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab2b"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "镜片无法调节", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab2c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab2c"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "镜片脱落", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab2d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab2d"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "门打不开", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab2e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab2e"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "门把手开关失灵", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab2f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab2f"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "门锁不上", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab30") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab30"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "门锁不良", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab31") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab31"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "闪屏", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab32") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab32"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "闪烁", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab33") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab33"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "闪退", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab34") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab34"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "问题", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab35") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab35"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "间隙", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab36") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab36"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "间隙不一致", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab37") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab37"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "间隙大", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab38") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab38"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "间隙左右差", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab39") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab39"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "阅读灯常亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab3a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab3a"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "防尘套漏油", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab3b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab3b"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "防盗自动报警", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab3c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab3c"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "防盗误报警", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab3d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab3d"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "降不到底", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab3e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab3e"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "降噪块脱落", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab3f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab3f"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "除雾不彻底", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab40") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab40"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "除雾功能失效", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab41") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab41"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "隔音差", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab42") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab42"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "雨刮一直作动", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab43") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab43"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "雨刮不良", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab44") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab44"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "雨刮常作动", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab45") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab45"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "雨刮无间歇档", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab46") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab46"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "雷达不工作", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab47") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab47"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "雾灯常亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab48") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab48"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "雾灯闪烁", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab49") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab49"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "非对象车收到召回书", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab4a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab4a"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "非对象车辆收到召回通知", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab4b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab4b"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "面板凸起", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab4c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab4c"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "音乐播放卡顿", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab4d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab4d"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "音响噪音大", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab4e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab4e"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "音响无声音", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab4f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab4f"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "音响有杂音", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab50") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab50"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "音响杂音", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab51") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab51"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "音响没有声音", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab52") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab52"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "音响系统不工作", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab53") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab53"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "音响自动重启", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab54") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab54"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "音响面板灯常亮", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab55") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab55"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "音色异常", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab56") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab56"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "音质差", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab57") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab57"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "音量无法调节", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab58") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab58"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "顿挫", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab59") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab59"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "频繁启动", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab5a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab5a"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "颜色不一样", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab5b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab5b"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "颜色异常", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab5c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab5c"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "风向不能完全调节", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab5d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab5d"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "风噪", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab5e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab5e"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "风扇无法关闭", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab5f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab5f"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "风速小", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab60") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab60"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "饰件开裂", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab61") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab61"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "饰件断裂", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab62") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab62"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "饰件脱漆", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab63") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab63"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "饰件脱落", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab64") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab64"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "饰件起泡", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab65") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab65"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "饰条变形", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab66") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab66"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "饰条翘起", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab67") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab67"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "饰条脱落", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab68") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab68"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "饰条起泡", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab69") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab69"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "饰板分离", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab6a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab6a"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "高度无法调节", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab6b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab6b"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "高温报警", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab6c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab6c"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "高温溶解", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab6d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab6d"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "高转速异响", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab6e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab6e"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "高速异响", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab6f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab6f"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "高速抖动", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab70") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab70"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "黄色污迹流出", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab71") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab71"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "黑屏", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab72") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab72"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "黑烟", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab73") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab73"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "鼓包", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });
    db.Categories.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab74") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab74"), "WorkspaceId": ObjectId("5fb3660530b3090001e0c6d8"), "Name": "龟裂", "CreatedBy": ObjectId("5fb5d373f03c2600017e34d0"), "CreatedAt": ISODate() }, { upsert: true });

    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7d3") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7d3"), "Name": "4档冒烟", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7d4") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7d4"), "Name": "ACCH不良", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7d5") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7d5"), "Name": "AC开关无电", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7d6") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7d6"), "Name": "DA屏常亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7d7") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7d7"), "Name": "DVD无法启动", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7d8") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7d8"), "Name": "EPB作动音", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7d9") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7d9"), "Name": "EPB异响", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7da") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7da"), "Name": "EPB作动音", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7db") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7db"), "Name": "H标开裂", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7dc") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7dc"), "Name": "LED灯脱落", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7dd") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7dd"), "Name": "USB无法充电", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7de") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7de"), "Name": "U盘无法读取", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7df") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7df"), "Name": "故障灯亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7e0") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7e0"), "Name": "故障灯亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7e1") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7e1"), "Name": "acuralink无法使用", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7e2") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7e2"), "Name": "acuralink无法正常使用", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7e3") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7e3"), "Name": "nan", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7e4") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7e4"), "Name": "上下无法调节", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7e5") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7e5"), "Name": "上下调节异响", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7e6") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7e6"), "Name": "上升反弹", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7e7") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7e7"), "Name": "上升回弹", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7e8") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7e8"), "Name": "上升缓慢", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7e9") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7e9"), "Name": "下垂", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7ea") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7ea"), "Name": "下调异响", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7eb") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7eb"), "Name": "下降异响", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7ec") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7ec"), "Name": "不亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7ed") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7ed"), "Name": "不作动", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7ee") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7ee"), "Name": "不保修", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7ef") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7ef"), "Name": "不充电", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7f0") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7f0"), "Name": "不出风", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7f1") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7f1"), "Name": "不制冷", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7f2") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7f2"), "Name": "不发电", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7f3") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7f3"), "Name": "不响", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7f4") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7f4"), "Name": "不回位", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7f5") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7f5"), "Name": "不回正", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7f6") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7f6"), "Name": "不对称", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7f7") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7f7"), "Name": "不对齐", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7f8") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7f8"), "Name": "不工作", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7f9") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7f9"), "Name": "不平", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7fa") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7fa"), "Name": "不报警", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7fb") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7fb"), "Name": "不正", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7fc") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7fc"), "Name": "不聚光", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7fd") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7fd"), "Name": "不能停止", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7fe") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7fe"), "Name": "不能前后移动", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa7ff") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa7ff"), "Name": "不能复位", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa800") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa800"), "Name": "不舒适", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa801") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa801"), "Name": "不良", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa802") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa802"), "Name": "不贴", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa803") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa803"), "Name": "不走车", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa804") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa804"), "Name": "与官网图不一致", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa805") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa805"), "Name": "与车身间隙大", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa806") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa806"), "Name": "中央显示屏不工作", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa807") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa807"), "Name": "中央显示屏不显示", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa808") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa808"), "Name": "中央显示屏按键失灵", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa809") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa809"), "Name": "中央显示屏无电", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa80a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa80a"), "Name": "中央显示屏黑屏", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa80b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa80b"), "Name": "中控台异响", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa80c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa80c"), "Name": "中控异响", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa80d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa80d"), "Name": "中部凹陷", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa80e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa80e"), "Name": "中间位置凹陷", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa80f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa80f"), "Name": "主控开关背景灯闪烁", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa810") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa810"), "Name": "主驾座椅开关失灵", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa811") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa811"), "Name": "乱码", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa812") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa812"), "Name": "亏电", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa813") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa813"), "Name": "亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa814") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa814"), "Name": "亮度不一样", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa815") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa815"), "Name": "仪表", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa816") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa816"), "Name": "仪表不显示", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa817") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa817"), "Name": "仪表亮度无法调节", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa818") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa818"), "Name": "仪表异常", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa819") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa819"), "Name": "仪表提示机油压力低故障", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa81a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa81a"), "Name": "仪表提示音异常", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa81b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa81b"), "Name": "仪表故障亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa81c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa81c"), "Name": "仪表故障灯", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa81d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa81d"), "Name": "仪表故障灯亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa81e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa81e"), "Name": "仪表无提示音", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa81f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa81f"), "Name": "仪表无显示", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa820") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa820"), "Name": "仪表显示不全", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa821") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa821"), "Name": "仪表氛围灯不亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa822") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa822"), "Name": "仪表温度显示不准", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa823") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa823"), "Name": "仪表温度显示异常", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa824") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa824"), "Name": "仪表灯不亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa825") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa825"), "Name": "仪表盘背景灯不亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa826") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa826"), "Name": "仪表胎压灯亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa827") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa827"), "Name": "仪表花屏", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa828") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa828"), "Name": "仪表里程不显示", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa829") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa829"), "Name": "仪表黑屏", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa82a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa82a"), "Name": "伤口", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa82b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa82b"), "Name": "位置设计不合理", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa82c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa82c"), "Name": "低", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa82d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa82d"), "Name": "低速抖动", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa82e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa82e"), "Name": "低速顿挫", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa82f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa82f"), "Name": "低频共振", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa830") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa830"), "Name": "作动不畅", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa831") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa831"), "Name": "作动不良", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa832") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa832"), "Name": "作动时有电流声", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa833") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa833"), "Name": "作动音", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa834") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa834"), "Name": "信号差", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa835") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa835"), "Name": "倒挡抖动", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa836") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa836"), "Name": "倒车影像左右线偏", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa837") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa837"), "Name": "倒车影像异常", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa838") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa838"), "Name": "倒车无影像", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa839") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa839"), "Name": "倒车时中控无反应", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa83a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa83a"), "Name": "倒车时刹车异响", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa83b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa83b"), "Name": "倒车灯不亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa83c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa83c"), "Name": "倒车灯常亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa83d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa83d"), "Name": "倒车雷达故障", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa83e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa83e"), "Name": "倾斜", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa83f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa83f"), "Name": "偏磨", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa840") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa840"), "Name": "偏移", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa841") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa841"), "Name": "做工差", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa842") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa842"), "Name": "充电不足", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa843") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa843"), "Name": "光晕", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa844") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa844"), "Name": "光线不聚焦", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa845") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa845"), "Name": "关不上", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa846") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa846"), "Name": "关不紧", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa847") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa847"), "Name": "关门力大", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa848") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa848"), "Name": "关闭时翘起", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa849") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa849"), "Name": "其他", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa84a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa84a"), "Name": "内侧生锈", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa84b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa84b"), "Name": "内侧裂开", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa84c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa84c"), "Name": "内部变形", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa84d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa84d"), "Name": "减配", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa84e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa84e"), "Name": "减震差", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa84f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa84f"), "Name": "凹凸不平", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa850") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa850"), "Name": "凹陷", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa851") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa851"), "Name": "出风异常", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa852") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa852"), "Name": "划伤", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa853") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa853"), "Name": "划痕", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa854") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa854"), "Name": "划痕多", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa855") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa855"), "Name": "刮不干净", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa856") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa856"), "Name": "制动异响", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa857") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa857"), "Name": "制动抖动", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa858") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa858"), "Name": "制动灯亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa859") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa859"), "Name": "刹车失灵", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa85a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa85a"), "Name": "刹车异响", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa85b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa85b"), "Name": "刹车抖动", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa85c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa85c"), "Name": "刹车灯不亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa85d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa85d"), "Name": "刹车灯常亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa85e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa85e"), "Name": "刹车硬", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa85f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa85f"), "Name": "前后异响", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa860") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa860"), "Name": "前后调节异响", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa861") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa861"), "Name": "前后调节时声音大", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa862") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa862"), "Name": "加改装导致不加热", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa863") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa863"), "Name": "加油不走", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa864") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa864"), "Name": "加油跳枪", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa865") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa865"), "Name": "加热异常", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa866") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa866"), "Name": "加装导航不显示", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa867") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa867"), "Name": "加速不良", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa868") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa868"), "Name": "加速受限", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa869") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa869"), "Name": "加速受限、机油增多", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa86a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa86a"), "Name": "加速延迟", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa86b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa86b"), "Name": "加速异响", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa86c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa86c"), "Name": "加速慢", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa86d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa86d"), "Name": "加速抖动", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa86e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa86e"), "Name": "加速无力", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa86f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa86f"), "Name": "加速顿挫", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa870") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa870"), "Name": "动力不足", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa871") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa871"), "Name": "升到顶反弹", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa872") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa872"), "Name": "升级后车辆无法启动", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa873") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa873"), "Name": "升起后挤压门饰板", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa874") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa874"), "Name": "升降不良", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa875") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa875"), "Name": "升降反弹", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa876") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa876"), "Name": "升降异常", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa877") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa877"), "Name": "升降慢", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa878") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa878"), "Name": "升降缓慢", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa879") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa879"), "Name": "单音", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa87a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa87a"), "Name": "卡扣损坏", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa87b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa87b"), "Name": "卡扣断裂", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa87c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa87c"), "Name": "卡扣脱落", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa87d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa87d"), "Name": "卡滞", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa87e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa87e"), "Name": "卡滞、机油增多", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa87f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa87f"), "Name": "卡滞、漏油", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa880") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa880"), "Name": "卡滞、顿挫", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa881") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa881"), "Name": "卡滞升级", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa882") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa882"), "Name": "卡滞软件升级", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa883") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa883"), "Name": "卡滞（服务投诉）", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa884") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa884"), "Name": "卡顿", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa885") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa885"), "Name": "印痕", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa886") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa886"), "Name": "印迹", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa887") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa887"), "Name": "参数异常", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa888") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa888"), "Name": "发动机故障灯亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa889") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa889"), "Name": "发卡", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa88a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa88a"), "Name": "发热", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa88b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa88b"), "Name": "发白", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa88c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa88c"), "Name": "发罩弹起", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa88d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa88d"), "Name": "发花", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa88e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa88e"), "Name": "发霉/泛白", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa88f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa88f"), "Name": "发黑", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa890") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa890"), "Name": "变形", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa891") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa891"), "Name": "变暗", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa892") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa892"), "Name": "变色", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa893") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa893"), "Name": "变色过保索赔", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa894") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa894"), "Name": "变速箱", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa895") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa895"), "Name": "口哨声", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa896") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa896"), "Name": "口哨音", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa897") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa897"), "Name": "召回", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa898") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa898"), "Name": "召回前费用", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa899") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa899"), "Name": "召回后二次不良", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa89a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa89a"), "Name": "召回后动力不足", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa89b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa89b"), "Name": "召回后抖动", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa89c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa89c"), "Name": "召回后油耗高", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa89d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa89d"), "Name": "召回后油表显示不正常", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa89e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa89e"), "Name": "召回后漏油", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa89f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa89f"), "Name": "召回后问题", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8a0") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8a0"), "Name": "召回安排", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8a1") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8a1"), "Name": "召回态度差", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8a2") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8a2"), "Name": "召回抱怨", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8a3") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8a3"), "Name": "召回更换新油泵后车辆无法启动", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8a4") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8a4"), "Name": "召回疑问", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8a5") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8a5"), "Name": "召回相关", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8a6") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8a6"), "Name": "召回等待时间长", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8a7") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8a7"), "Name": "召回维修后漏油", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8a8") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8a8"), "Name": "召回维修问题", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8a9") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8a9"), "Name": "召回补偿", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8aa") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8aa"), "Name": "召回费用", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ab") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ab"), "Name": "召回路程长", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ac") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ac"), "Name": "召回通知不及时", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ad") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ad"), "Name": "召回零件安排不当", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ae") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ae"), "Name": "右前油漆不良", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8af") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8af"), "Name": "吃胎", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8b0") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8b0"), "Name": "后备箱自动开启", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8b1") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8b1"), "Name": "后排温度高", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8b2") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8b2"), "Name": "后视镜折叠开关失灵", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8b3") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8b3"), "Name": "后视镜无法折叠", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8b4") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8b4"), "Name": "后视镜片无法调整", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8b5") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8b5"), "Name": "后视镜自动折叠", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8b6") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8b6"), "Name": "后视镜镜片无法调节", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8b7") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8b7"), "Name": "后轮抱死", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8b8") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8b8"), "Name": "后轮磨损", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8b9") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8b9"), "Name": "后雨刮自动工作", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ba") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ba"), "Name": "后雾灯常亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8bb") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8bb"), "Name": "后雾灯指示不良", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8bc") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8bc"), "Name": "吐槽", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8bd") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8bd"), "Name": "启停不工作", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8be") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8be"), "Name": "启停作动时异响", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8bf") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8bf"), "Name": "启动困难", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8c0") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8c0"), "Name": "启动异响", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8c1") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8c1"), "Name": "启动抖动", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8c2") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8c2"), "Name": "响声", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8c3") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8c3"), "Name": "响声大", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8c4") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8c4"), "Name": "啃胎", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8c5") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8c5"), "Name": "喇叭不响", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8c6") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8c6"), "Name": "喇叭长响", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8c7") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8c7"), "Name": "噪声大", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8c8") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8c8"), "Name": "噪音", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8c9") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8c9"), "Name": "噪音大", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ca") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ca"), "Name": "回位不良", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8cb") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8cb"), "Name": "地图不更新", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8cc") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8cc"), "Name": "地毯积水", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8cd") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8cd"), "Name": "坐起来不舒服", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ce") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ce"), "Name": "堵塞", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8cf") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8cf"), "Name": "塌陷", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8d0") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8d0"), "Name": "增多", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8d1") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8d1"), "Name": "声音大", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8d2") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8d2"), "Name": "声音小", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8d3") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8d3"), "Name": "壳体熔损", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8d4") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8d4"), "Name": "外伤", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8d5") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8d5"), "Name": "外壳变色", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8d6") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8d6"), "Name": "外观不良", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8d7") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8d7"), "Name": "多功能按键失灵", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8d8") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8d8"), "Name": "多功能按键开裂", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8d9") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8d9"), "Name": "多功能按键断裂", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8da") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8da"), "Name": "多功能按键破损", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8db") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8db"), "Name": "多功能按键脱漆", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8dc") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8dc"), "Name": "大灯不亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8dd") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8dd"), "Name": "大灯异常", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8de") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8de"), "Name": "大灯熔损", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8df") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8df"), "Name": "大灯闪烁", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8e0") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8e0"), "Name": "大灯高度无法调节", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8e1") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8e1"), "Name": "天窗无法打开", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8e2") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8e2"), "Name": "天窗无法正常开合", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8e3") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8e3"), "Name": "失去动力", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8e4") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8e4"), "Name": "失效", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8e5") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8e5"), "Name": "失灵", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8e6") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8e6"), "Name": "失速", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8e7") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8e7"), "Name": "安全性差", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8e8") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8e8"), "Name": "安全性质疑", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8e9") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8e9"), "Name": "安装不良", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ea") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ea"), "Name": "定速巡航无法使用", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8eb") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8eb"), "Name": "对象内", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ec") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ec"), "Name": "对象内，服务", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ed") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ed"), "Name": "对象内，补偿", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ee") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ee"), "Name": "对象内，费用", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ef") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ef"), "Name": "对象内，退车", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8f0") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8f0"), "Name": "对象内，通知错误", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8f1") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8f1"), "Name": "导航不准", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8f2") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8f2"), "Name": "导航定位不准", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8f3") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8f3"), "Name": "导航屏幕不亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8f4") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8f4"), "Name": "导航无法使用", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8f5") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8f5"), "Name": "导航闪屏", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8f6") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8f6"), "Name": "导轨饰板扣破损", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8f7") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8f7"), "Name": "小灯常亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8f8") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8f8"), "Name": "尾灯常亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8f9") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8f9"), "Name": "尾箱不灵敏", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8fa") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8fa"), "Name": "尾箱打不开", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8fb") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8fb"), "Name": "尾箱无法关闭", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8fc") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8fc"), "Name": "尾门一脚踢功能异常", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8fd") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8fd"), "Name": "尾门打不开", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8fe") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8fe"), "Name": "尾门无法一键上锁", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ff") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa8ff"), "Name": "尾门无法开启", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa900") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa900"), "Name": "尾门无法打开", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa901") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa901"), "Name": "尾门电机不作动", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa902") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa902"), "Name": "尾门电机异响", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa903") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa903"), "Name": "局部凹陷", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa904") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa904"), "Name": "屏幕有白点", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa905") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa905"), "Name": "屏幕漏光", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa906") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa906"), "Name": "工作音", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa907") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa907"), "Name": "左前扬声器不响", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa908") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa908"), "Name": "左前扬声器杂音", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa909") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa909"), "Name": "左右大灯亮度不一样", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa90a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa90a"), "Name": "左右大灯颜色不一样", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa90b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa90b"), "Name": "左右大灯颜色不一致", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa90c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa90c"), "Name": "左右差", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa90d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa90d"), "Name": "左右间隙不一致", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa90e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa90e"), "Name": "左转向指示灯常亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa90f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa90f"), "Name": "左转向灯常亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa910") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa910"), "Name": "差", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa911") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa911"), "Name": "常亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa912") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa912"), "Name": "座椅加热故障灯亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa913") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa913"), "Name": "座椅靠背无法调节", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa914") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa914"), "Name": "开关不灵敏", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa915") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa915"), "Name": "开关不畅", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa916") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa916"), "Name": "开关不良", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa917") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa917"), "Name": "开关失灵", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa918") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa918"), "Name": "开关拉手断裂", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa919") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa919"), "Name": "开关无法回位", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa91a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa91a"), "Name": "开县", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa91b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa91b"), "Name": "开启卡顿", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa91c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa91c"), "Name": "开启困难", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa91d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa91d"), "Name": "开启异响", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa91e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa91e"), "Name": "开启范围小", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa91f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa91f"), "Name": "开小灯天窗打开", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa920") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa920"), "Name": "开机慢", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa921") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa921"), "Name": "开空调有异物吹出", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa922") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa922"), "Name": "开线", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa923") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa923"), "Name": "开裂", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa924") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa924"), "Name": "开裂抖动", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa925") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa925"), "Name": "开门时灯不亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa926") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa926"), "Name": "异味", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa927") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa927"), "Name": "异响", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa928") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa928"), "Name": "异响、抖动", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa929") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa929"), "Name": "异响、顿挫", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa92a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa92a"), "Name": "异响（上铰链）", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa92b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa92b"), "Name": "异常", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa92c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa92c"), "Name": "异常作动", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa92d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa92d"), "Name": "异常磨损", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa92e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa92e"), "Name": "异物", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa92f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa92f"), "Name": "弹起", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa930") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa930"), "Name": "强度不足", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa931") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa931"), "Name": "怠速不稳", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa932") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa932"), "Name": "怠速中控异响", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa933") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa933"), "Name": "怠速启停失效", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa934") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa934"), "Name": "怠速异常", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa935") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa935"), "Name": "怠速抖动", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa936") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa936"), "Name": "怠速显示高", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa937") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa937"), "Name": "怠速起停不工作", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa938") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa938"), "Name": "恢复初始值", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa939") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa939"), "Name": "感应不良", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa93a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa93a"), "Name": "感应式雨刮失灵", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa93b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa93b"), "Name": "手刹放不了", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa93c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa93c"), "Name": "手刹无法解开", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa93d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa93d"), "Name": "手刹无法解除", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa93e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa93e"), "Name": "扎钉", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa93f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa93f"), "Name": "打不开", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa940") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa940"), "Name": "打不开门", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa941") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa941"), "Name": "打方向异响", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa942") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa942"), "Name": "打方向重", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa943") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa943"), "Name": "打滑", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa944") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa944"), "Name": "扬声器不响", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa945") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa945"), "Name": "扬声器无声音", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa946") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa946"), "Name": "扬声器有杂音", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa947") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa947"), "Name": "扬声器杂音", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa948") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa948"), "Name": "扬声器没声音", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa949") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa949"), "Name": "扶手卡不住", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa94a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa94a"), "Name": "扶手调整不良", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa94b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa94b"), "Name": "技术帖", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa94c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa94c"), "Name": "抓地力不足", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa94d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa94d"), "Name": "抖动", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa94e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa94e"), "Name": "抖动、熄火", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa94f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa94f"), "Name": "抖动异响", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa950") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa950"), "Name": "抖动故障灯亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa951") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa951"), "Name": "折叠过度", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa952") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa952"), "Name": "折皱", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa953") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa953"), "Name": "抛光蜡残留", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa954") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa954"), "Name": "拔不下来", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa955") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa955"), "Name": "拧不动", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa956") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa956"), "Name": "挂R挡抖动", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa957") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa957"), "Name": "挂不了档", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa958") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa958"), "Name": "挂倒挡异响", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa959") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa959"), "Name": "挂挡不走", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa95a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa95a"), "Name": "挂挡不走车", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa95b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa95b"), "Name": "挂挡异响", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa95c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa95c"), "Name": "挂挡无法走车", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa95d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa95d"), "Name": "挂挡熄火", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa95e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa95e"), "Name": "指示灯错误", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa95f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa95f"), "Name": "指针不回位", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa960") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa960"), "Name": "指针不良", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa961") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa961"), "Name": "指针异常复位", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa962") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa962"), "Name": "按不下去", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa963") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa963"), "Name": "按喇叭异响", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa964") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa964"), "Name": "按钮断裂", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa965") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa965"), "Name": "按钮无法调节", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa966") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa966"), "Name": "按钮脱落", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa967") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa967"), "Name": "按键不亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa968") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa968"), "Name": "按键卡滞", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa969") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa969"), "Name": "按键回位不良", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa96a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa96a"), "Name": "按键失灵", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa96b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa96b"), "Name": "按键开裂", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa96c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa96c"), "Name": "按键损坏", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa96d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa96d"), "Name": "按键断裂", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa96e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa96e"), "Name": "按键无反应", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa96f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa96f"), "Name": "按键无背景光", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa970") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa970"), "Name": "按键松动", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa971") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa971"), "Name": "按键灯不亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa972") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa972"), "Name": "按键破损", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa973") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa973"), "Name": "按键背光灯不亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa974") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa974"), "Name": "按键背景灯闪烁", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa975") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa975"), "Name": "按键脱漆", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa976") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa976"), "Name": "损坏", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa977") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa977"), "Name": "换挡抖动", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa978") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa978"), "Name": "换挡顿挫", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa979") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa979"), "Name": "掉漆", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa97a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa97a"), "Name": "掉皮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa97b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa97b"), "Name": "掉色", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa97c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa97c"), "Name": "接口处生锈", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa97d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa97d"), "Name": "播放音乐死机", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa97e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa97e"), "Name": "操作力大", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa97f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa97f"), "Name": "操作疑问", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa980") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa980"), "Name": "支撑问题", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa981") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa981"), "Name": "收放机不工作", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa982") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa982"), "Name": "收音机无法使用", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa983") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa983"), "Name": "收音机有杂音", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa984") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa984"), "Name": "改装导致失去动力", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa985") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa985"), "Name": "故障", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa986") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa986"), "Name": "故障灯亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa987") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa987"), "Name": "故障灯亮加速无力", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa988") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa988"), "Name": "故障灯亮，加速异常", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa989") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa989"), "Name": "故障灯异常", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa98a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa98a"), "Name": "故障码", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa98b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa98b"), "Name": "故障闪烁", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa98c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa98c"), "Name": "敏感", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa98d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa98d"), "Name": "断裂", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa98e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa98e"), "Name": "方向不正", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa98f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa98f"), "Name": "方向卡滞", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa990") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa990"), "Name": "方向卡滞升级", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa991") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa991"), "Name": "方向卡滞软件升级", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa992") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa992"), "Name": "方向发卡", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa993") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa993"), "Name": "方向盘多功能按键失灵", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa994") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa994"), "Name": "方向盘按键失灵", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa995") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa995"), "Name": "旋钮松动", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa996") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa996"), "Name": "无助力", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa997") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa997"), "Name": "无声音", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa998") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa998"), "Name": "无影像", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa999") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa999"), "Name": "无提示", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa99a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa99a"), "Name": "无提示音", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa99b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa99b"), "Name": "无显示", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa99c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa99c"), "Name": "无法上锁", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa99d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa99d"), "Name": "无法使用", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa99e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa99e"), "Name": "无法关闭", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa99f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa99f"), "Name": "无法升级", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9a0") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9a0"), "Name": "无法升降", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9a1") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9a1"), "Name": "无法启动", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9a2") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9a2"), "Name": "无法开启", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9a3") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9a3"), "Name": "无法开机", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9a4") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9a4"), "Name": "无法开闭", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9a5") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9a5"), "Name": "无法打开", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9a6") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9a6"), "Name": "无法折叠", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9a7") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9a7"), "Name": "无法拉出", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9a8") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9a8"), "Name": "无法换挡", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9a9") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9a9"), "Name": "无法断电", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9aa") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9aa"), "Name": "无法显示里程", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ab") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ab"), "Name": "无法熄火", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ac") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ac"), "Name": "无法自动升降", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ad") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ad"), "Name": "无法自动折叠", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ae") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ae"), "Name": "无法行驶", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9af") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9af"), "Name": "无法解锁", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9b0") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9b0"), "Name": "无法调节", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9b1") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9b1"), "Name": "无法转向", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9b2") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9b2"), "Name": "无法连接", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9b3") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9b3"), "Name": "无法锁车", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9b4") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9b4"), "Name": "无网络信号", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9b5") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9b5"), "Name": "无防夹功能", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9b6") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9b6"), "Name": "无阻尼", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9b7") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9b7"), "Name": "日行灯不亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9b8") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9b8"), "Name": "日行灯损坏", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9b9") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9b9"), "Name": "日行灯间隙性不亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ba") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ba"), "Name": "早期磨损", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9bb") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9bb"), "Name": "时钟不准", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9bc") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9bc"), "Name": "时间显示错误", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9bd") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9bd"), "Name": "显示不全", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9be") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9be"), "Name": "显示不准", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9bf") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9bf"), "Name": "显示不良", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9c0") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9c0"), "Name": "显示内容颠倒", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9c1") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9c1"), "Name": "显示屏不工作", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9c2") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9c2"), "Name": "显示屏外伤", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9c3") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9c3"), "Name": "显示屏支架断裂", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9c4") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9c4"), "Name": "显示屏黑屏", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9c5") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9c5"), "Name": "显示异常", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9c6") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9c6"), "Name": "显示颠倒", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9c7") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9c7"), "Name": "晃动", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9c8") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9c8"), "Name": "晃动音", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9c9") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9c9"), "Name": "晒伤", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ca") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ca"), "Name": "智能进入失效", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9cb") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9cb"), "Name": "智能进入失灵", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9cc") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9cc"), "Name": "智能钥匙失灵", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9cd") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9cd"), "Name": "暗电流大", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ce") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ce"), "Name": "曲轴磨损卡死", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9cf") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9cf"), "Name": "有凸点", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9d0") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9d0"), "Name": "有印迹", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9d1") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9d1"), "Name": "有反光点", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9d2") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9d2"), "Name": "有异物", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9d3") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9d3"), "Name": "有毛刺", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9d4") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9d4"), "Name": "有水渍", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9d5") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9d5"), "Name": "有水痕", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9d6") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9d6"), "Name": "有褶皱", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9d7") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9d7"), "Name": "有雪花状异物", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9d8") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9d8"), "Name": "有黄色污迹", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9d9") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9d9"), "Name": "有黄色污迹流出", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9da") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9da"), "Name": "有黄色油迹流出", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9db") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9db"), "Name": "未作动", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9dc") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9dc"), "Name": "未启动", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9dd") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9dd"), "Name": "未展开", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9de") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9de"), "Name": "未弹起", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9df") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9df"), "Name": "机油压力低", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9e0") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9e0"), "Name": "机油增多", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9e1") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9e1"), "Name": "机油增多、加速受限", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9e2") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9e2"), "Name": "机盖弹起", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9e3") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9e3"), "Name": "杂音", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9e4") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9e4"), "Name": "松动", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9e5") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9e5"), "Name": "松脱", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9e6") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9e6"), "Name": "档位抖动", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9e7") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9e7"), "Name": "档位无显示", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9e8") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9e8"), "Name": "模糊", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9e9") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9e9"), "Name": "正常", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ea") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ea"), "Name": "正常磨损", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9eb") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9eb"), "Name": "死机", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ec") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ec"), "Name": "段差", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ed") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ed"), "Name": "段差大", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ee") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ee"), "Name": "气囊壳破损", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ef") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ef"), "Name": "气囊灯亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9f0") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9f0"), "Name": "气囊皮磨损", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9f1") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9f1"), "Name": "气囊皮纹理不均匀", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9f2") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9f2"), "Name": "气囊皮起泡", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9f3") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9f3"), "Name": "气泡", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9f4") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9f4"), "Name": "氛围灯不亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9f5") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9f5"), "Name": "氧化", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9f6") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9f6"), "Name": "水温高", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9f7") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9f7"), "Name": "水纹", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9f8") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9f8"), "Name": "污迹", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9f9") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9f9"), "Name": "沙眼", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9fa") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9fa"), "Name": "油漆不良", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9fb") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9fb"), "Name": "油漆开裂", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9fc") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9fc"), "Name": "油漆污染", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9fd") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9fd"), "Name": "油漆腐蚀", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9fe") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9fe"), "Name": "油漆黄点", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ff") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baa9ff"), "Name": "油漆龟裂", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa00") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa00"), "Name": "油耗高", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa01") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa01"), "Name": "油表不准", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa02") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa02"), "Name": "油表显示不准", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa03") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa03"), "Name": "油表显示错误", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa04") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa04"), "Name": "油量确认", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa05") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa05"), "Name": "泄露", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa06") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa06"), "Name": "波浪纹", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa07") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa07"), "Name": "波纹", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa08") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa08"), "Name": "流痕", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa09") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa09"), "Name": "海绵塌陷", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa0a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa0a"), "Name": "消声块脱落", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa0b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa0b"), "Name": "清洗后故障", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa0c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa0c"), "Name": "渗油", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa0d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa0d"), "Name": "温度显示异常", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa0e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa0e"), "Name": "温度高", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa0f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa0f"), "Name": "温控制开关损坏", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa10") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa10"), "Name": "溢出", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa11") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa11"), "Name": "滑动门框掉漆", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa12") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa12"), "Name": "滑牙", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa13") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa13"), "Name": "滴水", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa14") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa14"), "Name": "滴水痕", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa15") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa15"), "Name": "漆斑", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa16") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa16"), "Name": "漆面不良", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa17") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa17"), "Name": "漆面腐蚀", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa18") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa18"), "Name": "漏光", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa19") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa19"), "Name": "漏冷却液", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa1a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa1a"), "Name": "漏冷媒", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa1b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa1b"), "Name": "漏气", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa1c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa1c"), "Name": "漏水", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa1d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa1d"), "Name": "漏汽油", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa1e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa1e"), "Name": "漏油", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa1f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa1f"), "Name": "漏油、顿挫", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa20") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa20"), "Name": "漏液", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa21") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa21"), "Name": "漏电", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa22") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa22"), "Name": "漏装", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa23") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa23"), "Name": "漏雪种", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa24") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa24"), "Name": "漏风", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa25") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa25"), "Name": "灯不亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa26") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa26"), "Name": "灯光不亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa27") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa27"), "Name": "灯光不聚焦", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa28") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa28"), "Name": "灯光亮度不一致", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa29") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa29"), "Name": "灯光偏暗", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa2a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa2a"), "Name": "灯光偏黄", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa2b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa2b"), "Name": "灯光发蓝", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa2c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa2c"), "Name": "灯光发黄", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa2d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa2d"), "Name": "灯光开关失灵", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa2e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa2e"), "Name": "灯光异常", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa2f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa2f"), "Name": "灯光暗", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa30") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa30"), "Name": "灯光泛黄", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa31") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa31"), "Name": "灯光颜色不一致", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa32") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa32"), "Name": "灯光高度不一样", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa33") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa33"), "Name": "灯光高度不一致", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa34") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa34"), "Name": "灯光高度无法调节", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa35") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa35"), "Name": "灯壳开裂", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa36") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa36"), "Name": "灯带脱落", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa37") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa37"), "Name": "灯泡破损", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa38") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa38"), "Name": "灯罩模糊", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa39") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa39"), "Name": "灰尘", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa3a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa3a"), "Name": "灼伤", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa3b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa3b"), "Name": "点火开关失灵", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa3c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa3c"), "Name": "点烟器不工作", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa3d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa3d"), "Name": "烧损", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa3e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa3e"), "Name": "烧机油", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa3f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa3f"), "Name": "照度", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa40") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa40"), "Name": "煮水音", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa41") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa41"), "Name": "熄火", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa42") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa42"), "Name": "熄火后中央显示屏常亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa43") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa43"), "Name": "熔损", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa44") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa44"), "Name": "熔断", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa45") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa45"), "Name": "爆瓦", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa46") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa46"), "Name": "爆缸", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa47") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa47"), "Name": "爆胎", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa48") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa48"), "Name": "爆裂", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa49") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa49"), "Name": "版本低", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa4a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa4a"), "Name": "玻璃升降不良", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa4b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa4b"), "Name": "玻璃升降器倾斜", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa4c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa4c"), "Name": "玻璃升降器异响", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa4d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa4d"), "Name": "玻璃升降异常", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa4e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa4e"), "Name": "玻璃无法升降", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa4f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa4f"), "Name": "瑕疵", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa50") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa50"), "Name": "生产日期不一致", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa51") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa51"), "Name": "生锈", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa52") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa52"), "Name": "生锈卡滞", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa53") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa53"), "Name": "电压低", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa54") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa54"), "Name": "电池头氧化", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa55") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa55"), "Name": "电流音", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa56") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa56"), "Name": "画面模糊", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa57") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa57"), "Name": "痕迹", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa58") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa58"), "Name": "白屏", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa59") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa59"), "Name": "盲区显示不清晰", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa5a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa5a"), "Name": "盲区显示失灵", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa5b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa5b"), "Name": "看物体不清晰", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa5c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa5c"), "Name": "真皮发白", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa5d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa5d"), "Name": "真皮发黄", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa5e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa5e"), "Name": "真皮起皱异常", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa5f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa5f"), "Name": "石子敲击音", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa60") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa60"), "Name": "破损", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa61") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa61"), "Name": "破漆", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa62") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa62"), "Name": "破裂", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa63") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa63"), "Name": "破音", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa64") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa64"), "Name": "硬", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa65") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa65"), "Name": "磨损", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa66") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa66"), "Name": "磨损变形", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa67") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa67"), "Name": "磨损异常", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa68") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa68"), "Name": "磨损车漆", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa69") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa69"), "Name": "离地高度不等", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa6a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa6a"), "Name": "积水", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa6b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa6b"), "Name": "积碳", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa6c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa6c"), "Name": "积碳无响应", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa6d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa6d"), "Name": "空", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa6e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa6e"), "Name": "空调不制冷", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa6f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa6f"), "Name": "空调不工作", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa70") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa70"), "Name": "空调不良", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa71") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa71"), "Name": "空调出风口不出风", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa72") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa72"), "Name": "空调失灵", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa73") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa73"), "Name": "空调异常", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa74") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa74"), "Name": "空调控制面板失灵", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa75") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa75"), "Name": "空调控制面板无显示", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa76") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa76"), "Name": "空调控制面板背景灯不亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa77") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa77"), "Name": "空调无法启动", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa78") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa78"), "Name": "空调温度无法调节", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa79") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa79"), "Name": "空调自动开启", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa7a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa7a"), "Name": "空调面板背景灯不亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa7b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa7b"), "Name": "维修补偿", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa7c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa7c"), "Name": "维修质疑", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa7d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa7d"), "Name": "维修费用索赔", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa7e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa7e"), "Name": "缝线断裂", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa7f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa7f"), "Name": "缝隙不良", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa80") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa80"), "Name": "缝隙大", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa81") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa81"), "Name": "缺火", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa82") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa82"), "Name": "翘起", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa83") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa83"), "Name": "老化磨损", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa84") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa84"), "Name": "背景灯不亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa85") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa85"), "Name": "胎压不报警", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa86") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa86"), "Name": "胎压不显示", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa87") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa87"), "Name": "胎压灯亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa88") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa88"), "Name": "胎压灯点亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa89") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa89"), "Name": "胎压监测异常", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa8a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa8a"), "Name": "胎噪", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa8b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa8b"), "Name": "胎噪大", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa8c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa8c"), "Name": "胎噪音", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa8d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa8d"), "Name": "脚部无法调节", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa8e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa8e"), "Name": "脱出", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa8f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa8f"), "Name": "脱漆", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa90") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa90"), "Name": "脱皮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa91") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa91"), "Name": "脱线", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa92") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa92"), "Name": "脱落", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa93") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa93"), "Name": "腐蚀", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa94") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa94"), "Name": "自动下降", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa95") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa95"), "Name": "自动保存系统故障", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa96") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa96"), "Name": "自动关闭", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa97") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa97"), "Name": "自动刹车", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa98") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa98"), "Name": "自动开启", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa99") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa99"), "Name": "自动弹起", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa9a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa9a"), "Name": "自动感应失灵", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa9b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa9b"), "Name": "自动报警", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa9c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa9c"), "Name": "自动熄屏", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa9d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa9d"), "Name": "自动调节灵敏", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa9e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa9e"), "Name": "自动重启", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaa9f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaa9f"), "Name": "自展开", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaa0") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaa0"), "Name": "自燃", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaa1") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaa1"), "Name": "自爆", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaa2") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaa2"), "Name": "色差", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaa3") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaa3"), "Name": "艾力绅加速无力", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaa4") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaa4"), "Name": "节气门积碳", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaa5") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaa5"), "Name": "花屏", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaa6") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaa6"), "Name": "蓄电池亏电", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaa7") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaa7"), "Name": "蓝牙播放音乐有杂音", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaa8") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaa8"), "Name": "蓝牙播放音乐没有声音", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaa9") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaa9"), "Name": "蓝牙无法连接", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaaa") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaaa"), "Name": "蓝牙无法通话", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaab") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaab"), "Name": "蓝牙电话有杂音", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaac") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaac"), "Name": "蓝牙连接卡顿", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaad") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaad"), "Name": "蓝牙连接异常", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaae") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaae"), "Name": "蓝牙连接无声音", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaaf") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaaf"), "Name": "蓝牙连接时屏幕无字幕", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaab0") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaab0"), "Name": "蓝牙连接没有声音", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaab1") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaab1"), "Name": "蓝牙连接维修无法使用", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaab2") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaab2"), "Name": "薄", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaab3") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaab3"), "Name": "螺栓滑牙", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaab4") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaab4"), "Name": "行人保护系统作动", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaab5") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaab5"), "Name": "行李箱盖自动开启", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaab6") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaab6"), "Name": "行驶中熄火", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaab7") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaab7"), "Name": "行驶中熄火（缸内有缺火）", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaab8") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaab8"), "Name": "行驶异响", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaab9") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaab9"), "Name": "行驶熄火", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaba") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaba"), "Name": "表皮脱漆", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaabb") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaabb"), "Name": "表面不平", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaabc") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaabc"), "Name": "表面不平整", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaabd") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaabd"), "Name": "表面外伤", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaabe") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaabe"), "Name": "表面有裂口", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaabf") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaabf"), "Name": "衬套生锈", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaac0") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaac0"), "Name": "裂痕", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaac1") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaac1"), "Name": "裂纹", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaac2") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaac2"), "Name": "裂缝", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaac3") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaac3"), "Name": "褪色", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaac4") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaac4"), "Name": "褶皱", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaac5") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaac5"), "Name": "要求换单品", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaac6") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaac6"), "Name": "要求换总成", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaac7") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaac7"), "Name": "视线弯曲", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaac8") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaac8"), "Name": "解锁无反应", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaac9") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaac9"), "Name": "触摸屏无反应", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaca") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaca"), "Name": "误作动", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaacb") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaacb"), "Name": "误报警", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaacc") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaacc"), "Name": "调节不良", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaacd") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaacd"), "Name": "调节异响", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaace") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaace"), "Name": "调节异常", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaacf") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaacf"), "Name": "调节按钮失灵", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaad0") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaad0"), "Name": "调节故障", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaad1") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaad1"), "Name": "谐振器脱落", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaad2") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaad2"), "Name": "负极氧化", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaad3") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaad3"), "Name": "质疑", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaad4") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaad4"), "Name": "质量差", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaad5") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaad5"), "Name": "费用", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaad6") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaad6"), "Name": "起步慢", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaad7") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaad7"), "Name": "起步抖动", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaad8") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaad8"), "Name": "起步熄火", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaad9") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaad9"), "Name": "起步顿挫", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaada") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaada"), "Name": "起毛", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaadb") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaadb"), "Name": "起泡", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaadc") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaadc"), "Name": "起火", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaadd") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaadd"), "Name": "起皮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaade") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaade"), "Name": "起皱", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaadf") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaadf"), "Name": "起雾", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaae0") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaae0"), "Name": "跑偏", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaae1") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaae1"), "Name": "跳动", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaae2") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaae2"), "Name": "跳档", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaae3") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaae3"), "Name": "踩刹车异响", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaae4") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaae4"), "Name": "踩刹车抖动", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaae5") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaae5"), "Name": "踩油门发动机故障灯亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaae6") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaae6"), "Name": "踩油门异响", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaae7") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaae7"), "Name": "车内阅读灯不亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaae8") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaae8"), "Name": "车漆薄", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaae9") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaae9"), "Name": "车窗升降困难", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaea") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaea"), "Name": "车窗无法升降", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaeb") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaeb"), "Name": "车身薄", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaec") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaec"), "Name": "车辆启动困难", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaed") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaed"), "Name": "车辆无法启动", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaee") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaee"), "Name": "车辆无法行驶", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaef") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaef"), "Name": "车辆进老鼠咬断线束", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaf0") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaf0"), "Name": "车门不自动落锁", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaf1") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaf1"), "Name": "车门无法上锁", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaf2") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaf2"), "Name": "车门无法开启", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaf3") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaf3"), "Name": "车门无法解锁", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaf4") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaf4"), "Name": "车门自动解锁", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaf5") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaf5"), "Name": "车门锁止不良", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaf6") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaf6"), "Name": "车门锁止键失灵", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaf7") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaf7"), "Name": "车顶灯常亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaf8") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaf8"), "Name": "转不动", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaf9") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaf9"), "Name": "转向指示灯常亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaafa") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaafa"), "Name": "转向灯不亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaafb") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaafb"), "Name": "转向灯常亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaafc") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaafc"), "Name": "转向灯异常", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaafd") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaafd"), "Name": "转向灯闪", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaafe") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaafe"), "Name": "转向灯闪动频率快", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baaaff") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baaaff"), "Name": "转向灯闪烁", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab00") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab00"), "Name": "转过位", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab01") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab01"), "Name": "转速高", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab02") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab02"), "Name": "轮胎偏磨", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab03") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab03"), "Name": "轮胎异常磨损", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab04") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab04"), "Name": "轮胎磨损", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab05") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab05"), "Name": "软件升级", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab06") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab06"), "Name": "辅助驻车失效", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab07") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab07"), "Name": "边缘开裂", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab08") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab08"), "Name": "边缘腐蚀", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab09") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab09"), "Name": "过保损坏", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab0a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab0a"), "Name": "近光灯不亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab0b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab0b"), "Name": "近光灯漏光", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab0c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab0c"), "Name": "进昆虫", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab0d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab0d"), "Name": "进水", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab0e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab0e"), "Name": "进水爆缸", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab0f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab0f"), "Name": "进水维修", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab10") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab10"), "Name": "进液体", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab11") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab11"), "Name": "进虫子", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab12") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab12"), "Name": "远光无法关闭", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab13") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab13"), "Name": "远光灯不亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab14") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab14"), "Name": "远程启动异常", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab15") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab15"), "Name": "远近光无法关闭", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab16") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab16"), "Name": "遥控失灵", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab17") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab17"), "Name": "遥控钥匙失灵", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab18") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab18"), "Name": "部分功能失灵", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab19") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab19"), "Name": "配置少", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab1a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab1a"), "Name": "里程不显示", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab1b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab1b"), "Name": "里程数被锁死", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab1c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab1c"), "Name": "重复启动", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab1d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab1d"), "Name": "钢带断裂", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab1e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab1e"), "Name": "钥匙在车内自锁", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab1f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab1f"), "Name": "钥匙失灵", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab20") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab20"), "Name": "钥匙无法扣紧", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab21") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab21"), "Name": "锁不上", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab22") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab22"), "Name": "锁不住", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab23") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab23"), "Name": "锁死", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab24") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab24"), "Name": "锁车后屏幕常亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab25") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab25"), "Name": "锈点", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab26") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab26"), "Name": "锈蚀", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab27") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab27"), "Name": "锈迹", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab28") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab28"), "Name": "错位", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab29") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab29"), "Name": "错装", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab2a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab2a"), "Name": "镜片变色", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab2b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab2b"), "Name": "镜片无法调节", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab2c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab2c"), "Name": "镜片脱落", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab2d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab2d"), "Name": "门打不开", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab2e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab2e"), "Name": "门把手开关失灵", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab2f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab2f"), "Name": "门锁不上", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab30") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab30"), "Name": "门锁不良", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab31") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab31"), "Name": "闪屏", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab32") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab32"), "Name": "闪烁", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab33") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab33"), "Name": "闪退", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab34") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab34"), "Name": "问题", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab35") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab35"), "Name": "间隙", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab36") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab36"), "Name": "间隙不一致", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab37") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab37"), "Name": "间隙大", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab38") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab38"), "Name": "间隙左右差", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab39") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab39"), "Name": "阅读灯常亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab3a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab3a"), "Name": "防尘套漏油", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab3b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab3b"), "Name": "防盗自动报警", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab3c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab3c"), "Name": "防盗误报警", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab3d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab3d"), "Name": "降不到底", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab3e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab3e"), "Name": "降噪块脱落", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab3f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab3f"), "Name": "除雾不彻底", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab40") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab40"), "Name": "除雾功能失效", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab41") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab41"), "Name": "隔音差", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab42") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab42"), "Name": "雨刮一直作动", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab43") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab43"), "Name": "雨刮不良", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab44") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab44"), "Name": "雨刮常作动", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab45") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab45"), "Name": "雨刮无间歇档", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab46") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab46"), "Name": "雷达不工作", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab47") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab47"), "Name": "雾灯常亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab48") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab48"), "Name": "雾灯闪烁", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab49") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab49"), "Name": "非对象车收到召回书", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab4a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab4a"), "Name": "非对象车辆收到召回通知", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab4b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab4b"), "Name": "面板凸起", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab4c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab4c"), "Name": "音乐播放卡顿", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab4d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab4d"), "Name": "音响噪音大", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab4e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab4e"), "Name": "音响无声音", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab4f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab4f"), "Name": "音响有杂音", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab50") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab50"), "Name": "音响杂音", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab51") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab51"), "Name": "音响没有声音", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab52") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab52"), "Name": "音响系统不工作", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab53") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab53"), "Name": "音响自动重启", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab54") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab54"), "Name": "音响面板灯常亮", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab55") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab55"), "Name": "音色异常", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab56") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab56"), "Name": "音质差", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab57") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab57"), "Name": "音量无法调节", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab58") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab58"), "Name": "顿挫", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab59") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab59"), "Name": "频繁启动", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab5a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab5a"), "Name": "颜色不一样", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab5b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab5b"), "Name": "颜色异常", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab5c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab5c"), "Name": "风向不能完全调节", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab5d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab5d"), "Name": "风噪", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab5e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab5e"), "Name": "风扇无法关闭", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab5f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab5f"), "Name": "风速小", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab60") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab60"), "Name": "饰件开裂", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab61") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab61"), "Name": "饰件断裂", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab62") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab62"), "Name": "饰件脱漆", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab63") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab63"), "Name": "饰件脱落", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab64") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab64"), "Name": "饰件起泡", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab65") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab65"), "Name": "饰条变形", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab66") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab66"), "Name": "饰条翘起", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab67") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab67"), "Name": "饰条脱落", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab68") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab68"), "Name": "饰条起泡", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab69") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab69"), "Name": "饰板分离", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab6a") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab6a"), "Name": "高度无法调节", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab6b") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab6b"), "Name": "高温报警", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab6c") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab6c"), "Name": "高温溶解", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab6d") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab6d"), "Name": "高转速异响", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab6e") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab6e"), "Name": "高速异响", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab6f") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab6f"), "Name": "高速抖动", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab70") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab70"), "Name": "黄色污迹流出", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab71") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab71"), "Name": "黑屏", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab72") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab72"), "Name": "黑烟", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab73") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab73"), "Name": "鼓包", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    db.Honda_Syndromes.update({ "_id": ObjectId("5fd6e4a2c8cecc87c4baab74") }, { "_id": ObjectId("5fd6e4a2c8cecc87c4baab74"), "Name": "龟裂", "BadGrade": "C", "CreatedAt": ISODate() }, { upsert: true });
    

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
                "ExecutorName": "HondaDataAnnotator",
                "Ignore": false
            }
        }, {
            "ActionId": "DataConsumption",
            "Description": "Consume data by labeling tool.",
            "IsDefault": false,
            "Options": {
                "ExecutorName": "HondaBusinessDataConsumer",
                "Ignore": false,
                "Consumers": 2
            }
        }],
        "LastRunAt": null
    }, { upsert: true });

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

    db.ScheduledJobs.update({ "_id": "2a8b8a89-8e2f-4895-b5df-25379b24696d" }, {
        "_id": "2a8b8a89-8e2f-4895-b5df-25379b24696d",
        "Name": "Warning-Calculator",
        "JobType": "WarningCalculator",
        "Description": "Calculate warning task.",
        "Schedule": "0 0 16 * * ?",
        "Actions": [{
            "ActionId": "CollectVehicleFaultDataAction",
            "Description": "Collect vehicle fault data from master db.",
            "IsDefault": true,
            "Options": {
                "ExecutorName": "VehicleFaultDataCollector",
                "Ignore": false
            }
        }, {
            "ActionId": "LinkRelatedDataAction",
            "Description": "Link related data to vehicle fault data, like QIC/QIS.",
            "IsDefault": false,
            "Options": {
                "ExecutorName": "RelatedDataLinker",
                "Ignore": false
            }
        }, {
            "ActionId": "CalculateWarningAction",
            "Description": "Calculate warning from vehicle fault data and related data.",
            "IsDefault": false,
            "Options": {
                "ExecutorName": "WarningRecordCalculator",
                "Ignore": false
            }
        }, {
            "ActionId": "SaveCalculationResultAction",
            "Description": "Save warning records to master database.",
            "IsDefault": false,
            "Options": {
                "ExecutorName": "CalculationResultSaver",
                "Ignore": false
            }
        }, {
            "ActionId": "AnnounceWarningAction",
            "Description": "Send warning emails for this task.",
            "IsDefault": false,
            "Options": {
                "ExecutorName": "WarningAnnouncer",
                "Ignore": false
            }
        }],
        "LastRunAt": null
    }, { upsert: true });

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

    db.ScheduledJobTasks.update({ "_id": "4e27e221-acf1-401b-9d5a-0315d3ed7da4" }, {
        "_id": "4e27e221-acf1-401b-9d5a-0315d3ed7da4",
        "JobId": "76def8ec-1a30-4890-ab72-c8d56f27dde8",
        "Name": "MQIToLabeling",
        "Description": "Translate MQI data from raw database (mariadb) to labeling tool.",
        "IsRunOnce": false,
        "StartTime": null,
        "EndTime": null,
        "Executor": {
            "UserName": "honda_manager",
            "Password": "honda123",
            "Domain": null
        },
        "MerchantCreationTime": ISODate(),
        "ManagedAt": ISODate(),
        "ManagedBy": null,
        "IsDeleted": false,
        "TaskDefine": {
            "DataSourceName": "MQI",
            "WorkspaceId": "5fb3660530b3090001e0c6d8",
            "DBSettings": {
                "ConnectionString": "Server=10.110.101.52;Port=3306;User ID=mqpaiopr;Password=mqpaighac;Database=mqpai_db",
                "Database": "",
                "TableName": "RawMQIData",
                "TimestampFieldName": "Timestamp"
            },
            "DataConsumerSettings": {
                "TagFeatures": [],
                "DateTimeFormat": "yyyyMMddHHmmss",
                "ContentType": "application/json",
                "ItemsPerDocument": 50
            },
            "DataAnnotatorSettings": {
                "AnnotationServiceEndpoint": "NotUsed",
                "ModelVersion": "",
                "ItemsCountPerRequest": 20
            }
        },
        "Status": {
            "LastStatus": "准备中",
            "LastRunAt": null,
            "LastFinishedAt": null,
            "LastSucceedAt": null,
            "TryTimes": 0
        }
    }, { upsert: true });

    db.ScheduledJobTasks.update({ "_id": "1afa267a-d22a-4841-899d-ae35ef8d399c" }, {
        "_id": "1afa267a-d22a-4841-899d-ae35ef8d399c",
        "JobId": "76def8ec-1a30-4890-ab72-c8d56f27dde8",
        "Name": "TechSupportToLabeling",
        "Description": "Translate Tech support data from raw database (mariadb) to labeling tool.",
        "IsRunOnce": false,
        "StartTime": null,
        "EndTime": null,
        "Executor": {
            "UserName": "honda_manager",
            "Password": "honda123",
            "Domain": null
        },
        "MerchantCreationTime": ISODate(),
        "ManagedAt": ISODate(),
        "ManagedBy": null,
        "IsDeleted": false,
        "TaskDefine": {
            "DataSourceName": "TECH_CONSULTING",
            "WorkspaceId": "5fb3660530b3090001e0c6d8",
            "DBSettings": {
                "ConnectionString": "Server=10.110.101.52;Port=3306;User ID=mqpaiopr;Password=mqpaighac;Database=mqpai_db",
                "Database": "",
                "TableName": "RawTechSupportData",
                "TimestampFieldName": "Timestamp"
            },
            "DataConsumerSettings": {
                "TagFeatures": [],
                "DateTimeFormat": "yyyyMMddHHmmss",
                "ContentType": "application/json",
                "ItemsPerDocument": 50
            },
            "DataAnnotatorSettings": {
                "AnnotationServiceEndpoint": "NotUsed",
                "ModelVersion": "",
                "ItemsCountPerRequest": 20
            }
        },
        "Status": {
            "LastStatus": "准备中",
            "LastRunAt": null,
            "LastFinishedAt": null,
            "LastSucceedAt": null,
            "TryTimes": 0
        }
    }, { upsert: true });

    db.ScheduledJobTasks.update({ "_id": "1964e109-67f9-4d97-accc-3e50db4cd862" }, {
        "_id": "1964e109-67f9-4d97-accc-3e50db4cd862",
        "JobId": "76def8ec-1a30-4890-ab72-c8d56f27dde8",
        "Name": "HotlineToLabeling",
        "Description": "Translate 800 data from raw database (mariadb) to labeling tool.",
        "IsRunOnce": false,
        "StartTime": null,
        "EndTime": null,
        "Executor": {
            "UserName": "honda_manager",
            "Password": "honda123",
            "Domain": null
        },
        "MerchantCreationTime": ISODate(),
        "ManagedAt": ISODate(),
        "ManagedBy": null,
        "IsDeleted": false,
        "TaskDefine": {
            "DataSourceName": "HOTLINE",
            "WorkspaceId": "5fb3660530b3090001e0c6d8",
            "DBSettings": {
                "ConnectionString": "Server=10.110.101.52;Port=3306;User ID=mqpaiopr;Password=mqpaighac;Database=mqpai_db",
                "Database": "",
                "TableName": "RawHotlineData",
                "TimestampFieldName": "Timestamp"
            },
            "DataConsumerSettings": {
                "TagFeatures": [],
                "DateTimeFormat": "yyyyMMddHHmmss",
                "ContentType": "application/json",
                "ItemsPerDocument": 50
            },
            "DataAnnotatorSettings": {
                "AnnotationServiceEndpoint": "NotUsed",
                "ModelVersion": "",
                "ItemsCountPerRequest": 20
            }
        },
        "Status": {
            "LastStatus": "准备中",
            "LastRunAt": null,
            "LastFinishedAt": null,
            "LastSucceedAt": null,
            "TryTimes": 0
        }
    }, { upsert: true });

    db.ScheduledJobTasks.update({ "_id": "aa82a260-3117-4bb6-b0b1-5eff5198a338" }, {
        "_id": "aa82a260-3117-4bb6-b0b1-5eff5198a338",
        "JobId": "07270eb6-aeaa-490d-82b2-b565ae6c80e4",
        "Name": "GOV-Realtime",
        "Description": "Translate GOV data from mongo db to labeling tool.",
        "IsRunOnce": false,
        "StartTime": null,
        "EndTime": null,
        "Executor": {
            "UserName": "honda_manager",
            "Password": "honda123",
            "Domain": null
        },
        "MerchantCreationTime": ISODate(),
        "ManagedAt": ISODate(),
        "ManagedBy": null,
        "IsDeleted": false,
        "TaskDefine": {
            "DataSourceName": "GOV",
            "WorkspaceId": "5fb3660530b3090001e0c6d8",
            "DBSettings": {
                "ConnectionString": "mongodb://mongodb:27017",
                "Database": "HondaStaging",
                "TableName": "RawGOVData",
                "TimestampFieldName": "Timestamp"
            },
            "DataConsumerSettings": {
                "TagFeatures": [],
                "DateTimeFormat": "yyyyMMddHHmmss",
                "ContentType": "application/json",
                "ItemsPerDocument": 50
            },
            "DataAnnotatorSettings": {
                "AnnotationServiceEndpoint": "NotUsed",
                "ModelVersion": "",
                "ItemsCountPerRequest": 20
            }
        },
        "Status": {
            "LastStatus": "准备中",
            "LastRunAt": null,
            "LastFinishedAt": null,
            "LastSucceedAt": null,
            "TryTimes": 0
        }
    }, { upsert: true });

    db.ScheduledJobTasks.update({ "_id": "b7804de1-54ac-48f7-8706-1fbeaaf779ba" }, {
        "_id": "b7804de1-54ac-48f7-8706-1fbeaaf779ba",
        "JobId": "07270eb6-aeaa-490d-82b2-b565ae6c80e4",
        "Name": "MEDIAMAIN-Realtime",
        "Description": "Translate Media Main data from mongo db to labeling tool.",
        "IsRunOnce": false,
        "StartTime": null,
        "EndTime": null,
        "Executor": {
            "UserName": "honda_manager",
            "Password": "honda123",
            "Domain": null
        },
        "MerchantCreationTime": ISODate(),
        "ManagedAt": ISODate(),
        "ManagedBy": null,
        "IsDeleted": false,
        "TaskDefine": {
            "DataSourceName": "MEDIA_MAIN",
            "WorkspaceId": "5fb3660530b3090001e0c6d8",
            "DBSettings": {
                "ConnectionString": "mongodb://mongodb:27017",
                "Database": "HondaStaging",
                "TableName": "RawMediaMainData",
                "TimestampFieldName": "Timestamp"
            },
            "DataConsumerSettings": {
                "TagFeatures": [],
                "DateTimeFormat": "yyyyMMddHHmmss",
                "ContentType": "application/json",
                "ItemsPerDocument": 50
            },
            "DataAnnotatorSettings": {
                "AnnotationServiceEndpoint": "NotUsed",
                "ModelVersion": "",
                "ItemsCountPerRequest": 20
            }
        },
        "Status": {
            "LastStatus": "准备中",
            "LastRunAt": null,
            "LastFinishedAt": null,
            "LastSucceedAt": null,
            "TryTimes": 0
        }
    }, { upsert: true });

    db.ScheduledJobTasks.update({ "_id": "fab33e76-ab60-4f67-8eee-44112c4e69a6" }, {
        "_id": "fab33e76-ab60-4f67-8eee-44112c4e69a6",
        "JobId": "07270eb6-aeaa-490d-82b2-b565ae6c80e4",
        "Name": "MediaSub-Realtime",
        "Description": "Translate Media Sub data from mongo db to labeling tool.",
        "IsRunOnce": false,
        "StartTime": null,
        "EndTime": null,
        "Executor": {
            "UserName": "honda_manager",
            "Password": "honda123",
            "Domain": null
        },
        "MerchantCreationTime": ISODate(),
        "ManagedAt": ISODate(),
        "ManagedBy": null,
        "IsDeleted": false,
        "TaskDefine": {
            "DataSourceName": "MEDIA_SUB",
            "WorkspaceId": "5fb3660530b3090001e0c6d8",
            "DBSettings": {
                "ConnectionString": "mongodb://mongodb:27017",
                "Database": "HondaStaging",
                "TableName": "RawMediaSubData",
                "TimestampFieldName": "Timestamp"
            },
            "DataConsumerSettings": {
                "TagFeatures": [],
                "DateTimeFormat": "yyyyMMddHHmmss",
                "ContentType": "application/json",
                "ItemsPerDocument": 50
            },
            "DataAnnotatorSettings": {
                "AnnotationServiceEndpoint": "NotUsed",
                "ModelVersion": "",
                "ItemsCountPerRequest": 20
            }
        },
        "Status": {
            "LastStatus": "准备中",
            "LastRunAt": null,
            "LastFinishedAt": null,
            "LastSucceedAt": null,
            "TryTimes": 0
        }
    }, { upsert: true });

    db.ScheduledJobTasks.update({ "_id": "44ad3e66-fcba-41c1-97b7-f36ca8424d52" }, {
        "_id": "44ad3e66-fcba-41c1-97b7-f36ca8424d52",
        "JobId": "51682d04-bbcc-4c35-84d6-375d56f6fa5e",
        "Name": "ResultHandler",
        "Description": "Handle labeling result.",
        "IsRunOnce": false,
        "StartTime": null,
        "EndTime": null,
        "Executor": {
            "UserName": "honda_manager",
            "Password": "honda123",
            "Domain": null
        },
        "MerchantCreationTime": ISODate("2021-03-08T03:54:48.487Z"),
        "ManagedAt": ISODate("2021-03-08T03:57:58.466Z"),
        "ManagedBy": null,
        "IsDeleted": false,
        "TaskDefine": {
            "WorkspaceId": "5fb3660530b3090001e0c6d8",
            "UpdatedTaskStatus": "KnowledgeMerged"
        },
        "Status": {
            "LastStatus": "准备中",
            "LastRunAt": null,
            "LastFinishedAt": null,
            "LastSucceedAt": null,
            "TryTimes": 0
        }
    }, { upsert: true });
    