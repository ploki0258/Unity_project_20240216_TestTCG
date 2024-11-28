using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaySystemTutorial : MonoBehaviour
{
    [SerializeField] SayStuff 開頭對話 = null;
    void Start()
    {
        SaySystem.instance.StartSay(開頭對話, 開頭對話反饋);
    }
    [SerializeField] SayStuff 明白了對話 = null;
    [SerializeField] SayStuff 還是不太懂 = null;
    void 開頭對話反饋(int result)
    {
        if (result == 0) // 回答 我明白了...
        {
            SaySystem.instance.AddSay(明白了對話);
        }
        else if (result == 1) // 回答 還是不太懂...
        {
            SaySystem.instance.AddSay(還是不太懂, 還是不太懂對話反饋);
        }
        else if (result == 2) // 回答 直接結束對話
        {
            // 啥都不做
        }
    }
    void 還是不太懂對話反饋(int result)
    {
        if (result == 0) // 回答 好啊
        {
            SaySystem.instance.AddSay(開頭對話, 開頭對話反饋);
        }
        else if (result == 1) // 回答 不需要
        {
            // 啥都不做
        }
    }
}
