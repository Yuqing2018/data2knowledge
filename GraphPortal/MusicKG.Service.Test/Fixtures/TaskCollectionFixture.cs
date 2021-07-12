using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MusicKG.Service.Test.Fixtures
{
    [CollectionDefinition("TaskCollection", DisableParallelization = false)]
    public class TaskCollectionFixture : ICollectionFixture<TaskFixture>
    {
    }
}
