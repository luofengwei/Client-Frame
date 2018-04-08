///Data         2018.02.01
///Description
///

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameFramework
{   
    public class LogToScreen : MonoBehaviour
    {
        public enum LogColor
        {
            NORMAL, WARNING, ERROR
        }

        public Color normalColor = Color.green;
        public Color warningColor = Color.yellow;
        public Color errorColor = Color.red;

        [Tooltip("The maximum number of lines to display")]
        public int maxLines = 30;
        public float lineSpacing = 0.03f;
        
        [Tooltip("Whether this should only be active an debug builds or editor mode")]
        public bool DebugBuildsOrEditorOnly = true;

        public GameObject debugGui = null;       
        public Vector3 defaultGuiPosition = new Vector3(0.01f, 0.98f, 0f);

        public class LogItem
        {        
            public string strLog = string.Empty;
            public LogColor logColor = LogColor.NORMAL;
        }

        private ObjectItemPool<LogItem> LogMessagesPool = new ObjectItemPool<LogItem>();
        private List<GUIText> guiList = new List<GUIText>();

        static string strLog = string.Empty;     
        public bool draggable = true;
        private bool bHidden = true;

        void Awake()
        {
           
        }

        void OnEnable()
        {
            if (GDebug.IsDebugBuildOrEditor || !DebugBuildsOrEditorOnly)
            {
                Application.logMessageReceived += HandleLogMessage;
            }
            InitGuis();
        }

        void OnDisable()
        {
            if (GDebug.IsDebugBuildOrEditor || !DebugBuildsOrEditorOnly)
            {
                Application.logMessageReceived -= HandleLogMessage;
            }
            ClearScreen();
        }

        void HandleLogMessage(string logString, string stackTrace, LogType type)
        {
            LogItem logItem = LogMessagesPool.GetObject();
            logItem.strLog = "\n [" + type + "] : " + logString;    
            switch (type)
            {
                case LogType.Warning:
                    logItem.logColor = LogColor.WARNING;
                    break;
                case LogType.Error:
                    logItem.logColor = LogColor.ERROR;
                    break;
                case LogType.Exception:
                    logItem.strLog = "\n" + stackTrace;
                    break;
                default:
                    break;
            }

            while (LogMessagesPool.GetCount() > maxLines)
            {
                LogMessagesPool.PopFront();
            }

            Display();
        }

        private void Display()
        {
            if (bHidden)
            {
                ClearScreen();
                return;
            }

            int logCount = LogMessagesPool.GetCount();
            int guiCount = 0;
            strLog = string.Empty;
            while (guiCount < guiList.Count && guiCount < logCount)
            {
                LogItem logItem = LogMessagesPool.GetItem(guiCount);

                GUIText gt = guiList[guiCount];
                gt.text = logItem.strLog;
                LogColor lc = logItem.logColor;
                switch (lc)
                {
                    case LogColor.NORMAL:
                        {
                            gt.material.color = this.normalColor;
                        }
                        break;
                    case LogColor.WARNING:
                        {
                            gt.material.color = this.warningColor;
                        }
                        break;
                    case LogColor.ERROR:
                        {
                            gt.material.color = this.errorColor;
                        }
                        break;
                }

                // 循环
                ++guiCount;
                strLog += logItem.strLog;
            }           
        }

        private void InitGuis()
        {
            // 初始化第一个gui
            if (debugGui == null)
            {
                debugGui = new GameObject();
                debugGui.name = "DebugGUI(0)";
                debugGui.transform.SetParent(this.transform, false);
                debugGui.transform.position = defaultGuiPosition;

                GUIText gt = debugGui.AddComponent<GUIText>();
                guiList.Add(gt);
            }

            // 创建其它的gui
            Vector3 position = debugGui.transform.position;
            int guiCount = 1;
            while (guiCount < maxLines)
            {
                position.y -= lineSpacing;

                GameObject clone = (GameObject)Instantiate(debugGui, position, transform.rotation);
                clone.name = string.Format("DebugGUI({0})", guiCount);
                GUIText gt = clone.GetComponent<GUIText>();
                guiList.Add(gt);

                position = clone.transform.position;

                ++guiCount;
            }
            // 设置父节点
            guiCount = 0;
            while (guiCount < guiList.Count)
            {
                GUIText temp = (GUIText)guiList[guiCount];
                temp.transform.parent = debugGui.transform;

                ++guiCount;
            }
        }

        private void ClearScreen()
        {
            // 对象还没创建全的情况下不清
            if (guiList.Count < maxLines)
                return;

            int count = 0;
            while (count < guiList.Count)
            {
                GUIText gt = guiList[count];
                gt.text = string.Empty;

                ++count;
            }
        }

        // 连接到手指
        private bool connectedToMouse = false;
        void Update()
        {
            // 拖拽移动
            if (draggable)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (connectedToMouse)
                    {
                        connectedToMouse = false;
                    }
                    else if (!connectedToMouse && debugGui.GetComponent<GUIText>().HitTest((Vector3)Input.mousePosition))
                    {
                        connectedToMouse = true;
                    }
                }

                if (connectedToMouse)
                {
                    float posX = Input.mousePosition.x / Screen.width;
                    float posY = Input.mousePosition.y / Screen.height;
                    debugGui.transform.position = new Vector3(posX, posY, 0F);
                }
            }
        }
        
        void OnGUI()
        {
            if (GDebug.IsDebugBuildOrEditor)
            {
                if (bHidden)
                {
                    if (GUI.Button(new Rect(Screen.width - 100, 10, 80, 20), "Show"))
                    {
                        bHidden = false;
                    }
                }
                else
                {
                    GUI.TextArea(new Rect(0, 0, (float)Screen.width / 3, Screen.height), strLog);
                    if (GUI.Button(new Rect(Screen.width - 100, 10, 80, 20), "Hide"))
                    {
                        bHidden = true;
                    }
                }
            }
        }
    }
}