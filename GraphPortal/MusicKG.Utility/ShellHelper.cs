using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MusicKG.Utility
{
    public static class ShellHelper
    {
        public static bool RunShell(string shellFile, string workDirectory, double? timeoutInMillsecond,
            Action<string> errorOutputHandler, Action<string> standardOutputHandler,
            params string[] args)
        {
            if (!Directory.Exists(workDirectory))
            {
                errorOutputHandler?.Invoke($"Work directory {workDirectory} is not found.");
                return false;
            }

            shellFile = Path.Combine(workDirectory, shellFile);

            if (!File.Exists(shellFile))
            {
                errorOutputHandler?.Invoke($"Shell script file {shellFile} is not found.");
                return false;
            }

            if (!timeoutInMillsecond.HasValue)
            {
                timeoutInMillsecond = (int?)TimeSpan.FromHours(72).TotalMilliseconds;
            }

            return RunCommand(shellFile, workDirectory, timeoutInMillsecond.Value, 
                errorOutputHandler, standardOutputHandler, args);
        }

        public static bool RunCommand(string command, string workDirectory, double timeoutInMillsecond, Action<string> errorOutputHandler, Action<string> standardOutputHandler, params string[] args)
        {
            var arguments = string.Join(" ", args);
            try
            {
                ProcessStartInfo start = new ProcessStartInfo
                {
                    //python interprater location
                    FileName = command,
                    //argument with file name and input parameters
                    Arguments = arguments,
                    UseShellExecute = false,// Do not use OS shell
                    CreateNoWindow = true, // We don't need new window
                    WorkingDirectory = workDirectory,
                    StandardOutputEncoding = Encoding.UTF8,
                    StandardErrorEncoding = Encoding.UTF8,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                using (Process process = Process.Start(start))
                {
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                    process.OutputDataReceived += (sender, e) =>
                    {
                        if (!string.IsNullOrWhiteSpace(e.Data))
                            standardOutputHandler?.Invoke(e.Data);
                    };
                    process.ErrorDataReceived += (sender, e) =>
                    {
                        if (!string.IsNullOrWhiteSpace(e.Data))
                            errorOutputHandler?.Invoke(e.Data);
                    };
                    process.WaitForExit(Convert.ToInt32(timeoutInMillsecond));
                }

                return true;
            }
            catch (Exception ex)
            {
                errorOutputHandler?.Invoke($"Run command '{command} {arguments}' failed, details: {ex}");
                return false;
            }
        }
    }
}
