using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	[SerializeField] string sceneNameStart = "Home";

	public void StartGame()
    {
		// 初始化遊戲數據 先載入卡牌商店資料之後再載入玩家資料
		CardStore.instance.LoadCardData(); // 從腳本化物件載入卡牌資料
		PlayerDataManager.instance.LoadPlayerData(); // 載入玩家資料
		SceneManager.LoadScene(sceneNameStart); // 切換到遊戲場景
	}

	public void QuitGame()
	{
		Application.Quit(); // 退出遊戲應用程式
	}

	public void ChangeScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName); // 切換到指定的場景
	}

	public void ChangeScene(int sceneIndex)
	{
		SceneManager.LoadScene(sceneIndex); // 切換到指定的場景
	}
}
