using MusicKG.HondaPlugins.DataAccess.DataModels.Raws;
using MusicKG.HondaPlugins.Services.Enums;
using MusicKG.HondaPlugins.Services.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MusicKG.HondaPlugins.DataAccess.Enums;

namespace MusicKG.HondaPlugins.Services.Repositories.Implementations
{
    public partial class VehicleFaultStatisticsService
    {
        public async Task<Dictionary<string, (List<List<string>> countTable, List<List<string>> amountTable)>> ExportDatas(BaseSearchModel search)
        {
            var dataList = await vehicleFaultDataService.ListExportableAsync(search.CarModel, search.Datasource, search.CarType, search.YearModels, search.PartName, search.Syndrome, null, null);

            if (dataList == null || dataList.Count == 0)
                return null;
             
            var result = dataList.GroupBy(x => x.ModelYear).ToDictionary(k => k.Key,
                v => ReConstructTable(v.ToList(), new List<string> { v.Key }, search.CarModel, search.CarType));

            return result;
        }

        public (List<List<string>> table_Count, List<List<string>> table_Amount) ReConstructTable(List<VehicleFaultServiceModel> dataList, List<string> yearModels, List<string> carModels, List<string> carTypes)
        {  
            var mqiList = dataList.Where(x => DataSourceMatch(x.DataSource, DataSourceType.MQI)).ToList();
            var salesDatas = rawSaleService.GetSalesDatas(carModels, carTypes, yearModels, null, null, null).Result;
            var salesCount = salesDatas.Count;
            var exportDatas = mqiList.GroupBy(x => new { x.PartName, SyndromeName = x.SyndromeModel?.Name })
                    .Select(x =>
                    {
                        var totalCount = x.Count();
                        var fee = x.Sum(x => float.TryParse(x.Features["总修理费用"], out float costRepair) ? costRepair : 0);
                        var relatedList = x.Where(r => r.RelatedInfo != null).SelectMany(a => a.RelatedInfo).Where(x => x.PermanentCntrTime != null).ToList();
                        var lastQisInfo = relatedList.OrderByDescending(r => r.PermanentCntrTime)?.FirstOrDefault();
                        var permanents = relatedList.GroupBy(a => a.PermanentCntrTime)
                                .Select(a =>
                                {
                                    //对策前不良件数
                                    var count = x.Count(b => b.FaultDate <= a.Key);
                                    return new PermanentCounterMove()
                                    {
                                        CountBeforeCntr = count,
                                        PermanentCntrTime = a.Key,
                                    };
                                }).ToList();
                        return new StatisticsExportServiceModel()
                        {
                            PartNo = string.Join(',',x.Select(a=>a.PartNo).Distinct()),
                            PartName = x.Key.PartName,
                            Syndrome = x.Key.SyndromeName,
                            AllHistoryMonth = x.GroupBy(a => a.FaultDate.ToString("yyyy-MM"))
                            .Select(a =>
                            {
                                var monthFee = a.Sum(x => float.TryParse(x.Features["总修理费用"], out float costRepair) ? costRepair : 0);
                                return new MonthCountAndAmount
                                {
                                    Month = a.Key,
                                    TotalCount = a.Count(),
                                    TotalAmount = monthFee
                                };
                            }).ToList(),
                            MaintainUnitPrice = totalCount > 0 ? (float)fee / totalCount : 0,
                            AccumulativeRate = salesCount > 0 ? (float)totalCount / salesCount : 0,
                            AccumulativeAmount = fee,
                            SingleUnitAmount = salesCount > 0 ? (float)fee / salesCount : 0,
                            MQITotalCount = totalCount,
                            LastPermanentCntrBeforeRate = salesCount > 0 ? (float)totalCount / salesCount : 0,
                            PermanentCntrs = permanents,
                            QICNo = lastQisInfo?.QICNo,
                            QISNo = lastQisInfo?.QISNo,
                            CntrMesrType  = lastQisInfo?.CntrMesrType,
                            CntrMesrReasonDesc = lastQisInfo?.CntrMesrReasonDesc,
                            PermanentCntr = lastQisInfo?.PermanentCntr,
                            PermanentCntrTime = lastQisInfo?.PermanentCntrTime,
                            GovCount = dataList.Where(x => x.DataSource == DataSource.GOV).Count(a => a.PartName == x.Key.PartName && a.Syndrome == x.Key.SyndromeName),
                            HotLineCount = dataList.Where(x => x.DataSource == DataSource.HOTLINE).Count(a => a.PartName == x.Key.PartName && a.Syndrome == x.Key.SyndromeName),
                            TechConsultingCount = dataList.Where(x => x.DataSource == DataSource.TECH_CONSULTING).Count(a => a.PartName == x.Key.PartName && a.Syndrome == x.Key.SyndromeName),
                            MediaCount = dataList.Where(x =>x.DataSource == DataSource.MEDIA_MAIN).Count(a => a.PartName == x.Key.PartName && a.Syndrome == x.Key.SyndromeName)
                        };
                    }).ToList();

            var monthList = exportDatas.SelectMany(x => x.AllHistoryMonth.Select(a => a.Month)).OrderBy(x => x).Distinct().ToList();
            var minDate = monthList.Count() ==0? DateTime.MinValue: DateTime.Parse($"{monthList.Min()}-01");
            var maxDate = monthList.Count() == 0 ? DateTime.MinValue : DateTime.Parse($"{monthList.Max()}-01").AddMonths(1);
            int monthSpan = (maxDate.Year - minDate.Year) * 12 + (maxDate.Month - minDate.Month);
            var monthHeader = Enumerable.Range(0, monthSpan).Select(x => minDate.AddMonths(x).ToString("yyyy-MM")).ToList();

            exportDatas.ForEach(item =>
            {
                var monthDict = item.AllHistoryMonth.ToDictionary(k => k.Month, v => v);
                item.AllHistoryMonth = Enumerable.Range(0, monthSpan).Select(x =>
                {
                    var monthKey = minDate.AddMonths(x).ToString("yyyy-MM");
                    return monthDict.ContainsKey(monthKey) ? monthDict[monthKey] : new MonthCountAndAmount() { Month = monthKey }; ;
                }).ToList();
                //对策数
                var count = item.PermanentCntrs.Count;
                if (count > 0)
                {
                    for (int i = 0; i < count - 1; i++)
                    {
                        var firstCntr = item.PermanentCntrs[i];
                        var nextCntr = item.PermanentCntrs[i + 1];
                        //获取对策后总销量
                        var cntrSalesCount = salesDatas.LongCount(x => x.ProductionDate >= firstCntr.PermanentCntrTime && x.ProductionDate < nextCntr.PermanentCntrTime);
                        item.PermanentCntrs[i].AfterRate = cntrSalesCount > 0 ? (float)(nextCntr.CountBeforeCntr - firstCntr.CountBeforeCntr) / cntrSalesCount : 0;
                    }

                    //最后一次对策
                    var lastCntr = item.PermanentCntrs[count - 1];
                    //获取对策后总销量
                    var lastCntrSalesCount = salesDatas.LongCount(x => x.ProductionDate >= lastCntr.PermanentCntrTime);
                    item.PermanentCntrs[count - 1].AfterRate = lastCntrSalesCount != 0?(float)(item.MQITotalCount - lastCntr.CountBeforeCntr) / lastCntrSalesCount : 0f;

                    //获取最后一次对策前总销量
                    var salesCountBeforeLastCntr = salesDatas.LongCount(x => x.ProductionDate < lastCntr.PermanentCntrTime);
                    item.LastPermanentCntrBeforeRate = salesCountBeforeLastCntr != 0? (float)lastCntr.CountBeforeCntr / salesCountBeforeLastCntr : 0f;
                }

            });
            #region 单台件数 table
            int i = 0;
            var tableBody_Count = exportDatas.GroupBy(x => new { x.PartName })
                .OrderByDescending(x => x.Sum(v => v.MQITotalCount)).SelectMany(x =>
            {
                ++i;
                var valueList = x.OrderByDescending(x => x.MQITotalCount).Select(x =>
               {
                   var monthCount = x.AllHistoryMonth.Select(m => m.TotalCount.ToString()).ToList();
                   var row = new List<string>()
                   {
                       "",
                       "",
                       "",
                       x.Syndrome
                   };
                   row.AddRange(monthCount);
                   row.AddRange(new List<string>() {
                    x.MQITotalCount.ToString(),
                    x.MaintainUnitPrice.ToString("f3"),
                    x.AccumulativeRate.ToString("P2")
                       });
                   var lastThreePermanencs = x.PermanentCntrs.Skip(x.PermanentCntrs.Count() - 3).Take(3).ToList();
                   while (lastThreePermanencs.Count() < 3)
                   {
                       lastThreePermanencs.Add(new PermanentCounterMove()
                       {
                           AfterRate = null,
                           PermanentCntrTime = null
                       });
                   }
                   var lastThree = lastThreePermanencs.Select(a => a.PermanentCntrTime?.ToString("yyyy-MM-dd")).ToList();

                   row.AddRange(lastThree);
                   row.Add(x.LastPermanentCntrBeforeRate.ToString("P2"));
                   row.AddRange(lastThreePermanencs.Select(a => a.AfterRate?.ToString("P2")).ToList());
                   row.AddRange(new List<string>() {
                    x.CntrMesrType?.ToString(),
                    x.QICNo,
                    x.QISNo,
                    x.CntrMesrReasonDesc,
                    x.PermanentCntr,
                    x.PermanentCntrTime?.ToString("yyyy-MM-dd"),
                    x.TechConsultingCount.ToString(),
                    x.HotLineCount.ToString(),
                    x.GovCount.ToString(),
                    x.MediaCount.ToString()
                       });
                   return row;
               }).ToList();
                var partNos = string.Join(',', x.SelectMany(a => a.PartNo.Split(',')).Distinct().OrderBy(x => x));
                if (x.Count() > 1)
                {
                    var summaryByPartNoRow = new List<string>()
                    {
                        $"{i}",
                        partNos,
                        x.Key.PartName,
                        "",
                        x.Sum(x=>x.MQITotalCount).ToString(),
                        ((double)x.Sum(x=>x.MaintainUnitPrice * x.MQITotalCount) / x.Sum(x=>x.MQITotalCount)).ToString("f3"),
                        x.Sum(x=>x.AccumulativeRate).ToString("P2"),
                        "","","","","","","","","","","","","",
                        x.Sum(x=>x.TechConsultingCount).ToString(),
                        x.Sum(x=>x.HotLineCount).ToString(),
                        x.Sum(x=>x.GovCount).ToString(),
                        x.Sum(x=>x.MediaCount).ToString()
                    };
                    summaryByPartNoRow.InsertRange(4, x.SelectMany(x => x.AllHistoryMonth).GroupBy(x => x.Month).Select(a => a.Sum(b => b.TotalCount).ToString()));

                    valueList.Insert(0, summaryByPartNoRow);
                }
                else
                {
                    valueList[0][0] = $"{i}";
                    valueList[0][1] = partNos;
                    valueList[0][2] = x.Key.PartName;
                }
                return valueList;
            });

            var header_Count = new List<List<string>>()
            {
                new List<string>()
                {
                    "排名","零件号","零件名","不良症状","截至目前的累计件数","维修单价",
                    "截至目前的累计发生率","对策情况(日期)","对策情况(日期)","对策情况(日期)",
                    "对策前不良率","对策后不良发生率","对策后不良发生率","对策后不良发生率",
                    "原因区分","QIC No","QIS No","不良原因","对策内容","对策日期","技术咨询","800","总局","媒体"
                },
                 new List<string>()
                {
                    "排名","零件号","零件名","不良症状","截至目前的累计件数","维修单价",
                    "截至目前的累计发生率","1次","2次","3次","对策前不良率","1次","2次","3次","原因区分","QIC No","QIS No","不良原因","对策内容","对策日期","技术咨询","800","总局","媒体"
                },
            };
            header_Count.ForEach(item => { item.InsertRange(4, monthHeader); });
            var count_title = new List<string>()
            {
                $"{string.Join(',',yearModels)} {string.Join(',',carModels)}_{string.Join(',', carTypes)} 单台件数"
            };
            header_Count.Insert(0, count_title);
            var table_Count = new List<List<string>>();
            table_Count.AddRange(header_Count);
            table_Count.AddRange(tableBody_Count);
            #endregion

            #region 单台金额table
            i = 0;
            var tableBody_Amount = exportDatas.GroupBy(x => new { x.PartName}).OrderByDescending(x => x.Sum(v=>v.AccumulativeAmount)).SelectMany(x =>
            {
                ++i;
                var valueList = x.OrderByDescending(x => x.AccumulativeAmount).Select(x =>
                {
                    var monthCount = x.AllHistoryMonth.Select(m => m.TotalAmount.ToString()).ToList();
                    var row = new List<string>()
                    {
                        "",
                        "",
                        "",
                        x.Syndrome
                    };
                    row.AddRange(monthCount);
                    row.AddRange(new List<string>() {
                        x.AccumulativeAmount.ToString("f3"),
                        x.MQITotalCount.ToString(),
                        x.SingleUnitAmount.ToString("f3")
                        });
                    var lastThreePermanencs = x.PermanentCntrs.Skip(x.PermanentCntrs.Count() - 3).Take(3).ToList();
                    while (lastThreePermanencs.Count < 3)
                    {
                        lastThreePermanencs.Add(new PermanentCounterMove()
                        {
                            AfterRate = null,
                            PermanentCntrTime = null
                        });
                    }
                    var lastThree = lastThreePermanencs.Select(a => a.PermanentCntrTime?.ToString("yyyy-MM-dd")).ToList();

                    row.AddRange(lastThree);
                    row.Add(x.LastPermanentCntrBeforeRate.ToString("P2"));
                    row.AddRange(lastThreePermanencs.Select(a => a.AfterRate?.ToString("P2")).ToList());
                    row.AddRange(new List<string>() {
                        x.CntrMesrType?.ToString(),
                        x.QICNo,
                        x.QISNo,
                        x.CntrMesrReasonDesc,
                        x.PermanentCntr,
                        x.PermanentCntrTime?.ToString("yyyy-MM-dd"),
                        x.TechConsultingCount.ToString(),
                        x.HotLineCount.ToString(),
                        x.GovCount.ToString(),
                        x.MediaCount.ToString()
                        });
                    return row;
                }).ToList();
                var partNos = string.Join(',', x.SelectMany(a => a.PartNo.Split(',')).Distinct().OrderBy(x=>x));
                if (x.Count() > 1)
                {
                    var summaryByPartNoRow = new List<string>()
                    {
                        $"{i}",
                        partNos,
                        x.Key.PartName,
                        "",
                        x.Sum(x=>x.AccumulativeAmount).ToString("f3"),
                        x.Sum(x=>x.MQITotalCount).ToString(),
                        x.Sum(x=>x.SingleUnitAmount).ToString("f3"),
                        "","","","","","","","","","","","","",
                        x.Sum(x=>x.TechConsultingCount).ToString(),
                        x.Sum(x=>x.HotLineCount).ToString(),
                        x.Sum(x=>x.GovCount).ToString(),
                        x.Sum(x=>x.MediaCount).ToString()
                    };
                    summaryByPartNoRow.InsertRange(4, x.SelectMany(x => x.AllHistoryMonth).GroupBy(x => x.Month).Select(a => a.Sum(b => b.TotalAmount).ToString()));

                    valueList.Insert(0, summaryByPartNoRow);
                }
                else
                {
                    valueList[0][0] = $"{i}";
                    valueList[0][1] = partNos;
                    valueList[0][2] = x.Key.PartName;
                }

                return valueList;
            });

            var header_Amount = new List<List<string>>()
            {
                new List<string>()
                {
                    "排名","零件号","零件名","不良症状","截至目前的累计金额","累计件数",
                    "单台累计金额","对策情况(日期)","对策情况(日期)","对策情况(日期)",
                    "对策前不良率","对策后不良发生率","对策后不良发生率","对策后不良发生率",
                    "原因区分","QIC No","QIS No","不良原因","对策内容",
                    "对策日期","技术咨询","800","总局","媒体"
                },
                 new List<string>()
                {
                     "排名","零件号","零件名","不良症状","截至目前的累计金额","累计件数",
                     "单台累计金额","1次","2次","3次","对策前不良率","1次","2次","3次",
                     "原因区分","QIC No","QIS No","不良原因","对策内容",
                     "对策日期","技术咨询","800","总局","媒体"
                },
            };
            header_Amount.ForEach(item => { item.InsertRange(4, monthHeader); });
            var amount_title = new List<string>()
            {
                $"{string.Join(',',yearModels)} {string.Join(',',carModels)}_{string.Join(',', carTypes)} 单台金额"
            };
            header_Amount.Insert(0, amount_title);

            var table_Amount = new List<List<string>>();
            table_Amount.AddRange(header_Amount);
            table_Amount.AddRange(tableBody_Amount);
            #endregion
            return (table_Count, table_Amount);  
        }
    }
}
