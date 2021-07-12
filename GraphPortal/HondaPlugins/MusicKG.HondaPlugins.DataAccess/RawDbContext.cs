using MusicKG.HondaPlugins.DataAccess.DataModels.Raws;
using MusicKG.HondaPlugins.DataAccess.Settings;
using Microsoft.EntityFrameworkCore;

namespace MusicKG.HondaPlugins.DataAccess
{
    public class RawDbContext : DbContext, IRawDbContext
    {
        private readonly MariaRawDataDbSettings rawDataDbSettings;

        public RawDbContext(MariaRawDataDbSettings rawDataDbSettings) : base()
        {
            this.rawDataDbSettings = rawDataDbSettings;
        }

        public DbSet<RawVehicleData> RawVehicleData { get; set; }
        
        public DbSet<RawSalesData> RawSalesData { get; set; }
        
        public DbSet<RawRegionData> RawRegionData { get; set; }
        
        public DbSet<RawQICData> RawQICData { get; set; }
        
        public DbSet<RawQISData> RawQISData { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(rawDataDbSettings.ConnectionString);
            optionsBuilder.EnableDetailedErrors();
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}