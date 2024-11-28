using UnityEngine;

[System.Serializable]
public class Card
{
	[Header("卡片名稱")] public string cardName;
	[Header("卡片類型")] public CardType cardType;
	[Header("卡片稀有度")] public CardRare cardRare;
	[Header("可否被攻擊")] public bool canHurt;
	[Header("卡片費用")] public int cardCost;
	[Header("攻擊力")] public int attack;
	[Header("生命值")] public int health;
	[Header("卡片圖示")] public Sprite cardIcon;
	[Header("卡圖背景")] public Sprite cardBG;
	[Header("卡片描述")][TextArea(5, 7)] public string cardDescription;
	[Header("卡片特性")][TextArea(1, 3)] internal string[] cardFeature;
}
// TODO
/// <summary>
/// 卡牌種類
/// </summary>
public enum CardType
{
	/// <summary>
	/// 生物
	/// </summary>
	Biology,
	/// <summary>
	/// 法術
	/// </summary>
	Sell,
	/// <summary>
	/// 裝備
	/// </summary>
	Equipment,
}

/// <summary>
/// 卡牌稀有度
/// </summary>
public enum CardRare
{
	/// <summary>
	/// 常見(Common 普卡)
	/// </summary>
	Common,
	/// <summary>
	/// 不常見(Uncommon 普卡)
	/// </summary>
	Uncommon,
	/// <summary>
	/// 稀有(Rare 閃卡)
	/// </summary>
	Rare,
	/// <summary>
	/// 雙重稀有(Double Rare)
	/// </summary>
	RR,
	/// <summary>
	/// 三重稀有(Triple Rare)
	/// </summary>
	RRR,
	/// <summary>
	/// 非常稀有(Super Rare)
	/// </summary>
	SR,
	/// <summary>
	/// 極其稀有(Ultra Rare)
	/// </summary>
	UR,
}