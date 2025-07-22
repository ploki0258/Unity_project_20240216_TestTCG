using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 用以顯示卡牌的資訊
/// </summary>
public class CardDisplay : MonoBehaviour
{
	#region 欄位
	[Tooltip("卡牌")] public Card card;

	[SerializeField, Header("卡牌")] Card[] cards;
	[SerializeField, Header("生物卡")] BiologyCard[] biologyCards;
	[SerializeField, Header("法術卡")] SpellCard[] spellCards;
	[SerializeField, Header("裝備卡")] EquipmentCard[] equipmentCards;
	//--- 卡牌資訊 UI 元件
	[SerializeField, Header("卡名")] Text nameText;
	[SerializeField, Header("效果文")] Text descriptionText;
	[SerializeField, Header("屬性")] Text featureText;
	[SerializeField, Header("費用")] TextMeshProUGUI costText;
	[SerializeField, Header("攻擊力")] TextMeshProUGUI atkText;
	[SerializeField, Header("血量")] TextMeshProUGUI hpText;
	[SerializeField, Header("圖示")] Image iconImage;
	[SerializeField, Header("卡背")] Image bgImage;
	[SerializeField, Header("攻擊圖示")] Image atkImage;
	[SerializeField, Header("血量圖示")] Image hpImage;
	#endregion

	private void Start()
	{
		for (int i = 0; i < CardStore.instance.cardList.Count; i++)
		{
			ShowCard();
		}
	}

	public void ShowCard()
	{
		nameText.text = card.cardName;
		descriptionText.text = card.cardDescription;
		costText.text = card.cardCost.ToString();
		iconImage.sprite = card.cardIcon;
		bgImage.sprite = card.cardBG;
		bgImage.color = CardStore.instance.GetCardDataById(card.id).cardColor;

		/*if (card is BiologyCard)
		{
			BiologyCard biology = card as BiologyCard;
			atkText.text = biology.attack.ToString();
			hpText.text = biology.health.ToString();
		}*/

		switch (card.cardType)
		{
			case CardType.Biology:
				BiologyCard biology = card as BiologyCard;
				atkText.text = biology.attack.ToString();
				hpText.text = biology.health.ToString();
				featureText.text = string.Join("、", biology.cardFeature);
				break;
			case CardType.Spell:
				//SpellCard spell = card as SpellCard;
				featureText.text = "";
				atkImage.gameObject.SetActive(false);
				hpImage.gameObject.SetActive(false);
				break;
			case CardType.Equipment:
				featureText.text = "";
				atkImage.gameObject.SetActive(false);
				hpImage.gameObject.SetActive(false);
				break;
		}
	}

	public void ShowCard(CardData cardData)
	{
		this.card = CardStore.instance.GetCardById(cardData.id); // 根據ID獲取卡牌實例
		nameText.text = card.cardName;
		descriptionText.text = card.cardDescription;
		costText.text = card.cardCost.ToString();
		iconImage.sprite = card.cardIcon;
		bgImage.sprite = card.cardBG;
		bgImage.color = cardData.cardColor;

		switch (cardData.cardType)
		{
			case CardType.Biology:
				BiologyCard biology = this.card as BiologyCard;
				atkText.text = biology.attack.ToString();
				hpText.text = biology.health.ToString();
				featureText.text = string.Join("、", biology.cardFeature); // 將卡片屬性陣列轉為字串
				break;
			case CardType.Spell:
				SpellCard spell = this.card as SpellCard;
				featureText.text = "";
				atkImage.gameObject.SetActive(false);
				hpImage.gameObject.SetActive(false);
				break;
			case CardType.Equipment:
				EquipmentCard equipment = this.card as EquipmentCard;
				featureText.text = "";
				atkImage.gameObject.SetActive(false);
				hpImage.gameObject.SetActive(false);
				break;
			default:
				break;
		}
	}
}
