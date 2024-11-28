using UnityEditor;
using UnityEngine;
using UnityEditor.Callbacks;

public class MyCustomBuildProcessor : MonoBehaviour
{
    [PostProcessBuildAttribute(0)]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
    {
        PlayerSettings.Android.bundleVersionCode = PlayerSettings.Android.bundleVersionCode + 1;
        PlayerSettings.bundleVersion = System.DateTime.Now.ToString("s").Replace('T', ' ') + " v" + PlayerSettings.Android.bundleVersionCode;
        PlayerSettings.iOS.buildNumber = PlayerSettings.Android.bundleVersionCode.ToString();
    }
}