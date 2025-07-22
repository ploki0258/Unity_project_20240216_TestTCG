using UnityEngine;

/// <summary>
/// 商店管理器
/// </summary>
public class StoreManager : MonoBehaviour
{
	public static StoreManager instance;

	public TextAsset playerData;

	private void Awake()
	{
		instance = this;
	}

	void Start()
	{
		//CardStore.instance.LoadCardData(); // 從腳本化物件載入卡牌資料
		//PlayerDataManager.instance.LoadPlayerData(); // 載入玩家資料
	}
}
