namespace MusicKG.HondaPlugins.DataManager
{
    public enum HondaExecutors
    {
        #region Raw data translation

        HondaDataNormalizer,

        HondaDataAnnotator,

        HondaBusinessDataConsumer,

        HondaLabelingToolDataConsumer,

        #endregion

        #region Master DB data translation

        HondaVehicleFaultDataCollector,

        HondaVehicleFaultDataNormalizer,

        HondaVehicleFaultDataConsumer,

        #endregion

        #region Result handling

        HondaDocumentCollector,

        HondaDataHandler

        #endregion
    }
}