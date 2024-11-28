using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AYE
{
    [AddComponentMenu("AYE/ScrollPage")]
    /// <summary>擺在ScrollRect旁，將ScrollRect改造成翻頁的運作方式。</summary>
    public class ScrollPage : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] [Header("速度")] float smooting = 4;
        [Header("總頁數")] public int totalNumberOfPages = 3;
        [SerializeField] [Header("多少像素可跳頁")] float jumpPagePixel = 270f;
        [SerializeField] [Header("跳頁標準時間")] float jumpPageTime = 0.1f;

        public int nowPage { get { return _nowPage; } }
        int _nowPage = 0;

        private ScrollRect rect;
        // 梯狀數值
        private List<float> pages = new List<float>();
        private int currentPageIndex = -1;

        // 起始位置
        private float targethorizontal = 0;

        // 是否拖拽结束
        bool isDrag = false;

        /// <summary>
        /// 取得總頁數 / 頁碼
        /// </summary>
        public System.Action<int, int> OnPageChanged;

        float startime = 0f;
        float delay = 0.1f;

        void Start()
        {
            rect = transform.GetComponent<ScrollRect>();
            startime = Time.time;
            UpdatePages();
        }

        void LateUpdate()
        {
            if (!rect)
                return;
            if (Time.time < startime + delay)
            {
                return;
            }
            if (!isDrag && pages.Count > 0)
            {
                rect.horizontalNormalizedPosition = Mathf.Lerp(rect.horizontalNormalizedPosition, targethorizontal, Time.deltaTime * smooting);
            }
        }

        Vector2 startDragPos = Vector2.zero;
        float startTime = 0f;
        public void OnBeginDrag(PointerEventData eventData)
        {
            isDrag = true;

            PointerEventData ped = eventData as PointerEventData;
            startDragPos = ped.position;
            startTime = Time.time;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isDrag = false;

            PointerEventData ped = eventData as PointerEventData;
            if (ped != null)
            {
                float of = (ped.position.x - startDragPos.x);
                if (of > jumpPagePixel * ((Time.time - startTime) / jumpPageTime))
                {
                    SetPage(nowPage - 1);
                    return;
                }
                else if (of < -jumpPagePixel * ((Time.time - startTime) / jumpPageTime))
                {
                    SetPage(nowPage + 1);
                    return;
                }
            }

            float posX = rect.horizontalNormalizedPosition;
            int index = 0;

            float offset = Mathf.Abs(pages[index] - posX);
            for (int i = 1; i < pages.Count; i++)
            {
                float temp = Mathf.Abs(pages[i] - posX);
                if (temp < offset)
                {
                    index = i;
                    offset = temp;
                }
            }

            if (index != currentPageIndex)
            {
                currentPageIndex = index;
                if (OnPageChanged != null)
                {
                    OnPageChanged(pages.Count, currentPageIndex);
                }
            }
            targethorizontal = pages[index];
            _nowPage = index;
        }

        private void UpdatePages()
        {
            int count = totalNumberOfPages;

            if (pages.Count != count)
            {
                if (count != 0)
                {
                    pages.Clear();
                    for (int i = 0; i < count; i++)
                    {
                        float page = 0;
                        if (count != 1)
                        {
                            page = i / ((float)(count - 1));
                        }
                        pages.Add(page);
                    }
                }
                OnEndDrag(null);
            }
        }

        public void SetTargethorizontal()
        {
            targethorizontal = 0;
        }

        public void SetPage (int v)
        {
            if (v > totalNumberOfPages - 1)
                v = totalNumberOfPages - 1;
            if (v < 0)
                v = 0;
            targethorizontal = pages[v];
            _nowPage = v;
        }
    }
}

// 2020 by 阿葉
