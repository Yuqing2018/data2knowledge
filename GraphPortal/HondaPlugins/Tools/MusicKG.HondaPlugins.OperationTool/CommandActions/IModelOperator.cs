using MusicKG.DataManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.OperationTool.CommandActions
{
    public interface IModelOperator
    {
        public Task TrainAsync(string modelName);

        public Task<List<ModelTrainingHistory>> ListAsync(string modelName);

        public Task RevertAsync(string modelName, int version);
    }
}
