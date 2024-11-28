using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Accessibility;
using UnityEngine.UI;
[AddComponentMenu("AYE/GoodUIBar")]
public class GoodUIBar : MonoBehaviour
{
    [SerializeField][Header("測試值")] float testValue = 12;
    [SerializeField][Header("測試最大值")] float textMaxValue = 12;
    [SerializeField] [Header("立即測試")] bool testNow = false;
    private void Update()
    {
        if (testNow)
        {
            testNow = false;
            mainValue = testValue;
            maxValue = textMaxValue;
        }
        DelayBarUpdate();
        AddBarUpdate();
        ShockUpdate();
    }

    /// <summary>主要數值</summary>
    public float mainValue
    {
        get { return _mainValue; }
        set
        {
            if (_mainValue == value)
                return;
            if (value < _mainValue)
                loseValue();
            else
                addValue();
            _mainValue = value;
            mainUIChange();
        }
    }
    float _mainValue = -1f;

    /// <summary>最大數值</summary>
    public float maxValue
    {
        get { return _maxValue; }
        set
        {
            if (_maxValue == value)
                return;
            _maxValue = value;
            mainUIChange();
            GridUIChange();
        }
    }
    float _maxValue = -1f;
    /// <summary>主條</summary>
    [SerializeField] [Header("主UI條")] Image mainBar = null;
    /// <summary>主要變化</summary>
    void mainUIChange()
    {
        if (mainBar == null)
            return;
        mainBar.fillAmount = Mathf.Clamp01(mainValue / maxValue);
    }
    /// <summary>延遲條</summary>
    [SerializeField] [Header("延遲條")] Image delayBar = null;
    /// <summary>增益內條</summary>
    [SerializeField] [Header("增益內延遲條")] Image addBar = null;
    /// <summary>延遲條百分比</summary>
    float delayBarPercent = 1f;
    /// <summary>延遲條凝固到何時</summary>
    float delayBarDelayTime = 0f;
    /// <summary>延遲條凝固多久</summary>
    [SerializeField] [Header("延遲條凝固多久")] float delayTime = 0.3f;
    /// <summary>延遲條消失速度</summary>
    [SerializeField] [Header("延遲條消失速度")] float delayBarSpeed = 0.75f;
    /// <summary>黑條</summary>
    [SerializeField] [Header("黑條")] Image blackBar = null;
    /// <summary>黑條厚度</summary>
    [SerializeField] [Header("黑條厚度")] float blackBarLength = 0.02f;
    void DelayBarUpdate()
    {
        if (delayBar == null || mainBar == null || blackBar == null)
            return;
        if (mainBar.fillAmount > delayBarPercent)
        {
            delayBarPercent = mainBar.fillAmount;
            delayBarDelayTime = 0f;
        }
        else if (Time.time > delayBarDelayTime)
        {
            delayBarPercent = Mathf.MoveTowards(delayBarPercent, mainBar.fillAmount, delayBarSpeed * Time.deltaTime);
        }
        delayBar.fillAmount = delayBarPercent;
        blackBar.fillAmount = mainBar.fillAmount + blackBarLength;
    }
    float addBarPercent = 0f;
    float addDelayBarDelayTime = 0f;
    void AddBarUpdate()
    {
        if (addBar == null)
            return;
        if (addBarPercent > mainBar.fillAmount)
        {
            addBarPercent = mainBar.fillAmount;
            addDelayBarDelayTime = 0f;
        }
        else if (Time.time > addDelayBarDelayTime)
        {
            addBarPercent = Mathf.MoveTowards(addBarPercent, mainBar.fillAmount, delayBarSpeed * Time.deltaTime);
        }
        addBar.fillAmount = addBarPercent;
    }
    /// <summary>震動根物件</summary>
    [SerializeField] [Header("震動根物件")] RectTransform shockRoot = null;
    /// <summary>最大震動幅度</summary>
    [SerializeField] [Header("最大震動幅度")] float maxShockRange = 300f;
    /// <summary>震動速度</summary>
    [SerializeField] [Header("震動速度")] float shockSpeed = 30f;
    /// <summary>震動衰減速度</summary>
    [SerializeField] [Header("震動衰減速度")] float loseShockSpeed = 900f;
    float shockRange = 0f;
    float t = 0f;
    void ShockUpdate()
    {
        if (shockRoot == null)
            return;
        if (shockRange < 0.001f)
        {
            shockRoot.anchoredPosition = Vector2.zero;
            return;
        }
        t += Time.deltaTime * shockSpeed;
        shockRoot.anchoredPosition = new Vector2((-0.5f * shockRange) + (Mathf.PerlinNoise1D(t) * shockRange), Random.Range(-1f, 1f));
        shockRange = Mathf.MoveTowards(shockRange, 0f, loseShockSpeed * Time.deltaTime);
    }
    void loseValue()
    {
        shockRange = maxShockRange;
        delayBarDelayTime = Time.time + delayTime;
    }
    void addValue()
    {
        addDelayBarDelayTime = Time.time + delayTime;
    }
    /// <summary>格線背景</summary>
    [SerializeField] [Header("格線背景")] RectTransform gridUIBg = null;
    /// <summary>格線物件</summary>
    [SerializeField] [Header("格線物件")] GameObject gridUI = null;
    /// <summary>多少數值一格</summary>
    [SerializeField] [Header("多少數值一格")] float distanceByValue = 3f;
    List<GameObject> recycle = new List<GameObject>();
    void GridUIChange()
    {
        if (gridUIBg == null || gridUI == null)
            return;
        float 總長度 = gridUIBg.sizeDelta.x;
        float 格線數量 = maxValue / distanceByValue;
        float 格線間距 = 總長度 / 格線數量;

        for (int i = 0; i < recycle.Count; i++)
        {
            Destroy(recycle[i]);
        }
        gridUI.SetActive(true);
        recycle.Clear();
        for (int i = 1; i < (int)格線數量+1; i++)
        {
            float 位置 = 格線間距 * i;
            if (位置 >= 總長度)
                continue;
            GameObject 格線 = Instantiate(gridUI, gridUIBg);
            RectTransform rectTransform = 格線.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(位置, rectTransform.anchoredPosition.y);
            recycle.Add(格線);
        }
        gridUI.SetActive(false);
    }
}
