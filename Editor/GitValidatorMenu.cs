using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace NoaTheSoap.GUBV
{
    public class GitValidatorMenu : EditorWindow
    {
        private static string mainBranch = "main";
        private bool displayWarning = true;
    
        private static List<string> existing_branches = new();
    
        [MenuItem("Tools/GitBuildValidator")]
        public static void ShowWindow()
        {
            GetAllBranches();
            GetWindow<GitValidatorMenu>();
            mainBranch = EditorPrefs.GetString("GitValidator_MainBranch", "main");
        }
        
        private void OnEnable()
        {
            mainBranch = EditorPrefs.GetString("GitValidator_MainBranch", "main");
            displayWarning = EditorPrefs.GetBool("GitValidator_DisplayWarning", true);
            GetAllBranches();
        }
    
        private void OnGUI()
        {
            mainBranch = EditorGUILayout.TextField("Branch", mainBranch);
            if (!existing_branches.Contains(mainBranch))
            {
                EditorGUILayout.HelpBox("This branch is not existing in this project.", MessageType.Error);
            }
            displayWarning = EditorGUILayout.Toggle("Display Warning", displayWarning);
    
            EditorPrefs.SetString("GitValidator_MainBranch", mainBranch);
            EditorPrefs.SetBool("GitValidator_DisplayWarning", displayWarning);
        }
    
    
        static void GetAllBranches()
        {
            existing_branches.Clear();
            
            GitCommand.RunGitCommand("fetch");
            string branches = GitCommand.RunGitCommand($"branch -r");
            
            // Remove head and "origin/" from string
            foreach (var line in branches.Split('\n'))
            {
                string trimmed = line.Trim();
                if (string.IsNullOrEmpty(trimmed) || trimmed.Contains("HEAD"))
                    continue;
            
                int slash = trimmed.IndexOf('/');
                if (slash >= 0 && slash + 1 < trimmed.Length)
                    existing_branches.Add(trimmed.Substring(slash + 1));
            }
    
            foreach (var a in existing_branches)
            {
               Debug.Log(a); 
            }
        }
    }
}
