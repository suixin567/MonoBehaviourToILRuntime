using UnityEngine;
using System.Collections.Generic;
using System;

namespace libx
{
    public class ResMgr
    {
        List<AssetRequest> loadedAssets = new List<AssetRequest>();

        public static byte[] LoadDll(string assetPath)
        {
            //Debug.Log("加载dll: "  + assetPath);
            var asset = Assets.LoadAsset(assetPath, typeof(UnityEngine.Object));
            var ta = (TextAsset)asset.asset;
            return ta.bytes;
        }

        public static GameObject LoadPrefab(string assetPath)
        {
            var asset = Assets.LoadAsset(assetPath, typeof(UnityEngine.Object));
            var ta = (GameObject)asset.asset;
            return ta;
        }

        public static AssetRequest LoadSprite(string assetPath)
        {
            var asset = Assets.LoadAsset(assetPath, typeof(Sprite));          
            return asset;
        }


        public static void LoadScene(string assetPath, Action<AssetRequest> completed)
        {
            //Debug.Log("加载场景: " + assetPath);

            var asset = Assets.LoadSceneAsync(assetPath, false);
            asset.completed += delegate (AssetRequest a)
            {
                completed?.Invoke(a);
            };
        }
    }
}