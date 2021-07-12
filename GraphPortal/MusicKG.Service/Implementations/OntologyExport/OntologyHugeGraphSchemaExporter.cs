using MusicKG.Service.Constants;
using MusicKG.Service.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicKG.Service.Implementations.OntologyExport
{
    public class OntologyHugeGraphSchemaExporter : IOntologyExportProvider
    {
        /// <summary>
        /// Constructor of exporter for ontology export as hugegraph schema.
        /// </summary>
        public OntologyHugeGraphSchemaExporter()
        {
        }

        public async Task<byte[]> ExportAsync(OntologyDownloadServiceModel downloadModel)
        {
            return await Task.Run(() =>
            {
                var statementList = GenerateSchemaOperationStatementList(downloadModel);
                return Encoding.UTF8.GetBytes(String.Join(OntologyConstant.HugeGraphNewLineCharacter, statementList));
            });
        }

        private List<string> GenerateSchemaOperationStatementList(OntologyDownloadServiceModel downloadModel)
        {
            List<HugeGraphSchemaPropertyKeyServiceModel> PropertyKeys = new List<HugeGraphSchemaPropertyKeyServiceModel>();
            List<HugeGraphSchemaVertexLabelServiceModel> VertexLabels = new List<HugeGraphSchemaVertexLabelServiceModel>();
            List<HugeGraphSchemaEdgeLabelServiceModel> EdgeLabels = new List<HugeGraphSchemaEdgeLabelServiceModel>();

            var primaryKey = new HugeGraphSchemaPropertyKeyServiceModel(OntologyConstant.HugeGraphId, OntologyConstant.HugeGraphText);
            PropertyKeys.Add(primaryKey);

            foreach (var entity in downloadModel.Entities)
            {
                var propertyNames = new List<string>
                {
                    primaryKey.Name
                };

                foreach (var property in entity.Properties)
                {
                    var temp = new HugeGraphSchemaPropertyKeyServiceModel(property.Name, property.Type);

                    if (PropertyKeys.Exists(x => x.Name == temp.Name && x.Datatype == temp.Datatype))
                    {
                        propertyNames.Add(temp.Name);
                        continue;
                    }
                    else
                    {
                        if (PropertyKeys.Exists(x => x.Name == temp.Name && x.Datatype != temp.Datatype))
                        {
                            temp.Name = $"{entity.Name}_{property.Name}";
                        }

                        PropertyKeys.Add(temp);

                        propertyNames.Add(temp.Name);
                    }
                }

                var nullableKeys = propertyNames.Where(x => x != primaryKey.Name).ToList();
                VertexLabels.Add(new HugeGraphSchemaVertexLabelServiceModel(entity.Name, propertyNames, new List<string>() { primaryKey.Name }, nullableKeys));
            };

            foreach (var relation in downloadModel.Relations)
            {
                var propertyNames = new List<string>
                {
                    primaryKey.Name
                };

                foreach (var property in relation.Properties)
                {
                    var temp = new HugeGraphSchemaPropertyKeyServiceModel(property.Name, property.Type);

                    if (PropertyKeys.Contains(temp))
                    {
                        propertyNames.Add(temp.Name);
                        continue;
                    }
                    else
                    {
                        if (PropertyKeys.Exists(x => x.Name == temp.Name && x.Datatype != temp.Datatype))
                        {
                            temp.Name = $"{relation.Name}_{property.Name}";
                        }

                        PropertyKeys.Add(temp);
                        propertyNames.Add(temp.Name);
                    }
                }

                var nullableKeys = propertyNames.Where(x => x != primaryKey.Name).ToList();

                EdgeLabels.Add(new HugeGraphSchemaEdgeLabelServiceModel(relation.Name, relation.FirstEntityName, relation.SecondEntityName, propertyNames, nullableKeys));
            };

            var statementList = new List<string>();

            var propertyKeyFormat = OntologyConstant.HugeGraphPropertyKeyFormat;
            var propertyKeyContents = PropertyKeys.Distinct().Select(x => String.Format(propertyKeyFormat,
                x.Name,
                x.Datatype.Split(OntologyConstant.HugeGraphList).First(),
                x.IsMultiValue ? OntologyConstant.HugeGraphValueSet : String.Empty)).ToList();
            statementList.AddRange(propertyKeyContents);
            statementList.Add(OntologyConstant.HugeGraphNewLineCharacter);

            var vertexLabelFormat = OntologyConstant.HugeGraphVertexLabelFormat;
            var vertexLabelContents = VertexLabels.Select(x => String.Format(vertexLabelFormat,
                x.Name,
                String.Join(",", x.Properties.Select(p => $"\"{p}\"")),
                String.Join(",", x.PrimaryKeys.Select(p => $"\"{p}\"")),
                String.Join(",", x.NullableKeys.Select(p => $"\"{p}\"")))).ToList();
            statementList.AddRange(vertexLabelContents);
            statementList.Add(OntologyConstant.HugeGraphNewLineCharacter);

            var edgeLabelFormat = OntologyConstant.HugeGraphEdgeLabelFormat;
            var edgeLabelContents = EdgeLabels.Select(x => String.Format(edgeLabelFormat,
               x.Name,
               x.SourceLabel,
               x.TargetLabel,
               String.Join(",", x.Properties.Select(p => $"\"{p}\"")),
               String.Join(",", x.NullableKeys.Select(p => $"\"{p}\""))
               )).ToList();
            statementList.AddRange(edgeLabelContents);

            return statementList;
        }

    }
}
