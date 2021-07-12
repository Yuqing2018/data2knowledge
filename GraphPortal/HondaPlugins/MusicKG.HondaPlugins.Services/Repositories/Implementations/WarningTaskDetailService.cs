using MusicKG.HondaPlugins.DataAccess.DataModels.Warnings;
using MusicKG.HondaPlugins.DataAccess.Enums;
using MusicKG.HondaPlugins.DataAccess;
using MusicKG.HondaPlugins.Services.Resources;
using MusicKG.HondaPlugins.Services.Helpers;
using MusicKG.HondaPlugins.Services.Models.WarningTasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;
using MusicKG.HondaPlugins.Services.Enums;
using MusicKG.HondaPlugins.DataAccess.DataModels.Business;
using MusicKG.HondaPlugins.DataAccess.Settings;

namespace MusicKG.HondaPlugins.Services.Repositories.Implementations
{
    public partial class WarningTaskDetailService : IWarningTaskDetailService
    {
        private readonly IHondaMongoDbContext context;
        private readonly ISyndromeService syndromeService;
        private readonly IWarningTaskService warningTaskService;
        private readonly ILogger<WarningTaskDetailService> logger;

        /// <summary>
        /// WarningTaskDetail service constructor.
        /// </summary>
        /// <param name="context">Honda mongodb context.</param>
        public WarningTaskDetailService(
            IHondaMongoDbContext context,
            ISyndromeService syndromeService,
            IWarningTaskService warningTaskService,
            ILogger<WarningTaskDetailService> logger)
        {
            this.context = context;
            this.warningTaskService = warningTaskService;
            this.syndromeService = syndromeService;
            this.logger = logger;
        }

        private IMongoQueryable<WarningTaskDetailDataModel> GetTaskDetailFilterAsync(List<string> taskIds, List<string> partNames,List<string> syndromes, FocusType? focusType)
        {
            var querable = context.WarningTaskDetail.AsQueryable().Where(x => taskIds.Contains(x.TaskId));

            if (partNames!= null && partNames.Count > 0)
            {
                querable = querable.Where(x => partNames.Contains(x.PartName));
            }

            if (syndromes != null && syndromes.Count > 0)
            {
                querable = querable.Where(x => syndromes.Contains(x.Syndrome));
            }

            if (focusType.HasValue)
            {
                switch (focusType.Value)
                {
                    case FocusType.已关注:
                        querable = querable.Where(x => x.IsFocused == true);
                        break;
                    case FocusType.已退出:
                        querable = querable.Where(x => x.IsFocused == false && x.CancelFocusedDate != null);
                        break;
                    case FocusType.未关注:
                        querable = querable.Where(x => x.IsFocused == false && x.CancelFocusedDate == null);
                        break;
                }
            }

            return querable;
        }

        public async Task<Tuple<long, IEnumerable<WarningTaskDetailServiceModel>>> GetPageListAsync(List<string> taskIds, WarningUnit? warningUnit, string createdBy, List<string> carModels, List<string> carTypes, List<string> yearModels, List<string> partNames, List<string> syndromes, FocusType? focusType, int from, int? size)
        {
            var tasks = (await warningTaskService.GetWarningTasksAsync(warningUnit, null, createdBy, carModels, carTypes, yearModels, 0, null, taskIds))?.Item2;
            
            var selectedTaskIds = tasks?.Select(x => x.Id)?.ToList();

            var querable = GetTaskDetailFilterAsync(selectedTaskIds, partNames, syndromes, focusType);
            var totalCount = querable.Count();
            querable = querable.OrderByDescending(x => x.PendingCount).ThenByDescending(x => x.TotalCount);
            if (from > 0)
                querable = querable.Skip(from);

            if (size.HasValue)
                querable = querable.Take(size.Value);

            var taskDetails = await querable.ToListAsync();
            
            var result_syndromeIds = taskDetails.Select(x => x.Syndrome);
            var result_syndromes = await syndromeService.GetDistinctSyndromeAsync(result_syndromeIds.ToList());
            var syndromeDict = result_syndromes.ToDictionary(k => k.Id, v => v);

            var results = (from r in taskDetails
                           join t in tasks on r.TaskId.ToString() equals t.Id into tasklist
                           from task in tasklist.DefaultIfEmpty()
                           select new WarningTaskDetailServiceModel()
                           {
                               Id = r.Id.ToString(),
                               PartNo = r.PartNo,
                               PartName = r.PartName,
                               Syndrome = r.Syndrome,
                               WarningTask = task,
                               Frequency = r.Frequency,
                               SpecifiedDate = r.SpecifiedDate,
                               FocusType = r.IsFocused ? FocusType.已关注 : 
                               (!r.FocusedDate.HasValue ? FocusType.未关注 : FocusType.已退出),
                               PendingCount = r.PendingCount,
                               TotalCount = r.TotalCount,
                               SyndromeModel = syndromeDict[r.Syndrome],
                               FocusedDate = r.FocusedDate,
                               CancelFocusedDate = r.CancelFocusedDate
                           }).ToList();

            return new Tuple<long, IEnumerable<WarningTaskDetailServiceModel>>(totalCount, results);
        }

        /// <summary>
        /// 导出风险预警列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<RiskRecordMetricsExportModel>> ExportRiskRecordListAsync(List<string> taskIds, WarningUnit? warningUnit, string createdBy, List<string> carModels, List<string> carTypes, List<string> yearModels, List<string> partNames, List<string> syndromes, FocusType? focusType)
        {
            const string detailName = "Details", syndromeIdName = "SyndromeId", 
                syndromeModelName = "SyndromeModel", syndromeNameName = "SyndromeName",
                detailTaskId = "detail_TaskId", detailPartName = "detail_PartName", detailSyndrome = "detail_Syndrome";

            var tasks = (await warningTaskService.GetWarningTasksAsync(warningUnit, null, createdBy, carModels, carTypes, yearModels, 0, null,taskIds))?.Item2?.ToList();

            var taskDic = tasks?.ToDictionary(k => k.Id, v => v);

            if (taskDic?.Count == 0)
                return new List<RiskRecordMetricsExportModel>();

            var result_taskIds = taskDic?.Keys?.ToList();
            var taskArray = new BsonArray();
            taskArray.AddRange(result_taskIds.Select(x => new BsonString(x)));
            var matchs = new List<BsonDocument>()
            {
                new BsonDocument
                {
                    { "$match",
                        new BsonDocument
                        {
                            {$"{nameof(WarningTaskDetailDataModel.TaskId)}",
                                new BsonDocument
                                {
                                    {"$in", taskArray}
                                }
                            }
                        }
                    }
                },
            };

            if (partNames!= null && partNames.Count > 0)
            {
                var partNameArray = new BsonArray();
                partNameArray.AddRange(partNames.Select(x => new BsonString(x)));
                matchs.Add(new BsonDocument
                {
                    { "$match" ,
                        new BsonDocument
                        {
                            {$"{nameof(WarningRecordDataModel.PartName)}",
                                new BsonDocument
                                { 
                                    {"$in", partNameArray} 
                                } 
                            }
                        }
                    }
                });
            }

            if (syndromes != null && syndromes.Count > 0)
            {
                var realSyndromes = await syndromeService.GetSameSyndromeAsync(syndromes);

                if (realSyndromes != null || realSyndromes.Count > 0)
                {
                    var syndromeArray = new BsonArray();
                    syndromeArray.AddRange(realSyndromes.Select(x => new BsonString(x.Id.ToString())));
                    matchs.Add(new BsonDocument
                    {
                        { "$match" ,
                            new BsonDocument
                            {
                                {$"{nameof(WarningRecordDataModel.Syndrome)}",
                                    new BsonDocument
                                    {
                                        {"$in", syndromeArray}
                                    }
                                }
                            }
                        }
                    });
                }
            }

            var sort = new BsonDocument("$sort", new BsonDocument(nameof(WarningRecordDataModel.WarningTime), -1));

            var lookups = new List<BsonDocument>()
            { 
                #region syndrome lookup
                new BsonDocument
                {
                    { "$addFields",
                        new BsonDocument
                        {
                            { $"{syndromeIdName}",
                                new BsonDocument{
                                    { "$toObjectId", $"${nameof(WarningRecordDataModel.Syndrome)}" } 
                                }
                            }
                        }
                    }
                },
                new BsonDocument
                {
                    { "$lookup",
                        new BsonDocument
                        {
                            { "from", $"Honda_{nameof(context.Syndromes)}" },
                            { "localField", $"{syndromeIdName}"},
                            { "foreignField","_id" },
                            { "as", $"{syndromeModelName}"}
                        }
                    }
                },
                new BsonDocument
                {
                    { "$unwind",$"${syndromeModelName}"}
                },
                #endregion

                #region projects

                new BsonDocument("$project", new BsonDocument
                {
                    { "MultipleMetrics", 0 },
                    { "AgainMetrics", 0 }
                }),

                #endregion

                #region group

                new BsonDocument("$group",
                    new BsonDocument
                    {
                        { "_id", new BsonDocument {
                            { nameof(WarningRecordDataModel.TaskId), $"${nameof(WarningRecordDataModel.TaskId)}" },
                            { nameof(WarningRecordDataModel.PartName), $"${nameof(WarningRecordDataModel.PartName)}" },
                            { syndromeNameName, $"${syndromeModelName}.{nameof(SyndromeDataModel.Name)}" }
                        } },
                        { nameof(WarningRecordDataModel.Syndrome), new BsonDocument("$first", $"${nameof(WarningRecordDataModel.Syndrome)}") },
                        { nameof(WarningRecordDataModel.PartNo), new BsonDocument("$first", $"${nameof(WarningRecordDataModel.PartNo)}") },
                        { nameof(WarningRecordDataModel.WarningTime), new BsonDocument("$first", $"${nameof(WarningRecordDataModel.WarningTime)}") },
                        { nameof(WarningRecordDataModel.ConfirmRecord), new BsonDocument("$push", $"${nameof(WarningRecordDataModel.ConfirmRecord)}") },
                        { nameof(WarningRecordDataModel.RiskMetrics), new BsonDocument("$first", $"${nameof(WarningRecordDataModel.RiskMetrics)}") },
                        { nameof(WarningRecordDataModel.IsMultipleWarning), new BsonDocument("$first", $"${nameof(WarningRecordDataModel.IsMultipleWarning)}") },
                        { nameof(WarningRecordDataModel.IsRiskWarning), new BsonDocument("$first", $"${nameof(WarningRecordDataModel.IsRiskWarning)}") },
                        { nameof(WarningRecordDataModel.IsAgainWarning), new BsonDocument("$first", $"${nameof(WarningRecordDataModel.IsAgainWarning)}") },
                        { nameof(WarningRecordDataModel.MatchedFrequency), new BsonDocument("$first", $"${nameof(WarningRecordDataModel.MatchedFrequency)}") },
                        { nameof(SyndromeDataModel.BadGrade), new BsonDocument("$first", $"${syndromeModelName}.{nameof(SyndromeDataModel.BadGrade)}") }
                    }),

                #endregion
            };

            var focusTypeMatch = new List<BsonDocument>();

            if (focusType.HasValue)
            {
                lookups.Add(new BsonDocument("$lookup",
                    new BsonDocument
                    {
                        { "from", $"Honda_Warning_{nameof(HondaMongoDbContext.WarningTaskDetail)}" },
                        { "let", new BsonDocument {
                            { detailTaskId, $"$_id.{nameof(WarningRecordDataModel.TaskId)}" },
                            { detailPartName, $"$_id.{nameof(WarningRecordDataModel.PartName)}" },
                            { detailSyndrome, $"${nameof(WarningRecordDataModel.Syndrome)}" } }
                        },
                        { "pipeline", new BsonArray {
                            new BsonDocument("$match", new BsonDocument("$expr",
                                new BsonDocument("$and", new BsonArray
                                    {
                                        new BsonDocument("$eq", new BsonArray
                                            { $"${nameof(WarningTaskDetailDataModel.TaskId)}", $"$${detailTaskId}" }),
                                        new BsonDocument("$eq", new BsonArray
                                            { $"${nameof(WarningTaskDetailDataModel.PartName)}", $"$${detailPartName}" }),
                                        new BsonDocument("$eq", new BsonArray
                                            { $"${nameof(WarningTaskDetailDataModel.Syndrome)}", $"$${detailSyndrome}" })
                                    }))),
                                new BsonDocument("$project", new BsonDocument
                                    {
                                        { nameof(WarningTaskDetailDataModel.IsFocused), 1 },
                                        { nameof(WarningTaskDetailDataModel.CancelFocusedDate), 1 },
                                        { nameof(WarningTaskDetailDataModel.FocusedDate), 1 }
                                    })}
                        },
                        { "as", detailName }
                    }));
                lookups.Add(new BsonDocument("$unwind", new BsonDocument("path", $"${detailName}")));

                switch (focusType.Value)
                {
                    case FocusType.已关注:
                        focusTypeMatch.Add(new BsonDocument
                        {
                            { "$match" ,
                                new BsonDocument
                                {
                                    {$"{detailName}.{nameof(WarningTaskDetailDataModel.IsFocused)}",true }
                                }
                            }
                        });
                        break;
                    case FocusType.已退出:
                        focusTypeMatch.Add(new BsonDocument
                        {
                            { "$match" ,
                                new BsonDocument
                                {
                                    { $"{detailName}.{nameof(WarningTaskDetailDataModel.IsFocused)}",false },
                                    { $"{detailName}.{nameof(WarningTaskDetailDataModel.CancelFocusedDate)}",
                                        new BsonDocument{ { "$ne", BsonNull.Value }  }
                                    }
                                }
                            }
                        });
                        break;
                    case FocusType.未关注:
                        focusTypeMatch.Add(new BsonDocument
                        {
                            { "$match" ,
                                new BsonDocument
                                {
                                    { $"{detailName}.{nameof(WarningTaskDetailDataModel.IsFocused)}", false },
                                    { $"{detailName}.{nameof(WarningTaskDetailDataModel.FocusedDate)}", BsonNull.Value }
                                }
                            }
                        });
                        break;
                }
            }

            var project = new BsonDocument("$project", new BsonDocument
                {
                    { nameof(WarningRecordDataModel.TaskId), $"$_id.{nameof(WarningRecordDataModel.TaskId)}" },
                    { nameof(WarningRecordDataModel.PartName), $"$_id.{nameof(WarningRecordDataModel.PartName)}" },
                    { syndromeNameName, $"$_id.{syndromeNameName}" },
                    { nameof(WarningRecordDataModel.WarningTime), 1 },
                    { nameof(SyndromeDataModel.BadGrade), 1 },
                    { nameof(WarningRecordDataModel.PartNo), 1 },
                    { nameof(WarningRecordDataModel.RiskMetrics), 1 },
                    { nameof(WarningRecordDataModel.ConfirmRecord), 1},
                    { "_id", 0 }
                });

            var limit = new BsonDocument("$limit", ConstantSettings.MaxExportLine);

            var pipeline = new List<BsonDocument>();
            pipeline.AddRange(matchs);
            pipeline.Add(sort);
            pipeline.AddRange(lookups);
            pipeline.AddRange(focusTypeMatch);
            pipeline.Add(project);
            pipeline.Add(limit);

            var queryResult = await context.WarningRecord.Aggregate<RiskRecordExportModel>(pipeline).ToListAsync();

            var result = queryResult?.Select(r =>
            {
                var task = taskDic[r.TaskId];

                return new RiskRecordMetricsExportModel
                {
                    TaskName = task.Name,
                    CarModels = task.CarModels,
                    CarTypes = task.CarTypes,
                    YearModels = task.YearModels,
                    PartNo = r.PartNo,
                    PartName = r.PartName,
                    Syndrome = r.SyndromeName,
                    BadGrade = r.BadGrade,
                    WarningTime = r.WarningTime,
                    ConfirmRecord = r.ConfirmRecord?.Where(x => x != null)?.FirstOrDefault(),
                    RiskMetrics = r.RiskMetrics
                };
            })?.ToList();

            return result;
        }

        public async Task<WarningTaskDetailServiceModel> GetAsync(string id)
        {
            var result = await context.WarningTaskDetail.AsQueryable().FirstOrDefaultAsync(x => x.Id == new ObjectId(id));

            if (result == null)
            {
                var message = MusicKGHondaPluginsMessage.WarningTaskDoesNotExist;
                logger?.LogError(message);
                ErrorHelper.ThrowException(message, HttpStatusCode.BadRequest);
            }

            var task = await warningTaskService.GetAsync(result.TaskId);
            var syndromes =  await syndromeService.GetSameSyndromeAsync(new List<string> { result.Syndrome });

            return new WarningTaskDetailServiceModel()
            {
                Id = result.Id.ToString(),
                WarningTask = task,
                PartNo = result.PartNo,
                PartName = result.PartName,
                Syndrome = result.Syndrome,
                SyndromeModel = syndromes.FirstOrDefault(),
                Frequency = result.Frequency,
                SpecifiedDate = result.SpecifiedDate,
                FocusType = result.IsFocused ? FocusType.已关注 :(!result.FocusedDate.HasValue ? FocusType.未关注 : FocusType.已退出),
                Settings = result.Settings,
                LastFrequencySetDate = result.LastFrequencySetDate,
                FocusedDate = result.FocusedDate,
                CancelFocusedDate = result.CancelFocusedDate
            };

        }

        /// <summary>
        /// 更新预警频率
        /// </summary>
        /// <param name="id"></param>
        /// <param name="frequency"></param>
        /// <param name="isFocused"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(string id, WarningFrequency frequency, bool isFocused, DateTime? date)
        {
            var detailModel = await GetAsync(id);
            if (frequency != WarningFrequency.指定日期)
                date = null;

            var update = Builders<WarningTaskDetailDataModel>.Update
                .Set(u => u.Frequency, frequency)
                .Set(u=>u.SpecifiedDate, date)
                .Set(u => u.IsFocused, isFocused)
                .Set(u=> u.LastFrequencySetDate, DateTime.UtcNow);

            if (isFocused)
            {//重点关注
                update = update.Set(u => u.FocusedDate, DateTime.UtcNow);
            }
            else if (detailModel.FocusedDate.HasValue)
            {//取消重点关注
                update = update.Set(u => u.CancelFocusedDate, DateTime.UtcNow);
            }

            try
            {
                var result = await context.WarningTaskDetail.UpdateOneAsync(x => x.Id == new ObjectId(id), update);

                return result.IsAcknowledged && result.ModifiedCount == 1;
            }
            catch (MongoCommandException e)
            {
                var message = MusicKGHondaPluginsMessage.WarningTaskDetailUpdateFailed;
                logger?.LogError(e, message);
                ErrorHelper.ThrowException(message, HttpStatusCode.BadRequest);
            }
            return false;
        }
        /// <summary>
        /// 更新跳转设置
        /// </summary>
        /// <param name="id"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public async Task<bool> UpdateSettingsAsync(string id, WarningTaskDetailSettingDataModel settings)
        {
            var update = Builders<WarningTaskDetailDataModel>.Update
                .Set(u => u.Settings, settings);
            try
            {
                var result = await context.WarningTaskDetail.UpdateOneAsync(x => x.Id == new ObjectId(id), update);

                return result.IsAcknowledged && result.ModifiedCount == 1;
            }
            catch (MongoCommandException e)
            {
                var message = MusicKGHondaPluginsMessage.WarningTaskDetailUpdateFailed;
                logger?.LogError(e, message);
                ErrorHelper.ThrowException(message, HttpStatusCode.BadRequest);
            }
            return false;
        }

        public async Task<List<List<string>>> ExportAsync(List<string> taskIds, WarningUnit? warningUnit, string createdBy, List<string> carModels, List<string> carTypes, List<string> yearModels, List<string> partNames, List<string> syndromes, FocusType? focusType)
        {
            var results = (await GetPageListAsync(taskIds, warningUnit, createdBy, carModels, carTypes, yearModels, partNames,syndromes, focusType, 0, null)).Item2;
            
            var table = new List<List<string>>();
            var header = new List<string>() { "序号", "预警单元", "零件号", "零件名", "不良症状", "车款", "车型", "年款", "所属任务", "预警频率","指定日期","待处理条数", "总预警条数", "重点关注", "创建人" };
            int i = 0;
            var contents = results.OrderBy(x => x.WarningTask.CreateBy).Select(x =>
            {
                return new List<string>()
                {
                    (++i).ToString(),
                    x.WarningTask.WarningUnit.ToString(),
                    x.PartNo,
                    x.PartName,
                    x.SyndromeModel.Name,
                    x.WarningTask?.CarModels == null ? "" : string.Join(',', x.WarningTask.CarModels),
                    x.WarningTask?.CarTypes == null ? "" : string.Join(',', x.WarningTask.CarTypes),
                    x.WarningTask?.YearModels == null ? "" : string.Join(',', x.WarningTask.YearModels),
                    x.WarningTask.Name,
                    x.Frequency.ToString(),
                    x.SpecifiedDate.ToString(),
                    x.PendingCount.ToString(),
                    x.TotalCount.ToString(),
                    x.FocusType.ToString(),
                    x.WarningTask.CreateBy
                };
            });
            table.Add(header);
            table.AddRange(contents);
            return table;
        }

        public async Task<bool> UpdatePendingAndTotalCount(string taskId, string partName, string syndrome, int? pendingCount, int? totalCount)
        {
            if (!pendingCount.HasValue || !totalCount.HasValue)
            {
                var querable = context.WarningRecord.AsQueryable().Where(x => x.TaskId == taskId && x.MatchedFrequency && (x.IsMultipleWarning || x.IsRiskWarning || x.IsAgainWarning));
               
                if (!string.IsNullOrWhiteSpace(partName))
                    querable = querable.Where(x => x.PartName == partName);

                if (!string.IsNullOrWhiteSpace(syndrome))
                    querable = querable.Where(x => x.Syndrome == syndrome);

                var recordList = await querable.ToListAsync();

                pendingCount = recordList.Count(x=>x.ConfirmRecord == null);
                totalCount = recordList.Count;
            }

            try
            {
                var taskDetail = await GetTaskDetail(taskId,partName, syndrome);
                var update = Builders<WarningTaskDetailDataModel>.Update
                       .Set(u => u.PendingCount, pendingCount)
                       .Set(u => u.TotalCount, totalCount);

                var result = await context.WarningTaskDetail.UpdateOneAsync(t => t.Id == taskDetail.Id, update);
                return result.IsAcknowledged && result.ModifiedCount == 1;
            }
            catch (MongoCommandException e)
            {
                var message = MusicKGHondaPluginsMessage.WarningTaskDetailUpdateFailed;
                logger?.LogError(e, message);
                ErrorHelper.ThrowException(message, HttpStatusCode.BadRequest);
            }
            return false;
        }
        /// <summary>
        /// 获取某一个预警跟踪记录Id,不存在返回null.
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="partNo"></param>
        /// <param name="partName"></param>
        /// <param name="syndrome"></param>
        /// <returns></returns>
        public async Task<WarningTaskDetailDataModel> GetTaskDetail(string taskId, string partName, string syndrome)
        {
            var detail = await context.WarningTaskDetail.Find(x
                => x.TaskId == taskId
                && x.PartName == partName
                && x.Syndrome == syndrome).FirstOrDefaultAsync();
            return detail;
        }

        public async Task<int> UnhandledCountAsync(string createdBy)
        {
            var tasks = (await warningTaskService.GetWarningTasksAsync(null, WarningTaskStatus.预警中, createdBy, null, null, null, 0, null))?.Item2;
            var taskIds = tasks.Select(x => x.Id).ToList();

            var count = await context.WarningRecord.AsQueryable().CountAsync(x => x.MatchedFrequency
            && (x.IsMultipleWarning || x.IsRiskWarning || x.IsAgainWarning)
            && taskIds.Contains(x.TaskId) && x.ConfirmRecord == null);

            return count;
        }
    }
}
