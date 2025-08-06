using UnityEngine;
using UnityEngine.EventSystems;

public class AttackTarget : MonoBehaviour, IPointerClickHandler, IAttackable
{
	public bool attackable;
	public AttackerType attacker = AttackerType.biology;

	bool isPlayer = false;
	CardDisplay cardDisplay;

	private void Start()
	{
		cardDisplay = GetComponent<CardDisplay>();
		isPlayer = cardDisplay == null;
		attacker = isPlayer ? AttackerType.player : AttackerType.biology;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (attackable)
		{
			switch (attacker)
			{
				case AttackerType.player:
					BattleManager.instance.AttackConfirm(this.gameObject);
					break;
				case AttackerType.biology:
					BattleManager.instance.AttackConfirm(this.gameObject);
					break;
			}
		}
	}

	public void TakeDamage(int damage)
	{
		BiologyCard biology = (BiologyCard)GetComponent<CardDisplay>().card;
		biology.currentHealth -= damage;

		if (biology.currentHealth <= 0)
			Dead();
	}

	/// <summary>
	/// 死亡
	/// </summary>
	void Dead()
	{
		Destroy(this.gameObject);
	}
}

/// <summary>
/// 被攻擊者類型
/// </summary>
public enum AttackerType
{
	player,     // 玩家
	biology,    // 生物
}

/// <summary>
/// 可被傷害
/// </summary>
public interface IAttackable
{
	/// <summary>
	/// 受傷
	/// </summary>
	/// <param name="damage">傷害量</param>
	public void TakeDamage(int damage);
}
