using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 好看的逐字顯示器
/// </summary>
public class VeryGoodText : MonoBehaviour
{
    private void Reset()
    {
        accountSayText = this.gameObject.GetComponent<Text>();
    }
    public string text
    {
        get { return accountSayText.text; }
    }
    [SerializeField] Text accountSayText = null;
    string tempAccountSayText = "";
    [Header("最多顯示幾行")]
    [SerializeField] int maxCanAccountSayTextLine = 4;
    int nowCanAccountSayTextLine = 0;
    [SerializeField] float speed = 0.05f;
    [Header("不換行")]
    [SerializeField] bool isOneLine = false;
    [Header("初始化時自動顯示內容")]
    [Multiline(4)]
    [SerializeField] string autoAttMe = "";
    private void Start()
    {
        if (autoAttMe != "")
            Add(autoAttMe);
    }
    /// <summary>刪除東西之後添加一行</summary>
    public void CleanAdd(string v)
    {
        Clean();
        Add(v);
    }
    /// <summary>全刪了</summary>
    public void Clean()
    {
        nowCanAccountSayTextLine = 0;
        tempAccountSayText = "";
        accountSayText.text = "";
    }
    /// <summary>添加內容 一次一行</summary>
    public void Add(string v)
    {
        if (v.Contains("\n"))
        {
            // 文章處理
            string[] alls = v.Split('\n');
            for (int i = 0; i < alls.Length; i++)
            {
                if (alls[i] == "")
                    continue;
                if (isOneLine)
                    tempAccountSayText = tempAccountSayText + alls[i];
                else
                    tempAccountSayText = tempAccountSayText + alls[i] + "\n";
            }
        }
        else
        {
            // 單行處理
            if (isOneLine)
                tempAccountSayText = tempAccountSayText + v;
            else
                tempAccountSayText = tempAccountSayText + v + "\n";
        }
    }
    float nextUpdateAccountSayTextTime = 0f;
    bool jumpTempAccountSayText = false;
    [SerializeField] bool isRun = true;
    public void Stop()
    {
        isRun = false;
    }
    public void Play()
    {
        isRun = true;
    }
    /// <summary>是否在運作</summary>
    public bool isMove
    {
        get { return tempAccountSayText.Length > 0; }
    }
    void Update()
    {
        if (!isRun)
            return;

        if (Time.time > nextUpdateAccountSayTextTime || jumpTempAccountSayText)
        {
            nextUpdateAccountSayTextTime = Time.time + speed;
            if (tempAccountSayText.Length > 0)
            {
                // 移除多的行數
                if (tempAccountSayText[0] == '\n')
                {
                    nowCanAccountSayTextLine++;
                    if (nowCanAccountSayTextLine > maxCanAccountSayTextLine)
                    {
                        string all = accountSayText.text;
                        string[] alls = all.Split('\n');
                        string done = "";
                        for (int i = 1; i < alls.Length; i++)
                        {
                            if (i < alls.Length - 1)
                                done = done + alls[i] + "\n";
                            else
                                done = done + alls[i];
                        }
                        accountSayText.text = done;
                        nowCanAccountSayTextLine--;
                    }
                }
                // 遇到括弧加速
                if (tempAccountSayText[0] == '<')
                    jumpTempAccountSayText = true;
                if (tempAccountSayText[0] == '>')
                    jumpTempAccountSayText = false;
                // 添加字
                accountSayText.text = accountSayText.text + tempAccountSayText[0].ToString();
                tempAccountSayText = tempAccountSayText.Remove(0, 1);
            }
        }
        accountSayText.supportRichText = !isMove;
    }
}
