using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 我的工作是負責確認情況並產生OpenTime物件到場地上，因為他是不死之身，重複載入關卡可能會創造出一大堆分身。
/// </summary>
public class OpenTimeManager : MonoBehaviour
{
    [SerializeField] GameObject openTimeObj = null;
    void Start()
    {
        GameObject mainOpenTime = GameObject.Find("OpenTime");
        if (mainOpenTime == null)
        {
            // 看來場上真的不存在OpenTime可以放心的建立了。
            GameObject temp = Instantiate(openTimeObj);
            temp.name = "OpenTime";
        }
    }
}
