using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class SaveEditorTool : MonoBehaviour
{
    [MenuItem("AYE/SaveEditor/DeleteAllPlayerPrefs")]
    public static void DeleteAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
