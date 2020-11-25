#if ILRuntime
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class IL2CppStrip : MonoBehaviour
{
    private const string KMarkAssetsWithName = "Tools/ILRuntime/剪裁检测";

    [MenuItem(KMarkAssetsWithName)]
    public static void FindAllUnityClass()
    {
        EditorUtility.DisplayProgressBar("Progress", "Find Class...", 0);
        string[] dirs = { "Assets/Res" };
        var asstIds = AssetDatabase.FindAssets("t:Prefab", dirs);
        int count = 0;
        List<string> classList = new List<string>();
        for (int i = 0; i < asstIds.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(asstIds[i]);
            var pfb = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            foreach (Transform item in pfb.transform)
            {
                var coms = item.GetComponentsInChildren<Component>();
                foreach (var com in coms)
                {
                    string tName = com.GetType().FullName;
                    if (!classList.Contains(tName) && tName.StartsWith("UnityEngine"))
                    {
                        classList.Add(tName);
                    }
                }
            }
            count++;
            EditorUtility.DisplayProgressBar("Find Class", pfb.name, count / (float)asstIds.Length);
        }
        for (int i = 0; i < classList.Count; i++)
        {
            classList[i] = string.Format("<type fullname=\"{0}\" preserve=\"all\"/>", classList[i]);
        }
        System.IO.File.WriteAllLines(Application.dataPath + "/ClassTypes.txt", classList);
        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
    }
}
#endif