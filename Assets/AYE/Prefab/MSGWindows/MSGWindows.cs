using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class MSGWindows : Windows<MSGWindows>
{
    public bool autoClose = true; 
    /// <summary>簡單訊息</summary>
    public void Show(string info)
    {
        ShowOptions(info, null);
        SetOptionInfo(0, "OK", MSGWindowsButtonColor.Green);
    }
    /// <summary>簡單訊息 含確認事件</summary>
    public void ShowYES(string info, Action done)
    {
        ShowOptions(info, done);
        SetOptionInfo(0, "OK", MSGWindowsButtonColor.Green);
    }
    /// <summary>確認是否</summary>
    public void ShowYESNO(string info, Action no, Action yes)
    {
        ShowOptions(info, no, yes);
        SetOptionInfo(0, "取消", MSGWindowsButtonColor.White);
        SetOptionInfo(1, "確認", MSGWindowsButtonColor.Yellow);
    }
    [SerializeField] RectTransform bg = null;
    [SerializeField] GameObject button = null;
    List<MSGWindowsButton> buttons = new List<MSGWindowsButton>();
    [SerializeField] Text text = null;
    /// <summary>自訂選項</summary>
    public void ShowOptions(string info, params Action[] options)
    {
        for (int i = 0; i < buttons.Count;i++)
            Destroy(buttons[i].gameObject);
        buttons.Clear();
        if (options == null)
        {
            options = new Action[1];
            options[0] = null;
        }
        for (int i = 0; i < options.Length; i++)
        {
            GameObject temp = Instantiate(button, bg);
            temp.SetActive(true);
            MSGWindowsButton mSGWindowsButton = temp.GetComponent<MSGWindowsButton>();
            mSGWindowsButton.SetAction(options[i]);
            buttons.Add(mSGWindowsButton);
        }
        Open();
        this.text.text = info;
    }
    /// <summary>指定選項的外觀 請從0號開始填寫</summary>
    public void SetOptionInfo(int number, string info, MSGWindowsButtonColor colorType)
    {
        buttons[number].SetInfo(info, colorType);
    }
    protected override void Awake()
    {
        base.Awake();
        button.SetActive(false);
    }
    [SerializeField] VerticalLayoutGroup verticalLayoutGroup = null;
    public override void Open()
    {
        base.Open();
        verticalLayoutGroup.enabled = false;
        Invoke("TryVerticalLayoutGroup", 0.05f);
    }
    public override void Close()
    {
        base.Close();
    }
    void TryVerticalLayoutGroup()
    {
        verticalLayoutGroup.enabled = true;
    }
}
public enum MSGWindowsButtonColor
{
    White, Yellow, Green, Blue, Red
}
