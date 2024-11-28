using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTimeExample : MonoBehaviour
{
    public void WakeUp(float seconds)
    {
        Debug.Log("自從上關閉遊戲到線在，總共過了 " + seconds + " 秒呢!");
    }
}
