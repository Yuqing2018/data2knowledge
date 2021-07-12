using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using MusicKG.DataAccess.Models;

namespace MusicKG.DataAccess
{
    public interface IMusicKGContext
    {
        IMongoClient Client { get; }

        IMongoDatabase Database { get; }

        IGridFSBucket Bucket { get; }

        IMongoCollection<UserDataModel> Users { get; }

        IMongoCollection<WorkspaceDataModel> Workspaces { get; }

        IMongoCollection<OptionDataModel> Options { get; }

        IMongoCollection<RuleDataModel> Rules { get; }
        
        IMongoCollection<DictionaryDataModel> Dictionaries { get; }

        IMongoCollection<DocumentDataModel> Documents { get; }

        IMongoCollection<TaskDataModel> Tasks { get; }

        IMongoCollection<WorkflowDataModel> Workflows { get; }

        IMongoCollection<WorkspaceTypeDataModel> WorkspaceTypes { get; }

        IMongoCollection<OntologyEntityDataModel> OntologyEntities { get; }

        IMongoCollection<OntologyEntityPropertyTypeDataModel> EntityPropertyTypes { get; }

        IMongoCollection<OntologyRelationDataModel> OntologyRelations { get; }

        IMongoCollection<DialogOntologyDataModel> DialogOntologies { get; }

        IMongoCollection<TaskTypeDataModel> TaskTypes { get; }

        IMongoCollection<TagDataModel> Tags { get; set; }

        IMongoCollection<AutoTaskCreationRulesDataModel> AutoTaskCreationRules { get; set; }

        IMongoCollection<CategoryDataModel> Categories { get; set; }
    }
}
