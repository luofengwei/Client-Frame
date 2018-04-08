using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.IO;
using System.Text;

//	[System.Serializable]
public class PathID2Name
{
    [XmlAttribute("id")]
    public int id = -1;         //id属性
    [XmlAttribute("name")]
    public string name;         //名字属性

    //ID字典，          ID为Key,文件为value
    protected Dictionary<int, PathID2Name> _childrenIdMap = new Dictionary<int, PathID2Name>();

    //名称字典          名字为key，文件为value
    protected Dictionary<string, PathID2Name> _childrenNameMap = new Dictionary<string, PathID2Name>();
    [XmlIgnoreAttribute]
    public SDictionary<string, string> fileToFull = new SDictionary<string, string>();
    public List<PathID2Name> children = new List<PathID2Name>();

    private static Regex numName = new Regex("^[0-9]+");

    public PathID2Name this[object folder]
    {
        get
        {
            return folder is int ? this[(int)folder] : this[(string)folder];
        }
    }

    //通过I名字获取
    public PathID2Name this[string folder]
    {
        get
        {
            PathID2Name v = null;
            _childrenNameMap.TryGetValue(folder, out v);
            return v;
        }
    }

    //通过ID获取
    public PathID2Name this[int id]
    {
        get
        {
            PathID2Name v = null;
            _childrenIdMap.TryGetValue(id, out v);
            return v;
        }
    }

    //把文件添加到PathId2Name的字典中
    public static void Make(string path, PathID2Name root)
    {
        if (root != null)
        {
            //去除Assert/Resource的前缀
            path = path.Replace(ResourceID.ResABFolder, "");

            //分隔符
            string[] folders = path.Split('/', '\\', System.IO.Path.AltDirectorySeparatorChar, System.IO.Path.DirectorySeparatorChar);

            //如果是SVN，跳过
            if (System.Array.IndexOf(folders, ".svn") > -1)
            {
                return;
            }

            //如果是Script文件
            if (System.Array.IndexOf(folders, "Script") > -1)
            {
                //获取后缀名
                string extension = Path.GetExtension(path);
                //获取路径中最后一个文件名
                string file = Path.GetFileNameWithoutExtension(path);
                if (string.IsNullOrEmpty(extension))
                {
                    throw new System.Exception(string.Format("{0} {1}", "检查lua脚本是否没有后缀", path));
                }
                path = path.Replace(extension, "");
#if UNITY_EDITOR
                //如果有\\符号，替换成/符号
                path = path.Replace(System.IO.Path.DirectorySeparatorChar, '/');
#endif
                //把文件及其路径添加到root的文件字典下
                root.fileToFull[file] = path;
            }
            PathID2Name parent = root;
            StringBuilder full = new StringBuilder();
            for (int i = 0; i < folders.Length; ++i)
            {
                full.Append(folders[i]);
                full.Append("/");
                parent = parent.AddChild(folders[i], full.ToString());
            }
        }
    }

    public PathID2Name()
    {
    }

    //有参构造
    public PathID2Name(string folder)
    {
        Match packNum = numName.Match(folder);
        if (packNum.Success && packNum.Value != "")
        {
            this.id = ParseInt(packNum.Value);
        }
        this.name = System.IO.Path.GetFileNameWithoutExtension(folder);
        ReInit();
    }

    //重新初始化
    public void ReInit()
    {
        _childrenIdMap = new Dictionary<int, PathID2Name>();
        _childrenNameMap = new Dictionary<string, PathID2Name>();
        children = new List<PathID2Name>();
    }

    //添加路径，如果文件夹中有重复名字的文件，报错避免png文件的名字和atlas文件的名字一样
    public PathID2Name AddChild(string path, string fullPath)
    {
        PathID2Name id2Name;
        //如果名字字典中没有该文件的数据，才可以添加
        if (!_childrenNameMap.TryGetValue(path, out id2Name))
        {
            id2Name = new PathID2Name(path);
            //如果ID字典存在，并且不包含该文件的ID时，添加
            if (_childrenIdMap != null && _childrenIdMap.ContainsKey(id2Name.id) == false)
                _childrenIdMap.Add(id2Name.id, id2Name);

            //如果名字字典不为空
            if (_childrenNameMap != null)
            {
                //如果名字字典不包含目标名字，才添加，否则提示已经添加过相同名字的文件
                if (_childrenNameMap.ContainsKey(id2Name.name) == false)
                    _childrenNameMap.Add(id2Name.name, id2Name);
                else
                {
                    //Debug.LogError ("有重复文件名的资源！->" + fullPath);
                }
            }
            //bundle文件加入到文件夹列表中
            children.Add(id2Name);
        }
        return id2Name;
    }

    //Bundle文件添加到字典    是否是递归
    public void ListToMap(bool recursive = false, PathID2Name root = null, string path = "")
    {
        for (int i = 0; i < children.Count; i++)
        {
            if (recursive && children[i].hasChildren)
                children[i].ListToMap(recursive, root, (string.IsNullOrEmpty(path) ? "" : path + "/") + children[i].name);
            if (_childrenIdMap.ContainsKey(children[i].id) == false)
                _childrenIdMap.Add(children[i].id, children[i]);

            if (!_childrenNameMap.ContainsKey(children[i].name))
                _childrenNameMap.Add(children[i].name, children[i]);

            //因为Script是整个打包，所以不用循环
            if (path.Contains("Script") && !children[i].hasChildren)
            {
                root.fileToFull[children[i].name] = path + "/" + children[i].name;
            }
        }
    }

    //是否有子对象
    public bool hasChildren
    {
        get
        {
            return children != null && children.Count > 0;
        }
    }

    //ID字典中是否有该id的key
    public bool hasChild(int id)
    {
        return _childrenIdMap.ContainsKey(id);
    }

    //路径字典中是否有该路径的key
    public bool hasChild(string path)
    {
        return _childrenNameMap.ContainsKey(path);
    }

    private int ParseInt(string str)
    {
        try
        {
            return int.Parse(str);
        }
        catch
        {
            //Debug.LogError (string.Format ("{0} cannot parse to Int", str));
            return -1;
        }
    }
}
