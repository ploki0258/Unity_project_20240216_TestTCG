using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Physics 2D/FloorCheck 2D")]
public class FloorCheck2D : MonoBehaviour {
    [Header ("可以踩的圖層")]
    [SerializeField] LayerMask floorChakerMask;
    /// <summary>是否在地面上</summary>
    [Header ("判定是否在地板上")]
    public bool onFloor = true;
    [Header ("高度微調")]
    [SerializeField] float offsetY;
    [Header("尺寸微調")]
    [SerializeField] float radius;
    private void Reset()
    {
        floorChakerMask = 0;
        floorChakerMask |= (1 << LayerMask.NameToLayer("Default"));
        offsetY = -1f;
        radius = 0.2f;
    }
    void FixedUpdate()
    {
        // 物理模擬一個碰撞器 並且得知碰到了什麼
        Collider2D[] allStuff = Physics2D.OverlapCircleAll(new Vector2(this.transform.position.x, this.transform.position.y + offsetY), 0.3f, floorChakerMask);

        onFloor = false;
        for (int i = 0; i < allStuff.Length; i++)
        {
            // 碰到任何不是我的東西時
            if (allStuff[i].gameObject != this.gameObject)
            {
                // 有地板可以踩
                onFloor = true;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Vector3 pos = this.transform.position;
        pos.y += offsetY;
        Gizmos.color = (onFloor) ? Color.green : Color.red;
        Gizmos.DrawSphere(pos, radius);
    }
}

// 2020 by 阿葉