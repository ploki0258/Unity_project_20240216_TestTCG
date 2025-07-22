using UnityEngine;

/// <summary>
/// 合成複用原則
/// </summary>
public class CompositeReusePrinciple : MonoBehaviour
{
	void Start()
	{
		Player player = new Player();
		Sword sword = new Sword();
		Bow bow = new Bow();
		// 使用武器攻擊
		player.weapon = sword;
		player.Attack(player.weapon); // 使用劍攻擊

		player.weapon = bow;
		player.Attack(player.weapon); // 使用弓箭攻擊
	}

	abstract class Weapon
	{
		public virtual void Attack()
		{

		}
	}

	// 劍
	class Sword : Weapon
	{
		public override void Attack()
		{
			base.Attack();
			Debug.Log($"<color=#f0f>Sword attack!</color>");
		}
	}

	// 弓箭
	class Bow : Weapon
	{
		public override void Attack()
		{
			base.Attack();
			Debug.Log($"<color=#f09>Bow attack!</color>");
		}
	}

	class Player
	{
		public Weapon weapon;

		public void Attack(Weapon weapon)
		{
			weapon.Attack();
		}
	}
}
