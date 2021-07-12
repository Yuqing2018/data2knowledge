using MusicKG.DataAccess.Enums;
using MusicKG.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Service
{
    public interface IOptionService
    {
        Task<IEnumerable<OptionServiceModel>> GetOptions(OptionTypeEnum? optionType);
    }
}
