using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>群組系統的管理器，可以找到場上所有的point。</summary>
public class PointManager : SingletonMonoBehaviour<PointManager>
{
    public List<Point> points = new List<Point>();
}
