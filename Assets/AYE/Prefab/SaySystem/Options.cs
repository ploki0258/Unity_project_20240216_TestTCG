using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Options : MonoBehaviour
{
    [SerializeField] Text text = null;
    [SerializeField] Color normalColor = Color.white;
    [SerializeField] Color selectedColor = Color.yellow;
    public void Enter()
    {
        text.color = selectedColor;
        text.fontStyle = FontStyle.Bold;
    }
    public void Exit()
    {
        text.color = normalColor;
        text.fontStyle = FontStyle.Normal;
    }
    public void Click()
    {
        Debug.Log("ÂIÀ»¤F" + text.text);
    }
}
