using MusicKG.HondaPlugins.DataAccess;
using MusicKG.HondaPlugins.DataAccess.Enums;
using MusicKG.HondaPlugins.DataAccess.DataModels.Business;
using MusicKG.HondaPlugins.DataAccess.Settings;
using MusicKG.HondaPlugins.Services.Helpers;
using MusicKG.HondaPlugins.Services.Resources;
using MusicKG.HondaPlugins.Services.Models;
using MusicKG.HondaPlugins.ModelTrainer.Extensions;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System;

namespace MusicKG.HondaPlugins.Services.Repositories.Implementations
{
    public class VehicleFaultDataService : IVehicleFaultDataService
    {
        private readonly IHondaMongoDbContext context;
        private readonly IRawVehicleDataService rawVehicleDataService;
        private readonly IRawQISService rawQISService;
        private readonly ISyndromeService syndromeService;
        private readonly VehicleDataConstructorSettings vehicleDataConstructorSettings;
        private readonly ILogger<VehicleFaultStatisticsService> logger;

        private const int utcOffset = -8;

        /// <summary>
        /// MQIRepository service constructor.
        /// </summary>
        /// <param name="context">MusicKG mongodb context.</param>
        public VehicleFaultDataService(
            VehicleDataConstructorSettings vehicleDataConstructorSettings,
            IHondaMongoDbContext context,
            IRawQISService rawQISService,
            ISyndromeService syndromeService,
            IRawVehicleDataService rawVehicleDataService,
            ILogger<VehicleFaultStatisticsService> logger)
        {
            this.context = context;
            this.rawQISService = rawQISService;
            this.syndromeService = syndromeService;
            this.vehicleDataConstructorSettings = vehicleDataConstructorSettings;
            this.rawVehicleDataService = rawVehicleDataService;
            this.logger = logger;
        }

        public async Task<Dictionary<string, string>> DetailAsync(string rawId)
        {
            var result = await context.VehicleFault.AsQueryable().FirstOrDefaultAsync(v => v.RawId == rawId);

            return result?.Features;
        }

        public async Task UpdateAsync(string rawId, string partName, string syndrome, bool isAddToTraining)
        {
            if (string.IsNullOrWhiteSpace(rawId))
                ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.RawIdCannotBeEmpty, HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(partName))
                ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.PartNameCannotBeEmpty, HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(syndrome))
                ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.SyndromeCannotBeEmpty, HttpStatusCode.BadRequest);

            var data = await context.VehicleFault.AsQueryable().FirstOrDefaultAsync(v => v.RawId == rawId);

            if (data == null)
                ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.RawIdNotExists, HttpStatusCode.BadRequest);

            var syndromeData = await context.Syndromes.AsQueryable().FirstOrDefaultAsync(s => s.Id == new ObjectId(syndrome));

            if (data.Syndrome != syndrome || data.PartName != partName)
            {
                if (syndromeData == null)
                    ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.SyndromeNotExists, HttpStatusCode.BadRequest);

                var update = Builders<VehicleFaultDataModel>.Update.Set(v => v.PartName, partName).Set(v => v.Syndrome, syndrome);

                var result = await context.VehicleFault.UpdateOneAsync(v => v.RawId == rawId, update);

                if (result.ModifiedCount == 0)
                    ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.UpdateVehicleFaultFailed, HttpStatusCode.InternalServerError);
            }

            if (isAddToTraining)
            {
                data.Syndrome = syndromeData.Name;
                data.PartName = partName;
                await context.TrainingData.ReplaceOneAsync(t => t.Id == data.RawId, data.ToModelTrainingData(vehicleDataConstructorSettings), new ReplaceOptions { IsUpsert = true });
            }
        }

        public async Task<(long TotalCount, IEnumerable<VehicleFaultListServiceModel> Data)> ListAsync(
            List<string> carModel, List<DataSource> dataSource, List<string> carType, List<string> modelYears,
            List<string> partName, List<string> syndrome, DateTime? faultDateStart, DateTime? faultDateEnd,
            int from, int? size, int maxSize)
        {
            var querable = await GetFiltersAsync(carModel, dataSource, carType, modelYears, partName, syndrome, faultDateStart, faultDateEnd);

            querable = querable.OrderByDescending(x => x.FaultDate);

            var data = await querable.Select(v => new VehicleFaultListServiceModel
            {
                RawId = v.RawId,
                CarModel = v.CarModel,
                CarType = v.CarType,
                DealerCD = v.DealerCD,
                DealerName = v.DealerName,
                DataSource = v.DataSource,
                FaultDate = v.FaultDate,
                FrameNo = v.FrameNo,
                InitialRegistDate = v.InitialRegistDate,
                MileAge = v.MileAge,
                ModelYear = v.ModelYear,
                PartName = v.PartName ?? ConstantSettings.UnknownString,
                PartNo = v.PartNo,
                ProductionDate = v.ProductionDate,
                Region = v.Region,
                Province = v.Province,
                CostRepair = v.CostRepair,
                Syndrome = v.Syndrome
            }).Skip(from).Take(maxSize).ToListAsync();

            var count = data?.Count < maxSize ? data?.Count : maxSize;

            if (size.HasValue)
                data = data?.Take(size.Value)?.ToList();

            if (data?.Count > 0)
                await syndromeService.LinkSyndromeAsync(data);

            return (count == null ? from : from + count.Value, data);
        }

        public async Task<List<VehicleFaultListServiceModel>> ListStatisticableAsync(
            List<string> carModel, List<DataSource> dataSource, List<string> carType, List<string> modelYears,
            List<string> partName, List<string> syndrome, DateTime? faultDateStart, DateTime? faultDateEnd)
        {
            var querable = await GetFiltersAsync(carModel, dataSource, carType, modelYears, partName, syndrome, faultDateStart, faultDateEnd);

            querable = querable.Where(v => v.Region != null && v.Region != "")
                .Where(v => v.DealerCD != null && v.DealerCD != "")
                .Where(v => v.MileAge > -1)
                .Where(v => v.ProductionDate > DateTime.MinValue && v.ProductionDate <= DateTime.UtcNow)
                .Where(v => v.InitialRegistDate > DateTime.MinValue && v.InitialRegistDate <= DateTime.UtcNow)
                .Where(v => v.FaultDate > DateTime.MinValue && v.FaultDate <= DateTime.UtcNow);

            var data = await querable.Select(v => new VehicleFaultListServiceModel
            {
                RawId = v.RawId,
                CarModel = v.CarModel,
                CarType = v.CarType,
                DealerCD = v.DealerCD,
                DealerName = v.DealerName,
                DataSource = v.DataSource,
                FaultDate = v.FaultDate,
                FrameNo = v.FrameNo,
                InitialRegistDate = v.InitialRegistDate,
                MileAge = v.MileAge,
                ModelYear = v.ModelYear,
                PartName = v.PartName ?? ConstantSettings.UnknownString,
                PartNo = v.PartNo,
                ProductionDate = v.ProductionDate,
                Region = v.Region,
                Province = v.Province,
                CostRepair = v.CostRepair,
                Syndrome = v.Syndrome
            }).ToListAsync();

            await syndromeService.LinkSyndromeAsync(data);

            return data;
        }

        public async Task<List<VehicleFaultServiceModel>> ListExportableAsync(
            List<string> carModels, List<DataSource> dataSource, List<string> carType, List<string> modelYears,
            List<string> partName, List<string> syndrome, DateTime? faultDateStart, DateTime? faultDateEnd)
        {
            var querable = await GetFiltersAsync(carModels, dataSource, carType, modelYears, partName, syndrome, faultDateStart, faultDateEnd);

            querable = querable.Where(v => v.Region != null && v.Region != "")
                .Where(v => v.DealerCD != null && v.DealerCD != "")
                .Where(v => v.MileAge > -1)
                .Where(v => v.ProductionDate > DateTime.MinValue && v.ProductionDate <= DateTime.UtcNow)
                .Where(v => v.InitialRegistDate > DateTime.MinValue && v.InitialRegistDate <= DateTime.UtcNow)
                .Where(v => v.FaultDate > DateTime.MinValue && v.FaultDate <= DateTime.UtcNow);

            var data = await querable.Select(v => new VehicleFaultServiceModel
            {
                RawId = v.RawId,
                CarModel = v.CarModel,
                CarType = v.CarType,
                DealerCD = v.DealerCD,
                DealerName = v.DealerName,
                DataSource = v.DataSource,
                FaultDate = v.FaultDate,
                FrameNo = v.FrameNo,
                InitialRegistDate = v.InitialRegistDate,
                MileAge = v.MileAge,
                ModelYear = v.ModelYear,
                PartName = v.PartName,
                PartNo = v.PartNo,
                ProductionDate = v.ProductionDate,
                Region = v.Region,
                Province = v.Province,

                CostRepair = v.CostRepair,
                Syndrome = v.Syndrome,
                Features = v.Features,
                RelatedInfo = v.RelatedInfo
            }).ToListAsync();

            await syndromeService.LinkSyndromeAsync(data);

            return data;
        }

        public async Task<List<string>> ListIdsAsync(
            List<string> carModels, List<string> carType, List<string> modelYears,
            List<string> partName, List<string> syndrome, DateTime? faultDateStart, DateTime? faultDateEnd, bool ignoreUnknownPart)
        {
            var querable = await GetFiltersAsync(carModels, new List<DataSource> { DataSource.MQI, DataSource.TECH_CONSULTING, DataSource.HOTLINE, DataSource.GOV, DataSource.MEDIA_MAIN }, carType, modelYears, partName, syndrome, faultDateStart, faultDateEnd);

            querable = querable.Where(v => v.Region != null && v.Region != "")
                .Where(v => v.DealerCD != null && v.DealerCD != "")
                .Where(v => v.MileAge > -1)
                .Where(v => v.ProductionDate > DateTime.MinValue && v.ProductionDate <= DateTime.UtcNow)
                .Where(v => v.InitialRegistDate > DateTime.MinValue && v.InitialRegistDate <= DateTime.UtcNow)
                .Where(v => v.FaultDate > DateTime.MinValue && v.FaultDate <= DateTime.UtcNow);

            if (ignoreUnknownPart)
                querable.Where(v => v.PartName != ConstantSettings.UnknownString);

            var data = await querable.Select(v => v.RawId).ToListAsync();

            return data;
        }

        public async Task<List<VehicleFaultServiceModel>> LinkRelatedDataAsyncBatch(List<string> rawIds)
        {
            var vehicleFaultData = await context.VehicleFault.AsQueryable().Select(v => new VehicleFaultServiceModel
            {
                RawId = v.RawId,
                CarModel = v.CarModel,
                CarType = v.CarType,
                DealerCD = v.DealerCD,
                DealerName = v.DealerName,
                DataSource = v.DataSource,
                FaultDate = v.FaultDate,
                FrameNo = v.FrameNo,
                InitialRegistDate = v.InitialRegistDate,
                MileAge = v.MileAge,
                ModelYear = v.ModelYear,
                PartName = v.PartName,
                PartNo = v.PartNo,
                ProductionDate = v.ProductionDate,
                Region = v.Region,
                Province = v.Province,
                CostRepair = v.CostRepair,
                Syndrome = v.Syndrome,
                RelatedInfo = v.RelatedInfo
            }).Where(v => rawIds.Contains(v.RawId)).ToListAsync();

            var qis = await rawQISService.GetRelatedQISAsync(vehicleFaultData?.Where(v => v.DataSource == DataSource.MQI)?.Select(v => v.RawId)?.ToList());

            foreach (var data in vehicleFaultData)
            {
                if (data.RelatedInfo == null)
                    data.RelatedInfo = new List<VehicleFaultRelatedDataModel>();

                if (qis.ContainsKey(data.RawId))
                {
                    var qiss = qis[data.RawId];

                    if (qiss == null || qiss.Count == 0)
                        continue;

                    qiss.ForEach(r =>
                    {
                        var qicNo = r.QicData?.QIC_NO;
                        var qisNo = r.QisData?.QIS_NO;

                        var existed = data.RelatedInfo.FirstOrDefault(i => i.QICNo == qicNo);

                        if (existed == null)
                        {
                            var qicUpdateTime = r.QicData?.LastUpdateTime?.AddHours(utcOffset) ?? (r.QicData?.Timestamp ?? DateTime.MinValue);
                            var qisUpdateTime = r.QisData?.LastUpdateTime?.AddHours(utcOffset) ?? (r.QisData?.Timestamp ?? DateTime.MinValue);
                            var dataUpdateTime = new List<DateTime> { qicUpdateTime, qisUpdateTime }.Max();

                            data.RelatedInfo.Add(new VehicleFaultRelatedDataModel
                            {
                                QICNo = r.QisData?.QIC_NO ?? r.QicData?.QIC_NO,
                                QISNo = r.QisData?.QIS_NO,
                                CntrMesrReasonDesc = r.QisData?.CNTR_MESR_REASON_DESC,
                                CntrMesrType = r.QisData?.CNTR_MESR_TYPE,
                                PermanentCntr = r.QisData?.PERMANENT_CNTR,
                                PermanentCntrTime = r.QisData?.PERMANENT_CNTR_TIME,
                                QicDamPartArrDate = r.QicData?.DAM_PART_ARR_DATE,
                                QicDeal1STCode = r.QicData?.DEAL_1ST_CODE,
                                QicDeal1STDesc = r.QicData?.DEAL_1ST_DESC,
                                QicDeal2NDCode = r.QicData?.DEAL_2ND_CODE,
                                QicGDFlag = r.QicData?.IS_IMPORT_FLAG,
                                QicIsImportFlag = r.QicData?.IS_IMPORT_FLAG,
                                QicRank = r.QicData?.RANK,
                                QicReceiveDate = r.QicData?.RECEIVE_DATE,
                                QicReleaseDate = r.QicData?.RELEASE_DATE,
                                QicTheme = r.QicData?.THEME,
                                LastModifiedAt = dataUpdateTime == DateTime.MinValue ? DateTime.UtcNow : dataUpdateTime
                            });
                        }
                        else
                        {
                            var qicUpdateTime = r.QicData?.LastUpdateTime?.AddHours(utcOffset) ?? DateTime.MinValue;
                            var qisUpdateTime = r.QisData?.LastUpdateTime?.AddHours(utcOffset) ?? DateTime.MinValue;
                            var rawUpdateTime = new List<DateTime> { qicUpdateTime, qisUpdateTime }.Max();
                            var dataUpdateTime = existed.LastModifiedAt;

                            if (rawUpdateTime > dataUpdateTime)
                            {
                                existed.QISNo = r.QisData?.QIS_NO;
                                existed.QicDamPartArrDate = r.QicData?.DAM_PART_ARR_DATE;
                                existed.QicDeal1STCode = r.QicData?.DEAL_1ST_CODE;
                                existed.QicDeal1STDesc = r.QicData?.DEAL_1ST_DESC;
                                existed.QicDeal2NDCode = r.QicData?.DEAL_2ND_CODE;
                                existed.QicGDFlag = r.QicData?.IS_IMPORT_FLAG;
                                existed.QicIsImportFlag = r.QicData?.IS_IMPORT_FLAG;
                                existed.QicRank = r.QicData?.RANK;
                                existed.QicReceiveDate = r.QicData?.RECEIVE_DATE;
                                existed.QicReleaseDate = r.QicData?.RELEASE_DATE;
                                existed.QicTheme = r.QicData?.THEME;
                                existed.CntrMesrReasonDesc = r.QisData?.CNTR_MESR_REASON_DESC;
                                existed.CntrMesrType = r.QisData?.CNTR_MESR_TYPE;
                                existed.PermanentCntr = r.QisData?.PERMANENT_CNTR;
                                existed.PermanentCntrTime = r.QisData?.PERMANENT_CNTR_TIME;
                                existed.LastModifiedAt = rawUpdateTime;
                            }
                        }
                    });

                    var dataFrom = data.RelatedInfo.Count > 0 ? "系统匹配" : null;

                    await context.VehicleFault.UpdateOneAsync(v => v.RawId == data.RawId,
                            Builders<VehicleFaultDataModel>.Update
                            .Set(u => u.DataFrom, dataFrom)
                            .Set(u => u.RelatedInfo, data.RelatedInfo.ToList())
                            .Set(u => u.LastModifiedAt, DateTime.UtcNow));
                }
            }

            await syndromeService.LinkSyndromeAsync(vehicleFaultData);

            return vehicleFaultData;
        }

        public async Task<List<string>> ListCarModelsAsync(DataSource? dataSource)
        {
            return await rawVehicleDataService.ListCarModelsAsync();
        }

        public async Task<List<string>> ListCarTypesAsync(DataSource? dataSource, List<string> carModel)
        {
            return await rawVehicleDataService.ListCarTypesAsync(carModel);
        }

        public async Task<List<string>> ListModelYearsAsync(DataSource? dataSource, List<string> carModel, List<string> carType)
        {
            return await rawVehicleDataService.ListModelYearsAsync(carModel, carType);
        }

        public async Task<List<string>> ListPartNamesAsync(DataSource? dataSource, List<string> carModel, List<string> carType, List<string> modelYears, List<string> syndrome)
        {
            if (dataSource.HasValue || carModel?.Count > 0 || carType?.Count > 0 || modelYears?.Count > 0 || syndrome?.Count > 0)
            {
                var querable = await GetFiltersAsync(carModel, dataSource.HasValue ? new List<DataSource> { dataSource.Value } : null, carType, modelYears, null, syndrome, null, null);

                return await querable.Select(v => v.PartName).Distinct()
                    .ToListAsync();
            }
            else
            {
                return await context.Parts.AsQueryable().SelectMany(p => p.Names).Distinct().ToListAsync();
            }
        }

        public async Task<List<SyndromeServiceModel>> ListSyndromesAsync(DataSource? dataSource, List<string> carModel, List<string> carType, List<string> modelYears, List<string> partName)
        {
            if (dataSource.HasValue || carModel?.Count > 0 || carType?.Count > 0 || modelYears?.Count > 0 || partName?.Count > 0)
            {
                var querable = await GetFiltersAsync(carModel, dataSource.HasValue ? new List<DataSource> { dataSource.Value } : null, carType, modelYears, partName, null, null, null);

                var syndromeIds = await querable.Select(v => v.Syndrome).Distinct()
                    .ToListAsync();

                return await syndromeService.GetDistinctSyndromeAsync(syndromeIds);
            }
            else
            {
                return await context.Syndromes.AsQueryable().Select(s => new SyndromeServiceModel
                {
                    MongoId = s.Id,
                    Name = s.Name,
                    BadGrade = s.BadGrade
                }).ToListAsync();
            }
        }

        public async Task<Dictionary<DataSource, List<List<string>>>> ExportAsync(
            List<string> carModel, List<DataSource> dataSource, List<string> carType, List<string> modelYears,
            List<string> partName, List<string> syndrome, DateTime? faultDateStart, DateTime? faultDateEnd, int maxCount)
        {
            var querable = await GetFiltersAsync(carModel, dataSource, carType, modelYears, partName, syndrome, faultDateStart, faultDateEnd);
            var dataList = await querable.Take(maxCount).ToListAsync();
            var syndromes = await syndromeService.GetDistinctSyndromeAsync(dataList.Select(x => x.Syndrome).Distinct().ToList());
            var tables = dataList.GroupBy(x => x.DataSource).ToDictionary(k => k.Key, v =>
               {
                   var table = new List<List<string>>();
                   var header = v.FirstOrDefault().Features.Keys.ToList();
                   header.InsertRange(0, new List<string>() { "车款（解析）", "车型（解析）", "年款（解析）", "零件名", "不良症状", "特约店名称（映射）", "生产日期（映射）", "销售日期（映射）" });
                   var contents = v.OrderByDescending(x => x.FaultDate).Select(x =>
                   {
                       var columns = x.Features.Values.ToList();
                       columns.InsertRange(0, new List<string>()
                       {
                           x.CarModel,
                           x.CarType,
                           x.ModelYear,
                           x.PartName,
                           syndromes.FirstOrDefault(s => s.Id == x.Syndrome).Name,
                           x.DealerName,
                           x.ProductionDate.ToString("yyyy-MM-dd HH:mm:ss"),
                           x.InitialRegistDate.ToString("yyyy-MM-dd HH:mm:ss")
                       });
                       return columns;
                   });
                   table.Add(header);
                   table.AddRange(contents);
                   return table;
               });
            return tables;
        }

        public async Task<(long TotalCount, IEnumerable<VehicleFaultQISInfoServiceModel> Data)> ListQisAsync(List<DataSource> dataSource, List<string> carModel, List<string> carType, List<string> modelYears, List<string> partName, string frameNo, int from, int? size)
        {
            var querable = await GetFiltersAsync(carModel, dataSource, carType, modelYears, partName, null, null, null);

            if (!string.IsNullOrWhiteSpace(frameNo))
                querable = querable.Where(v => v.FrameNo == frameNo);

            var totalCount = await querable.LongCountAsync();

            var vehicleDatas = await querable.Select(v => new VehicleFaultDataModel
            {
                RelatedInfo = v.RelatedInfo,
                Syndrome = v.Syndrome,
                Id = v.Id,
                RawId = v.RawId,
                FrameNo = v.FrameNo,
                PartNo = v.PartNo,
                FaultDate = v.FaultDate,
                DataSource = v.DataSource,
                DataFrom = v.DataFrom,
                DataFromDesc = v.DataFromDesc,
                RelatedPartName = v.RelatedPartName,
                LastModifiedAt = v.LastModifiedAt
            }).ToListAsync();

            var syndromes = await syndromeService.GetDistinctSyndromeAsync(vehicleDatas.Select(x => x.Syndrome).Distinct().ToList());

            var results = vehicleDatas.Select(x =>
            {
                var qis = x.RelatedInfo?.OrderByDescending(r => r.LastModifiedAt)?.FirstOrDefault();
                var syndrome = syndromes.FirstOrDefault(s => s.Id == x.Syndrome);
                var qisInfo = new VehicleFaultQISInfoServiceModel()
                {
                    Id = x.Id.ToString(),
                    RawId = x.RawId,
                    PartNo = x.PartNo,
                    FaultDate = x.FaultDate,
                    FrameNo = x.FrameNo,
                    DataSource = x.DataSource,
                    SyndromeModel = new SyndromeServiceModel
                    {
                        MongoId = new ObjectId(syndrome?.Id),
                        Name = syndrome?.Name ?? "未知",
                        BadGrade = syndrome?.BadGrade ?? BadGrade.C
                    },
                    QICNo = qis?.QICNo,
                    QISNo = qis?.QISNo,
                    CntrMesrType = qis?.CntrMesrType,
                    CntrMesrReasonDesc = qis?.CntrMesrReasonDesc,
                    PermanentCntr = qis?.PermanentCntr,
                    PermanentCntrTime = qis?.PermanentCntrTime,
                    RelatedPartName = x.RelatedPartName,
                    DataFrom = x.DataFrom,
                    DataFromDesc = x.DataFrom == "系统匹配" ? "系统匹配" : x.DataFromDesc,
                    LastModifiedAt = qis?.LastModifiedAt,
                    HasRelated = qis != null
                };
                return qisInfo;
            }).OrderByDescending(x => x.HasRelated).ThenBy(x => x.SyndromeModel.Name).ThenByDescending(x => x.LastModifiedAt).ThenBy(x => x.QICNo).Skip(from).Take(size ?? int.MaxValue).ToList();

            return (totalCount, results);
        }

        /// <summary>
        /// 更新关联零件和系统匹配
        /// </summary>
        /// <param name="rawId"></param>
        /// <param name="carType"></param>
        /// <param name="partNo"></param>
        /// <returns></returns>
        public async Task<bool> UpdateRelatedPartName(string rawId, string carType, string partNo)
        {
            var data = await context.VehicleFault.AsQueryable().FirstOrDefaultAsync(v => v.RawId == rawId);

            if (data == null)
                ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.RawIdNotExists, HttpStatusCode.BadRequest);

            var update = Builders<VehicleFaultDataModel>.Update
                .Set(x => x.RelatedPartName, new RelatedPart { No = partNo, CarType = carType })
                .Set(x => x.LastModifiedAt, DateTime.UtcNow);

            var result = context.VehicleFault.UpdateOneAsync(v => v.Id == data.Id, update).Result;

            if (result.ModifiedCount == 0)
                ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.UpdateVehicleFaultFailed, HttpStatusCode.InternalServerError);

            return true;
        }

        /// <summary>
        /// 更新系统匹配
        /// </summary>
        /// <param name="rawId"></param>
        /// <param name="dataFromDesc"></param>
        /// <returns></returns>
        public async Task<bool> UpdateDataFromAsync(string rawId, string dataFromDesc)
        {
            var data = await context.VehicleFault.AsQueryable().FirstOrDefaultAsync(v => v.RawId == rawId);

            if (data == null)
                ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.RawIdNotExists, HttpStatusCode.BadRequest);

            var update = Builders<VehicleFaultDataModel>.Update
                .Set(x => x.DataFrom, "人工录入")
                .Set(x => x.DataFromDesc, dataFromDesc)
                .Set(x => x.LastModifiedAt, DateTime.UtcNow);

            var result = context.VehicleFault.UpdateOneAsync(v => v.Id == data.Id, update).Result;

            if (result.ModifiedCount == 0)
                ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.UpdateVehicleFaultFailed, HttpStatusCode.InternalServerError);

            return true;
        }

        public async Task<bool> UpdateQISAsync(string rawId, VehicleFaultRelatedDataModel qisInfo)
        {
            var data = await context.VehicleFault.AsQueryable().FirstOrDefaultAsync(v => v.RawId == rawId);

            if (data == null)
                ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.RawIdNotExists, HttpStatusCode.BadRequest);

            var qisToUpdate = data.RelatedInfo?.OrderByDescending(x => x.PermanentCntrTime).FirstOrDefault();

            var update = Builders<VehicleFaultDataModel>.Update
                            .Set(x => x.LastModifiedAt, DateTime.UtcNow);

            if (qisToUpdate != null)
            {
                var index = data.RelatedInfo.IndexOf(qisToUpdate);
                qisToUpdate.CntrMesrType = qisInfo.CntrMesrType;
                qisToUpdate.CntrMesrReasonDesc = qisInfo.CntrMesrReasonDesc;
                qisToUpdate.PermanentCntr = qisInfo.PermanentCntr;
                qisToUpdate.PermanentCntrTime = qisInfo.PermanentCntrTime;
                qisToUpdate.LastModifiedAt = DateTime.UtcNow;
                update = update.Set(x => x.RelatedInfo[index], qisToUpdate);
            }
            else
            {
                qisInfo.LastModifiedAt = DateTime.UtcNow;
                if (data.RelatedInfo == null)
                    update = update.Set(x => x.RelatedInfo, new List<VehicleFaultRelatedDataModel> { qisInfo });
                else
                    update = update.Push(x => x.RelatedInfo, qisInfo);
            }

            var result = context.VehicleFault.UpdateOneAsync(v => v.Id == data.Id, update).Result;

            if (result.ModifiedCount == 0)
                ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.UpdateVehicleFaultFailed, HttpStatusCode.InternalServerError);

            return true;
        }

        public async Task<bool> AutoLinkQISAsync(string rawId, string qICNo, string qISNo)
        {
            var data = await context.VehicleFault.AsQueryable().FirstOrDefaultAsync(v => v.RawId == rawId);

            if (data == null)
                ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.RawIdNotExists, HttpStatusCode.BadRequest);

            if (!string.IsNullOrWhiteSpace(qICNo) || !string.IsNullOrWhiteSpace(qISNo))
            {
                var (qicData, qisData) = await rawQISService.GetQISInfo(qICNo, qISNo);

                if (qicData == null)
                {
                    ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.QISInfoCannotBeFound, HttpStatusCode.InternalServerError);
                }

                var exsited = data.RelatedInfo?.FirstOrDefault(r => r.QICNo == qicData.QIC_NO);

                if (exsited == null)
                {
                    if (data.RelatedInfo == null)
                        data.RelatedInfo = new List<VehicleFaultRelatedDataModel>();

                    data.RelatedInfo.Add(new VehicleFaultRelatedDataModel
                    {
                        QICNo = qisData?.QIC_NO,
                        QISNo = qisData?.QIS_NO,
                        CntrMesrReasonDesc = qisData?.CNTR_MESR_REASON_DESC,
                        CntrMesrType = qisData?.CNTR_MESR_TYPE,
                        PermanentCntr = qisData?.PERMANENT_CNTR,
                        PermanentCntrTime = qisData?.PERMANENT_CNTR_TIME,
                        QicDamPartArrDate = qicData?.DAM_PART_ARR_DATE,
                        QicDeal1STCode = qicData?.DEAL_1ST_CODE,
                        QicDeal1STDesc = qicData?.DEAL_1ST_DESC,
                        QicDeal2NDCode = qicData?.DEAL_2ND_CODE,
                        QicGDFlag = qicData?.GD_FLAG,
                        QicIsImportFlag = qicData?.IS_IMPORT_FLAG,
                        QicRank = qicData?.RANK,
                        QicReceiveDate = qicData?.RECEIVE_DATE,
                        QicReleaseDate = qicData?.RELEASE_DATE,
                        QicTheme = qicData?.THEME,
                        LastModifiedAt = DateTime.UtcNow
                    });
                }
                else
                {
                    exsited.QISNo = qisData?.QIS_NO;
                    exsited.CntrMesrReasonDesc = qisData?.CNTR_MESR_REASON_DESC;
                    exsited.CntrMesrType = qisData?.CNTR_MESR_TYPE;
                    exsited.PermanentCntr = qisData?.PERMANENT_CNTR;
                    exsited.PermanentCntrTime = qisData?.PERMANENT_CNTR_TIME;
                    exsited.QicDamPartArrDate = qicData?.DAM_PART_ARR_DATE;
                    exsited.QicDeal1STCode = qicData?.DEAL_1ST_CODE;
                    exsited.QicDeal1STDesc = qicData?.DEAL_1ST_DESC;
                    exsited.QicDeal2NDCode = qicData?.DEAL_2ND_CODE;
                    exsited.QicGDFlag = qicData?.GD_FLAG;
                    exsited.QicIsImportFlag = qicData?.IS_IMPORT_FLAG;
                    exsited.QicRank = qicData?.RANK;
                    exsited.QicReceiveDate = qicData?.RECEIVE_DATE;
                    exsited.QicReleaseDate = qicData?.RELEASE_DATE;
                    exsited.QicTheme = qicData?.THEME;
                    exsited.LastModifiedAt = DateTime.UtcNow;
                }

                var update = Builders<VehicleFaultDataModel>.Update
                    .Set(x => x.RelatedInfo, data.RelatedInfo)
                    .Set(x => x.LastModifiedAt, DateTime.UtcNow);

                var result = await context.VehicleFault.UpdateOneAsync(v => v.Id == data.Id, update);

                if (result.ModifiedCount == 0)
                    ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.UpdateVehicleFaultFailed, HttpStatusCode.InternalServerError);
            }

            return true;
        }

        public async Task<bool> UpdateLastPermanentCntrTimeAsync(string rawId, DateTime cntrTime)
        {
            var data = await context.VehicleFault.AsQueryable().FirstOrDefaultAsync(v => v.RawId == rawId);

            if (data == null)
                ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.RawIdNotExists, HttpStatusCode.BadRequest);

            var qisToUpdate = data.RelatedInfo?.OrderByDescending(x => x.PermanentCntrTime).FirstOrDefault();

            if (qisToUpdate == null)
            {
                ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.NoRelatedQisInfo, HttpStatusCode.BadRequest);
            }

            var index = data.RelatedInfo.IndexOf(qisToUpdate);
            qisToUpdate.PermanentCntrTime = cntrTime;
            qisToUpdate.LastModifiedAt = DateTime.UtcNow;

            var update = Builders<VehicleFaultDataModel>.Update
                .Set(x => x.LastModifiedAt, DateTime.UtcNow)
                .Set(x => x.RelatedInfo[index], qisToUpdate);

            var result = context.VehicleFault.UpdateOneAsync(v => v.Id == data.Id, update).Result;

            if (result.ModifiedCount == 0)
                ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.UpdateVehicleFaultFailed, HttpStatusCode.InternalServerError);

            return true;
        }

        public async Task<bool> DeleteRelatedInfo(string rawId)
        {
            var data = await context.VehicleFault.AsQueryable().FirstOrDefaultAsync(v => v.RawId == rawId);

            if (data == null)
                ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.RawIdNotExists, HttpStatusCode.BadRequest);

            var qisToDelete = data.RelatedInfo?.OrderByDescending(x => x.PermanentCntrTime).FirstOrDefault();
            if (qisToDelete == null)
            {
                ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.NoRelatedQisInfo, HttpStatusCode.BadRequest);
            }

            var update = Builders<VehicleFaultDataModel>.Update
                .Set(x => x.DataFrom, null)
                .Pull(x => x.RelatedInfo, qisToDelete)
                .Set(x => x.LastModifiedAt, DateTime.UtcNow);

            var result = context.VehicleFault.UpdateOneAsync(v => v.Id == data.Id, update).Result;

            if (result.ModifiedCount == 0)
                ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.UpdateVehicleFaultFailed, HttpStatusCode.InternalServerError);

            return true;
        }

        private async Task<IMongoQueryable<VehicleFaultDataModel>> GetFiltersAsync(
            List<string> carModels,
            List<DataSource> dataSource,
            List<string> carType, List<string> modelYears,
            List<string> partName, List<string> syndrome,
            DateTime? faultDateStart, DateTime? faultDateEnd)
        {
            var querable = context.VehicleFault.AsQueryable().Where(v => v.FaultDate >= DateTime.MinValue);

            if (carModels != null && carModels.Count > 0 && carModels.All(t => !string.IsNullOrWhiteSpace(t)))
                querable = querable.Where(v => carModels.Contains(v.CarModel));

            if (dataSource != null && dataSource.Count > 0)
                querable = querable.Where(v => dataSource.Contains(v.DataSource));

            if (carType != null && carType.Count > 0 && carType.All(t => !string.IsNullOrWhiteSpace(t)))
                querable = querable.Where(v => carType.Contains(v.CarType) || string.IsNullOrEmpty(v.CarType));

            if (modelYears != null && modelYears.Count > 0 && modelYears.All(m => !string.IsNullOrWhiteSpace(m)))
                querable = querable.Where(v => modelYears.Contains(v.ModelYear));

            if (partName != null && partName.Count > 0 && partName.All(p => !string.IsNullOrWhiteSpace(p)))
                querable = querable.Where(v => partName.Contains(v.PartName));

            if (faultDateStart != null)
                querable = querable.Where(v => v.FaultDate >= faultDateStart);

            if (faultDateEnd != null)
                querable = querable.Where(v => v.FaultDate <= faultDateEnd);

            if (syndrome != null && syndrome.Count > 0 && syndrome.All(s => !string.IsNullOrWhiteSpace(s)))
            {
                var syndromes = await syndromeService.GetSameSyndromeAsync(syndrome);

                var syndromeIds = syndromes?.Select(s => s.Id)?.ToHashSet();

                querable = querable.Where(v => syndromeIds.Contains(v.Syndrome));
            }

            return querable;
        }
    }
}