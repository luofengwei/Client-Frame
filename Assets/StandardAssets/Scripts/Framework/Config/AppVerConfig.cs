using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class AppVerConfig
{
    static TextAsset config = null;
    const string fileName = "ConstFolder/Txt/AppVersion";
    static Dictionary<string, int> appVerInfo = null;

    static bool InitConfig()
    {
        if(appVerInfo == null)
        {
            Init();
            config = Resources.Load(fileName) as TextAsset;
            if (config == null) return false;
            MemoryStream configStream = new MemoryStream(config.bytes);
            StreamReader str = new StreamReader(configStream);
            LoadFile(str);
            str.Close();
            configStream.Close();
        }
        return true;
    }

    static void LoadFile(StreamReader sr)
    {
        if (sr == null) return;
        string line = string.Empty;
        while ((line = sr.ReadLine()) != null)
        {
            line = line.Trim();
            if (line == string.Empty) continue;

            int pos = line.IndexOf("#");
            if (pos >= 0)
            {
                line = line.Substring(0, pos);
            }
            line = line.Trim("[] ".ToCharArray());
            if (line == string.Empty) continue;

            pos = line.IndexOf("=");
            if (pos < 0) continue;

            // 这里先不做判定了。。。
            string key = line.Substring(0, pos).Trim();
            string value = line.Substring(pos + 1).Trim();
            int verValue = 0;
            try
            {
                verValue = int.Parse(value);
            }
            catch
            {
                // to do ...
            }
            if (appVerInfo.ContainsKey(key))
            {
                appVerInfo[key] = verValue;
            }
            else
            {
                appVerInfo.Add(key, verValue);
            }
        }
    }

    static void Init()
    {
        if (appVerInfo == null) appVerInfo = new Dictionary<string, int>();
        appVerInfo.Add("Ios", 0);
        appVerInfo.Add("FakeIos", 0);
        appVerInfo.Add("Android", 0);
        appVerInfo.Add("AndroidCommon", 0);
        appVerInfo.Add("Windows", 0);
        appVerInfo.Add("OSX", 0);
    }

    public static void DestoryConfig()
    {
        if(config != null)
        {
            Resources.UnloadAsset(config);
            config = null;
        }

        if(appVerInfo != null)
        {
            appVerInfo.Clear();
            appVerInfo = null;
        }
    }

    /// <summary>
    /// 获取当前平台d
    /// </summary>
    public static int curAppVer
    {
        get
        {
            return GetAppVer(AppHelper.GameChannel);
        }
    }

    public static int GetAppVer(ChannelType channelID)
    {
        if(config == null && !InitConfig())
        {
            return 0;
        }

        switch(channelID)
        {
            case ChannelType.Ios:
                return AppIosVer;
            case ChannelType.Android:
                return AppAndroidVer;
            case ChannelType.Window:
                return AppWindowsVer;
            case ChannelType.OSX:
                return AppOSXVer;
            default:
                return 0;
        }
    }

    private static int AppIosVer
    {
        get
        {
            if (appVerInfo != null && 
                appVerInfo.ContainsKey("Ios"))
            {
                return appVerInfo["Ios"];
            }
            return 0;
        }
    }

    private static int AppAndroidVer
    {
        get
        {
            if (appVerInfo != null && 
                appVerInfo.ContainsKey("Android"))
            {
                return appVerInfo["Android"];
            }
            return 0;
        }
    }

    private static int AppWindowsVer
    {
        get
        {
            if (appVerInfo != null && 
                appVerInfo.ContainsKey("Windows"))
            {
                return appVerInfo["Windows"];
            }
            return 0;
        }
    }

    private static int AppOSXVer
    {
        get
        {
            if (appVerInfo != null && 
                appVerInfo.ContainsKey("OSX"))
            {
                return appVerInfo["OSX"];
            }
            return 0;
        }
    }
}
