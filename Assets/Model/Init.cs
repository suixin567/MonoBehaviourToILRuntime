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
                DontDestroyOnLoad(gameObject);
                
                Game.Hotfix.LoadHotfixAssembly();

                Game.Hotfix.GotoHotfix();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            yield return null;
        }
    }
}