using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Security.Cryptography;

public class SaySystem : MonoBehaviour
{
    public static SaySystem instance = null;
    public LanguageISO639_1 languageISO = LanguageISO639_1.zh_TW;
    private void Awake()
    {
        instance = this;
        SaySystemManager.instance.Load();
    }

    [SerializeField] SayStuff playOnStart = null;
    private void Start()
    {
        // 播放測試文件
        if (playOnStart != null)
        {
            SaySystem.instance.StartSay(playOnStart);
        }
    }
    [SerializeField, Header("當前列表")]
    List<SayStuffPakage> current = new List<SayStuffPakage>();
    public bool isPlay = false;

    /// <summary>開始對話</summary>
    public void StartSay(SayStuff sayStuff, Action<int> doneAction = null)
    {
        // 如果正在對話就忽略這個命令
        if (isPlay == true)
        {
            Debug.LogError("已經在對話了，不能加入新的命令。");
            return;
        }
        SayStuffPakage newSayStuffPakage = new SayStuffPakage(sayStuff, doneAction);
        // 取得文本
        current = new List<SayStuffPakage>
        {
            newSayStuffPakage
        };
        startSay.Invoke();
        // 開始異步執行對話
        StartCoroutine(對話());
    }

    [SerializeField] UnityEvent startSay = null;

    /// <summary>追加對話</summary>
    public void AddSay(SayStuff sayStuff, Action<int> doneAction = null)
    {
        if (isPlay == false)
        {
            Debug.LogError("StartSay之後才能加入新的命令。");
            return;
        }
        SayStuffPakage newSayStuffPakage = new SayStuffPakage(sayStuff, doneAction);
        current.Add(newSayStuffPakage);
    }

    [SerializeField] Animator anim = null;
    [SerializeField] Animator continueAnim = null;
    [SerializeField] Text titleText = null;
    [SerializeField] Text mainText = null;
    [SerializeField] Text titleTextDone = null;
    [SerializeField] Text mainTextDone = null;
    [SerializeField] Transform continueObj = null;
    [SerializeField] [Header("每個字要等待多久")] float speed = 0.05f;
    [SerializeField] RectTransform box1 = null, box2 = null, titleTextRect1 = null, titleTextRect2 = null, mainTextRect1 = null, mainTextRect2 = null;
    // 箭頭保留空間
    [SerializeField] [Header("繼續箭頭保留空間")] float arrowSpace = 40f;
    // 最小寬度
    [SerializeField] [Header("最小寬度")] float minWidth = 882f;
    [SerializeField] RectTransform optionBg = null;
    [SerializeField] GameObject optionPrefab = null;
    List<SaySystemOptions> saySystemOptions = new List<SaySystemOptions>();
    [SerializeField] UnityEvent bobobo = null;
    [SerializeField] UnityEvent endSay = null;
    IEnumerator 對話()
    {
        isPlay = true;
        bool optionsOut = false;
        bool isOptionsContinue = false;
        // 先不顯示內容 關閉提示

        titleText.text = "";
        mainText.text = "";
        titleTextDone.text = "";
        mainTextDone.text = "";
        titleText.gameObject.SetActive(true);
        titleTextDone.gameObject.SetActive(true);

        // 關閉右下角E提示
        continueObj.localScale = Vector3.zero;
        

        // 多對話表
        for (int k = 0; k < current.Count; k++)
        {
            // 對話總表
            for (int j = 0; j < current[k].sayStuff.list.Count; j++)
            {
                // 關閉右下角E提示
                continueObj.localScale = Vector3.zero;
                // 逐步顯示每一個字到畫面上
                string 最終顯示的內容 = "";
                canJump = true;
                jumping = false;
                string[] allLine = current[k].sayStuff.list[j].info.Trim().Split(',');

                // 如果有抬頭才啟用
                string title = current[k].sayStuff.list[j].title.Trim();
                titleText.gameObject.SetActive(title != "" && title != string.Empty);
                titleText.text = SaySystemManager.instance.GetText(languageISO, title);
                // 顯示原始對話作為背景
                for (int i = 0; i < allLine.Length; i++)
                {
                    string oneLineString = SaySystemManager.instance.GetText(languageISO, allLine[i]).Trim();
                    for(int i2 = 0; i2 < oneLineString.Length; i2++)
                    {
                        // 有幾個字就會跑幾圈
                        最終顯示的內容 = 最終顯示的內容 + oneLineString[i2];
                        // 顯示到畫面上
                        mainText.text = 最終顯示的內容;
                        // 每顯示一個字等待0.05秒
                        //yield return new WaitForSeconds(jumping ? 0.001f : speed);
                    }
                    // 如果不是最後一行
                    if (i != allLine.Length - 1)
                    {
                        // 顯示換行
                        最終顯示的內容 = 最終顯示的內容 + "\n";
                        // 顯示到畫面上
                        mainText.text = 最終顯示的內容;
                    }
                }

                yield return new WaitForEndOfFrame();

                // 寬度考慮箭頭 同事不能小於最小寬度
                float width = box1.sizeDelta.x + arrowSpace;
                if (width < minWidth)
                {
                    width = minWidth;
                }

                box2.sizeDelta = new Vector2(width, box1.sizeDelta.y);
                titleTextRect2.sizeDelta = titleTextRect1.sizeDelta;
                mainTextRect2.sizeDelta = mainTextRect1.sizeDelta;
                titleTextRect2.anchoredPosition = titleTextRect1.anchoredPosition;
                mainTextRect2.anchoredPosition = mainTextRect1.anchoredPosition;

                // 如果從選項中出來需要等待
                if (isOptionsContinue)
                {
                    isOptionsContinue = false;
                    titleTextDone.text = "";
                    mainTextDone.text = "";
                    yield return new WaitForSeconds(0.5f);
                }

                // 如果動畫未啟動先等動畫啟動
                if (anim.GetBool("Play") == false)
                {
                    // 啟動動畫
                    anim.SetBool("Play", true);
                    // 等待0.5秒
                    yield return new WaitForSeconds(0.5f);
                }

                // 顯示最終對話
                titleTextDone.gameObject.SetActive(titleText.gameObject.activeSelf);
                titleTextDone.text = titleText.text;
                mainTextDone.text = "";
                for (int i2 = 0; i2 < mainText.text.Length; i2++)
                {
                    // 顯示到畫面上
                    mainTextDone.text += mainText.text[i2];
                    // 每顯示一個字等待0.05秒
                    if (jumping == false)
                    {
                        bobobo.Invoke();
                        yield return new WaitForSeconds(speed);
                    }
                }

                // 顯示繼續提示 讓玩家按了繼續
                continueObj.localScale = Vector3.one;
                //continueAnim.SetTrigger("Update");
                isStop = true;
                canJump = false;
                // 如果還沒按E 就卡死在這邊等待
                while (needContinue == false)
                {
                    // 等待0.1秒
                    yield return new WaitForSeconds(0.1f);
                }
                isStop = false;
                needContinue = false;
            }

            // 如果到達最後一圈
            if (k >= current.Count-1)
            {
                // 沒有選項就回傳事件0
                if (current[k].sayStuff.opctionList.Count <= 0)
                {
                    current[k].doneAction?.Invoke(0);
                    // 跳出迴圈來結束系統
                    break;
                }
                else
                {
                    // 有選項時播放選項動畫
                    anim.SetTrigger("OpenOptions");
                    // 刪除舊的選項
                    for(int l = 0; l < saySystemOptions.Count; l++)
                    {
                        Destroy(saySystemOptions[l].root);
                    }
                    saySystemOptions.Clear();
                    // 顯示選項
                    optionPrefab.SetActive(true);
                    for(int m = 0; m < current[k].sayStuff.opctionList.Count; m++)
                    {
                        GameObject newOption = Instantiate(optionPrefab, optionBg);
                        SaySystemOptions saySystemOption = newOption.GetComponentInChildren<SaySystemOptions>();
                        saySystemOption.Set(SaySystemManager.instance.GetText(languageISO, current[k].sayStuff.opctionList[m]), current[k].doneAction, m, OpctionDone);
                        saySystemOptions.Add(saySystemOption);
                    } 
                    optionPrefab.SetActive(false);
                    // 等待動畫啟動完成
                    yield return new WaitForSeconds(0.5f);
                    // 等待輸入
                    isOpction = true;
                    while (isOpction)
                    {
                        yield return new WaitForSeconds(0.1f);
                    }
                    // 如果此時添加的對話
                    if (k < current.Count - 1)
                    {
                        anim.SetTrigger("OptionsContinue");
                        isOptionsContinue = true;
                    }
                    else
                    {
                        optionsOut = true;
                        break;
                    }
                }
            }
        }

        if (optionsOut == false)
        {
            // 所有的迴圈都真正結束了 關閉動畫並且等0.5秒後表示對話結束
            anim.SetBool("Play", false);
        }
        else
        {
            anim.SetBool("Play", false);
            anim.SetTrigger("OptionsOut");
        }
        endSay.Invoke();
        yield return new WaitForSeconds(0.5f);
        isPlay = false;
    }

    bool needContinue = false;
    bool isStop = false;
    bool canJump = false;
    bool jumping = false;
    bool isOpction = false;
    /// <summary>繼續對話的按鈕</summary>
    [SerializeField] [Header("按什麼按鈕繼續")] KeyCode continueKey = KeyCode.E;
    private void Update()
    {
        if (Input.GetKeyDown(continueKey) )
        {
            if (isStop && needContinue == false)
            {
                needContinue = true;
            }
            else if (canJump == true && jumping == false)
            {
                jumping = true;
            }
        }
    }
    void OpctionDone()
    {
        if (isOpction && isPlay)
        {
            isOpction = false;
        }
    }
    public struct SayStuffPakage
    {
        public SayStuff sayStuff;
        public Action<int> doneAction;
        public SayStuffPakage(SayStuff sayStuff, Action<int> doneAction)
        {
            this.sayStuff = sayStuff;
            this.doneAction = doneAction;
        }
    }
}