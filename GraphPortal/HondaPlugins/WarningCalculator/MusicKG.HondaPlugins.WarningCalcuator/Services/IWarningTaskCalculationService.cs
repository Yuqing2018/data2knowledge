using MusicKG.HondaPlugins.DataAccess.DataModels.Warnings;
using MusicKG.HondaPlugins.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.WarningCalculator.Services
{
    public interface IWarningTaskCalculationService
    {
        Task<List<WarningRecordDataModel>> CalculateWarningAsync(string taskId,
            DateTime currentRunTime, WarningTaskDataModel task, 
            List<VehicleFaultServiceModel> data);
    }
}
