using TMPro;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI hpText = null;

	private void Start()
	{
		BiologyCard biology = GetComponent<CardDisplay>().card as BiologyCard;
		biology.renewHealthChange += UpdateBiologyCardLifeText;
	}

	private void OnDisable()
	{
		BiologyCard biology = GetComponent<CardDisplay>().card as BiologyCard;
		biology.renewHealthChange -= UpdateBiologyCardLifeText;
	}

	void UpdateBiologyCardLifeText()
	{
		BiologyCard biology = GetComponent<CardDisplay>().card as BiologyCard;
		hpText.text = $"{biology.currentHealth}";
	}
}
