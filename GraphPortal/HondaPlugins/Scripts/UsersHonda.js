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