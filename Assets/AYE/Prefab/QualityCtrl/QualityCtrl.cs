using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class QualityCtrl : MonoBehaviour
{
    [SerializeField] Dropdown dropdown;
    [SerializeField] bool autoSaveAndLoad = true;
    string[] levels = new string[0];
    private void Awake()
    {
        levels = QualitySettings.names;
        dropdown.ClearOptions();
        dropdown.AddOptions(new List<string>(levels));
    }

    int ignoreFrame = 0;

    void Start()
    {
        if (autoSaveAndLoad)
        {
            LoadQuality();
        }
        else
        {
            ignoreFrame = Time.frameCount;
            dropdown.value = QualitySettings.GetQualityLevel();
        }
    }

    public void SetQuality(int index)
    {
        if (ignoreFrame == Time.frameCount)
        {
            return;
        }
        Debug.Log("SetQuality: " + levels[index]);
        QualitySettings.SetQualityLevel(index);

        ignoreFrame = Time.frameCount;
        dropdown.value = index;

        if (autoSaveAndLoad)
        {
            SaveQuality();
        }
    }

    void LoadQuality()
    {
        int quality = PlayerPrefs.GetInt("Quality", QualitySettings.GetQualityLevel());
        Debug.Log("LoadQuality: " + levels[quality]);
        QualitySettings.SetQualityLevel(quality);

        ignoreFrame = Time.frameCount;
        dropdown.value = quality;
    }
    void SaveQuality()
    {
        PlayerPrefs.SetInt("Quality", QualitySettings.GetQualityLevel());
        Debug.Log("SaveQuality: " + levels[QualitySettings.GetQualityLevel()]);
    }
}
