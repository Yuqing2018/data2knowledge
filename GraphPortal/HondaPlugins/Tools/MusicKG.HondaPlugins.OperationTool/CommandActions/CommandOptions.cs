using MusicKG.HondaPlugins.DataAccess.Enums;
using CommandLine;

namespace MusicKG.HondaPlugins.OperationTool.CommandActions
{
    [Verb("model", HelpText = "Train and serve model automaically.")]
    public class ModelOptions
    {
        [Option('t', "train", Group = "model action", HelpText = "The model name to be trained. TextClassificationModel or DecisionTreeModel.")]
        public bool Train { get; set; }

        [Option('l', "list", Group = "model action", HelpText = "List model versions.")]
        public bool List { get; set; }

        [Option('r', "revert", Group = "model action", HelpText = "Revert model to specified version")]
        public bool Revert { get; set; }

        [Option('n', "name", HelpText = "The model name to be trained. TextClassificationModel or DecisionTreeModel.")]
        public HondaModelNames? ModelName { get; set; }

        [Option('v', "version", HelpText = "The version to be reverted to.")]
        public int? Version { get; set; }
    }

    [Verb("user", HelpText = "User maintenance.")]
    public class UserOptions
    {
        [Option('l', "list", Group = "user action", HelpText = "List users.")]
        public bool List { get; set; }

        [Option('r', "reset", Group = "user action", HelpText = "Reset user password.")]
        public bool ResetPassword { get; set; }

        [Option('d', "disable", Group = "user action", HelpText = "Disable user.")]
        public bool Disable { get; set; }

        [Option('e', "enable", Group = "user action", HelpText = "Enable user")]
        public bool Enable { get; set; }

        [Option('c', "create", Group = "user action", HelpText = "Create user.")]
        public bool Create { get; set; }

        [Option('a', "rename", Group = "user action", HelpText = "Rename user")]
        public bool Rename { get; set; }

        [Option('u', "user", HelpText = "The name of user.")]
        public string UserName { get; set; }

        [Option('p', "password", HelpText = "The password of user.")]
        public string Password { get; set; }

        [Option('n', "name", HelpText = "The new name of user.")]
        public string NewName { get; set; }
    }
}
