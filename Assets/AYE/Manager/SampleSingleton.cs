using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>單例設計模式底層</summary>
public class SampleSingleton<T> where T : class, new()
{
    public static T instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new T();
            }
            return _instance;
        }
    }
    static T _instance = null;

    public bool isLoad = false;
    public void Load()
    {
        if (isLoad == true)
            return;
        isLoad = true;
        OnLoad();
    }
    virtual public void OnLoad()
    {
        Debug.Log(typeof(T).ToString() + " loading completed");
    }
}
