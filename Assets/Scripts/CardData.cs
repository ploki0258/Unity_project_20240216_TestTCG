using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Jack/Add New Card")]
public class CardData : ScriptableObject
{
	// 卡牌基本
	[Tooltip("必填，且不能重複")]
	[Header("卡片編號")] public int id;
	[Header("卡片名稱")] public string cardName;
	[Header("卡片類型")] public CardType cardType;
	[Header("卡片稀有度")] public CardRare cardRare;
	[Header("卡片費用")] public int cardCost;
	[Header("卡片圖示")] public Sprite cardIcon;
	[Header("卡圖背景")] public Sprite cardBG;
	[Header("卡片描述")][TextArea(3, 5)] public string cardDescription;
	// 生物卡專屬
	[Tooltip("生物卡用")]
	[Header("攻擊力")] public int attack;
	[Header("生命值")] public int health;
	[Header("卡片屬性")] public string[] cardFeature;
}
