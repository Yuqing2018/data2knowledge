namespace MusicKG.DataManager.Models
{
    /// <summary>
    /// Action names used for this scheduler job.
    /// </summary>
    public enum DataTranslationActions
    {
        DataCollection,

        DataNormalization,

        DataAnnotation,

        DataConsumption,

        TaskCreation
    }
}