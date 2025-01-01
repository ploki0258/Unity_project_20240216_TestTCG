using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Jack/Add New Card")]
public class CardData : ScriptableObject
{
	// �d�P��
	[Tooltip("����A�B���୫��")]
	[Header("�d���s��")] public int id;
	[Header("�d���W��")] public string cardName;
	[Header("�d������")] public CardType cardType;
	[Header("�d���}����")] public CardRare cardRare;
	[Header("�d���O��")] public int cardCost;
	[Header("�d���ϥ�")] public Sprite cardIcon;
	[Header("�d�ϭI��")] public Sprite cardBG;
	[Header("�d���y�z")][TextArea(3, 5)] public string cardDescription;
	// �ͪ��d�M��
	[Tooltip("�ͪ��d��")]
	[Header("�����O")] public int attack;
	[Header("�ͩR��")] public int health;
	[Header("�d���ݩ�")] public string[] cardFeature;
}
