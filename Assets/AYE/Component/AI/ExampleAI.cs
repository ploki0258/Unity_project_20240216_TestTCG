using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExampleAI
{
    public class ExampleAI : AYEStatusBehaviour<ExampleStatus>
    {
        void Awake()
        {
            // 登記狀態
            AddStatus(ExampleStatus.IDLE, OnIdleEnter, UpdateIdle);
            AddStatus(ExampleStatus.MOVE, OnMoveEnter, null, OnMoveExit, FixedUpdateMove);
        }

        // 待命
        void OnIdleEnter()
        {
            Debug.Log(lastStatus.ToString() + "-->" + status.ToString());
        }
        void UpdateIdle()
        {
            // 發呆一段時間走路
            if (IsTime(Random.Range(1f, 5f), 0))
            {
                status = ExampleStatus.MOVE;
            }
        }

        // 移動
        void OnMoveEnter()
        {
            Debug.Log(lastStatus.ToString() + "-->" + status.ToString());
            Debug.Log("播放走路動畫");
        }
        void OnMoveExit()
        {
            Debug.Log("停止走路動畫");
        }
        void FixedUpdateMove()
        {
            // 移動....

            // 開始移動立即跳一下，然後每一秒跳一下
            if (IsTime(1f, 0, true))
            {
                Debug.Log("跳躍");
            }

            // 移動一段時間後待命
            if (IsTime(Random.Range(1f, 5f), 1))
            {
                status = ExampleStatus.IDLE;
            }
        }
    }

    public enum ExampleStatus
    {
        IDLE,
        MOVE,
    }
}

// 2020 by 阿葉