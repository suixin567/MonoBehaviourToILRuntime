using libx;
using UnityEngine;

public class FristScene : MonoBehaviour
{
    public static FristScene instance;

    private void Awake()
    {
        if (this.Unity2Hotfix()) return;
        //你的代码
        instance = this;
    }

    void Start()
    {
        //这里创建第一个热更面板
        Instantiate(ResMgr.LoadPrefab(@"Assets/Res/UI/hotfixPanel.prefab"));
    }
}