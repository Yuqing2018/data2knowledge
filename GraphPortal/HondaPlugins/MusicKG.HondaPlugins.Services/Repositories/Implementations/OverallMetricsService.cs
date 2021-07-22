using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MusicKG.HondaPlugins.DataAccess;
using MusicKG.HondaPlugins.DataAccess.DataModels.Business;
using MusicKG.HondaPlugins.DataAccess.DataModels.Warnings;
using MusicKG.HondaPlugins.DataAccess.Enums;
using MusicKG.HondaPlugins.DataAccess.Settings;
using MusicKG.HondaPlugins.Services.Enums;
using MusicKG.HondaPlugins.Services.Models;
using MusicKG.HondaPlugins.Services.Models.Overall;
using MusicKG.HondaPlugins.Services.Models.WarningTasks;
using MusicKG.Scheduler.DataAccess;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.Services.Repositories.Implementations
{
    public class OverallMetricsService : IOverallMetricsService
    {
        private readonly IHondaMongoDbContext context;
        private readonly ISchedulerDbContext schedulerContext;

        public OverallMetricsService(IHondaMongoDbContext context,
            ISchedulerDbContext schedulerContext)
        {
            this.context = context;
            this.schedulerContext = schedulerContext;
        }

        public async Task<List<TopFaultCountMetricsModel>> GetLastMonthTopMetrics(DateTime? start, DateTime? end)
        {
            start ??= DateTime.UtcNow.AddMonths(-1);
            end ??= DateTime.UtcNow;
            var pipeline = new List<BsonDocument>()
            {
                new BsonDocument
                {
                    { "$match",
                        new BsonDocument
                        {
                            { $"{nameof(VehicleFaultDataModel.FaultDate)}",
                                new BsonDocument
                                {
                                    { "$gte", start},
                                    {"$lt", end}
                                }
                            }
                        }
                    }
                },
                new BsonDocument
                {
                    { "$addFields",
                        new BsonDocument
                        {
                            {"SyndromeId",
                                new BsonDocument{
                                    { "$toObjectId",$"${nameof(VehicleFaultDataModel.Syndrome)}" } }
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
                            { "localField", "SyndromeId" },
                            { "foreignField","_id" },
                            {"as","SyndromeModel"}
                        }
                    }
                },
                new BsonDocument
                {
                    { "$unwind","$SyndromeModel"}
                },
                new BsonDocument
                {
                    {"$group",
                        new BsonDocument
                        {
                            { "_id",
                                new BsonDocument
                                {
                                    { "datasource", $"${nameof(VehicleFaultDataModel.DataSource)}"},
                                    { "carModel",$"${nameof(VehicleFaultDataModel.CarModel)}" },
                                    { "partName",$"${nameof(VehicleFaultDataModel.PartName)}"},
                                    { "syndrome","$SyndromeModel.Name"}
                                }
                            },
                            { "syndromeId",
                                new BsonDocument{
                                    { "$first", $"${nameof(VehicleFaultDataModel.Syndrome)}" }
                                }
                            },
                            { "count",
                                new BsonDocument
                                {
                                    { "$sum", 1 }
                                }
                            }
                        }
                    }
                },
                new BsonDocument
                {
                    { "$project",
                        new BsonDocument
                        {
                            {"_id",0 },
                            { "DataSource","$_id.datasource" },
                            { "CarModel","$_id.carModel" },
                            { "PartName","$_id.partName"},
                            { "SyndromeName","$_id.syndrome"},
                            { "Syndrome","$syndromeId"},
                            { "Count","$count"}
                        }
                    }
                },
                new BsonDocument
                {
                    {
                        "$sort",new BsonDocument{ { "Count", -1 } }
                    }
                },
                new BsonDocument
                {
                    { "$group",
                                new BsonDocument
                                {
                                    { "_id", "$DataSource" },
                                    { "items",
                                        new BsonDocument
                                        {
                                            { "$push","$$ROOT" }
                                        }
                                    }
                                }
                            }
                },
                new BsonDocument
                {
                    { "$project",
                        new BsonDocument
                        {
                            {"Results",
                                new BsonDocument
                                {
                                    { "$slice",new BsonArray{"$items", 10} }
                                }
                            }
                        }
                    }
                }
            };

            var results = await context.VehicleFault.Aggregate<TopFaultCountMetricsModel>(pipeline).ToListAsync();

            return results;
        }

        public async Task<List<MonthMetric>> GetVehicleFaultMetrics(DateTime? start, DateTime? end, DataSource datasource, StatisticalFrequency frequency)
        {
            start ??= DateTime.UtcNow.AddDays(-13);
            end ??= DateTime.UtcNow;
            var results = new List<MonthMetric>();
            var datas = await context.VehicleFault.AsQueryable()
                .Where(x => x.FaultDate >= start && x.FaultDate < end)
                .Select(x => new { DataSource = x.DataSource, FaultDate = x.FaultDate }).ToListAsync();

            datas = datas?.Where(x => x.DataSource == datasource)?.ToList();

            switch (frequency)
            {
                case StatisticalFrequency.日:
                    var daydict = datas.GroupBy(v => v.FaultDate.Date).ToDictionary(k => k.Key, v => v.Count());
                    var daySpan = (int)(end.Value.Date - start.Value.Date).TotalDays;
                    results = Enumerable.Range(0, daySpan + 1)
                        .Select(x =>
                        {
                            var day = start.Value.AddDays(x).Date;
                            return new MonthMetric()
                            {
                                Month = day.ToString("MM-dd"),
                                TotalCount = daydict.ContainsKey(day) ? daydict[day] : 0
                            };
                        }).ToList();
                    break;
                case StatisticalFrequency.周:
                    var weekdict = datas.GroupBy(v => WeekOfYear(v.FaultDate))
                        .ToDictionary(k => k.Key, v => v.Count());

                    var weeks = GetWeeks(start.Value, end.Value);

                    results = weeks?.Select(x => new MonthMetric
                    {
                        Month = x,
                        TotalCount = weekdict.ContainsKey(x) ? weekdict[x] : 0
                    })?.ToList();

                    break;

                case StatisticalFrequency.月:
                    var monthdict = datas.GroupBy(v => v.FaultDate.ToString("yyyy-MM"))
                        .ToDictionary(k => k.Key, v => v.Count());

                    var monthSpan = ((end.Value.Year - start.Value.Year) * 12) + end.Value.Month - start.Value.Month + 1;
                    results = Enumerable.Range(0, monthSpan).Select(x =>
                    {
                        var monthKey = start.Value.AddMonths(x).ToString("yyyy-MM");
                        return new MonthMetric()
                        {
                            Month = monthKey,
                            TotalCount = monthdict.ContainsKey(monthKey) ? monthdict[monthKey] : 0
                        };
                    }).ToList();
                    break;
            }

            return results;
        }

        private async Task<WarningRecordDataModel> GetRecordAsync(WarningTaskDetailDataModel detail)
        {
            var records = await context.WarningRecord.AsQueryable().Where(r => r.TaskId == detail.TaskId && r.PartName == detail.PartName && r.Syndrome == detail.Syndrome).ToListAsync();

            var latest = records?.OrderByDescending(r => r.WarningTime)?.FirstOrDefault();

            return latest;
        }

        public async Task<List<FocusedProjectMetricsModel>> GetFocusedProjectList()
        {
            var details = await context.WarningTaskDetail.AsQueryable().Where(d => d.IsFocused && d.FocusedDate != null)
                .OrderByDescending(d => d.FocusedDate).Take(100).ToListAsync();

            if (details == null || details.Count == 0)
                return new List<FocusedProjectMetricsModel>();

            var taskIds = new HashSet<string>();
            var syndromeIds = new HashSet<ObjectId>();

            details?.ForEach(d =>
            {
                taskIds.Add(d.TaskId);
                syndromeIds.Add(new ObjectId(d.Syndrome));
            });

            var taskDefines = await schedulerContext.JobTasks.AsQueryable().Where(t => taskIds.Contains(t.Id)).ToListAsync();
            
            var taskDic = taskDefines?.Select(w => new WarningTaskServiceModel(w.Id, w.TaskDefine))?.ToDictionary(k => k.Id, v => v);

            var syndromes = await context.Syndromes.AsQueryable().Where(s => syndromeIds.Contains(s.Id)).ToListAsync();

            var syndromeDic = syndromes?.ToDictionary(k => k.Id.ToString(), v => v);

            var result = new List<FocusedProjectMetricsModel>();

            foreach (var detail in details)
            {
                taskDic.TryGetValue(detail.TaskId, out var task);

                syndromeDic.TryGetValue(detail.Syndrome, out var syndrome);

                var record = await GetRecordAsync(detail);

                result.Add(new FocusedProjectMetricsModel
                {
                    CarModels = task?.CarModels,
                    YearModels = task?.YearModels,
                    Syndrome = detail.Syndrome,
                    SyndromeName = syndrome?.Name,
                    FocusedDate = detail.FocusedDate,
                    CancelFocusedDate = detail.CancelFocusedDate,
                    PartName = detail.PartName,
                    RiskLevel = record?.RiskMetrics?.RiskLevel,
                    AIRiskLevel = record?.RiskMetrics?.AIRiskLevel,
                    CntrStatus = record?.RiskMetrics?.PermanentCntrStatus
                });
            }

            return result;
        }

        public async Task<Dictionary<string, List<DayMetric>>> GetLastWeekWarningMetrics(DateTime? start, DateTime? end)
        {
            start ??= DateTime.UtcNow.AddDays(-6);
            end ??= DateTime.UtcNow;
            var records = await context.WarningRecord.Find(x => x.MatchedFrequency
            && (x.IsMultipleWarning || x.IsRiskWarning || x.IsAgainWarning)
            && x.WarningTime >= start && x.WarningTime < end).ToListAsync();

            var results = Enum.GetValues(typeof(WarningType)).Cast<WarningType>().ToDictionary(k => k.ToString(), v =>
              {
                  List<WarningRecordDataModel> selectedRecords = null;
                  switch (v)
                  {
                      case WarningType.多发预警:
                          selectedRecords = records.Where(x => x.IsMultipleWarning).ToList();
                          break;
                      case WarningType.风险预警:
                          selectedRecords = records.Where(x => x.IsRiskWarning).ToList();
                          break;
                      case WarningType.再发预警:
                          selectedRecords = records.Where(x => x.IsAgainWarning
                          ).ToList();
                          break;
                  }

                  var metricsDict = selectedRecords.GroupBy(x => x.WarningTime.Date).ToDictionary(k => k.Key, v => v.Count());
                  var daySpan = (int)(end.Value.Date - start.Value.Date).TotalDays;
                  var metrics = Enumerable.Range(0, daySpan + 1)
                      .Select(x =>
                      {
                          var day = start.Value.AddDays(x).Date;
                          return new DayMetric()
                          {
                              Day = day.ToString("MM-dd"),
                              Count = metricsDict.ContainsKey(day) ? metricsDict[day] : 0
                          };
                      }).ToList();
                  return metrics;
              });

            return results;
        }

        public async Task<Dictionary<string, List<DayMetric>>> GetLastWeekWarningMetricsByCarType(DateTime? start, DateTime? end, WarningType warningType)
        {
            start ??= DateTime.UtcNow.AddDays(-6);
            end ??= DateTime.UtcNow;
            var querable = context.WarningRecord.AsQueryable().Where(x => x.MatchedFrequency && x.WarningTime >= start && x.WarningTime < end);

            switch (warningType)
            {
                case WarningType.多发预警:
                    querable = querable.Where(x => x.IsMultipleWarning);
                    break;
                case WarningType.风险预警:
                    querable = querable.Where(x => x.IsRiskWarning);
                    break;
                case WarningType.再发预警:
                    querable = querable.Where(x => x.IsAgainWarning);
                    break;
            }

            var records = await querable.ToListAsync();
            var taskIds = records.Select(x => x.TaskId).Distinct().ToList();
            var tasks = await schedulerContext.JobTasks.Find(x => x.JobId == ConstantSettings.WarningCalculationJobId && taskIds.Contains(x.Id)).ToListAsync();
            var selectRecords = records.Select(x =>
            {
                var task = tasks.FirstOrDefault(t => t.Id == x.TaskId);
                return new WarningRecordServiceModel(x)
                {
                    WarningTask = new WarningTaskServiceModel(task.Id, task.TaskDefine)
                };
            }).ToList();

            var results = selectRecords.GroupBy(x => x.WarningTask.CarModels.Count > 1 ? "多车款" : x.WarningTask.CarModels?.FirstOrDefault()).ToDictionary(k => k.Key,
                v =>
                {

                    var metricsDict = v.GroupBy(a => a.WarningTime.Date).ToDictionary(k => k.Key, v => v.Count());
                    var daySpan = (int)(end.Value.Date - start.Value.Date).TotalDays;
                    var metrics = Enumerable.Range(0, daySpan + 1)
                        .Select(x =>
                        {
                            var day = start.Value.AddDays(x).Date;
                            return new DayMetric()
                            {
                                Day = day.ToString("MM-dd"),
                                Count = metricsDict.ContainsKey(day) ? metricsDict[day] : 0
                            };
                        }).ToList();
                    return metrics;
                });
            return results;
        }

        private List<string> GetWeeks(DateTime start, DateTime end)
        {
            var result = new List<string>();

            var ci = CultureInfo.InvariantCulture;

            var date = start;

            while (date <= end)
            {
                var dateWeek = ci.Calendar.GetWeekOfYear(date, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek);

                var endOfYear = new DateTime(date.Year, 12, 31);

                int endWeek;

                if (endOfYear > end)
                    endWeek = ci.Calendar.GetWeekOfYear(end, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek);
                else
                    endWeek = ci.Calendar.GetWeekOfYear(endOfYear, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek);

                for (int i = dateWeek; i <= endWeek; i++)
                {
                    result.Add($"{date.Year}年第{i}周");
                }

                date = new DateTime(date.Year + 1, 1, 1);
            }

            return result;
        }

        public static string WeekOfYear(DateTime dt)
        {
            var ci = CultureInfo.InvariantCulture;
            var week = ci.Calendar.GetWeekOfYear(dt, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek);

            return $"{dt.Year}年第{week}周";
        }
    }
}
