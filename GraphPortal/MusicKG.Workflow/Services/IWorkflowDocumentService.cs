using MusicKG.DataAccess.Enums;
using MusicKG.DataAccess.Models;
using MusicKG.Workflow.Models.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Workflow.Services
{
    public interface IWorkflowDocumentService
    {
        Task<List<string>> FetchDocumentsAsync(WorkflowStepInputFilterServiceModel filter, int count);

        Task UpdateDocumentAsync(string stepId, DocumentProcessorServiceModel processResult);

        Task<DocumentContentServiceModel> GetDocumentContentAsync(string documentId);

        Task<string> CreateDocumentAsync(WorkflowDocumentCreateServiceModel serviceModel);
    }
}
