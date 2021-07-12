using MongoDB.Bson.Serialization.Attributes;
using MusicKG.DataAccess.Enums;
using MusicKG.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.Service.Test.Models
{
    public class TaskDataModelTest : TaskDataModel
    {
        [BsonIgnore]
        public TaskStatusEnum ExpectedTaskStatus { get; set; }
    }
}
