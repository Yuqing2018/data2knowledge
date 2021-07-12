using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MusicKG.Workflow.Test.Fixtures
{
    [CollectionDefinition("MongoCollection")]
    public class MongoCollectionFixture : ICollectionFixture<MongoFixture>
    {
    }
}
