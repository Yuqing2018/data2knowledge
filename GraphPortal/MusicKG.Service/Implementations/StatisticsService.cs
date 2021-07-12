using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MusicKG.DataAccess.Constants;
using MusicKG.DataAccess.Enums;
using MusicKG.DataAccess.Models;
using MusicKG.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Linq;
using MusicKG.DataAccess;
using Microsoft.Extensions.Logging;
using MusicKG.Service.Helpers;

namespace MusicKG.Service.Implementations
{
    public partial class StatisticService : IStatisticService
    {
        private const string tempUserIdName = "userId";
        private const string tempAnnotatorName = "annotator";
        private const string tempRelatedDocumentName = "RelatedTaskDocuments";
        private const string tempRelatedWorkspaceName = "RelatedWorkspaces";
        private const string tempRelatedWorkspaceTypeName = "RelatedWorkspaceTypes";
        private const string tempRelatedUserName = "RelatedUsers";
        private const string tempWorkspaceId = "WorkspaceId";
        private const string tempMeName = "Me";

        private readonly IMusicKGContext context;
        private readonly ILogger<StatisticService> logger;

        public StatisticService(
            IMusicKGContext context,
            ILogger<StatisticService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<IEnumerable<StatisticsOverviewServiceModel>> GetStatisticsOverviewAsync(DateTime fromDate, DateTime toDate, string workspaceTypeId)
        {
            var statisiticsResult = await GetStatisiticsResultAsync(fromDate, toDate, workspaceTypeId, null);

            var result = statisiticsResult.Select(x =>
            {
                return new StatisticsOverviewServiceModel
                {
                    User = new UserServiceModel
                    {
                        Id = x.Id.ToString(),
                        Name = x.Name,
                        Roles = x.Roles
                    },
                    Statistics = new StatisticsServiceModel
                    {
                        CreatedTaskCount = x.Roles.Contains(UserRoleEnum.Manager) ?
                            x.TaskStatistics.Count(t => t.IsCreatedInTime) : 0,
                        FinishedTaskCount = x.TaskStatistics.Count(t => t.IsFinishedInTime),
                        NotFinishedTaskCount = x.TaskStatistics.Count(t => !t.IsFinishedInTime),
                        FinishedDocumentCount = x.TaskStatistics.Sum(t => t.FinishedDocumentCount),
                        NotFinishedDocumentCount = x.TaskStatistics.Sum(t => t.NotFinishedDocumentCount)
                    }
                };
            });

            return result;
        }

        public async Task<StatisticsDetailsServiceModel> GetStatisticsDetailsAsync(string userId, DateTime fromDate, DateTime toDate, string workspaceTypeId)
        {
            var statisiticsResult = await GetStatisiticsResultAsync(fromDate, toDate, workspaceTypeId, userId);

            if (statisiticsResult == null || statisiticsResult.Count == 0)
            {
                ErrorHelper.ThrowException(Resources.MusicKGMessages.UserNotExistMessage, System.Net.HttpStatusCode.BadRequest);
            }

            var statDetails = statisiticsResult.First();

            return new StatisticsDetailsServiceModel
            {
                User = new UserServiceModel
                {
                    Id = statDetails.Id.ToString(),
                    Name = statDetails.Name,
                    Roles = statDetails.Roles
                },
                TaskStatisticsDetails = statDetails.TaskStatistics.Select(x => new TaskStatisticsDetailsServiceModel
                {
                    TaskId = x.Id.TaskInfo.Id.ToString(),
                    TaskName = x.Id.TaskInfo.TaskName,
                    RelatedUsers = x.Id.RelatedUsers.Select(u => new TaskAnnotatorServiceModel
                    {
                        Annotator = new UserServiceModel
                        {
                            Id = u.UserId.ToString(),
                            Name = u.UserName
                        },
                        IsManager = u.IsManager
                    }),
                    CreatedAt = x.Id.TaskInfo.CreatedAt,
                    ExpectedDueAt = x.Id.TaskInfo.ExpectedDueAt,
                    Workspace = new WorkspaceServiceModel
                    {
                        Id = x.Id.TaskInfo.WorkspaceId.ToString(),
                        Name = x.WorkspaceName,
                        Type = new WorkspaceTypeServiceModel
                        {
                            Id = x.WorkspaceType.ToString(),
                            Name = x.WorkspaceTypeName
                        }
                    },
                    FinishedDocumentCount = x.FinishedDocumentCount
                })
            };
        }

        private async Task<List<StatisiticsDetailsResult>> GetStatisiticsResultAsync(DateTime fromDate, DateTime toDate, string workspaceTypeId, string userId)
        {
            var pipeline = new List<BsonDocument>();
            pipeline.Add(new BsonDocument
            {
                { "$project",
                    new BsonDocument
                    {
                        { $"{nameof(UserDataModel.Roles)}", 1 },
                        { $"{nameof(UserDataModel.Status)}", 1 },
                        { $"{nameof(UserDataModel.Name)}", 1 }
                    }
                }
            });

            pipeline.Add(new BsonDocument
            {
                {
                    "$lookup", new BsonDocument
                    {
                        { "from", $"{CollectionNames.Task}"},
                        { "let", new BsonDocument { { tempUserIdName, "$_id" } } },
                        { "pipeline", BuildTaskFilters(fromDate, toDate, workspaceTypeId, !string.IsNullOrWhiteSpace(userId)) },
                        { "as", nameof(StatisiticsDetailsResult.TaskStatistics) }
                    }
                }
            });

            if (string.IsNullOrWhiteSpace(userId))
            {
                pipeline.Add(new BsonDocument
                {
                    {
                        "$match", new BsonDocument {
                            { "$and", new BsonArray {
                                new BsonDocument { { $"{nameof(UserDataModel.Roles)}", new BsonDocument { { "$in", new BsonArray { UserRoleEnum.Manager, UserRoleEnum.Annotator, UserRoleEnum.Inspector, UserRoleEnum.Acceptor } } } } },
                                new BsonDocument { { $"{nameof(UserDataModel.Status)}", UserStatusEnum.Enabled } } }
                            } }
                    }
                });
            }
            else
            {
                pipeline.Add(new BsonDocument
                {
                    {
                        "$match", new BsonDocument { { "_id", new ObjectId(userId) } }
                    }
                });
            }

            return await context.Users.Aggregate<StatisiticsDetailsResult>(pipeline).ToListAsync();
        }

        private BsonArray BuildTaskFilters(DateTime fromDate, DateTime toDate, string workspaceTypeId, bool queryForUser)
        {
            if (!queryForUser)
            {
                return new BsonArray
                {
                    new BsonDocument { { "$project", BuildProjectedFields() } },
                    BsonDocumentHelper.BuildUnwindStage($"${nameof(TaskDataModel.Annotators)}"),
                    new BsonDocument { { "$group", BuildGroupRelatedUsers(queryForUser) } },
                    BsonDocumentHelper.BuildUnwindStage( $"${tempMeName}.{nameof(TaskAnnotatorDataModel.Documents)}" ),
                    BsonDocumentHelper.BuildLookupStage(CollectionNames.Document, $"{tempMeName}.{nameof(TaskAnnotatorDataModel.Documents)}.{nameof(TaskDocumentDataModel.DocumentId)}", "_id", tempRelatedDocumentName),
                    BsonDocumentHelper.BuildLookupStage(CollectionNames.Workspace, $"_id.{nameof(TaskDataModel.WorkspaceId)}", "_id", tempRelatedWorkspaceName),
                    BsonDocumentHelper.BuildUnwindStage($"${tempRelatedDocumentName}"),
                    BsonDocumentHelper.BuildUnwindStage($"${tempRelatedWorkspaceName}"),
                    new BsonDocument { { "$group", BuildFinalGroupped(fromDate, toDate, queryForUser) } },
                    new BsonDocument { { "$addFields", new BsonDocument { { nameof(TaskStatisticsDetailsResult.IsCreatedInTime), BuildInTimeFilter($"$_id.{nameof(TaskResult.TaskInfo)}.{nameof(TaskInfoResult.CreatedAt)}", fromDate, toDate) } } } },
                    new BsonDocument { { "$addFields", new BsonDocument { { nameof(TaskStatisticsDetailsResult.IsFinishedInTime), BuildInTimeFilter($"${nameof(TaskStatisticsDetailsResult.FinishedAt)}", fromDate, toDate) } } } },
                    new BsonDocument { { "$match", BuildStatisitcsFilter(fromDate, toDate, workspaceTypeId)} }
                };
            }
            else
            {
                return new BsonArray
                {
                    new BsonDocument { { "$project", BuildProjectedFields() } },
                    BsonDocumentHelper.BuildUnwindStage($"${nameof(TaskDataModel.Annotators)}"),
                    BsonDocumentHelper.BuildLookupStage(CollectionNames.User, $"{nameof(TaskDataModel.Annotators)}.{nameof(TaskAnnotatorDataModel.AnnotatorId)}", "_id", tempRelatedUserName),
                    BsonDocumentHelper.BuildUnwindStage($"${tempRelatedUserName}"),
                    new BsonDocument { { "$group", BuildGroupRelatedUsers(queryForUser) } },
                    BsonDocumentHelper.BuildUnwindStage( $"${tempMeName}.{nameof(TaskAnnotatorDataModel.Documents)}" ),
                    BsonDocumentHelper.BuildLookupStage(CollectionNames.Document, $"{tempMeName}.{nameof(TaskAnnotatorDataModel.Documents)}.{nameof(TaskDocumentDataModel.DocumentId)}", "_id", tempRelatedDocumentName),
                    BsonDocumentHelper.BuildLookupStage(CollectionNames.Workspace, $"_id.{nameof(TaskDataModel.WorkspaceId)}", "_id", tempRelatedWorkspaceName),
                    BsonDocumentHelper.BuildUnwindStage($"${tempRelatedDocumentName}"),
                    BsonDocumentHelper.BuildUnwindStage($"${tempRelatedWorkspaceName}"),
                    BsonDocumentHelper.BuildLookupStage(CollectionNames.WorkspaceType, $"{tempRelatedWorkspaceName}.{nameof(WorkspaceDataModel.Type)}", "_id", tempRelatedWorkspaceTypeName),
                    BsonDocumentHelper.BuildUnwindStage($"${tempRelatedWorkspaceTypeName}"),
                    new BsonDocument { { "$group", BuildFinalGroupped(fromDate, toDate, queryForUser) } },
                    new BsonDocument { { "$addFields", new BsonDocument { { nameof(TaskStatisticsDetailsResult.IsCreatedInTime), BuildInTimeFilter($"$_id.{nameof(TaskResult.TaskInfo)}.{nameof(TaskInfoResult.CreatedAt)}", fromDate, toDate) } } } },
                    new BsonDocument { { "$addFields", new BsonDocument { { nameof(TaskStatisticsDetailsResult.IsFinishedInTime), BuildInTimeFilter($"${nameof(TaskStatisticsDetailsResult.FinishedAt)}", fromDate, toDate) } } } },
                    new BsonDocument { { "$match", BuildStatisitcsFilter(fromDate, toDate, workspaceTypeId)} }
                };
            }
        }

        private BsonDocument BuildFinalGroupped(DateTime fromDate, DateTime toDate, bool queryForUser)
        {
            return new BsonDocument
            {
                { "_id", new BsonDocument {
                    { nameof(TaskResult.TaskInfo), "$_id" },
                    { nameof(TaskResult.RelatedUsers), $"${nameof(TaskResult.RelatedUsers)}" },
                } },
                { $"{nameof(TaskStatisticsDetailsResult.WorkspaceName)}", BsonDocumentHelper.BuildFirstItem($"${tempRelatedWorkspaceName}.{nameof(WorkspaceDataModel.Name)}") },
                { $"{nameof(TaskStatisticsDetailsResult.WorkspaceType)}", BsonDocumentHelper.BuildFirstItem($"${tempRelatedWorkspaceName}.{nameof(WorkspaceDataModel.Type)}") },
                { $"{nameof(TaskStatisticsDetailsResult.WorkspaceTypeName)}", BsonDocumentHelper.BuildFirstItem(queryForUser ? $"${tempRelatedWorkspaceTypeName}.{nameof(WorkspaceTypeDataModel.Name)}" : string.Empty) },
                { $"{nameof(TaskStatisticsDetailsResult.FinishedAt)}", new BsonDocument { { "$max", $"${tempMeName}.{nameof(TaskAnnotatorDataModel.Documents)}.{nameof(TaskDocumentDataModel.AnnotatedAt)}" } } },
                { $"{nameof(TaskStatisticsDetailsResult.NotFinishedDocumentCount)}", new BsonDocument { { "$sum", BuildNotFinishedDocumentCountFilter(toDate) } } },
                { $"{nameof(TaskStatisticsDetailsResult.FinishedDocumentCount)}", new BsonDocument { { "$sum", BuildFinishedDocumentCountFilter(fromDate, toDate) } } }
            };
        }

        private BsonDocument BuildGroupRelatedUsers(bool queryForUser)
        {
            if (!queryForUser)
            {
                return new BsonDocument
                {
                    { "_id", BuildTaskInfo()},
                    { tempMeName, BuildAnnotators() }
                };
            }
            else
            {
                return new BsonDocument
                {
                    { "_id", BuildTaskInfo()},
                    { nameof(TaskResult.RelatedUsers), new BsonDocument {
                            { "$push", new BsonDocument {
                                    { nameof(RelatedUserResult.UserId), $"${tempRelatedUserName}._id" },
                                    { nameof(RelatedUserResult.UserName), $"${tempRelatedUserName}.{nameof(UserDataModel.Name)}" },
                                    { nameof(RelatedUserResult.IsManager), $"${nameof(TaskDataModel.Annotators)}.{nameof(TaskAnnotatorDataModel.IsManager)}" }
                            }}
                        }
                    },
                    { tempMeName, BuildAnnotators() }
                };
            }
        }

        private BsonDocument BuildTaskInfo()
        {
            return new BsonDocument {
                { "_id", "$_id" },
                { nameof(TaskInfoResult.TaskName), $"${nameof(TaskDataModel.Name)}" },
                { $"{nameof(TaskDataModel.WorkspaceId)}", $"${nameof(TaskDataModel.WorkspaceId)}" },
                { $"{nameof(TaskDataModel.CreatedAt)}", $"${nameof(TaskDataModel.CreatedAt)}" },
                { $"{nameof(TaskDataModel.ExpectedDueAt)}", $"${nameof(TaskDataModel.ExpectedDueAt)}" },
                { $"{nameof(TaskDataModel.IsDeleted)}", $"${nameof(TaskDataModel.IsDeleted)}" },
            };
        }

        private BsonDocument BuildAnnotators()
        {
            return new BsonDocument {
                { "$max", new BsonDocument {
                    { "$cond", new BsonDocument {
                        { "if", new BsonDocument {
                            { "$eq", new BsonArray { $"${nameof(TaskDataModel.Annotators)}.{nameof(TaskAnnotatorDataModel.AnnotatorId)}", $"$${tempUserIdName}" } }
                        } },
                        { "then", $"${nameof(TaskDataModel.Annotators)}" },
                        { "else", BsonNull.Value}
                    } }
                } }
            };
        }

        private BsonDocument BuildInTimeFilter(string fieldName, DateTime fromDate, DateTime toDate)
        {
            return new BsonDocument
            {
                { "$cond", new BsonDocument {
                    { "if", new BsonDocument {
                        {
                            "$and", new BsonArray {
                                BsonDocumentHelper.BuildCompareStage("$lte", fieldName, toDate),
                                BsonDocumentHelper.BuildCompareStage("$gte", fieldName, fromDate)
                            }
                        } }
                    },
                    { "then", true },
                    { "else", false } }
                }
            };
        }

        private BsonDocument BuildProjectedFields()
        {
            return new BsonDocument
            {
                { $"{nameof(TaskDataModel.Annotators)}", 1 },
                { $"{nameof(TaskDataModel.CreatedAt)}", 1 },
                { $"{nameof(TaskDataModel.Name)}", 1 },
                { $"{nameof(TaskDataModel.WorkspaceId)}", 1 },
                { $"{nameof(TaskDataModel.ExpectedDueAt)}", 1 },
                { $"{nameof(TaskDataModel.IsDeleted)}", 1 }
            };
        }

        private BsonDocument BuildFinishedDocumentCountFilter(DateTime fromDate, DateTime toDate)
        {
            return new BsonDocument
            {
                { "$cond", new BsonDocument {
                    { "if", BsonDocumentHelper.BuildInTimeCondition($"${tempMeName}.{nameof(TaskAnnotatorDataModel.Documents)}.{nameof(TaskDocumentDataModel.AnnotatedAt)}", fromDate, toDate) },
                    { "then", 1 },
                    { "else", 0 } }
                }
            };
        }

        private BsonDocument BuildNotFinishedDocumentCountFilter(DateTime toDate)
        {
            return new BsonDocument
            {
                { "$cond", new BsonDocument {
                    { "if", BsonDocumentHelper.BuildCompareStage("$gt", $"${tempMeName}.{nameof(TaskAnnotatorDataModel.Documents)}.{nameof(TaskDocumentDataModel.AnnotatedAt)}", toDate) },
                    { "then", 1 },
                    { "else", 0 } }
                }
            };
        }

        private BsonDocument BuildStatisitcsFilter(DateTime fromDate, DateTime toDate, string workspaceTypeId)
        {
            if (string.IsNullOrWhiteSpace(workspaceTypeId))
            {
                return new BsonDocument
                {
                    {
                        "$and", new BsonArray {
                            new BsonDocument { { $"_id.{nameof(TaskResult.TaskInfo)}.{nameof(TaskDataModel.IsDeleted)}", new BsonDocument { { "$eq", false } } } },
                            BsonDocumentHelper.BuildDateTimeFilter($"_id.{nameof(TaskResult.TaskInfo)}.{nameof(TaskInfoResult.CreatedAt)}", "$lte", toDate),
                            BsonDocumentHelper.BuildDateTimeFilter(nameof(TaskStatisticsDetailsResult.FinishedAt), "$gte", fromDate) }
                    }
                };
            }
            else
            {
                return new BsonDocument
                {
                    {
                        "$and", new BsonArray {
                            new BsonDocument { { $"_id.{nameof(TaskResult.TaskInfo)}.{nameof(TaskDataModel.IsDeleted)}", new BsonDocument { { "$eq", false } } } },
                            BsonDocumentHelper.BuildDateTimeFilter($"_id.{nameof(TaskResult.TaskInfo)}.{nameof(TaskInfoResult.CreatedAt)}", "$lte", toDate),
                            BsonDocumentHelper.BuildDateTimeFilter(nameof(TaskStatisticsDetailsResult.FinishedAt), "$gte", fromDate),
                            new BsonDocument { { nameof(TaskStatisticsDetailsResult.WorkspaceType), new BsonDocument { { "$eq", new ObjectId(workspaceTypeId) } } } } }
                    }
                };
            }
        }

        [BsonIgnoreExtraElements]
        private class StatisiticsDetailsResult : UserDataModel
        {
            public IEnumerable<TaskStatisticsDetailsResult> TaskStatistics { get; set; }
        }

        private class TaskStatisticsDetailsResult
        {
            public TaskResult Id { get; set; }
            public string WorkspaceName { get; set; }
            public ObjectId WorkspaceType { get; set; }
            public string WorkspaceTypeName { get; set; }
            public DateTime FinishedAt { get; set; }
            public long NotFinishedDocumentCount { get; set; }
            public long FinishedDocumentCount { get; set; }
            public bool IsCreatedInTime { get; set; }
            public bool IsFinishedInTime { get; set; }
        }

        private class TaskResult
        {
            public TaskInfoResult TaskInfo { get; set; }
            public IEnumerable<RelatedUserResult> RelatedUsers { get; set; }
        }

        private class TaskInfoResult
        {
            [BsonId]
            public ObjectId Id { get; set; }
            public string TaskName { get; set; }
            public ObjectId WorkspaceId { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime ExpectedDueAt { get; set; }
            public bool IsDeleted { get; set; }
        }

        private class RelatedUserResult
        {
            public ObjectId UserId { get; set; }
            public string UserName { get; set; }
            public bool IsManager { get; set; }
        }
    }
}
