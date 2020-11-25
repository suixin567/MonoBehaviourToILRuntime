using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

class BuildPreprocess : IPreprocessBuildWithReport, IPostprocessBuildWithReport
{
    public int callbackOrder => 0;
    public void OnPreprocessBuild(BuildReport report)
    {
        if (File.Exists("Assets/Hotfix/Unity.Hotfix.asmdef"))
        {
            File.Move("Assets/Hotfix/Unity.Hotfix.asmdef", "Assets/Hotfix/Unity.Hotfix.asmdef~");
            AssetDatabase.Refresh();
            Debug.Log("发布前移除Unity.Hotfix.asmdef");
        }
        else {
            Debug.LogWarning("发布前移除Unity.Hotfix.asmdef 出错，文件不存在。");
        }
    }
    public void OnPostprocessBuild(BuildReport report)
    {
        if (File.Exists("Assets/Hotfix/Unity.Hotfix.asmdef~"))
        {
            File.Move("Assets/Hotfix/Unity.Hotfix.asmdef~", "Assets/Hotfix/Unity.Hotfix.asmdef");
            AssetDatabase.Refresh();
            Debug.Log("发布后恢复Unity.Hotfix.asmdef");
        }
        else {
            Debug.LogError("发布后恢复Unity.Hotfix.asmdef 出错，文件不存在。");
        }
    }
}


