using System;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using MongoDB.Driver.GridFS;
using MusicKG.DataAccess.Constants;
using MusicKG.DataAccess.Models;
using MusicKG.DataAccess.Settings;

namespace MusicKG.DataAccess
{
    public class MusicKGContext : IMusicKGContext
    {
        public const string DefaultBucketName = "fs";

        public IMongoClient Client { get; private set; }

        public IMongoDatabase Database { get; private set; }

        public IGridFSBucket Bucket { get; private set; }

        public MusicKGContext(IOptions<MongoDBSettings> options, ILogger<MusicKGContext> logger)
        {
            var settings = MongoClientSettings.FromConnectionString(options.Value.ConnectionString);

            settings.MaxConnectionIdleTime = TimeSpan.FromSeconds(30);

            settings.ClusterConfigurator = builder =>
            {
                // Enable TCP keep alive to relief MongoConnectionException.
                void SocketConfigurator(Socket s) => s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                builder.ConfigureTcp(tcp => tcp.With(socketConfigurator: (Action<Socket>)SocketConfigurator));

                // Log MongoDB statements.
                builder.Subscribe<CommandStartedEvent>(e =>
                {
                    logger?.LogDebug($"{e.CommandName} - {e.Command.ToJson()}");
                });
            };

            Setup(new MongoClient(settings), options.Value.Database, options.Value.Bucket, logger);
        }

        public MusicKGContext(
            IMongoClient client,
            string database,
            string bucket = null,
            ILogger<MusicKGContext> logger = null)
        {
            Setup(client, database, bucket, logger);
        }

        private void Setup(
            IMongoClient client,
            string database,
            string bucket = null,
            ILogger<MusicKGContext> logger = null)
        {
            Client = client;
            Database = Client.GetDatabase(database);
            Bucket = new GridFSBucket(Database, new GridFSBucketOptions
            {
                BucketName = bucket ?? DefaultBucketName
            });

            GetCollections();

            EnsureIndexes();
        }

        private void GetCollections()
        {
            Users = Database.GetCollection<UserDataModel>(CollectionNames.User);
            Workspaces = Database.GetCollection<WorkspaceDataModel>(CollectionNames.Workspace);
            Options = Database.GetCollection<OptionDataModel>(CollectionNames.Option);
            Rules = Database.GetCollection<RuleDataModel>(CollectionNames.Rule);
            Dictionaries = Database.GetCollection<DictionaryDataModel>(CollectionNames.Dictionary);
            Documents = Database.GetCollection<DocumentDataModel>(CollectionNames.Document);
            Tasks = Database.GetCollection<TaskDataModel>(CollectionNames.Task);
            Workflows = Database.GetCollection<WorkflowDataModel>(CollectionNames.Workflow);
            WorkspaceTypes = Database.GetCollection<WorkspaceTypeDataModel>(CollectionNames.WorkspaceType);
            OntologyEntities = Database.GetCollection<OntologyEntityDataModel>(CollectionNames.OntologyEntity);
            OntologyRelations = Database.GetCollection<OntologyRelationDataModel>(CollectionNames.OntologyRelation);
            EntityPropertyTypes = Database.GetCollection<OntologyEntityPropertyTypeDataModel>(CollectionNames.OntologyEntityPropertyType);
            DialogOntologies = Database.GetCollection<DialogOntologyDataModel>(CollectionNames.DialogOntology);
            TaskTypes = Database.GetCollection<TaskTypeDataModel>(CollectionNames.TaskType);
            Tags = Database.GetCollection<TagDataModel>(CollectionNames.Tags);
            AutoTaskCreationRules = Database.GetCollection<AutoTaskCreationRulesDataModel>(CollectionNames.AutoTaskCreationRules);
            Categories = Database.GetCollection<CategoryDataModel>(CollectionNames.Categories);
        }

        private void EnsureIndexes()
        {
            Users.Indexes.CreateOne(
                new CreateIndexModel<UserDataModel>(Builders<UserDataModel>.IndexKeys.Ascending(_ => _.Name),
                    new CreateIndexOptions { Background = true, Unique = true }));

            Workspaces.Indexes.CreateOne(
                new CreateIndexModel<WorkspaceDataModel>(Builders<WorkspaceDataModel>.IndexKeys.Ascending(_ => _.Name),
                    new CreateIndexOptions { Background = true, Unique = true }));

            Rules.Indexes.CreateOne(
                new CreateIndexModel<RuleDataModel>(Builders<RuleDataModel>.IndexKeys.Ascending(_ => _.WorkspaceId).Ascending(_ => _.Name),
                    new CreateIndexOptions { Background = true, Unique = true }));

            Dictionaries.Indexes.CreateOne(
                new CreateIndexModel<DictionaryDataModel>(Builders<DictionaryDataModel>.IndexKeys.Ascending(_ => _.WorkspaceId).Ascending(_ => _.Name), 
                    new CreateIndexOptions { Background = true, Unique = true }));

            Documents.Indexes.CreateOne(
                new CreateIndexModel<DocumentDataModel>(Builders<DocumentDataModel>.IndexKeys.Ascending(_ => _.WorkspaceId).Descending(_ => _.UploadedAt),
                    new CreateIndexOptions { Background = true, Unique = false }));

            Tasks.Indexes.CreateMany(new[]
            {
                new CreateIndexModel<TaskDataModel>(Builders<TaskDataModel>.IndexKeys.Ascending(_ => _.WorkspaceId).Ascending(_ => _.Name),
                    new CreateIndexOptions { Background = true, Unique = true }),
                new CreateIndexModel<TaskDataModel>(Builders<TaskDataModel>.IndexKeys.Ascending(_ => _.WorkspaceId).Descending(_ => _.CreatedAt),
                    new CreateIndexOptions { Background = true, Unique = false })
            });

            Workflows.Indexes.CreateMany(new[]
            {
                new CreateIndexModel<WorkflowDataModel>(Builders<WorkflowDataModel>.IndexKeys.Ascending(_ => _.Name),
                    new CreateIndexOptions { Background = true, Unique = true }),
                new CreateIndexModel<WorkflowDataModel>(Builders<WorkflowDataModel>.IndexKeys.Ascending(_ => _.Id).Ascending($"{nameof(WorkflowDataModel.Steps)}.{nameof(WorkflowStepDataModel.ProcessorClass)}").Ascending($"{nameof(WorkflowDataModel.Steps)}.{nameof(WorkflowStepDataModel.ProcessorAssembly)}"),
                    new CreateIndexOptions { Background = true, Unique = true }),
            });

            WorkspaceTypes.Indexes.CreateOne(
                new CreateIndexModel<WorkspaceTypeDataModel>(Builders<WorkspaceTypeDataModel>.IndexKeys.Ascending(_ => _.Name),
                    new CreateIndexOptions { Background = true, Unique = true }));

            OntologyEntities.Indexes.CreateMany(new[]
            {
                new CreateIndexModel<OntologyEntityDataModel>(Builders<OntologyEntityDataModel>.IndexKeys.Ascending(_ => _.WorkspaceId).Ascending(_ => _.Name),
                    new CreateIndexOptions { Background = true, Unique = true }),
                new CreateIndexModel<OntologyEntityDataModel>(Builders<OntologyEntityDataModel>.IndexKeys.Ascending(_ => _.Id).Ascending($"{nameof(OntologyEntityDataModel.Properties)}.{nameof(OntologyEntityPropertyDataModel.Name)}"),
                    new CreateIndexOptions { Background = true, Unique = true }),
            });

            OntologyRelations.Indexes.CreateMany(new[]
            {
                new CreateIndexModel<OntologyRelationDataModel>(Builders<OntologyRelationDataModel>.IndexKeys.Ascending(_ => _.WorkspaceId).Ascending(_ => _.Name),
                    new CreateIndexOptions { Background = true, Unique = true }),
                new CreateIndexModel<OntologyRelationDataModel>(Builders<OntologyRelationDataModel>.IndexKeys.Ascending(_ => _.Id).Ascending($"{nameof(OntologyRelationDataModel.Properties)}.{nameof(OntologyRelationPropertyDataModel.Name)}"),
                    new CreateIndexOptions { Background = true, Unique = true })
            });

            EntityPropertyTypes.Indexes.CreateOne(
                new CreateIndexModel<OntologyEntityPropertyTypeDataModel>(Builders<OntologyEntityPropertyTypeDataModel>.IndexKeys.Ascending(_ => _.Name),
                    new CreateIndexOptions { Background = true, Unique = true }));

            DialogOntologies.Indexes.CreateOne(
                new CreateIndexModel<DialogOntologyDataModel>(Builders<DialogOntologyDataModel>.IndexKeys.Ascending(_ => _.WorkspaceId),
                    new CreateIndexOptions { Background = true, Unique = true }));

            Tags.Indexes.CreateOne(
                new CreateIndexModel<TagDataModel>(Builders<TagDataModel>.IndexKeys.Ascending(_ => _.WorkspaceId).Ascending(_ => _.Type),
                    new CreateIndexOptions { Background = true, Unique = true }));

            AutoTaskCreationRules.Indexes.CreateOne(
                new CreateIndexModel<AutoTaskCreationRulesDataModel>(Builders<AutoTaskCreationRulesDataModel>.IndexKeys.Ascending(_ => _.WorkspaceId),
                    new CreateIndexOptions { Background = true, Unique = true }));
        }

        public IMongoCollection<UserDataModel> Users { get; private set; }

        public IMongoCollection<WorkspaceDataModel> Workspaces { get; private set; }

        public IMongoCollection<OptionDataModel> Options { get; private set; }

        public IMongoCollection<RuleDataModel> Rules { get; private set; }

        public IMongoCollection<DictionaryDataModel> Dictionaries { get; private set; }

        public IMongoCollection<DocumentDataModel> Documents { get; private set; }

        public IMongoCollection<TaskDataModel> Tasks { get; private set; }

        public IMongoCollection<WorkflowDataModel> Workflows { get; private set; }

        public IMongoCollection<WorkspaceTypeDataModel> WorkspaceTypes { get; private set; }

        public IMongoCollection<OntologyEntityDataModel> OntologyEntities { get; private set; }

        public IMongoCollection<OntologyRelationDataModel> OntologyRelations { get; private set; }

        public IMongoCollection<OntologyEntityPropertyTypeDataModel> EntityPropertyTypes { get; private set; }

        public IMongoCollection<DialogOntologyDataModel> DialogOntologies { get; private set; }

        public IMongoCollection<TaskTypeDataModel> TaskTypes { get; private set; }

        public IMongoCollection<TagDataModel> Tags { get; set; }

        public IMongoCollection<AutoTaskCreationRulesDataModel> AutoTaskCreationRules { get; set; }

        public IMongoCollection<CategoryDataModel> Categories { get; set; }
    }
}
