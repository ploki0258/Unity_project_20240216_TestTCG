using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public float volume = 1f;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        GizmosX.DrawWireDisc(transform.position, Vector3.up, volume);
    }
    private void Reset()
    {
        GetSoundManager();
    }
    void GetSoundManager()
    {
        if (soundManager == null)
            soundManager = Resources.Load<GameObject>("SoundManager");
    }
    [SerializeField] GameObject soundManager = null;
    private void OnEnable()
    {
        GetSoundManager();
        if (SoundManager.ins == null)
        {
            Instantiate(soundManager).name = "SoundManager";
        }
        SoundManager.ins.sounds.Add(this);
    }
    private void OnDisable()
    {
        SoundManager.ins.sounds.Remove(this);
    }
}
