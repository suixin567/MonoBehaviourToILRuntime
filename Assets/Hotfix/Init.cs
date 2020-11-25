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
				//注册热更层回调
				//ETModel.Game.Hotfix.Update = () => { Update(); };
				//ETModel.Game.Hotfix.LateUpdate = () => { LateUpdate(); };
				//ETModel.Game.Hotfix.FixedUpdate = () => { FixedUpdate(); };
				//ETModel.Game.Hotfix.OnApplicationQuit = () => { OnApplicationQuit(); };


				//加载热更配置
				//ETModel.Game.Scene.GetComponent<ResourcesComponent>().LoadBundle("config.unity3d");
				//Game.Scene.AddComponent<ConfigComponent>();
				//ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle("config.unity3d");

				//切换场景
				ResMgr.LoadScene(@"Assets/Res/Scenes/LoginScene.unity", delegate (AssetRequest a)
				{
				});
            }
			catch (Exception e)
			{
				Debug.LogError(e);
			}
		}

        #region 添加热更程序集脚本
        public static void Unity2Hotfix<T>(GameObject go) where T : MonoBehaviour
		{
			//Debug.LogWarning("添加热更脚本");
			go.AddComponent<T>();			
		}

		public static GameObject InstantiateWithClass<T>(GameObject prefab) where T : MonoBehaviour
		{
			var obj = GameObject.Instantiate(prefab);
			obj.AddComponent<T>();
			return obj;
		}
        #endregion

        public static void Update()
		{
			//try
			//{
			//	Game.EventSystem.Update();
			//}
			//catch (Exception e)
			//{
			//	Log.Error(e);
			//}
		}

		public static void LateUpdate()
		{
			//try
			//{
			//	Game.EventSystem.LateUpdate();
			//}
			//catch (Exception e)
			//{
			//	Log.Error(e);
			//}
		}
		public static void FixedUpdate()
		{
			//try
			//{
			//	Game.EventSystem.FixedUpdate();
			//}
			//catch (Exception e)
			//{
			//	Log.Error(e);
			//}
		}
	}
}