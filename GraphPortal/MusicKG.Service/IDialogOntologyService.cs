using MusicKG.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicKG.Service
{
    public interface IDialogOntologyService
    {
        Task<bool> DailogOntologyExistsAsync(string workspaceId);

        Task<DialogOntologyServiceModel> GetDialogOntologyAsync(string workspaceId);

        Task UploadDialogOntologyAsync(string workspaceId, DialogOntologyCreateServiceModel serviceModel);
    }
}
