using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// <br>Distance 計算單位離自己的距離，取最短。</br>
/// <br>Find 是否有看見任何對向。</br>
/// <br>CanSee 偵測是否可以直視。</br>
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
/// <summary>適用於3D人形角色的AI底層，配合物理、RootMotion，基於AYEStatusBehaviour。</summary>
public class AYEMonster<StatusEnum> : AYEStatusBehaviour<StatusEnum> where StatusEnum : Enum
{
    public NavMeshAgent nav = null;
    public Animator anim = null;
    private void Reset()
    {
        GetAllComponent();
    }
    protected override void Start()
    {
        base.Start();
        GetAllComponent();
        nav.updatePosition = false;
        nav.updateRotation = false;
        nav.updateUpAxis = false;
    }
    void GetAllComponent()
    {
        if (nav == null)
            nav = this.gameObject.GetComponent<NavMeshAgent>();
        if (nav == null)
            nav = this.gameObject.AddComponent<NavMeshAgent>();
        if (anim == null)
            anim = this.gameObject.GetComponent<Animator>();
        if (anim == null)
            anim = this.gameObject.AddComponent<Animator>();
    }
    float faceForceTime = 0f;
    protected override void Update()
    {
        base.Update();
        nav.nextPosition = this.transform.position;
        // 在看某個東西時 如果角度太大就會轉身
        if (isFace)
        {
            // 失去物件就取消看東西
            if (tempLookTarget == null)
            {
                CancelFace();
                return;
            }
            Vector3 t = tempLookTarget.position + tempOffset;
            t.y = this.transform.position.y;
            if (Vector3.Angle(this.transform.forward, t - this.transform.position) > 80f)
                faceForceTime = 0.5f;
            if (faceForceTime > 0f)
            {
                Quaternion q = Quaternion.LookRotation(t - transform.position, Vector3.up);
                // 輔助視線速度只有3分之1視線速度。
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, q, Time.deltaTime * (tempSpeed * 0.33f));
                faceForceTime -= Time.deltaTime;
            }
        }
    }
    #region 依賴碰撞器的物理性質檢定器
    float lastFindTime = 0f;
    /// <summary>
    /// 判定是否能看見該對象
    /// </summary>
    /// <param name="cd">在Update時可以帶入cd節省性能</param>
    /// <param name="m">視線距離</param>
    /// <param name="angle">角度</param>
    /// <param name="eye">自身眼睛</param>
    /// <param name="player">輸出看到的對象(近到遠)</param>
    /// <param name="blockMask">會阻擋視線的圖層</param>
    /// <param name="canLookMask">目標要看的圖層</param>
    /// <returns>是否有看見任何對向</returns>
    public Collider PhysicsFindCanSeeLayer(float cd, Transform eye, LayerMask canLookMask, LayerMask blockMask, float m, float angle)
    {
        if (cd > 0f && Time.time < lastFindTime + cd)
            return null;
        lastFindTime = Time.time;
        // 圓形判定
        Collider[] overlapSphere = Physics.OverlapSphere(eye.position, m, canLookMask);
        // 檢定是否在扇形內
        for (int i = 0; i < overlapSphere.Length; i++)
        {
            float n = Vector3.Angle(eye.forward, overlapSphere[i].transform.position - eye.position);
            if (n < angle * 0.5f)
            {
                // 可以直視不被阻擋
                if (IsCanSee(eye.position, overlapSphere[i].transform.position, blockMask))
                {
                    return overlapSphere[i];
                }
            }
        }
        return null;
    }
    float lastFindTagTime = 0f;
    /// <summary>回傳附近有相關標記的東西</summary>
    public Transform PhysicsFindTag(float cd, Transform eye, string tagName, float m = 5f, float angle = 360f)
    {
        if (Time.time < lastFindTagTime + cd)
            return null;
        lastFindTagTime = Time.time;
        // 如果周邊有有趣的東西就盯著看
        Collider[] stuff = Physics.OverlapSphere(eye.position, m);
        for (int i = 0; i < stuff.Length; i++)
        {
            if (Vector3.Angle(eye.forward, stuff[i].transform.position - eye.transform.position) < angle * 0.5f)
            {
                if (stuff[i].tag == tagName)
                {
                    return stuff[i].transform;
                }
            }
        }
        return null;
    }
    float lastFindCanSeeTagTime = 0f;
    /// <summary>回傳附近有相關標記並且可以直視不受阻礙的東西</summary>
    public Transform PhysicsFindCanSeeTag(float cd, Transform eye, string tagName, LayerMask blockMask, float m = 5f, float angle = 360f)
    {
        if (Time.time < lastFindCanSeeTagTime + cd)
            return null;
        lastFindCanSeeTagTime = Time.time;
        // 如果周邊有有趣的東西就盯著看
        Collider[] stuff = Physics.OverlapSphere(eye.position, m);
        for (int i = 0; i < stuff.Length; i++)
        {
            if (Vector3.Angle(eye.forward, stuff[i].transform.position - eye.transform.position) < angle * 0.5f)
            {
                if (stuff[i].tag == tagName)
                {
                    if (IsCanSee(eye.position, stuff[i].transform.position, blockMask))
                        return stuff[i].transform;
                }
            }
        }
        return null;
    }
    #endregion
    #region 純檢定
    /// <summary>是否在角度中</summary>
    public bool IsOverAngle(Transform eye, Vector3 target, float m = 5f, float angle = 360f)
    {
        return Vector3.Angle(eye.forward, target - eye.position) < (angle * 0.5f);
    }
    float lastFindCanSeeTime = 0f;
    /// <summary>是否在角度且能直視</summary>
    public bool IsOverAngleCanSee(float cd, Transform eye, Vector3 target, LayerMask blockMask, float m = 5f, float angle = 360f)
    {
        if (Time.time < lastFindCanSeeTime + cd)
            return false;
        lastFindCanSeeTime = Time.time;
        if (IsOverAngle(eye, target, m, angle))
        {
            return IsCanSee(eye.position, target, blockMask);
        }
        return false;
    }
    /// <summary>偵測是否可以直視</summary>
    public bool IsCanSee(Vector3 a, Vector3 b, LayerMask blockMask)
    {
        Vector3 direction = b - a;
        float maxDistance = Vector3.Distance(a, b);
        // 不被阻擋表示可以直視
        return !Physics.Raycast(a, direction, direction.magnitude, blockMask);
    }
    /// <summary>是否足夠近</summary>
    public bool IsClose(Vector3 pos, bool ignoreY, float range = 0.3f)
    {
        if (ignoreY)
            pos.y = this.transform.position.y;
        return Vector3.Distance(pos, this.transform.position) < range;
    }
    /// <summary>計算單位離自己的距離，取最短。</summary>
    public float GetMinDistance(params Vector3[] poss)
    {
        float d = 9999f;
        float n = 0f;
        for (int i = 0; i < poss.Length; i++)
        {
            n = Vector3.Distance(this.transform.position, poss[i]);
            if (n < d)
                d = n;
        }
        return d;
    }
    /// <summary>目的地路徑有效</summary>
    public bool IsPathComplete(Vector3 target)
    {
        NavMeshPath path = new NavMeshPath();
        if (nav.CalculatePath(target, path))
        {
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>在定義範圍內找尋一個可以抵達的位置</summary>
    /// <param name="minDistance">最小距離</param>
    /// <param name="maxDistance">最大距離</param>
    /// <param name="showLog">Log演算次數</param>
    /// <param name="start">起始位置，不指定就從自己開始</param>
    /// <param name="maxPathLength">可以接受的最遠距離 -1為不必判斷</param>
    /// <returns>可去點</returns>
    public Vector3 GetRandomAIPos(float minDistance, float maxDistance, Vector3? start = null, float maxPathLength = -1f, bool showLog = false)
    {
        // 每一次找點失敗就會增加1米容許範圍，容許範圍越大時離最初訂下的最小最大距離會越遠，但也能保證一定能找到座標，就算輸入0也一樣。
        float tryRange = 1f;
        // 有給值就用值
        Vector3 startPos = this.transform.position;
        if (start != null)
            startPos = (Vector3)start;
        Vector3 randomDirection = Vector3.zero;
        NavMeshHit navMeshHit;
        int tryTimes = 0;
        bool done = false;
        Vector3 finalPos = this.transform.position;
        // 最大上限100次演算避免死機
        while (done == false && tryTimes < 100)
        {
            tryTimes++;
            // 沒有有效點就增加容許範圍再試一次
            randomDirection = UnityEngine.Random.insideUnitSphere.normalized * UnityEngine.Random.Range(minDistance, maxDistance);
            randomDirection += startPos;
            bool 找到有效點 = NavMesh.SamplePosition(randomDirection, out navMeshHit, tryRange, NavMesh.AllAreas);
            if (找到有效點 == false)
            {
                tryRange++;
                continue;
            }
            // 這個座標如果無法抵達也為失敗
            if (IsPathComplete(navMeshHit.position) == false)
            {
                tryRange++;
                continue;
            }
            // 如果有需要判定距離上限時才判斷
            if (maxPathLength != -1)
            {
                if (GetPathDistance(navMeshHit.position) > maxPathLength)
                {
                    tryRange++;
                    continue;
                }
            }
            finalPos = navMeshHit.position;
            done = true;
        }
        if (showLog)
            Debug.Log("GetAIPos X " + tryTimes);
        return finalPos;
    }
    /// <summary>找到離這個點最近的可走座標</summary>
    public Vector3 GetAIPos(Vector3 pos, bool showLog = false)
    {
        NavMeshHit navMeshHit;
        float tryRange = 0.01f;
        int tryTimes = 0;
        bool done = false;
        Vector3 finalPos = this.transform.position;
        while (done == false && tryTimes < 100)
        {
            tryTimes++;
            bool 找到有效點 = NavMesh.SamplePosition(pos, out navMeshHit, tryRange, NavMesh.AllAreas);
            if (找到有效點 == false)
            {
                tryRange++;
                continue;
            }
            // 這個座標如果無法抵達也為失敗
            if (IsPathComplete(navMeshHit.position) == false)
            {
                tryRange++;
                continue;
            }
            done = true;
            finalPos = navMeshHit.position;
        }
        if (showLog)
            Debug.Log("GetOkPoint X " + tryTimes);
        return finalPos;
    }
    /// <summary>計算到某處的尋路距離</summary>
    public float GetPathDistance(Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();
        if (nav.CalculatePath(targetPosition, path))
        {
            float distance = 0f;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                distance += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }
            return distance;
        }
        return -1f; // 如果無法計算路徑，返回-1
    }
    #endregion
    #region 路徑轉向與視覺轉向
    Vector3[] eight = new Vector3[]
    {
    new Vector3(0, 0, 1),
    new Vector3(0, 0, -1),
    new Vector3(-1, 0, 0),
    new Vector3(1, 0, 0),
    new Vector3(-0.75f, 0, 0.75f),
    new Vector3(0.75f, 0, 0.75f),
    new Vector3(-0.75f, 0, -0.75f),
    new Vector3(0.75f, 0, -0.75f),
    };
    float lastWayChange = 0f;
    Vector3 lastWay = Vector3.zero;
    /// <summary>在使用UpdateWay可以讀取到離下個轉角的角度</summary>
    public float wayAngle
    {
        get { return Vector3.Angle(transform.forward, wayTarget - transform.position); }
    }
    Vector3 wayTarget = Vector3.zero;
    /// <summary>每幀更新設定移動的方向，直接用ws、ad值反饋給動畫系統，請務必將角色的應用設定八方。(輸出導航座標供參考用)</summary>
    public void UpdateWay(Vector3 target, float rotateSpeed = 5f, float animMixSpeed = 5f)
    {
        // 導航
        nav.SetDestination(target);
        wayTarget = nav.steeringTarget;
        // 高度一致
        wayTarget.y = this.transform.position.y;
        // 如果沒有面向需求就讓自己逐漸配合轉向
        if (isFace == false)
        {
            if (wayTarget - transform.position != Vector3.zero)
            {
                Quaternion q = Quaternion.LookRotation(wayTarget - transform.position, Vector3.up);
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, q, Time.deltaTime * rotateSpeed);
                SetWSADAnim(Vector2.up);
            }
        }
        else
        {
            // 計算出相對位置
            Vector3 ab = target - this.transform.position;
            // 將世界方向轉為本地方向
            Vector3 localab = this.transform.InverseTransformDirection(ab);
            // 限定長度成為向量
            localab = Vector3.ClampMagnitude(localab * 999f, 1f);
            float d = 999f;
            int win = 0;
            for (int i = 0; i < eight.Length; i++)
            {
                float n = Vector3.Distance(eight[i], localab);
                if (n < d)
                {
                    d = n;
                    win = i;
                }
            }
            localab = eight[win];
            if (lastWay != localab && Time.time > lastWayChange + 0.5f)
            {
                lastWay = localab;
                lastWayChange = Time.time;
            }
            // 讀取動畫原本的數值
            Vector2 wsad = new Vector2(anim.GetFloat("ad"), anim.GetFloat("ws"));
            // 漸變
            wsad = Vector2.Lerp(wsad, new Vector2(lastWay.x, lastWay.z), Time.deltaTime * animMixSpeed);
            // 動畫輸出
            SetWSADAnim(wsad);
        }
    }
    Vector2 lastWSAD = Vector2.zero;
    void SetWSADAnim(Vector2 wsad)
    {
        if (lastWSAD != wsad)
        {
            lastWSAD = wsad;
            anim.SetFloat("ad", wsad.x);
            anim.SetFloat("ws", wsad.y);
        }
    }
    Transform tempLookTarget = null;
    Vector3 tempOffset = Vector3.zero;
    float tempSpeed = 0f;
    /// <summary>取消看東西時頭轉回來的速度</summary>
    public float stopFaceSpeed = 2f;
    bool isFace = false;
    /// <summary>設定面朝某個東西 需要搭配CancelFace使用</summary>
    public void Face(Transform target, float speed = 3f)
    {
        Face(target, Vector3.zero, speed);
    }
    /// <summary>面向某個東西</summary>
    public void Face(Transform target, Vector3 offset, float speed = 3f)
    {
        tempSpeed = speed;
        tempLookTarget = target;
        this.tempOffset = offset;
        isFace = true;
    }
    /// <summary>取消面向某個東西</summary>
    public void CancelFace()
    {
        isFace = false;
    }
    /// <summary>視線權重</summary>
    float lookAtWeight = 0f;
    /// <summary>看的目標</summary>
    Vector3 lookPoint = Vector3.zero;
    private void OnAnimatorIK(int layerIndex)
    {
        if(isFace)
            lookAtWeight = Mathf.Lerp(lookAtWeight, 1f, Time.deltaTime * tempSpeed);
        else
            lookAtWeight = Mathf.Lerp(lookAtWeight, 0f, Time.deltaTime * stopFaceSpeed);
        anim.SetLookAtWeight(lookAtWeight);
        if (tempLookTarget != null)
            anim.SetLookAtPosition(tempLookTarget.position + tempOffset);
    }
    #endregion
}