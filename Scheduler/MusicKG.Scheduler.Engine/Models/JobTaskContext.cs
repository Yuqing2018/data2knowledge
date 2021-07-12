using MongoDB.Bson.Serialization;
using MusicKG.Scheduler.Service.Models;
using System;

namespace MusicKG.Scheduler.Engine.Models
{
    public abstract class JobTaskContext
    {
        public const string Unknown = "Unknown";

        public virtual DateTime From { get; set; }

        public virtual DateTime To { get; set; }

        public virtual DateTime TaskRunTime { get; set; }

        public virtual JobTaskServiceModel Task { get; set; }

        public abstract void Initialize();

        public abstract void Dispose();
    }

    public abstract class JobTaskContext<TTaskDefine> : JobTaskContext where TTaskDefine : class
    {
        public virtual TTaskDefine Parameters { get; set; }

        public override void Initialize()
        {
            Parameters = BsonSerializer.Deserialize<TTaskDefine>(Task.TaskDefine);
        }

        public override void Dispose()
        {
            Parameters = null;
        }
    }
}