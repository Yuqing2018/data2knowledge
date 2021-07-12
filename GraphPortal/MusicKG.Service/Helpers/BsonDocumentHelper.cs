using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.Service.Helpers
{
    public class BsonDocumentHelper
    {
        public static BsonDocument BuildUnwindStage(string path)
        {
            return new BsonDocument
            {
                { "$unwind", new BsonDocument { { "path", path } } }
            };
        }

        public static BsonDocument BuildLookupStage(string from, string localField, string foreignField, string asDocument)
        {
            return new BsonDocument {
                { "$lookup", new BsonDocument {
                        { "from", from },
                        { "localField", localField },
                        { "foreignField", foreignField },
                        { "as",  asDocument }
                    }
                }
            };
        }

        public static BsonDocument BuildFirstItem(string fieldName)
        {
            return new BsonDocument
            {
                { "$first", $"{fieldName}" }
            };
        }

        public static BsonDocument BuildDateTimeFilter(string fieldName, string compareOperator, DateTime date)
        {
            return new BsonDocument
            {
                { fieldName, new BsonDocument { { compareOperator, date } } }
            };
        }

        public static BsonDocument BuildCompareStage(string compareOperator, string fieldName, DateTime date)
        {
            return new BsonDocument
            {
                { compareOperator, new BsonArray { fieldName, date } }
            };
        }

        public static BsonDocument BuildInTimeCondition(string fieldName, DateTime fromDate, DateTime toDate)
        {
            return new BsonDocument
            {
                {
                    "$and", new BsonArray {
                        BuildCompareStage("$lte", fieldName, toDate),
                        BuildCompareStage("$gte", fieldName, fromDate)
                    }
                }
            };
        }
    }
}
