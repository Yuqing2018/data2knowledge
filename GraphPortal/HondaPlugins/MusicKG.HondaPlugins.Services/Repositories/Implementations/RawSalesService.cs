using MusicKG.HondaPlugins.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using MusicKG.HondaPlugins.DataAccess.DataModels.Raws;
using MusicKG.HondaPlugins.Services.Models;
using MusicKG.HondaPlugins.DataAccess.Settings;

namespace MusicKG.HondaPlugins.Services.Repositories.Implementations
{
    public class RawSalesService : IRawSalesService
    {
        private readonly IRawDbContext context;
        private readonly ILogger<RawSalesService> logger;

        public RawSalesService(IRawDbContext context, ILogger<RawSalesService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<long> GetSalesCount(List<string> carModels, List<string> carTypes, List<string> yearModels, string frameNo, DateTime? start, DateTime? end)
        {
            var querable = context.RawSalesData.AsQueryable();

            if (carModels?.Count > 0)
                querable = querable.Where(sale => carModels.Contains(sale.CarModel));

            if (carTypes?.Count > 0)
                querable = querable.Where(sale => carTypes.Contains(sale.CarType));

            if (yearModels?.Count > 0)
                querable = querable.Where(sale => yearModels.Contains(sale.ModelYear));

            if (start != null)
                querable = querable.Where(sale => sale.ProductionDate.CompareTo(start.Value.ToString("yyyyMMdd")) >= 0);

            if (end != null)
                querable = querable.Where(sale => sale.ProductionDate.CompareTo(end.Value.ToString("yyyyMMdd")) < 0);

            if (!string.IsNullOrWhiteSpace(frameNo))
                querable = querable.Where(sale => sale.FrameNo == frameNo);

            var count = await querable.LongCountAsync();

            return count;
        }

        public async Task<DateTime> GetSalesInitRegisterDate(List<string> carModels, List<string> carTypes, List<string> yearModels, string frameNo, DateTime? start, DateTime? end)
        {
            var querable = context.RawSalesData.AsQueryable().Where(x => x.InitialRegistDate != null);

            if (carModels?.Count > 0)
                querable = querable.Where(sale => carModels.Contains(sale.CarModel));

            if (carTypes?.Count > 0)
                querable = querable.Where(sale => carTypes.Contains(sale.CarType));

            if (yearModels?.Count > 0)
                querable = querable.Where(sale => yearModels.Contains(sale.ModelYear));

            if (start != null)
                querable = querable.Where(sale => sale.ProductionDate.CompareTo(start.Value.ToString("yyyyMMdd")) >= 0);

            if (end != null)
                querable = querable.Where(sale => sale.ProductionDate.CompareTo(end.Value.ToString("yyyyMMdd")) < 0);

            if (!string.IsNullOrWhiteSpace(frameNo))
                querable = querable.Where(sale => sale.FrameNo == frameNo);
            var initDates = await querable.Select(x => x.InitialRegistDate).Distinct().ToListAsync();

            DateTime initData = DateTime.ParseExact(initDates.Min(), "yyyyMMdd", null);
            return initData;
        }

        public async Task<List<RawSalesDataServiceModel>> GetSalesDatas(List<string> carModels, List<string> carTypes, List<string> yearModels, string frameNo, DateTime? start, DateTime? end)
        {
            var querable = context.RawSalesData.AsQueryable().Where(x => x.InitialRegistDate != null);

            if (carModels?.Count > 0)
                querable = querable.Where(sale => carModels.Contains(sale.CarModel));

            if (carTypes?.Count > 0)
                querable = querable.Where(sale => carTypes.Contains(sale.CarType));

            if (yearModels?.Count > 0)
                querable = querable.Where(sale => yearModels.Contains(sale.ModelYear));

            if (start != null)
                querable = querable.Where(sale => sale.ProductionDate.CompareTo(start.Value.ToString("yyyyMMdd")) >= 0);

            if (end != null)
                querable = querable.Where(sale => sale.ProductionDate.CompareTo(end.Value.ToString("yyyyMMdd")) < 0);

            if (!string.IsNullOrWhiteSpace(frameNo))
                querable = querable.Where(sale => sale.FrameNo == frameNo);

            var results = await querable.Select(x => new RawSalesDataServiceModel()
            {
                CarModel = string.IsNullOrWhiteSpace(x.CarModel) ? ConstantSettings.UnknownString : x.CarModel,
                CarType = string.IsNullOrWhiteSpace(x.CarType) ? ConstantSettings.UnknownString : x.CarType,
                ModelYear = string.IsNullOrWhiteSpace(x.ModelYear) ? ConstantSettings.UnknownString : x.ModelYear,
                FrameNo = string.IsNullOrWhiteSpace(x.FrameNo) ? ConstantSettings.UnknownString : x.FrameNo,
                ProductionDate = string.IsNullOrWhiteSpace(x.ProductionDate) ? null : DateTime.ParseExact(x.ProductionDate, "yyyyMMdd", null),
                InitialRegistDate = string.IsNullOrWhiteSpace(x.InitialRegistDate) ? null : DateTime.ParseExact(x.InitialRegistDate, "yyyyMMdd", null),
                Timestamp = x.Timestamp,
            }).ToListAsync();
            return results;
        }
    }
}
