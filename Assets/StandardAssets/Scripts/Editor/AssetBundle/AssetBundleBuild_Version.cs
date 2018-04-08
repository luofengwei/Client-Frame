using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public static partial class AssetBundleBuild
{
    #region 打版本使用-基础包流程
    [MenuItem("AssetBundle/版本机使用-基础包流程/全部执行", false, 2000)]
    public static void BaseVersion()
    {
        //清除非本平台的StreamingAsset包
        ClearOtherPlatBundles_WithPak();

        BaseVersionStep1();
        BaseVersionStep2();
    }

    private static void BaseVersionStep1()
    {
        //1.先删除跟包的AB目录
        ClearAllBundles_WithPak();
        ClearAllBundles_WithLocalAB();

        //2.制作跟包的AB目录
        BuildBundleNameMenu_WithPak_Version();
    }

    private static void BaseVersionStep2()
    {
        //1.最后删除下载的AB目录
        ClearAllBundles_AssetBundles();
    }

    //跟包用的AB打包-打包到LocalAB目录下
    private static void BuildBundleNameMenu_WithPak_Version()
    {
        menuStart("跟包AB打包", sw);

        //1.跟包的AB的打包命名
        BuildStrategy.BuildBundleName_WithPak_Version();

        //2.开始打包需要跟包的非压缩AB包
        BuildAssetBundles(outputLocalABPath);

        //3.清除所有的BundleName
        ClearAllBundleNameMenu();
        
        //4.将非压缩的AB压缩存放到StreamingAssets目录下
        VersionManager.CopyDir(ABPathHelper.GetAddPrefixPath(outputLocalABPath), ABPathHelper.GetAddPrefixPath(outputStreamingAssetsPath));
        ////5.生成标记文件
        SaveOrCreateDecompressSuccess();

        menuEnd("跟包AB打包", sw);
    }

    private static void SaveOrCreateDecompressSuccess()
    {
        string strDecompressSuccess = Application.dataPath;
        strDecompressSuccess = strDecompressSuccess.Substring(0, strDecompressSuccess.Length - 6) + "LocalAB/" + ABPathHelper.platformFolder + "/" + ABPathHelper.DecompressSuccess;
        string strDirName = Path.GetDirectoryName(strDecompressSuccess);
        if (!Directory.Exists(strDirName))
        {
            Directory.CreateDirectory(strDirName);
        }
        using (FileStream fs = new FileStream(strDecompressSuccess, FileMode.OpenOrCreate))
        {
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.Write(AppVerConfig.curAppVer);
            }
        }
    }

    #endregion

    #region 打版本使用-增量包流程

    #endregion
}
