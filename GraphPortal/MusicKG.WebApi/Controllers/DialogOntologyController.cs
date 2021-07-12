using MusicKG.DataAccess.Enums;
using MusicKG.Service;
using MusicKG.Service.Constants;
using MusicKG.Service.Helpers;
using MusicKG.Service.Models;
using MusicKG.Service.Resources;
using MusicKG.WebApi.Filters;
using MusicKG.WebApi.Helpers;
using MusicKG.WebApi.Extensions;
using MusicKG.WebApi.Contract.ViewModels;
using MusicKG.WebApi.Contract.Attributes;
using MusicKG.WebApi.Contract.BindingModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace MusicKG.WebApi.Controllers
{
    /// <summary>
    /// Dialog ontology controller.
    /// </summary>
    [Route("api/Workspace/{workspaceId}/[controller]")]
    [ApiController]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.InternalServerError)]
    public class DialogOntologyController : ControllerBase
    {
        private readonly IDialogOntologyService dialogOntologyService;
        private readonly IDocumentService documentService;

        /// <summary>
        /// Dialog ontology controller constructor.
        /// </summary>
        public DialogOntologyController(IDialogOntologyService dialogOntologyService, IDocumentService documentService)
        {
            this.dialogOntologyService = dialogOntologyService;
            this.documentService = documentService;
        }

        /// <summary>
        /// Get dialog ontology object.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <returns>Dialog ontology object.</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DialogOntologyViewModel), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager, UserRoleEnum.Annotator, UserRoleEnum.ReadOnly })]
        public async Task<DialogOntologyViewModel> GetDialogOntology(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId)
        {
            var serviceModel = await dialogOntologyService.GetDialogOntologyAsync(workspaceId);

            return new DialogOntologyViewModel
            {
                EntityDocumentId = serviceModel?.DialogEntityDocumentId,
                IntentDocumentId = serviceModel?.DialogIntentDocumentId
            };
        }

        /// <summary>
        /// Upload dialog intent.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="intentFile">Intent file.</param>
        /// <param name="entityFile">Entity file.</param>
        /// <returns>Nothing.</returns>
        [HttpPost("Content")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task UploadOntology(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)]  string workspaceId,
            IFormFile entityFile,
            IFormFile intentFile
            )
        {
            var currentOntology = await dialogOntologyService.GetDialogOntologyAsync(workspaceId);

            if (currentOntology != null &&
                !string.IsNullOrWhiteSpace(currentOntology.DialogEntityDocumentId) &&
                entityFile != null)
            {
                ErrorHelper.ThrowException(MusicKGMessages.DialogOntologyEntityDuplicateMessage, HttpStatusCode.BadRequest);
            }

            if (currentOntology != null &&
                !string.IsNullOrWhiteSpace(currentOntology.DialogIntentDocumentId) &&
                intentFile != null)
            {
                ErrorHelper.ThrowException(MusicKGMessages.DialogOntologyIntentDuplicateMessage, HttpStatusCode.BadRequest);
            }

            var currentUser = HttpContextHelper.GetCurrentUser(HttpContext);

            string ontologEntityDocumentId = null, ontologyIntentDocumentId = null;

            if (entityFile != null)
            {
                var entityDocument = new DocumentUploadServiceModel
                {
                    Name = entityFile.Name,
                    ItemCount = 1,
                    Tags = new List<string> { entityFile.Name },
                    ParentDocumentId = null,
                    Content = entityFile.GetContent(),
                    ContentType = entityFile.ContentType,
                    UploadedBy = currentUser?.Item1
                };

                ontologEntityDocumentId = await documentService.UploadDocumentAsync(workspaceId, entityDocument);
            }

            if (intentFile != null)
            {
                var intentDocument = new DocumentUploadServiceModel
                {
                    Name = intentFile.Name,
                    ItemCount = 1,
                    Tags = new List<string> { intentFile.Name },
                    ParentDocumentId = null,
                    Content = intentFile.GetContent(),
                    ContentType = intentFile.ContentType,
                    UploadedBy = currentUser?.Item1
                };

                ontologyIntentDocumentId = await documentService.UploadDocumentAsync(workspaceId, intentDocument);
            }

            await dialogOntologyService.UploadDialogOntologyAsync(workspaceId, new DialogOntologyCreateServiceModel
            {
                CurrentId = currentOntology?.Id,
                DialogEntityDocumentId = ontologEntityDocumentId,
                DialogIntentDocumentId = ontologyIntentDocumentId
            });
        }
    }
}