using UnityEngine;

public class BasicCard : MonoBehaviour
{
	public ICardEffect cardEffect;
	GameObject card_bg = null;

	private void OnMouseDown()
	{
		if (card_bg.activeSelf)
			card_bg.SetActive(false);
		else
			card_bg.SetActive(true);
	}

	private void Awake()
	{
		//card_bg = GetComponent<GameObject>();
	}

	private void Start()
	{

	}
}
