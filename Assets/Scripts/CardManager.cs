using UnityEngine;

public class CardManager : MonoBehaviour
{
	[Header("卡牌預製物")][SerializeField] GameObject cardPrefab = null;
	[Header("卡牌最大數量")][SerializeField] int maxCardCount;
	[Header("卡牌起始位置")][SerializeField] Vector3 firstPos;
}

/// <summary>
/// 卡牌效果
/// </summary>
public interface ICardEffect
{
	public int ChangeAttack();
	public int ChangeHp();
}
