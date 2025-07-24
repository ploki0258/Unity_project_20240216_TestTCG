using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
	[SerializeField, Header("牌組生成位置")] Transform deckPanel;
	[SerializeField, Header("卡牌生成位置")] Transform libraryPanel;
	[SerializeField, Header("卡牌預製物")] GameObject cardPrefab;
	[SerializeField, Header("牌組最少數量")] int minDeck = 0;
	[SerializeField, Header("牌組最大數量")] int maxDeck = 0;
	[SerializeField, Header("同名卡最大數量")] int maxCard = 4; // 同名卡最大數量

	private Dictionary<int, GameObject> libraryDic = new Dictionary<int, GameObject>();
	private Dictionary<int, GameObject> deckDic = new Dictionary<int, GameObject>();

	void Start()
	{
		UpdateLibrary();
		UpdateDeck();
	}

	/// <summary>
	/// 更新倉庫卡牌列表
	/// </summary>
	void UpdateLibrary()
	{
		for (int i = 0; i < PlayerDataManager.instance.playerDatas.playerCards.Length; i++)
		{
			if (PlayerDataManager.instance.playerDatas.playerCards[i] > 0)
			{
				CreateCard(CardState.inLibrary, i);

				/*GameObject tempCard = Instantiate(cardPrefab, libraryPanel.position, Quaternion.identity, libraryPanel);
				// 更新卡牌數量
				tempCard.GetComponent<CardCounter>().UpdateCardCount(PlayerDataManager.instance.playerDatas.playerCards[i]);
				// 設定卡牌資訊
				tempCard.GetComponent<CardDisplay>().card = CardStore.instance.cardList[i];
				// 設定卡牌狀態
				tempCard.GetComponent<ClickCard>().state = CardState.inLibrary; // 設置卡牌狀態為在倉庫中
				libraryDic.Add(i, tempCard); // 將卡牌加入字典中以便後續使用*/
			}
		}
	}

	/// <summary>
	/// 更新牌組列表
	/// </summary>
	void UpdateDeck()
	{
		for (int i = 0; i < PlayerDataManager.instance.playerDatas.playerDecks.Length; i++)
		{
			if (PlayerDataManager.instance.playerDatas.playerDecks[i] > 0)
			{
				CreateCard(CardState.inDeck, i);

				/*GameObject tempCard = Instantiate(cardPrefab, deckPanel.position, deckPanel.rotation, deckPanel);
				tempCard.GetComponent<CardCounter>().UpdateCardCount(PlayerDataManager.instance.playerDatas.playerDecks[i]);
				tempCard.GetComponent<CardDisplay>().card = CardStore.instance.cardList[i];
				tempCard.GetComponent<ClickCard>().state = CardState.inDeck; // 設置卡牌狀態為在牌組中
				deckDic.Add(i, tempCard); // 將卡牌加入字典中以便後續使用*/
			}
		}
	}

	/// <summary>
	/// 創建卡牌
	/// </summary>
	/// <param name="state"></param>
	/// <param name="id"></param>
	void CreateCard(CardState state, int id)
	{
		Transform targetPanel = state == CardState.inDeck ? deckPanel : libraryPanel;
		int[] refData = state == CardState.inDeck ? PlayerDataManager.instance.playerDatas.playerDecks : PlayerDataManager.instance.playerDatas.playerCards;
		Dictionary<int, GameObject> targetDic = state == CardState.inDeck ? deckDic : libraryDic;

		GameObject tempCard = Instantiate(cardPrefab, targetPanel.position, targetPanel.rotation, targetPanel);
		tempCard.GetComponent<CardCounter>().UpdateCardCount(refData[id]);
		tempCard.GetComponent<CardDisplay>().card = CardStore.instance.cardList[id];
		tempCard.GetComponent<ClickCard>().state = state == CardState.inDeck ? CardState.inDeck : CardState.inLibrary;
		targetDic.Add(id, tempCard);
	}

	/// <summary>
	/// 更新卡牌狀態
	/// </summary>
	/// <param name="state"></param>
	/// <param name="id"></param>
	public void UpdateCard(CardState state, int id)
	{
		switch (state)
		{
			case CardState.inDeck:
				// 在牌組的卡牌被點擊時，牌組數量:-1，倉庫:+1
				// 更新牌組與倉庫數量 (資料層)
				PlayerDataManager.instance.playerDatas.playerDecks[id]--;
				PlayerDataManager.instance.playerDatas.playerCards[id]++;

				// 牌組數量-1 (顯示層)
				if (deckDic[id].GetComponent<CardCounter>().UpdateCardCount(-1))
					deckDic.Remove(id);

				// 若倉庫已有該卡牌則數量+1，否則創建新卡牌
				if (libraryDic.ContainsKey(id))
				{
					libraryDic[id].GetComponent<CardCounter>().UpdateCardCount(1);  // (顯示層)
				}
				else
				{
					CreateCard(CardState.inLibrary, id);
				}
				break;
			case CardState.inLibrary:
				// 若牌組的同名卡的數量已達同名卡限制，就不能再減少
				if (PlayerDataManager.instance.playerDatas.playerDecks[id] == maxCard)
				{
					Debug.Log($"牌組中同名卡: {CardStore.instance.GetCardDataById(id).cardName} 已達到最大數量限制 {maxCard}。");
					return; // 如果超過限制，則不進行後續操作
				}

				// 在倉庫的卡牌被點擊時，牌組數量:+1，倉庫:-1
				// 更新牌組與倉庫數量
				PlayerDataManager.instance.playerDatas.playerCards[id]--;
				PlayerDataManager.instance.playerDatas.playerDecks[id]++;

				if (libraryDic[id].GetComponent<CardCounter>().UpdateCardCount(-1))
					libraryDic.Remove(id);

				if (deckDic.ContainsKey(id))
				{
					// 若牌組的同名卡的數量超過同名卡限制，就不能再添加
					if (PlayerDataManager.instance.playerDatas.playerDecks[id] <= maxCard)
					{
						// 牌組數量+1
						deckDic[id].GetComponent<CardCounter>().UpdateCardCount(1);  // (顯示層)
					}
				}
				else
				{
					CreateCard(CardState.inDeck, id);
				}
				break;
		}
	}

	/// <summary>
	/// 檢查牌組數量
	/// </summary>
	public void CheckDeck()
	{
		int deckCount = 0;

		for (int i = 0; i < PlayerDataManager.instance.playerDatas.playerDecks.Length; i++)
		{
			deckCount += PlayerDataManager.instance.playerDatas.playerDecks[i];
		}

		if (deckCount < minDeck)
		{
			Debug.Log($"牌組數量不足 {deckCount}，至少需要 {minDeck} 張卡牌。\n確定要儲存嗎?");
		}
		else if (deckCount > maxDeck)
		{
			Debug.Log($"牌組數量超過上限 {deckCount}，最多只能有 {maxDeck} 張卡牌。\n確定要儲存嗎?");
		}
		else
		{
			PlayerDataManager.instance.SavePlayerData(); // 儲存玩家資料
			Debug.Log($"牌組數量 {deckCount} 符合要求，可以儲存。");
		}
	}
}