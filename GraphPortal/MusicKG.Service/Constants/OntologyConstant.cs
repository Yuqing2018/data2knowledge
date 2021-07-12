using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.Service.Constants
{
    public static class OntologyConstant
    {
        public const string PropertyAndRelationNamePattern = "^[a-zA-Z0-9_]+$";

        public const string OntologyJsonFileFormat = "Ontology_{0}.json";

        public const string HugeGraphNewLineCharacter = "\n";
        public const string HugeGraphId = "Id";
        public const string HugeGraphText = "Text";
        public const string HugeGraphPropertyKeyFormat = "schema.propertyKey(\"{0}\").as{1}(){2}.ifNotExist().create();";
        public const string HugeGraphList = "List";
        public const string HugeGraphValueSet = ".valueSet()";
        public const string HugeGraphVertexLabelFormat = "schema.vertexLabel(\"{0}\").properties({1}).primaryKeys({2}).nullableKeys({3}).ifNotExist().create();";
        public const string HugeGraphEdgeLabelFormat = "schema.edgeLabel(\"{0}\").sourceLabel(\"{1}\").targetLabel(\"{2}\").properties({3}).nullableKeys({4}).ifNotExist().create();";
        public const string HugeGraphOntologyFileFormat = "OntologySchame_{0}.groovy";
    }
}
