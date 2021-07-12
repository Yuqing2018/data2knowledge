using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using Moq;
using MusicKG.DataAccess.Enums;
using MusicKG.Service;
using MusicKG.Service.Models;
using MusicKG.WebApi.Controllers;
using MusicKG.WebApi.Contract.ViewModels;
using MusicKG.WebApi.Contract.Attributes;
using MusicKG.WebApi.Contract.BindingModels;
using Xunit;
using static System.Threading.Tasks.Task;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MusicKG.Service.Helpers;
using MusicKG.Service.Resources;
using System.Net;

namespace MusicKG.WebApi.Test.Controllers
{
    public partial class TaskCreationRuleControllerTest
    {
        [Fact]
        public async Task GetAutoTaskCreationRuleTest()
        {
            var existTaskRule = new TaskCreationRuleServiceModel()
            {
                WorkspaceId = ObjectId.GenerateNewId().ToString(),
                Rules = Enumerable.Range(1, 3).Select(i => new AutoTaskRuleServiceModel
                {
                    Name = $"Rule {i}",
                    Annotators = new List<string>() { },
                    MaxFinishDays = 7,
                    DocumentCount = 1,
                    DocumentTags = new List<string>() { },
                    IsAutoApproved = true,
                    IsAutoMerged = true,
                    Overlap = 1
                }).ToList(),
            };

            var taskRuleServiceMock = new Mock<ITaskCreationRuleService>();
            taskRuleServiceMock.Setup(m => m.GetTaskCreationRuleAsync(It.IsAny<string>()))
                .Returns(FromResult(existTaskRule));

            var controller = new TaskCreationRuleController(taskRuleServiceMock.Object);

            var taskReturned = await controller.GetAutoTaskCreationRuleAsync(existTaskRule.WorkspaceId);

            Assert.All(taskReturned.Rules, a =>
            {
                var taskRuleExpected = existTaskRule.Rules.Where(u => u.Name.Equals(a.Name)).First();
                Assert.Equal(taskRuleExpected.Name, a.Name);
                Assert.Equal(taskRuleExpected.Annotators, a.Annotators);
                Assert.Equal(taskRuleExpected.MaxFinishDays, a.MaxFinishDays);
                Assert.Equal(taskRuleExpected.DocumentTags, a.DocumentTags);
                Assert.Equal(taskRuleExpected.DocumentCount, a.DocumentCount);
                Assert.Equal(taskRuleExpected.IsAutoApproved, a.IsAutoApproved);
                Assert.Equal(taskRuleExpected.IsAutoMerged, a.IsAutoMerged);
                Assert.Equal(taskRuleExpected.Overlap, a.Overlap);
            });
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public async Task UpdateAutoTaskCreationRuleTest(bool hasDefault, bool defaultContainsTags)
        {
            var (workspaces, taskRules) = PrepareTaskRuleTestData();

            var taskRuleToUpdateBindingModel = new TaskCreationRuleUpdateBindingModel()
            {
                Rules = Enumerable.Range(1, 2).Select(i => new AutoTaskRuleUpdateBindingModel
                {
                    Name = $"UpdateTaskRule_{i}",
                    Annotators = new List<string>() {"Annotator1","Annoatator2" },
                    MaxFinishDays = 2,
                    DocumentCount = 2,
                    DocumentTags = new List<string>() {"Update","Test" },
                    IsAutoApproved = true,
                    IsAutoMerged = true,
                    Overlap = 1
                }).ToList(),
            };

            if (hasDefault)
            {
                taskRuleToUpdateBindingModel.Rules.Add(new AutoTaskRuleUpdateBindingModel
                {
                    Name = "Default",
                    Annotators = new List<string>() { "Annotator1", "Annoatator2" },
                    MaxFinishDays = 2,
                    DocumentCount = 2,
                    DocumentTags = defaultContainsTags ? new List<string>() { "Update", "Test" } : null,
                    IsAutoApproved = true,
                    IsAutoMerged = true,
                    Overlap = 1
                });
            }

            var updateServiceModel = new TaskCreationRuleUpdateServiceModel
            {
                Rules = taskRuleToUpdateBindingModel.Rules?.Select(rule => new AutoTaskRuleServiceModel
                {
                    Annotators = rule.Annotators,
                    MaxFinishDays = rule.MaxFinishDays,
                    DocumentCount = rule.DocumentCount,
                    DocumentTags = rule.DocumentTags,
                    IsAutoApproved = rule.IsAutoApproved,
                    IsAutoMerged = rule.IsAutoMerged,
                    Name = rule.Name,
                    Overlap = rule.Overlap
                })?.ToList()
            };

            var taskRuleServiceMock = new Mock<ITaskCreationRuleService>();
            taskRuleServiceMock.Setup(x => x.UpdateRuleAsync(It.IsAny<string>(), It.IsAny<TaskCreationRuleUpdateServiceModel>())).Returns(Task.FromResult(updateServiceModel));
            var controller = new TaskCreationRuleController(taskRuleServiceMock.Object);

            if (hasDefault && !defaultContainsTags)
            {
                await controller.UpdateAutoTaskCreationRuleAsync(ObjectId.GenerateNewId().ToString(), taskRuleToUpdateBindingModel);

                taskRuleServiceMock.Verify(t => t.UpdateRuleAsync(It.IsAny<string>(), It.IsAny<TaskCreationRuleUpdateServiceModel>()), Times.Once);
            }
            else
            {
                var exception = await Assert.ThrowsAsync<ErrorHelper.ErrorMessageException>(() => controller.UpdateAutoTaskCreationRuleAsync(ObjectId.GenerateNewId().ToString(), taskRuleToUpdateBindingModel));

                if (!hasDefault)
                {
                    Assert.Equal(MusicKGMessages.MustHaveDefaultRuleForTaskCreation, exception.Message);
                }
                else if (defaultContainsTags)
                {
                    Assert.Equal(MusicKGMessages.DefaultRuleMustNotContainAnyTag, exception.Message);
                }
                Assert.Equal(HttpStatusCode.BadRequest, exception.Data[ErrorHelper.StatusCodeKey]);
                taskRuleServiceMock.Verify(t => t.UpdateRuleAsync(It.IsAny<string>(), It.IsAny<TaskCreationRuleUpdateServiceModel>()), Times.Never);
            }
        }

        private (IEnumerable<WorkspaceServiceModel>, IEnumerable<TaskCreationRuleServiceModel>) PrepareTaskRuleTestData()
        {
            var workspaces = Enumerable.Range(1, 3).Select(i => new WorkspaceServiceModel
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = $"Workspace {i}",
                Type = new WorkspaceTypeServiceModel
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = $"Workspace type {i}"
                }
            }).ToList();

            var taskRules = workspaces.Select(x => new TaskCreationRuleServiceModel()
            {
                WorkspaceId = x.Id,
                Rules = Enumerable.Range(1, 3).Select(i => new AutoTaskRuleServiceModel
                {
                    Name = $"Rule {i}",
                    Annotators = new List<string>() { },
                    MaxFinishDays = 7,
                    DocumentCount = 1,
                    DocumentTags = new List<string>() { },
                    IsAutoApproved = true,
                    IsAutoMerged = true,
                    Overlap = 1
                }).ToList(),
            }).ToList();
            return (workspaces, taskRules);
        }
    }
}
