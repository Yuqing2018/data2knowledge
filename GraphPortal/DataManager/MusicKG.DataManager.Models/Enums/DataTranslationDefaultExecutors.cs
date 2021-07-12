namespace MusicKG.DataManager.Models
{
    /// <summary>
    /// Action executor names used for this scheduler job.
    /// </summary>
    public enum DataTranslationDefaultExecutors
    {
        DefaultMongoDataCollector,

        DefaultMySqlDataCollector,

        DefaultDataConstructor,

        DefaultDataNormalizer,

        DefaultDataPreserver,

        DefaultDataConsumer,

        DefaultLabelingToolDataConsumer,

        DefaultDataAnnotator,

        DefaultTaskCreator
    }
}