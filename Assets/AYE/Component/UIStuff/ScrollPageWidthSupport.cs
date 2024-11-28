using AYE;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[AddComponentMenu("AYE/ScrollPageWidthSupport")]
/// <summary>專門用來擴充ScrollPage功能，使其成為允許保護高度的類皇室戰爭介面。</summary>
public class ScrollPageWidthSupport : MonoBehaviour
{
    private void Reset()
    {
        scrollPage = this.GetComponent<ScrollPage>();
    }

    [SerializeField] ScrollPage scrollPage = null;
    [SerializeField][Header("需要偵測縮放")] Canvas canvas = null;
    [SerializeField][Header("整體縮放用的Content")] RectTransform content = null;
    [SerializeField][Header("影響的頁面")] RectTransform[] pages = new RectTransform[0];

    float lastWidth = 0f;
    float lastCanvasScaleFactor = 0f;
    private void LateUpdate()
    {
        if (lastWidth != Screen.width || lastCanvasScaleFactor != canvas.scaleFactor)
        {
            lastWidth = Screen.width;
            lastCanvasScaleFactor = canvas.scaleFactor;
            UpdateUI();
        }
    }
    [SerializeField][Header("測試LOG")] bool showLog = false;
    void UpdateUI()
    {
        float w = Screen.width / canvas.scaleFactor;
        content.sizeDelta = new Vector2(w * (float)scrollPage.totalNumberOfPages, content.sizeDelta.y);
        for(int i = 0; i < pages.Length; i++)
        {
            pages[i].sizeDelta = new Vector2(w, pages[i].sizeDelta.y);
            pages[i].anchoredPosition = new Vector2((float)i * w, pages[i].anchoredPosition.y);
        }
        if (showLog)
            Debug.Log("重設尺寸");
    }
}
