///Data         2018.02.01
///Description
///

using UnityEngine;
using System.IO;
using System.Text;

namespace GameFramework
{
    public class LogToDisk : SingleTonGO<LogToDisk>
    {       
        [Tooltip("Filename to save to")]
        public string LogFileName = "log.txt";
      
        [Tooltip("Whether this should only be active an debug builds")]
        public bool DebugBuildsOnly = true;

        StreamWriter LogWriter = null;

        public void Init()
        {
            if (!Debug.isDebugBuild && DebugBuildsOnly) return;

            StringBuilder logPath = new StringBuilder(128);
            logPath.Append(ABPathHelper.AssetsURL);
            logPath.Append("Log");
            logPath.Append(ABPathHelper.Separator);
            logPath.Append(LogFileName);

            PathUtil.CreateDirectory(logPath.ToString());
            LogWriter = new FileInfo(logPath.ToString()).CreateText();
            LogWriter.AutoFlush = true;
        }

        public void Destroy()
        {       
            if (LogWriter != null)
            {
                LogWriter.Close();
                LogWriter = null;
            }        
        }

        void OnEnable()
        {
            if (Debug.isDebugBuild || !DebugBuildsOnly)
            {
                Application.logMessageReceived += HandleLogMessage;
            }
        }

        void OnDisable()
        {
            if (Debug.isDebugBuild || !DebugBuildsOnly)
            {
                Application.logMessageReceived -= HandleLogMessage;
            }
        }
      
        void HandleLogMessage(string logString, string stackTrace, LogType type)
        {
            if (type == LogType.Exception || type == LogType.Error)
            {
                LogWriter.WriteLine("Logged at: " + System.DateTime.Now + " - Log Desc: " + logString + " - Trace: " + stackTrace + " - Type: " + type);
            }
        }
    }
}