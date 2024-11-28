using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
// 這是事件工具用的東西
using UnityEngine.Events;

/// <summary>
/// 這是放置遊戲用的計時器，在玩家下線再上線之後將取得下線期間的秒數。
/// </summary>
public class OpenTime : MonoBehaviour
{
    // Unity事件
    [SerializeField] UnityEvent<float> how_long_have_you_slept;
    [SerializeField] bool never_kill_me = false;

    private void Start()
    {
        if (never_kill_me)
        {
            // 確保這個物件不會在切換關卡時被移除，但是使用這個功能要謹慎，在重複載入這個關卡時會造成分身讓計算錯誤，不建議啟動。
            DontDestroyOnLoad(this.gameObject);
        }

        // 檢查是否有紀錄 如果沒有表示玩家是第一次上線，不需要處理下線在上線的放置時間。
        if (PlayerPrefs.GetString("CLOSE_GAME_TIME", "") != "")
        {
            // 當前時間(別忘了這是初始化) - 上次關閉時間 就可以求出間距了
            TimeSpan timeSpan = DateTime.Now.Subtract(Aye.GetTimeByString(PlayerPrefs.GetString("CLOSE_GAME_TIME")));
            // 發出事件讓大家計算玩家共睡了幾秒 要拿到什麼等等
            how_long_have_you_slept.Invoke((float)timeSpan.TotalSeconds);
        }
    }
    private void OnDestroy()
    {
        // 當這個物件被移除時紀錄下線時間
        PlayerPrefs.SetString("CLOSE_GAME_TIME", Aye.GetStringByDateTime(DateTime.Now));
    }
}
