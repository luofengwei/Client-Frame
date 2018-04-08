using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public static class BundleBuildRule
{
    static SDictionary<string, BuildRule> _buildRules = new SDictionary<string, BuildRule>();

    #region 基础接口
    public static BuildRule GetRule(string name)
    {
        return _buildRules[name];
    }

    //匹配规则
    public static BuildRule MatchRule(string name)
    {
        foreach (KeyValuePair<string, BuildRule> kvp in _buildRules)
        {
            if (name.Contains(kvp.Key))
                return kvp.Value;
        }
        return null;
    }

    static BundleBuildRule()
    {
        //设置Data目录的筛选
        SetDataFolderRule();

        //设置ABResource目录的筛选
        SetABResourceFolderRule();

        //设置Resource目录的筛选
        SetResourceFolderRule();
    }

    static void AddRule(string folder, string specifyName, EBuildRule rule)
    {
        _buildRules[folder] = new BuildRule(folder, specifyName, rule);
    }
    #endregion

    #region Data目录的AB签名规则

    static void SetDataFolderRule()
    {
    }

    #endregion

    #region ABResources目录的AB签名规则

    static void SetABResourceFolderRule()
    {                
        //UI
        AddRule("ABResources/UI", null, EBuildRule.FileFolder);
        //PIC
        AddRule("ABResources/Pic", null, EBuildRule.SelfName);
        //Icon
        //AddRule("ABResources/Icon", null, EBuildRule.SelfName);
        //config
        AddRule("ABResources/GameConfig", null, EBuildRule.FolderName);
        //tabel
        AddRule("ABResources/TableConfig", null, EBuildRule.FolderName);
        //设置pathidmap
        AddRule("ABResources/AutoGenerate", null, EBuildRule.SelfName);
    }
    #endregion

    #region Resources目录的AB签名规则

    static void SetResourceFolderRule()
    {     
    }

    #endregion
}
