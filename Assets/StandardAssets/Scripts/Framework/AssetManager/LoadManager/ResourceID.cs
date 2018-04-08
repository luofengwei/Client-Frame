using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using Ghost.Utils;

#if UNITY_EDITOR || UNITY_EDITOR_OSX
using System.Linq;
#endif

public class ResourceID
{
    public static string holderAssetPath = "Assets/ABResources/AutoGenerate/";
    //分隔符
    private const string SEPARATOR = "/";
    //资源文件夹
    public const string ResFolder = "Assets/Resources/";
    public const string ResABFolder = "Assets/ABResources/";
    //Lua文件夹
    public const string LuaFolder = "Script/";
    //获取资源的ID字典
    private static SDictionary<string, ResourceID> _cachedResID = new SDictionary<string, ResourceID>();
    //路径文件数据
    static PathID2Name _pathData;
    //转化后的真实路径
    string _ParsedRealPath;

    public string IDStr { get; private set; }

    public List<object> parts;

    //获取路径文件数据
    public static PathID2Name pathData
    {
        get { return _pathData; }
    }

    //空构造函数
    private ResourceID() { }

    //获取真实路径(每个bundle文件的路径)
    public string getRealPath
    {
        get
        {
            string res = _ParsedRealPath;
            if (string.IsNullOrEmpty(res))
            {
                //如果文件数据存在，那么拼出它的文件路径
                PathID2Name p = _pathData;
                if (_pathData != null)
                {
                    for (int i = 0; i < parts.Count; i++)
                    {
                        p = p[parts[i]];
                        if (p == null)
                            break;
                        res += "/" + p.name;
                    }
                }

                //如果当前的路径不为空，那么去掉第一个分隔符
                if (!string.IsNullOrEmpty(res))
                    res = res.Remove(0, 1);
                else
                    res = IDStr;

                //如果是Resource加载方式，那么直接使用本地的文件夹
#if (UNITY_EDITOR_OSX || UNITY_EDITOR)
                _ParsedRealPath = res;
#else
                _ParsedRealPath = res.ToLower();
#endif
            }
            return _ParsedRealPath;
        }
    }

    //重新生成路径字典
    public static void ReMap(string config)
    { 
#if(UNITY_EDITOR_OSX || UNITY_EDITOR)
        //如果使用本地脚本或是生成Bundle的时候，重新生成xml表
        if (GameUtils.ScriptLoad)
        {
            MakeAndReadFromResource();
        }
        else
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PathID2Name));
            StringReader sr = new StringReader(config);
            _pathData = (PathID2Name)serializer.Deserialize(sr);
            _pathData.ListToMap(true, _pathData, _pathData.name);
            sr.Close();  
        }
#else
        if (_pathData == null && config != null)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PathID2Name));
            StringReader sr = new StringReader(config);
            _pathData = (PathID2Name)serializer.Deserialize(sr);
            _pathData.ListToMap(true, _pathData, _pathData.name);
            sr.Close();
        }
#endif
    }

    //把Resource文件夹下的所有资源重新生成到_pathData，并且重新保存路径信息到AutoGenerate文件的pathIdMap.xml文件
    public static void MakeAndReadFromResource()
    {
#if UNITY_EDITOR_OSX || UNITY_EDITOR        
        _pathData = new PathID2Name("");     
        string[] files = Directory.GetFiles("Assets/ABResources/Script", "*.*", SearchOption.AllDirectories);
        //string[] files = Directory.GetFiles(ResourceID.ResFolder, "*.*", SearchOption.AllDirectories);
        for (int i = 0; i < files.Length; ++i)
        {
            //忽略.meta文件
            if (files[i].EndsWith(".meta"))
                continue;
            //忽略Matericl文件夹非prefab文件，也就是不打包png文件和atlas文件
            if (Path.GetDirectoryName(files[i]).EndsWith("Material") && !files[i].EndsWith(".prefab"))
                continue;
            //把选择出来的文件加入到_pathData列表下
            PathID2Name.Make(files[i], _pathData);
        }
        //重新保存
        string path = holderAssetPath + "pathIdMap.xml";
        XMLHelper.SerializerObject(path, _pathData);
#endif
    }

    public static ResourceID Make(params object[] parts)
    {
        string idStr = StringUtils.ConnectToStringWithSeparator(SEPARATOR, parts);
        ResourceID resID = _cachedResID[idStr];
        if (resID == null)
        {
            resID = new ResourceID();
            resID.parts = new List<object>();
            resID.IDStr = idStr;
            string[] splits = resID.IDStr.Split('/');
            for (int i = 0; i < splits.Length; i++)
            {
                int id;
                if (int.TryParse(splits[i], out id))
                    resID.parts.Add(id);
                else
                    resID.parts.Add(splits[i]);
            }
            _cachedResID[idStr] = resID;
        }
        return resID;
    }      

    //字符串转化为整形
    private static bool ParseInt(string str, out int id)
    {
        id = -1;
        try
        {
            id = int.Parse(str);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
