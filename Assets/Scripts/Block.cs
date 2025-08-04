using UnityEngine;
using UnityEngine.EventSystems;

public class Block : MonoBehaviour, IPointerDownHandler
{
	[SerializeField] public GameObject highlightBlock = null;
	[SerializeField] public Animator aniCard;

	public GameObject card;

	public void Reset()
	{
		aniCard = GetComponent<Animator>();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (highlightBlock.activeInHierarchy)
			BattleManager.instance.SummonConfirm(this.transform);
	}
}
