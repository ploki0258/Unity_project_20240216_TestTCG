using UnityEngine;
using UnityEngine.EventSystems;

public class BattleCard : MonoBehaviour, IPointerDownHandler
{
	public CardState cardState = CardState.inHand;
	public int playerId;

	public void OnPointerDown(PointerEventData eventData)
	{
		if (this.gameObject.GetComponent<CardDisplay>().card is BiologyCard)
		{
			if (cardState == CardState.inHand)
			{
				// 將int 轉為 Enum
				BattleManager.instance.SummonRequest((PlayerType)playerId, this.gameObject);
			}
		}
	}
}
