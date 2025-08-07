using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerDataManager
{
	#region 單例模式
	public static PlayerDataManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new PlayerDataManager();
			}
			return _instance;
		}
	}
	static PlayerDataManager _instance;
	#endregion

	public TextAsset playerData;
	public PlayerDatas playerDatas; // 玩家資料
	public EnemyDatas enemyDatas;   // 對手資料

	/// <summary>
	/// 載入玩家資料
	/// </summary>
	public void LoadPlayerData()
	{
		//string[] a = File.ReadAllLines(Application.dataPath + "/Datas/playerdata.csv");  // 讀取玩家資料文件
		string path = File.ReadAllText(Application.dataPath + "/Resources/PlayerData/playerdata.csv"); // 讀取玩家資料文件
		Debug.Log($"載入玩家資料：\n{path}");

		playerDatas = new PlayerDatas(); // 初始化玩家資料
		playerDatas.playerCards = new int[CardStore.instance.cardList.Count];   // 初始化玩家卡牌陣列
		playerDatas.playerDecks = new int[CardStore.instance.cardList.Count];   // 初始化玩家牌組陣列
		string[] dataRow = path.Trim().Split('\n');    // 解析玩家資料
		foreach (string row in dataRow)
		{
			string row1 = row.Trim(); // 去除行首尾空白
			string[] rowArray = row1.Split(',');
			if (rowArray[0] == "#")
				continue; // 跳過註解行
			else if (rowArray[0] == "coins")
			{
				// 解析玩家金幣數量
				playerDatas.playerCoins = int.Parse(rowArray[1].Trim());
			}
			else if (rowArray[0] == "card")
			{
				//Debug.Log("Length2:" + playerDatas.playerCards.Length);
				int id = int.Parse(rowArray[1].Trim());
				int cardCount = int.Parse(rowArray[2].Trim());
				if (playerDatas.playerCards == null)
				{
					playerDatas.playerCards = new int[CardStore.instance.cardDataList.Count];
				}
				// 檢查ID是否在範圍內
				if (id >= 0 && id < playerDatas.playerCards.Length)
				{
					// 儲存卡牌數量到玩家資料中
					playerDatas.playerCards[id] = cardCount;
				}
				else
				{
					Debug.Log($"玩家卡牌ID {id} 超出範圍，載入失敗。");
				}
			}
			else if (rowArray[0] == "deck")
			{
				int id = int.Parse(rowArray[1].Trim());
				int cardCount = int.Parse(rowArray[2].Trim());
				// 載入玩家牌組資料
				playerDatas.playerDecks[id] = cardCount;
			}
			else
			{
				Debug.Log($"未知的資料行：{row}");
			}
		}
	}

	/// <summary>
	/// 載入對手資料
	/// </summary>
	/// <param name="dataPath">對手資料路徑，其路徑為/Resources/EnemyData/底下的檔名.csv</param>
	public void LoadEnemyData(string dataPath = "/Resources/EnemyData/enemydata.csv")
	{
		string path = File.ReadAllText(Application.dataPath + dataPath);
		Debug.Log($"載入對手資料：\n{path}");

		enemyDatas = new EnemyDatas();
		enemyDatas.enemyCards = new int[CardStore.instance.cardList.Count]; // 初始化對手卡牌陣列
		enemyDatas.enemyDecks = new int[CardStore.instance.cardList.Count]; // 初始化對手牌組陣列

		string[] dataRow = path.Trim().Split('\n');    // 解析對手資料
		foreach (string row in dataRow)
		{
			string row1 = row.Trim(); // 去除行首尾空白
			string[] rowArray = row1.Split(',');
			if (rowArray[0] == "#")
				continue; // 跳過註解行
			else if (rowArray[0] == "card")
			{
				int id = int.Parse(rowArray[1].Trim());
				int cardCount = int.Parse(rowArray[2].Trim());
				if (enemyDatas.enemyCards == null)
				{
					enemyDatas.enemyCards = new int[CardStore.instance.cardDataList.Count];
				}
				// 檢查ID是否在範圍內
				if (id >= 0 && id < enemyDatas.enemyCards.Length)
				{
					// 儲存卡牌數量到對手資料中
					enemyDatas.enemyCards[id] = cardCount;
				}
				else
				{
					Debug.Log($"對手卡牌ID {id} 超出範圍，載入失敗。");
				}
			}
			else if (rowArray[0] == "deck")
			{
				int id = int.Parse(rowArray[1].Trim());
				int cardCount = int.Parse(rowArray[2].Trim());
				// 載入對手牌組資料
				enemyDatas.enemyDecks[id] = cardCount;
			}
			else
			{
				Debug.Log($"未知的資料行：{row}");
			}
		}
	}

	/// <summary>
	/// 儲存玩家資料
	/// </summary>
	public void SavePlayerData()
	{
		// 儲存玩家資料到Resources/playerdata.csv
		string path = Application.dataPath + "/Resources/playerdata.csv";
		Debug.Log($"儲存玩家資料到：{path}");
		List<string> datas = new List<string>();
		if (playerDatas != null)
		{
			// 儲存玩家金幣數量
			datas.Add($"coins, {playerDatas.playerCoins}");
			// 儲存玩家卡牌資料
			for (int i = 0; i < playerDatas.playerCards.Length; i++)
			{
				// 只儲存擁有的卡牌數量大於0的卡牌
				if (playerDatas.playerCards[i] != 0)
				{
					// 檢查ID是否在範圍內
					datas.Add($"card, {i}, {playerDatas.playerCards[i]}");
				}
			}
			// 儲存玩家牌組資料
			for (int i = 0; i < playerDatas.playerDecks.Length; i++)
			{
				if (playerDatas.playerDecks[i] != 0)
					datas.Add($"deck, {i}, {playerDatas.playerDecks[i]}");
			}
		}
		// 儲存資料到文件中
		File.WriteAllLines(path, datas);
		Debug.Log("儲存玩家資料...");
	}
}

/// <summary>
/// 玩家資料
/// </summary>
[System.Serializable]
public class PlayerDatas
{
	#region 玩家資料
	#region 卡牌
	public int[] playerCards
	{
		get { return _playerCards; }
		set
		{
			_playerCards = value;
			if (renewCardCountChange != null)
				renewCardCountChange.Invoke();
		}
	}
	[SerializeField] public int[] _playerCards; // 玩家擁有的卡牌ID陣列

	public int[] playerDecks
	{
		get { return _playerDecks; }
		set
		{
			for (int i = 0; i < _playerDecks.Length; i++)
			{
				_playerDecks[i] = value[i];
			}
			if (renewCardCountChange != null)
				renewCardCountChange.Invoke(); // 更新卡牌數量變更事件，傳遞新的牌組長度
		}
	}
	[SerializeField] public int[] _playerDecks = new int[CardStore.instance.cardList.Count]; // 玩家擁有的牌組陣列
	[Tooltip("卡牌數量更新事件")]
	public Action renewCardCountChange;
	#endregion

	#region 玩家金幣數量
	public int playerCoins
	{
		get { return _playerCoins; }
		set
		{
			if (value < 0)
			{
				Debug.Log("玩家金幣數量不能為負數，已重設為0。");
				_playerCoins = 0;
			}
			else
			{
				_playerCoins = value;
			}

			if (renewPlayerCoinsCallback != null)
				renewPlayerCoinsCallback();
		}
	}
	[SerializeField] int _playerCoins = 100; // 玩家金幣數量
	[SerializeField] public Action renewPlayerCoinsCallback; // 玩家金幣數量更新回調
	#endregion

	#region 玩家生命值
	public int playerLifePoint
	{
		get => _playerLifePoint;
		set
		{
			_playerLifePoint = Mathf.Clamp(value, 0, _playerLifePoint);

			if (renewPlayerLifePointChange != null)
				renewPlayerLifePointChange();
		}
	}
	[SerializeField] int _playerLifePoint = 100; // 玩家生命值
	[SerializeField] public Action renewPlayerLifePointChange; // 玩家生命值事件
	#endregion
	#endregion
}

/// <summary>
/// 對手資料
/// </summary>
[System.Serializable]
public class EnemyDatas
{
	#region 對手資料
	#region 卡牌
	public int[] enemyCards
	{
		get { return _enemyCards; }
		set { _enemyCards = value; }
	}
	[SerializeField] public int[] _enemyCards; // 敵人擁有的卡牌ID陣列

	public int[] enemyDecks
	{
		get { return _enemyDecks; }
		set { _enemyDecks = value; }
	}
	[SerializeField] public int[] _enemyDecks; // 敵人擁有的牌組陣列
	#endregion

	#region 對手生命值
	public int enemyLifePoint
	{
		get => _enemyLifePoint;
		set
		{
			_enemyLifePoint = Mathf.Clamp(value, 0, _enemyLifePoint);

			if (renewEnemyLifePointChange != null)
				renewEnemyLifePointChange();
		}
	}
	[SerializeField] int _enemyLifePoint = 100; // 對手生命值
	[SerializeField] public Action renewEnemyLifePointChange; // 對手生命值事件
	#endregion
	#endregion
}
