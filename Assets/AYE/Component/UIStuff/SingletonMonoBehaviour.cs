using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : class
{
    static public T ins = null;
    virtual protected void Awake()
    {
        ins = this as T;
    }
}
