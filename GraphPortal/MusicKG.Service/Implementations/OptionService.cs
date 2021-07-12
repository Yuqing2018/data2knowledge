using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MusicKG.Service.Models;
using MongoDB.Driver;
using MusicKG.DataAccess;
using MusicKG.DataAccess.Enums;
using MusicKG.DataAccess.Models;

namespace MusicKG.Service.Implementations
{
    public class OptionService : IOptionService
    {
        private readonly IMusicKGContext context;

        public OptionService(IMusicKGContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<OptionServiceModel>> GetOptions(OptionTypeEnum? optionType)
        {
            var result = new List<OptionServiceModel>();

            if (!optionType.HasValue)
            {
                foreach (var value in Enum.GetValues(typeof(OptionTypeEnum)))
                {
                    result.AddRange(await this.GetSingleOptionType((OptionTypeEnum)value));
                }
            }
            else
            {
                result.AddRange(await this.GetSingleOptionType(optionType.Value));
            }

            return result;
        }

        private async Task<List<OptionServiceModel>> GetSingleOptionType(OptionTypeEnum optionType)
        {
            var result = new List<OptionServiceModel>();

            switch (optionType)
            {
                case OptionTypeEnum.Language:
                    result = this.GetOptionFromEnum(optionType, typeof(LanguageEnum));
                    break;
                case OptionTypeEnum.Model:
                    //TODO: Add AIModelDataModel
                    break;
                case OptionTypeEnum.UserRole:
                    result = this.GetOptionFromEnum(optionType, typeof(UserRoleEnum));
                    break;
                case OptionTypeEnum.WorkspaceType:
                    result = await this.GetWorkspaceTypeOptionAsync();
                    break;
                case OptionTypeEnum.EntityPropertyType:
                    result = await this.GetEntityPropertyTypeOptionAsync();
                    break;
                case OptionTypeEnum.Workflow:
                    result = await this.GetWorkflowOptionAsync();
                    break;
                case OptionTypeEnum.DocumentStatus:
                    result = this.GetOptionFromEnum(optionType, typeof(DocumentStatusEnum));
                    break;
                case OptionTypeEnum.TaskStatus:
                    result = this.GetOptionFromEnum(optionType, typeof(TaskStatusEnum));
                    break;
                case OptionTypeEnum.TaskDocumentStatus:
                    result = this.GetOptionFromEnum(optionType, typeof(TaskDocumentStatusEnum));
                    break;
                case OptionTypeEnum.TaskTypes:
                    result = await this.GetTaskTypeOptionAsync();
                    break;
            }

            return result;
        }

        private async Task<List<OptionServiceModel>> GetTaskTypeOptionAsync()
        {
            var taskTypes = await this.context.TaskTypes.Find(x => true).ToListAsync();

            return taskTypes?.Select(x => new OptionServiceModel
            {
                Type = OptionTypeEnum.TaskTypes,
                Value = x.Id.ToString(),
                DisplayName = x.Name
            }).ToList();
        }

        private async Task<List<OptionServiceModel>> GetWorkflowOptionAsync()
        {
            var workflows = await this.context.Workflows.Find(x => true).ToListAsync();

            return workflows.Select(x => new OptionServiceModel
            {
                Type = OptionTypeEnum.Workflow,
                Value = x.Id.ToString(),
                DisplayName = x.Name
            }).ToList();
        }

        private async Task<List<OptionServiceModel>> GetWorkspaceTypeOptionAsync()
        {
            var builder = Builders<WorkspaceTypeDataModel>.Filter;

            var filter = builder.Eq(x => x.Status, WorkspaceTypeStatusEnum.Enabled);

            var workspaceTypes = await this.context.WorkspaceTypes.Find(filter).ToListAsync();

            return workspaceTypes.Select(x => new OptionServiceModel
            {
                Type = OptionTypeEnum.WorkspaceType,
                Value = x.Id.ToString(),
                DisplayName = x.Name
            }).ToList();
        }

        private List<OptionServiceModel> GetOptionFromEnum(OptionTypeEnum optionType, Type enumType)
        {
            var result = new List<OptionServiceModel>();
            foreach (var name in Enum.GetNames(enumType))
            {
                result.Add(new OptionServiceModel
                {
                    Type = optionType,
                    DisplayName = name,
                    Value = Enum.Parse(enumType, name).ToString()
                });
            }
            return result;
        }

        private async Task<List<OptionServiceModel>> GetEntityPropertyTypeOptionAsync()
        {
            var propertyTypes = await this.context.EntityPropertyTypes.Find(x => true).ToListAsync();

            return propertyTypes.Select(x => new OptionServiceModel
            {
                Type = OptionTypeEnum.EntityPropertyType,
                Value = x.Id.ToString(),
                DisplayName = x.Name
            }).ToList();
        }
    }
}
