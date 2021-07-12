using MusicKG.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Service
{
    public interface IStorageService
    {
        Task Create(DocumentStoreServiceModel serviceModel);

        Task<byte[]> Read(string filePath);
    }
}
