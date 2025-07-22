using UnityEngine;
using UnityEngine.UI;

public class CoinDisplay : MonoBehaviour
{
	[SerializeField, Header("金幣文字")] Text coinText;

	private void Start()
	{
		RenewCoin(); // 初始化顯示金幣數量
		PlayerDataManager.instance.playerDatas.renewPlayerCoinsCallback += RenewCoin;
	}

	private void OnDisable()
	{
		PlayerDataManager.instance.playerDatas.renewPlayerCoinsCallback -= RenewCoin;
	}

	/// <summary>
	/// 更新玩家金幣顯示
	/// </summary>
	void RenewCoin()
	{
		coinText.text = $"x {PlayerDataManager.instance.playerDatas.playerCoins.ToString()}";
		//Debug.Log($"玩家金幣數量: {PlayerDataManager.instance.playerDatas.playerCoins}");
	}
}
