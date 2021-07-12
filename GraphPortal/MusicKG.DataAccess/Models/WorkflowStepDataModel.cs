using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MusicKG.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.DataAccess.Models
{
    [BsonIgnoreExtraElements]
    public class WorkflowStepDataModel
    {
        public ObjectId StepId { get; set; }

        public string Name { get; set; }

        public string ProcessorAssembly { get; set; }

        public string ProcessorClass { get; set; }

        public WorkflowStepInputFilterDataModel InputFilter { get; set; }

        public DocumentStatusEnum ResultDocumentStatus { get; set; }

        //Only used in synchronized workflow.
        public bool AutoDoNext { get; set; }
    }
}
