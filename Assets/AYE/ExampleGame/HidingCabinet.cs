using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingCabinet : MonoBehaviour
{
    [SerializeField] Animator anim = null;
    public void F()
    {
        anim.SetTrigger("Change");
    }
}

