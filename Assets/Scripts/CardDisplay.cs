using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
	[Header("�d�P")] public Card[] cards;
	[Header("�ͪ��d")] public BiologyCard[] biologyCards;
	[Header("�k�N�d")] public SpellCard[] spellCards;
	[Header("�˳ƥd")] public EquipmentCard[] equipmentCards;
	[Header("�d�W")] public Text nameText;
	[Header("�O��")] public Text costText;
	[Header("�����O")] public Text atkText;
	[Header("��q")] public Text hpText;
	[Header("�ĪG��")] public Text descriptionText;
	[Header("�ϥ�")] public Image iconImage;
	[Header("�d�I")] public Image bgImage;
}
