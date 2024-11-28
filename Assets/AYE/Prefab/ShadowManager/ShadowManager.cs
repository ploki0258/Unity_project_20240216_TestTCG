using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;
/// <summary>黑幕過場</summary>
public class ShadowManager : SingletonMonoBehaviour<ShadowManager>
{
    public float speed = 1f;
    [SerializeField] AudioMixer audioMixer = null;
    [SerializeField] string audioMixerName = "過場音量";
    [SerializeField] CanvasGroup canvasGroup = null;
    private void Reset()
    {
        if (anim == null)
            anim = GetComponent<Animator>();
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();
    }
    private void Start()
    {
        if (anim == null)
            anim = GetComponent<Animator>();
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();
        if (anim != null)
            anim.speed = speed;
        ctrlAudio = true;
        if (audioMixer != null && canvasGroup != null)
        {
            audioMixer.SetFloat(audioMixerName, -80f);
        }
    }
    [SerializeField] Animator anim = null;
    Action action = null;
    /// <summary>是否正在執行</summary>
    public bool IsPlay()
    {
        return isPley;
    }
    /// <summary>是否在運作</summary>
    bool isPley = true;
    /// <summary>正在白幕</summary>
    bool statusInOut = false;
    /// <summary>進入黑幕</summary>
    public void Out(Action action)
    {
        if (isPley)
        {
            Debug.LogError("不可連續呼叫ShadowManager");
            return;
        }
        ctrlAudio = true;
        if (statusInOut == false)
        {
            Debug.LogError("只有在白幕的狀態下才能進入黑幕");
            return;
        }
        isPley = true;
        this.action = action;
        anim.SetTrigger("Out");
    }
    public void AnimOut()
    {
        Debug.Log("ShadowManager Out");
        isPley = false;
        statusInOut = false;
        ctrlAudio = false;
        if (audioMixer != null && canvasGroup != null)
        {
            audioMixer.SetFloat(audioMixerName, -80f);
        }
        if (action != null)
        {
            action.Invoke();
            action = null;
        }
    }
    /// <summary>進入白幕</summary>
    public void In(Action action)
    {
        if (isPley)
        {
            Debug.LogError("不可連續呼叫ShadowManager");
            return;
        }
        ctrlAudio = true;
        if (statusInOut == true)
        {
            Debug.LogError("只有在黑幕的狀態下才能進入白幕");
            return;
        }
        isPley = true;
        this.action = action;
        anim.SetTrigger("In");
    }
    public void AnimIn()
    {
        Debug.Log("ShadowManager In");
        isPley = false;
        statusInOut = true;
        ctrlAudio = false;
        if (audioMixer != null && canvasGroup != null)
        {
            audioMixer.SetFloat(audioMixerName, 0f);
        }
        if (action != null)
        {

            action.Invoke();
            action = null;
        }
    }

    [SerializeField][ShowOnly] bool ctrlAudio = false;
    private void Update()
    {
        if (ctrlAudio)
        {
            if (audioMixer != null && canvasGroup != null)
            {
                audioMixer.SetFloat(audioMixerName, canvasGroup.alpha * -80f);
            }
        }
    }
}
