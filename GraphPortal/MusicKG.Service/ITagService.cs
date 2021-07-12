using MusicKG.DataAccess.Enums;
using MusicKG.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicKG.Service
{
    public interface ITagService
    {
        Task<(long total, IEnumerable<TagValueServiceModel> values)> GetTagValuesAsync(string workspaceId, TagTypeEnum type, string strFilter, int from, int? size);

        Task ReplaceTagAsync(TagServiceModel serviceModel);

        Task AddTagValuesAsync(TagServiceModel serviceModel);

        Task UpdateTagValueAsync(string workspaceId, TagTypeEnum type, string valueId, TagValueUpdateServiceModel serviceModel);

        Task DeleteTagValuesAsync(string workspaceId, TagTypeEnum type, List<string> valueIds);
    }
}
