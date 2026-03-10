using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace NoaTheSoap.GUBV
{
    public class GitBuildValidator : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;
    
        public void OnPreprocessBuild(BuildReport report)
        {
            if (!EditorPrefs.GetBool("GitValidator_DisplayWarning", true))
                return;
        
            if (!IsGitUpToDate())
            {
                // Viser en popup-dialog i Unity
                bool proceed = EditorUtility.DisplayDialog(
                    "Git Warning!",
                    $"You have not pulled the latest changes from GitHub (you are {GitCommand.GetBehindCount()} commits behind 'origin/{EditorPrefs.GetString("GitValidator_MainBranch", "main")}'). Do you want to build anyways?",
                    "Build",
                    "Cancel build"
                );

                if (!proceed)
                {
                    throw new BuildFailedException("Build was canceled by the user because Git is not up to date.");
                }
            }
        }
    
        private bool IsGitUpToDate()
        {
            string behindCount = GitCommand.GetBehindCount();
            return behindCount == "0";
        }
    }
}

