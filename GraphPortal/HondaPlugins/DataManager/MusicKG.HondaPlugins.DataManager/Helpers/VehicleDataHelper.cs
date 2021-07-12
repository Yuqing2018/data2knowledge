using MusicKG.HondaPlugins.DataAccess;
using MusicKG.HondaPlugins.DataAccess.DataModels.Business;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.DataManager.Helpers
{
    public static class VehicleDataHelper
    {
        public async static Task UpsertVehicleDataAsync(IHondaMongoDbContext context, VehicleFaultDataModel data)
        {
            var originalData = await context.VehicleFault.AsQueryable()
                .Select(v => new { RawId = v.RawId, RelatedInfo = v.RelatedInfo, SyncTimestamp = v.SyncTimestamp })
                .FirstOrDefaultAsync(v => v.RawId == data.RawId);

            if (originalData == null)
                await context.VehicleFault.InsertOneAsync(data);
            else
            {
                if (originalData.SyncTimestamp.HasValue && data.SyncTimestamp.HasValue && originalData.SyncTimestamp >= data.SyncTimestamp)
                    await context.VehicleFault.UpdateOneAsync(v => v.RawId == data.RawId,
                        Builders<VehicleFaultDataModel>.Update.Set(v => v.PartName, data.PartName)
                        .Set(v => v.Syndrome, data.Syndrome));
                else
                {
                    data.RelatedInfo = originalData.RelatedInfo;
                    await context.VehicleFault.ReplaceOneAsync(v => v.RawId == data.RawId, data,
                        new ReplaceOptions { IsUpsert = true });
                }
            }
        }
    }
}
