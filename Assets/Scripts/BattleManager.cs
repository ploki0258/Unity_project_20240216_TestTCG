using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

/// <summary>
/// 回合管理器
/// </summary>
public class BattleManager : MonoBehaviour
{
	#region 單例模式
	public static BattleManager instance;
	#endregion

	#region 欄位
	[SerializeField, Header("玩家手牌區")] Transform playerHand = null; // 玩家手牌位置
	[SerializeField, Header("對手手牌區")] Transform enemyHand = null; // 敌人手牌位置
	[SerializeField, Header("玩家區域")] GameObject[] playerAreas = null; // 玩家區域
	[SerializeField, Header("對手區域")] GameObject[] enemyAreas = null; // 敵人區域
	[SerializeField, Header("卡牌預製物")] GameObject cardPrefab = null; // 卡牌預製物
	[SerializeField, Header("起始手牌張數"), Range(0, 7)] int startHandCount = 5; // 起始手牌數量
	[SerializeField, Header("是否啟用手牌上限")] bool isHandMax = true; // 是否啟用手牌上限
	[SerializeField, Header("手牌上限"), Range(0, 10)] int maxHandCount = 9; // 手牌上限
	[SerializeField, Header("能量上限"), Range(0, 20)] int maxEnergy = 10;  // 最大能量
	[SerializeField, Header("最大召喚次數")] int[] summonCountMax = new int[2] { 2, 2 };
	[SerializeField, Header("攻擊箭頭物件")] GameObject attackingArrow = null;

	public List<Card> playerDeckList = new List<Card>(); // 玩家牌組列表
	public List<Card> enemyDeckList = new List<Card>(); // 敵人牌組列表
	public List<Card> shuffletDeck = new List<Card>();

	int[] summonCount = new int[2];
	bool isCanSummon = false;
	GameObject waitingCard = null;
	PlayerType waitingPlayer = PlayerType.player;
	GameObject attackingCard = null;
	#endregion

	SimpleDeckShufflet simpleDeck = new SimpleDeckShufflet(); // 使用簡單的隨機洗牌算法
	FisherYatesDeckShufflet yatesDeckShufflet = new FisherYatesDeckShufflet(); // 使用Fisher-Yates洗牌算法

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		PlayerDataManager.instance.LoadEnemyData(); // 載入對手資料
		GameInit();
	}

	// 遊戲流程
	// 遊戲開始：載入數據 牌組洗牌 抽起始手牌
	// 回合階段

	/// <summary>
	/// 初始化遊戲：
	/// 1. 載入數據
	/// 2. 牌組洗牌
	/// 3. 抽起始手牌
	/// </summary>
	void GameInit()
	{
		// 讀取數據
		LoadDeckData();
		// 牌組洗牌
		ShuffletDeck(PlayerType.player, simpleDeck);
		ShuffletDeck(PlayerType.enemy, yatesDeckShufflet);
		// 抽起始手牌
		DrawCard(PlayerType.player, startHandCount);    // 玩家抽5張卡
		DrawCard(PlayerType.enemy, startHandCount);     // 對手抽5張卡
		currentPhase = PhaseType.playerDraw;            // 設置當前階段為玩家抽牌階段
	}

	/// <summary>
	/// 設置能量為最大值
	/// </summary>
	void SetEnergyFull()
	{
		energy = maxInitEnergy; // 設置能量為現有最大值
		Debug.Log($"能量：{energy}");
	}

	/// <summary>
	/// 增加能量最大值
	/// </summary>
	/// <param name="value">增加的能量</param>
	/// <param name="energyUpperLimit">能量上限</param>
	void IncreaseEnergyMax(int value, int energyUpperLimit)
	{
		// 如果能量最大值已達上限 則不增加
		if (maxInitEnergy >= energyUpperLimit)
			return;
		maxInitEnergy += value; // 增加能量最大值
		Debug.Log($"能量上限：{maxInitEnergy}");
	}

	/// <summary>
	/// 設置召喚次數
	/// </summary>
	void SetSummonCounter()
	{
		for (int i = 0; i < summonCountMax.Length; i++)
		{
			summonCount[i] = summonCountMax[i];
		}
	}

	/// <summary>
	/// 載入牌組數據
	/// </summary>
	void LoadDeckData()
	{
		// 載入玩家牌組數據
		LoadDeck(PlayerType.player);
		/*for (int i = 0; i < PlayerDataManager.instance.playerDatas.playerDecks.Length; i++)
		{
			if (PlayerDataManager.instance.playerDatas.playerDecks[i] != 0)
			{
				int count = PlayerDataManager.instance.playerDatas.playerDecks[i];
				for (int j = 0; j < count; j++)
				{
					playerDeckList.Add(CardStore.instance.CopyCard(i));
				}
			}
		}*/

		// 載入對手牌組數據
		LoadDeck(PlayerType.enemy);
		/*for (int i = 0; i < PlayerDataManager.instance.enemyDatas.enemyDecks.Length; i++)
		{
			if (PlayerDataManager.instance.enemyDatas.enemyDecks[i] != 0)
			{
				int count = PlayerDataManager.instance.enemyDatas.enemyDecks[i];
				for (int j = 0; j < count; j++)
				{
					enemyDeckList.Add(CardStore.instance.CopyCard(i));
				}
			}
		}*/
	}

	void LoadDeck(PlayerType playerType)
	{
		int[] deck = null;
		List<Card> deckList = new List<Card>();

		switch (playerType)
		{
			case PlayerType.player:
				deck = PlayerDataManager.instance.playerDatas.playerDecks; // 獲取玩家牌組數據
				deckList = playerDeckList; // 玩家牌組列表
				break;
			case PlayerType.enemy:
				deck = PlayerDataManager.instance.enemyDatas.enemyDecks; // 獲取對手牌組數據
				deckList = enemyDeckList; // 對手牌組列表
				break;
		}

		for (int i = 0; i < deck.Length; i++)
		{
			if (deck[i] != 0)
			{
				int count = deck[i];
				for (int j = 0; j < count; j++)
				{
					deckList.Add(CardStore.instance.CopyCard(i));
				}
			}
		}
	}

	/// <summary>
	/// 牌組洗牌
	/// </summary>
	/// <param name="playerType">玩家類型</param>
	void ShuffletDeck(PlayerType playerType)
	{
		List<Card> shuffletDeck = new List<Card>();
		switch (playerType)
		{
			case PlayerType.player:
				shuffletDeck = playerDeckList;
				break;
			case PlayerType.enemy:
				shuffletDeck = enemyDeckList;
				break;
		}
		// 洗牌邏輯
		shuffletDeck = shuffletDeck.OrderBy(random => UnityEngine.Random.Range(0, 999)).ToList();
	}

	/// <summary>
	/// 牌組洗牌
	/// </summary>
	/// <param name="playerType">玩家類型</param>
	/// <param name="shufflet">洗牌方式</param>
	void ShuffletDeck(PlayerType playerType, IDeckShufflet shufflet)
	{
		shuffletDeck = new List<Card>();
		// 使用接口方法進行洗牌
		switch (playerType)
		{
			case PlayerType.player:
				//shuffletDeck = playerDeckList;
				playerDeckList = shufflet.ShuffletDeck(playerDeckList);
				break;
			case PlayerType.enemy:
				//shuffletDeck = enemyDeckList;
				enemyDeckList = shufflet.ShuffletDeck(enemyDeckList);
				break;
		}
	}

	/// <summary>
	/// 抽取卡牌
	/// </summary>
	/// <param name="player">玩家類型</param>
	/// <param name="drawCount">抽取張數</param>
	void DrawCard(PlayerType player, int drawCount)
	{
		List<Card> drawDeck = new List<Card>();
		Transform hand = this.transform;

		switch (player)
		{
			case PlayerType.player:
				drawDeck = playerDeckList;
				hand = playerHand;
				break;
			case PlayerType.enemy:
				drawDeck = enemyDeckList;
				hand = enemyHand;
				break;
		}

		for (int i = 0; i < drawCount; i++)
		{
			// 當牌組數量 大於 所抽張數時 則抽取相應張數
			// 否則牌組數量 小於 所抽張數時 則抽取剩餘張數
			GameObject card = Instantiate(cardPrefab, hand); // 實例化卡牌
			card.GetComponent<CardDisplay>().card = CardStore.instance.CopyCard(drawDeck[0].id); // 抽取第一張卡牌
			card.GetComponent<BattleCard>().playerId = (int)player;
			card.GetComponent<BattleCard>().cardState = CardState.inHand;
			drawDeck.RemoveAt(0); // 從牌組中移除

			// 盡可能抽取剩餘的卡牌
			if (drawDeck.Count == 0)
				break; // 如果沒有卡牌可抽，則退出循環
		}
	}

	public void OnPlayerDraw()
	{
		if (currentPhase == PhaseType.playerDraw)
		{
			DrawPhase();
			currentPhase = PhaseType.playerMain; // 進入玩家主階段
		}
	}

	public void OnEnemyDraw()
	{
		if (currentPhase == PhaseType.enemyDraw)
		{
			DrawPhase();
			currentPhase = PhaseType.enemyMain; // 進入對手主階段
		}
	}

	public void OnClickTurnEnd()
	{
		TurnEnd();
	}

	void TurnEnd()
	{
		if (currentPhase == PhaseType.playerMain)
		{
			currentPhase = PhaseType.playerEnd; // 玩家結束主要階段，進入玩家結束階段
		}
		else if (currentPhase == PhaseType.enemyMain)
		{
			currentPhase = PhaseType.enemyEnd; // 對手結束主要階段，進入對手結束階段
		}
		StartCoroutine(TurnChange());
	}

	// 待處理 回合交換
	IEnumerator TurnChange()
	{
		if (currentPhase == PhaseType.playerEnd)
		{
			StartCoroutine(HandCheckEnd()); // 玩家進入結束階段，檢查手牌數量
			yield return new WaitForSeconds(1); // 等待秒數
			currentPhase = PhaseType.enemyDraw; // 玩家進入結束階段，進入對手抽牌階段
		}
		else if (currentPhase == PhaseType.enemyEnd)
		{
			yield return new WaitForSeconds(1); // 等待秒數
			currentPhase = PhaseType.playerDraw; // 對手進入結束階段，進入玩家抽牌階段
		}
	}

	void DrawPhase()
	{
		// 在抽牌階段時 進行抽牌的處理
		if (currentPhase == PhaseType.playerDraw)
		{
			DrawCard(PlayerType.player, 1); // 玩家抽一張卡牌
											// 在抽牌之後 進行能量的處理
		}
		else if (currentPhase == PhaseType.enemyDraw)
		{
			DrawCard(PlayerType.enemy, 1); // 對手抽一張卡牌
		}
	}

	// 結束階段時 進行手牌上限的檢查 部份效果的處理與結算etc.
	IEnumerator HandCheckEnd()
	{
		// 在結束階段時 檢查手牌數量是否超過上限
		if (isHandMax) // 如果啟用手牌上限
		{
			List<Card> hand = new List<Card>();
			switch (currentPhase)
			{
				case PhaseType.playerEnd:
					hand = playerHand.GetComponentsInChildren<CardDisplay>().Select(cd => cd.card).ToList(); // 獲取玩家手牌
					break;
				case PhaseType.enemyEnd:
					hand = enemyHand.GetComponentsInChildren<CardDisplay>().Select(cd => cd.card).ToList(); // 獲取對手手牌
					break;
			}
			if (hand.Count > maxHandCount) // 如果手牌數量超過上限
			{
				int excessCount = hand.Count - maxHandCount; // 計算超出上限的張數
				Debug.Log($"手牌數量超過上限，需捨棄 {excessCount} 張手牌");
				// 選擇要丟棄的手牌
				// 等待玩家選擇要丟棄的手牌
				// 如果手牌沒有小於上限的話 就一直等待 直至手牌小於上限
				while (excessCount > 0)
				{
					// 這裡可以添加UI提示讓玩家選擇要丟棄的手牌
					// 假設玩家選擇了要丟棄的手牌，並且已經處理了丟棄邏輯
					// 例如：hand.Remove(selectedCard);
					excessCount--; // 模擬丟棄一張手牌
					yield return new WaitForSeconds(0.1f);
				}
			}
			else
			{
				Debug.Log("手牌數量未超過上限，無需捨棄");
			}
		}

	}

	void NextPhase()
	{
		if ((int)currentPhase == System.Enum.GetNames(currentPhase.GetType()).Length - 1)
		{
			currentPhase = PhaseType.playerDraw;
		}
		else
		{
			currentPhase += 1;
		}
	}

	#region Summon 召喚
	/// <summary>
	/// 召喚請求
	/// </summary>
	/// <param name="playerType">玩家類型</param>
	/// <param name="card">要召喚的卡牌物件</param>
	public void SummonRequest(PlayerType playerType, GameObject card)
	{
		if (isCanSummon == false)
			return;

		GameObject[] blocks = null;
		bool isEmptyBlock = false;

		if (playerType == PlayerType.player && currentPhase == PhaseType.playerMain)
		{
			blocks = playerAreas;
		}
		else if (playerType == PlayerType.enemy && currentPhase == PhaseType.enemyMain)
		{
			blocks = enemyAreas;
		}
		else
		{
			return;
		}

		// 召喚次數 大於 0
		if (summonCount[(int)playerType] > 0)
		{
			foreach (GameObject blockItem in blocks)
			{
				if (blockItem.GetComponent<Block>().card == null)
				{
					Block tempBlock = blockItem.GetComponent<Block>();
					tempBlock.highlightBlock.SetActive(true);
					tempBlock.aniCard.SetBool("highlight", true);

					isEmptyBlock = true;
				}
			}
		}
		// 若場上的格子為空
		if (isEmptyBlock)
		{
			waitingCard = card;
			waitingPlayer = playerType;
		}
	}

	public void SummonConfirm(Transform block)
	{
		Summon(waitingPlayer, waitingCard, block);

		GameObject[] blocks = null;
		switch (waitingPlayer)
		{
			case PlayerType.player:
				blocks = playerAreas;
				break;
			case PlayerType.enemy:
				blocks = enemyAreas;
				break;
		}

		foreach (GameObject blockItem in blocks)
		{
			// 召喚確認後 關閉所有召喚相關的顯示
			Block tempBlock = blockItem.GetComponent<Block>();
			tempBlock.highlightBlock.SetActive(false);
			tempBlock.aniCard.SetBool("highlight", false);
		}
	}

	private void Summon(PlayerType playerType, GameObject card, Transform block)
	{
		card.transform.SetParent(block);
		card.transform.localPosition = Vector3.zero;
		card.transform.GetComponent<BattleCard>().cardState = CardState.inArea;
		int atk = card.transform.GetComponent<BattleCard>().SetAttackCount(1);
		block.GetComponent<Block>().card = card;
		summonCount[(int)playerType]--;
	}
	#endregion

	public void AttackRequest(PlayerType playerType, GameObject card)
	{
		GameObject[] blocks = null;
		bool isCardBlock = false;

		if (playerType == PlayerType.player && currentPhase == PhaseType.playerMain)
		{
			blocks = enemyAreas;
		}
		else if (playerType == PlayerType.enemy && currentPhase == PhaseType.enemyMain)
		{
			blocks = playerAreas;
		}
		else
		{
			return;
		}

		foreach (GameObject blockItem in blocks)
		{
			if (blockItem.GetComponent<Block>().card != null)
			{
				Block tempBlock = blockItem.GetComponent<Block>();

				isCardBlock = true;
			}
		}

		if (isCardBlock)
		{
			attackingCard = card;
			// 產生箭頭
			GameObject arrow = Instantiate(attackingArrow, card.transform.position, card.transform.rotation);
			arrow.GetComponent<Arrow>().SetStartPoint(attackingCard.transform.position);
		}
	}

	public void AttackConfirm(GameObject target)
	{

	}

	void Attack(GameObject card, GameObject enemyCard)
	{
		Card card1 = card.GetComponent<CardDisplay>().card;
		BiologyCard biologyPlayer = card1 as BiologyCard;

		Card card2 = enemyCard.GetComponent<CardDisplay>().card;
		BiologyCard biologyEnemy = card2 as BiologyCard;

		biologyPlayer.health -= biologyEnemy.attack;
		biologyEnemy.health -= biologyPlayer.attack;
	}

	// 當前階段
	#region 階段
	public PhaseType currentPhase
	{
		get => _currentPhase;
		set
		{
			_currentPhase = value;
			isCanSummon = value == PhaseType.playerMain || value == PhaseType.enemyMain ? true : false;

			switch (value)
			{
				case PhaseType.none:
					break;
				case PhaseType.gameInit:
					break;
				case PhaseType.playerDraw:
					SetSummonCounter();
					IncreaseEnergyMax(1, maxEnergy);  // 初始能量增加1
					SetEnergyFull();    // 能量恢復到最大值
					break;
				case PhaseType.playerMain:
					break;
				case PhaseType.playerEnd:
					break;
				case PhaseType.enemyDraw:
					break;
				case PhaseType.enemyMain:
					break;
				case PhaseType.enemyEnd:
					break;
			}

			onPhaseChange?.Invoke(); // 觸發階段變化事件
		}
	}
	PhaseType _currentPhase = PhaseType.none;
	public Action onPhaseChange; // 階段變化事件
	#endregion

	// 能量
	#region 能量
	public int energy   // 當前能量
	{
		get => _energy; // 獲取當前能量
		set
		{
			_energy = Mathf.Clamp(value, 0, maxEnergy); // 限制能量在0到最大能量之間
			onEnergyChange?.Invoke(); // 觸發能量變化事件
		}
	}
	int _energy = 0;
	public int maxInitEnergy    // 最大能量初始值
	{
		get => _maxInitEnergy;
		set
		{
			_maxInitEnergy = Mathf.Clamp(value, 0, maxEnergy); // 限制最大能量初始值在0到最大能量之間
			onEnergyChange?.Invoke(); // 觸發能量變化事件
		}
	}
	int _maxInitEnergy = 0;
	public Action onEnergyChange; // 能量變化事件
	#endregion
}

/// <summary>
/// 階段類型
/// </summary>
public enum PhaseType
{
	/*	回合階段可以有 主要階段含有戰鬥(爐石、SV、OPCG...) 或不含戰鬥(YGO...)
	 */
	none,           // 無階段
	gameInit,       // 遊戲初始化階段
	playerDraw,     // 玩家抽牌階段
	playerMain,     // 玩家主階段
					//playerBattle,	// 玩家戰鬥階段
	playerEnd,      // 玩家結束階段
	enemyDraw,      // 敵人抽牌階段
	enemyMain,      // 敵人主階段
					//enemyBattle,	// 敵人戰鬥階段
	enemyEnd        // 敵人結束階段
}

/// <summary>
/// 玩家類型
/// </summary>
public enum PlayerType
{
	player = 0, // 玩家
	enemy = 1 // 對手
}

public interface IDeckShufflet
{
	/// <summary>
	/// 洗牌
	/// </summary>
	public List<Card> ShuffletDeck(List<Card> deck);
}

public interface IHandScheduling
{
	/// <summary>
	/// 手牌調度
	/// </summary>
	/// <param name="selectHand">所選擇的手牌</param>
	/// <param name="deck">牌組</param>
	/// <param name="discard">棄牌區</param>
	public void HandScheduling(List<Card> hand, List<Card> selectHand, List<Card> deck, List<Card> discard = null);
}

public class SimpleDeckShufflet : IDeckShufflet
{
	List<Card> shuffletDeck = new List<Card>();

	/// <summary>
	/// 使用簡單的隨機洗牌算法
	/// </summary>
	public List<Card> ShuffletDeck(List<Card> shuffletDeck)
	{
		this.shuffletDeck = shuffletDeck.OrderBy(random => UnityEngine.Random.Range(0, 999)).ToList();
		Debug.Log($"使用簡單的隨機洗牌算法，洗牌後的卡牌數量：{this.shuffletDeck.Count}\n{this.shuffletDeck[0].cardName} {this.shuffletDeck[1].cardName} {this.shuffletDeck[2].cardName}");
		return this.shuffletDeck;
	}
}

public class FisherYatesDeckShufflet : IDeckShufflet
{
	/// <summary>
	/// 使用Fisher-Yates洗牌算法
	/// </summary>
	public List<Card> ShuffletDeck(List<Card> shuffletDeck)
	{
		for (int i = 0; i < shuffletDeck.Count; i++)
		{
			int randomIndex = UnityEngine.Random.Range(i, shuffletDeck.Count);
			Card temp = shuffletDeck[i]; // 暫存
			shuffletDeck[i] = shuffletDeck[randomIndex];
			shuffletDeck[randomIndex] = temp;
		}
		Debug.Log($"使用Fisher-Yates洗牌算法，洗牌後的卡牌數量：{shuffletDeck.Count}\n{shuffletDeck[0].cardName} {shuffletDeck[1].cardName} {shuffletDeck[2].cardName}");
		return shuffletDeck;
	}
}

public class HandSchedulingBySample : IHandScheduling
{
	// 手牌上限
	#region 手牌上限
	int maxHandCount
	{
		get => _maxHandCount;
		set
		{
			_maxHandCount = Mathf.Clamp(value, 0, _maxHandCount); // 限制手牌上限在0到上限本身
		}
	}
	int _maxHandCount;
	#endregion

	public HandSchedulingBySample(int maxHandCount = 9)
	{
		this.maxHandCount = maxHandCount; // 設定手牌上限
	}

	public void HandScheduling(List<Card> hand, List<Card> selectHand, List<Card> deck, List<Card> discard = null)
	{
		// 從現有手牌中選擇任意張手牌 將選擇的手牌返回牌組中洗牌後 再從牌組中抽出與返回牌組的張數相同的手牌
		int selectedCount = selectHand.Count; // 選擇的手牌張數
		foreach (Card card in selectHand)
		{
			hand.Remove(card);   // 從手牌中移除選擇的手牌
			deck.Add(card);      // 將選擇的手牌返回牌組中
		}

		SimpleDeckShufflet shufflet = new SimpleDeckShufflet();
		shufflet.ShuffletDeck(deck); // 洗牌

		for (int i = 0; i < selectedCount; i++)
		{
			if (deck.Count > 0) // 如果牌組還有卡牌
			{
				Card drawCard = deck[0]; // 抽取第一張卡牌
				deck.RemoveAt(0); // 從牌組中移除
				hand.Add(drawCard); // 添加到手牌中
			}
			else
			{
				// 如果沒有卡牌可抽，則退出循環
				Debug.LogWarning("牌組不夠抽啦！");
				break;
			}
		}
	}
}