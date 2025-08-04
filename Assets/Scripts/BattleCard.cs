using UnityEngine;
using UnityEngine.EventSystems;

public class BattleCard : MonoBehaviour, IPointerDownHandler
{
	public CardState cardState = CardState.inHand;
	public int playerId;

	int attackCount = 0;
	
	public void OnPointerDown(PointerEventData eventData)
	{
		if (this.gameObject.GetComponent<CardDisplay>().card is BiologyCard)
		{
			// 在手牌中
			if (cardState == CardState.inHand)
			{
				// 將 int 轉為 Enum
				BattleManager.instance.SummonRequest((PlayerType)playerId, this.gameObject);
			}
			// 在場上
			else if (cardState == CardState.inArea)
			{
				BattleManager.instance.AttackRequest((PlayerType)playerId, this.gameObject);
			}
		}
	}

	public int SetAttackCount(int atkCount)
	{
		attackCount = atkCount;
		return attackCount;
	}
}
