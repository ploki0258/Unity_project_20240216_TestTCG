using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AYE
{
    public class FPS : MonoBehaviour
    {
        [SerializeField] Text fpsText = null;
        float fps = 0;
        int frameCount = 0;
        void LateUpdate()
        {
            fps += Time.unscaledDeltaTime;
            frameCount++;

            if (frameCount >= 100)
            {
                //fpsText.text = (100f / fps).ToString("F0") + " fps";
                // 顯示CPU使用率
                fpsText.text = (100f / fps).ToString("F0") + " fps\n" + SystemInfo.processorType + "\n" + SystemInfo.graphicsDeviceName + "\nRAM " + SystemInfo.systemMemorySize + "MB";

                frameCount = 0;
                fps = 0f;
            }
        }
    }
}

// 2020 by 阿葉