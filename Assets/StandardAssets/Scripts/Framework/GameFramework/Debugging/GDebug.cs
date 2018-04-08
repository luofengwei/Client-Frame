///Data         2018.02.01
///Description
///

using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace GameFramework
{   
    public class GDebug
    {
        public enum DebugLevelType 
        { 
            Information, 
            Warning, 
            Error, 
            None = 99
        }

        public static DebugLevelType DebugLevel 
        { 
            get; 
            set; 
        }
     
        static GDebug()
        {
            DebugLevel = DebugLevelType.None;
        }
   
        public static bool IsDebugBuildOrEditor
        {
#if UNITY_EDITOR
            get { return true; }
#else
            get { return UnityEngine.Debug.isDebugBuild; }
#endif
        }

        public static void DebugText(string text)
        {
            if (!IsDebugBuildOrEditor)
                return;

            GameObject go = GameObject.Find("DebugText");
            if (go != null)
            {
                Text label = go.GetComponent<Text>();
                if (label != null)
                {
                    label.text = text;
                }
            }
        }

        [Conditional("UNITY_EDITOR")]
        public static void Log(object message)
        {
            if (DebugLevel <= DebugLevelType.Information)
                Debug.Log(message);
        }

        [Conditional("UNITY_EDITOR")]
        public static void LogF(string format, params object[] args)
        {
            if (DebugLevel <= DebugLevelType.Information)
                Debug.Log(string.Format(format, args));
        }

        [Conditional("UNITY_EDITOR")]
        public static void Log(object message, Object context)
        {
            if (DebugLevel <= DebugLevelType.Information)
                Debug.Log(message, context);
        }

        [Conditional("UNITY_EDITOR")]
        public static void LogError(object message)
        {
            if (DebugLevel <= DebugLevelType.Error)
                Debug.LogError("<color=red>" + message + "</color>");
        }

        [Conditional("UNITY_EDITOR")]
        public static void LogErrorF(string format, params object[] args)
        {
            if (DebugLevel <= DebugLevelType.Error)
                Debug.LogError(string.Format(format, args));
        }

        [Conditional("UNITY_EDITOR")]
        public static void LogError(object message, Object context)
        {
            if (DebugLevel <= DebugLevelType.Error)
                Debug.LogError(message, context);
        }

        [Conditional("UNITY_EDITOR")]
        public static void LogWarning(object message)
        {
            if (DebugLevel <= DebugLevelType.Warning)
                Debug.LogWarning("<color=yellow>" + message.ToString() + "</color>");
        }
    
        [Conditional("UNITY_EDITOR")]
        public static void LogWarningF(string format, params object[] args)
        {
            if (DebugLevel <= DebugLevelType.Warning)
                Debug.LogWarning(string.Format(format, args));
        }

        [Conditional("UNITY_EDITOR")]
        public static void LogWarning(object message, Object context)
        {
            if (DebugLevel <= DebugLevelType.Warning)
                Debug.LogWarning(message.ToString(), context);
        }
  
        [Conditional("UNITY_EDITOR")]
        public static void DrawLine(Vector3 start, Vector3 end, Color color = default(Color), float duration = 0.0f, bool depthTest = true)
        {
            Debug.DrawLine(start, end, color, duration, depthTest);
        }
   
        [Conditional("UNITY_EDITOR")]
        public static void DrawRay(Vector3 start, Vector3 dir, Color color = default(Color), float duration = 0.0f, bool depthTest = true)
        {
            Debug.DrawRay(start, dir, color, duration, depthTest);
        }

        [Conditional("UNITY_EDITOR")]
        [System.Obsolete("Deprecated: Use LogWarning instead.")]
        public static void Throw(string message)
        {
            Debug.LogWarning(message);
        }
    
        [Conditional("UNITY_EDITOR")]
        public static void DrawCube(Vector3 position, Color color, Vector3 size, float duration = 0.0f)
        {
            Vector3 halfScale = size * 0.5f;

            Vector3[] points =
            {
                position + new Vector3(halfScale.x,      halfScale.y,    halfScale.z),
                position + new Vector3(-halfScale.x,     halfScale.y,    halfScale.z),
                position + new Vector3(-halfScale.x,     -halfScale.y,   halfScale.z),
                position + new Vector3(halfScale.x,      -halfScale.y,   halfScale.z),
                position + new Vector3(halfScale.x,      halfScale.y,    -halfScale.z),
                position + new Vector3(-halfScale.x,     halfScale.y,    -halfScale.z),
                position + new Vector3(-halfScale.x,     -halfScale.y,   -halfScale.z),
                position + new Vector3(halfScale.x,      -halfScale.y,   -halfScale.z),
            };

            Debug.DrawLine(points[0], points[1], color, duration);
            Debug.DrawLine(points[1], points[2], color, duration);
            Debug.DrawLine(points[2], points[3], color, duration);
            Debug.DrawLine(points[3], points[0], color, duration);
        }
    
        [Conditional("UNITY_EDITOR")]
        public static void DrawRect(Rect rect, Color color, float duration = 0.0f)
        {
            Vector3 pos = new Vector3(rect.x + rect.width / 2, rect.y + rect.height / 2, 0.0f);
            Vector3 scale = new Vector3(rect.width, rect.height, 0.0f);

            DrawRect(pos, color, scale, duration);
        }
    
        [Conditional("UNITY_EDITOR")]
        public static void DrawRect(Vector3 position, Color color, Vector3 size, float duration = 0.0f)
        {
            Vector3 halfSize = size * 0.5f;

            Vector3[] points = 
            {
                position + new Vector3(halfSize.x,      halfSize.y,    halfSize.z),
                position + new Vector3(-halfSize.x,     halfSize.y,    halfSize.z),
                position + new Vector3(-halfSize.x,     -halfSize.y,   halfSize.z),
                position + new Vector3(halfSize.x,      -halfSize.y,   halfSize.z),
            };

            Debug.DrawLine(points[0], points[1], color, duration);
            Debug.DrawLine(points[1], points[2], color, duration);
            Debug.DrawLine(points[2], points[3], color, duration);
            Debug.DrawLine(points[3], points[0], color, duration);
        }

        [Conditional("UNITY_EDITOR")]
        public static void DrawPoint(Vector3 pos, Color color, float size, float duration = 0.0f)
        {
            Vector3[] points = 
            {
                pos + Vector3.up * size,
                pos - Vector3.up * size,
                pos + Vector3.right * size,
                pos - Vector3.right * size,
                pos + Vector3.forward * size,
                pos - Vector3.forward * size
            };

            Debug.DrawLine(points[0], points[1], color, duration);
            Debug.DrawLine(points[2], points[3], color, duration);
            Debug.DrawLine(points[4], points[5], color, duration);

            Debug.DrawLine(points[0], points[2], color, duration);
            Debug.DrawLine(points[0], points[3], color, duration);
            Debug.DrawLine(points[0], points[4], color, duration);
            Debug.DrawLine(points[0], points[5], color, duration);

            Debug.DrawLine(points[1], points[2], color, duration);
            Debug.DrawLine(points[1], points[3], color, duration);
            Debug.DrawLine(points[1], points[4], color, duration);
            Debug.DrawLine(points[1], points[5], color, duration);

            Debug.DrawLine(points[4], points[2], color, duration);
            Debug.DrawLine(points[4], points[3], color, duration);
            Debug.DrawLine(points[5], points[2], color, duration);
            Debug.DrawLine(points[5], points[3], color, duration);
        }

        /// <summary>
        /// Draw a 2D GizmoRect for use with Editor mode
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="color"></param>
        [Conditional("UNITY_EDITOR")]
        public static void DrawGizmoRect(Rect rect, Color color)
        {
            Gizmos.color = color;
            Gizmos.DrawLine(new Vector3(rect.xMin, rect.yMin, 0), new Vector3(rect.xMax, rect.yMin, 0));
            Gizmos.DrawLine(new Vector3(rect.xMax, rect.yMin, 0), new Vector3(rect.xMax, rect.yMax, 0));
            Gizmos.DrawLine(new Vector3(rect.xMax, rect.yMax, 0), new Vector3(rect.xMin, rect.yMax, 0));
            Gizmos.DrawLine(new Vector3(rect.xMin, rect.yMax, 0), new Vector3(rect.xMin, rect.yMin, 0));
        }
    }
}