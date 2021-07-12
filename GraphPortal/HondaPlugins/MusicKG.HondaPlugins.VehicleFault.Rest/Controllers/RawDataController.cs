using MusicKG.HondaPlugins.VehicleFault.Rest.Models.ViewModels;
using MusicKG.HondaPlugins.VehicleFault.Rest.Helpers;
using MusicKG.HondaPlugins.Services.Resources;
using MusicKG.HondaPlugins.DataAccess.Enums;
using MusicKG.HondaPlugins.DataAccess.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Net;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;

namespace MusicKG.HondaPlugins.VehicleFault.Rest.Controllers
{
    [Route("api/honda/v1/vehiclefault/rawdata")]
    [ApiController]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.InternalServerError)]
    public class RawDataController : ControllerBase
    {
        private const int titleIndex = 1;

        private readonly MongoRawDataDbSettings dbSettings;
        private readonly VehicleDataConstructorSettings settings;

        public RawDataController(MongoRawDataDbSettings dbSettings, VehicleDataConstructorSettings settings)
        {
            this.dbSettings = dbSettings;
            this.settings = settings;
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task UploadAsync(DataSource dataSource, IFormFile file)
        {
            var result = ExcelFileHelpers.ProcessFile(file, titleIndex);

            if (result == null || result.Count == 0)
                ErrorHelper.ThrowException("No data in excel file.", HttpStatusCode.BadRequest);

            RawDataTableSettings tableSettings = null;

            var vehicleSettings = settings.VehicleDataSettings[dataSource.ToString()];

            ValidateFile(dataSource.ToString(), vehicleSettings, result);

            switch (dataSource)
            {
                case DataSource.GOV:
                    tableSettings = dbSettings.GOVDataSettings;
                    break;
                case DataSource.MEDIA_MAIN:
                    tableSettings = dbSettings.MediaMainDataSettings;
                    break;
                case DataSource.MEDIA_SUB:
                    tableSettings = dbSettings.MediaSubDataSettings;
                    break;
                default:
                    ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.DataSourceMustTranlsatedFromMariaDB, HttpStatusCode.BadRequest);
                    break;
            }

            var collection = GetMongoCollection(tableSettings);

            var dataModels = result.Select(r =>
            {
                var rawData = new BsonDocument(r);
                rawData.AddRange(new BsonDocument(tableSettings.TimestampFieldName, DateTime.UtcNow));
                return rawData;
            }).ToList();

            await collection.InsertManyAsync(dataModels);
        }

        private void ValidateFile(string dataSource, VehicleDataSettings dataSettings, List<Dictionary<string, object>> data)
        {
            var keyFeatures = dataSettings.KeyFeatures;

            var line = 1;

            data?.ForEach(d =>
            {
                if (!keyFeatures.Any(key => d.ContainsKey(key) && !string.IsNullOrWhiteSpace(d[key]?.ToString())))
                    ErrorHelper.ThrowException(
                        string.Format(MusicKGHondaPluginsMessage.KeyFeatureNotFoundMessage, dataSource, string.Join(",", keyFeatures), line), HttpStatusCode.BadRequest);

                if (!ValidateFeature(dataSettings.CarModelFeature, d))
                    ErrorHelper.ThrowException(string.Format(MusicKGHondaPluginsMessage.FeatureNotFoundMessage, dataSource, "车款"), HttpStatusCode.BadRequest);

                if (!ValidateFeature(dataSettings.CarTypeFeature, d))
                    ErrorHelper.ThrowException(string.Format(MusicKGHondaPluginsMessage.FeatureNotFoundMessage, dataSource, "车型"), HttpStatusCode.BadRequest);

                if (!ValidateFeature(dataSettings.CostRepairFeature, d))
                    ErrorHelper.ThrowException(string.Format(MusicKGHondaPluginsMessage.FeatureNotFoundMessage, dataSource, "总维修费用"), HttpStatusCode.BadRequest);

                if (!ValidateFeature(dataSettings.DealerCDFeature, d))
                    ErrorHelper.ThrowException(string.Format(MusicKGHondaPluginsMessage.FeatureNotFoundMessage, dataSource, "特约店"), HttpStatusCode.BadRequest);

                if (!ValidateFeature(dataSettings.FaultDateFeature, d))
                    ErrorHelper.ThrowException(string.Format(MusicKGHondaPluginsMessage.FeatureNotFoundMessage, dataSource, "故障日期"), HttpStatusCode.BadRequest);

                if (!ValidateFeature(dataSettings.InitialRegistDateFeature, d))
                    ErrorHelper.ThrowException(string.Format(MusicKGHondaPluginsMessage.FeatureNotFoundMessage, dataSource, "购买日期"), HttpStatusCode.BadRequest);

                if (!ValidateFeature(dataSettings.MileAgeFeature, d))
                    ErrorHelper.ThrowException(string.Format(MusicKGHondaPluginsMessage.FeatureNotFoundMessage, dataSource, "里程数"), HttpStatusCode.BadRequest);

                if (!ValidateFeature(dataSettings.ModelYearFeature, d))
                    ErrorHelper.ThrowException(string.Format(MusicKGHondaPluginsMessage.FeatureNotFoundMessage, dataSource, "年款"), HttpStatusCode.BadRequest);

                if (!ValidateFeature(dataSettings.PartNameFeature, d))
                    ErrorHelper.ThrowException(string.Format(MusicKGHondaPluginsMessage.FeatureNotFoundMessage, dataSource, "零件名"), HttpStatusCode.BadRequest);

                if (!ValidateFeature(dataSettings.PartNoFeature, d))
                    ErrorHelper.ThrowException(string.Format(MusicKGHondaPluginsMessage.FeatureNotFoundMessage, dataSource, "零件号"), HttpStatusCode.BadRequest);

                if (!ValidateFeature(dataSettings.ProductionDateFeature, d))
                    ErrorHelper.ThrowException(string.Format(MusicKGHondaPluginsMessage.FeatureNotFoundMessage, dataSource, "生产日期"), HttpStatusCode.BadRequest);

                if (!ValidateFeature(dataSettings.RawIdFeature, d))
                    ErrorHelper.ThrowException(string.Format(MusicKGHondaPluginsMessage.FeatureNotFoundMessage, dataSource, "数据唯一号"), HttpStatusCode.BadRequest);

                if (!ValidateFeature(dataSettings.RegionFeature, d))
                    ErrorHelper.ThrowException(string.Format(MusicKGHondaPluginsMessage.FeatureNotFoundMessage, dataSource, "片区"), HttpStatusCode.BadRequest);

                if (!ValidateFeature(dataSettings.SyndromeFeature, d))
                    ErrorHelper.ThrowException(string.Format(MusicKGHondaPluginsMessage.FeatureNotFoundMessage, dataSource, "不良症状"), HttpStatusCode.BadRequest);

                if (!ValidateFeature(dataSettings.VINFeature, d))
                    ErrorHelper.ThrowException(string.Format(MusicKGHondaPluginsMessage.FeatureNotFoundMessage, dataSource, "车架号"), HttpStatusCode.BadRequest);

                line += 1;
            });
        }

        private bool ValidateFeature(string featureName, Dictionary<string, object> data)
        {
            if (string.IsNullOrWhiteSpace(featureName))
                return true;

            if (data.ContainsKey(featureName))
                return true;

            return false;
        }

        private IMongoCollection<BsonDocument> GetMongoCollection(RawDataTableSettings tableSettings)
        {
            var settings = MongoClientSettings.FromConnectionString(dbSettings.ConnectionString);

            settings.MaxConnectionIdleTime = TimeSpan.FromSeconds(30);

            var client = new MongoClient(settings);

            var database = client.GetDatabase(dbSettings.Database);

            return database.GetCollection<BsonDocument>(tableSettings.TableName);
        }
    }
}
