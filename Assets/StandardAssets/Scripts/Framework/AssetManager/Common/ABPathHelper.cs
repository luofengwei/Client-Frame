///author       xuhan
///Data         2016.07.19
///Description

using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

//5.Resources

//可以在根目录下，也可以在子目录里，只要名子叫Resources就可以。比如目录：/xxx/xxx/Resources  和 /Resources 是一样的，
//无论多少个叫Resources的文件夹都可以。Resources文件夹下的资源不管你用还是不用都会被打包进.apk或者.ipa

//Resource.Load ：编辑时和运行时都可以通过Resource.Load来直接读取。

//Resources.LoadAssetAtPath() ：它可以读取Assets目录下的任意文件夹下的资源，它可以在编辑时或者编辑器运行时用，它但是它
//不能在真机上用，它的路径是”Assets/xx/xx.xxx” 必须是这种路径，并且要带文件的后缀名。

//AssetDatabase.LoadAssetAtPath()：它可以读取Assets目录下的任意文件夹下的资源，它只能在编辑时用。它的路径是”Assets/xx/xx.xxx”
//必须是这种路径，并且要带文件的后缀名。

//我觉得在电脑上开发的时候尽量来用Resource.Load() 或者 Resources.LoadAssetAtPath() ，假如手机上选择一部分资源要打assetbundle，
//一部分资源Resource.Load().那么在做.apk或者.ipa的时候 现在都是用脚本来自动化打包，在打包之前 可以用AssetDatabase.MoveAsset()
//把已经打包成assetbundle的原始文件从Resources文件夹下移动出去在打包，这样打出来的运行包就不会包行多余的文件了。打完包以后再把移
//动出去的文件夹移动回来。

//6. StreamingAssets

//这个文件夹下的资源也会全都打包在.apk或者.ipa 它和Resources的区别是，Resources会压缩文件，但是它不会压缩原封不动的打包进去。并且
//它是一个只读的文件夹，就是程序运行时只能读 不能写。它在各个平台下的路径是不同的，不过你可以用Application.streamingAssetsPath 它
//会根据当前的平台选择对应的路径。

//有些游戏为了让所有的资源全部使用assetbundle，会把一些初始的assetbundle放在StreamingAssets目录下，运行程序的时候在把这些
//assetbundle拷贝在Application.persistentDataPath目录下，如果这些assetbundle有更新的话，那么下载到新的assetbundle在把
//Application.persistentDataPath目录下原有的覆盖掉。

//因为Application.persistentDataPath目录是应用程序的沙盒目录，所以打包之前是没有这个目录的，直到应用程序在手机上安装完毕才
//有这个目录。

//StreamingAssets目录下的资源都是不压缩的，所以它比较大会占空间，比如你的应用装在手机上会占用100M的容量，那么你又在StreamingAssets
//放了一个100M的assetbundle，那么此时在装在手机上就会在200M的容量。


//Unity3d的Resource、AssetBundle与手游动态更新的报告，在这里分享一下，希望能够对各位用Unity的朋友有些许帮助。
//目录：
//1.Unity的资源数据加载
//2.Resource、StreamingAsset文件夹，安装后的路径（Android，iOS）
//3.Unity在打包和安装的时候怎么处理persistentDataPath
//4.Unity的Android和IOS上相关的目录结构
//5.Unity常用目录对应的Android && iOS平台地址


//1.Unity的资源数据加载 - Resources、AssetBundle、StreamingAsset、PersistentDataPath
//Resources 
//- 打包集成到.asset文件里面及引用的资源as后se一个文件里面面
//- 主线程加载
//- 想要动态更新资源则不考虑
//AssetBundle 
//- unity定义的二进制文件类型
//- 用WWW类下载

//StreamingAssets
//- 可读不可写
//- 内容限制 - 无 
//- 只能用WWW类下载
//PersistentDataPath目录下
//- 可读可写
//- 内容限制 - 无
//- 清除手机缓存文件会一并清理这里的东西
//- 随意弄，可作为本地目录让WWW下载、也可以自己用FileInfo乱整

//2.Resource、StreamingAsset文件夹，安装后的路径（Android，iOS）

//StreamingAsset 
//- iOS : Application.dataPath + /Raw
//- Android : jar:file:// + Application.dataPath + !/assets/


//Resources
//- 打包成一个Asset文件

//3.Unity在打包和安装的时候怎么处理PersistentDataPath
//- PersistentDataPath- 就是com.**.**/files 的路径而已 


//4.Unity的Android和IOS上相关的目录结构
//Android:
//- assets 游戏内容相关的都在这里了
//- lib JNI相关的东西
//- META-INF Java包跟rar包的区别
//- res 图标之类的
//- AndroidManifest.xml Android配置文件
//- classes.dex Java虚拟机runtime的东西
//- resources.arsc Java编译后的二进制文件

//IOS:
//- level0/level1… Scene
//- sharedassets0/shaedassets1/… Scene相关的东西
//- Managed 脚本编译后的dll
//- resources.assets Resources里面的东西
//- Raw StreamingAssets里面的东西

//5. Unity常用目录对应的Android && iOS平台地址
//IOS:
//Application.dataPath : Application/xxxxx/xxx.app/Data
//Application.streamingAssetsPath : Application/xxxxx/xxx.app/Data/Raw
//Application.persistentDataPath : Application/xxxxx/Documents
//Application.temporaryCachePath : Application/xxxxx/Library/Caches


//Android:
//Application.dataPath : /data/app/xxx.xxx.xxx.apk
//Application.streamingAssetsPath : jar:file:///data/app/xxx.xxx.xxx.apk/!/assets
//Application.persistentDataPath : /data/data/xxx.xxx.xxx/files
//Application.temporaryCachePath : /data/data/xxx.xxx.xxx/cache

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ABPathHelper
{
    public static string DataPrefix = "data/";
    public static string ResourcePrefix = "resources/";
    public static string ABResourcePrefix = "abresources/";
    public static string Unity3dSuffix = ".unity3d";
    public static string MP4Suffix = ".mp4";
    public static string AssetPrefix = "assets/resources/";
    public static string AssetABPrefix = "assets/abresources/";
    public static string AssetNoResourcePrefix = "assets/";
    public static string FontPrefix = "uiscene/font/";
    public static string PrefabSuffix = ".prefab";
    public static string ShaderSuffix = ".shader";
    public static string ManifestSuffix = ".manifest";
    public static string AssetXmlSuffix = ".xml";
    public static string AssetTxtSuffix = ".txt";
    public static string AssetAniSuffix = ".anim";
    public static string AssetPngSuffix = ".png";
    public static string Separator = "/";

    public static string GameConfigPath = "gameconfig";
 
    public static string GamePreLoadConfigFile = "gamepreloadconfig";  // 游戏预加载文件

    /// <summary>
    /// 预加载资源的路径
    /// </summary>
    public static string ResourceConstFloder = "resources/constfolder";
    public static string ResourceAssetPerLoad = "resources/constfolder/ui/preload.unity3d";
    public static string ResourceAssetPerLoadMiniFest = "resources/constfolder/ui/preload.unity3d.manifest";

    public static string ResourceAnimation = "abresources/animation";
    public static string ResourceAvatar = "abresources/avatar";
    public static string ResourceCharacterui = "abresources/ui";

    public static string ResourceMaleAnim = "abresources/animation/male";
    public static string ResourceFemaleAnim = "abresources/animation/female";
    public static string ResourceFemaleHairAnim = "abresources/animation/hair";
    public static string ResourceBasketryAnim = "abresources/animation/basketry";

    public static string ResourceMatchRes = "abresources/matchres";

    public static string ResourceBaseEffect = "abresources/effect/effectbase";
    public static string ResourceGameEffect = "abresources/effect/othereffect";
    public static string ResourceMatchEffect = "abresources/effect/matcheffect";
    public static string ResourceMatchEffectGuide = "abresources/effect/matchguide";
    public static string ResourceMatchEffectPVP = "abresources/effect/matchpvp";
    public static string ResourceMatchEffectBuff = "abresources/effect/matchbuff";

    public static string ResourceMatchCV = "abresources/sound/sound/matchcv";
    public static string ResourceMatchRV = "abresources/sound/sound/matchrv";
    public static string ResourceMatchSound = "abresources/sound/sound/matchsound";
    public static string ResourceRoleTouchSound = "abresources/sound/sound/hallrv";
    public static string ResourceRoleChooseSound = "abresources/sound/sound/othersound";
    public static string ResourceGuideSound = "abresources/sound/sound/guidesound";
    public static string ResourceUISound = "abresources/sound/sound/uisound";
    public static string ResourceGameMusic = "abresources/sound/music";
    public static string ResourceStartRV = "abresources/sound/sound/startrv";

    /// <summary>
    /// 切屏动画的预设路径
    /// </summary>
    public static string EffectCutScreen = "Effect/MatchPVP/CutScreenRoot";
    
    //版本相关文件
    public static string localVerInfoFile = "VersionInfo.txt";
    public static string localVerMD5File = "VersionMD5.txt";

    public static string RemoteBaseMD5File = "BaseMD5.txt";
    public static string RemoteDiffMD5File = "DiffMD5.txt";

    public static string DecompressSuccess = "DecompressSuccess.txt";

    private static string CacheBasePath = string.Empty;
    private static string LocalCacheDataPath = string.Empty;
    private static string ABBasePath = string.Empty;
    private static string LocalABDataPath = string.Empty;

    public static string FTPDiffPath = string.Empty;
    public static string BasePackPath = string.Empty;
    public static string AssetsURL = string.Empty;
    public static string AssetsLocalABURL = string.Empty;
    public static string AssetsStreamURL = string.Empty;
    public static string AssetsWWWStreamURL = string.Empty;

    // 获取外网的下载路径
    public static string GetUrlRealPath(string path, bool isBase = false)
    {
		if (isBase)
			return GameUtils.GetCombinePath(BasePackPath, path);//读取基础包版本地址
        else
			return GameUtils.GetCombinePath(FTPDiffPath, path); //读取新地址
	}

    //获取本地的下载路径(没有考虑下载类型区分的文件路径)
    public static string GetCacheRealPath(string path)
    {
        return GameUtils.GetCombinePath(LocalCacheDataPath, path);
    }

    public static int cHashNameLen = 16;
    public static string HashPath(string path)
    {
        return CUtility.MD5HashString(path).Substring(0, cHashNameLen);
    }
 
    public static void InitCachePath()
    {
#if (UNITY_EDITOR_OSX || UNITY_EDITOR)
        CacheBasePath = Application.dataPath;
        ABBasePath = Application.dataPath;
        if (GameUtils.UseSelfBundle)
        {
            //使用自己的bundle       //xxxx/AssetBundles                 
            CacheBasePath = CacheBasePath.Substring(0, CacheBasePath.Length - 6) + "AssetBundles";
        }
        else
        {
            //使用localcache         //xxxx/localcache               
            CacheBasePath = CacheBasePath.Substring(0, CacheBasePath.Length - 6) + "LocalCache";
        }
        ABBasePath = ABBasePath.Substring(0, ABBasePath.Length - 6) + "LocalAB";
#else
        CacheBasePath = Application.persistentDataPath; 
        ABBasePath = Application.persistentDataPath + "/LocalAB"; 
#endif

        LocalCacheDataPath = GameUtils.GetCombinePath(CacheBasePath, platformFolder); 
        AssetsURL = LocalCacheDataPath;
        AssetsURL += ABPathHelper.Separator;

        LocalABDataPath = GameUtils.GetCombinePath(ABBasePath, platformFolder);
        AssetsLocalABURL = LocalABDataPath;
        AssetsLocalABURL += ABPathHelper.Separator;

        AssetsStreamURL = CUtility.GetLocalResPath();
        AssetsStreamURL += platformFolder;
        AssetsStreamURL += ABPathHelper.Separator;

        AssetsWWWStreamURL = CUtility.GetLocalResPathByWWW();
        AssetsWWWStreamURL += platformFolder;
        AssetsWWWStreamURL += ABPathHelper.Separator;
    }

    public static string platformFolder
    {
        get
        {
#if (UNITY_EDITOR_OSX || UNITY_EDITOR)
            return GetPlatformFolderForAssetBundles(EditorUserBuildSettings.activeBuildTarget);
#else
			return GetPlatformFolderForAssetBundles(Application.platform);
#endif
        }
    }

#if UNITY_EDITOR
    //获取编译目标的平台名称
    public static string GetPlatformFolderForAssetBundles(BuildTarget target)
    {
        switch (target)
        {
            case BuildTarget.Android:
                return "Android";
            case BuildTarget.iOS:
                return "iOS";
            case BuildTarget.WebPlayer:
                return "WebPlayer";
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                return "Windows";
            case BuildTarget.StandaloneOSXIntel:
            case BuildTarget.StandaloneOSXIntel64:
            case BuildTarget.StandaloneOSXUniversal:
                return "OSX";
            // Add more build targets for your own.
            // If you add more targets, don't forget to add the same platforms to GetPlatformFolderForAssetBundles(RuntimePlatform) function.
            default:
                return null;
        }
    }
#endif

    public static string GetPlatformFolderForAssetBundles(RuntimePlatform platform)
    {
        switch (platform)
        {
            case RuntimePlatform.Android:
                return "Android";
            case RuntimePlatform.IPhonePlayer:
                return "iOS";
            case RuntimePlatform.WindowsWebPlayer:
            case RuntimePlatform.OSXWebPlayer:
                return "WebPlayer";
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                return "Windows";
            case RuntimePlatform.OSXPlayer:
                return "OSX";
            // Add more build platform for your own.
            // If you add more platforms, don't forget to add the same targets to GetPlatformFolderForAssetBundles(BuildTarget) function.
            default:
                return null;
        }
    }  

	public static void recreatePath(string path)
	{
		if (Directory.Exists(path))
		{
			Directory.Delete(path, true);
		}
		Directory.CreateDirectory(path);
	}

	//生成中间路径
	public static  void tryCreatePath(string path)
	{
		string path2Create = "";
		string[] paths = path.Split(new char[]{'/', '\\'});
		for (int i = 0; i < paths.Length; ++i)
		{
			path2Create += paths[i];
			if (!string.IsNullOrEmpty(paths[i]) && !Directory.Exists(path2Create))
			{
				//Debug.Log("path2Create:" + path2Create);
				Directory.CreateDirectory(path2Create);  
			}
            path2Create += ABPathHelper.Separator;
		}
	}

    //拼接地址
    public static string GetCombinePath(string path1, string path2, params string[] path3)
    {
        StringBuilder back = new StringBuilder();
        back.Append(path1);
        back.Append(ABPathHelper.Separator);
        back.Append(path2);
        if (path3 != null)
        {
            for (int i = 0; i < path3.Length; ++i)
            {
                back.Append(ABPathHelper.Separator);
                back.Append(path3[i]);
            }
        }
        return back.ToString();
    }

    /// <summary>
    /// 获取全路径名称
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetAddPrefixPath(string path)
    {
        string ret = Application.dataPath;
        int index = ret.LastIndexOfAny(new char[] { '/', '\\' }) + 1;
        ret = ret.Substring(0, index);
        ret = ret + path;
        return ret;
    }
}