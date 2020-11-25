using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Hong : MonoBehaviour
{
    [MenuItem("Tools/切换ILRuntime模式")]
    private static void ILRuntimeModel()
    {
        BuildTargetGroup target;
#if UNITY_STANDALONE
        target = BuildTargetGroup.Standalone;
#endif
#if UNITY_ANDROID
        target = BuildTargetGroup.Android;
#endif
#if UNITY_IOS
        target = BuildTargetGroup.iOS;
#endif
        string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(target);
        List<string> list = new List<string>(defines.Split(';'));
        if (!list.Contains("ILRuntime"))
        {
            defines += ";ILRuntime";
            PlayerSettings.SetScriptingDefineSymbolsForGroup(target, defines);
            Debug.Log("切换到ILRuntime模式");
        }
        else
        {
            Debug.Log("切换到ILRuntime模式");
        }
    }

    [MenuItem("Tools/切换Mono模式")]
    private static void MonoModel()
    {
        BuildTargetGroup target;
#if UNITY_STANDALONE
        target = BuildTargetGroup.Standalone;
#endif
#if UNITY_ANDROID
        target = BuildTargetGroup.Android;
#endif
#if UNITY_IOS
        target = BuildTargetGroup.iOS;
#endif
        string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(target);
        List<string> list = new List<string>(defines.Split(';'));
        if (list.Contains("ILRuntime"))
        {
            list.Remove("ILRuntime");
            defines = string.Empty;
            foreach (var item in list)
            {
                defines += string.Format("{0};", item);
            }
            PlayerSettings.SetScriptingDefineSymbolsForGroup(target, defines);
            Debug.Log("切换到Mono模式");
        }
        else
        {
            Debug.Log("切换到Mono模式");
        }
    }

}
