using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MusicKG.HondaPlugins.DataAccess;
using MusicKG.HondaPlugins.DataAccess.DataModels.Raws;
using MusicKG.HondaPlugins.DataAccess.DataModels.Warnings;
using MusicKG.HondaPlugins.DataAccess.Enums;
using MusicKG.HondaPlugins.DataAccess.Settings;
using MusicKG.HondaPlugins.Services.Repositories;
using MusicKG.HondaPlugins.Services.Repositories.Implementations;
using MusicKG.HondaPlugins.Services.Test.Fixtures;
using MusicKG.HondaPlugins.Services.Test.Helpers;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MusicKG.HondaPlugins.Services.Test.Services
{
    [Collection("MongoCollection")]
    public class RawSalesServiceTest
    {
        private readonly HondaMongoFixture mongoFixture;
        private readonly IRawDbContext context;
        private readonly IRawSalesService rawService;

        public RawSalesServiceTest(HondaMongoFixture mongoFixture)
        {
            this.mongoFixture = mongoFixture;
            this.context = mongoFixture.RawContext;
            this.rawService = new RawSalesService(context,null);
        }

        [Fact]
        public async Task GetSalesCount()
        {
           //await PrepareDatas();
           var count = await rawService.GetSalesCount(null,null,null,null,null,null);
           var actual = await context.RawSalesData.LongCountAsync();
            Assert.Equal(actual, count);

            var taskId = Guid.NewGuid().ToString();
            var task = new WarningTaskDataModel()
            {
                Name = "testTask",
                CarModels = new List<string>() { "ACCH", "CDX", "RDX", "TLX" },
                CarTypes = new List<string>() { "CR6", "RH4", "CV3", "UB4" },
                YearModels = new List<string>() { "2016", "2017", "2018" },
                WarningUnit = WarningUnit.零件_不良症状,
                WarningStatus = WarningTaskStatus.预警中,
                WarningIndex = new List<WarningIndexDataModel>()
            };

            var task_sales_count = await rawService.GetSalesCount(task.CarModels, task.CarTypes, task.YearModels, null, null, DateTime.UtcNow);

            var actual_sales_count = await context.RawSalesData.Where(x=> task.CarModels.Contains(x.CarModel) && task.CarTypes.Contains(x.CarType) && task.YearModels.Contains(x.ModelYear) && x.ProductionDate.CompareTo(DateTime.UtcNow.ToString("yyyyMMdd")) < 0).LongCountAsync();
            Assert.Equal(task_sales_count, actual_sales_count);
        }

        public async Task PrepareDatas()
        {
            var file = $"{AppDomain.CurrentDomain.BaseDirectory}/Files/知识标注平台测试数据_20210329.xlsx";
            var fileStream = StreamHelper.FileToStream(file);
            var salesTable = ExcelReader.ReadTables(fileStream, 1);
            var salesList = salesTable.Skip(1).Select(row => new RawSalesData()
            {
                CarModel = row[1].ToString(),
                CarType = row[2].ToString(),
                ModelYear = row[3].ToString(),
                ProductionDate = DateTime.FromOADate(double.Parse(row[5].ToString())).ToString("yyyyMMdd"),
                InitialRegistDate = DateTime.FromOADate(double.Parse(row[6].ToString())).ToString("yyyyMMdd"),
            }).ToList();

            await context.RawSalesData.AddRangeAsync(salesList);
        }
    }
}
