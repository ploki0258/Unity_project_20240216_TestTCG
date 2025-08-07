using TMPro;
using UnityEngine;

public class LifePointDisplay : MonoBehaviour
{
	[SerializeField, Header("�ͩR��")] TextMeshProUGUI lifePlayerText, lifeEnemyText;

	private void Start()
	{
		UpdatePlayerLifeText(); // ��l����ܥͩR��
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
