using TMPro;
using UnityEngine;

public class LifePointDisplay : MonoBehaviour
{
	[SerializeField, Header("生命值")] TextMeshProUGUI lifePlayerText, lifeEnemyText;

	private void Start()
	{
		UpdatePlayerLifeText(); // 初始化顯示生命值
		UpdateEnemyLifeText();
		PlayerDataManager.instance.playerDatas.renewPlayerLifePointChange += UpdatePlayerLifeText;
		PlayerDataManager.instance.enemyDatas.renewEnemyLifePointChange += UpdateEnemyLifeText;
	}

	private void OnDisable()
	{
		PlayerDataManager.instance.playerDatas.renewPlayerLifePointChange -= UpdatePlayerLifeText;
		PlayerDataManager.instance.enemyDatas.renewEnemyLifePointChange -= UpdateEnemyLifeText;
	}

	void UpdatePlayerLifeText() => lifePlayerText.text = $"{PlayerDataManager.instance.playerDatas.playerLifePoint}";
	void UpdateEnemyLifeText() => lifeEnemyText.text = $"{PlayerDataManager.instance.enemyDatas.enemyLifePoint}";
	
}
