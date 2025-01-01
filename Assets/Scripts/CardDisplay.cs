using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
	[Header("卡牌")] public Card[] cards;
	[Header("生物卡")] public BiologyCard[] biologyCards;
	[Header("法術卡")] public SpellCard[] spellCards;
	[Header("裝備卡")] public EquipmentCard[] equipmentCards;
	[Header("卡名")] public Text nameText;
	[Header("費用")] public Text costText;
	[Header("攻擊力")] public Text atkText;
	[Header("血量")] public Text hpText;
	[Header("效果文")] public Text descriptionText;
	[Header("圖示")] public Image iconImage;
	[Header("卡背")] public Image bgImage;
}
