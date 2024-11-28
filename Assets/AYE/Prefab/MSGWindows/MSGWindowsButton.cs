using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MSGWindowsButton : MonoBehaviour
{
    Action doStuff = null;
    public void SetAction(Action doStuff)
    {
        this.doStuff = doStuff;
    }
    [SerializeField] Sprite White = null, Yellow = null, Green = null, Blue = null, Red = null;
    [SerializeField] Image myColor = null;
    [SerializeField] Text info = null;
    public void SetInfo(string info, MSGWindowsButtonColor colorType)
    {
        this.info.text = info;
        switch (colorType)
        {
            case MSGWindowsButtonColor.White:
                myColor.sprite= White;
                break;
            case MSGWindowsButtonColor.Yellow:
                myColor.sprite= Yellow;
                break;
            case MSGWindowsButtonColor.Green:
                myColor.sprite= Green;
                break; 
            case MSGWindowsButtonColor.Blue:
                myColor.sprite= Blue;
                break;
            case MSGWindowsButtonColor.Red:
                myColor.sprite= Red;
                break; 
           default:
                myColor.sprite = White;
                break;
        }
    }
    public void ButtonOn()
    {
        if (doStuff != null)
            doStuff.Invoke();
        if (MSGWindows.ins.autoClose)
            MSGWindows.ins.Close();
    }
}
