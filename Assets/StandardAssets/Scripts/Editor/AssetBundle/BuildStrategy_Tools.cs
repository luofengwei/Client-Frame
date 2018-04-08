using UnityEngine;
using System;
using System.IO;
using UnityEditor;

public static partial class BuildStrategy
{
    //重置Assets为空
    static string TrimAssets(string path)
    {
        return path.Replace("Assets/", "").Replace("Assets\\", "");
    }

    //获取资源类型
    public static AssetType GetAssetType(string extension)
    {
        if (extension.Contains(Path.DirectorySeparatorChar.ToString()))
            extension = Path.GetExtension(extension);
        switch (extension)
        {
            case ".prefab":
                return AssetType.Prefab;
            case ".shader":
                return AssetType.Shader;
            case ".cs":
                return AssetType.Csharp;
            case ".asset":
                return AssetType.Asset;
            case ".anim":
                return AssetType.Anim;
        }
        return AssetType.UnKnown;
    }


    /// <summary>
    /// 设置脚本的PathIdMap规则
    /// </summary>
    public static void CreateDataAsset()
    {
        if (!Directory.Exists(ResourceID.holderAssetPath))
            Directory.CreateDirectory(ResourceID.holderAssetPath);
        ResourceID.MakeAndReadFromResource();
        AssetDatabase.Refresh();
    }


    //设置当前的lua文件为Resources/Script
    private static void HandleScriptAndItsDepends(string path)
    {
        if (path.Contains(" "))
        {
            //Debug.LogError("当前打包路径存在空格 : " + path);
            return;
        }

        AssetImporter importer = AssetImporter.GetAtPath(path);
        if (path.EndsWith(".txt"))
            InternalSetScriptName(null, TrimAssets, importer, "ABResources/Script");
        else
            HandleClearSelect(path);                //清除标记
    }

    //设置lua脚本的规则
    private static void InternalSetScriptName(string path, Func<string, string> modifyPath, AssetImporter importer = null, string bundleName = null)
    {
        if (importer == null)
            importer = AssetImporter.GetAtPath(path);
        if (string.IsNullOrEmpty(bundleName))
            bundleName = modifyPath(path);
        //如果bundleName有后缀，需要删除
        string extension = Path.GetExtension(bundleName);
        if (!string.IsNullOrEmpty(extension))
            bundleName = bundleName.Replace(extension, "");
        importer.assetBundleName = bundleName + ABPathHelper.Unity3dSuffix;
    }

    static void HandleClearSelect(string path)
    {
        if (GetAssetType(Path.GetExtension(path)) == AssetType.Csharp)
            return;
        AssetImporter importer = AssetImporter.GetAtPath(path);
        if (importer == null)
        {
            //Debug.LogError("path import is null"+path);
            return;
        }
        importer.assetBundleName = null;
        importer = null;
    }

    //处理Data
    static void HandlDataResources(string path)
    {
        path = path.Trim();

        AssetImporter importer = AssetImporter.GetAtPath(path);
        string bundleName = null;
        if (InternalCheckSet(path, CheckExtension, "", out bundleName))
            InternalSetBundleNameByRule(path, TrimAssets, importer);
        else
            InternalSetBundleName(path, TrimAssets, importer);
    }

    //检查是否设置
    static bool InternalCheckSet(string path, string[] endWiths, string fixName, out string bundleName)
    {
        bool isEndWith = false;
        foreach (string endwith in endWiths)
        {
            if (path.EndsWith(endwith))
            {
                isEndWith = true;
                break;
            }
        }
        if (isEndWith)
        {
            string[] directories = Path.GetDirectoryName(path).Split(Path.DirectorySeparatorChar);
            bundleName = Path.GetDirectoryName(path) + "/" + directories[directories.Length - 1] + fixName;
            return true;
        }
        bundleName = null;
        return false;
    }

    //通过规则设置Bundle的名称
    static void InternalSetBundleNameByRule(string path, Func<string, string> modifyPath, AssetImporter importer = null, string bundleName = null)
    {
        if (importer == null)
            importer = AssetImporter.GetAtPath(path);
        if (string.IsNullOrEmpty(bundleName))
            bundleName = modifyPath(path);
        string extension = Path.GetExtension(bundleName);
        if (string.IsNullOrEmpty(extension) == false)
            bundleName = bundleName.Replace(extension, "");
        string file_name = bundleName;
        BuildRule rule = BundleBuildRule.MatchRule(path);
        if (rule != null)
            bundleName = rule.GetBundleName(bundleName);
        if (bundleName.Contains("Material"))
        {
            int index = bundleName.IndexOf("Material");
            bundleName = bundleName.Substring(0, index - 1);
        }
        else if (bundleName.Contains("Materials"))
        {
            //针对模型导入
            int index = bundleName.IndexOf("Materials");
            bundleName = bundleName.Substring(0, index - 1);
        }
        if (string.IsNullOrEmpty(bundleName))
        {
            importer.assetBundleName = string.Empty;
        }
        else
        {
            importer.assetBundleName = bundleName + ABPathHelper.Unity3dSuffix;
        }
    }

    //无规则设置bundle名称             路径              修改路径的方法                                             bundle的名字
    static void InternalSetBundleName(string path, Func<string, string> modifyPath, AssetImporter importer = null, string bundleName = null)
    {
        if (importer == null)
            importer = AssetImporter.GetAtPath(path);
        if (string.IsNullOrEmpty(bundleName))
            bundleName = modifyPath(path);
        //如果bundleName有后缀，需要删除
        string extension = Path.GetExtension(bundleName);
        if (!string.IsNullOrEmpty(extension))
            bundleName = bundleName.Replace(extension, "");
        string file_name = bundleName;
        BuildRule rule = BundleBuildRule.MatchRule(path);
        if (rule != null)
            bundleName = rule.GetBundleName(bundleName);
        if (bundleName.Contains("Material"))
        {
            //针对UI
            int index = bundleName.IndexOf("Material");
            bundleName = bundleName.Substring(0, index - 1);
        }
        else if (bundleName.Contains("Materials"))
        {
            //针对模型导入
            int index = bundleName.IndexOf("Materials");
            bundleName = bundleName.Substring(0, index - 1);
        }
        if (bundleName.Contains(" "))
        {
            Debug.LogError("当前bundleName存在空格 : " + bundleName);
            return;
        }

        importer.assetBundleName = bundleName + ABPathHelper.Unity3dSuffix;
    }
}
