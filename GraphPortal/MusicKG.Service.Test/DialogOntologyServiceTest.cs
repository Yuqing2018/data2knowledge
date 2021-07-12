using MongoDB.Bson;
using MongoDB.Driver;
using MusicKG.DataAccess;
using MusicKG.DataAccess.Models;
using MusicKG.Service.Helpers;
using MusicKG.Service.Implementations;
using MusicKG.Service.Models;
using MusicKG.Service.Test.Fixtures;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MusicKG.Service.Test
{
    public class DialogOntologyServiceTest : IClassFixture<MongoFixture>
    {
        private readonly MongoFixture mongoFixture;
        private readonly IMusicKGContext context;

        public DialogOntologyServiceTest(MongoFixture mongoFixture)
        {
            this.mongoFixture = mongoFixture;
            this.context = new MusicKGContext(mongoFixture.Context.Client, RandomStringHelper.RandomString(10));
        }

        [Theory]
        [InlineData(DialogOntologyStatus.NotExists)]
        [InlineData(DialogOntologyStatus.OnlyEntity)]
        [InlineData(DialogOntologyStatus.OnlyIntent)]
        [InlineData(DialogOntologyStatus.EntityAndIntent)]
        public async Task GetDialogOntologyTest(DialogOntologyStatus dialogOntology)
        {
            var workspaceId = ObjectId.GenerateNewId().ToString();

            var expectedOntology = await PrepareDialogOntology(workspaceId, dialogOntology);

            var service = new DialogOntologyService(context, null);

            var actualOntology = await service.GetDialogOntologyAsync(dialogOntology == DialogOntologyStatus.NotExists ? ObjectId.GenerateNewId().ToString() : expectedOntology.WorkspaceId.ToString());

            if (dialogOntology == DialogOntologyStatus.NotExists)
            {
                Assert.Null(actualOntology);
            }
            else
            {
                switch (dialogOntology)
                {
                    case DialogOntologyStatus.EntityAndIntent:
                        Assert.NotNull(actualOntology.DialogEntityDocumentId);
                        Assert.NotNull(actualOntology.DialogIntentDocumentId);
                        break;
                    case DialogOntologyStatus.OnlyEntity:
                        Assert.Null(actualOntology.DialogIntentDocumentId);
                        Assert.NotNull(actualOntology.DialogEntityDocumentId);
                        break;
                    case DialogOntologyStatus.OnlyIntent:
                        Assert.NotNull(actualOntology.DialogIntentDocumentId);
                        Assert.Null(actualOntology.DialogEntityDocumentId);
                        break;
                    default:
                        break;
                }
                Assert.Equal(expectedOntology.Id.ToString(), actualOntology.Id);
                Assert.Equal(expectedOntology.IntentDocumentId?.ToString(), actualOntology.DialogIntentDocumentId);
                Assert.Equal(expectedOntology.EntityDocumentId?.ToString(), actualOntology.DialogEntityDocumentId);
            }
        }

        [Theory]
        [InlineData(true, DialogOntologyStatus.OnlyEntity)]
        [InlineData(true, DialogOntologyStatus.OnlyIntent)]
        [InlineData(true, DialogOntologyStatus.EntityAndIntent)]
        [InlineData(false, DialogOntologyStatus.OnlyEntity)]
        [InlineData(false, DialogOntologyStatus.OnlyIntent)]
        [InlineData(false, DialogOntologyStatus.EntityAndIntent)]
        public async Task UploadOntologyTest(bool ontologyExists, DialogOntologyStatus dialogOntology)
        {
            var workspaceId = ObjectId.GenerateNewId().ToString();

            var existedOntology = ontologyExists ? await PrepareDialogOntology(workspaceId, DialogOntologyStatus.EntityAndIntent) : null;

            var updatedEntityDocumentId = ObjectId.GenerateNewId().ToString();
            var updatedIntentDocumentId = ObjectId.GenerateNewId().ToString();

            var serviceModel = new DialogOntologyCreateServiceModel
            {
                CurrentId = ontologyExists ? existedOntology.Id.ToString() : null,
                DialogEntityDocumentId = dialogOntology == DialogOntologyStatus.EntityAndIntent || dialogOntology == DialogOntologyStatus.OnlyEntity ? updatedEntityDocumentId : null,
                DialogIntentDocumentId = dialogOntology == DialogOntologyStatus.EntityAndIntent || dialogOntology == DialogOntologyStatus.OnlyIntent ? updatedIntentDocumentId : null
            };

            var service = new DialogOntologyService(context, null);

            await service.UploadDialogOntologyAsync(workspaceId, serviceModel);

            var actualOntology = await context.DialogOntologies.Find(d => d.WorkspaceId == new ObjectId(workspaceId)).FirstOrDefaultAsync();

            Assert.NotNull(actualOntology);
            if (ontologyExists)
            {
                Assert.Equal(existedOntology.Id, actualOntology.Id);
            }
            switch (dialogOntology)
            {
                case DialogOntologyStatus.EntityAndIntent:
                    Assert.Equal(updatedEntityDocumentId, actualOntology.EntityDocumentId.ToString());
                    Assert.Equal(updatedIntentDocumentId, actualOntology.IntentDocumentId.ToString());
                    break;
                case DialogOntologyStatus.OnlyEntity:
                    Assert.Equal(updatedEntityDocumentId, actualOntology.EntityDocumentId.ToString());
                    if (ontologyExists)
                    {
                        Assert.Equal(existedOntology.IntentDocumentId, actualOntology.IntentDocumentId);
                    }
                    else
                    {
                        Assert.Null(actualOntology.IntentDocumentId);
                    }
                    break;
                case DialogOntologyStatus.OnlyIntent:
                    Assert.Equal(updatedIntentDocumentId, actualOntology.IntentDocumentId.ToString());
                    if (ontologyExists)
                    {
                        Assert.Equal(existedOntology.EntityDocumentId, actualOntology.EntityDocumentId);
                    }
                    else
                    {
                        Assert.Null(actualOntology.EntityDocumentId);
                    }
                    break;
                default:
                    break;
            }
        }

        private async Task<DialogOntologyDataModel> PrepareDialogOntology(string workspaceId, DialogOntologyStatus dialogOntology)
        {
            var ontology = new DialogOntologyDataModel
            {
                Id = ObjectId.GenerateNewId(),
                EntityDocumentId = dialogOntology == DialogOntologyStatus.OnlyEntity || dialogOntology == DialogOntologyStatus.EntityAndIntent ? ObjectId.GenerateNewId() : (ObjectId?)null,
                IntentDocumentId = dialogOntology == DialogOntologyStatus.OnlyIntent || dialogOntology == DialogOntologyStatus.EntityAndIntent ? ObjectId.GenerateNewId() : (ObjectId?)null,
                WorkspaceId = new ObjectId(workspaceId)
            };

            await context.DialogOntologies.InsertOneAsync(ontology);

            return ontology;
        }

        public enum DialogOntologyStatus
        {
            NotExists,

            OnlyEntity,

            OnlyIntent,

            EntityAndIntent
        }
    }
}
