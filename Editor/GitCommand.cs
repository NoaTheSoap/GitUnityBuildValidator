using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace NoaTheSoap.GUBV
{
    public static class GitCommand
    {
        public static string RunGitCommand(string args)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = "git",
                Arguments = args,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = Application.dataPath+"/.."
            };

            using Process process = Process.Start(startInfo);
            process.WaitForExit();
            return process.StandardOutput.ReadToEnd();
        }

        public static string GetBehindCount()
        {
            string branch = EditorPrefs.GetString("GitValidator_MainBranch", "main");
            RunGitCommand("fetch");
            string behindCount = RunGitCommand($"rev-list --count HEAD..origin/{branch}").Trim();
            return behindCount;
        }
    }
}

