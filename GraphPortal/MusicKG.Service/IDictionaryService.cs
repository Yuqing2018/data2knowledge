using MusicKG.Service.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicKG.Service
{
    public interface IDictionaryService
    {
        Task<string> GetDictionaryIdByNameAsync(string workspaceId, string name);

        Task<DictionaryServiceModel> GetDictionaryAsync(string workspaceId, string id);

        Task<Tuple<long, IEnumerable<DictionaryListItemServiceModel>>> GetDictionariesAsync(string workspaceId, int from, int? size, bool topLatest = true);

        Task<Tuple<long, IEnumerable<string>>> GetDictionaryEntriesAsync(string workspaceId, string dictionaryId, string filterStr, int from, int? size);

        Task<IEnumerable<string>> GetAllDictionaryEntriesAsync(string workspaceId);

        Task<DictionaryServiceModel> CreateDictionaryAsync(DictionaryCreateServiceModel serviceModel);

        Task<DictionaryServiceModel> UpdateDictionaryAsync(string workspaceId, string dictionaryId, DictionaryUpdateServiceModel serviceModel);

        Task DeleteDictionaryAsync(string workspaceId, string dictionaryId);

        Task UpdateOneVocabsAsync(string workspaceId, string Id, List<string> vocabs);
    }
}
