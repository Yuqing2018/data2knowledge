using MongoDB.Bson;
using MongoDB.Driver;
using MusicKG.DataAccess;
using MusicKG.DataAccess.Models;
using MusicKG.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MusicKG.Service.Helpers;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using MusicKG.Service.Enums;
using MusicKG.Service.Extensions;
using MusicKG.Service.Resources;

namespace MusicKG.Service.Implementations
{
    public class OntologyService : IOntologyService
    {
        private readonly IMusicKGContext context;
        private readonly ILogger<OntologyService> logger;
        private readonly Func<OntologyDownloadFileTypeEnum, IOntologyExportProvider> exporterFactory;

        /// <summary>
        /// Ontology service constructor.
        /// </summary>
        /// <param name="context">MusicKG mongodb context.</param>
        public OntologyService(
            IMusicKGContext context, ILogger<OntologyService> logger,
            Func<OntologyDownloadFileTypeEnum, IOntologyExportProvider> exporterFactory)
        {
            this.context = context;
            this.logger = logger;
            this.exporterFactory = exporterFactory;
        }

        #region Ontology Entity

        /// <summary>
        /// Get OntologyEntity by ontology entity ID.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="id">Ontology entity ID.</param>
        /// <returns>Ontology entity service object.</returns>
        public async Task<OntologyEntityServiceModel> GetOntologyEntityAsync(string workspaceId, string id)
        {
            var ontologyEntity = await context.OntologyEntities.Find(w => w.WorkspaceId == new ObjectId(workspaceId) && w.Id == new ObjectId(id)).FirstOrDefaultAsync();

            if (ontologyEntity == null)
                ErrorHelper.ThrowException(MusicKGMessages.OntologyEntityNotExistMessage, HttpStatusCode.BadRequest);

            return OntologyEntityDataModelToServiceModel(ontologyEntity);
        }

        /// <summary>
        /// Update ontology entity.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="OntologyEntityId">Ontology entity ID.</param>
        /// <param name="serviceModel">Service object.</param>
        /// <returns>OntologyEntity service object.</returns>
        public async Task<OntologyEntityServiceModel> UpdateOntologyEntityAsync(string workspaceId, string entityId, OntologyEntityUpdateServiceModel serviceModel)
        {
            var checkedResult = await serviceModel.Check(workspaceId, entityId, context);

            if (!checkedResult.Item1)
                ErrorHelper.ThrowException(checkedResult.Item2, HttpStatusCode.BadRequest);

            var update = Builders<OntologyEntityDataModel>.Update.Set(u => u.Id, new ObjectId(entityId));

            if (serviceModel.IsNameAssigned)
                update = update.Set(u => u.Name, serviceModel.Name);

            if (serviceModel.IsDescriptionAssigned)
                update = update.Set(u => u.Description, serviceModel.Description);

            if (serviceModel.IsColorAssigned)
                update = update.Set(u => u.Color, serviceModel.Color);

            if (serviceModel.IsPropertiesAssigned)
            {
                if (serviceModel.Properties != null && serviceModel.Properties.Count > 0)
                {
                    update = update.Set(u => u.Properties, serviceModel.Properties.Select(w => new OntologyEntityPropertyDataModel
                    {
                        Name = w.Name,
                        Description = w.Description,
                        Type = new ObjectId(w.Type)
                    }));
                }
                else
                {
                    update = update.Unset(u => u.Properties);
                }
            }

            OntologyEntityDataModel entity = null;

            try
            {
                entity = await context.OntologyEntities.FindOneAndUpdateAsync<OntologyEntityDataModel>(u => u.Id == new ObjectId(entityId) && u.WorkspaceId == new ObjectId(workspaceId), update,
                new FindOneAndUpdateOptions<OntologyEntityDataModel>
                {
                    ReturnDocument = ReturnDocument.After
                });
            }
            catch (MongoCommandException e)
            {
                var message = MusicKGMessages.OntologyEntityUpdateFailedMessage;

                if ("DuplicateKey".Equals(e.CodeName))
                    message = MusicKGMessages.OntologyEntityNameExistMessage;

                logger?.LogError(e, message);
                ErrorHelper.ThrowException(message, HttpStatusCode.BadRequest);
            }

            return OntologyEntityDataModelToServiceModel(entity);
        }

        /// <summary>
        /// Create a ontology entity.
        /// </summary>
        /// <param name="serviceModel">Ontology entity create service object.</param>
        /// <returns>Ontology entity service object.</returns>
        public async Task<OntologyEntityServiceModel> CreateOntologyEntityAsync(OntologyEntityCreateServiceModel serviceModel)
        {
            var entity = new OntologyEntityDataModel
            {
                WorkspaceId = new ObjectId(serviceModel.WorkspaceId),
                Name = serviceModel.Name,
                Description = serviceModel.Description,
                Properties = new List<OntologyEntityPropertyDataModel>(),
                Color = serviceModel.Color,
            };

            try
            {
                await context.OntologyEntities.InsertOneAsync(entity);
            }
            catch (MongoWriteException e)
            {
                var message = MusicKGMessages.OntologyEntityCreateFailedMessage;

                if (ServerErrorCategory.DuplicateKey == e.WriteError?.Category)
                    message = MusicKGMessages.OntologyEntityNameExistMessage;

                logger?.LogError(e, message);
                ErrorHelper.ThrowException(message, HttpStatusCode.BadRequest);
            }

            return OntologyEntityDataModelToServiceModel(entity);
        }

        /// <summary>
        /// Get ontology entities by a workspace id.
        /// </summary>
        /// <param name="workspaceId">Workspace id.</param>
        /// <param name="from"></param>
        /// <param name="size">The size you want to get.</param>
        /// <returns>Tuple object</returns>
        public async Task<Tuple<long, IEnumerable<OntologyEntityListItemServiceModel>>> GetOntologyEntitiesAsync(string workspaceId, int from, int? size)
        {
            var builder = Builders<OntologyEntityDataModel>.Filter;

            var filter = builder.Eq(u => u.WorkspaceId, new ObjectId(workspaceId));

            var entitiesFind = context.OntologyEntities.Find(filter);

            var totalCount = await entitiesFind.CountDocumentsAsync();

            if (from > 0)
                entitiesFind = entitiesFind.Skip(from);

            if (size.HasValue)
                entitiesFind = entitiesFind.Limit(size.Value);


            var entities = await entitiesFind.ToListAsync();

            var serviceModels = entities.Select(u => new OntologyEntityListItemServiceModel
            {
                Id = u.Id.ToString(),
                WorkspaceId = u.WorkspaceId.ToString(),
                Name = u.Name,
                Color = u.Color
            });

            return new Tuple<long, IEnumerable<OntologyEntityListItemServiceModel>>(totalCount, serviceModels);
        }

        /// <summary>
        /// Delete ontology entity.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="OntologyEntityId">Ontology entity ID.</param>
        /// <returns>null</returns>
        public async Task DeleteOntologyEntityAsync(string workspaceId, string entityId)
        {
            var builder = Builders<OntologyRelationDataModel>.Filter;

            var filter = builder.Eq(u => u.WorkspaceId, new ObjectId(workspaceId))
                & (builder.Eq(u => u.FirstEntityId, new ObjectId(entityId))
                | builder.Eq(u => u.SecondEntityId, new ObjectId(entityId)));

            var deleteRelationsResult = await context.OntologyRelations.DeleteManyAsync(filter);

            var result = await context.OntologyEntities.DeleteOneAsync(u => u.WorkspaceId == new ObjectId(workspaceId) && u.Id == new ObjectId(entityId));

            if (result.DeletedCount == 0)
                ErrorHelper.ThrowException(MusicKGMessages.OntologyEntityNotExistMessage, HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Convert ontology entity data model to service model.
        /// </summary>
        /// <param name="dataModel">Data object.</param>
        /// <returns>Ontology entity service object.</returns>
        private OntologyEntityServiceModel OntologyEntityDataModelToServiceModel(OntologyEntityDataModel dataModel)
        {
            return new OntologyEntityServiceModel
            {
                Id = dataModel.Id.ToString(),
                WorkspaceId = dataModel.WorkspaceId.ToString(),
                Name = dataModel.Name,
                Properties = dataModel.Properties?.Select(x => new OntologyEntityPropertyServiceModel()
                {
                    Name = x.Name,
                    Description = x.Description,
                    Type = x.Type.ToString()
                }).ToList() ?? new List<OntologyEntityPropertyServiceModel>(),
                Description = dataModel.Description,
                Color = dataModel.Color
            };
        }

        #endregion

        #region Ontology Relation

        /// <summary>
        /// Get OntologyRelation by ontology Relation ID.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="id">Ontology Relation ID.</param>
        /// <returns>Ontology Relation service object.</returns>
        public async Task<OntologyRelationServiceModel> GetOntologyRelationAsync(string workspaceId, string id)
        {
            var ontologyRelation = await context.OntologyRelations.Find(w => w.WorkspaceId == new ObjectId(workspaceId) && w.Id == new ObjectId(id)).FirstOrDefaultAsync();

            if (ontologyRelation == null)
                ErrorHelper.ThrowException(MusicKGMessages.OntologyRelationNotExistMessage, HttpStatusCode.BadRequest);

            return OntologyRelationDataModelToServiceModel(ontologyRelation);
        }

        /// <summary>
        /// Update ontology relation.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="OntologyRelationId">Ontology relation ID.</param>
        /// <param name="serviceModel">Service object.</param>
        /// <returns>OntologyRelation service object.</returns>
        public async Task<OntologyRelationServiceModel> UpdateOntologyRelationAsync(string workspaceId, string relationId, OntologyRelationUpdateServiceModel serviceModel)
        {
            var checkedResult = await serviceModel.Check(workspaceId, relationId, context);

            if (!checkedResult.Item1)
                ErrorHelper.ThrowException(checkedResult.Item2, HttpStatusCode.BadRequest);

            var update = Builders<OntologyRelationDataModel>.Update.Set(u => u.Id, new ObjectId(relationId));

            if (serviceModel.IsNameAssigned)
                update = update.Set(u => u.Name, serviceModel.Name);

            if (serviceModel.IsDescriptionAssigned)
                update = update.Set(u => u.Description, serviceModel.Description);

            if (serviceModel.IsSecondEntityAssigned)
                update = update.Set(u => u.SecondEntityId, new ObjectId(serviceModel.SecondEntityId));

            if (serviceModel.IsPropertiesAssigned)
            {
                if (serviceModel.Properties != null && serviceModel.Properties.Count > 0)
                {
                    update = update.Set(u => u.Properties, serviceModel.Properties.Select(w => new OntologyRelationPropertyDataModel
                    {
                        Name = w.Name,
                        Description = w.Description,
                        Type = new ObjectId(w.Type)
                    }));
                }
                else
                {
                    update = update.Unset(u => u.Properties);
                }
            }

            OntologyRelationDataModel relation = null;

            try
            {
                relation = await context.OntologyRelations.FindOneAndUpdateAsync<OntologyRelationDataModel>(u => u.Id == new ObjectId(relationId) && u.WorkspaceId == new ObjectId(workspaceId), update,
                new FindOneAndUpdateOptions<OntologyRelationDataModel>
                {
                    ReturnDocument = ReturnDocument.After
                });
            }
            catch (MongoCommandException e)
            {
                var message = MusicKGMessages.OntologyRelationUpdateFailedMessage;

                if ("DuplicateKey".Equals(e.CodeName))
                    message = MusicKGMessages.OntologyRelationNameExistMessage;

                logger?.LogError(e, message);
                ErrorHelper.ThrowException(message, HttpStatusCode.BadRequest);
            }

            return OntologyRelationDataModelToServiceModel(relation);
        }

        /// <summary>
        /// Create a ontology relation.
        /// </summary>
        /// <param name="serviceModel">Ontology relation create service object.</param>
        /// <returns>Ontology relation service object.</returns>
        public async Task<OntologyRelationServiceModel> CreateOntologyRelationAsync(OntologyRelationCreateServiceModel serviceModel)
        {
            var checkedResult = await serviceModel.Check(serviceModel.WorkspaceId, context);

            if (!checkedResult.Item1)
                ErrorHelper.ThrowException(checkedResult.Item2, HttpStatusCode.BadRequest);

            var relation = new OntologyRelationDataModel
            {
                WorkspaceId = new ObjectId(serviceModel.WorkspaceId),
                Name = serviceModel.Name,
                FirstEntityId = new ObjectId(serviceModel.FirstEntityId),
                SecondEntityId = new ObjectId(serviceModel.SecondEntityId),
                Properties = new List<OntologyRelationPropertyDataModel>(),
                Description = serviceModel.Description,
            };

            try
            {
                await context.OntologyRelations.InsertOneAsync(relation);
            }
            catch (MongoWriteException e)
            {
                var message = MusicKGMessages.OntologyRelationCreateFailedMessage;

                if (ServerErrorCategory.DuplicateKey == e.WriteError?.Category)
                    message = MusicKGMessages.OntologyRelationNameExistMessage;

                logger?.LogError(e, message);

                ErrorHelper.ThrowException(message, HttpStatusCode.BadRequest);
            }

            return OntologyRelationDataModelToServiceModel(relation);
        }

        /// <summary>
        /// Get ontology relations by a workspace id.
        /// </summary>
        /// <param name="workspaceId">Workspace id.</param>
        /// <param name="from"></param>
        /// <param name="size">The size you want to get.</param>
        /// <returns>Tuple object</returns>
        public async Task<Tuple<long, IEnumerable<OntologyRelationListItemServiceModel>>> GetOntologyRelationsAsync(string workspaceId, int from, int? size)
        {
            var builder = Builders<OntologyRelationDataModel>.Filter;

            var filter = builder.Eq(u => u.WorkspaceId, new ObjectId(workspaceId));

            var relationsFind = context.OntologyRelations.Find(filter);

            var totalCount = await relationsFind.CountDocumentsAsync();

            if (from > 0)
                relationsFind = relationsFind.Skip(from);

            if (size.HasValue)
                relationsFind = relationsFind.Limit(size.Value);

            var relations = await relationsFind.ToListAsync();

            var serviceModels = new List<OntologyRelationListItemServiceModel>();

            foreach (var relation in relations)
            {
                serviceModels.Add(new OntologyRelationListItemServiceModel
                {
                    Id = relation.Id.ToString(),
                    WorkspaceId = relation.WorkspaceId.ToString(),
                    Name = relation.Name,
                    FirstEntityId = relation.FirstEntityId.ToString(),
                    FirstEntityName = (await this.GetOntologyEntityAsync(relation.WorkspaceId.ToString(), relation.FirstEntityId.ToString())).Name,
                    SecondEntityId = relation.SecondEntityId.ToString(),
                    SecondEntityName = (await this.GetOntologyEntityAsync(relation.WorkspaceId.ToString(), relation.SecondEntityId.ToString())).Name,
                    Description = relation.Description
                });
            }

            return new Tuple<long, IEnumerable<OntologyRelationListItemServiceModel>>(totalCount, serviceModels);
        }

        /// <summary>
        /// Delete ontology relation.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="OntologyRelationId">Ontology relation ID.</param>
        /// <returns>null</returns>
        public async Task DeleteOntologyRelationAsync(string workspaceId, string RelationId)
        {
            var result = await context.OntologyRelations.DeleteOneAsync(u => u.WorkspaceId == new ObjectId(workspaceId) && u.Id == new ObjectId(RelationId));

            if (result.DeletedCount == 0)
                ErrorHelper.ThrowException(MusicKGMessages.OntologyRelationNotExistMessage, HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Convert ontology relation data model to service model.
        /// </summary>
        /// <param name="dataModel">Data object.</param>
        /// <returns>Ontology Relation service object.</returns>
        private OntologyRelationServiceModel OntologyRelationDataModelToServiceModel(OntologyRelationDataModel dataModel)
        {
            return new OntologyRelationServiceModel
            {
                Id = dataModel.Id.ToString(),
                WorkspaceId = dataModel.WorkspaceId.ToString(),
                Name = dataModel.Name,
                FirstEntityId = dataModel.FirstEntityId.ToString(),
                SecondEntityId = dataModel.SecondEntityId.ToString(),
                Properties = dataModel.Properties?.Select(x => new OntologyRelationPropertyServiceModel()
                {
                    Name = x.Name,
                    Description = x.Description,
                    Type = x.Type.ToString()
                }).ToList() ?? new List<OntologyRelationPropertyServiceModel>(),
                Description = dataModel.Description,
            };
        }
        #endregion

        #region Ontology Upload & Download

        public async Task<OntologyUploadServiceModel> UploadOntologyAsync(string workspaceId, OntologyUploadServiceModel serviceModel)
        {
            var insertEntities = new List<OntologyEntityDataModel>();
            var insertRelations = new List<OntologyRelationDataModel>();

            if (null != serviceModel.Entities)
            {
                var checkedentitiesResult = serviceModel.CheckUploadEntities(workspaceId, context);

                if (!checkedentitiesResult.Item1)
                    ErrorHelper.ThrowException(checkedentitiesResult.Item2, HttpStatusCode.BadRequest);

                foreach (var item in serviceModel.Entities)
                {
                    var entity = new OntologyEntityDataModel()
                    {
                        WorkspaceId = new ObjectId(workspaceId),
                        Name = item.Name,
                        Properties = new List<OntologyEntityPropertyDataModel>(),
                        Description = item.Description,
                        Color = item.Color
                    };

                    if (null != item.Properties)
                    {
                        foreach (var p in item.Properties)
                        {
                            entity.Properties.Add(
                                new OntologyEntityPropertyDataModel()
                                {
                                    Name = p.Name,
                                    Description = p.Description,
                                    Type = (await this.context.EntityPropertyTypes.Find(t => t.Name == p.Type).FirstOrDefaultAsync()).Id,
                                });
                        }
                    }

                    insertEntities.Add(entity);
                }

                if (null != insertEntities && insertEntities.Count > 0)
                {
                    try
                    {
                        await context.OntologyEntities.InsertManyAsync(insertEntities);
                    }
                    catch (MongoWriteException e)
                    {
                        var message = MusicKGMessages.OntologyEntityCreateFailedMessage;

                        if (ServerErrorCategory.DuplicateKey == e.WriteError?.Category)
                            message = MusicKGMessages.OntologyEntityNameExistMessage;

                        logger?.LogError(e, message);
                        ErrorHelper.ThrowException(message, HttpStatusCode.BadRequest);
                    }
                }
            }

            if (null != serviceModel.Relations)
            {
                var checkedRelationsResult = await serviceModel.CheckUploadRelationss(workspaceId, context);

                if (!checkedRelationsResult.Item1)
                    ErrorHelper.ThrowException(checkedRelationsResult.Item2, HttpStatusCode.BadRequest);

                foreach (var item in serviceModel.Relations)
                {
                    var relation = new OntologyRelationDataModel()
                    {
                        WorkspaceId = new ObjectId(workspaceId),
                        Name = item.Name,
                        FirstEntityId = (await context.OntologyEntities.Find(x => x.WorkspaceId == new ObjectId(workspaceId) && x.Name == item.FirstEntityName).FirstOrDefaultAsync()).Id,
                        SecondEntityId = (await context.OntologyEntities.Find(x => x.WorkspaceId == new ObjectId(workspaceId) && x.Name == item.SecondEntityName).FirstOrDefaultAsync()).Id,
                        Properties = new List<OntologyRelationPropertyDataModel>(),
                        Description = item.Description,
                    };

                    if (null != item.Properties)
                    {
                        foreach (var p in item.Properties)
                        {
                            relation.Properties.Add(
                                new OntologyRelationPropertyDataModel()
                                {
                                    Name = p.Name,
                                    Description = p.Description,
                                    Type = (await context.EntityPropertyTypes.Find(x => x.Name == p.Type).FirstOrDefaultAsync()).Id,
                                });
                        }
                    }

                    insertRelations.Add(relation);
                }

                if (null != insertRelations && insertRelations.Count > 0)
                {
                    try
                    {
                        await context.OntologyRelations.InsertManyAsync(insertRelations);
                    }
                    catch (MongoWriteException e)
                    {
                        var message = MusicKGMessages.OntologyRelationCreateFailedMessage;

                        if (ServerErrorCategory.DuplicateKey == e.WriteError?.Category)
                            message = MusicKGMessages.OntologyRelationNameExistMessage;

                        logger?.LogError(e, message);

                        ErrorHelper.ThrowException(message, HttpStatusCode.BadRequest);
                    }
                }
            }

            return new OntologyUploadServiceModel()
            {
                Entities = insertEntities?.Select(x => OntologyEntityDataModelToServiceModel(x)).ToList(),
                Relations = insertRelations?.Select(x => OntologyRelatonDataModelToDownloadServiceModel(x)).ToList(),
            };
        }

        public async Task<byte[]> DownloadOntologyAsync(string workspaceId, OntologyDownloadFileTypeEnum type = OntologyDownloadFileTypeEnum.Json)
        {
            var entityBuilder = Builders<OntologyEntityDataModel>.Filter;

            var entityFilter = String.IsNullOrEmpty(workspaceId) ? null : entityBuilder.Eq(u => u.WorkspaceId, new ObjectId(workspaceId));

            var entitiesFind = context.OntologyEntities.Find(entityFilter);

            var entities = await entitiesFind.ToListAsync();

            var relationBuilder = Builders<OntologyRelationDataModel>.Filter;

            var relationFilter = String.IsNullOrEmpty(workspaceId) ? null : relationBuilder.Eq(u => u.WorkspaceId, new ObjectId(workspaceId));

            var relationsFind = context.OntologyRelations.Find(relationFilter);

            var relations = await relationsFind.ToListAsync();

            var serviceModel = new OntologyDownloadServiceModel()
            {
                Entities = entities?.Select(x => OntologyEntityDataModelToDownloadServiceModel(x)).ToList(),
                Relations = relations?.Select(x => OntologyRelatonDataModelToDownloadServiceModel(x)).ToList(),
            };

            return await this.exporterFactory(type).ExportAsync(serviceModel);
        }

        private OntologyRelationDownloadServiceModel OntologyRelatonDataModelToDownloadServiceModel(OntologyRelationDataModel dataModel)
        {
            return new OntologyRelationDownloadServiceModel
            {
                Id = dataModel.Id.ToString(),
                WorkspaceId = dataModel.WorkspaceId.ToString(),
                Name = dataModel.Name,
                FirstEntityName = this.context.OntologyEntities.Find(u => u.Id == dataModel.FirstEntityId).First().Name,
                SecondEntityName = this.context.OntologyEntities.Find(u => u.Id == dataModel.SecondEntityId).First().Name,
                Properties = dataModel.Properties?.Select(x => new OntologyRelationPropertyServiceModel()
                {
                    Name = x.Name,
                    Description = x.Description,
                    Type = this.context.EntityPropertyTypes.Find(u => u.Id == x.Type).First().Name,
                }).ToList() ?? new List<OntologyRelationPropertyServiceModel>(),
                Description = dataModel.Description,
            };
        }

        private OntologyEntityServiceModel OntologyEntityDataModelToDownloadServiceModel(OntologyEntityDataModel dataModel)
        {
            return new OntologyEntityServiceModel
            {
                Id = dataModel.Id.ToString(),
                WorkspaceId = dataModel.WorkspaceId.ToString(),
                Name = dataModel.Name,
                Properties = dataModel.Properties?.Select(x => new OntologyEntityPropertyServiceModel()
                {
                    Name = x.Name,
                    Description = x.Description,
                    Type = this.context.EntityPropertyTypes.Find(u => u.Id == x.Type).First().Name,
                }).ToList() ?? new List<OntologyEntityPropertyServiceModel>(),
                Description = dataModel.Description,
                Color = dataModel.Color
            };
        }

        #endregion
    }
}
