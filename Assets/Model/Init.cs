using System;
using System.Collections;
using UnityEngine;

namespace ETModel
{

    public class Init : MonoBehaviour
    {
        void Start()
        {
            StartCoroutine(ETVoid());
        }

        private IEnumerator ETVoid()
        {
            try
            {
                //SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);

                DontDestroyOnLoad(gameObject);
                
                //事件系统
                //Game.EventSystem.Add(DLLType.Model, typeof(Init).Assembly);

                Game.Hotfix.LoadHotfixAssembly();

                // 加载配置
                //Game.Scene.GetComponent<ResourcesComponent>().LoadBundle("config.unity3d");
                //Game.Scene.AddComponent<ConfigComponent>();
                //Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle("config.unity3d");
                //Game.Scene.AddComponent<OpcodeTypeComponent>();
                //Game.Scene.AddComponent<MessageDispatcherComponent>();

                Game.Hotfix.GotoHotfix();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            yield return null;
        }

        private void Update()
        {
            //OneThreadSynchronizationContext.Instance.Update();
            Game.Hotfix.Update?.Invoke();
            //Game.EventSystem.Update();
        }

        private void LateUpdate()
        {
            Game.Hotfix.LateUpdate?.Invoke();
            //Game.EventSystem.LateUpdate();
        }

        private void OnApplicationQuit()
        {
            Game.Hotfix.OnApplicationQuit?.Invoke();
            Game.Close();
        }

    }
}