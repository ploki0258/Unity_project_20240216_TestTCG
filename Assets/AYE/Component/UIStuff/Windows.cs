using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// <br>好用的視窗底層，提供開啟關閉、淡入淡出、單利模式等功能。</br>
/// <br>canvasGroup : 淡入淡出與開關控制權的必要組件，需要掛上</br>
/// <br>speed : 控制淡入淡出的速度，預設為10</br>
/// <br>windowsTimeType : 決定這個介面開關是否要受到Time.timeScale影響</br>
/// <br>windowsTopType : 決定是否自動將開啟視窗推到上層顯示</br>
/// <br>Open() : 啟動視窗</br>
/// <br>Close() : 關閉視窗</br>
/// <br>OnOpen() : 淡入完畢徹底開啟視窗之後</br>
/// <br>OnClose() : 淡出完畢徹底關閉視窗之後</br>
/// <br>isOpen : 視窗是否開啟</br>
/// <br>isFirst : 是否為第一個視窗</br>
/// <br>isSaveFirst : 是否為第一個視窗按鈕判定用 避免同時關閉多個視窗</br>
/// <br>OnFirst : 成為第一個視窗時</br>
/// <br>OnNotFirst : 不是第一個視窗時</br>
/// </summary>
public abstract class Windows<T> : SingletonMonoBehaviour<T> where T : class
{
    /// <summary>初始化時自動打開</summary>
    [SerializeField] bool openOnStart = false;
    [ShowOnly] public bool isOpen = false;
    [ShowOnly] public bool isFirst = false;
    /// <summary>自動將開啟視窗推到上層</summary>
    public WindowsTopType windowsTopType = WindowsTopType.No;
    /// <summary>視窗的時間運作方式</summary>
    public WindowsTimeType windowsTimeType = WindowsTimeType.Time_unscaledDeltaTime;
    [Tooltip("淡入淡出與開關控制權的必要組件，需要掛上")]
    [SerializeField] CanvasGroup canvasGroup = null;

    /// <summary>淡入淡出速度</summary>
    [HideInInspector] public float speed = 10f;
    private void Reset()
    {
        GetCanvasGroup();
    }
    void GetCanvasGroup()
    {
        if (canvasGroup == null)
        {
            canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
            Debug.LogWarning("建議自行安裝CanvasGroup到Windows以達到啟動時最佳效能。", this.gameObject);
        }
        if (canvasGroup == null)
        {
            canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
        }
    }
    protected override void Awake()
    {
        WindowsManager.Load();
        base.Awake();
        GetCanvasGroup();
        WindowsManager.instance.Act_FirstWindows += FirstChange;
    }
    private void OnDisable()
    {
        if (WindowsManager.instance != null)
        {
            WindowsManager.instance.Act_FirstWindows -= FirstChange;
        }
    }
    string ogName = "";
    virtual protected void Start()
    {
        ogName = this.gameObject.name;
        canvasGroup.alpha = 0f;
        OnNotFirst();
        Close(true);
        // 自動開啟
        if (openOnStart)
            Open();
    }
    
    /// <summary>啟動介面</summary>
    virtual public void Open()
    {
        if (isOpen)
            return;
        if (windowsTopType == WindowsTopType.ToTop)
            transform.SetAsLastSibling();
        // 如果還沒執行上次關閉要做的事情就做
        if (isOnClose == false)
        {
            OnClose();
            isOnClose = true;
        }
        targetAlpha = 1f;
        isOnOpen = false;
        isOpen = true;
        canvasGroup.blocksRaycasts = true;
#if UNITY_EDITOR
        this.gameObject.name = ">>>>>>>" + ogName + "<<<<<<<";
#endif
        WindowsManager.instance.Act_CloseAllWindows += Close;
        WindowsManager.instance.OpenWindows(this.transform.GetInstanceID());
    }
    virtual public void Close()
    {
        Close(false);
    }
    /// <summary>關閉介面</summary>
    void Close(bool isStart)
    {
        if (isOpen == false && isStart == false)
            return;
        WindowsManager.instance.Act_CloseAllWindows -= Close;
        // 如果還沒執行開啟完畢要做的事情就做
        if (isOnOpen == false)
        {
            OnOpen();
            isOnOpen = true;
            isOnClose = false;
        }
        targetAlpha = 0f;
        isOpen = false;
        canvasGroup.blocksRaycasts = false;
#if UNITY_EDITOR
        this.gameObject.name = ogName;
#endif
        WindowsManager.instance.CloseWindows(this.transform.GetInstanceID());
    }
    float targetAlpha = 0f;
    bool isOnOpen = false;
    bool isOnClose = false;
    virtual protected void Update()
    {
        if(windowsTimeType == WindowsTimeType.Time_unscaledDeltaTime)
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, Time.unscaledDeltaTime * speed);
        else if (windowsTimeType == WindowsTimeType.Time_deltaTime)
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, Time.deltaTime * speed);

        if (canvasGroup.alpha > 0.9f && isOnOpen == false && targetAlpha == 1f)
        {
            OnOpen();
            isOnOpen = true;
            isOnClose = false;
        }
        if (canvasGroup.alpha < 0.1f && isOnClose == false && targetAlpha == 0f)
        {
            OnClose();
            isOnClose = true;
        }
    }
    /// <summary>0為關 1為開 淡入淡出配合表現</summary>
    public float alpha
    {
        get { return canvasGroup.alpha; }
    }
    /// <summary>充分完成開啟介面</summary>
    virtual public void OnOpen() { }
    /// <summary>充分完成關閉介面</summary>
    virtual public void OnClose() { }
    void FirstChange(int id)
    {
        if (id == this.transform.GetInstanceID())
        {
            isFirst = true;
            isFirstTime = Time.frameCount;
            OnFirst();
        }
        else
        {
            isFirst = false;
            OnNotFirst();
        }
    }
    int isFirstTime = 0;
    /// <summary>安全的判定自己是否為第一個 適合按鈕用 在當上的同一幀不會通過</summary>
    public bool isSaveFirst
    {
        get { return (isFirst && (Time.frameCount != isFirstTime)); }
    }
    /// <summary>自己是第一個時要做的事情</summary>
    virtual public void OnFirst() { }
    /// <summary>自己不是第一個時要做的事情</summary>
    virtual public void OnNotFirst() { }

}
public enum WindowsTimeType
{
    Time_unscaledDeltaTime = 0,
    Time_deltaTime,
}
public enum WindowsTopType
{
    ToTop = 0,
    No,
}
