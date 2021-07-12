using MusicKG.DataAccess;
using MusicKG.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using System.Threading.Tasks;
using MongoDB.Bson;
using System.Linq;
using System.Text.RegularExpressions;
using MusicKG.DataAccess.Models;
using MusicKG.Service.Resources;
using MusicKG.Service.Constants;

namespace MusicKG.Service.Extensions
{
    public static class OntologyCheckerExtensions
    {
        #region Ontology Entity

        public static async Task<Tuple<bool, string>> Check(this OntologyEntityUpdateServiceModel serviceModel, string workspaceId, string entityId, IMusicKGContext context)
        {
            var ontologyEntity = await context.OntologyEntities.Find(w => w.WorkspaceId == new ObjectId(workspaceId) && w.Id == new ObjectId(entityId)).FirstOrDefaultAsync();

            if (ontologyEntity == null)
                return new Tuple<bool, string>(false, MusicKGMessages.OntologyEntityNotExistMessage);

            return new Tuple<bool, string>(true, string.Empty);
        }

        #endregion

        #region Ontology Relation

        public static async Task<Tuple<bool, string>> Check(this OntologyRelationCreateServiceModel serviceModel, string workspaceId, IMusicKGContext context)
        {
            if (ObjectId.TryParse(serviceModel.FirstEntityId, out ObjectId firstEntityId))
            {
                if (!(await CheckEntityInRelationExistAsync(workspaceId, firstEntityId, context)))
                {
                    return new Tuple<bool, string>(false, MusicKGMessages.OntologyRelationFirstEntityNotExistMessage);
                }
            }
            else
            {
                return new Tuple<bool, string>(false, MusicKGMessages.OntologyRelationFirstEntityIdWrongMessage);
            }

            if (ObjectId.TryParse(serviceModel.SecondEntityId, out ObjectId secondEntityId))
            {
                if (!(await CheckEntityInRelationExistAsync(workspaceId, secondEntityId, context)))
                {
                    return new Tuple<bool, string>(false, MusicKGMessages.OntologyRelationSecondEntityNotExistMessage);
                }
            }
            else
            {
                return new Tuple<bool, string>(false, MusicKGMessages.OntologyRelationSecondEntityIdWrongMessage);
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        public static async Task<Tuple<bool, string>> Check(this OntologyRelationUpdateServiceModel serviceModel, string workspaceId, string relationId, IMusicKGContext context)
        {
            var ontologyRelation = await context.OntologyRelations.Find(w => w.WorkspaceId == new ObjectId(workspaceId) && w.Id == new ObjectId(relationId)).FirstOrDefaultAsync();

            if (ontologyRelation == null)
                return new Tuple<bool, string>(false, MusicKGMessages.OntologyRelationNotExistMessage);

            if (serviceModel.IsSecondEntityAssigned)
            {
                if (ObjectId.TryParse(serviceModel.SecondEntityId, out ObjectId secondEntityId))
                {
                    if (!(await CheckEntityInRelationExistAsync(workspaceId, secondEntityId, context)))
                    {
                        return new Tuple<bool, string>(false, MusicKGMessages.OntologyRelationSecondEntityNotExistMessage);
                    }
                }
                else
                {
                    return new Tuple<bool, string>(false, MusicKGMessages.OntologyRelationSecondEntityIdWrongMessage);
                }
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        #endregion

        #region Ontology Upload

        public static Tuple<bool, string> CheckUploadEntities(this OntologyUploadServiceModel serviceModel, string workspaceId, IMusicKGContext context)
        {
            if (null != serviceModel.Entities)
            {
                var uploadEntityNames = serviceModel.Entities.Select(e => e.Name.ToLower());

                //上传文件中 实体名称重复
                if (uploadEntityNames.Distinct().Count() != uploadEntityNames.Count())
                {
                    return new Tuple<bool, string>(false, MusicKGMessages.OntologyUploadDuplicateEntityNameMessage);
                }

                //上传文件中 循环检查每个实体
                foreach (var item in serviceModel.Entities)
                {
                    if (string.IsNullOrEmpty(item.Name))
                    {
                        return new Tuple<bool, string>(false, MusicKGMessages.OntologyEntityNameEmptyMessage);
                    }

                    if (null != item.Properties)
                    {
                        var propertyNames = item.Properties.Select(x => x.Name.ToLower());

                        if (propertyNames.Distinct().Count() != item.Properties.Count)
                        {
                            return new Tuple<bool, string>(false, MusicKGMessages.OntologyEntityDuplicatePropertyNameMessage);
                        }

                        foreach (var propertyName in propertyNames)
                        {
                            if (!Regex.IsMatch(propertyName, OntologyConstant.PropertyAndRelationNamePattern))
                            {
                                return new Tuple<bool, string>(false, MusicKGMessages.OntologyEntityPropertyNameWrongMessage);
                            }
                        }
                    }
                }

                var builder = Builders<OntologyEntityDataModel>.Filter;

                //上传文件中 存在实体名称已在库中
                var nonInFilter = builder.Eq(x => x.WorkspaceId, new ObjectId(workspaceId))
                    & builder.In(x => x.Name, uploadEntityNames);

                var exists = context.OntologyEntities.Find(nonInFilter);

                if (exists.CountDocuments() > 0)
                {
                    return new Tuple<bool, string>(false, MusicKGMessages.OntologyEntityNameExistMessage);
                }
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        public async static Task<Tuple<bool, string>> CheckUploadRelationss(this OntologyUploadServiceModel serviceModel, string workspaceId, IMusicKGContext context)
        {
            if (null != serviceModel.Relations)
            {
                var uploadRelationNames = serviceModel.Relations.Select(e => e.Name.ToLower());

                //上传文件中 关系名称重复
                if (uploadRelationNames.Distinct().Count() != uploadRelationNames.Count())
                {
                    return new Tuple<bool, string>(false, MusicKGMessages.OntologyUploadDuplicateRelationNameMessage);
                }

                var builder = Builders<OntologyRelationDataModel>.Filter;

                //上传文件中 存在关系名称已在库中
                var nonInFilter = builder.Eq(x => x.WorkspaceId, new ObjectId(workspaceId))
                    & builder.In(x => x.Name, uploadRelationNames);

                var exists = context.OntologyRelations.Find(nonInFilter);

                if (exists.CountDocuments() > 0)
                {
                    return new Tuple<bool, string>(false, MusicKGMessages.OntologyRelationNameExistMessage);
                }

                //上传文件中 检查每个关系
                foreach (var item in serviceModel.Relations)
                {
                    if (string.IsNullOrEmpty(item.Name))
                    {
                        return new Tuple<bool, string>(false, MusicKGMessages.OntologyRelationNameEmptyMessage);
                    }

                    if (!Regex.IsMatch(item.Name, OntologyConstant.PropertyAndRelationNamePattern))
                    {
                        return new Tuple<bool, string>(false, MusicKGMessages.OntologyRelationNameWrongMessage);
                    }

                    if (!string.IsNullOrEmpty(item.FirstEntityName))
                    {
                        if (!(await CheckEntityInRelationExistAsync(workspaceId, item.FirstEntityName, context)))
                        {
                            return new Tuple<bool, string>(false, MusicKGMessages.OntologyRelationFirstEntityNotExistMessage);
                        }
                    }

                    if (!string.IsNullOrEmpty(item.SecondEntityName))
                    {
                        if (!(await CheckEntityInRelationExistAsync(workspaceId, item.SecondEntityName, context)))
                        {
                            return new Tuple<bool, string>(false, MusicKGMessages.OntologyRelationSecondEntityNotExistMessage);
                        }
                    }

                    if (null != item.Properties)
                    {
                        var propertyNames = item.Properties.Select(x => x.Name.ToLower());

                        if (propertyNames.Distinct().Count() != item.Properties.Count)
                        {
                            return new Tuple<bool, string>(false, MusicKGMessages.OntologyRelationDuplicatePropertyNameMessage);
                        }

                        foreach (var propertyName in propertyNames)
                        {
                            if (!Regex.IsMatch(propertyName, OntologyConstant.PropertyAndRelationNamePattern))
                            {
                                return new Tuple<bool, string>(false, MusicKGMessages.OntologyRelationPropertyNameWrongMessage);
                            }
                        }
                    }
                }
            }

            return new Tuple<bool, string>(true, string.Empty);
        }
        #endregion

        private static async Task<bool> CheckEntityInRelationExistAsync(string workspaceId, ObjectId entityId, IMusicKGContext context)
        {
            var count = await context.OntologyEntities.CountDocumentsAsync(x => x.WorkspaceId == new ObjectId(workspaceId) && x.Id == entityId);

            if (0 == count)
            {
                return false;
            }

            return true;
        }

        private static async Task<bool> CheckEntityInRelationExistAsync(string workspaceId, string entityName, IMusicKGContext context)
        {
            var count = await context.OntologyEntities.CountDocumentsAsync(x => x.WorkspaceId == new ObjectId(workspaceId) && x.Name == entityName);

            if (0 == count)
            {
                return false;
            }

            return true;
        }

    }
}
