using MusicKG.HondaPlugins.DataAccess.DataModels.Warnings;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.WarningCalculator.Services
{
    public interface IRiskWarningAIService
    {
        Task CalculateRiskWarnings(List<WarningRecordDataModel> records, int batchSize);
    }
}
