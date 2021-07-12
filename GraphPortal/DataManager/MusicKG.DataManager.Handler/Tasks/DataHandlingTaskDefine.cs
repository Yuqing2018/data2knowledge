using MusicKG.DataAccess.Enums;

namespace MusicKG.DataManager.Handler.Tasks
{
    public class DataHandlingTaskDefine
    {
        public string WorkspaceId { get; set; }

        public TaskStatusEnum UpdatedTaskStatus { get; set; }
    }
}