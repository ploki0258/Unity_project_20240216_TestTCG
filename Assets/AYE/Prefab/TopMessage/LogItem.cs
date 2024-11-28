using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogItem : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform = null;
    [SerializeField] float maxTime = 3.5f;
    float timer = 0;
    void Start()
    {
        timer = 0;
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > maxTime)
        {
            timer = 0f;
            TopMessage.instance.Remove(rectTransform);
            Destroy(this.gameObject);
        }
    }
}

// 2020 by 阿葉