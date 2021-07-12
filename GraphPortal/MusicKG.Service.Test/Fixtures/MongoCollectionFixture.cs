using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MusicKG.Service.Test.Fixtures
{
    [CollectionDefinition("MongoCollection", DisableParallelization = false)]
    public class MongoCollectionFixture : ICollectionFixture<MongoFixture>
    {
    }
}
