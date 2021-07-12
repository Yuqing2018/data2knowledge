using Microsoft.Extensions.Options;
using Mongo2Go;
using MongoDB.Driver;
using MusicKG.Service;
using MusicKG.Service.Implementations;
using MusicKG.DataAccess.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using MusicKG.DataAccess;
using MusicKG.Service.Test.Helpers;
using MusicKG.DataAccess.Models;
using MusicKG.DataAccess.Enums;
using MusicKG.Service.Test.Models;

namespace MusicKG.Service.Test.Fixtures
{
    public class StatisiticsFixture : MongoFixture
    {
        public readonly (IEnumerable<UserDataModel>, IEnumerable<TaskDataModel>, IEnumerable<DocumentDataModel>) Data;

        public StatisiticsFixture()
        {
            Data = StatisiticsDataHelper.PrepareTasksForStatisitics(Context).Result;
        }
    }
}
