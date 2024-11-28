using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ScriptableObject 自訂物件

[CreateAssetMenu(fileName = "新的文本", menuName = "建立新文本")]
public class SayStuff : ScriptableObject
{
    // 表單
    [SerializeField]
    [Header("填寫行數")]
    public List<SayData> list;
    public List<string> opctionList;
}
[System.Serializable]
public struct SayData
{
    public string info;
    public string title;
}