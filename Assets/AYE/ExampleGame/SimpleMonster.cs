using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using UnityEngine;

public class SimpleMonster : AYEMonster<SimpleMonsterAIType>
{
    [SerializeField] Transform eye = null;
    [SerializeField] LayerMask playerMask;
    [SerializeField] LayerMask blockMask;
    [SerializeField] float 視線距離 = 10f;
    [SerializeField] float 額外距離 = 5f;
    [SerializeField] float 視線角度 = 100f;
    [SerializeField] float 衝向目標時維持鎖定時間 = 0.5f;
    Transform target = null;
    private void Awake()
    {
        AddStatus(SimpleMonsterAIType.待機, 待機, 刷新待機, 離開待機);
        AddStatus(SimpleMonsterAIType.巡邏, 巡邏, 刷新巡邏, 離開巡邏);
        AddStatus(SimpleMonsterAIType.看見什麼, 看見什麼, 刷新看見什麼, 離開看見什麼);
        AddStatus(SimpleMonsterAIType.檢查位置, 檢查位置, 刷新檢查位置, 離開檢查位置);
        AddStatus(SimpleMonsterAIType.左顧右盼, 左顧右盼, 刷新左顧右盼, 離開左顧右盼);
        AddStatus(SimpleMonsterAIType.衝向目標, 衝向目標, 刷新衝向目標, 離開衝向目標, null, null, null, null, 衝向目標Gizmos);
        AddStatus(SimpleMonsterAIType.跳嚇玩家, 跳嚇玩家, 刷新跳嚇玩家, 離開跳嚇玩家);
        AddStatus(SimpleMonsterAIType.失去目標, 失去目標, 刷新失去目標, 離開失去目標);
        AddStatus(SimpleMonsterAIType.搜索目標, 搜索目標, 刷新搜索目標, 離開搜索目標);
    }
    #region 待機
    bool isIdleFind = false;
    void 待機()
    {
        isIdleFind = false;
    }
    void 刷新待機()
    {
        // 停頓一秒後嘗試看看附近有啥
        if (statusTime > 1f && isIdleFind == false)
        {
            isIdleFind = true;
            Transform funStuff = PhysicsFindCanSeeTag(0f, eye, "AIFunPoint", blockMask, 20f);
            if (funStuff != null)
                Face(funStuff);
        }
        if (statusTime > 3f)
            status = SimpleMonsterAIType.巡邏;
        // 每秒偵測附近4次
        Transform temp = PhysicsFindCanSeeTag(0.25f, eye, "Player", blockMask, 視線距離, 視線角度);
        if (temp != null)
        {
            target = temp.transform;
            status = SimpleMonsterAIType.看見什麼;
        }
    }
    void 離開待機()
    {
        CancelFace();
    }
    #endregion
    #region 巡邏
    Vector3 walkTarget = Vector3.zero;
    void 巡邏()
    {
        //只去能去的地方
        walkTarget = GetRandomAIPos(15f, 15f, null);
        anim.SetBool("WALK", true);
    }
    void 刷新巡邏()
    {
        UpdateWay(walkTarget);
        if (IsClose(walkTarget, true))
            status = SimpleMonsterAIType.待機;
        if (statusTime > 30f)
            status = SimpleMonsterAIType.待機;
        // 每秒偵測附近4次
        Transform temp = PhysicsFindCanSeeTag(0.25f, eye, "Player", blockMask, 視線距離, 視線角度);
        if (temp != null)
        {
            target = temp.transform;
            status = SimpleMonsterAIType.看見什麼;
        }
    }
    void 離開巡邏()
    {
        anim.SetBool("WALK", false);
    }
    #endregion
    #region 看見什麼
    // ※ 懷疑時持續看著目標，如果1.5秒後依然能看見目標就追逐、否則到一開始看見的位置找尋。
    Vector3 startSuspectPos = Vector3.zero;
    Vector3 finalSuspectPos = Vector3.zero;
    void 看見什麼()
    {
        Face(target);
        startSuspectPos = target.position;
    }
    void 刷新看見什麼()
    {
        if (IsCanSee(eye.position, target.position, blockMask))
            finalSuspectPos = target.position;
        if (statusTime > 1.5f)
        {
            if (IsCanSee(eye.position, target.position, blockMask))
                status = SimpleMonsterAIType.衝向目標;
            else
                status = SimpleMonsterAIType.檢查位置;
        }
    }
    void 離開看見什麼()
    {
        CancelFace();
    }
    #endregion
    #region 檢查位置 懷疑
    void 檢查位置()
    {
        anim.SetBool("WALK", true);
        finalSuspectPos = GetAIPos(finalSuspectPos);
        startSuspectPos = GetAIPos(startSuspectPos);
    }
    void 刷新檢查位置()
    {
        UpdateWay(finalSuspectPos);
        if (IsClose(finalSuspectPos, true))
            status = SimpleMonsterAIType.左顧右盼;
        if (statusTime > 30f)
            status = SimpleMonsterAIType.左顧右盼;
        // 懷疑時看見玩家就直接追
        Transform tempTarget = PhysicsFindCanSeeTag(0.1f, eye, "Player", blockMask, 視線距離, 視線角度);
        if (tempTarget != null)
        {
            target = tempTarget.transform;
            status = SimpleMonsterAIType.衝向目標;
        }
    }
    void 離開檢查位置()
    {
        anim.SetBool("WALK", false);
    }
    #endregion
    #region 左顧右盼 懷疑
    [SerializeField] public GameObject 左顧右盼視野動畫物件 = null;
    void 左顧右盼()
    {
        GameObject temp = Instantiate(左顧右盼視野動畫物件, this.transform.position, Quaternion.LookRotation(finalSuspectPos- startSuspectPos, Vector3.up));
        Face(temp.transform.GetChild(0).GetChild(0));
    }
    void 刷新左顧右盼()
    {
        if (statusTime > 5f)
            status = SimpleMonsterAIType.待機;
        // 懷疑時看見玩家就直接追
        Transform tempTarget = PhysicsFindCanSeeTag(0.1f, eye, "Player", blockMask, 視線距離, 視線角度);
        if (tempTarget != null)
        {
            target = tempTarget.transform;
            status = SimpleMonsterAIType.衝向目標;
        }
    }
    void 離開左顧右盼()
    {
        CancelFace();
    }
    #endregion
    #region 衝向目標
    float 衝向目標維持時間 = 0f;
    void 衝向目標()
    {
        衝向目標維持時間 = 衝向目標時維持鎖定時間;
        衝刺最後前往地點 = GetAIPos(target.position);
    }
    Vector3 衝刺最後前往地點 = Vector3.zero;
    void 刷新衝向目標()
    {
        if (target == null)
        {
            status = SimpleMonsterAIType.待機;
            return;
        }
        // 偵測指定目標
        if (IsOverAngleCanSee(0.1f, eye, target.position, blockMask, 視線距離, 視線角度))
            衝向目標維持時間 = 衝向目標時維持鎖定時間;
        // 時間內通靈，超時後依然走向目標地點
        if (衝向目標維持時間 > 0f)
        {
            衝刺最後前往地點 = GetAIPos(target.position);
            衝向目標維持時間 -= Time.deltaTime;
        }
        UpdateWay(衝刺最後前往地點, 10f);
        // 身體向目標時才走路
        anim.SetBool("RUN", wayAngle < 90f);
        // 靠近目標
        if (IsClose(衝刺最後前往地點, true, 1f))
        {
            // 靠近且直視目標
            if (IsClose(target.position, true, 1f) && IsCanSee(eye.position, target.position, blockMask))
                status = SimpleMonsterAIType.跳嚇玩家;
            else
                status = SimpleMonsterAIType.失去目標;
        }
    }
    void 衝向目標Gizmos()
    {
        Gizmos.DrawLine(eye.position, 衝刺最後前往地點);
    }
    void 離開衝向目標()
    {
        anim.SetBool("RUN", false);
    }
    #endregion
    #region 跳嚇玩家
    void 跳嚇玩家()
    {
        Face(target);
    }
    void 刷新跳嚇玩家()
    {
        if (statusTime > 2f)
            status = SimpleMonsterAIType.待機;
    }
    void 離開跳嚇玩家()
    {
        CancelFace();
    }
    #endregion
    #region 失去目標 懷疑
    [SerializeField] public GameObject 快速左顧右盼視野動畫物件 = null;
    void 失去目標()
    {
        GameObject temp = Instantiate(快速左顧右盼視野動畫物件, this.transform.position, Quaternion.LookRotation(finalSuspectPos - startSuspectPos, Vector3.up));
        Face(temp.transform.GetChild(0).GetChild(0));
    }
    void 刷新失去目標()
    {
        if (statusTime > 3f)
        {
            if (Aye.IsRandom(80))
                status = SimpleMonsterAIType.搜索目標;
            else
                status = SimpleMonsterAIType.巡邏;
        }
        // 懷疑時看見玩家就直接追
        Transform tempTarget = PhysicsFindCanSeeTag(0.1f, eye, "Player", blockMask, 視線距離, 視線角度);
        if (tempTarget != null)
        {
            target = tempTarget.transform;
            status = SimpleMonsterAIType.衝向目標;
        }
    }
    void 離開失去目標()
    {
        CancelFace();
    }
    #endregion
    #region 搜索目標 懷疑
    Vector3 搜索位置 = Vector3.zero;
    [SerializeField] float 搜索最低距離 = 3f;
    [SerializeField] float 搜索最大距離 = 10f;
    [SerializeField] float 搜索路徑最大距離 = 15f;
    void 搜索目標()
    {
        搜索位置 = GetRandomAIPos(搜索最低距離, 搜索最大距離, 衝刺最後前往地點, 搜索路徑最大距離, true);
    }
    void 刷新搜索目標()
    {
        UpdateWay(搜索位置, 10f);
        // 身體向目標時才走路
        anim.SetBool("RUN", wayAngle < 90f);
        if (statusTime > 5f || IsClose(搜索位置, true))
            status = SimpleMonsterAIType.失去目標;
        // 懷疑時看見玩家就直接追
        Transform tempTarget = PhysicsFindCanSeeTag(0.1f, eye, "Player", blockMask, 視線距離, 視線角度);
        if (tempTarget != null)
        {
            target = tempTarget.transform;
            status = SimpleMonsterAIType.衝向目標;
        }
    }
    void 離開搜索目標()
    {
        anim.SetBool("RUN", false);
    }
    #endregion
}
public enum SimpleMonsterAIType
{
    待機,
    巡邏,
    看見什麼,
    檢查位置,
    左顧右盼,
    衝向目標,
    失去目標,
    搜索目標,
    跳嚇玩家,
}