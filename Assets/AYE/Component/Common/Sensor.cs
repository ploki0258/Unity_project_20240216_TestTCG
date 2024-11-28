using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("AYE/Sensor")]
public class Sensor : MonoBehaviour
{
    bool _on = false;
    [ShowOnly] public bool on = false;
    [SerializeField] SensorType sensorType = SensorType.Sphere;
    [SerializeField] LayerMask layerMask = 0;
    [Header("Sphere")]
    [SerializeField] float sphereRange = 1f;
    [Header("Box")]
    [SerializeField] Vector3 boxRange = Vector3.one;
    [Header("Line")]
    [SerializeField] float lineRange = 1f;
    [Header("變化時事件輸出")]
    [SerializeField] UnityEvent<bool> change = null;
    [SerializeField] UnityEvent changeOn = null;
    [SerializeField] UnityEvent changeOff = null;
    [Header("強制送出第一幀的事件作為初始值")]
    [SerializeField] bool autoInvokeOnFirstTime = true;
    bool isFirst = true;
    private void FixedUpdate()
    {
        if (sensorType == SensorType.Sphere)
        {
            Collider[] cols = Physics.OverlapSphere(transform.position, sphereRange, layerMask);
            on = cols.Length > 0;
        }
        else if (sensorType == SensorType.Box)
        {
            Collider[] cols = Physics.OverlapBox(transform.position, boxRange * 0.5f, transform.rotation, layerMask);
            on = cols.Length > 0;
        }
        else if (sensorType == SensorType.Line)
        {
            RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, lineRange, layerMask);
            on = hits.Length > 0;
        }

        if (on != _on || (isFirst && autoInvokeOnFirstTime))
        {
            _on = on;
            isFirst = false;
            change.Invoke(_on);
            if (_on)
                changeOn.Invoke();
            else
                changeOff.Invoke();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.matrix = rotationMatrix;

        if (sensorType == SensorType.Sphere)
        {
            if (on == false)
                Gizmos.DrawWireSphere(Vector3.zero, sphereRange);
            else
                Gizmos.DrawSphere(Vector3.zero, sphereRange);
        }
        else if (sensorType == SensorType.Box)
        {
            if (on == false)
                Gizmos.DrawWireCube(Vector3.zero, boxRange);
            else
                Gizmos.DrawCube(Vector3.zero, boxRange);
        }
        else if (sensorType == SensorType.Line)
        {
            Vector3 a = Vector3.zero;
            Vector3 b = Vector3.forward * lineRange;
            if (on == false)
            {
                Gizmos.DrawLine(a, b);
            }
            else
            {
                Gizmos.DrawLine(a, b);
                for (int i = 0; i < 6; i++)
                {
                    Gizmos.DrawSphere(Vector3.Lerp(a, b, (float)i / 5f), 0.05f);
                }
            }
        }
    }

    public enum SensorType
    {
        Sphere, Box, Line
    }
}

