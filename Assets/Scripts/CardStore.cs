using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardStore
{
	// 單例模式
	#region 單例模式
	public static CardStore instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new CardStore(new LoadCardDataFormScriptableObject());
			}
			return _instance;
		}
	}
	static CardStore _instance;
	#endregion

	public TextAsset cardData;  // 卡牌資料文件
	[Tooltip("卡牌資料列表")] public List<CardData> cardDataList = new List<CardData>();  // 卡牌資料列表(腳本化物件)
	[Tooltip("卡牌列表")] public List<Card> cardList = new List<Card>();  // 卡牌列表
	[Tooltip("稀有度掉落率字典")] Dictionary<CardRare, float> dropRatesDic = new Dictionary<CardRare, float>();

	ICardDataReader cardDataReader;

	// 建構子(構造函式)
	public CardStore(ICardDataReader cardDataReader)
	{
		this.cardDataReader = cardDataReader;
	}

	// 從Resources文件夾中仔入卡片數據存
	// 注意：Resources文件夾中的文件名必須與此處一致
	public void LoadDataFormTextAsset() => cardData = Resources.Load<TextAsset>("CardsData");
	public void LoadDataFormScriptableObject() => cardDataList = Resources.LoadAll<CardData>("CardsDataList").ToList();

	/// <summary>
	/// 載入卡牌資料
	/// </summary>
	public void LoadCardData() => cardDataReader.LoadCardData();

	/// <summary>
	/// 載入測試
	/// </summary>
	public void TestLoad()
	{
		Debug.Log("測試載入卡牌數據：" + cardList.Count);
		foreach (var item in cardList)
		{
			if (item.cardType == CardType.Biology)
			{
				BiologyCard biologyCard = item as BiologyCard;
				Debug.Log($"卡片名稱: {item.cardName}, ID: {item.id}, 費用: {item.cardCost}, 稀有度: {item.cardRare}, 描述: {item.cardDescription},ATK: {biologyCard.attack}, DEF: {biologyCard.healthMax}, 屬性: {biologyCard.cardFeature}");
			}
			else
			{
				Debug.Log($"卡片名稱: {item.cardName}, ID: {item.id}, 費用: {item.cardCost}, 稀有度: {item.cardRare}, 描述: {item.cardDescription}");
			}
		}
	}

	/// <summary>
	/// 依據ID取得卡牌
	/// </summary>
	/// <param name="id">卡牌ID</param>
	/// <returns></returns>
	public Card GetCardById(int id)
	{
		for (int i = 0; i < cardList.Count; i++)
		{
			if (cardList[i].id == id)
			{
				return cardList[i];
			}
		}
		return null;
	}

	/// <summary>
	/// 依據ID取得卡牌腳本化物件
	/// </summary>
	/// <param name="id">卡牌ID</param>
	/// <returns></returns>
	public CardData GetCardDataById(int id)
	{
		for (int i = 0; i < cardDataList.Count; i++)
		{
			if (cardDataList[i].id == id)
			{
				return cardDataList[i];
			}
		}
		return null;
	}

	/// <summary>
	/// 隨機取得卡牌
	/// </summary>
	/// <returns></returns>
	public Card RandomGetCard()
	{
		if (cardList.Count == 0)
		{
			Debug.Log("卡牌列表為空，無法隨機取得卡牌。請先載入卡牌數據。");
			return null;
		}
		Card card = cardList[Random.Range(0, cardList.Count)];
		//Debug.Log($"隨機取得卡牌：{card.id}");
		return card;
	}

	public Card RandomGetCard(float[] dropRates)
	{
		// 依據稀有度的指定機率，隨機取得卡牌
		// 隨機選擇一張卡牌
		// 注意：這裡假設cardList已經被填充了卡牌數據
		// 如果需要更複雜的隨機邏輯，可以根據稀有度或其他條件進行篩選
		// 例如：可以根據稀有度的機率來決定選擇哪一類卡牌
		if (cardList.Count == 0)
		{
			Debug.Log("卡牌列表為空，無法隨機取得卡牌。請先載入卡牌數據。");
			return null;
		}

		for (int i = 0; i < cardList.Count; i++)
		{
			// 如果卡牌的稀有度掉落率大於等於指定的掉落率，則選擇這張卡牌
			if (dropRatesDic[cardList[i].cardRare] >= dropRates[i])
			{
				return cardList[i];
			}
			else
			{
				//Debug.Log($"跳過卡牌：{cardList[i].id}，稀有度：{cardList[i].cardRare}，掉落率：{dropRatesDic[cardList[i].cardRare]}");
			}
		}

		return null;
	}

	/// <summary>
	/// 設定掉落率
	/// </summary>
	/// <param name="dropRates"></param>
	public void SetDropRates(float[] dropRates)
	{
		for (int i = 0; i < dropRates.Length; i++)
		{
			// 依據稀有度的指定機率，設置掉落率
			CardRare rare = (CardRare)i; // 假設稀有度的枚舉與掉落率的索引對應
			if (dropRatesDic.ContainsKey(rare))
			{
				dropRatesDic[rare] = dropRates[i]; // 更新掉落率
			}
			else
			{
				dropRatesDic.Add(rare, dropRates[i]); // 添加新的掉落率
			}
		}
	}

	/// <summary>
	/// 複製卡牌
	/// </summary>
	/// <param name="id">卡牌ID</param>
	/// <returns></returns>
	public Card CopyCard(int id)
	{
		Card copyCard = GetCardById(id);
		CardData cardData = GetCardDataById(copyCard.id);

		if (copyCard == null || cardData == null)
		{
			Debug.Log($"無法找到ID為{id}的卡牌，無法複製。");
			return null;
		}

		// 根據卡牌類型創建新的卡牌實例
		switch (cardData.cardType)
		{
			case CardType.Biology:
				var biologyCard = (BiologyCard)copyCard;
				copyCard = new BiologyCard(biologyCard.id, biologyCard.cardName, biologyCard.cardCost, biologyCard.cardRare, biologyCard.cardDescription, biologyCard.attack, biologyCard.healthMax, biologyCard.cardFeature);
				copyCard.cardBG = cardData.cardBG; // 複製卡牌背景圖
				copyCard.cardIcon = cardData.cardIcon; // 複製卡牌圖示
				break;
			//return copyCard;
			case CardType.Spell:
				var spellCard = (SpellCard)copyCard;
				copyCard = new SpellCard(spellCard.id, spellCard.cardName, spellCard.cardCost, spellCard.cardRare, spellCard.cardDescription);
				copyCard.cardBG = cardData.cardBG; // 複製卡牌背景圖
				copyCard.cardIcon = cardData.cardIcon; // 複製卡牌圖示
				break;
			//return copyCard;
			case CardType.Equipment:
				var equipmentCard = (EquipmentCard)copyCard;
				copyCard = new EquipmentCard(equipmentCard.id, equipmentCard.cardName, equipmentCard.cardCost, equipmentCard.cardRare, equipmentCard.cardDescription);
				copyCard.cardBG = cardData.cardBG; // 複製卡牌背景圖
				copyCard.cardIcon = cardData.cardIcon; // 複製卡牌圖示
				break;
			//return copyCard;
			default:
				Debug.Log($"無法複製未知類型的卡牌：{copyCard.cardType}");
				return null;
		}
		return copyCard;
	}
}

/// <summary>
/// 卡牌資料讀取介面
/// </summary>
public interface ICardDataReader
{
	/// <summary>
	/// 載入卡牌資料
	/// </summary>
	void LoadCardData();
}

/// <summary>
/// 從TextAsset載入卡牌數據
/// </summary>
public class LoadCardDataFormTextAsset : ICardDataReader
{
	public void LoadCardData()
	{
		CardStore.instance.LoadDataFormTextAsset();
		if (CardStore.instance.cardData == null)
		{
			Debug.Log("卡牌數據文件未找到！請檢查Resources文件夾中的文件名。");
			return;
		}
		else
			Debug.Log("卡牌數據文件已從TextAsset成功載入！");

		string[] daraRow = CardStore.instance.cardData.text.Split('\n');
		foreach (var row in daraRow)
		{
			string[] rowArray = row.Split(',');
			if (rowArray[0] == "#")
			{
				continue; // 跳過註解行
			}
			else if (rowArray[0] == "Biology")
			{
				// 創建生物卡
				int id = int.Parse(rowArray[1]);
				string name = rowArray[2];
				int cost = int.Parse(rowArray[3]);
				CardRare rare = (CardRare)System.Enum.Parse(typeof(CardRare), rowArray[4]);
				string description = rowArray[5];
				int attack = int.Parse(rowArray[6]);
				int health = int.Parse(rowArray[7]);
				string[] feature = rowArray[8].Split('、'); // 特性，額外的屬性
				BiologyCard biologyCard = new BiologyCard(id, name, cost, rare, description, attack, health, feature);
				biologyCard.cardBG = CardStore.instance.GetCardDataById(id).cardBG; // 卡牌背景圖
				biologyCard.cardIcon = CardStore.instance.GetCardDataById(id).cardIcon; // 卡牌圖示
				CardStore.instance.cardList.Add(biologyCard);
				/*foreach (string item in feature)
				{
					Debug.Log($"已創建生物卡：{biologyCard.cardName}\nID: {biologyCard.id}\n費用:{biologyCard.cardCost}\n稀有度:{biologyCard.cardRare}\nATK:{biologyCard.attack}\nDEF:{biologyCard.health}\n屬性:{item}");
				}*/
			}
			else if (rowArray[0] == "Spell")
			{
				// 創建法術卡
				int id = int.Parse(rowArray[1]);
				string name = rowArray[2];
				int cost = int.Parse(rowArray[3]);
				CardRare rare = (CardRare)System.Enum.Parse(typeof(CardRare), rowArray[4]);
				string description = rowArray[5];
				SpellCard spellCard = new SpellCard(id, name, cost, rare, description);
				CardStore.instance.cardList.Add(spellCard);
			}
			else if (rowArray[0] == "Equipment")
			{
				// 創建裝備卡
				int id = int.Parse(rowArray[1]);
				string name = rowArray[2];
				int cost = int.Parse(rowArray[3]);
				CardRare rare = (CardRare)System.Enum.Parse(typeof(CardRare), rowArray[4]);
				string description = rowArray[5];
				EquipmentCard equipmentCard = new EquipmentCard(id, name, cost, rare, description);
				CardStore.instance.cardList.Add(equipmentCard);
			}
		}
	}
}

/// <summary>
/// 從ScriptableObject載入卡牌數據
/// </summary>
public class LoadCardDataFormScriptableObject : ICardDataReader
{
	public void LoadCardData()
	{
		CardStore.instance.LoadDataFormScriptableObject();
		if (CardStore.instance.cardDataList.Count == 0)
		{
			Debug.Log("卡牌數據文件未找到！請檢查Resources文件夾中的文件名。");
			return;
		}
		else
			Debug.Log("卡牌數據文件已從ScriptableObject成功載入！");

		foreach (CardData cardData in CardStore.instance.cardDataList)
		{
			// 根據卡牌類型創建對應的卡牌實例
			switch (cardData.cardType)
			{
				case CardType.Biology:
					// 創建生物卡
					BiologyCard biologyCard = new BiologyCard(cardData.id, cardData.cardName, cardData.cardCost, cardData.cardRare, cardData.cardDescription, cardData.attack, cardData.health, cardData.cardFeature);
					// 設置卡牌圖示和背景和背景顏色
					biologyCard.cardBG = cardData.cardBG;
					biologyCard.cardIcon = cardData.cardIcon;
					CardStore.instance.cardList.Add(biologyCard);
					break;
				case CardType.Spell:
					// 創建法術卡
					SpellCard spellCard = new SpellCard(cardData.id, cardData.cardName, cardData.cardCost, cardData.cardRare, cardData.cardDescription);
					spellCard.cardBG = cardData.cardBG;
					spellCard.cardIcon = cardData.cardIcon;
					CardStore.instance.cardList.Add(spellCard);
					break;
				case CardType.Equipment:
					// 創建裝備卡
					EquipmentCard equipmentCard = new EquipmentCard(cardData.id, cardData.cardName, cardData.cardCost, cardData.cardRare, cardData.cardDescription);
					equipmentCard.cardBG = cardData.cardBG;
					equipmentCard.cardIcon = cardData.cardIcon;
					CardStore.instance.cardList.Add(equipmentCard);
					break;
				default:
					break;
			}
		}
	}
}
