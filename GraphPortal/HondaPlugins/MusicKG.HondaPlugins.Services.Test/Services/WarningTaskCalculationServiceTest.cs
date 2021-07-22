using MongoDB.Bson;
using Moq;
using MusicKG.HondaPlugins.DataAccess;
using MusicKG.HondaPlugins.DataAccess.DataModels.Business;
using MusicKG.HondaPlugins.DataAccess.DataModels.Raws;
using MusicKG.HondaPlugins.DataAccess.DataModels.Warnings;
using MusicKG.HondaPlugins.DataAccess.Enums;
using MusicKG.HondaPlugins.DataAccess.Settings;
using MusicKG.HondaPlugins.Services.Models;
using MusicKG.HondaPlugins.Services.Repositories;
using MusicKG.HondaPlugins.Services.Repositories.Implementations;
using MusicKG.HondaPlugins.Services.Test.Fixtures;
using MusicKG.HondaPlugins.Services.Test.Helpers;
using MusicKG.HondaPlugins.WarningCalculator.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MusicKG.HondaPlugins.Services.Test
{
    [Collection("MongoCollection")]
    public class WarningTaskCalculationServiceTest
    {
        private readonly HondaMongoFixture mongoFixture;
        private readonly IHondaMongoDbContext context;
        private readonly IRawDbContext rawContext;
        private readonly IRawSalesService rawService;
        private readonly Mock<IWarningRecordService> warningRecordsService;
        private readonly IWarningTaskCalculationService warningTaskCalculation;

        public WarningTaskCalculationServiceTest(HondaMongoFixture mongoFixture)
        {
            this.mongoFixture = mongoFixture;
            context = new HondaMongoDbContext(mongoFixture.Context.Client, StringHelpers.RandomString(10));
            rawContext = mongoFixture.RawContext;
            this.rawService = new RawSalesService(rawContext, null);
            this.warningRecordsService = new Mock<IWarningRecordService>();
            this.warningTaskCalculation = new WarningTaskCalculationService(rawService, warningRecordsService.Object, null);
        }
        [Theory]
        [InlineData(WarningUnit.零件_不良症状)]
        [InlineData(WarningUnit.未知零件_不良症状)]
        [InlineData(WarningUnit.不良症状)]
        public async Task CalculateWarningTest(WarningUnit warningUnit)
        {
            var taskId = Guid.NewGuid().ToString();
            var currentTime = DateTime.UtcNow;
            var task = new WarningTaskDataModel()
            {
                Name = "testTask",
                CarModels = new List<string>() { "ACCH", "CDX", "RDX", "TLX" },
                CarTypes = new List<string>() { "CR6", "RH4", "CV3", "UB4" },
                YearModels = new List<string>() { "2016", "2017", "2018" },
                WarningUnit = warningUnit,
                WarningStatus = WarningTaskStatus.预警中,
                WarningIndex = new List<WarningIndexDataModel>()
                {
                    new WarningIndexDataModel()
                    {
                        Id = new  ObjectId("5fe19d6a6dd8200001810150"),
                        WarningType = WarningType.风险预警,
                        IndexName= WarningIndexNames.风险等级,
                        Value = "其他关注"
                    },
                    new WarningIndexDataModel()
                     {
                       Id = new  ObjectId("5fe19d6a6dd820000181014e"),
                       WarningType = WarningType.多发预警,
                       IndexName= WarningIndexNames.近三个月发生件数,
                       Value = "4",
                    },
                    new WarningIndexDataModel()
                    {
                        Id = new  ObjectId("5fe19d6a6dd8200001810151"),
                        WarningType = WarningType.再发预警,
                        IndexName= WarningIndexNames.对策后生产车辆的MQI件数,
                        Value= "4"
                    }
                }
            };

            var partNamesFilter = new List<string>();
            bool ignoreUnknownPart;

            switch (task.WarningUnit)
            {
                case WarningUnit.未知零件_不良症状:
                    partNamesFilter.Add(ConstantSettings.UnknownString);
                    ignoreUnknownPart = false;
                    break;
                case WarningUnit.零件_不良症状:
                    partNamesFilter = null;
                    ignoreUnknownPart = true;
                    break;
                default:
                    partNamesFilter = null;
                    ignoreUnknownPart = false;
                    break;
            }

            var all_datas = await GetVehicleFaultServiceModels();

            var datas = all_datas.Where(x => task.CarModels.Contains(x.CarModel) && task.CarTypes.Contains(x.CarType) && task.YearModels.Contains(x.ModelYear)).ToList();

            var list = datas.GroupBy(x => task.WarningUnit != WarningUnit.不良症状
            ? new { x.PartName, x.SyndromeModel }
            : new { PartName = "", x.SyndromeModel }).ToDictionary(k => k.Key, v => v.ToList());

            var confirms = GetConfirmRecords();

            foreach (var item in list)
            {
                var last_confirm = confirms.OrderByDescending(x => x.Confirm.LastConfirmdTime).FirstOrDefault(x => item.Value.Any(v => v.RawId == x.RawId))?.Confirm;
                this.warningRecordsService.Setup(x => x.GetLastConfirmRecordAsync(taskId, item.Key.PartName, item.Key.SyndromeModel.Id)).Returns(Task.FromResult(last_confirm));
            }

            var actuals = (await warningTaskCalculation.CalculateWarningAsync(taskId, currentTime, task, datas)).OrderBy(x => x.PartName).ThenBy(x => x.Syndrome).ToList();

            var all_Records = GetWarningRecordsResult(
                new List<string>
                {
                    warningUnit == WarningUnit.零件_不良症状?taskId:Guid.NewGuid().ToString(),
                    warningUnit == WarningUnit.未知零件_不良症状?taskId:Guid.NewGuid().ToString(),
                    warningUnit == WarningUnit.不良症状?taskId:Guid.NewGuid().ToString(),
                });

            var expected = all_Records.Where(x => x.TaskId == taskId && (ignoreUnknownPart
                ? x.PartName != ConstantSettings.UnknownString
                : (partNamesFilter != null ? partNamesFilter.Contains(x.PartName) : true))).OrderBy(x => x.PartName).ThenBy(x => x.Syndrome).ToList();

            Assert.Equal(expected.Count, actuals.Count());

            for (int i = 0; i < expected.Count; i++)
            {
                var actual = actuals[i];
                var expect = expected[i];
                Assert.Equal(expect.PartName, actual.PartName);
                Assert.Equal(expect.Syndrome, actual.Syndrome);
                Assert.Equal(expect.TaskId, actual.TaskId);

                var expect_multi_MQI = expect.MultipleMetrics[DataSource.MQI.ToString()];
                var actual_multi_MQI = actual.MultipleMetrics.ContainsKey(DataSource.MQI.ToString()) 
                    ? expect.MultipleMetrics[DataSource.MQI.ToString()] 
                    : new MultipleWarningIndexMetricsDataModel()
                    {
                        DefectRateByYearModel = new Dictionary<string, double>(),
                        
                    };
                Assert.Equal(Math.Round(expect_multi_MQI.LastThreeMonthAscentRate,4), Math.Round(actual_multi_MQI.LastThreeMonthAscentRate,4));
                Assert.Equal(expect_multi_MQI.TotalCount, actual_multi_MQI.TotalCount);
                Assert.Equal(Math.Round(expect_multi_MQI.DefectRate,4), Math.Round(actual_multi_MQI.DefectRate,4));
                Assert.Equal(Math.Round(expect_multi_MQI.DefectRateByYearModel["2016"],4), actual_multi_MQI.DefectRateByYearModel.ContainsKey("2016")? Math.Round(actual_multi_MQI.DefectRateByYearModel["2016"],4):0);
                Assert.Equal(Math.Round(expect_multi_MQI.DefectRateByYearModel["2017"],4), actual_multi_MQI.DefectRateByYearModel.ContainsKey("2017") ? Math.Round(actual_multi_MQI.DefectRateByYearModel["2017"],4) : 0); 
                Assert.Equal(Math.Round(expect_multi_MQI.DefectRateByYearModel["2018"],4), actual_multi_MQI.DefectRateByYearModel.ContainsKey("2018") ? Math.Round(actual_multi_MQI.DefectRateByYearModel["2018"],4) : 0);

                var expect_risk = expect.RiskMetrics;
                var actual_risk = actual.RiskMetrics;

                Assert.Equal(Math.Round(expect_risk.DataSourceMetrics["MQI"].DefectRate, 4), actual_risk.DataSourceMetrics.ContainsKey("MQI")
                    ? Math.Round(actual_risk.DataSourceMetrics["MQI"].DefectRate, 4)
                    : 0);
                Assert.Equal(expect_risk.DataSourceMetrics["MQI"].TotalCount, actual_risk.DataSourceMetrics.ContainsKey("MQI")
                    ? actual_risk.DataSourceMetrics["MQI"].TotalCount : 0);
                Assert.Equal(Math.Round(expect_risk.DataSourceMetrics["MQI"].LastThreeMonthAscentRate, 4), actual_risk.DataSourceMetrics.ContainsKey("MQI")
                    ? Math.Round(actual_risk.DataSourceMetrics["MQI"].LastThreeMonthAscentRate, 4)
                    : 0);

                Assert.Equal(expect_risk.IsExcessive, actual_risk.IsExcessive);
                Assert.Equal(expect_risk.PushStatus, actual_risk.PushStatus);
                Assert.Equal(Math.Round(expect_risk.RiskScore,4), Math.Round(actual_risk.RiskScore,4));
                Assert.Equal(expect_risk.RiskLevel, actual_risk.RiskLevel);
                Assert.Equal(expect_risk.PermanentCntrStatus, actual_risk.PermanentCntrStatus);
                Assert.Equal(expect_risk.PermanentCntrTime, actual_risk.PermanentCntrTime);
                Assert.Equal(Math.Round(expect_risk.MeanCostRepair,4), Math.Round(actual_risk.MeanCostRepair,4));
                
                var expect_again = expect.AgainMetrics;
                var actual_again = actual.AgainMetrics;

                Assert.Equal(expect_again.PermanentCntrTime, actual_again.PermanentCntrTime);
                Assert.Equal(Math.Round(expect_again.DefectRateBeforeCntr,4), Math.Round(actual_again.DefectRateBeforeCntr,4));
                Assert.Equal(expect_again.CountAfterCntr, actual_again.CountAfterCntr);
                Assert.Equal(Math.Round(expect_again.DefectRateAfterCntr,4), Math.Round(actual_again.DefectRateAfterCntr,4));
            }
        }

        /// <summary>
        /// string taskId, DateTime currentRunTime, WarningTaskDataModel task, List<VehicleFaultServiceModel> data
        /// </summary>
        /// <returns></returns>
        public async Task<List<VehicleFaultServiceModel>> GetVehicleFaultServiceModels()
        {
            var file = $"{AppDomain.CurrentDomain.BaseDirectory}/Files/知识标注平台测试数据_20210329.xlsx";
            var fileStream = StreamHelper.FileToStream(file);
            //var table1 = ExcelReader.ReadTables(fileStream, 0);//confirm records 
            //var table2 = ExcelReader.ReadTables(fileStream, 1);//sales
            //var table3 = ExcelReader.ReadTables(fileStream, 2);//vehicledatas
            //var table4 = ExcelReader.ReadTables(fileStream, 3);//warning record
            var pntr_table = ExcelReader.ReadTables(fileStream, 0);
            var pntr_records = pntr_table.Select(row
                => new KeyValuePair<string, string>(
                    row[0].ToString(),
                    row[4].ToString())
                ).ToList();

            var table = ExcelReader.ReadTables(fileStream, 2);

            var vehicleList = table.Skip(1).Select(row =>
            {
                var data = new VehicleFaultServiceModel()
                {
                    RawId = row[1].ToString(),
                    DataSource = (DataSource)Enum.Parse(typeof(DataSource), row[2].ToString()),
                    CarType = row[4].ToString(),
                    CarModel = row[3].ToString(),
                    ModelYear = row[5].ToString(),
                    PartName = row[6].ToString(),
                    Syndrome = row[7].ToString(),
                    SyndromeModel = new SyndromeServiceModel()
                    {
                        MongoId = new ObjectId(row[7].ToString()),
                        Name = row[8].ToString(),
                        BadGrade = (BadGrade)Enum.Parse(typeof(BadGrade), row[9].ToString()),
                    },
                    ProductionDate = DateTime.FromOADate(double.Parse(row[10].ToString())),
                    InitialRegistDate = DateTime.FromOADate(double.Parse(row[11].ToString())),
                    FaultDate = DateTime.FromOADate(double.Parse(row[12].ToString())),
                    MileAge = int.Parse(row[13].ToString()),
                    DealerCD = row[14].ToString(),
                    Region = row[15].ToString(),
                    CostRepair = float.Parse(row[16].ToString()),
                };
                data.RelatedInfo = pntr_records.Where(x => x.Key == row[1].ToString()
                    && !string.IsNullOrEmpty(x.Value))
                .Select(x => new VehicleFaultRelatedDataModel()
                {
                    PermanentCntrTime = DateTime.Parse(x.Value)
                })?.ToList();
                return data;
            }).ToList();

            return vehicleList;
        }

        //预警跟踪记录
        public List<WarningRecordDataModel> GetWarningRecordsResult(List<string> taskIds)
        {
            var file = $"{AppDomain.CurrentDomain.BaseDirectory}/Files/知识标注平台测试数据_20210329.xlsx";
            var fileStream = StreamHelper.FileToStream(file);
            var recordsTable = ExcelReader.ReadTables(fileStream, 3);
            var recordsList = recordsTable.Skip(2).Select(row =>
            {
                var taskId = "";
                var task_unit = (WarningUnit)Enum.Parse(typeof(WarningUnit), row[0].ToString());
                if (task_unit == WarningUnit.零件_不良症状)
                    taskId = taskIds[0];
                else if (task_unit == WarningUnit.未知零件_不良症状)
                    taskId = taskIds[1];
                else if (task_unit == WarningUnit.不良症状)
                    taskId = taskIds[2];

                var record = new WarningRecordDataModel()
                {
                    TaskId = taskId,
                    PartName = row[4].ToString(),
                    Syndrome = row[5].ToString(),
                    MultipleMetrics = new Dictionary<string, MultipleWarningIndexMetricsDataModel>()
                    {
                        { DataSource.MQI.ToString(),
                            new MultipleWarningIndexMetricsDataModel()
                            {
                                TopOrder = 0,
                                LastThreeMonthCount = new Dictionary<string, int>() { },
                                LastThreeMonthAscentRate = double.Parse(row[17].ToString()),
                                DefectRateByYearModel = new Dictionary<string, double>()
                                {
                                    { "2016", double.Parse(row[19].ToString()) },
                                    { "2017", double.Parse(row[21].ToString()) },
                                    { "2018", double.Parse(row[23].ToString()) },
                                },
                                TotalCount = int.Parse(row[27].ToString()),
                                DefectRate = double.Parse(row[28].ToString()),
                            }
                        },
                    },
                    RiskMetrics = new RiskWarningIndexMetricsDataModel()
                    {
                        DataSourceMetrics = new Dictionary<string, DataSourceMetricsDataModel>()
                        {
                            { DataSource.MQI.ToString(),
                                new DataSourceMetricsDataModel()
                                {
                                    TotalCount = int.Parse(row[27].ToString()),
                                    DefectRate = double.Parse(row[28].ToString()),
                                    LastThreeMonthAscentRate = double.Parse(row[34].ToString()),
                                }
                            }
                            //{DataSource.HOTLINE.ToString(),int.Parse(row[8].ToString())},
                            //{DataSource.GOV.ToString(),int.Parse(row[9].ToString())},
                            //{DataSource.MEDIA_MAIN.ToString(),int.Parse(row[10].ToString())},
                            //{DataSource.MQI.ToString(),int.Parse(row[11].ToString())}
                        },
                        PushStatus = string.IsNullOrEmpty(row[44].ToString()) ? null : (PushStatus)Enum.Parse(typeof(PushStatus), row[44].ToString()),
                        RiskScore = double.Parse(row[50].ToString()),
                        MeanCostRepair = double.Parse(row[47].ToString()),
                        IsExcessive = row[48].ToString() == "是",
                        PermanentCntrStatus = string.IsNullOrEmpty(row[49].ToString()) ? null : (PermanentCntrStatus)Enum.Parse(typeof(PermanentCntrStatus), row[49].ToString()),
                        RiskLevel = string.IsNullOrEmpty(row[51].ToString()) ? null : (RiskLevel)Enum.Parse(typeof(RiskLevel), row[51].ToString()),
                        PermanentCntrTime = DateTime.FromOADate(double.Parse(row[52].ToString())),
                    },
                    AgainMetrics = new AgainWarningIndexMetricsDataModel()
                    {
                        PermanentCntrTime = DateTime.FromOADate(double.Parse(row[52].ToString())),
                        DefectRateBeforeCntr = double.Parse(row[55].ToString()),
                        CountAfterCntr = int.Parse(row[56].ToString()),
                        DefectRateAfterCntr = double.Parse(row[58].ToString()),
                    }
                };
                return record;
            }).ToList();

            return recordsList;
        }


        public List<ConfirmRecordTest> GetConfirmRecords()
        {
            var file = $"{AppDomain.CurrentDomain.BaseDirectory}/Files/知识标注平台测试数据_20210329.xlsx";
            var fileStream = StreamHelper.FileToStream(file);
            var confirmTable = ExcelReader.ReadTables(fileStream, 0);

            //MQI编号	推进分类(QIS)	超标影响(人工填写)	对策状态(人工填写)	对策时间(QIS)
            var confirm_records = confirmTable.Skip(1).Select(row 
                => {
                    var confirm = new ConfirmRecordTest
                    {
                        RawId = row[0].ToString(),
                        Confirm = new WarningConfirmRecordDataModel()
                        {
                            PushStatus = (PushStatus)Enum.Parse(typeof(PushStatus), row[1].ToString()),
                            IsExcessive = row[2].ToString() == "是",
                            PermanentCntrStatus = string.IsNullOrEmpty(row[3].ToString()) ? null : (PermanentCntrStatus)Enum.Parse(typeof(PermanentCntrStatus), row[3].ToString()),
                            LastConfirmdTime = DateTime.Parse(row[4].ToString())
                        }
                    };
                    return confirm;
                }
            ).ToList();

            return confirm_records;
        }
    }

    public class ConfirmRecordTest
    {
        public string RawId { get; set; }
        public WarningConfirmRecordDataModel Confirm { get; set; }
    } 
}
