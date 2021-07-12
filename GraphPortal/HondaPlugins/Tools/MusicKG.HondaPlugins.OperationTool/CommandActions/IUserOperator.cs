using MusicKG.DataAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.OperationTool.CommandActions
{
    public interface IUserOperator
    {
        Task<List<UserDataModel>> ListAsync(string userName);

        Task DisableAsync(string userName);

        Task EnableAsync(string userName);

        Task RenameAsync(string userName, string newName);

        Task ResetPasswordAsync(string userName, string password);

        Task CreateAsync(string userName, string password);
    }
}
