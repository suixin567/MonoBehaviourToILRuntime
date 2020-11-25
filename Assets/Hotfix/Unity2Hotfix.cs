using System.Reflection;
using UnityEngine;


public static class MonoBehaviourExtension
{
    //把主工程集中的脚本替换为热更程序集脚本。此方法在Awake方法中使用。
    public static bool Unity2Hotfix(this MonoBehaviour self)
    {
#if ILRuntime
        if (Assembly.GetExecutingAssembly().FullName.StartsWith("Unity"))
        {
            //Debug.LogWarning("删除Unity域脚本准备替换为热更域脚本" + self.name);
            ETModel.Game.Hotfix.Unity2Hotfix(self.gameObject, self.GetType());
            GameObject.Destroy(self);
            return true;
        }
        else if (Assembly.GetExecutingAssembly().FullName.StartsWith("Assembly"))
        {
            Debug.LogError(self.GetType() + "不在Hotfix程序集中，请检查此脚本所在位置！");
            return true;
        }
        return false;
#endif
        return false;
    }

    //判断如果在主项目中，则return。此方法在OnDestory方法中使用。
    public static bool NeedReturn(this MonoBehaviour self) {
#if ILRuntime
        if (Assembly.GetExecutingAssembly().FullName.StartsWith("Unity"))
        {        
            return true;
        }
        else {
            return false;
        }
#endif
        return false;
    }
}