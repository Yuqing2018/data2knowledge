using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MusicKG.DataAccess;
using MusicKG.Service.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Bson;
using MusicKG.DataAccess.Models;
using MusicKG.Service.Resources;
using System.Net;
using MusicKG.Service.Helpers;

namespace MusicKG.Service.Implementations
{
    public class DialogOntologyService : IDialogOntologyService
    {
        private readonly IMusicKGContext context;
        private readonly ILogger<DialogOntologyService> logger;

        /// <summary>
        /// DialogOntology service constructor.
        /// </summary>
        /// <param name="context">MusicKG mongodb context.</param>
        public DialogOntologyService(
            IMusicKGContext context,
            ILogger<DialogOntologyService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task UploadDialogOntologyAsync(string workspaceId, DialogOntologyCreateServiceModel serviceModel)
        {
            ObjectId? intentDocumentId, entityDocumentId;

            if (serviceModel.DialogEntityDocumentId == null)
            {
                entityDocumentId = null;
            }
            else
            {
                entityDocumentId = new ObjectId(serviceModel.DialogEntityDocumentId);
            }

            if (serviceModel.DialogIntentDocumentId == null)
            {
                intentDocumentId = null;
            }
            else
            {
                intentDocumentId = new ObjectId(serviceModel.DialogIntentDocumentId);
            }

            if (string.IsNullOrWhiteSpace(serviceModel.CurrentId))
            {
                var dataModel = new DialogOntologyDataModel
                {
                    Id = ObjectId.GenerateNewId(),
                    IntentDocumentId = intentDocumentId,
                    EntityDocumentId = entityDocumentId,
                    WorkspaceId = new ObjectId(workspaceId)
                };
                await context.DialogOntologies.InsertOneAsync(dataModel);
            }
            else
            {
                var currentId = new ObjectId(serviceModel.CurrentId);
                var builder = Builders<DialogOntologyDataModel>.Update;
                var update = builder.Set(d => d.Id, currentId);
                if (entityDocumentId != null)
                {
                    update = update.Set(d => d.EntityDocumentId, entityDocumentId);
                }
                if (intentDocumentId != null)
                {
                    update = update.Set(d => d.IntentDocumentId, intentDocumentId);
                }
                await context.DialogOntologies.UpdateOneAsync(d => d.Id == currentId, update);
            }
        }

        public async Task<DialogOntologyServiceModel> GetDialogOntologyAsync(string workspaceId)
        {
            var dataModel = await context.DialogOntologies.AsQueryable().Where(dialogOntology => dialogOntology.WorkspaceId == new ObjectId(workspaceId)).FirstOrDefaultAsync();

            return dataModel == null ? null : new DialogOntologyServiceModel
            {
                Id = dataModel.Id.ToString(),
                DialogEntityDocumentId = dataModel.EntityDocumentId?.ToString(),
                DialogIntentDocumentId = dataModel.IntentDocumentId?.ToString()
            };
        }

        public async Task<bool> DailogOntologyExistsAsync(string workspaceId)
        {
            var count = await context.DialogOntologies.CountDocumentsAsync(dialogOntology => dialogOntology.WorkspaceId == new ObjectId(workspaceId));

            return count > 0;
        }
    }
}

