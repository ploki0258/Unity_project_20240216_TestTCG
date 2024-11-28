using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
public class SaySystemOptions : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    void Start()
    {
        UnSelect();
    }
    Action<int> action = null;
    Action click = null;
    int number = 0;
    public void Set(string say, Action<int> action, int number, Action click)
    {
        this.action = action;
        this.number = number;
        this.click = click;
        text.text = say; 
    }
    [SerializeField] UnityEvent onClick = null;
    public void OnPointerClick(PointerEventData eventData)
    {
        Select();
        if (action != null)
            action.Invoke(number);
        if(click != null)
            click.Invoke();
    }

    [SerializeField] UnityEvent onEnter = null;
    public void OnPointerEnter(PointerEventData eventData)
    {
        Select();
    }

    [SerializeField] UnityEvent onExit = null;
    public void OnPointerExit(PointerEventData eventData)
    {
        UnSelect();
    }
    public GameObject root = null;
    [SerializeField] Transform select = null;
    [SerializeField] Text text = null;
    [SerializeField] Color selectColor = Color.white;
    [SerializeField] Color unSelectColor = Color.white;
    void Select()
    {
        select.localScale = Vector3.one;
        text.color = selectColor;
    }
    void UnSelect()
    {
        select.localScale = Vector3.zero;
        text.color = unSelectColor;
    }
}
