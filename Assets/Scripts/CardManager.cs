using UnityEngine;

public class CardManager : MonoBehaviour
{
	[Header("�d�P�w�s��")][SerializeField] GameObject cardPrefab = null;
	[Header("�d�P�̤j�ƶq")][SerializeField] int maxCardCount;
	[Header("�d�P�_�l��m")][SerializeField] Vector3 firstPos;
}

/// <summary>
/// �d�P�ĪG
/// </summary>
public interface ICardEffect
{
	public int ChangeAttack();
	public int ChangeHp();
}
