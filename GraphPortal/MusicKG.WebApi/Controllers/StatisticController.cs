using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicKG.DataAccess.Enums;
using MusicKG.Service;
using MusicKG.Service.Helpers;
using MusicKG.WebApi.Filters;
using MusicKG.WebApi.Contract.ViewModels;
using MusicKG.WebApi.Contract.Attributes;
using MusicKG.WebApi.Contract.BindingModels;

namespace MusicKG.WebApi.Controllers
{
    /// <summary>
    /// Statistic controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.InternalServerError)]
    public class StatisticController : ControllerBase
    {
        private readonly IStatisticService statisticService;

        /// <summary>
        /// Statistic controller constructor.
        /// </summary>
        public StatisticController(IStatisticService statisticService)
        {
            this.statisticService = statisticService;
        }

        /// <summary>
        /// Get overview statisitics list.
        /// </summary>
        /// <param name = "fromDate" > From date.</param>
        /// <param name = "toDate" > To date.</param>
        /// <param name = "workspaceTypeId" > Workspace type id.</param>
        /// <returns>Statisitics overview list.</returns>
        [HttpGet]
        [Route("Overview")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(StatisiticsOverviewViewModel), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Administrator })]
        public async Task<StatisiticsOverviewViewModel> GetStatisiticsOverview(
            [FromQuery] [RequiredLocalized] DateTime fromDate,
            [FromQuery] [RequiredLocalized] DateTime toDate,
            [FromQuery] string workspaceTypeId)
        {
            if (fromDate >= toDate)
            {
                ErrorHelper.ThrowException(Service.Resources.MusicKGMessages.FromDateGreaterThanToDateMessage, HttpStatusCode.BadRequest);
            }

            var serviceResult = await this.statisticService.GetStatisticsOverviewAsync(fromDate, toDate, workspaceTypeId);

            return new StatisiticsOverviewViewModel
            {
                AnnotatedTaskCount = serviceResult.Sum(x => x.Statistics.FinishedTaskCount),
                AnnotatedDocumentCount = serviceResult.Sum(x => x.Statistics.FinishedDocumentCount),
                Managers = serviceResult.Where(x => x.User.Roles.Contains(UserRoleEnum.Manager)).Select(x => new StatisiticsViewModel
                {
                    User = new UserViewModel
                    {
                        Id = x.User.Id,
                        Name = x.User.Name,
                        Roles = x.User.Roles
                    },
                    Statisitics = new StatisiticsOverviewDetailsViewModel
                    {
                        CreatedTaskCount = x.Statistics.CreatedTaskCount,
                        FinishedDocumentCount = x.Statistics.FinishedDocumentCount,
                        FinishedTaskCount = x.Statistics.FinishedTaskCount,
                        NotFinishedDocumentCount = x.Statistics.NotFinishedDocumentCount,
                        NotFinishedTaskCount = x.Statistics.NotFinishedTaskCount
                    }
                }),
                Annotators = serviceResult.Where(x => x.User.Roles.Contains(UserRoleEnum.Annotator)).Select(x => new StatisiticsViewModel
                {
                    User = new UserViewModel
                    {
                        Id = x.User.Id,
                        Name = x.User.Name,
                        Roles = x.User.Roles
                    },
                    Statisitics = new StatisiticsOverviewDetailsViewModel
                    {
                        CreatedTaskCount = x.Statistics.CreatedTaskCount,
                        FinishedDocumentCount = x.Statistics.FinishedDocumentCount,
                        FinishedTaskCount = x.Statistics.FinishedTaskCount,
                        NotFinishedDocumentCount = x.Statistics.NotFinishedDocumentCount,
                        NotFinishedTaskCount = x.Statistics.NotFinishedTaskCount
                    }
                }),
                Inspectors = serviceResult.Where(x => x.User.Roles.Contains(UserRoleEnum.Inspector)).Select(x => new StatisiticsViewModel
                {
                    User = new UserViewModel
                    {
                        Id = x.User.Id,
                        Name = x.User.Name,
                        Roles = x.User.Roles
                    },
                    Statisitics = new StatisiticsOverviewDetailsViewModel
                    {
                        CreatedTaskCount = x.Statistics.CreatedTaskCount,
                        FinishedDocumentCount = x.Statistics.FinishedDocumentCount,
                        FinishedTaskCount = x.Statistics.FinishedTaskCount,
                        NotFinishedDocumentCount = x.Statistics.NotFinishedDocumentCount,
                        NotFinishedTaskCount = x.Statistics.NotFinishedTaskCount
                    }
                }),
                Acceptors = serviceResult.Where(x => x.User.Roles.Contains(UserRoleEnum.Acceptor)).Select(x => new StatisiticsViewModel
                {
                    User = new UserViewModel
                    {
                        Id = x.User.Id,
                        Name = x.User.Name,
                        Roles = x.User.Roles
                    },
                    Statisitics = new StatisiticsOverviewDetailsViewModel
                    {
                        CreatedTaskCount = x.Statistics.CreatedTaskCount,
                        FinishedDocumentCount = x.Statistics.FinishedDocumentCount,
                        FinishedTaskCount = x.Statistics.FinishedTaskCount,
                        NotFinishedDocumentCount = x.Statistics.NotFinishedDocumentCount,
                        NotFinishedTaskCount = x.Statistics.NotFinishedTaskCount
                    }
                })
            };
        }

        /// <summary>
        /// Get details statisitics list.
        /// </summary>
        /// <param name = "userId" >User Id.</param>
        /// <param name = "fromDate" > From date.</param>
        /// <param name = "toDate" > To date.</param>
        /// <param name = "workspaceTypeId" > Workspace type id.</param>
        /// <returns>Statisitics overview list.</returns>
        [HttpGet]
        [Route("{userId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(StatisiticsDetailsViewModel), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Administrator })]
        public async Task<StatisiticsDetailsViewModel> GetStatisiticsDetails(
            [FromRoute] [RequiredLocalized] string userId,
            [FromQuery] [RequiredLocalized] DateTime fromDate,
            [FromQuery] [RequiredLocalized] DateTime toDate,
            [FromQuery] string workspaceTypeId)
        {
            if (fromDate >= toDate)
            {
                ErrorHelper.ThrowException(Service.Resources.MusicKGMessages.FromDateGreaterThanToDateMessage, HttpStatusCode.BadRequest);
            }

            var serviceResult = await this.statisticService.GetStatisticsDetailsAsync(userId, fromDate, toDate, workspaceTypeId);

            return new StatisiticsDetailsViewModel
            {
                User = new UserViewModel
                {
                    Id = serviceResult.User.Id,
                    Name = serviceResult.User.Name,
                    Roles = serviceResult.User.Roles
                },
                StatisiticsDetails = serviceResult.TaskStatisticsDetails.Select(x => new TaskStatisiticsDetailsViewModel
                {
                    TaskId = x.TaskId,
                    TaskName = x.TaskName,
                    CreatedAt = x.CreatedAt,
                    ExpectedDueAt = x.ExpectedDueAt,
                    FinishedDocumentCount = x.FinishedDocumentCount,
                    Annotators = x.RelatedUsers.Select(ru => new TaskAnnotatorViewModel
                    {
                        AnnotatorId = ru.Annotator.Id,
                        AnnotatorName = ru.Annotator.Name,
                        IsManager = ru.IsManager
                    }),
                    Workspace = new WorkspaceViewModel
                    {
                        Id = x.Workspace.Id,
                        Name = x.Workspace.Name,
                        Type = new WorkspaceTypeViewModel
                        {
                            Id = x.Workspace.Type.Id,
                            Name = x.Workspace.Type.Name
                        }
                    }
                })
            };
        }
    }
}