using UnityEngine;
using System;

/// <summary>
/// 卡牌 (父類)
/// </summary>
[Serializable]
public abstract class Card
{
	#region 欄位
	// 共通欄位
	[Header("卡片編號")] public int id;
	[Header("卡片名稱")] public string cardName;
	[Header("卡片類型")] public CardType cardType;
	[Header("卡片稀有度")] public CardRare cardRare;
	//[Header("可否被攻擊")] public bool canHurt;
	[Header("卡片費用")] public int cardCost;
	[Header("卡片圖示")] public Sprite cardIcon;
	[Header("卡圖背景")] public Sprite cardBG;
	[Header("卡片描述")][TextArea(3, 7)] public string cardDescription;
	#endregion

	/// <summary>
	/// 卡牌父類的建構子
	/// </summary>
	/// <param name="_id">卡牌編號</param>
	/// <param name="_name">卡牌名稱</param>
	/// <param name="_cost">卡牌費用</param>
	/// <param name="_rare">卡牌稀有度</param>
	/// <param name="_des">卡牌說明</param>
	public Card(int _id, string _name, int _cost, CardRare _rare, string _des)
	{
		this.id = _id;
		this.cardName = _name;
		this.cardCost = _cost;
		this.cardRare = _rare;
		this.cardDescription = _des;
	}

	/// <summary>
	/// 取得卡牌編號
	/// </summary>
	/// <param name="card">Card類</param>
	/// <returns>int：卡牌編號</returns>
	public int GetCardID(Card card) => card.id;
	/// <summary>
	/// 取得卡牌名稱
	/// </summary>
	/// <param name="card">Card類</param>
	/// <returns>String：卡牌名稱</returns>
	public String GetCardName(Card card) => card.cardName;
	/// <summary>
	/// 取得卡牌類型
	/// </summary>
	/// <param name="card">Card類</param>
	/// <returns>CardType：卡牌類型</returns>
	public CardType GetCardType(Card card) => card.cardType;
}

/// <summary>
/// 生物卡 (子類)
/// </summary>
[Serializable]
public class BiologyCard : Card
{
	#region 欄位
	[Header("攻擊力")] public int attack;
	[Header("生命值")] public int health;
	[Tooltip("當前生命值")] private int currHealth;
	[Header("卡片屬性")] public string[] cardFeature;
	#endregion

	public BiologyCard(int _id, string _name, int _cost, CardRare _rare, string _des, int _atk, int _hea, string[] _feature) : base(_id, _name, _cost, _rare, _des)
	{
		this.cardType = CardType.Biology;
		this.attack = _atk;
		this.health = _hea;
		this.currHealth = this.health;
		this.cardFeature = _feature;
	}
}

/// <summary>
/// 法術卡 (子類)
/// </summary>
[Serializable]
public class SpellCard : Card
{
	public SpellCard(int _id, string _name, int _cost, CardRare _rare, string _des) : base(_id, _name, _cost, _rare, _des)
	{
		this.cardType = CardType.Spell;
	}
}

/// <summary>
/// 裝備卡 (子類)
/// </summary>
[Serializable]
public class EquipmentCard : Card
{
	public EquipmentCard(int _id, string _name, int _cost, CardRare _rare, string _des) : base(_id, _name, _cost, _rare, _des)
	{
		this.cardType = CardType.Equipment;
	}
}

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
	Spell,
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