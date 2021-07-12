using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MusicKG.HondaPlugins.DataAccess.Enums;
using MusicKG.HondaPlugins.Services.Models;
using MusicKG.HondaPlugins.VehicleFault.Rest.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.VehicleFault.Rest.Controllers
{
    public partial class VehicleFaultStatisticsController
    {
        /// <summary>
        /// 单台Top数据导出
        /// </summary>
        /// <param name="carModel">车款</param>
        /// <param name="carType">车型</param>
        /// <param name="yearModels">年款</param>
        /// <returns></returns>
        [HttpGet("report")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Export(
            [FromQuery] List<string> carModel,
            [FromQuery] List<string> carType,
            [FromQuery] List<string> yearModels)
        {
            var searchModel = new BaseSearchModel(carModel, carType, yearModels, null, null);
            var yearDict = await statisticsService.ExportDatas(searchModel);
             
            if (yearDict == null || yearDict.Count == 0)
                return NoContent();

            var excelSheetList = new List<ExcelSheet>();
            var orderDict = yearDict.OrderByDescending(x => x.Key);
            foreach (var item in orderDict)
            {
                #region 获取表头合并单元格
                /*countTable, amountTable 两表格的表头合并方式是一样的*/
                var count_header = item.Value.countTable.Take(3).ToList();
                var count_header_mergeCells = GetMergeCellsOfTableHeader(count_header);
                var amount_header = item.Value.amountTable.Take(3).ToList();
                var amount_header_mergeCells = GetMergeCellsOfTableHeader(amount_header);
                #endregion

                var excelsheets = new List<ExcelSheet>()
                {
                    new ExcelSheet()
                    {
                        SheetName = $"{item.Key}_单台件数统计表",
                        Table = item.Value.countTable,
                        MergeCells = count_header_mergeCells,
                        AddFilter = true
                    },
                    new ExcelSheet()
                    {
                        SheetName = $"{item.Key}_单台金额统计表",
                        Table = item.Value.amountTable,
                        MergeCells = amount_header_mergeCells,
                        AddFilter = true
                    }
                };
                excelSheetList.AddRange(excelsheets);
            }

            var content = ExcelHelper.ConvertTableToExcelBytes(excelSheetList);

            var nowTimeStr = DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
            var fileName = FormattableString.Invariant($@"统计分析_{nowTimeStr}.xlsx");
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(content, contentType, fileName);
        }
        /// <summary>
        /// 获取表头合并单元格
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        private List<(string cell1Name, string cell2Name)> GetMergeCellsOfTableHeader(List<List<string>> header)
        {
            var headerRow = header[1];
            var mergeCells_header = new List<(string cell1Name, string cell2Name)>
            {
                (ExcelHelper.GetCellReference(0, 1), ExcelHelper.GetCellReference(0, headerRow.Count))
            };

            for (int i = 0; i < headerRow.Count; i++)
            {
                var col = i + 1;
                if (i == headerRow.IndexOf("对策情况(日期)") || i == headerRow.IndexOf("对策后不良发生率"))
                {
                    var cell1 = ExcelHelper.GetCellReference(1, col);
                    var cell2 = ExcelHelper.GetCellReference(1, col + 2);
                    mergeCells_header.Add((cell1, cell2));
                    i += 2;
                }
                else
                {
                    mergeCells_header.Add((ExcelHelper.GetCellReference(1, col), ExcelHelper.GetCellReference(2, col)));
                }
            }
            return mergeCells_header;
        }

        /// <summary>
        /// 图表数据导出
        /// </summary>
        /// <param name="carModel">车款</param>
        /// <param name="carType">车型</param>
        /// <param name="yearModels">年款</param>
        /// <param name="partName">零件名</param>
        /// <param name="syndrome">不良症状</param>
        /// <param name="datasource"></param>
        /// <returns></returns>
        [HttpGet("report/chart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ExportChart(
            [FromQuery][Required] List<string> carModel,
            [FromQuery] List<string> carType,
            [FromQuery] List<string> yearModels,
            [FromQuery] string partName,
            [FromQuery] string syndrome,
            [FromQuery] List<DataSource> datasource)
        {
            var searchModel = new BaseSearchModel(carModel, carType, yearModels,
                string.IsNullOrWhiteSpace(partName) ? null : new List<string> { partName },
                string.IsNullOrWhiteSpace(syndrome) ? null : new List<string> { syndrome },
                datasource);
            var table = await ExportStatisticMetrics(searchModel);

            if (table == null || table.Count == 0)
                return NoContent();

            var excelsheets = new List<ExcelSheet>()
            {
                new ExcelSheet()
                {
                    SheetName = "图表统计数据",
                    Table = table,
                    MergeCells = null,
                    AddFilter = true
                }
            };
            var content = ExcelHelper.ConvertTableToExcelBytes(excelsheets);

            var nowTimeStr = DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
            var fileName = FormattableString.Invariant($@"图表统计分析_{nowTimeStr}.xlsx");
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(content, contentType, fileName);
        }

        private async Task<List<List<string>>> ExportStatisticMetrics(BaseSearchModel search)
        {
            var table = new List<List<string>>();
            var table_bounder = new List<string>();
            var metrics = await statisticsService.CalculateMetrics(search);
            if (search.PartName != null && search.PartName.Count > 0)
            {
                var syndromeMetrics = await statisticsService.GetSyndromeMetrics(search);
                var syndrome_header = new List<string>() { "车款", "合计", "不良症状名称", "风险等级", "不良件数" };
                var syndrome_body = new List<List<string>>();
                syndromeMetrics.ForEach(item =>
                {
                    if (1 == item.Metrics.Count)
                    {
                        var firstMetric = item.Metrics.FirstOrDefault();
                        syndrome_body.Add(new List<string>() { item.CarModel, item.TotalCount.ToString(), firstMetric.Name, firstMetric.RiskLevel, firstMetric.TotalCount.ToString() });
                    }
                    else
                    {
                        syndrome_body.Add(new List<string>() { item.CarModel, item.TotalCount.ToString(), "","", "" });
                        var detail_syndromes = item.Metrics.Select(x => new List<string>() { "", "", x.Name, x.RiskLevel, x.TotalCount.ToString() });
                        syndrome_body.AddRange(detail_syndromes);
                    }
                });
                table.Add(new List<string>() { $"零件分析：{search.PartName.FirstOrDefault()}" });
                table.Add(syndrome_header);
                table.AddRange(syndrome_body);
                table.Add(table_bounder);
            }

            if (search.Syndrome != null && search.Syndrome.Count > 0)
            {
                var partNameMetrics = await statisticsService.GetPartNameMetrics(search);
                var partName_header = new List<string>() { "零件名", "不良件数" };
                var partName_body = partNameMetrics.Select(x => new List<string>()
                { x.Name, x.TotalCount.ToString() });
                
                table.Add(new List<string>() { $"不良症状分析：{metrics.ProductDistribution?.Keys.FirstOrDefault()}" });
                table.Add(partName_header);
                table.AddRange(partName_body);
                table.Add(table_bounder);
            }

            #region 生产分布

            var product_header = new List<string>() { "不良症状" };
            if (metrics.ProductDistribution != null)
            {
                var product_monthes = metrics.ProductDistribution?.FirstOrDefault().Value.Select(x => x.Month).ToList();

                if (product_monthes != null && product_monthes.Count > 0)
                    product_header.AddRange(product_monthes);
            }

            var product_body = metrics.ProductDistribution?.Select(x =>
            {
                var row = new List<string>()
                {
                    x.Key,
                };
                row.AddRange(x.Value.Select(v => v.TotalCount.ToString()));
                return row;
            }).ToList();

            table.Add(new List<string>() { "生产分布" });
            table.Add(product_header);
            if(product_body != null)
                table.AddRange(product_body);
            table.Add(table_bounder);

            #endregion

            #region 故障分布
            var fault_header = new List<string>() { "不良症状" };
            if (metrics.FaultDistribution != null)
            {
                var fault_monthes = metrics.FaultDistribution?.FirstOrDefault().Value.Select(x => x.Month).ToList();

                if (fault_monthes != null && fault_monthes.Count > 0)
                    fault_header.AddRange(fault_monthes);
            }
            
            var fault_body = metrics.FaultDistribution?.Select(x =>
            {
                var row = new List<string>()
                {
                    x.Key,
                };
                row.AddRange(x.Value.Select(v => v.TotalCount.ToString()));
                return row;
            }).ToList();

            table.Add(new List<string>() { "故障分布" });
            table.Add(fault_header);
            if (fault_body != null)
                table.AddRange(fault_body);
            table.Add(table_bounder);
            #endregion

            #region 经过分布
            var faultSpan_header = new List<string>() { "不良症状" };
            var monthSpans = metrics.FaultSpanDistribution?.FirstOrDefault().Value.Select(x => x.Month).ToList();
            if(monthSpans != null && monthSpans.Count > 0)
                faultSpan_header.AddRange(monthSpans);

            var faultSpan_body = metrics.FaultSpanDistribution?.Select(x =>
            {
                var row = new List<string>()
                {
                    x.Key,
                };
                row.AddRange(x.Value.Select(v => v.TotalCount.ToString()));
                return row;
            }).ToList();
            table.Add(new List<string>() { "经过月分布" });
            table.Add(faultSpan_header);
            if (faultSpan_body != null)
                table.AddRange(faultSpan_body);
            table.Add(table_bounder);
            #endregion

            #region 不良率分布
            var defectRate_header = new List<string>() { "经过月" };
            var years = metrics.DefectRateDistribution?.Keys.OrderBy(x => x).ToList();
            if (years != null && years.Count > 0)
                defectRate_header.AddRange(years);
            if (metrics.DefectRateDistribution == null)
            {
                table.Add(new List<string>() { "按经过时间不良率分布" });
                table.Add(defectRate_header);
                table.Add(table_bounder);
            }
            else
            {
                var monthDict = metrics.DefectRateDistribution.SelectMany(x => x.Value.Select(v => new
                {
                    year = x.Key,
                    month = int.TryParse(Regex.Replace(v.Month, @"[^0-9]+", ""), out int monthSpan) ? monthSpan : 0,
                    rate = v.DefectRate
                })).GroupBy(x => x.month).ToDictionary(v => v.Key, v => v.ToDictionary(vk => vk.year, vv => vv.rate));
                var month = monthDict.Keys.ToList();
                var min = month.Min();
                var span = month.Max() - min + 1;
                var defectRate_body = Enumerable.Range(min, span).Select(x =>
                {
                    var flag = monthDict.ContainsKey(x);
                    var row = new List<string>()
                    {
                    $"{x}个月"
                    };

                    var yearsRow = years.Select(y => (flag && monthDict[x].ContainsKey(y)) ? monthDict[x][y].ToString("P") : "");
                    row.AddRange(yearsRow);
                    return row;
                });

                table.Add(new List<string>() { "按经过时间不良率分布" });
                table.Add(defectRate_header);
                table.AddRange(defectRate_body);
                table.Add(table_bounder);
            }
            #endregion

            #region 按对策时间的不良率分布
            var defectRateByCntr_header = new List<string>() { "经过月" };
            if (metrics.DefectRateByCntrDistribution == null)
            {
                table.Add(new List<string>() { "按对策时间不良率分布" });
                table.Add(defectRateByCntr_header);
                table.Add(table_bounder);
            }
            else
            {
                var cntrYears = metrics.DefectRateByCntrDistribution.Keys.OrderBy(x => x).SelectMany(x => new List<string>() { x, "对策时间" }).ToList();
                if (cntrYears != null && cntrYears.Count > 0)
                    defectRateByCntr_header.AddRange(cntrYears);

                var cntrMonthDict = metrics.DefectRateByCntrDistribution.SelectMany(x => x.Value.Select(v => new
                {
                    year = x.Key,
                    month = int.TryParse(Regex.Replace(v.Month, @"[^0-9]+", ""), out int monthSpan) ? monthSpan : 0,
                    rate = v.DefectRate,
                    cntrTime = v.CntrTime
                })).GroupBy(x => x.month).ToDictionary(v => v.Key, v => v.ToDictionary(vk => vk.year, vv => vv));
                var cntrMonth = cntrMonthDict.Keys.ToList();
                var minM = cntrMonth.Min();
                var spanM = cntrMonth.Max() - minM + 1;
                var defectRateByCntr_body = Enumerable.Range(minM, spanM).Select(x =>
                {
                    var flag = cntrMonthDict.ContainsKey(x);
                    var row = new List<string>()
                    {
                    $"{x}个月",
                    };

                    var yearsRow = years.SelectMany(y => (flag && cntrMonthDict[x].ContainsKey(y))
                ? new List<string>()
                {
                cntrMonthDict[x][y].rate.ToString("P"),
                cntrMonthDict[x][y].cntrTime?.ToString("yyyy-MM-dd HH:mm:ss")
                }
                : new List<string>() { "", "" }).ToList();
                    row.AddRange(yearsRow);
                    return row;
                });

                table.Add(new List<string>() { "按对策时间不良率分布" });
                table.Add(defectRateByCntr_header);
                table.AddRange(defectRateByCntr_body);
                table.Add(table_bounder);
            }
            #endregion

            #region 里程分布
            var mileAge_header = new List<string>() { "里程", "不良件数" };
            var mileAge_body = metrics.MileAgeDistribution?.Select(x => new List<string>() { x.Key.ToString(), x.Value.ToString() });
            table.Add(new List<string>() { "里程分布" });
            table.Add(mileAge_header);
            if(mileAge_body != null)
                table.AddRange(mileAge_body);

            table.Add(table_bounder);
            #endregion

            #region 特约店分布
            var dealder_header = new List<string>() { "特约店", "合计不良件数" };
            var yearModels = metrics.DealerDistribution?.Keys.ToList();
            if (yearModels != null && yearModels.Count > 0)
                dealder_header.AddRange(yearModels);

            var dealerDict = metrics.DealerDistribution?.SelectMany(x => x.Value.Select(v => new
            {
                year = x.Key,
                dealer = v.Key,
                count = v.Value
            })).GroupBy(x => x.dealer).ToDictionary(v => v.Key, v => v.ToDictionary(vk => vk.year, vv => vv.count));
            var dealder_body = dealerDict?.Select(x =>
            {
                var row = new List<string>()
                {
                    x.Key,
                    x.Value.Sum(v=>v.Value).ToString(),
                };
                row.AddRange(x.Value.Select(v=>v.Value.ToString()));
                return row;
            }).ToList();
            table.Add(new List<string>() { "特约店分布" });
            table.Add(dealder_header);
            if(dealder_body != null)
                table.AddRange(dealder_body);
            table.Add(table_bounder);
            #endregion

            #region 片区分布
            var region_header = new List<string>() { "省份", "不良件数" };
            var region_body = metrics.RegionDistribution?.Select(x => new List<string>() { x.Name, x.Value.ToString() });

            table.Add(new List<string>() { "片区分布" });
            table.Add(region_header);
            if(region_body != null)
                table.AddRange(region_body);
            table.Add(table_bounder);
            #endregion

            #region 各渠道信息分布
            var dataSource_header = new List<string>() { "月份" };
            var sources = metrics.DataSourceDistribution?.Keys.ToList();
            if (sources != null && sources.Count > 0)
                dataSource_header.AddRange(sources);

            var sourceDict = metrics.DataSourceDistribution?.SelectMany(x => x.Value.Select(v => new
            {
                source = x.Key,
                month = v.Month,
                count = v.TotalCount
            })).GroupBy(x => x.month).ToDictionary(v => v.Key, v => v.ToDictionary(vk => vk.source, vv => vv.count));

            var dataSource_body = sourceDict?.Select(x =>
            {
                var row = new List<string>()
                {
                    x.Key,
                };
                row.AddRange(x.Value.Select(v => v.Value.ToString()));
                return row;
            }).ToList();
            table.Add(new List<string>() { "各渠道信息分布" });
            table.Add(dataSource_header);
            if(dataSource_body != null)
                table.AddRange(dataSource_body);
            table.Add(table_bounder);
            #endregion

            return table;
        }
    }
}
