using MusicKG.HondaPlugins.DataAccess;
using MusicKG.HondaPlugins.DataAccess.DataModels.Raws;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace MusicKG.HondaPlugins.Services.Repositories.Implementations
{
    public class RawQISService : IRawQISService
    {
        private readonly IRawDbContext rawDbContext;
        private readonly ILogger<RawQISService> logger;

        public RawQISService(IRawDbContext rawDbContext, ILogger<RawQISService> logger)
        {
            this.rawDbContext = rawDbContext;
            this.logger = logger;
        }

        public async Task<Dictionary<string, List<(RawQICData QicData, RawQISData QisData)>>> GetRelatedQISAsync(List<string> mqiIds)
        {
            if (mqiIds?.Count == 0)
                return new Dictionary<string, List<(RawQICData, RawQISData)>>();

            var qics = await rawDbContext.RawQICData.AsQueryable().Where(q => mqiIds.Contains(q.MC_NO)).ToListAsync();

            var data = (from qic in qics.AsQueryable()
                        join qis in rawDbContext.RawQISData.AsQueryable() on qic.QIC_NO equals qis.QIC_NO into qiss
                        from r in qiss.DefaultIfEmpty()
                        select new { Qic = qic, Qis = r }).Select(q => q).ToList();

            return data?.GroupBy(d => d.Qic.MC_NO)?.ToDictionary(k => k.Key, v => v.Select(x => (x.Qic, x.Qis)).ToList());
        }


        public async Task<(RawQICData, RawQISData)> GetQISInfo(string qicNo, string qisNo)
        {
            if (string.IsNullOrWhiteSpace(qisNo))
                return (await rawDbContext.RawQICData.AsQueryable().Where(x => x.QIC_NO == qicNo).FirstOrDefaultAsync(), null);

            var querable = rawDbContext.RawQISData.AsQueryable();

            if (!string.IsNullOrWhiteSpace(qicNo))
            {
                querable = querable.Where(x => x.QIC_NO == qicNo);
            }
            if (!string.IsNullOrWhiteSpace(qisNo))
            {
                querable = querable.Where(x => x.QIS_NO == qisNo);
            }

            var qisInfo = await querable.FirstOrDefaultAsync();

            var qicInfo = qisInfo == null ? null : 
                await rawDbContext.RawQICData.AsQueryable().Where(x => x.QIC_NO == qisInfo.QIC_NO).FirstOrDefaultAsync();

            return (qicInfo, qisInfo);
        }

        public async Task<Tuple<long, IEnumerable<RawQICData>>> GetRawQICDatas(int from, int? size)
        {
            var qisDatas = rawDbContext.RawQICData.AsQueryable().ToListAsync().Result;
            var totalCount = qisDatas.Count;
            var results = qisDatas.Skip(from).Take(size ?? int.MaxValue);
            return new Tuple<long, IEnumerable<RawQICData>>(totalCount, results);
        }

        public async Task<Tuple<long, IEnumerable<RawQISData>>> GetRawQISDatas(int from, int? size)
        {
            var qisDatas = rawDbContext.RawQISData.AsQueryable().ToListAsync().Result;
            var totalCount = qisDatas.Count;
            var results = qisDatas.Skip(from).Take(size ?? int.MaxValue);
            return new Tuple<long, IEnumerable<RawQISData>>(totalCount, results);
        }
    }
}
