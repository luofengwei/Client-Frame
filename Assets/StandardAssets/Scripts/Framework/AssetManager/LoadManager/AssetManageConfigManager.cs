///author       xuhan
///Data         2016.07.21
///Description

using UnityEngine;
using GameFramework;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

public enum AssetManageMode
{
    [XmlEnum(Name = "UnLoadImmediately")]
    UnLoadImmediately,
    AMMMax,
}

public enum AssetEncryptMode
{
    [XmlEnum(Name = "None")]
    None,
    [XmlEnum(Name = "DES")]
    DES,
    [XmlEnum(Name = "DES1")]
    DES1,
    [XmlEnum(Name = "AES")]
    AES,
    [XmlEnum(Name = "AES1")]
    AES1,
    [XmlEnum(Name = "AES2")]
    AES2,
}
[CustomLuaClassAttribute]
public enum ELoadType
{
    UI = 0,     //UI资源
    Animation,  //动作文件
    Music,      //背景音乐   
    Sound,      //音效文件      
    Avatar,     //Avatar资源
    Effect,     //特效文件
    Icon,       //图标（商城物品等）   
    Model,      //模型资源  
    Font,    
    Shader,
    ShaderVariant,
    Spine2D,    //Spine动画
    TableConfig,
    LuaScript,
    Scene,
    Config,     //脚本文件（都打成一个包） AutoGenerate,EmojiConfig,TableConfig,GameConfig,ShootSet,Magic,Script
    Max,
};

[System.Serializable]
public class AssetManageInfo
{
    private static AssetManageInfo _Default = null;
    [XmlAttribute("Path")]
    public string path = string.Empty;
    [XmlAttribute("LoadType")]
    public ELoadType loadType = ELoadType.Config;
    [XmlAttribute("DelayDelTime")]
    public float delayDelTime = 0.0f;
    [XmlAttribute("LimitCount")]
    public int limitCount = 0;
    [XmlAttribute("Encryption")]
    public AssetEncryptMode encryption = AssetEncryptMode.None;
  
    public void ResetInfo(string path, AssetManageMode bundleMode, AssetEncryptMode encryption, ELoadType loadType, float delayDelTime)
    {       
        this.path = path;
        this.loadType = loadType;
        this.delayDelTime = delayDelTime;       
    }

    public static AssetManageInfo Default
    {
        get
        {
            if(_Default == null)
            {
                _Default = new AssetManageInfo();
                _Default.ResetInfo(ABPathHelper.ABResourcePrefix, AssetManageMode.UnLoadImmediately, AssetEncryptMode.None, ELoadType.Config, 0.0f);
            }
            return _Default;
        }
    }

    public void UnLoad()
    {
    }
}

[System.Serializable]
public class AssetManageConfigFile
{
    public List<AssetManageInfo> infos = new List<AssetManageInfo>();

    public AssetManageInfo GetInfo(string key)
    {      
        for (int i = 0; i < infos.Count; ++i)
        {
            if (key.IndexOf(infos[i].path, System.StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return infos[i];
            }
        }
        return AssetManageInfo.Default;
    }

    public void AddInfo(AssetManageInfo info)
    {
        infos.Add(info);
    }

    public void ToLower()
    {        
        for (int i = 0; i < infos.Count; ++i)
        {
            ToLower(infos[i]);          
        }
    }

    private void ToLower(AssetManageInfo info)
    {
        info.path = info.path.ToLower();
    }

    public void UnLoad()
    {
        if (infos != null)
        {
            infos.Clear();
            infos = null;
        }
    }
}

public class AssetManageConfigManager : SingletonO<AssetManageConfigManager>
{
    public static string strAssetManageConfigName = "constfolder/txt/assetmanageconfig";
    private AssetManageConfigFile m_AssetConfigFile = null;
    private AssetManageInfo[] m_Configs = null;

    public void Load()
    {     
        m_AssetConfigFile = XMLHelper.DeSerializerObjectResourceLoad(strAssetManageConfigName, typeof(AssetManageConfigFile)) as AssetManageConfigFile;
        if (m_AssetConfigFile != null)
            m_AssetConfigFile.ToLower();
        if (m_Configs == null)
            m_Configs = new AssetManageInfo[(int)ELoadType.Max];
        if (m_Configs != null && m_AssetConfigFile != null)
        {
            for (int i = 0; i < m_AssetConfigFile.infos.Count; ++i)
            {
                m_Configs[(int)m_AssetConfigFile.infos[i].loadType] = m_AssetConfigFile.infos[i];
            }
            m_AssetConfigFile.UnLoad();
            m_AssetConfigFile = null;
        }
    }

    public AssetManageInfo GetInfo(string key)
    {
        if (m_Configs == null)
        {
            Debug.Log("<color=red> assetmanageconfig file load fail: " + strAssetManageConfigName + ABPathHelper.AssetXmlSuffix + "</color>");   

            return AssetManageInfo.Default;
        }
        AssetManageInfo info = null;

        for (int i = 0; i < m_Configs.Length; ++i)
        {
            if (m_Configs[i] != null)
            {
                if (key.IndexOf(m_Configs[i].path, System.StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    info = m_Configs[i];
                    break;
                }
            }
        }

        return (info != null) ? info : AssetManageInfo.Default;
    }

    public AssetManageInfo GetInfo(ELoadType loadType)
    {
        if (m_Configs == null)
            return AssetManageInfo.Default;
        AssetManageInfo info = null;
        if ((int)loadType < (int)ELoadType.Max)
            info = m_Configs[(int)loadType];

        return (info != null) ? info : AssetManageInfo.Default;
    }

    public AssetManageConfigFile GetConfigFile()
    {
         return m_AssetConfigFile;
    }

    public void UnLoadAll()
    {
        if (m_AssetConfigFile != null)
        {
            m_AssetConfigFile.UnLoad();
            m_AssetConfigFile = null;
        }
        m_Configs = null;
        _instance = null;
    }
}
