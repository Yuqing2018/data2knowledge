namespace MusicKG.HondaPlugins.OperationTool.CommandActions
{
    public static class OptionExtensions
    {
        public static bool Check(this ModelOptions options)
        {
            if (options.List)
                return !options.Revert && !options.Train;

            if (options.Revert)
                return !options.List && !options.Train && options.ModelName.HasValue && options.Version.HasValue;

            if (options.Train)
                return !options.Revert && !options.List && options.ModelName.HasValue;

            return false;
        }

        public static bool Check(this UserOptions options)
        {
            if (options.Create)
                return !options.Disable && !options.Enable && !options.List && !options.ResetPassword && !options.Rename && !string.IsNullOrWhiteSpace(options.UserName) && !string.IsNullOrWhiteSpace(options.Password);

            if (options.List)
                return !options.Disable && !options.Enable && !options.Create && !options.ResetPassword && !options.Rename;

            if (options.Disable)
                return !options.Enable && !options.Create && !options.List && !options.ResetPassword && !options.Rename && !string.IsNullOrWhiteSpace(options.UserName);

            if (options.Enable)
                return !options.Disable && !options.Create && !options.List && !options.ResetPassword && !options.Rename && !string.IsNullOrWhiteSpace(options.UserName);

            if (options.ResetPassword)
                return !options.Disable && !options.Enable && !options.List && !options.Create && !options.Rename && !string.IsNullOrWhiteSpace(options.UserName) && !string.IsNullOrWhiteSpace(options.Password);

            if (options.Rename)
                return !options.Disable && !options.Enable && !options.List && !options.Create && !options.ResetPassword && !string.IsNullOrWhiteSpace(options.UserName) && !string.IsNullOrWhiteSpace(options.NewName);
            
            return false;
        }
    }
}
