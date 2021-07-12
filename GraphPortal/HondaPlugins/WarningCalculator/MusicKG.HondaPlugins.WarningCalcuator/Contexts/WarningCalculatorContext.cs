using MusicKG.HondaPlugins.DataAccess.DataModels.Warnings;
using MusicKG.Scheduler.Engine.Models;
using MongoDB.Bson;
using System.Collections.Generic;

namespace MusicKG.HondaPlugins.WarningCalculator.Contexts
{
    public class WarningCalculatorContext : JobTaskContext<WarningTaskDataModel>
    {
        public List<ObjectId> SavedRecordIds { get; set; }

        public List<ObjectId> SavedDetailIds { get; set; }

        public override void Initialize()
        {
            base.Initialize();
            SavedRecordIds = new List<ObjectId>();
            SavedDetailIds = new List<ObjectId>();
        }

        public override void Dispose()
        {
            SavedRecordIds.Clear();
            SavedDetailIds.Clear();
            base.Dispose();
        }
    }
}
