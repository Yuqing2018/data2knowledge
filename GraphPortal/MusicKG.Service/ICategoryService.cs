using MusicKG.DataAccess.Models;
using MusicKG.Service.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicKG.Service
{
    public interface ICategoryService
    {
        Task<CategoryDataModel> GetOneAsync(string workspaceId, string categoryId);
        Task<List<CategoryDataModel>> GetAllAsync(string workspaceId);

        Task<Tuple<long, IEnumerable<CategoryServiceModel>>> GetCategoriesAsync(string workspaceId, string keyword, int from, int? size);

        Task<CategoryServiceModel> UpdateAsync(string workspaceId, string id, string name);

        Task<bool> CreateManyAsync(string workspaceId, string createBy, List<string> names);

        Task<CategoryServiceModel> CreateOneAsync(CategoryServiceModel serviceModel);

        Task<Dictionary<string, string>> GetCategoryDictAsync(string workspaceId);
    }
}
