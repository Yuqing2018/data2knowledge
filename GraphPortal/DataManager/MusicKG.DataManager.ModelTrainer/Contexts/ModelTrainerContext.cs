using MongoDB.Driver;
using MusicKG.DataManager.Models;
using MusicKG.DataManager.ModelTrainer.Tasks;
using MusicKG.Scheduler.Engine.Models;

namespace MusicKG.DataManager.ModelTrainer.Contexts
{
    /// <summary>
    /// The context used for storing configuration or data across all actions in this task.
    /// </summary>
    public class ModelTrainerContext : JobTaskContext<ModelTrainingTaskDefine>
    {
        public string ModelName { get; set; }

        public bool NeedTrain { get; set; } = true;

        public int TrainingDataCount { get; set; }

        public int LastModelVersion { get; set; }

        public int? CurrentModelVersion { get; set; }

        public string TrainingDataInputFolder { get; set; }

        public IMongoCollection<ModelTrainingHistory> HistoryCollection { get; set; }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
