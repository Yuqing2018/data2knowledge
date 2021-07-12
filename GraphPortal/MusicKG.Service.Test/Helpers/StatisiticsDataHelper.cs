using MusicKG.Service.Test.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using MusicKG.DataAccess.Models;
using MongoDB.Bson;
using MusicKG.DataAccess.Enums;
using System.Threading.Tasks;
using MusicKG.DataAccess;

namespace MusicKG.Service.Test.Helpers
{
    public class StatisiticsDataHelper
    {
        public static DateTime EarliestCreatedAt = DateTime.Parse("2019-4-1 00:00:00").ToUniversalTime();

        public static DateTime LatestCreatedAt = DateTime.Parse("2019-4-3 23:59:59").ToUniversalTime();
        public static DateTime LatestFinishedAt = DateTime.Parse("2019-4-5 23:59:59").ToUniversalTime();
        public static DateTime ExpectedDueAt = DateTime.Parse("2019-4-10 23:59:59").ToUniversalTime();

        public static List<DocumentDataModel> Documents = Enumerable.Range(0, 10).Select(i =>
        {
            return new DocumentDataModel
            {
                Id = ObjectId.GenerateNewId(),
                Name = $"Document #{i}",
                Status = DocumentStatusEnum.Preannotated,
                ItemCount = (i + 1) * 5
            };
        }).ToList();

        public static List<TaskDataModel> Tasks = Enumerable.Range(0, 2).SelectMany(j => Enumerable.Range(0, 2).SelectMany(x =>
            Enumerable.Range(0, 20).Select(i =>
            {
                var createdAt = CalculateCreatedAt(i);

                return new TaskDataModel
                {
                    Id = ObjectId.GenerateNewId(),
                    Name = $"task #{j}_{x}_{i}_{createdAt}",
                    Status = TaskStatusEnum.Created,
                    WorkspaceId = TaskDataHelper.Workspaces.ToArray()[i % 3].Id,
                    CreatedAt = createdAt,
                    CreatedBy = TaskDataHelper.Users.First().Id,
                    ExpectedDueAt = ExpectedDueAt.AddDays(i),
                    Overlap = 20,
                    IsAutoApproved = true,
                    IsAutoMerged = true,
                    IsDeleted = j % 2 == 0 ? true : false,
                    Annotators = TaskDataHelper.Users.Select(u => new TaskAnnotatorDataModel
                    {
                        AnnotatorId = u.Id,
                        IsManager = u.Roles.Contains(UserRoleEnum.Manager),
                        Documents = Documents.Select(t => new TaskDocumentDataModel
                        {
                            DocumentId = t.Id,
                            Status = x == 0 ? TaskDocumentStatusEnum.Assigned : TaskDocumentStatusEnum.Annotated,
                            Results = new List<TaskDocumentResultDataModel>(),
                            AnnotatedAt = x == 0 ? DateTime.MaxValue.ToUniversalTime() : GetAnnotatedAt(i, createdAt)
                        }).ToList()
                    }).ToList(),
                };
            }).ToList()).ToList()).ToList();

        private static DateTime GetAnnotatedAt(int index, DateTime createdAt)
        {
            var hourDiffer = (LatestFinishedAt - createdAt).TotalHours;
            var addedHours = new Random().Next(1, Convert.ToInt32(hourDiffer) - 1);
            return index % 2 == 0 ? LatestFinishedAt : createdAt.AddHours(addedHours);
        }

        private static DateTime CalculateCreatedAt(int index)
        {
            switch (index % 3)
            {
                case 0:
                    return EarliestCreatedAt;
                case 1:
                    return LatestCreatedAt;
                case 2:
                    var hourDiffer = (LatestCreatedAt - EarliestCreatedAt).TotalHours;
                    var addedHours = new Random().Next(1, Convert.ToInt32(hourDiffer) - 1);
                    return EarliestCreatedAt.AddHours(addedHours);
                default:
                    throw new InvalidOperationException();
            }
        }

        public async static Task<(List<UserDataModel>, List<TaskDataModel>, List<DocumentDataModel>)> PrepareTasksForStatisitics(IMusicKGContext context)
        {
            await context.WorkspaceTypes.InsertManyAsync(TaskDataHelper.WorkspaceTypes);
            await context.Workspaces.InsertManyAsync(TaskDataHelper.Workspaces);
            await context.Users.InsertManyAsync(TaskDataHelper.Users);
            await context.Tasks.InsertManyAsync(Tasks);
            await context.Documents.InsertManyAsync(Documents);

            return (TaskDataHelper.Users.ToList(), Tasks, Documents);
        }
    }
}
