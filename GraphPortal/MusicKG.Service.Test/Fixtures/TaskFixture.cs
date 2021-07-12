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
    public class TaskFixture : MongoFixture
    {
        public readonly (
            IEnumerable<WorkspaceTypeDataModel>,
            IEnumerable<WorkspaceDataModel>,
            IEnumerable<UserDataModel>,
            IEnumerable<TaskDataModelTest>,
            IEnumerable<DocumentDataModel>,
            IEnumerable<DictionaryDataModel>,
            TaskStatusEnum[][]) Data;

        public TaskFixture()
        {
            Data = TaskDataHelper.PrepareTasksDataWithAllStatus(Context).Result;
        }
    }
}
