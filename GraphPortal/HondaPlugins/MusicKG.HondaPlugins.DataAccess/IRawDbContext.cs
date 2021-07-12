using MusicKG.HondaPlugins.DataAccess.DataModels.Raws;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.DataAccess
{
    public interface IRawDbContext
    {
        DbSet<RawVehicleData> RawVehicleData { get; set; }
        
        DbSet<RawSalesData> RawSalesData { get; set; }

        DbSet<RawRegionData> RawRegionData { get; set; }

        DbSet<RawQICData> RawQICData { get; set; }

        DbSet<RawQISData> RawQISData { get; set; }

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        EntityEntry<TEntity> Add<TEntity>(TEntity entity) where TEntity : class;

        ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default(CancellationToken)) where TEntity : class;

        void AddRange(IEnumerable<object> entities);

        Task AddRangeAsync(IEnumerable<object> entities, CancellationToken cancellationToken = default(CancellationToken));

        EntityEntry<TEntity> Attach<TEntity>(TEntity entity) where TEntity : class;
        
        void AttachRange(IEnumerable<object> entities);

        EntityEntry<TEntity> Update<TEntity>(TEntity entity) where TEntity : class;
        
        void UpdateRange(IEnumerable<object> entities);

        EntityEntry<TEntity> Remove<TEntity>(TEntity entity) where TEntity : class;
        
        void RemoveRange(params object[] entities);
    }
}
