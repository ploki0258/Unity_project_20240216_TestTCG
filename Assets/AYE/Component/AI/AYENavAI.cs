using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;
namespace AYENavAI
{
    /// <summary>
    /// <para>應用內建尋路系統的高級AI底層，基於AYEStatusBehaviour運作。</para>
    /// </summary>
    public class AYENavAI<StatusEnum> : AYEStatusBehaviour<StatusEnum> where StatusEnum : Enum
    {
        #region 初始化
        /// <summary>巡路系統</summary>
        [HideInInspector]
        public NavMeshAgent nav = null;
        virtual protected void Reset()
        {
            nav = GetComponent<NavMeshAgent>();
            if (nav == null)
                nav = this.gameObject.AddComponent<NavMeshAgent>();
        }
        void NavAIStart()
        {
            if (nav == null)
            {
                nav = GetComponent<NavMeshAgent>();
                Debug.LogWarning("請執行AI身上的Reset確保抓取NavMeshAgent");
            }
        }
        #endregion
        #region 數據調整
        void NavDataStart()
        {
            speed = nav.speed;
            rSpeed = nav.angularSpeed;
            radius = nav.radius;
            height = nav.height;
        }
        /// <summary>移動速度</summary>
        public float speed
        {
            get { return _speed; }
            set { _speed = value; UpdateNav(); }
        }
        float _speed = 3.5f;
        /// <summary>轉向速度</summary>
        public float rSpeed
        {
            get { return _rSpeed; }
            set { _rSpeed = value; UpdateNav(); }
        }
        float _rSpeed = 360f;
        /// <summary>移動速度運轉的百分比</summary>
        public float speedP
        {
            get { return _speedP; }
            set { _speedP = value; UpdateNav(); }
        }
        float _speedP = 1f;
        /// <summary>旋轉速度運轉的百分比</summary>
        public float rSpeedP
        {
            get { return _rSpeedP; }
            set { _rSpeedP = value; UpdateNav(); }
        }
        float _rSpeedP = 1f;
        /// <summary>寬度(直徑)</summary>
        public float radius
        {
            get { return _radios; }
            set { _radios = value; UpdateNav(); }
        }
        float _radios = 0.4f;
        /// <summary>身高</summary>
        public float height
        {
            get { return _height; }
            set { _height = value; UpdateNav(); }
        }
        float _height = 1f;
        /// <summary>寬度百分比</summary>
        public float radiusP
        {
            get { return _radiosP; }
            set { _radiosP = value; UpdateNav(); }
        }
        float _radiosP = 1f;
        /// <summary>身高百分比</summary>
        public float heightP
        {
            get { return _heightP; }
            set { _heightP = value; UpdateNav(); }
        }
        float _heightP = 1f;
        /// <summary>是否需要NAV幫忙移動</summary>
        public bool needNavCtrl
        {
            get { return _needNavCtrl; }
            set { _needNavCtrl = value; UpdateNav(); }
        }
        bool _needNavCtrl = true;
        void UpdateNav()
        {
            nav.speed = speed * speedP * (needNavCtrl? 1f : 0f);
            nav.angularSpeed = rSpeed * rSpeedP;
            nav.radius = radius * radiusP;
            nav.height = height * heightP;

            nav.updatePosition = needNavCtrl;
            nav.updateRotation = needNavCtrl;
            nav.updateUpAxis = needNavCtrl;
        }
        #endregion
        #region 到哪裡、停下
        /// <summary>到了</summary>
        Action Act_GotoDone = null;
        void UpdateGotoDone()
        {
            if (Act_GotoDone != null && !nav.pathPending)
            {
                if (nav.remainingDistance < 0.01f)
                {
                    Act_GotoDone.Invoke();
                    Act_GotoDone = null;
                }
            }
        }
        /// <summary>去這裡</summary>
        public void Goto(Vector3 pos, Action done = null)
        {
            nav.SetDestination(pos);
            Act_GotoDone = done;
        }
        /// <summary>停下移動</summary>
        public void StopGoto()
        {
            nav.SetDestination(this.transform.position);
            Act_GotoDone = null;
        }
        #endregion
        #region 取得座標
        /// <summary>遞迴取得半徑內可以循路的點 r水平半徑 h高度半徑(座標不指定就用自身)</summary>
        public Vector3 GetNAVRandomXZPos(float r, float h, Vector3 pos = default)
        {
            Vector3 rpos = GetRandomXZPos(r, h, pos);
            NavMeshHit hit;
            if (NavMesh.SamplePosition(rpos, out hit, 2.0f, NavMesh.AllAreas))
            {
                return hit.position;
            }
            else
            {
                // 遞迴自己
                return GetNAVRandomXZPos(r, h, pos);
            }
        }
        /// <summary>取得半徑內隨機XZ一點(座標不指定就用自身)</summary>
        public Vector3 GetRandomXZPos(float r, float h, Vector3 pos = default)
        {
            Vector3 add = new Vector3(UnityEngine.Random.Range(-r, r), UnityEngine.Random.Range(-h, h), UnityEngine.Random.Range(-r, r));
            add = Vector3.ClampMagnitude(add, r);
            if (pos == Vector3.zero)
                return this.transform.position + add;
            else
                return pos + add;
        }
        #endregion
        #region 視覺判定
        float lestLookTime = 0f;
        Vector3 lookPos = Vector3.zero;
        Vector3 startLookPos = Vector3.zero;
        /// <summary>取得附近看到的東西(不指定眼睛位置時用身高的一半位置)</summary>
        public Transform EyeLook(LayerMask find, float range, float angle, Transform eyePos = null ,List<int> ignoreID = null, int target = -1)
        {
            Vector3 pos = this.transform.position + (Vector3.up * height * 0.5f);
            Transform eyeTransform = this.transform;
            if (eyePos != null)
            {
                pos = eyePos.position;
                eyeTransform = eyePos;
            }
            Collider[] all = Physics.OverlapSphere(pos, range, find);
            for (int i = 0; i < all.Length; i++)
            {
                // 忽略自己
                if (all[i].transform.GetInstanceID() == this.transform.GetInstanceID())
                    continue;
                // 忽略朋友
                bool isFriend = false;
                if (ignoreID != null)
                {
                    for (int j = 0; j < ignoreID.Count; j++)
                    {
                        if (all[i].transform.GetInstanceID() == ignoreID[j])
                        {
                            isFriend = true;
                            break;
                        }
                    }
                }
                if (isFriend)
                    continue;
                // 忽略非目標
                if (target != -1)
                {
                    if (all[i].transform.GetInstanceID() != target)
                        continue;
                }
                // 檢查角度
                Vector3 ab = eyeTransform.forward;
                Vector3 ac = all[i].transform.position - eyeTransform.position;
                float a = Vector3.Angle(ab, ac);
                if (a < angle * 0.5f)
                {
                    // 是否能直視
                    RaycastHit hit;
                    bool isHit = Physics.Linecast(pos, all[i].transform.position, out hit);
                    if (isHit)
                    {
                        if (hit.transform.GetInstanceID() == all[i].transform.GetInstanceID())
                        {
                            lestLookTime = Time.time;
                            lookPos = all[i].transform.position;
                            startLookPos = pos;
                            return all[i].transform;
                        }
                    }
                }
            }
            return null;
        }
        /// <summary>是否可以直視該目標</summary>
        public bool IsLook(Transform target, Transform eyePos = null)
        {
            Vector3 pos = this.transform.position + (Vector3.up * height * 0.5f);
            if (eyePos != null)
                pos = eyePos.position;
            RaycastHit hit;
            bool isHit = Physics.Linecast(pos, target.position, out hit);
            if (!isHit)
                return false;
            if (hit.transform.GetInstanceID() == target.transform.GetInstanceID())
                return true;
            else
                return false;
        }
        /// <summary>是否面對這個位置</summary>
        public bool IsFace(Vector3 targetPos, float angle, Transform eyePos = null)
        {
            Vector3 pos = this.transform.position + (Vector3.up * height * 0.5f);
            Transform eyeTransform = this.transform;
            if (eyePos != null)
            {
                pos = eyePos.position;
                eyeTransform = eyePos;
            }
            Vector3 ab = eyeTransform.forward;
            Vector3 ac = targetPos - eyeTransform.position;
            float a = Vector3.Angle(ab, ac);
            return (a < angle * 0.5f);
        }
        #endregion
        #region Unity
        protected override void Update50()
        {
            base.Update50();
            UpdateGotoDone();
        }
        protected override void Start()
        {
            base.Start();
            NavAIStart();
            NavDataStart();
        }
        #endregion
        #region Gizmos
        private void OnDrawGizmosSelected()
        {
            if (!nav.pathPending)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawSphere(nav.destination, 0.2f);
            }
            if ((Time.time - lestLookTime) < 2f)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(startLookPos, lookPos);
            }

        }
        #endregion
    }
}

