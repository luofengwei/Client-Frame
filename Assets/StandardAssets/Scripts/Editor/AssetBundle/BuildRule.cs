using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public enum EBuildRule
{
    //指定名字
    SpecifyName,
    //文件夹
    FolderName,
    //所在文件夹
    FileFolder,
    //自己文件名
    SelfName,
    //
}

public class BuildRule
{
    public EBuildRule rule { get; private set; }

    public string bundleName { get; private set; }

    public string checkFolder { get; private set; }


    public BuildRule(string folderName, string specifiedBundleName, EBuildRule rule)
    {
        this.rule = rule;
        bundleName = specifiedBundleName;
        if (folderName.EndsWith("/"))
            folderName = folderName.Remove(folderName.Length - 1);
        checkFolder = folderName;
    }

    public string GetBundleName(string path)
    {
        string bundleName = path;
        switch (rule)
        {
            case EBuildRule.FolderName:
                bundleName = checkFolder;
                break;
            case EBuildRule.FileFolder:
                bundleName = Path.GetDirectoryName(path);
                break;
            case EBuildRule.SelfName:
                break;
            case EBuildRule.SpecifyName:
                bundleName = this.bundleName;
                break;
        }
        return bundleName;
    }
}
