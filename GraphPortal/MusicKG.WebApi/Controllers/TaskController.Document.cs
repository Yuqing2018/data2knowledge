using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicKG.WebApi.Filters;
using MusicKG.DataAccess.Enums;
using MusicKG.WebApi.Helpers;
using MusicKG.WebApi.Contract.ViewModels;
using MusicKG.WebApi.Contract.Attributes;
using MusicKG.Service.Constants;

namespace MusicKG.WebApi.Controllers
{
    public partial class TaskController : ControllerBase
    {
        /// <summary>
        /// Get task documents.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="taskId">Task ID.</param>
        /// <param name="from">Pagination start index.</param>
        /// <param name="size">Pagination size.</param>
        /// <returns>Task document object list.</returns>
        [HttpGet]
        [Route("{taskId}/Document")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PaginationViewModel<TaskDocumentViewModel>), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager, UserRoleEnum.Annotator, UserRoleEnum.ReadOnly, UserRoleEnum.Inspector })]
        public async Task<PaginationViewModel<TaskDocumentViewModel>> GetDocuments(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string taskId,
            [FromQuery] int from,
            [FromQuery] int? size)
        {
            var (currentUserId, _, currentUserRoles) = HttpContextHelper.GetCurrentUser(this.HttpContext);

            var task = await taskService.GetTaskAsync(workspaceId, taskId,
                (currentUserRoles.Contains(UserRoleEnum.ReadOnly) ||
                currentUserRoles.Contains(UserRoleEnum.Manager)) ? null : currentUserId);

            var taskDocumentsTotal = task.Annotators
                .SelectMany(a => a.TaskDocuments)
                .GroupBy(td => td.Document.Id)
                .OrderBy(g => g.Key)
                .ToList();

            var taskDocuments = taskDocumentsTotal
                .Skip(from)
                .Take(size ?? int.MaxValue)
                .Select(g =>
                {
                    var item = g.First();

                    return new TaskDocumentViewModel
                    {
                        Id = item.Document.Id,
                        Name = item.Document.Name,
                        Status = item.Status,
                        UploadedAt = item.Document.UploadedAt,
                        LatestResultSavedAt = g.Max(gi => gi.LatestResultSavedAt),
                        Annotators = g.Select(td => task.Annotators.Where(ta => ta.Annotator.Id.Equals(td.AnnotatorId)).First()).Select(ta => new UserViewModel
                        {
                            Id = ta.Annotator.Id,
                            Name = ta.Annotator.Name,
                            Roles = ta.Annotator.Roles
                        })
                    };
                })
                .ToList();

            return new PaginationViewModel<TaskDocumentViewModel>()
            {
                Count = taskDocuments.Count(),
                From = from,
                TotalCount = taskDocumentsTotal.Count(),
                Items = taskDocuments
            };
        }
    }
}