using MongoDB.Bson;
using MusicKG.DataAccess;
using MusicKG.DataAccess.Enums;
using MusicKG.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicKG.Service.Test.Helpers
{
    public static class DocumentDataHelper
    {
        public const string ExpectedUploader = "TestUploader";

        public static UserDataModel User = new UserDataModel
        {
            Id = ObjectId.GenerateNewId(),
            Name = DocumentDataHelper.ExpectedUploader
        };

        public static IEnumerable<WorkflowDataModel> Workflows = Enumerable.Range(1, 2).Select(i => new WorkflowDataModel
        {
            Id = ObjectId.GenerateNewId(),
            Name = $"workflow {i}",
            Steps = Enum.GetValues(typeof(DocumentStatusEnum)).Cast<DocumentStatusEnum>().Select((s, j) => new WorkflowStepDataModel
            {
                StepId = ObjectId.GenerateNewId(),
                Name = $"workflow {i} step {j}",
                ResultDocumentStatus = s
            }).ToList()
        }).ToList();

        public static IEnumerable<WorkspaceTypeDataModel> WorkspaceTypes = Workflows.Select((w, i) => new WorkspaceTypeDataModel
        {
            Id = ObjectId.GenerateNewId(),
            Name = $"workspace type {i}",
            Status = WorkspaceTypeStatusEnum.Enabled,
            WorkflowId = w.Id
        }).ToList();

        public static IEnumerable<WorkspaceDataModel> Workspaces = WorkspaceTypes.Select((t, i) => new WorkspaceDataModel
        {
            Id = ObjectId.GenerateNewId(),
            Name = $"workspace {i}",
            Type = t.Id,
            IsDeleted = false
        }).ToList();

        public static IEnumerable<DocumentDataModel> Documents = Workspaces.SelectMany((w, i) =>
        {
            var workspaceType = WorkspaceTypes.Where(wt => wt.Id == w.Type).First();
            var workflow = Workflows.Where(wf => wf.Id == workspaceType.WorkflowId).First();
            return workflow.Steps.Select((s, j) => new DocumentDataModel
            {
                Id = ObjectId.GenerateNewId(),
                Name = i % 2 == 0 ? $"document {i} {j}" : $"Document {i} {j}",
                WorkspaceId = w.Id,
                WorkflowId = workflow.Id,
                BirthStep = new DocumentProcessStepDataModel
                {
                    StepId = s.StepId
                },
                Status = DocumentStatusEnum.Preannotated,
                Tags = new List<string> { i % 2 == 0 ? $"Document Tag {i} {j}" : $"document tag {i} {j}" },
                UploadedBy = User.Id,
                UploadedAt = DateTime.UtcNow.AddMinutes(i * 10 + j)
            }).ToList();
        }).ToList();

        public static async Task<(IEnumerable<WorkflowDataModel>, IEnumerable<WorkspaceTypeDataModel>, IEnumerable<WorkspaceDataModel>, IEnumerable<DocumentDataModel>)> PrepareDocumentsData(IMusicKGContext context)
        {
            await context.Workflows.InsertManyAsync(Workflows);
            await context.WorkspaceTypes.InsertManyAsync(WorkspaceTypes);
            await context.Workspaces.InsertManyAsync(Workspaces);
            await context.Documents.InsertManyAsync(Documents);
            await context.Users.InsertOneAsync(User);

            return (Workflows, WorkspaceTypes, Workspaces, Documents);
        }
    }
}
