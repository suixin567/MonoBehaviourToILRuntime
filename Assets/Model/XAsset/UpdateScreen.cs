using libx;
using UnityEngine;
using UnityEngine.UI;

public class UpdateScreen : MonoBehaviour 
{
	public Text message;
	public Slider progressBar;
	public Text progressText;
	public Text loadingText;

	void Start() 
	{
		GetComponent<Updater>().completed += this.updateCompleted;
	}

	void updateCompleted() 
	{
		Debug.Log("资源更新完成，初始化冷更层...");
		GameObject ModelInit = new GameObject();
		ModelInit.name = "ModelInit";
		ModelInit.AddComponent<ETModel.Init>();
	}
}