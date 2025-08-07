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
				BattleManager.instance.SummonRequest((PlayerType)playerId, this.gameObject, transform.position);
			}
			// 在場上
			else if (cardState == CardState.inArea)
			{
				if (attackCount > 0)
				{
					BattleManager.instance.AttackRequest((PlayerType)playerId, this.gameObject, transform.position);
				}
			}
		}
	}

	public void SetAttackCount(int atkCount)
	{
		attackCount = atkCount;
	}

	/// <summary>
	/// 消耗攻擊次數
	/// </summary>
	/// <param name="atkCount">次數</param>
	public void CostAttackCount(int atkCount)
	{
		attackCount -= atkCount;
	}
}
