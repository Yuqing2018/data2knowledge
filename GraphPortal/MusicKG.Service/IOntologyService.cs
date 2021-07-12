using MusicKG.DataAccess.Models;
using MusicKG.Service.Enums;
using MusicKG.Service.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicKG.Service
{
    public interface IOntologyService
    {
        #region Ontology Entity

        Task<OntologyEntityServiceModel> GetOntologyEntityAsync(string workspaceId, string entityId);

        Task<Tuple<long, IEnumerable<OntologyEntityListItemServiceModel>>> GetOntologyEntitiesAsync(string workspaceId, int from, int? size);

        Task<OntologyEntityServiceModel> CreateOntologyEntityAsync(OntologyEntityCreateServiceModel serviceModel);

        Task<OntologyEntityServiceModel> UpdateOntologyEntityAsync(string workspaceId, string entityId, OntologyEntityUpdateServiceModel serviceModel);

        Task DeleteOntologyEntityAsync(string workspaceId, string entityId);

        #endregion

        #region Ontology Relation

        Task<OntologyRelationServiceModel> GetOntologyRelationAsync(string workspaceId, string RelationId);

        Task<Tuple<long, IEnumerable<OntologyRelationListItemServiceModel>>> GetOntologyRelationsAsync(string workspaceId, int from, int? size);

        Task<OntologyRelationServiceModel> CreateOntologyRelationAsync(OntologyRelationCreateServiceModel serviceModel);

        Task<OntologyRelationServiceModel> UpdateOntologyRelationAsync(string workspaceId, string RelationId, OntologyRelationUpdateServiceModel serviceModel);

        Task DeleteOntologyRelationAsync(string workspaceId, string RelationId);

        #endregion

        Task<OntologyUploadServiceModel> UploadOntologyAsync(string workspaceId,OntologyUploadServiceModel serviceModel);

        Task<byte[]> DownloadOntologyAsync(string workspaceId, OntologyDownloadFileTypeEnum type);
    }
}
