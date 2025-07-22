using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenPackage : MonoBehaviour
{
	[SerializeField, Header("卡牌預製物")] GameObject cardPrefabs; // 卡片預製件
	[SerializeField, Header("卡牌生成位置")] GameObject cardPool;
	[SerializeField, Header("卡包花費費用")] int openCost = 10; // 開啟卡包的費用
	[SerializeField, Header("稀有度機率值")] float[] dropRate;

	[Tooltip("已獲得的卡牌列表")]
	List<GameObject> cards = new List<GameObject>(); // 卡片列表

	private void Awake()
	{
		if (PlayerDataManager.instance.playerDatas == null)
			SceneManager.LoadScene("MainMenu"); // 如果玩家資料不存在，則返回主菜單
	}

	private void Start()
	{
		CardStore.instance.SetDropRates(dropRate); // 設置掉落率
	}

	private void Update()
	{
#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.A)) // 在編輯器中按下A鍵 增加玩家硬幣
		{
			PlayerDataManager.instance.playerDatas.playerCoins += 10;
		}
#endif
	}

	/// <summary>
	/// 開啟卡包，獲取卡牌(預設開5張卡牌)
	/// </summary>
	/// <param name="openCount">要開啟的卡牌張數</param>
	public void OnClickOpen(int openCount = 5)
	{
		// 如果玩家硬幣不足，則不執行
		if (PlayerDataManager.instance.playerDatas.playerCoins < openCost)
		{
			Debug.Log("<color=red>玩家硬幣不足，無法開啟卡包！</color>");
			return;
		}

		// 扣除開啟卡包的費用
		PlayerDataManager.instance.playerDatas.playerCoins -= openCost;

		// 抽卡前先清空卡片池中的所有卡片
		foreach (var card in cards)
		{
			Destroy(card);
		}
		cards.Clear();

		// 生成指定數量的卡片
		for (int i = 0; i < openCount; i++)
		{
			GameObject tempCard = Instantiate(cardPrefabs, cardPool.transform);
			tempCard.GetComponent<CardDisplay>().card = CardStore.instance.RandomGetCard(dropRate);
			cards.Add(tempCard);
		}

		// 更新卡片顯示
		for (int i = 0; i < cards.Count; i++)
		{
			cards[i].GetComponent<CardDisplay>().ShowCard(CardStore.instance.GetCardDataById(cards[i].GetComponent<CardDisplay>().card.id));
		}

		SaveOpenCard(); // 儲存開過的卡片
		PlayerDataManager.instance.SavePlayerData(); // 儲存玩家資料
	}

	/// <summary>
	/// 儲存開啟的卡片
	/// </summary>
	void SaveOpenCard()
	{
		foreach (GameObject card in cards)
		{
			int id = card.GetComponent<CardDisplay>().card.id;
			PlayerDataManager.instance.playerDatas.playerCards[id]++;
		}
	}
}
