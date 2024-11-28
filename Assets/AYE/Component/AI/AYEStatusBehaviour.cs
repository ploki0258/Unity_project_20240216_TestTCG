using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.ComponentModel;
using static UnityEngine.Rendering.DebugUI;

/// <summary>
/// <para>基於FSM狀態機設計的AI底層，使用時需在Awake執行AddStatus註冊狀態。</para>
/// <para>AddStatus : 登記狀態</para>
/// <para>AddTag : 登記現有狀態的標籤</para>
/// <para>IsTime : 狀態內泛用的計時器，切換清空</para>
/// <para>status : 當前狀態列舉，set時切換狀態</para>
/// <para>lestStatus : 上個狀態列舉，僅讀</para>
/// <para>statusTime : 目前的狀態累積多久時間，切換歸零，僅讀</para>
/// <para>ExitANY : 離開任何狀態後</para>
/// <para>OnANY : 進入任何狀態前</para>
/// <para>Update50 : 省效能的通用刷新</para>
/// <para>FixedUpdate30 : 省效能的通用物理刷新</para>
/// <para>IsTag : 當前狀態的標籤是否包含</para>
/// <para>IsLastTag : 上個狀態的標籤是否包含</para>
/// <para></para>
/// </summary>
public class AYEStatusBehaviour<StatusEnum> : MonoBehaviour where StatusEnum : Enum
{
    struct StatusPack
    {
        public StatusEnum status;
        public StatusEnum[] tag;
        public Action OnFunctionEnter;
        public Action UpdateFunction;
        public Action OnFunctionExit;
        public Action FixedUpdateFunction;
        public Action LateUpdateFunction;
        public Action UpdateFunction50;
        public Action FixedUpdateFunction30;
        public Action OnDrawGizmosFunction;
        /// <summary>這個狀態的標籤是否含有</summary>
        public bool IsTag(params StatusEnum[] list)
        {
            for (int i = 0; i < tag.Length; i++)
            {
                for (int j = 0; j < list.Length; j++)
                {
                    if (list[j].Equals(tag[i]))
                        return true;
                }
            }
            return false;
        }
    }
    List<StatusPack> list = new List<StatusPack>();
    StatusEnum firstStatus;
    /// <summary>
    /// <para>登記狀態</para>
    /// <para>※必須在Awake完成</para>
    /// <para>※第一個登記的狀態視為default值</para>
    /// <para>※default值將在Start執行</para>
    /// <para>※使用status值時自動切換登記的狀態</para>
    /// <para>※lestStatus為上一個狀態</para>
    /// </summary>
    /// <param name="status">對應列舉</param>
    /// <param name="OnFunctionEnter">進入狀態時</param>
    /// <param name="UpdateFunction">狀態刷新</param>
    /// <param name="OnFunctionExit">離開狀態時</param>
    /// <param name="FixedUpdateFunction">狀態物理刷新</param>
    /// <param name="LateUpdateFunction">狀態螢幕顯示刷新</param>
    /// <param name="UpdateFunction50">省效能狀態刷新</param>
    /// <param name="FixedUpdateFunction30">省效能狀態物理刷新</param>
    /// <param name="OnDrawGizmosFunction">Unity Gizmos</param>
    public void AddStatus(StatusEnum status, Action OnFunctionEnter, Action UpdateFunction, Action OnFunctionExit = null, Action FixedUpdateFunction = null, Action LateUpdateFunction = null, Action UpdateFunction50 = null, Action FixedUpdateFunction30 = null, Action OnDrawGizmosFunction = null)
    {
        StatusPack temp = new StatusPack();
        temp.status = status;
        temp.tag = new StatusEnum[0];
        temp.OnFunctionEnter = OnFunctionEnter;
        temp.UpdateFunction = UpdateFunction;
        temp.OnFunctionExit = OnFunctionExit;
        temp.FixedUpdateFunction = FixedUpdateFunction;
        temp.LateUpdateFunction = LateUpdateFunction;
        temp.UpdateFunction50 = UpdateFunction50;
        temp.FixedUpdateFunction30 = FixedUpdateFunction30;
        temp.OnDrawGizmosFunction = OnDrawGizmosFunction;
        list.Add(temp);
        if (list.Count == 1)
            firstStatus = status;
    }

    /// <summary>
    /// <br>為一個狀態添加別的狀態列舉作為標籤</br>
    /// <br>※這個做為標籤的狀態列舉不應該被當成一般狀態使用</br>
    /// </summary>
    /// <param name="status"></param>
    /// <param name="tag"></param>
    public void AddTag(StatusEnum status, params StatusEnum[] tag)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (status.Equals(list[i].status))
            {
                StatusPack temp = list[i];
                temp.tag = tag;
                list[i] = temp;
                break;
            }
        }
    }

    /// <summary>當前狀態，賦值時會先執行上個狀態的OnFunctionExit然後才更新status並執行OnFunctionEnter</summary>
    public StatusEnum status
    {
        get { return _status; }
        set
        {
            needUpdateStatus = value;
            needChangeStatus = true;
            if (statusLogTyoe == AyeStatusLogType.Log)
                Debug.Log(this.gameObject.name + " = " + needUpdateStatus, this.gameObject);
            //Switch(value);
            //if (statusLogTyoe == AyeStatusLogType.Log)
            //    Debug.Log(this.gameObject.name + " = " + value, this.gameObject);
        }
    }
    StatusEnum needUpdateStatus;
    bool needChangeStatus = false;
    [SerializeField] [ShowOnly] StatusEnum _status;
    [SerializeField] AyeStatusLogType statusLogTyoe = AyeStatusLogType.None;
    /// <summary>上次的狀態，僅供讀取</summary>
    public StatusEnum lastStatus
    {
        get { return _lastStatus; }
    }
    StatusEnum _lastStatus;

    StatusPack statusPack = new StatusPack();
    StatusPack lastStatusPack = new StatusPack();

    bool isFirstSwitch = true;
    bool isExit = false;
    bool isEnter = false;
    int switchTimes = 0;
    void Switch(StatusEnum status)
    {
        if (isEnter && status.Equals(_status))
        {
            Debug.LogError("這樣會無限迴圈。");
            return;
        }
        if (isExit)
        {
            Debug.LogError("離開任何狀態時不可以切換動畫，因為會再呼叫自己離開狀態並產生無限迴圈，請在其他情況下切換。");
            return;
        }
        // 非第一次才執行
        if (isFirstSwitch == false)
        {
            isExit = true;
            // 先執行當前狀態的OnFunctionExit
            if (statusPack.OnFunctionExit != null)
                statusPack.OnFunctionExit.Invoke();
            // 離開任何狀態後的執行
            ExitANY(_status);
            isExit = false;
        }
        isFirstSwitch = false;

        // 將上一個狀態設定為當前狀態
        _lastStatus = _status;

        // 將上一個狀態設定為當前狀態
        lastStatusPack = FindStuff(_status);

        // 更新當前狀態
        _status = status;

        // 更新當前狀態
        statusPack = FindStuff(_status);

        // 歸零狀態時間
        _statusTime = 0f;
        // 清空計時器
        timeList.Clear();

        // 進入任何狀態前的執行
        OnANY(status);

        isEnter = true;
        // 執行當前狀態的OnFunctionEnter
        if (statusPack.OnFunctionEnter != null)
            statusPack.OnFunctionEnter.Invoke();
        isEnter = false;
        switchTimes++;
    }
    /// <summary>
    /// <para>A狀態切換到B狀態，介於A狀態的離開和B狀態的進入之間，v為A狀態。</para>
    /// <para>比OnANY早</para>
    /// </summary>
    virtual protected void ExitANY(StatusEnum v)
    {

    }
    /// <summary>
    /// <para>A狀態切換到B狀態，介於A狀態的離開和B狀態的進入之間，v為B狀態。</para>
    /// <para>比ExitANY晚</para>
    /// </summary>
    virtual protected void OnANY(StatusEnum v)
    {

    }
    StatusPack FindStuff(StatusEnum status)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (status.Equals(list[i].status))
            {
                return list[i];
            }
        }
        Debug.LogError("切換到不存在的狀態囉，請確實在Awake登記狀態用的Function。", this.gameObject);
        return new StatusPack();
    }

    virtual protected void Start()
    {
        // 初始化時將狀態設定成第一個註冊的狀態
        status = firstStatus;
    }

    int updateTimes = 0;
    virtual protected void Update()
    {
        _statusTime += Time.deltaTime;
        if (statusPack.UpdateFunction != null)
            statusPack.UpdateFunction.Invoke();
        updateTimes++;
        if (updateTimes >= 50)
        {
            updateTimes -= UnityEngine.Random.Range(30, 70);
            if (statusPack.UpdateFunction50 != null)
                statusPack.UpdateFunction50.Invoke();
            Update50();
        }
    }
    /// <summary>約50個Update執行一次，比Update晚執行(攤平效能)</summary>
    virtual protected void Update50() { }

    int fixedUpdateTimes = 0;
    virtual protected void FixedUpdate()
    {
        if (statusPack.FixedUpdateFunction != null)
            statusPack.FixedUpdateFunction.Invoke();
        fixedUpdateTimes++;
        if (fixedUpdateTimes >= 30)
        {
            fixedUpdateTimes -= UnityEngine.Random.Range(10, 50);
            if (statusPack.FixedUpdateFunction30 != null)
                statusPack.FixedUpdateFunction30.Invoke();
            FixedUpdate30();
        }
    }
    /// <summary>約30個FixedUpdate執行一次，比FixedUpdate晚執行(攤平效能)</summary>
    virtual protected void FixedUpdate30() { }

    virtual protected void LateUpdate()
    {
        if (statusPack.LateUpdateFunction != null)
            statusPack.LateUpdateFunction.Invoke();
        // 每一幀結束後才切換狀態
        if (needChangeStatus)
        {
            needChangeStatus = false;
            Switch(needUpdateStatus);
        }
    }

    private void OnDrawGizmos()
    {
        if (statusPack.OnDrawGizmosFunction != null)
            statusPack.OnDrawGizmosFunction.Invoke();
    }

    /// <summary>狀態累進時間，每次切換都會歸零。</summary>
    protected float statusTime
    {
        get { return _statusTime; }
    }
    float _statusTime;

    struct TimePake
    {
        public byte id;
        public float nt;
    }
    List<TimePake> timeList = new List<TimePake>();
    int creatframe = 0;
    /// <summary>
    /// <para>狀態內泛用的計時器，間隔時間放行，請放在Update</para>
    /// <para>(有微小的誤差、切換狀態會清空值、不同狀態可用同個id)</para>
    /// </summary>
    /// <param name="cd">計時器的間隔</param>
    /// <param name="id">辨識每個計時器用的編號</param>
    /// <param name="executeNow">建立時立即通過一次</param>
    /// <returns></returns>
    protected bool IsTime(float cd, byte id = 0, bool executeNow = false)
    {
        for (int i = 0; i < timeList.Count; i++)
        {
            if (timeList[i].id == id)
            {
                if (Time.frameCount == creatframe)
                {
                    Debug.LogError("同狀態下使用多個計時器時，需要各別設置ID，不可重複使用同一個ID。", this.gameObject);
                }
                else if (statusTime > timeList[i].nt)
                {
                    TimePake current = timeList[i];
                    current.nt = statusTime + cd;
                    timeList[i] = current;
                    return true;
                }
                return false;
            }
        }
        // 創建時紀錄創建幀
        creatframe = Time.frameCount;
        // 找不到對象所以創建新的
        TimePake tp = new TimePake();
        tp.id = id;
        tp.nt = statusTime + cd;
        timeList.Add(tp);
        if (executeNow)
            return true;
        else
            return false;
    }
    /// <summary>當前狀態的標籤是否包含</summary>
    public bool IsTag(params StatusEnum[] list)
    {
        if (switchTimes < 0)
            return false;
        return statusPack.IsTag(list);
    }
    /// <summary>上個狀態的標籤是否包含</summary>
    public bool IsLastTag(params StatusEnum[] list)
    {
        if (switchTimes < 2)
            return false;
        return lastStatusPack.IsTag(list);
    }

    public enum AyeStatusLogType
    {
        // 不用顯示
        None,
        // 顯示
        Log,
    }
}

// 2023 by 阿葉