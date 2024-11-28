using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("AYE/AutoDestroy")]
public class AutoDestroy : MonoBehaviour
{
    [SerializeField] float cd = 3f;
    private void Start()
    {
        Destroy(this.gameObject, cd);
    }
}
