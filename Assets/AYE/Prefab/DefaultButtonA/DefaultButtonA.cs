using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class DefaultButtonA : MonoBehaviour
{
    [SerializeField] Animator anim = null;
    public void Enter()
    {
        anim.SetBool("ENTER", true);
    }
    public void Exit()
    {
        anim.SetBool("ENTER", false);
    }
    Vector3 startPos = Vector3.zero;
    public void Down()
    {
        anim.SetBool("DOWN", true);
        startPos = Input.mousePosition;
    }
    public void Up()
    {
        anim.SetBool("DOWN", false);
        if (Vector2.Distance(Input.mousePosition ,startPos) < 100f)
        {
            Click.Invoke();
        }
    }
    public UnityEvent Click;
}
