using MongoDB.Bson;
using MusicKG.Service.Implementations.OntologyExport;
using MusicKG.Service.Models;
using MusicKG.Service.Test.Fixtures;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MusicKG.Service.Test
{
    [Collection("MongoCollection")]
    public class OntologyHugeGraphSchemaExporterTest
    {
        private readonly IOntologyExportProvider exporterUnderTest;

        public OntologyHugeGraphSchemaExporterTest(MongoFixture mongoFixture)
        {
            exporterUnderTest = new OntologyHugeGraphSchemaExporter();
        }

        [Fact]
        public async Task Export()
        {
            OntologyDownloadServiceModel downloadModel = new OntologyDownloadServiceModel()
            {
                Entities = PrepareOntologyEntityDataModels(),
                Relations = PrepareOntologyRealtionDataModels(),
            };
            
            var bytes = await exporterUnderTest.ExportAsync(downloadModel);

            Assert.NotNull(bytes);

            Assert.True(bytes.Length > 0);
        }

        private List<OntologyRelationDownloadServiceModel> PrepareOntologyRealtionDataModels()
        {
            var result = new List<OntologyRelationDownloadServiceModel>();

            for (int i = 1; i < 4; i++)
            {
                result.Add(new OntologyRelationDownloadServiceModel
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    WorkspaceId = ObjectId.GenerateNewId().ToString(),
                    Name = "realtion" + i,
                    FirstEntityName = "firstEntity",
                    SecondEntityName = "secondEntity",
                    Properties = new List<OntologyRelationPropertyServiceModel>()
                    {
                        new OntologyRelationPropertyServiceModel()
                        {
                            Name="RelationProperty1",
                            Description="description",
                            Type ="Text"
                        },
                        new OntologyRelationPropertyServiceModel()
                        {
                            Name="RelationProperty12",
                            Description="description",
                            Type ="TextList"
                        },
                        new OntologyRelationPropertyServiceModel()
                        {
                            Name="RelationProperty3",
                            Description="description",
                            Type ="Float"
                        },
                        new OntologyRelationPropertyServiceModel()
                        {
                            Name="RelationProperty4",
                            Description="description",
                            Type ="FloatList"
                        }
                    },
                    Description = "Description",
                });
            }

            return result;
        }

        private List<OntologyEntityServiceModel> PrepareOntologyEntityDataModels()
        {
            var result = new List<OntologyEntityServiceModel>();

            for (int i = 1; i < 4; i++)
            {
                result.Add(new OntologyEntityServiceModel
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    WorkspaceId = ObjectId.GenerateNewId().ToString(),
                    Name = "entity" + i,
                    Properties = new List<OntologyEntityPropertyServiceModel>()
                    {
                        new OntologyEntityPropertyServiceModel()
                        {
                            Name="property1",
                            Description="description",
                            Type ="Text"
                        },
                        new OntologyEntityPropertyServiceModel()
                        {
                            Name="property2",
                            Description="description",
                            Type ="TextList"
                        },
                        new OntologyEntityPropertyServiceModel()
                        {
                            Name="property1",
                            Description="description",
                            Type ="Float"
                        },
                        new OntologyEntityPropertyServiceModel()
                        {
                            Name="property2",
                            Description="description",
                            Type ="FloatList"
                        }
                    },
                    Description = "Description",
                    Color = "#111456",
                });
            }
            return result;
        }
    }
}
