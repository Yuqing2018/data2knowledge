#coding:utf-8


ontology = {
    "entities":["Person","MusicRecording","MusicAlbum","MusicGroup"],
    "relations":{
        "Person.sing":["Person","MusicRecording"],
        "MusicRecording.sungBy":["MusicRecording","Person"],
    },
    "properties":["Person.age","MusicRecording.in_album"]
}