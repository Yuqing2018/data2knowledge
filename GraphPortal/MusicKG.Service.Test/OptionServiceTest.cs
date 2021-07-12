using MusicKG.Service.Test.Fixtures;
using MusicKG.DataAccess;
using MusicKG.Service.Helpers;
using MusicKG.Service.Implementations;
using MusicKG.DataAccess.Models;
using MusicKG.DataAccess.Enums;
using MusicKG.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Linq;

namespace MusicKG.Service.Test
{
    [Collection("MongoCollection")]
    public class OptionServiceTest
    {
        private readonly MongoFixture mongoFixture;
        private readonly IMusicKGContext context;

        private readonly IOptionService serviceUnderTest;

        private WorkspaceTypeDataModel[] expectedWorkspaceTypes;
        private WorkflowDataModel[] expectedWorkflows;
        private OntologyEntityPropertyTypeDataModel[] expectedEntityPropertyTypes;
        private TaskTypeDataModel[] expectedTaskTypes;

        public OptionServiceTest(MongoFixture mongoFixture)
        {
            this.mongoFixture = mongoFixture;
            this.context = new MusicKGContext(mongoFixture.Context.Client, RandomStringHelper.RandomString(10));
            this.serviceUnderTest = new OptionService(this.context);
            this.PrepareDataAsync().Wait();
        }

        [Fact]
        public async Task GetAllOptions()
        {
            var result = await this.serviceUnderTest.GetOptions(null);

            this.AssertEnumOptions(result.Where(x => x.Type == OptionTypeEnum.Language), typeof(LanguageEnum), OptionTypeEnum.Language);
            this.AssertEnumOptions(result.Where(x => x.Type == OptionTypeEnum.UserRole), typeof(UserRoleEnum), OptionTypeEnum.UserRole);
            this.AssertEnumOptions(result.Where(x => x.Type == OptionTypeEnum.DocumentStatus), typeof(DocumentStatusEnum), OptionTypeEnum.DocumentStatus);
            this.AssertEnumOptions(result.Where(x => x.Type == OptionTypeEnum.TaskStatus), typeof(TaskStatusEnum), OptionTypeEnum.TaskStatus);
            this.AssertEnumOptions(result.Where(x => x.Type == OptionTypeEnum.TaskDocumentStatus), typeof(TaskDocumentStatusEnum), OptionTypeEnum.TaskDocumentStatus);
            this.AssertWorkspaceTypes(result.Where(x => x.Type == OptionTypeEnum.WorkspaceType));
            this.AssertWorkflows(result.Where(x => x.Type == OptionTypeEnum.Workflow));
            this.AssertEntityPropertyTypes(result.Where(x => x.Type == OptionTypeEnum.EntityPropertyType));
            this.AssertTaskTypes(result.Where(x => x.Type == OptionTypeEnum.TaskTypes));
        }

        [Fact]
        public async Task GetLanguageOptions()
        {
            var result = await this.serviceUnderTest.GetOptions(OptionTypeEnum.Language);

            this.AssertEnumOptions(result, typeof(LanguageEnum), OptionTypeEnum.Language);
        }

        [Fact]
        public async Task GetUserRoleOptions()
        {
            var result = await this.serviceUnderTest.GetOptions(OptionTypeEnum.UserRole);

            this.AssertEnumOptions(result, typeof(UserRoleEnum), OptionTypeEnum.UserRole);
        }

        [Fact]
        public async Task GetWorkflowsOptions()
        {
            var result = await this.serviceUnderTest.GetOptions(OptionTypeEnum.Workflow);

            this.AssertWorkflows(result);
        }

        [Fact]
        public async Task GetWorkspaceTypeOptions()
        {
            var result = await this.serviceUnderTest.GetOptions(OptionTypeEnum.WorkspaceType);

            this.AssertWorkspaceTypes(result);
        }

        [Fact]
        public async Task GetTaskTypeOptions()
        {
            var result = await this.serviceUnderTest.GetOptions(OptionTypeEnum.TaskTypes);

            this.AssertTaskTypes(result);
        }

        [Fact]
        public async Task GetDocumentStatus()
        {
            var result = await this.serviceUnderTest.GetOptions(OptionTypeEnum.DocumentStatus);

            this.AssertEnumOptions(result, typeof(DocumentStatusEnum), OptionTypeEnum.DocumentStatus);
        }

        [Fact]
        public async Task GetTaskStatus()
        {
            var result = await this.serviceUnderTest.GetOptions(OptionTypeEnum.TaskStatus);

            this.AssertEnumOptions(result, typeof(TaskStatusEnum), OptionTypeEnum.TaskStatus);
        }

        [Fact]
        public async Task GetEntityPropertyTypeOptions()
        {
            var result = await this.serviceUnderTest.GetOptions(OptionTypeEnum.EntityPropertyType);

            this.AssertEntityPropertyTypes(result);
        }
        
        [Fact]
        public async Task GetTaskDocumentStatusOptions()
        {
            var result = await this.serviceUnderTest.GetOptions(OptionTypeEnum.TaskDocumentStatus);

            this.AssertEnumOptions(result, typeof(TaskDocumentStatusEnum), OptionTypeEnum.TaskDocumentStatus);
        }

        private void AssertWorkspaceTypes(IEnumerable<OptionServiceModel> actual)
        {
            var actualArray = actual.ToArray();

            var expectedArray = this.expectedWorkspaceTypes.Where(x => x.Status == WorkspaceTypeStatusEnum.Enabled).ToArray();

            Assert.Equal(expectedArray.Length, actualArray.Length);

            for (int i = 0; i < actualArray.Length; i++)
            {
                Assert.Equal(OptionTypeEnum.WorkspaceType, actualArray[i].Type);
                Assert.Equal(expectedArray[i].Name, actualArray[i].DisplayName);
                Assert.Equal(expectedArray[i].Id.ToString(), actualArray[i].Value);
            }
        }

        private void AssertTaskTypes(IEnumerable<OptionServiceModel> actual)
        {
            var actualArray = actual.ToArray();

            var expectedArray = this.expectedTaskTypes.ToArray();

            Assert.Equal(expectedArray.Length, actualArray.Length);

            for (int i = 0; i < actualArray.Length; i++)
            {
                Assert.Equal(OptionTypeEnum.TaskTypes, actualArray[i].Type);
                Assert.Equal(expectedArray[i].Name, actualArray[i].DisplayName);
                Assert.Equal(expectedArray[i].Id.ToString(), actualArray[i].Value);
            }
        }

        private void AssertWorkflows(IEnumerable<OptionServiceModel> actual)
        {
            var actualArray = actual.ToArray();

            Assert.Equal(this.expectedWorkflows.Length, actualArray.Length);

            for (int i = 0; i < actualArray.Length; i++)
            {
                Assert.Equal(OptionTypeEnum.Workflow, actualArray[i].Type);
                Assert.Equal(this.expectedWorkflows[i].Name, actualArray[i].DisplayName);
                Assert.Equal(this.expectedWorkflows[i].Id.ToString(), actualArray[i].Value);
            }
        }

        private void AssertEnumOptions(IEnumerable<OptionServiceModel> actual, Type enumType, OptionTypeEnum expectedOptionType)
        {
            var names = Enum.GetNames(enumType);

            var actualArray = actual.ToArray();

            Assert.Equal(names.Length, actualArray.Length);

            for (int i = 0; i < names.Length; i++)
            {
                Assert.Equal(expectedOptionType, actualArray[i].Type);
                Assert.Equal(names[i], actualArray[i].DisplayName);
                Assert.Equal(Enum.Parse(enumType, names[i]).ToString(), actualArray[i].Value);
            }
        }

        private void AssertEntityPropertyTypes(IEnumerable<OptionServiceModel> actual)
        {
            var actualArray = actual.ToArray();

            var expectedArray = this.expectedEntityPropertyTypes.ToArray();

            Assert.Equal(expectedArray.Length, actualArray.Length);

            for (int i = 0; i < actualArray.Length; i++)
            {
                Assert.Equal(OptionTypeEnum.EntityPropertyType, actualArray[i].Type);
                Assert.Equal(expectedArray[i].Name, actualArray[i].DisplayName);
                Assert.Equal(expectedArray[i].Id.ToString(), actualArray[i].Value);
            }
        }
        
        private async Task PrepareDataAsync()
        {
            this.expectedWorkspaceTypes = new WorkspaceTypeDataModel[]
            {
                new WorkspaceTypeDataModel
                {
                    Name = "WorkspaceType1",
                    Status = WorkspaceTypeStatusEnum.Enabled
                },
                new WorkspaceTypeDataModel
                {
                    Name = "WorkspaceType2",
                    Status = WorkspaceTypeStatusEnum.Enabled
                },
                new WorkspaceTypeDataModel
                {
                    Name = "WorkspaceType3",
                    Status = WorkspaceTypeStatusEnum.Disabled
                },
                new WorkspaceTypeDataModel
                {
                    Name = "WorkspaceType4",
                    Status = WorkspaceTypeStatusEnum.Enabled
                },
                new WorkspaceTypeDataModel
                {
                    Name = "WorkspaceType5",
                    Status = WorkspaceTypeStatusEnum.Disabled
                },
            };

            await this.context.WorkspaceTypes.InsertManyAsync(this.expectedWorkspaceTypes);

            this.expectedWorkflows = new WorkflowDataModel[]
            {
                new WorkflowDataModel
                {
                    Name = "Workflow1"
                },
                new WorkflowDataModel
                {
                    Name = "Workflow2"
                },
                new WorkflowDataModel
                {
                    Name = "Workflow3"
                },
                new WorkflowDataModel
                {
                    Name = "Workflow4"
                },
            };

            await this.context.Workflows.InsertManyAsync(this.expectedWorkflows);

            this.expectedEntityPropertyTypes = new OntologyEntityPropertyTypeDataModel[]
            {
                new OntologyEntityPropertyTypeDataModel()
                {
                    Name = "entityPropertyType1"

                },
                new OntologyEntityPropertyTypeDataModel()
                {
                    Name = "entityPropertyType2"
                },
                new OntologyEntityPropertyTypeDataModel()
                {
                    Name = "entityPropertyType3"
                },
                new OntologyEntityPropertyTypeDataModel()
                {
                    Name = "entityPropertyType4"
                }
            };

            await this.context.EntityPropertyTypes.InsertManyAsync(this.expectedEntityPropertyTypes);

            this.expectedTaskTypes = Enumerable.Range(1, 5).Select(i => new TaskTypeDataModel
            {
                Name = $"TaskType_{i}"
            }).ToArray();

            await this.context.TaskTypes.InsertManyAsync(this.expectedTaskTypes);
        }

        private IEnumerable<OptionDataModel> GenerateEnumData(Type enumType, OptionTypeEnum optionType)
        {
            List<OptionDataModel> result = new List<OptionDataModel>();
            foreach (string name in enumType.GetEnumNames())
            {
                result.Add(new OptionDataModel
                {
                    Type = optionType,
                    DisplayName = name,
                    Value = name
                });
            }
            return result;
        }
    }
}
