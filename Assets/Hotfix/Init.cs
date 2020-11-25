using System;
using ETModel;
using libx;
using UnityEngine;

namespace ETHotfix
{
	public static class Init
	{

		public static void Start()
		{
#if ILRuntime
			if (!Define.IsILRuntime)
			{
				Debug.LogError("冷更层是mono模式, 但是Hotfix层是ILRuntime模式");
			}
#else
			if (Define.IsILRuntime)
			{
				Debug.LogError("冷更层是ILRuntime模式, Hotfix层是mono模式");
			}
#endif
			Debug.Log("初始化热更层...");

			try
			{
				//切换场景
				ResMgr.LoadScene(@"Assets/Res/Scenes/FirstScene.unity", delegate (AssetRequest a) { });
            }
			catch (Exception e)
			{
				Debug.LogError(e);
			}
		}

        #region 添加热更程序集脚本
        public static void Unity2Hotfix<T>(GameObject go) where T : MonoBehaviour
		{
			go.AddComponent<T>();			
		}
        #endregion
	}
}