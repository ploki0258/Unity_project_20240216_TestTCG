using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>簡單用來切換場景的功能</summary>
[AddComponentMenu("AYE/SimpleChangeScene")]
public class SimpleChangeScene : MonoBehaviour
{
    [SerializeField] string sceneName = "";
    [SerializeField] bool useShadowManager = false;
    public void Run()
    {
        if (useShadowManager)
        {
            if (ShadowManager.ins != null)
            {
                ShadowManager.ins.Out(DoChangeScene);
            }
            else
            {
                Debug.LogWarning("場地上並沒有ShadowManager，所以並未等待過場動畫。");
                DoChangeScene();
            }
        }
    }
    void DoChangeScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
