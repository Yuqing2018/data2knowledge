namespace MusicKG.HondaPlugins.DataAccess.Settings
{
    public class MongoRawDataDbSettings
    {
        public string ConnectionString { get; set; }

        public string Database { get; set; }

        public RawDataTableSettings GOVDataSettings { get; set; }

        public RawDataTableSettings MediaMainDataSettings { get; set; }

        public RawDataTableSettings MediaSubDataSettings { get; set; }
    }
}
