using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CustomLuaClassAttribute]
public class AppHelper
{    
    #region 平台相关

    public static bool isIOS
    {
        get
        {
            return GameChannel == ChannelType.Ios;
        }
    }

    public static bool isAndroid
    {
        get
        {
            return GameChannel == ChannelType.Android;
        }
    }

    //当前的渠道ID   根据当前的目标环境设置
    public static ChannelType GameChannel
    {
        get
        {
            if (ABPathHelper.platformFolder.Equals("Android"))
                return ChannelType.Android;
            else if (ABPathHelper.platformFolder.Equals("iOS"))
                return ChannelType.Ios;
            else if (ABPathHelper.platformFolder.Equals("Windows"))
                return ChannelType.Window;
            else if (ABPathHelper.platformFolder.Equals("OSX"))
                return ChannelType.OSX;
            return ChannelType.Ios;
        }
    }

    #endregion

    #region 版本号相关
    /// <summary>
    /// 当前应用的版本号
    /// </summary>
    private static string _version = string.Empty;
    public static string version
    {
        get
        {
#if (UNITY_EDITOR_OSX || UNITY_EDITOR)
            if (GameUtils.UseSelfBundle)
            {
                return verionChannelBase;
            }
#endif
            if (!string.IsNullOrEmpty(_version))
            {
                return _version;
            }
            else
            {
                Debug.Log("外网FTP获取渠道信息失败");
            }
            return verionChannelBase;
        }
        set
        {
            _version = value;
        }
    }

    /// <summary>
    /// 获取当前应用的基础版本号
    /// </summary>
    private static string _versionBase = string.Empty;
    public static string versionBase
    {
        get
        {
#if (UNITY_EDITOR_OSX || UNITY_EDITOR)
            if (GameUtils.UseSelfBundle)
            {
                return verionChannelBase;
            }
#endif
            if (!string.IsNullOrEmpty(_version))
            {
                _versionBase = _version.Substring(0, _version.LastIndexOf('.') + 1);
                _versionBase += "0";
                return _versionBase;
            }
            return verionChannelBase;
        }
    }

    public static string verionChannelBase
    {
        get
        {
            return versionChannelTag + ".0.0";
        }
    }

    public static string versionChannelTag
    {
        get
        {
            if (isS1)
            {
                return "2";
            }
            else if (isS0)
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }
    }

    /// <summary>
    /// 当前是否是基础版本
    /// </summary>
    public static bool isVersionBase
    {
        get
        {
            return version.Equals(versionBase);
        }
    }
        
    #endregion

    #region 版本分支相关


    //游戏的版本分支，迁出新的分支只需要处理这里的标记即可
    public static string Branch = "s1";

    /// <summary>
    /// 是否是内网主干
    /// </summary>
    public static bool isTrunk
    {
        get { return Branch.Equals("trunk"); }
    }

    /// <summary>
    /// 是否是s0测试服
    /// </summary>
    public static bool isS0
    {
        get { return Branch.Equals("s0"); }
    }

    /// <summary>
    /// 是否是s1正式服
    /// </summary>
    public static bool isS1
    {
        get { return Branch.Equals("s1"); }
    }

    #endregion
}
