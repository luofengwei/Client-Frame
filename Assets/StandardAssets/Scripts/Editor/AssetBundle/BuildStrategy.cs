using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

//资源类型
public enum AssetType
{
    UnKnown,                            //位置资源
    Prefab,                             //预设
    Asset,                              //资源
    Text,                               //文件
    Model,                              //模型
    Anim,                               //动画
    Img,                                //图片
    Mat,                                //材质
    Shader,                             //shader
    Csharp,                             //C#脚本
}

public static partial class BuildStrategy
{
    //需要检测特殊规则的标识符
    public static string[] CheckExtension = new string[] { ".shader", ".mat", ".anim", ".prefab", ".png", ".tga", ".jpg", ".FBX", ".xml", ".txt", ".PNG", ".mor" };
    
    //设置跟包打包的AB所有签名
    public static void BuildBundleName_WithPak_Version()
    {
        //对所有需要打包成ab的资源进行签名
        BuildBundleName_Script();
        BuildBundleName_UI();
        BuildBundleName_Other();
    }

    public static void BuildBundleName_Script()
    {
        //生成pathidmap   针对Lua的脚本目录   
        CreateDataAsset();
        //打包Lua脚本
        BuildScriptTag();
    }

    public static void BuildBundleName_UI()
    {
        ////打包UI
        ScanAssets.ScanDependencies("ABResources/UI", new List<string>() { ".asset", ".prefab", ".xml", ".json", ".png", ".mat" }, null, HandlDataResources);

        //打包PIC
        ScanAssets.ScanDependencies("ABResources/Pic", new List<string>() { ".jpg", ".tga", ".png" }, null, HandlDataResources);
    }

    public static void BuildBundleName_Other()
    {
        //打包PathIdMap
        ScanAssets.ScanDependencies("ABResources/AutoGenerate", new List<string>() { ".xml" }, null, HandlDataResources);
        //打包TableConfig
        ScanAssets.ScanDependencies("ABResources/TableConfig", new List<string>() { ".txt" }, null, HandlDataResources);
        //打包GameConfig
        ScanAssets.ScanDependencies("ABResources/GameConfig", new List<string>() { ".xml", ".txt", }, null, HandlDataResources);
    }
    
    //打包脚本
    private static void BuildScriptTag()
    {
        ScanAssets.ScanDependencies("ABResources/Script", null, null, HandleScriptAndItsDepends);
    }

}
