using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public static partial class AssetBundleBuild
{
    public static string outputAssetBundlesPath = Path.Combine("AssetBundles/", ABPathHelper.platformFolder);
    public static string outputStreamingAssetsPath = Path.Combine("Assets/StreamingAssets/", ABPathHelper.platformFolder);
    public static string outputLocalABPath = Path.Combine("LocalAB/", ABPathHelper.platformFolder);
    public static string outputLocalCachePath = Path.Combine("LocalCache/", ABPathHelper.platformFolder);

    private static System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
    #region 基础接口

    private static void menuStart(string menuName, System.Diagnostics.Stopwatch sw)
    {
        Debug.Log(menuName);
        sw.Start();
    }

    private static void menuEnd(string menuName, System.Diagnostics.Stopwatch sw)
    {
        sw.Stop();
        Debug.Log(string.Format("{0}:{1}", menuName, sw.Elapsed.TotalMilliseconds / 1000));
    }
    #endregion

    #region 清除本地的目录

    [MenuItem("AssetBundle/清除目录/全部执行", false, 1000)]
    public static void ClearAllBundles()
    {
        ClearAllBundles_AssetBundles();
        ClearAllBundles_WithDownload();
        ClearAllBundles_WithPak();
    }

    [MenuItem("AssetBundle/清除目录/清除AssetBundles目录", false, 1050)]
    public static void ClearAllBundles_AssetBundles()
    {
        if (Directory.Exists(outputAssetBundlesPath))
            Directory.Delete(outputAssetBundlesPath, true);

        Debug.Log("<color=yellow>清除AssetBundles目录</color>");
    }

    [MenuItem("AssetBundle/清除目录/清除LocalCache下载目录", false, 1051)]
    public static void ClearAllBundles_WithDownload()
    {
        if (Directory.Exists(outputLocalCachePath))
            Directory.Delete(outputLocalCachePath, true);

        Debug.Log("<color=yellow>清除LocalCache下载目录</color>");
    }

    [MenuItem("AssetBundle/清除目录/清除LocalAB本地目录", false, 1052)]
    public static void ClearAllBundles_WithLocalAB()
    {
        if (Directory.Exists(outputLocalABPath))
            Directory.Delete(outputLocalABPath, true);

        Debug.Log("<color=yellow>清除LocalAB本地目录</color>");
    }

    [MenuItem("AssetBundle/清除目录/清除StreamingAssets跟包目录", false, 1053)]
    public static void ClearAllBundles_WithPak()
    {
        if (Directory.Exists(outputStreamingAssetsPath))
            Directory.Delete(outputStreamingAssetsPath, true);
        AssetDatabase.Refresh();

        Debug.Log("<color=yellow>清除StreamingAssets跟包目录成功</color>");
    }
    
    /// <summary>
    /// 清除非本平台的StreamingAssets包
    /// </summary>
    public static void ClearOtherPlatBundles_WithPak()
    {
        string delPath = "";
        if (ABPathHelper.platformFolder.Equals("iOS"))
        {
            delPath = "Assets/StreamingAssets/Android";
        }
        else
        {
            delPath = "Assets/StreamingAssets/iOS";
        }

        if (Directory.Exists(delPath))
            Directory.Delete(delPath, true);
        AssetDatabase.Refresh();
    }

    #endregion

    #region AB打包的接口相关

    /// <summary>
    /// 打包跟包的AB包
    /// </summary>
    public static void BuildAssetBundles(string outPath)
    {
        if (!Directory.Exists(outPath))
            Directory.CreateDirectory(outPath);

        //保证pathidmap存在
        BuildStrategy.CreateDataAsset();
        //                                                          压缩方式            设置的平台格式
        BuildPipeline.BuildAssetBundles(outPath, BuildAssetBundleOptions.UncompressedAssetBundle | BuildAssetBundleOptions.DeterministicAssetBundle, EditorUserBuildSettings.activeBuildTarget);

        //拷贝Movie目录到outPath
        CopyMovieFiles(outPath);

        BuildBundleMD5Files(outPath);
    }

    /// <summary>
    /// 生成目录下所有AssertBundle信息文件的MD5信息
    /// </summary>
    /// <param name="abPath"></param>
    public static void BuildBundleMD5Files(string abPath)
    {
        string assetBundlePath = abPath;// Path.Combine(abPath, ABPathHelper.platformFolder);
        if (!Directory.Exists(assetBundlePath))
            return;

        //先删除AllGameConfig文件
        string config_path = ABPathHelper.GetCombinePath(assetBundlePath, ABPathHelper.localVerMD5File);
        if (File.Exists(config_path))
            File.Delete(config_path);

        Dictionary<string, string> newFileMD5Map = new Dictionary<string, string>();
        string[] files = Directory.GetFiles(assetBundlePath, "*", SearchOption.AllDirectories);
        FileStream fs = new FileStream(assetBundlePath + "/" + ABPathHelper.localVerMD5File, FileMode.OpenOrCreate);

        using (StreamWriter sw = new StreamWriter(fs))
        {
            string filePath = "";
            string fileMd5 = string.Empty;
            string fileExt = "";
            string fileName = "";
            long fileSize = 0;
            foreach (string file in files)
            {
#if UNITY_EDITOR_OSX
            filePath = file.Replace(assetBundlePath + "/", "");
#else
                filePath = file.Replace(assetBundlePath + "\\", "");
#endif
                if (filePath.Contains(ABPathHelper.localVerMD5File))
                    continue;
                fileExt = Path.GetExtension(filePath);
                fileName = Path.GetFileNameWithoutExtension(filePath);
                if (!fileExt.Equals(ABPathHelper.Unity3dSuffix) && !fileExt.Equals(ABPathHelper.MP4Suffix))
                    continue;
                //判断是否与空格
                if (filePath.Contains(" "))
                {
                    Debug.Log("<color=red>有问题资源：</color>" + filePath);
                    continue;
                }

                CUtility.MD5Hash(file, ref fileMd5, ref fileSize);

                sw.WriteLine(string.Format("{0}*{1}*{2}", filePath, fileMd5, fileSize));
            }
        }
        fs.Close();
        fs.Dispose();
    }

    [MenuItem("AssetBundle/清除目录/清除所有AssetBundle名字", false, 1100)]
    public static void ClearAllBundleNameMenu()
    {
        //清除ABResources
        ScanAssets.ScanDependencies(Path.Combine(Application.dataPath, ABPathHelper.ABResourcePrefix), null, null, HandleClearSelect);

        //清除Data
        ScanAssets.ScanDependencies(Path.Combine(Application.dataPath, ABPathHelper.DataPrefix), null, null, HandleClearSelect);

        //清除Resources
        ScanAssets.ScanDependencies(Path.Combine(Application.dataPath, ABPathHelper.ResourcePrefix), null, null, HandleClearSelect);

        //AssetDatabase.RemoveUnusedAssetBundleNames();
        AssetDatabase.Refresh();

        Debug.Log("<color=yellow>清除所有AssetBundle名字</color>");
    }

    static void HandleClearSelect(string path)
    {
        //除去C#脚本
        if (BuildStrategy.GetAssetType(Path.GetExtension(path)) == AssetType.Csharp)
            return;
        AssetImporter importer = AssetImporter.GetAtPath(path);
        if (importer != null)
        {
            importer.assetBundleName = null;
            importer = null;
        }
    }

    #endregion


    //[MenuItem("AssetBundle/LuaPathIDGenerate", false, 0)]
    public static void LuaPathIDGenerate()
    {
        if (!Directory.Exists(ResourceID.holderAssetPath))
            Directory.CreateDirectory(ResourceID.holderAssetPath);
        ResourceID.MakeAndReadFromResource();
        AssetDatabase.Refresh();
    }

    private static void CopyMovieFiles(string outPath)
    {
        string strSrcPath = Application.dataPath;
        strSrcPath = strSrcPath.Replace("Assets", "Movie");
        outPath += "/Movie";

        CopyDir(strSrcPath, outPath);
    }

    private static void CopyDir(string srcPath, string tgtPath)
    {
        // 检查目标目录是否以目录分割字符结束如果不是则添加之
        if (tgtPath[tgtPath.Length - 1] != Path.DirectorySeparatorChar)
            tgtPath += Path.DirectorySeparatorChar;
        // 判断目标目录是否存在如果不存在则新建之
        if (!Directory.Exists(tgtPath))
            Directory.CreateDirectory(tgtPath);
        // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
        // 如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法
        // string[] fileList = Directory.GetFiles(srcPath);
        string[] fileList = Directory.GetFileSystemEntries(srcPath);
        // 遍历所有的文件和目录
        foreach (string file in fileList)
        {
            // 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
            if (Directory.Exists(file))
                CopyDir(file, tgtPath + Path.GetFileName(file));
            // 否则直接Copy文件
            else
            {
                //if (setDiffWithPakList.Count != 0)
                //{
                //    string fileBundleName = file.Replace("/", "\\");
                //    int subIndex = fileBundleName.IndexOf("Movie");
                //    if (subIndex >= 0)
                //    {
                //        fileBundleName = fileBundleName.Substring(subIndex);
                //        Debug.Log("<color=red>diffCheckBundleName:" + fileBundleName + "</color>");
                //        if (!setDiffWithPakList.Contains(fileBundleName))
                //        {
                //            continue;
                //        }
                //    }
                //}
                File.Copy(file, tgtPath + Path.GetFileName(file), true);
            }
        }
    }
}
