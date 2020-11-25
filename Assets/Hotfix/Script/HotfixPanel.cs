using UnityEngine;
using UnityEngine.UI;

public class HotfixPanel : MonoBehaviour
{
    private void Awake()
    {
        //下面这句话在每个脚本的Awake()中都要最先写上
        if (this.Unity2Hotfix()) return;
        //你的代码
        transform.SetParent(FristScene.instance.transform);
        transform.localPosition = Vector2.zero;
        transform.localScale = Vector2.one;
        transform.Find("Text").GetComponent<Text>().text = "我是第一个热更面板";
    }

    void OnDestroy()
    {
        //所有OnDestroy()脚本中都要先写下面这句话。
        if (this.NeedReturn()) return;
        //然后再你的代码
    }
}