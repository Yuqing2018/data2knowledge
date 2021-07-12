using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MusicKG.Service.Test.Fixtures
{
    [CollectionDefinition("StatisiticsCollection", DisableParallelization = false)]
    public class StatisiticsCollectionFixture : ICollectionFixture<StatisiticsFixture>
    {
    }
}
