using UnityEngine;
using UnityEngine.UI;

public class CardCounter : MonoBehaviour
{
	[SerializeField] Text countText;

	int count
	{
		get { return _count; }
		set
		{
			_count = value;
		}
	}
	int _count = 0;
	
	private void Start()
	{
		PlayerDataManager.instance.playerDatas.renewCardCountChange += RenewCardCount; // 訂閱牌數更新事件
	}

	private void OnDisable()
	{
		PlayerDataManager.instance.playerDatas.renewCardCountChange -= RenewCardCount; // 取消訂閱牌數更新事件
	}

	/// <summary>
	/// 更新卡牌數量顯示
	/// </summary>
	/// <param name="count">更新數量</param>
	public bool UpdateCardCount(int value)
	{
		count += value; // 更新當前卡牌數量
		RenewCardCount();

		if (count == 0)
		{
			Destroy(this.gameObject); // 如果卡牌數量為0，則銷毀該物件
			return true;
		}
		return false;
	}

	/// <summary>
	/// 更新玩家卡牌數量
	/// </summary>
	void RenewCardCount() => countText.text = count.ToString();
	
	/// <summary>
	/// 是否移除卡牌
	/// </summary>
	/// <returns></returns>
	bool IsRemoveCard()
	{
		if (count == 0)
		{
			Destroy(this.gameObject); // 如果卡牌數量為0，則銷毀該物件
			return true;
		}
		return false;
	}
}