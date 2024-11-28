namespace UnityEngine
{
    /// <summary>
    /// Gizmos 擴充
    /// </summary>
    public static class GizmosX
    {
        /// <summary>
        /// 在 3D 空間中繪製圓弧。
        /// </summary>
        /// <param name="center">The center of the circle.</param>
        /// <param name="normal">The normal of the circle.</param>
        /// <param name="from">The direction of the point on the circle circumference, relative to the center, where the arc begins.</param>
        /// <param name="angle"> The angle of the arc, in degrees.</param>
        /// <param name="radius">The radius of the circle Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</param>
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void DrawWireArc(Vector3 center, Vector3 normal, Vector3 from, float angle, float radius)
        {
#if UNITY_EDITOR
            UnityEditor.Handles.color = Gizmos.color;
            UnityEditor.Handles.DrawWireArc(center, normal, from, angle, radius);
#endif
        }

        /// <summary>繪製視覺範圍</summary>
        /// <param name="eye">眼睛物件</param>
        /// <param name="radius">視線長度</param>
        /// <param name="angle">角度</param>
        public static void DrawSeeRange(Transform eye, float radius, float angle)
        {
            Quaternion quaternion = Quaternion.AngleAxis(angle * -0.5f, Vector3.up);
            if (eye != null)
                GizmosX.DrawSolidArc(eye.position, Vector3.up, quaternion * eye.forward, angle, radius);
        }

        /// <summary>
        /// 在 3D 空間中繪製一個圓形扇區（餅圖）。
        /// </summary>
        /// <param name="center">The center of the circle.</param>
        /// <param name="normal">The normal of the circle.</param>
        /// <param name="from">The direction of the point on the circle circumference, relative to the center, where the arc begins.</param>
        /// <param name="angle"> The angle of the arc, in degrees.</param>
        /// <param name="radius">The radius of the circle Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</param>
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void DrawSolidArc(Vector3 center, Vector3 normal, Vector3 from, float angle, float radius)
        {
#if UNITY_EDITOR
            UnityEditor.Handles.color = Gizmos.color;
            UnityEditor.Handles.DrawSolidArc(center, normal, from, angle, radius);
#endif
        }

        /// <summary>
        /// 在 3D 空間中繪製平面圓盤的輪廓。
        /// </summary>
        /// <param name="center">The center of the disc.</param>
        /// <param name="normal">The normal of the disc.</param>
        /// <param name="radius">The radius of the disc.</param>
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void DrawWireDisc(Vector3 center, Vector3 normal, float radius)
        {
#if UNITY_EDITOR
            UnityEditor.Handles.color = Gizmos.color;
            UnityEditor.Handles.DrawWireDisc(center, normal, radius);
#endif
        }

        /// <summary>
        /// 在 3D 空間中繪製一個實心平面圓盤。
        /// 注意：在您可能希望擁有恆定屏幕大小的句柄的地方使用 HandleUtility.GetHandleSize。
        /// Draw a solid flat disc in 3D space.
        /// Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.
        /// </summary>
        /// <param name="center">The center of the disc.</param>
        /// <param name="normal">The normal of the disc.</param>
        /// <param name="radius">The radius of the disc.</param>
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void DrawSolidDisc(Vector3 center, Vector3 normal, float radius)
        {
#if UNITY_EDITOR
            UnityEditor.Handles.color = Gizmos.color;
            UnityEditor.Handles.DrawSolidDisc(center, normal, radius);
#endif
        }

        /// <summary>
        /// 在 3D 空間中製作一個文本標籤。
        /// </summary>
        /// <param name="position">Position in 3D space as seen from the current handle camera.</param>
        /// <param name="text">Text to display on the label.</param>
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void Label(Vector3 position, string text)
        {
#if UNITY_EDITOR
            UnityEditor.Handles.color = Gizmos.color;
            UnityEditor.Handles.Label(position, text);
#endif
        }

        /// <summary>
        /// 在 3D 空間中製作一個文本標籤。
        /// 注意：在您可能希望擁有恆定屏幕大小的句柄的地方使用 HandleUtility.GetHandleSize。
        /// Make a text label positioned in 3D space.
        /// Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.
        /// </summary>
        /// <param name="position">Position in 3D space as seen from the current handle camera.</param>
        /// <param name="content">Text, image and tooltip for this label.</param>
        /// <param name="style">The style to use. If left out, the label style from the current GUISkin is used.</param>
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void Label(Vector3 position, GUIContent content, GUIStyle style)
        {
#if UNITY_EDITOR
            UnityEditor.Handles.color = Gizmos.color;
            UnityEditor.Handles.Label(position, content, style);
#endif
        }
    }
}