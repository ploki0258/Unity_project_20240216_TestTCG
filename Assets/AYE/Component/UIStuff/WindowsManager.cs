using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WindowsManager : MonoBehaviour
{
    public static WindowsManager instance
    {
        get
        {
            return _instance;
        }
    }
    static WindowsManager _instance = null;

    public static void Load()
    {
        if (_instance == null)
        {
            _instance = new GameObject("WindowsManager").AddComponent<WindowsManager>();
        }
    }
    List<int> windowsIDList = new List<int>();
    void AddID(int id)
    {
        for(int i = windowsIDList.Count-1; i >= 0; i--)
        {
            if (windowsIDList[i] == id)
                windowsIDList.RemoveAt(i);
        }
        windowsIDList.Add(id);
    }
    void RemoveID(int id)
    {
        for (int i = windowsIDList.Count - 1; i >= 0; i--)
        {
            if (windowsIDList[i] == id)
                windowsIDList.RemoveAt(i);
        }
    }
    public void OpenWindows(int id)
    {
        // 將目前開啟的物件放到最新
        AddID(id);
        if (Act_FirstWindows != null)
            Act_FirstWindows.Invoke(windowsIDList[windowsIDList.Count-1]);
        if (Act_WindowsCountChange != null)
            Act_WindowsCountChange.Invoke(windowsIDList.Count);
    }
    public void CloseWindows(int id)
    {
        // 從列表移除這個物件
        RemoveID(id);
        if (windowsIDList.Count >= 1 && Act_FirstWindows != null)
            Act_FirstWindows.Invoke(windowsIDList[windowsIDList.Count - 1]);
        if (Act_WindowsCountChange != null)
            Act_WindowsCountChange.Invoke(windowsIDList.Count);
    }
    /// <summary>視窗數量發生變化</summary>
    public System.Action<int> Act_WindowsCountChange = null;
    /// <summary>最上層的視窗改變</summary>
    public System.Action<int> Act_FirstWindows = null;
    /// <summary>視窗數量</summary>
    public int windowsCount
    {
        get { return windowsIDList.Count; }
    }
    /// <summary>關閉所有視窗</summary>
    public System.Action Act_CloseAllWindows = null;
    /// <summary>關閉所有視窗</summary>
    public void CloseAllWindows()
    {
        if (Act_CloseAllWindows != null)
            Act_CloseAllWindows.Invoke();
    }
}
