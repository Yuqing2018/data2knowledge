using MusicKG.HondaPlugins.DataAccess;
using MusicKG.HondaPlugins.DataAccess.DataModels.Warnings;
using MusicKG.HondaPlugins.DataAccess.Enums;
using MusicKG.HondaPlugins.Services.Helpers;
using MusicKG.HondaPlugins.Services.Models;
using MusicKG.HondaPlugins.Services.Models.WarningTasks;
using MusicKG.HondaPlugins.Services.Resources;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using MusicKG.HondaPlugins.DataAccess.Extensions;
using Newtonsoft.Json;
using MusicKG.HondaPlugins.Services.Enums;

namespace MusicKG.HondaPlugins.Services.Repositories.Implementations
{
    public partial class WarningRecordService : IWarningRecordService
    {
        private readonly IHondaMongoDbContext context;
        private readonly ISyndromeService syndromeService;
        private readonly IWarningTaskService warningTaskService;
        private readonly IWarningTaskDetailService warningTaskDetailService;

        private readonly ILogger<WarningRecordService> logger;

        /// <summary>
        /// WarningTaskDetail service constructor.
        /// </summary>
        /// <param name="context">Honda mongodb context.</param>
        public WarningRecordService(
            IHondaMongoDbContext context,
            ISyndromeService syndromeService,
            IWarningTaskService warningTaskService,
            IWarningTaskDetailService warningTaskDetailService,
            ILogger<WarningRecordService> logger)
        {
            this.context = context;
            this.syndromeService = syndromeService;
            this.warningTaskService = warningTaskService;
            this.warningTaskDetailService = warningTaskDetailService;
            this.logger = logger;
        }

        public async Task<Tuple<long, IEnumerable<WarningRecordServiceModel>>> GetPageListAsync(
            ProcessStatus? isHandled, [Required] string detailId, int from, int? size)
        {
            var detail = await warningTaskDetailService.GetAsync(detailId);

            var querable = GetFilterRecordsAsync(
                string.IsNullOrWhiteSpace(detail.PartName) ? null : new List<string>() { detail.PartName },
                string.IsNullOrWhiteSpace(detail.Syndrome) ? null : new List<string>() { detail.Syndrome });

            querable = querable.Where(r => r.TaskId == detail.WarningTask.Id);

            if (isHandled.HasValue)
            {
                if (isHandled == ProcessStatus.待处理)
                    querable = querable.Where(w => w.ConfirmRecord == null);
                else
                    querable = querable.Where(w => w.ConfirmRecord != null);
            }

            querable = querable.OrderByDescending(x => x.WarningTime);
            var totalCount = await querable.LongCountAsync();

            if (from > 0)
                querable = querable.Skip(from);

            if (size.HasValue)
                querable = querable.Take(size.Value);

            var warningRecords = await querable.ToListAsync();

            var warningTasks = new Dictionary<string, WarningTaskServiceModel>();

            var results = warningRecords.Select(x => new WarningRecordServiceModel(x) { WarningTask = detail.WarningTask }).ToList();


            var syndromes = await syndromeService.GetDistinctSyndromeAsync(results.Select(x => x.Syndrome).ToList());
            var syndromeDict = syndromes.ToDictionary(k => k.Id, v => v);
            results.ForEach(x =>
            {
                x.SyndromeBadGrade = syndromeDict[x.Syndrome].BadGrade;
                x.SyndromeModel = syndromeDict[x.Syndrome];
            });

            return new Tuple<long, IEnumerable<WarningRecordServiceModel>>(totalCount, results);
        }

        /// <summary>
        /// 获取任务所属的详情列表
        /// </summary>
        /// <param name="taskId">任务Id</param>
        /// <returns></returns>
        public async Task<List<WarningRecordDataModel>> GetTaskWarningRecords(string taskId)
        {
            return await context.WarningRecord.AsQueryable().Where(x => x.TaskId == taskId).ToListAsync();
        }

        /// <summary>
        /// 添加确认记录
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="confirm"></param>
        /// <returns></returns>
        public async Task<bool> UpdateConfirmRecord(string recordId, WarningConfirmRecordDataModel confirm)
        {
            var record = await context.WarningRecord.AsQueryable().FirstOrDefaultAsync(x => x.Id == new ObjectId(recordId));

            if (record == null)
            {
                var message = MusicKGHondaPluginsMessage.WarningRecordCannotBeFound;
                logger?.LogError(message);
                ErrorHelper.ThrowException(message, HttpStatusCode.BadRequest);
            }

            var update = Builders<WarningRecordDataModel>.Update
                .Set(u => u.ConfirmRecord, confirm);
            try
            {
                var result = await context.WarningRecord.UpdateOneAsync(x => x.Id == new ObjectId(recordId), update);

                var flag = await warningTaskDetailService.UpdatePendingAndTotalCount(record.TaskId, record.PartName, record.Syndrome, null, null);

                return result.IsAcknowledged && result.ModifiedCount == 1;
            }
            catch (MongoCommandException e)
            {
                var message = MusicKGHondaPluginsMessage.WarningRecordConfirmUpdateFailed;
                logger?.LogError(e, message);
                ErrorHelper.ThrowException(message, HttpStatusCode.BadRequest);
            }

            return false;
        }

        private async Task<bool> UpdateMatchedFrequency(HashSet<ObjectId> recordIds)
        {
            var update = Builders<WarningRecordDataModel>.Update
                .Set(u => u.MatchedFrequency, true);
            try
            {
                var result = await context.WarningRecord.UpdateManyAsync(x => recordIds.Contains(x.Id), update);

                return result.IsAcknowledged && result.ModifiedCount == recordIds.Count;
            }
            catch (MongoCommandException e)
            {
                var message = MusicKGHondaPluginsMessage.WarningRecordMatchFrequencyUpdateFailed;
                logger?.LogError(e, message);
                ErrorHelper.ThrowException(message, HttpStatusCode.BadRequest);
            }

            return false;
        }

        public async Task<bool> UpdateAIRiskLevel(string recordId, RiskLevel? riskLevel, bool usedForModel)
        {
            var update = Builders<WarningRecordDataModel>.Update
                .Set(u => u.RiskMetrics.AIRiskLevel, riskLevel)
                .Set(u => u.RiskMetrics.UsedForModel, usedForModel);
            try
            {
                var result = await context.WarningRecord.UpdateOneAsync(x => x.Id == new ObjectId(recordId), update);

                return result.IsAcknowledged && result.ModifiedCount == 1;
            }
            catch (MongoCommandException e)
            {
                var message = MusicKGHondaPluginsMessage.WarningRecordConfirmUpdateFailed;
                logger?.LogError(e, message);
                ErrorHelper.ThrowException(message, HttpStatusCode.BadRequest);
            }

            return false;
        }

        public async Task<bool> SaveAsync(WarningRecordDataModel createModel)
        {
            try
            {
                await context.WarningRecord.InsertOneAsync(createModel);
            }
            catch (MongoWriteException e)
            {
                var message = MusicKGHondaPluginsMessage.WarningRecordCreateFailed;
                logger?.LogError(e, message);
                ErrorHelper.ThrowException(message, HttpStatusCode.BadRequest);
            }

            return true;
        }

        public async Task RemoveWarningRecords(List<ObjectId> recordIds)
        {
            await context.WarningRecord.DeleteManyAsync(r => recordIds.Contains(r.Id));
        }

        public async Task RemoveWarningDetails(List<ObjectId> detailIds)
        {
            await context.WarningTaskDetail.DeleteManyAsync(d => detailIds.Contains(d.Id));
        }

        public async Task<(List<ObjectId> recordIds, List<ObjectId> detailIds)> SaveWarningRecordsAsync(string taskId, List<WarningRecordDataModel> records)
        {
            await context.WarningRecord.InsertManyAsync(records);

            logger.LogInformation($"insert {records.Count} records into {nameof(WarningRecordDataModel)} collection");

            #region 保存和更新预警任务跟踪列表
            var all_records = await GetTaskWarningRecords(taskId);

            var allDetails = await context.WarningTaskDetail.AsQueryable().Where(x => x.TaskId == taskId).ToListAsync();

            var detailsDic = allDetails?.GroupBy(x => new { x.TaskId, x.PartName, x.Syndrome })?
                .ToDictionary(k => $"{k.Key.TaskId}_{k.Key.PartName}_{k.Key.Syndrome}", v => v.ToList());

            var details = new List<WarningTaskDetailDataModel>();

            var matchedIds = new HashSet<ObjectId>();

            var pendingCountUpdateValues = new List<WarningTaskDetailDataModel>();

            all_records.GroupBy(x => new { x.TaskId, x.PartName, x.Syndrome })?.ToList()?
                .ForEach(x =>
                {
                    detailsDic.TryGetValue($"{x.Key.TaskId}_{x.Key.PartName}_{x.Key.Syndrome}", out var existDetail);

                    var detailModel = existDetail?.FirstOrDefault() ?? new WarningTaskDetailDataModel()
                    {
                        TaskId = x.Key.TaskId,
                        PartNo = string.Join(',', x.Select(p => p.PartNo).Distinct().OrderBy(p => p)),
                        PartName = x.Key.PartName,
                        Syndrome = x.Key.Syndrome,
                        Frequency = WarningFrequency.每日,
                        LastFrequencySetDate = DateTime.UtcNow
                    };
                    //判断预警时间是否符合预警频率
                    var tmpMatchedIds = x.Select(r =>
                    {
                        var flag = IsWarningTimeMatchFrequency(detailModel, r.WarningTime);
                        return flag ? r.Id : ObjectId.Empty;
                    })?.Where(x => x != ObjectId.Empty)?.ToList();

                    if (tmpMatchedIds != null && tmpMatchedIds.Count > 0)
                        tmpMatchedIds.ForEach(x => matchedIds.Add(x));

                    details.Add(detailModel);

                    var occuredRecords = x.Where(y => (y.MatchedFrequency || tmpMatchedIds.Contains(y.Id)) && (y.IsMultipleWarning || y.IsRiskWarning || y.IsAgainWarning)).ToList();

                    var pendingCount = occuredRecords.Where(y => y.ConfirmRecord == null).Count();
                    var totalCount = occuredRecords.Count;

                    pendingCountUpdateValues.Add(new WarningTaskDetailDataModel
                    {
                        TaskId = x.Key.TaskId,
                        PartName = x.Key.PartName,
                        Syndrome = x.Key.Syndrome,
                        PendingCount = pendingCount,
                        TotalCount = totalCount
                    });
                });

            logger.LogInformation("Start updating matched frequency for warning records...");
            UpdateMatchedFrequency(matchedIds).GetAwaiter().GetResult();
            logger.LogInformation("Finish updating matched frequency for warning records...");

            var detailsToSave = details.Where(x => x.Id == ObjectId.Empty).ToList();

            if (detailsToSave != null && detailsToSave.Count > 0)
                await context.WarningTaskDetail.InsertManyAsync(detailsToSave);

            logger.LogInformation($"insert {detailsToSave.Count} records into {nameof(WarningTaskDetailDataModel)} collection");
            #endregion

            logger.LogInformation("Start updating pending count for warning task details...");
            await UpdatePendingCountAndTotalCountAsync(pendingCountUpdateValues);
            logger.LogInformation("Finish updating pending count for warning task details...");

            return (records.Select(r => r.Id).ToList(), detailsToSave.Select(d => d.Id).ToList());
        }

        private async Task UpdatePendingCountAndTotalCountAsync(List<WarningTaskDetailDataModel> pendingCountUpdateValues)
        {
            var updateModel = pendingCountUpdateValues.Select(p =>
            {
                var filterBuilder = Builders<WarningTaskDetailDataModel>.Filter;
                var updateBuilder = Builders<WarningTaskDetailDataModel>.Update;

                return new UpdateOneModel<WarningTaskDetailDataModel>(
                    filterBuilder.Eq(x => x.TaskId, p.TaskId) & filterBuilder.Eq(x => x.PartName, p.PartName) & filterBuilder.Eq(x => x.Syndrome, p.Syndrome),
                    updateBuilder.Set(x => x.PendingCount, p.PendingCount).Set(x => x.TotalCount, p.TotalCount));
            });

            await context.WarningTaskDetail.BulkWriteAsync(updateModel);
        }

        public async Task<List<bool>> UpdatePendingCountAndTotalCount()
        {
            var all_records = await context.WarningRecord.AsQueryable().ToListAsync();
            var result = all_records.GroupBy(x => new { x.TaskId, x.PartName, x.Syndrome })
               .Select(x =>
               {
                   var occuredRecords = x.Where(x => x.MatchedFrequency && (x.IsMultipleWarning || x.IsRiskWarning || x.IsAgainWarning)).ToList();

                   var pendingCount = occuredRecords.Where(x => x.ConfirmRecord == null).Count();
                   var totalCount = occuredRecords.Count;
                   var flag = warningTaskDetailService.UpdatePendingAndTotalCount(x.Key.TaskId, x.Key.PartName, x.Key.Syndrome, pendingCount, totalCount).Result;
                   return flag;
               }).ToList();

            return result;
        }
        public async Task<(RiskLevel?, RiskLevel?)> GetRiskLevel(BaseSearchModel search, WarningUnit warningUnit)
        {
            //获取符合条件的的预警任务
            var tasks = await warningTaskService.GetWarningTasksAsync(warningUnit, null, null, search.CarModel, search.CarType, search.YearModels, 0, null);

            var taskIds = tasks?.Item2?
                .Where(task => task.WarningIndex.Any(x => x.WarningType == WarningType.风险预警))?
                .Select(task => task.Id)?.ToList();

            var records_querable = GetAllCaculatedRecordsAsync(search.PartName, search.Syndrome);

            var riskMetrics = await records_querable.Where(x => taskIds.Contains(x.TaskId) && x.RiskMetrics != null).OrderByDescending(x => x.WarningTime).Select(x => x.RiskMetrics).FirstOrDefaultAsync();

            return (riskMetrics?.RiskLevel, riskMetrics?.AIRiskLevel);
        }

        public async Task<List<WarningTaskConfirmRecordServiceModel>> ListConfirmRecordAsync(BaseSearchModel search)
        {
            var querable = GetFilterRecordsAsync(search.PartName, search.Syndrome);
            var warningRecords = await querable.Where(x => x.ConfirmRecord != null).ToListAsync();

            var tasks = (await warningTaskService.GetWarningTasksAsync(null, null, null,
                search.CarModel, search.CarType, search.YearModels, 0, null))?.Item2?.ToList();

            var syndromes = await syndromeService.GetDistinctSyndromeAsync(warningRecords.Select(x => x.Syndrome).ToList());
            var syndrome_dict = syndromes.ToDictionary(k => k.Id.ToString(), v => v.Name);
            var results = (from r in warningRecords
                           join t in tasks on r.TaskId.ToString() equals t.Id into recordlist
                           from rec in recordlist.DefaultIfEmpty()
                           select new WarningTaskConfirmRecordServiceModel()
                           {
                               CarModels = rec.CarModels,
                               CarTypes = rec.CarTypes,
                               YearModels = rec.YearModels,
                               WarningUnit = rec.WarningUnit,
                               WarningTime = r.WarningTime,
                               ConfirmRecord = r.ConfirmRecord,
                               Syndrome = syndrome_dict[r.Syndrome],
                               PartNo = r.PartNo,
                               PartName = r.PartName
                           }).ToList();

            return results;
        }

        public async Task<WarningConfirmRecordDataModel> GetLastConfirmRecordAsync(string taskId, string partName, string syndrome)
        {
            var querable = context.WarningRecord.AsQueryable().Where(x => x.TaskId == taskId && (x.IsMultipleWarning || x.IsRiskWarning || x.IsAgainWarning));

            if (!string.IsNullOrWhiteSpace(partName))
            {
                querable = querable.Where(x => x.PartName == partName);
            }
            if (!string.IsNullOrWhiteSpace(syndrome))
            {
                querable = querable.Where(x => x.Syndrome == syndrome);
            }
            var last_record = await querable.Where(x => x.ConfirmRecord != null)
                .OrderByDescending(x => x.WarningTime)
                .ThenByDescending(x => x.ConfirmRecord.LastConfirmdTime)
                .Select(x => x.ConfirmRecord).FirstOrDefaultAsync();

            return last_record;
        }

        private IMongoQueryable<WarningRecordDataModel> GetFilterRecordsAsync(
            List<string> partNames,
            List<string> syndromes)
        {
            var querable = context.WarningRecord.AsQueryable();
            querable = querable.Where(x => x.MatchedFrequency && (x.IsMultipleWarning || x.IsRiskWarning || x.IsAgainWarning));

            if (partNames != null && partNames.Count > 0)
                querable = querable.Where(x => partNames.Contains(x.PartName));
            if (syndromes != null && syndromes.Count > 0)
                querable = querable.Where(x => syndromes.Contains(x.Syndrome));

            return querable;
        }

        private IMongoQueryable<WarningRecordDataModel> GetAllCaculatedRecordsAsync(
            List<string> partNames,
            List<string> syndromes)
        {
            var querable = context.WarningRecord.AsQueryable();

            if (partNames != null && partNames.Count > 0)
                querable = querable.Where(x => partNames.Contains(x.PartName));
            if (syndromes != null && syndromes.Count > 0)
                querable = querable.Where(x => syndromes.Contains(x.Syndrome));

            return querable;
        }

        private bool IsWarningTimeMatchFrequency(WarningTaskDetailDataModel detail, DateTime warningTime)
        {
            var result = false;
            var firstCalulateDay = detail.LastFrequencySetDate.AddDays(1).Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            switch (detail.Frequency)
            {
                case WarningFrequency.每日:
                    result = true;
                    break;
                case WarningFrequency.每周:
                    result = warningTime > firstCalulateDay && warningTime.DayOfWeek == firstCalulateDay.DayOfWeek;
                    break;
                case WarningFrequency.每月:
                    result = warningTime > firstCalulateDay && warningTime.Day == firstCalulateDay.Day;
                    break;
                case WarningFrequency.每季:
                    var quarterMonthList = Enumerable.Range(0, 4).Select(x => firstCalulateDay.Month + x * 3).ToList();
                    result = warningTime > firstCalulateDay && quarterMonthList.Contains(warningTime.Month) && warningTime.Day == firstCalulateDay.Day;
                    break;
                case WarningFrequency.指定日期:
                    result = warningTime.Date == detail.SpecifiedDate.Value.Date;
                    break;
                case WarningFrequency.不报警:
                default:
                    result = false;
                    break;
            }

            return result;
        }

        /// <summary>
        /// 导出确认记录
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<List<List<string>>> ExportAsync(BaseSearchModel search)
        {
            var results = await ListConfirmRecordAsync(search);

            var table = new List<List<string>>();
            var header = new List<string>() { "序号", "预警单位", "车款", "车型", "年款", "零件号", "零件名", "不良症状", "预警时间", "确认人", "确认时间", "确认记录", "推进分类", "对策状态", "超标影响" };
            int i = 0;
            var contents = results.OrderByDescending(x => x.ConfirmRecord.LastConfirmdTime).Select(x =>
             {
                 return new List<string>()
                 {
                    (++i).ToString(),
                    x.WarningUnit.ToString(),
                    x.CarModels == null ? "":string.Join(',', x.CarModels),
                    x.CarTypes == null ? "":string.Join(',', x.CarTypes),
                    x.YearModels == null ? "":string.Join(',', x.YearModels),
                    x.PartNo,
                    x.PartName,
                    x.Syndrome,
                    x.WarningTime.ToString("yyyy-MM-dd hh:mm:ss"),
                    x.ConfirmRecord.LastConfirmdUser,
                    x.ConfirmRecord.LastConfirmdTime.ToString("yyyy-MM-dd hh:mm:ss"),
                    x.ConfirmRecord.ConfirmedMessage,
                    x.ConfirmRecord.PushStatus?.ToString(),
                    x.ConfirmRecord.PermanentCntrStatus?.ToString(),
                    x.ConfirmRecord.IsExcessive.HasValue?x.ConfirmRecord.IsExcessive.Value ?"是":"否" : ""
                 };
             });
            table.Add(header);
            table.AddRange(contents);
            return table;
        }

        public async Task<List<WarningMessageServiceModel>> MessagesToSend()
        {
            var records = await context.WarningRecord.AsQueryable().Where(x => x.MatchedFrequency
            && (x.IsMultipleWarning || x.IsRiskWarning || x.IsAgainWarning)
            && x.WarningTime >= DateTime.UtcNow.AddDays(-7)
            && x.WarningTime < DateTime.UtcNow).ToListAsync();

            var messageList = records.GroupBy(x => x.TaskId).Select(x =>
            {
                var task = warningTaskService.GetAsync(x.Key).Result;
                var message = new WarningMessageServiceModel()
                {
                    TaskId = x.Key.ToString(),
                    CarModels = task.CarModels,
                    CarTypes = task.CarTypes,
                    YearModels = task.YearModels,
                    Count = x.Count(),
                    LastWeek = x.GroupBy(d => d.WarningTime.Date).OrderBy(d => d.Key).ToDictionary(k => k.Key.Day, v => v.Count())
                };
                return message;
            }).ToList();

            return messageList;
        }
    }
}
