using MusicKG.HondaPlugins.DataAccess.DataModels.Raws;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.Services.Repositories
{
    public interface IRawQISService
    {
        Task<Dictionary<string, List<(RawQICData QicData, RawQISData QisData)>>> GetRelatedQISAsync(List<string> mqiIds);

        Task<(RawQICData, RawQISData)> GetQISInfo(string qicNo, string qisNo);

        Task<Tuple<long, IEnumerable<RawQICData>>> GetRawQICDatas(int from, int? size);

        Task<Tuple<long, IEnumerable<RawQISData>>> GetRawQISDatas(int from, int? size);
    }
}
