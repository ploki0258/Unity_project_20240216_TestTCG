using UnityEngine;
using UnityEngine.EventSystems;

public class ClickCard : MonoBehaviour, IPointerDownHandler
{
	[SerializeField] public CardState state = CardState.inLibrary; // 卡牌狀態
	
	Deck deck;

	void Awake()
	{
		deck = GameObject.Find("DeckManager").GetComponent<Deck>();
	}

	private void Start()
	{
		//Debug.Log($"{GetComponent<CardDisplay>().card.cardName} ID:{GetComponent<CardDisplay>().card.id}:卡牌狀態設置為 {state}");
	}

	// 當玩家點擊卡牌時觸發
	public void OnPointerDown(PointerEventData eventData)
	{
		int id = GetComponent<CardDisplay>().card.id; // 獲取卡牌ID
		deck.UpdateCard(state, id);
	}

	/// <summary>
	/// 設置卡牌狀態
	/// </summary>
	/*void SetCardState(int id)
	{
		for (int i = 0; i < PlayerDataManager.instance.playerDatas.playerDecks.Length; i++)
		{
			//int id = GetComponent<CardDisplay>().card.id;
			// 檢查玩家擁有的卡牌ID是否與當前卡牌ID相同
			if (i == id)
			{
				state = CardState.inDeck;   // 否則表示卡牌在牌組中
			}
			else
			{
				state = CardState.inLibrary;    // 表示卡牌在倉庫中
			}
		}
	}*/
}

public enum CardState
{
	inDeck,		// 在牌組中
	inLibrary,  // 在倉庫中
}
