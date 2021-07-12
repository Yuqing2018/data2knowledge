using MusicKG.HondaPlugins.DataAccess.DataModels.Business;
using MusicKG.HondaPlugins.DataAccess.Enums;
using MusicKG.HondaPlugins.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.Services.Repositories
{
    public interface IVehicleFaultDataService
    {
        Task<Dictionary<string, string>> DetailAsync(string rawId);

        Task<(long TotalCount, IEnumerable<VehicleFaultListServiceModel> Data)> ListAsync(
            List<string> carModel, List<DataSource> dataSource, List<string> carType, List<string> modelYears,
            List<string> partName, List<string> syndrome, DateTime? faultDateStart, DateTime? faultDateEnd,
            int from, int? size, int maxSize);

        Task<List<VehicleFaultListServiceModel>> ListStatisticableAsync(
            List<string> carModel, List<DataSource> dataSource, List<string> carType, List<string> modelYears,
            List<string> partName, List<string> syndrome, DateTime? faultDateStart, DateTime? faultDateEnd);

        Task<List<VehicleFaultServiceModel>> ListExportableAsync(
            List<string> carModel, List<DataSource> dataSource, List<string> carType, List<string> modelYears,
            List<string> partName, List<string> syndrome, DateTime? faultDateStart, DateTime? faultDateEnd);

        Task<List<string>> ListIdsAsync(List<string> carModel, List<string> carType, List<string> modelYears,
            List<string> partName, List<string> syndrome, DateTime? faultDateStart, DateTime? faultDateEnd, bool ignoreUnknownPart);

        Task<List<VehicleFaultServiceModel>> LinkRelatedDataAsyncBatch(List<string> rawIds);

        Task UpdateAsync(string rawId, string partName, string syndrome, bool isAddToTraining);

        Task<List<string>> ListCarModelsAsync(DataSource? dataSource);

        Task<List<string>> ListCarTypesAsync(DataSource? dataSource, List<string> carModel);

        Task<List<string>> ListModelYearsAsync(DataSource? dataSource, List<string> carModel, List<string> carType);

        Task<List<string>> ListPartNamesAsync(DataSource? dataSource, List<string> carModel, List<string> carType, List<string> modelYears, List<string> syndrome);

        Task<List<SyndromeServiceModel>> ListSyndromesAsync(DataSource? dataSource, List<string> carModel, List<string> carType, List<string> modelYears, List<string> partName);

        Task<Dictionary<DataSource, List<List<string>>>> ExportAsync(
            List<string> carModel, List<DataSource> dataSource, List<string> carType, List<string> modelYears,
            List<string> partName, List<string> syndrome, DateTime? faultDateStart, DateTime? faultDateEnd, int maxCount);

        Task<(long TotalCount, IEnumerable<VehicleFaultQISInfoServiceModel> Data)> ListQisAsync(List<DataSource> dataSource, List<string> carModel, List<string> carType, List<string> modelYears, List<string> partName, string frameNo, int from, int? size);

        Task<bool> UpdateRelatedPartName(string rawId, string carType, string partNo);

        Task<bool> UpdateDataFromAsync(string rawId, string dataFromDesc);

        Task<bool> UpdateQISAsync(string rawId, VehicleFaultRelatedDataModel qisInfo);

        Task<bool> AutoLinkQISAsync(string rawId, string qICNo, string qISNo);

        Task<bool> UpdateLastPermanentCntrTimeAsync(string rawId, DateTime cntrTime);

        Task<bool> DeleteRelatedInfo(string rawId);
    }
}
