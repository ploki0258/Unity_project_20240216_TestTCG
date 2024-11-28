using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class AYENpc<StatusEnum> : AYEStatusBehaviour<StatusEnum>, IAYENpcEditorSupport where StatusEnum : Enum
{
    #region 準備
    public NavMeshAgent navMeshAgent = null;
    public Animator animator = null;
    public CapsuleCollider capsuleCollider = null;
    public Rigidbody rigidbody = null;
    /// <summary>頭部</summary>
    [SerializeField] public Transform head = null;
    public bool IsSetOK()
    {
        return navMeshAgent && animator && capsuleCollider && rigidbody && head;
    }
    public void AutoStart()
    {
        GetComponent();
    }
    [SerializeField][Header("讓NavMeshAgent數值符合CapsuleCollider")] bool autoSyncHeightAndRadius = true;
    virtual protected void Awake()
    {
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        navMeshAgent.updatePosition = false;
        if (autoSyncHeightAndRadius)
        {
            navMeshAgent.height = capsuleCollider.height;
            navMeshAgent.radius = capsuleCollider.radius;
        }
    }
    void GetComponent()
    {
        if (navMeshAgent == null)
            navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
            navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        navMeshAgent.updatePosition = false;
        navMeshAgent.speed = 0f;
        if (animator == null)
            animator = GetComponent<Animator>();
        if (animator == null)
            animator = gameObject.AddComponent<Animator>();
        if (head == null)
            head = transform.Find("Head");
        if (head == null)
            head = transform.Find("Head(Clone)");
        if (head == null)
        {
            // 建立一個新的空物件
            GameObject go = new GameObject("Head");
            head = go.transform;
            head.SetParent(this.transform);
            head.localPosition = Vector3.up * 1.5f;
            Debug.LogWarning("沒有頭，自動生成，請設定該物件座標。");
        }
        if (capsuleCollider == null)
            capsuleCollider = GetComponent<CapsuleCollider>();
        if (capsuleCollider == null)
            capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
        this.capsuleCollider.radius = 0.4f;
        this.capsuleCollider.height = 1.8f;
        this.capsuleCollider.center = Vector3.up * 0.9f;

        if (autoSyncHeightAndRadius)
        {
            navMeshAgent.height = capsuleCollider.height;
            navMeshAgent.radius = capsuleCollider.radius;
        }
        if (rigidbody == null)
            rigidbody = GetComponent<Rigidbody>();
        if (rigidbody == null)
            rigidbody = gameObject.AddComponent<Rigidbody>();
        rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rigidbody.freezeRotation = true;
    }
    #endregion

    /// <summary>轉身朝這</summary>
    public Vector3 face
    {
        get { return _face; }
        set { _face = new Vector3(value.x, this.transform.position.y, value.z); }
    }
    Vector3 _face = Vector3.zero;

    /// <summary>轉身速度</summary>
    [HideInInspector] public float faceSpeed = 1f;

    /// <summary>最小轉向距離</summary>
    [HideInInspector] public float minFaceDistance = 0.1f;

    /// <summary>距離目標朝向還有幾度</summary>
    public float faceAngle { get { return Vector3.Angle(this.transform.forward, _face - this.transform.position); } }

    /// <summary>看這裡</summary>
    [HideInInspector] public Vector3 look = Vector3.zero;

    /// <summary>看該方向的速度</summary>
    [HideInInspector] public float lookSpeed = 3.5f;

    /// <summary>目標視線權重</summary>
    [HideInInspector] public float lookWeight = 1f;
    /// <summary>目標視線權重轉變速度</summary>
    [HideInInspector] public float lookWeightSpeed = 10f;

    /// <summary>在忽略Y軸的情況下視線角度</summary>
    public float lookAngle { get { return Vector3.Angle(this.transform.forward, new Vector3(look.x, this.transform.position.y, look.z) - this.transform.position); } }

    /// <summary>上半身跟著頭部轉向的權重</summary>
    [Range(0, 1f)][Header("人形AI上半身權重")] public float topBodyWeight = 0.3f;
    /// <summary>專為沒有頭部IK的AI保留</summary>
    [SerializeField][Header("非人形AI適用\n讓一個物件替代頭進行轉動")] Transform lookTransformY = null;
    [SerializeField] Transform lookTransformXY = null;
    // --------------------------------------------------

    /// <summary>取得頭部正前方10米的座標</summary>
    public Vector3 headForward
    {
        get { return head.TransformPoint(Vector3.forward * 10f); }
    }

    /// <summary>(X:0 Y:頭部高度 Z:0)的座標，用來計算視線高度差</summary>
    public Vector3 headOffsetY
    {
        get { return Vector3.up * (head.position.y - this.transform.position.y); }
    }

    // --------------------------------------------------

    /// <summary>判定該座標是否於視椎範圍內</summary>
    /// <param name="eye">眼睛位置Z朝前</param>
    /// <param name="target">背叛定點</param>
    /// <param name="radius">有效距離</param>
    /// <param name="angle">有效角度0~360</param>
    /// <returns></returns>
    public bool IsRange(Vector3 target, float radius, float angle, Transform eye = null)
    {
        if (eye == null)
            eye = head;
        float distance = Vector3.Distance(eye.transform.position, target);
        if (distance < radius)
        {
            Vector3 ab = eye.transform.forward;
            Vector3 ac = target - eye.transform.position;
            float bac = Vector3.Angle(ab, ac);
            if (bac < angle * 0.5f)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>判定兩點之間是否可以直視</summary>
    /// <param name="start">起點，預設使用內建head位置</param>
    /// <param name="target">目標點</param>
    /// <param name="blockMask">阻擋視線的圖層，預設為Default</param>
    /// <returns></returns>
    public bool IsCanSee(Vector3 target, Vector3 start = default, LayerMask blockMask = default)
    {
        if (start == default)
            start = head.position;
        if (blockMask == default)
            blockMask = 1 << LayerMask.NameToLayer("Default");
        Vector3 dir = target - start;
        // 不被阻擋表示可以直視
        return !Physics.Raycast(start, dir, dir.magnitude, blockMask);
    }

    /// <summary>判定點是否在視椎範圍中並且可以直視</summary>
    /// <param name="eye">眼睛位置Z朝前，預設使用內建head位置</param>
    /// <param name="target">判定對象</param>
    /// <param name="radius">有效距離</param>
    /// <param name="angle">有效角度0~360</param>
    /// <param name="blockMask">阻擋圖層，預設使用Default</param>
    public bool IsRangeCanSee(Vector3 target, float radius, float angle, Transform eye = null, LayerMask blockMask = default)
    {
        if (eye == null)
            eye = head;
        if (blockMask == default)
            blockMask = 1 << LayerMask.NameToLayer("Default");
        bool isRange = IsRange(target, radius, angle, eye);
        if (isRange)
            return IsCanSee(target, eye.position, blockMask);
        return false;
    }

    /// <summary>
    /// 判定一個物件列表是否在視椎範圍內，並且輸出範圍內由近到遠排序過的列表。
    /// </summary>
    /// <param name="targetList">目標物件列標</param>
    /// <param name="radius">視椎範圍</param>
    /// <param name="angle">視椎角度</param>
    /// <param name="target">輸出最近的對象</param>
    /// <param name="eye">眼睛位置Z朝前，預設使用內建head位置</param>
    /// <returns></returns>
    public bool IsRangeList<T>(List<T> targetList, float radius, float angle, out List<T> target, Transform eye = null) where T : Component
    {
        if (eye == null)
            eye = head;
        target = new List<T>();
        for (int i = 0; i < targetList.Count; i++)
        {
            bool isIn = IsRange(targetList[i].transform.position, radius, angle, eye);
            target.Add(targetList[i]);
        }

        if (target.Count > 1)
        {
            // 由近到遠排序
            target.Sort((x, y) => Vector3.Distance(eye.position, x.transform.position).CompareTo(Vector3.Distance(eye.position, y.transform.position)));
        }
        // 有找到東西
        if (target.Count > 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 判定一個列表是否可以直視，並且輸出可以直視並由近到遠排序過的列表。
    /// </summary>
    /// <typeparam name="T">自訂Component</typeparam>
    /// <param name="targetList">所有對象</param>
    /// <param name="target">可直視對象</param>
    /// <param name="eye">眼睛位置Z朝前，預設使用內建head位置</param>
    /// <param name="blockMask">阻擋圖層，預設使用Default</param>
    /// <returns></returns>
    public bool IsCanSeeList<T>(List<T> targetList, out List<T> target, Vector3 eye = default, LayerMask blockMask = default) where T : Component
    {
        if (eye == default)
            eye = head.position;
        if (blockMask == default)
            blockMask = 1 << LayerMask.NameToLayer("Default");
        target = new List<T>();
        for (int i = 0; i < targetList.Count; i++)
        {
            bool canSee = IsCanSee(targetList[i].transform.position, eye, blockMask);
            if (canSee)
                target.Add(targetList[i]);
        }
        if (target.Count > 1)
        {
            // 由近到遠排序
            target.Sort((x, y) => Vector3.Distance(eye, x.transform.position).CompareTo(Vector3.Distance(eye, y.transform.position)));
        }
        // 有找到東西
        if (target.Count > 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 判定一個物件列表是否在視椎範圍內，並且可以直視，最終輸出範圍內由近到遠排序過的列表。
    /// </summary>
    /// <typeparam name="T">自訂Component</typeparam>
    /// <param name="targetList">判定列表</param>
    /// <param name="radius"></param>
    /// <param name="angle"></param>
    /// <param name="target"></param>
    /// <param name="eye"></param>
    /// <param name="blockMask"></param>
    /// <returns></returns>
    public bool IsRangeCanSeeList<T>(List<T> targetList, float radius, float angle, out List<T> target, Transform eye = null, LayerMask blockMask = default) where T : Component
    {
        if (eye == default)
            eye = head;
        if (blockMask == default)
            blockMask = 1 << LayerMask.NameToLayer("Default");
        bool isRange = IsRangeList<T>(targetList, radius, angle, out target, eye);
        // 範圍內無東西就提早結束
        if (isRange == false)
            return false;
        // 無法直視就提早結束
        List<T> outPut = new List<T>();
        bool isCanSee = IsCanSeeList<T>(target, out outPut, eye.position, blockMask);
        if (isCanSee == false)
            return false;
        // 在範圍內且可以直視
        target = outPut;
        return true;
    }

    // ------------------------------------------

    /// <summary>取得範圍內碰撞器</summary>
    /// <param name="center">中心點，預設為this.transform.position</param>
    /// <param name="radius">半徑</param>
    /// <param name="include">包含的碰撞器</param>
    public Collider[] GetRangeCollider(float radius, LayerMask include, string teg = "", Vector3 center = default)
    {
        if (center == default)
            center = this.transform.position;
        if (teg != "")
            return Physics.OverlapSphere(center, radius, include).Where(x => x.tag == teg).ToArray();
        else
            return Physics.OverlapSphere(center, radius, include);
    }

    // ------------------------------------------

    /// <summary>足夠靠近</summary>
    /// <param name="range">小於這個值表示靠近</param>
    /// <param name="maxY">高度差低於這個值時忽略高度計算</param>
    public bool IsClose(Vector3 pos, float range = 0.5f, float maxY = 2f)
    {
        // 小於高度差則忽略
        if (Mathf.Abs(this.transform.position.y - pos.y) < maxY)
            pos.y = this.transform.position.y;

        return Vector3.Distance(this.transform.position, pos) < range;
    }

    /// <summary>換算成與頭部等高的座標</summary>
    public Vector3 GetHeadSameYPos(Vector3 pos)
    {
        pos.y = head.position.y;
        return pos;
    }

    /// <summary>換算成與自己等高的座標</summary>
    public Vector3 GetSameYPos(Vector3 pos)
    {
        pos.y = this.transform.position.y;
        return pos;
    }

    /// <summary>計算與我的距離</summary>
    public float GetDistance(Vector3 pos)
    {
        return Vector3.Distance(this.transform.position, pos);
    }

    /// <summary>計算與我頭部的距離</summary>
    public float GetHeadDistance(Vector3 pos)
    {
        return Vector3.Distance(head.position, pos);
    }

    /// <summary>用角度取得水平視角座標</summary>
    public Vector3 GetLookPos(float angle)
    {
        Vector3 pos = head.transform.position + (Quaternion.Euler(0f, angle, 0f) * transform.forward * 10f);
        return pos;
    }

    /// <summary>取得視線用的周邊隨機位置</summary>
    /// <param name="minAngle">最少要旋轉幾度0~150</param>
    /// <param name="top">10米處可能向上的距離</param>
    /// <param name="down">10米處可能向下的距離</param>
    public Vector3 GetRandomLookPos(float minAngle = 90f, float maxAngle = 360f, float top = 2f, float down = 2f)
    {
        minAngle = Mathf.Clamp(minAngle, 0f, 150f);
        Vector3 random = UnityEngine.Random.insideUnitSphere.normalized;
        random.y = 0f;
        float angle = Vector3.Angle(head.transform.forward, random);
        while (angle < minAngle * 0.5f && angle > minAngle * 0.5f)
        {
            random = UnityEngine.Random.insideUnitSphere.normalized;
            random.y = 0f;
            angle = Vector3.Angle(head.transform.forward, random);
        }
        Vector3 pos = head.transform.position + random * 10f;
        pos.y = UnityEngine.Random.Range(head.transform.position.y - down, head.transform.position.y + top);
        return pos;
    }

    Vector3 setDestinationPos = Vector3.zero;
    /// <summary>計算路徑並給出下一個轉角或終點的座標</summary>
    /// <param name="targetPos">目標要去的位置</param>
    public Vector3 GetNavigationCorners(Vector3 targetPos)
    {
        if (navMeshAgent.enabled == false)
            return targetPos;
        navMeshAgent.nextPosition = transform.position;
        if (Vector3.Distance(navMeshAgent.nextPosition, transform.position) > 1f)
        {
            ChangePosition(transform.position);
            return targetPos;
        }
        if (setDestinationPos != targetPos)
        {
            setDestinationPos = targetPos;
            navMeshAgent.SetDestination(targetPos);
        }
        return navMeshAgent.steeringTarget;
    }
    /// <summary>目的地路徑有效</summary>
    public bool IsPathComplete(Vector3 target)
    {
        if (navMeshAgent.enabled == false)
            return false;
        NavMeshPath path = new NavMeshPath();
        if (navMeshAgent.CalculatePath(target, path))
        {
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>在定義範圍內找尋一個路徑可以抵達的位置</summary>
    /// <param name="minDistance">最小距離</param>
    /// <param name="maxDistance">最大距離</param>
    /// <param name="showLog">Log演算次數</param>
    /// <param name="start">起始位置，不指定就從自己開始</param>
    /// <param name="maxPathLength">可以接受的最遠距離 -1為不必判斷</param>
    public Vector3 GetRandomNavigationPos(float minDistance, float maxDistance, Vector3? start = null, float maxPathLength = -1f, bool showLog = false)
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
    /// <summary>找到離這個點最近的路徑可走座標</summary>
    public Vector3 GetNavigationPos(Vector3 pos, bool showLog = false)
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
        if (navMeshAgent.enabled == false)
            return 999f;
        NavMeshPath path = new NavMeshPath();
        if (navMeshAgent.CalculatePath(targetPosition, path))
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

    // --------------------------------------------------

    bool isChangePosition = false;
    /// <summary>異步更換位置</summary>
    public void ChangePosition(Vector3 pos, System.Action done = null)
    {
        if (isChangePosition)
        {
            Debug.LogError("不可連續切換位置");
            return;
        }
        StartCoroutine(IChangePosition(pos, done));
    }
    IEnumerator IChangePosition(Vector3 pos, System.Action done)
    {
        isChangePosition = true;
        navMeshAgent.enabled = false;
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        this.transform.position = pos;
        navMeshAgent.enabled = true;
        setDestinationPos = Vector3.zero;
        if (done != null)
            done.Invoke();
        isChangePosition = false;
    }

    // --------------------------------------------------

    protected override void Start()
    {
        base.Start();
        face = this.transform.position + this.transform.forward;
        lookRotation = Quaternion.LookRotation(head.forward);
        lookWeight = 1f;
    }
    protected override void Update()
    {
        base.Update();
        // 超過最小轉向距離
        bool isOverMinFaceDistance = Vector3.Distance(this.transform.position, _face) > minFaceDistance;
        _face.y = this.transform.position.y;
        if (isOverMinFaceDistance)
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(_face - this.transform.position, Vector3.up), Time.deltaTime * faceSpeed);
        lookRotation = Quaternion.Slerp(lookRotation, Quaternion.LookRotation(look - head.position, Vector3.up), Time.deltaTime * lookSpeed);
        // 非人形支援
        if (lookTransformY != null)
            lookTransformY.rotation = new Quaternion(0f, lookRotation.y, 0f, lookRotation.w);
        if (lookTransformXY != null)
            lookTransformXY.rotation = lookRotation;
        _lookWeight = Mathf.Lerp(_lookWeight, lookWeight, Time.deltaTime * lookWeightSpeed);
    }
    Quaternion lookRotation = Quaternion.identity;
    Vector3 realLook = Vector3.zero;
    float _lookWeight = 0f;
    private void OnAnimatorIK(int layerIndex)
    {
        // 設定目標視線
        animator.SetLookAtWeight(_lookWeight, topBodyWeight, 1f, 1f, 0.5f);
        // 計算出看的方向
        realLook = head.position + (lookRotation * Vector3.forward * 10f);
        animator.SetLookAtPosition(realLook);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // 顯示視線方向
        if (head != null && Application.isPlaying)
        {
            Gizmos.DrawLine(head.position, look);
            Gizmos.DrawSphere(look, 0.1f);
        }

        Gizmos.color = Color.blue;
        Vector3 way = Vector3.ClampMagnitude(_face - this.transform.position, 1f);
        // 顯示面向方向
        Gizmos.DrawLine(this.transform.position + Vector3.up, this.transform.position + way + Vector3.up);
        Gizmos.DrawSphere(this.transform.position + way + Vector3.up, 0.1f);
        // 顯示面向方向
        Gizmos.DrawLine(this.transform.position + Vector3.up, transform.TransformPoint(Vector3.forward) + Vector3.up);
        Gizmos.DrawWireSphere(transform.TransformPoint(Vector3.forward) + Vector3.up, 0.1f);
    }
}

public interface IAYENpcEditorSupport
{
    public bool IsSetOK();
    public void AutoStart();
}
