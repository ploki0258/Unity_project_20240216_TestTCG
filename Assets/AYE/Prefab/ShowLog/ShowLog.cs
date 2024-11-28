using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowLog : MonoBehaviour
{
    public void ClearLog()
    {
        logStr = "";
    }
    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }
    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }
    string _logStr = "";
    public Text logText;
    [SerializeField] Toggle toggle = null;
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        logStr += "\n" + logString;
        // 如果遇到嚴重問題且不是暫停狀態就主動顯示細節並暫停
        if (type == LogType.Error && isStop == false && toggle.isOn == true)
        {
            logStr += "\n" + stackTrace;
            ButtonStop();
        }
    }
    public string logStr
    {
        get { return _logStr; }
        set
        {
            _logStr = value;
            logText.text = logStr;
        }
    }
    bool isStop = false;
    [SerializeField] Text buttonSay;
    public void ButtonStop()
    {
        if (isStop == true)
        {
            isStop = false;
            buttonSay.text = "暫停";
            Application.logMessageReceived += HandleLog;
        }
        else if (isStop == false)
        {
            isStop = true;
            buttonSay.text = "繼續";
            Application.logMessageReceived -= HandleLog;
        }
    }
}

// 2020 by 阿葉
