using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>組織群組物件，用來簡單地找到根物件，建立時是用來判定視覺點。</summary>
public class Point : MonoBehaviour
{
    [SerializeField] Point root = null;
    /// <summary>尋找根物件</summary>
    public Point GetRoot()
    {
        if (root == null)
            return this;
        return root.GetRoot();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (root == null || root == this)
        {
            Gizmos.DrawSphere(transform.position, 0.02f);
        }
        else
        {
            Gizmos.DrawWireSphere(transform.position, 0.02f);
            Gizmos.DrawLine(transform.position, root.transform.position);
        }
    }
    private void Reset()
    {
        this.gameObject.name = "Point";
        GetPointManager();
    }
    void GetPointManager()
    {
        if (pointManager == null)
            pointManager = Resources.Load<GameObject>("PointManager");
    }
    [SerializeField] GameObject pointManager = null;
    private void OnEnable()
    {
        GetPointManager();
        if (PointManager.ins == null)
        {
            Instantiate(pointManager).name = "PointManager";
        }
        PointManager.ins.points.Add(this);
    }
    private void OnDisable()
    {
        PointManager.ins.points.Remove(this);
    }
}
