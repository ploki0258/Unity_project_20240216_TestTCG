using TMPro;
using UnityEngine;

public class AttackDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI atkText = null;

	private void Start()
	{
		BiologyCard biology = GetComponent<CardDisplay>().card as BiologyCard;
		biology.renewAttackChange += UpdateBiologyCardAttackText;
	}

	private void OnDisable()
	{
		BiologyCard biology = GetComponent<CardDisplay>().card as BiologyCard;
		biology.renewAttackChange -= UpdateBiologyCardAttackText;
	}

	void UpdateBiologyCardAttackText()
	{
		BiologyCard biology = GetComponent<CardDisplay>().card as BiologyCard;
		atkText.text = $"{biology.attack}";
	} 
}
