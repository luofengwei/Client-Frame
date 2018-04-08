using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using GameFramework;

//以下方法用的Assetbundle都是已经下载到本地SD卡再读取的加载方式	
//开始load到生成Assetbundle时间	内存Used Total变化
//一	WWW.LoadFromCacheOrDownload(string url, int version)	0.084s	51.6M-78.9M-104M
//二	Assetbundle.CreateFromFile(string path)	0.012s	52M-78M-103M
//三	Assetbndle.CreateFromMemory(byte[] bs)	2.599s	51M-108M-195M-217M

//一，WWW.LoadFromCacheOrDownload(string url， Int version): 
//异步方法 
//开始load到生成Assetbundle时间：0.084s 
//内存Used Total变化：51.6M-78.9M-104M

//二，Assetbundle.CreateFromFile(string path) 
//非异步方法，unity官方文档上说最快的加载Assetbundle方式。
//从指定路径开始load到生成Assetbundle时间：0.012s 内存Used Total变化：52M-78M-103M

//注意：这里的url指定的Assetbundle要求是没有压缩过的，此方法也只能加载未压缩过的，就是打包ab时选择不压缩。 
//这个方法加载效率高，内存占用低，推荐使用，只是使用未经过unity压缩的Assetbundle，
//但下载或打包进StreammingAssets时我们可以自己压缩。虽然如此，但最后还是得解压到手机SD卡，占用一定的手机SD卡空间。

//三，Assetbundle.CreateFromMemory(byte[] bs) 
//传入unity自身压缩过的Assetbundle文件的字节， 
//从指定路径开始load到生成Assetbundle时间：2.599s，（太大，直接停在这一帧了） 
//内存Used Total变化：51M-108M-195M-217M

//这个方法先把Assetbundle文件的字节加载进内存，再从这些字节生成Assetbundle，最后导致加载时间漫漫，内存使用飙升。
//他的好处是可以对字节进行处理，就是加密，这样就可以对手机SD卡中的Assetbundle文件进行加密也能够使用了。
//以上的内存最高值只是某一时刻的，当然，引用完Assetbundle后要使用Unload进行释放。

//如果觉得调用Unload(false)没什么效果，试试： 
//这是U3D没有处理好的一个环节。在WWW加载资源完毕后，对资源进行instantiate后，对其资源进行unload,这时问题就发生 
//了，instantiate处理渲染需要一定的时间，虽然很短，但也是需要1，2帧的时间。此时进行unload会对资源渲染造成影响，以至于没有贴图 
//等问题发生,等待个0.5秒到1秒之后再进行Unload。这样就不会出现instantiate渲染中就运行unload的情况了。

//注意点：
//1.资源A加载完了后，要记得Unload(false)，资源A的依赖资源要在 资源A加载完成后，才能Unload(false),否则无法正常加载资
//源A。
//2.不Unload的话也可以，那就自己做一个字典记录所有加载过的AssetBundle，还有它们的引用计数器。那样就可以先判断是否存在，
//然后再确定是否要去加载。

//详细说一下细节概念： 
//AssetBundle运行时加载: 
//来自文件就用CreateFromFile(注意这种方法只能用于standalone程序）这是最快的加载方法 
//也可以来自Memory,用CreateFromMemory(byte[]),这个byte[]可以来自文件读取的缓冲，www的下载或者其他可能的方式。 
//其实WWW的assetBundle就是内部数据读取完后自动创建了一个assetBundle而已 
//Create完以后，等于把硬盘或者网络的一个文件读到内存一个区域，这时候只是个AssetBundle内存镜像数据块，还没有Assets的概念。 
//Assets加载: 
//用AssetBundle.Load(同Resources.Load) 这才会从AssetBundle的内存镜像里读取并创建一个Asset对象，创建Asset对象同时也会分配相应内存用于存放(反序列化) 
//异步读取用AssetBundle.LoadAsync 
//也可以一次读取多个用AssetBundle.LoadAll 
//AssetBundle的释放： 
//AssetBundle.Unload(flase)是释放AssetBundle文件的内存镜像，不包含Load创建的Asset内存对象。 
//AssetBundle.Unload(true)是释放那个AssetBundle文件内存镜像和并销毁所有用Load创建的Asset内存对象。

public class DependencsLoader
{
    public int mLength = 0;
    public int mDoneCnt = 0;

    public bool AllDone
    {
        get { return mDoneCnt >= mLength; }
    }

    public void Reset()
    {
        mLength = 0;
        mDoneCnt = 0;
    }
}

public class BundleLoaderManager : SingletonO<BundleLoaderManager>
{
    /// <summary>
    /// 资源从那个目录读取的
    /// </summary>
    private enum LoadFromType
    {
        LoadFromNone = 0,
        LoadFromDownLoad = 1,
        LoadFromLocalAB = 2,
    }

    private AssetBundleManifest AssetBundleManifest = null;
    private HashSet<string> AssetBundleHashSet = new HashSet<string>();

    private AssetBundleManifest LocalABBundleManifest = null;
    private HashSet<string> LocalABBundleHashSet = new HashSet<string>();

    public void Init()
    {
        InitManifest();
        InitLocalManifest();
        InitResourceIDPath();
    }

    /// <summary>
    /// 加载下载文件的依赖文件
    /// </summary>
    private void InitManifest()
    {
        string bundleName = ABPathHelper.platformFolder;
        int nHashCode = bundleName.GetHashCode();
        LoadedAssetBundle loaded = ABManager.Instance.GetLoadedBundle(nHashCode);
        if (loaded != null)
        {
            loaded.Count();
            return;
        }

        string strBundleUrl = ABPathHelper.AssetsURL + bundleName;
        if (File.Exists(strBundleUrl))
        {
            AssetBundle ab = AssetBundle.LoadFromFile(strBundleUrl);
            if (ab != null)
            {
                loaded = ABManager.Instance.AddLoadedBundle(nHashCode, ab);
                if (loaded != null)
                {
                    AssetBundleManifest = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                    loaded.UnLoadBundle(false);

                    AssetBundleHashSet.Clear();
                    string[] assetBundles = AssetBundleManifest.GetAllAssetBundles();
                    for (int i = 0; i < assetBundles.Length; i++)
                    {
                        AssetBundleHashSet.Add(assetBundles[i]);
                    }
                }
            }
        }
        else
        {
            Debug.Log("本地未找到bundle: " + strBundleUrl);
        }
    }

    /// <summary>
    /// 加载跟包文件的依赖文件
    /// </summary>
    private void InitLocalManifest()
    {
        string bundleName = ABPathHelper.platformFolder;
        //跟包的ab依赖文件需要一个和下载的ab依赖文件不重名的名字，特加此后缀区分
        string diffBundleName = bundleName + "diff";
        int nHashCode = diffBundleName.GetHashCode();
        LoadedAssetBundle loaded = ABManager.Instance.GetLoadedBundle(nHashCode);
        if (loaded != null)
        {
            loaded.Count();
            return;
        }
        string strBundleUrl = ABPathHelper.AssetsLocalABURL + bundleName;
        AssetBundle ab = AssetBundle.LoadFromFile(strBundleUrl);
        if (ab != null)
        {
            loaded = ABManager.Instance.AddLoadedBundle(nHashCode, ab);
            if (loaded != null)
            {
                LocalABBundleManifest = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                loaded.UnLoadBundle(false);

                LocalABBundleHashSet.Clear();
                string[] assetBundles = LocalABBundleManifest.GetAllAssetBundles();
                for (int i = 0; i < assetBundles.Length; i++)
                {
                    LocalABBundleHashSet.Add(assetBundles[i]);
                }
            }
        }
    }

    /// <summary>
    /// 初始化脚本资源的IDMap配置
    /// </summary>
    void InitResourceIDPath()
    {
        TextAsset t = null;
#if UNITY_EDITOR
        if (GameUtils.ScriptLoad)
        {
            t = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>("assets/abresources/autogenerate/pathidmap.xml");
        }
        else
#endif
        {
            t = Load<TextAsset>("autogenerate/pathidmap", "");
        }
        if (t != null)
        {
            ResourceID.ReMap(t != null ? t.text : null);
            t = null;
        }
    }

    /// <summary>
    /// 仅供Resource目录下配置使用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="bundleName"></param>
    /// <param name="assetName"></param>
    /// <returns></returns>
    public T Load<T>(string bundleName, string assetName = "") where T : Object
    {
        if (string.IsNullOrEmpty(bundleName))
            return default(T);
        string strFullBundleName = GetBundleName(bundleName, ELoadType.Config);
        string strFullAssetsName = string.IsNullOrEmpty(assetName) ? assetName : GetAssertName(bundleName, assetName, ABPathHelper.AssetTxtSuffix, ELoadType.Config);
        Object asset = Load(strFullBundleName, strFullAssetsName, ELoadType.Config);
        if (asset != null && asset is GameObject && typeof(T).IsSubclassOf(typeof(MonoBehaviour)))
            return ((GameObject)asset).GetComponent<T>();
        return asset != null ? (T)asset : default(T);
    }

    /// <summary>
    /// 仅供Resource目录下lua脚本使用
    /// </summary>
    /// <param name="bundleName"></param>
    /// <returns></returns>
    public TextAsset LoadScript(string bundleName)
    {
        if (string.IsNullOrEmpty(bundleName))
            return null;
        string[] array = bundleName.Split('/');
        if (array.Length >= 1)
        {
            string strFullBundleName = GetBundleName(array[0].ToLower(), ELoadType.Config);
            string strFullAssetName = GetAssertName(bundleName, string.Empty, ABPathHelper.AssetTxtSuffix, ELoadType.Config);
            Object asset = Load(strFullBundleName, strFullAssetName, ELoadType.LuaScript);
            if (asset != null && asset is GameObject && typeof(TextAsset).IsSubclassOf(typeof(MonoBehaviour)))
                return ((GameObject)asset).GetComponent<TextAsset>();
            else
                return asset != null ? (TextAsset)asset : default(TextAsset);
        }
        return null;
    }

    //加载序列化xml对象
    public object Loadobject(string bundleName, string assetName)
    {
        if (string.IsNullOrEmpty(bundleName))
            return null;
        string strFullBundleName = GetBundleName(bundleName, ELoadType.Config);
        string strFullAssetName = GetAssertName(bundleName, assetName, ABPathHelper.AssetXmlSuffix, ELoadType.Config);
        return Load(strFullBundleName, strFullAssetName, ELoadType.Config);
    }

    //加载图片对象,图片是单个打包      
    public void LoadIcon(string bundleName, GameObject target)
    {
        if (string.IsNullOrEmpty(bundleName))
            return;

        string strFullBundleName = GetBundleName(bundleName, ELoadType.Icon);
        //Debug.Log("<color=yellow>资源：bundleName: " + strFullBundleName + "</color>");

        Object obj = Load(strFullBundleName, string.Empty, ELoadType.Icon);
        if (obj == null)
            return;

        ResourceManager.Instance.AddIcon(obj.GetInstanceID(), strFullBundleName);

        if (obj != null && target != null)
        {
            UITexture tex = target.transform.GetComponent<UITexture>();
            if (tex != null)
            {
                if (tex.mainTexture != null)
                {
                    ResourceManager.Instance.UnLoadIcon(tex.mainTexture);
                    tex.mainTexture = null;
                }
                tex.mainTexture = obj as Texture2D;
                tex.MarkAsChanged();
            }
        }
    }

    public void UnLoadAll()
    {
        AssetBundleManifest = null;
        if (AssetBundleHashSet != null)
        {
            AssetBundleHashSet.Clear();
            AssetBundleHashSet = null;
        }

        LocalABBundleManifest = null;
        if (LocalABBundleHashSet != null)
        {
            LocalABBundleHashSet.Clear();
            LocalABBundleHashSet = null;
        }
        UnloadAsync();

        ABManager.Instance.UnLoadAll();
        _instance = null;
    }
    public void UnloadAssetBundle(int nBundleId, bool unloadAllLoadedObjects = false)
    {
        ABManager.Instance.UnLoadBundle(nBundleId, unloadAllLoadedObjects);
    }

    public Object Load(string bundleName, string assetName, ELoadType loadType)
    {
        if (string.IsNullOrEmpty(bundleName))
            return null;

        int nHashCode = bundleName.GetHashCode();

        LoadedAssetBundle loaded = ABManager.Instance.GetLoadedBundle(nHashCode);
        if (loaded != null)
        {
            loaded.Count();
        }
        else
        {
            loaded = LoadSyn(bundleName);
        }

        if (loaded != null)
        {
            loaded.mLoadType = loadType;
            Object asset = null;
            if (loaded.mAssetBundle != null)
            {
                AssetBundle ab = loaded.mAssetBundle;

                if (string.IsNullOrEmpty(assetName))
                {
                    asset = ab.mainAsset != null ? ab.mainAsset : ab.LoadAsset(ab.GetAllAssetNames()[0]);
                    if (asset != null)
                    {
                        loaded.SetMainAsset(asset);
                    }
                }
                else
                {
                    loaded.MapAssets();
                    asset = loaded.LoadAsset(assetName);
                }
                ABManager.Instance.AfterLoad(loaded);
                //Debug.Log("<color=yellow>assetName:" + assetName + "</color>");
            }
            else
            {
                asset = loaded.LoadAsset(assetName);
                //Debug.Log("<color=red>assetName:"+ assetName + "</color>");
            }
            return asset;
        }
        return null;
    }

    public SDictionary<int, UnityEngine.Object> LoadAll(string bundleName, ELoadType loadType)
    {
        if (string.IsNullOrEmpty(bundleName))
            return null;

        LoadedAssetBundle loaded = ABManager.Instance.GetLoadedBundle(bundleName.GetHashCode());
        if (loaded != null)
        {
            loaded.Count();
        }
        else
        {
            loaded = LoadSyn(bundleName);
        }

        if (loaded != null)
        {
            loaded.mLoadType = loadType;
            if (loaded.mAssetBundle != null)
            {
                loaded.MapAssets();
                ABManager.Instance.AfterLoad(loaded);
            }
            return loaded.GetAllAssets();
        }
        return null;
    }

    public LoadedAssetBundle LoadSyn(string strFullName, UpdateProgress updateFunc = null, bool bSend = false)
    {
        LoadedAssetBundle loaded = ABManager.Instance.GetLoadedBundle(strFullName.GetHashCode());
        if (loaded != null)
        {
            loaded.Count();
            return loaded;
        }

        AssetManageInfo info = AssetManageConfigManager.Instance.GetInfo(strFullName);

        //资源是从DownLoad包里还是StreamAsset的包里读取的
        LoadFromType loadFromType = LoadFromType.LoadFromNone;
        string[] dependencies = null;
        if (AssetBundleManifest != null && AssetBundleHashSet.Contains(strFullName))
        {//是否在下载的包里
            loadFromType = LoadFromType.LoadFromDownLoad;
            dependencies = AssetBundleManifest.GetAllDependencies(strFullName);
        }
        else if (LocalABBundleManifest != null && LocalABBundleHashSet.Contains(strFullName))
        {//下载的包里找不到就到跟包的目录里寻找一遍看是否有依赖
            loadFromType = LoadFromType.LoadFromLocalAB;
            dependencies = LocalABBundleManifest.GetAllDependencies(strFullName);
        }

        //只要不是LoadFromNone，说明这个资源是有身份证的，并非来路不明
        if (loadFromType != LoadFromType.LoadFromNone)
        {
            int nCnt = 1;
            if (dependencies != null)
            {
                nCnt += dependencies.Length;
                for (int i = 0; i < dependencies.Length; ++i)
                {
                    LoadBundleByFileStream(dependencies[i], getLoadFromType(dependencies[i]), AssetEncryptMode.None);
                    if (updateFunc != null)
                    {
                        updateFunc((float)(i + 1) / nCnt, bSend);
                    }
                }
            }

            //Debug.Log("<color=red> LoadSyn: " + strFullName + "</color>");

            loaded = LoadBundleByFileStream(strFullName, loadFromType, info.encryption);
            if (loaded != null)
            {
                if (dependencies != null && dependencies.Length > 0)
                    loaded.AddDpendences(dependencies);
                if (info.loadType != ELoadType.Music && info.loadType != ELoadType.Scene)
                {
                    loaded.MapAssets();
                    ABManager.Instance.AfterLoad(loaded);
                }
                if (updateFunc != null)
                {
                    updateFunc(1.0f, bSend);
                }
            }

            return loaded;
        }
        else
        {
            Debug.Log("<color=red>资源：" + strFullName + "来路不明，在bundle表里不存在。</color>");
            return null;
        }
    }

    private LoadFromType getLoadFromType(string bundleName)
    {
        if (AssetBundleManifest != null && AssetBundleHashSet.Contains(bundleName))
        {//是否在下载的包里
            return LoadFromType.LoadFromDownLoad;
        }
        else if (LocalABBundleManifest != null && LocalABBundleHashSet.Contains(bundleName))
        {//下载的包里找不到就到跟包的目录里寻找一遍看是否有依赖
            return LoadFromType.LoadFromLocalAB;
        }
        return LoadFromType.LoadFromNone;
    }

    private LoadedAssetBundle LoadBundleByFileStream(string bundleName, LoadFromType loadFromType, AssetEncryptMode encript = AssetEncryptMode.None)
    {
        if (string.IsNullOrEmpty(bundleName))
            return null;

        int nHashCode = bundleName.GetHashCode();
        LoadedAssetBundle loaded = ABManager.Instance.GetLoadedBundle(nHashCode);

        if (loaded != null)
        {
            loaded.Count();
            return loaded;
        }

        //在对应目录里进行AB创建
        m_strFullName.Length = 0;
        if (loadFromType == LoadFromType.LoadFromDownLoad)
            m_strFullName.Append(ABPathHelper.AssetsURL);
        else
            m_strFullName.Append(ABPathHelper.AssetsLocalABURL);
        m_strFullName.Append(bundleName);

        string path = m_strFullName.ToString();
        if (File.Exists(path))
        {
            AssetBundle ab = AssetBundle.LoadFromFile(path);
            if (ab != null)
            {
                loaded = ABManager.Instance.AddLoadedBundle(nHashCode, ab);
            }
        }
        else
        {
            if (GameUtils.OnlyShowInInner)
            {
                Debug.Log("<color=red>资源: " + m_strFullName + "不存在</color>");
            }
        }

        return loaded;
    }

    private BetterList<int> mLoadingReqs = null;
    private ObjectItemPool<DependencsLoader> mDependItemPool = null;

    private void InitAsync()
    {
        if (mLoadingReqs == null)
            mLoadingReqs = new BetterList<int>();
        if (mDependItemPool == null)
            mDependItemPool = new ObjectItemPool<DependencsLoader>();
    }

    private void UnloadAsync()
    {
        if (mLoadingReqs != null)
        {
            mLoadingReqs.Release();
            mLoadingReqs = null;
        }
        if (mDependItemPool != null)
        {
            mDependItemPool.UnLoadAll();
            mDependItemPool = null;
        }
    }

    public IEnumerator LoadAsync(string strFullName)
    {
        if (AssetBundleManifest == null || string.IsNullOrEmpty(strFullName))
            yield break;

        InitAsync();

        int nHashCode = strFullName.GetHashCode();
        LoadedAssetBundle loaded = ABManager.Instance.GetLoadedBundle(nHashCode);
        if (loaded != null)
        {
            loaded.Count();
            yield break;
        }
        if (mLoadingReqs.Contains(nHashCode))
            yield break;

        LoadFromType loadFromType = LoadFromType.LoadFromNone;
        string[] dependencies = null;
        if (AssetBundleManifest != null && AssetBundleHashSet.Contains(strFullName))
        {
            loadFromType = LoadFromType.LoadFromDownLoad;
            dependencies = AssetBundleManifest.GetAllDependencies(strFullName);
        }
        else if (LocalABBundleManifest != null && LocalABBundleHashSet.Contains(strFullName))
        {
            loadFromType = LoadFromType.LoadFromLocalAB;
            dependencies = LocalABBundleManifest.GetAllDependencies(strFullName);
        }

        int nLength = dependencies.Length;
        if (nLength > 0)
        {
            DependencsLoader dependCallBack = mDependItemPool.GetObject();
            if (dependCallBack != null)
            {
                dependCallBack.Reset();
                dependCallBack.mLength = nLength;
            }
            for (int i = 0; i < nLength; ++i)
            {
                ResourceManager.Instance.StartCoroutine(LoadBundleByWWW(dependencies[i], dependCallBack, loadFromType));
            }
            while (!dependCallBack.AllDone)
            {
                yield return null;
            }
            mDependItemPool.RemoveObject(dependCallBack);
        }

        yield return ResourceManager.Instance.StartCoroutine(LoadBundleByWWW(strFullName, null, loadFromType));

        loaded = ABManager.Instance.GetLoadedBundle(nHashCode);
        if (loaded != null)
        {
            if (nLength > 0)
                loaded.AddDpendences(dependencies);
            loaded.MapAssets();
            ABManager.Instance.AfterLoad(loaded);
        }
        yield return null;
    }

    private IEnumerator LoadBundleByWWW(string bundleName, DependencsLoader callback, LoadFromType loadFromType, ELoadType loadType = ELoadType.Max)
    {
        if (string.IsNullOrEmpty(bundleName))
            yield break;
        int nHashCode = bundleName.GetHashCode();
        LoadedAssetBundle loaded = ABManager.Instance.GetLoadedBundle(nHashCode);
        if (loaded != null)
        {
            loaded.Count();
            yield break;
        }

        if (!mLoadingReqs.Contains(nHashCode))
        {
            mLoadingReqs.Add(nHashCode);

            string strBundleUrl = "file://";
            if (loadFromType == LoadFromType.LoadFromDownLoad)
            {
                strBundleUrl += ABPathHelper.AssetsURL;
            }
            else
            {
                strBundleUrl = ABPathHelper.AssetsLocalABURL;
            }
            strBundleUrl += bundleName;

            WWW www = new WWW(strBundleUrl);
            if (www != null)
            {
                while (!www.isDone && string.IsNullOrEmpty(www.error))
                {
                    yield return null;
                }
                if (string.IsNullOrEmpty(www.error))
                {
                    Debug.Log("<color=yellow>AddLoadedBundle：" + bundleName + "</color>  " + Time.realtimeSinceStartup);
                    ABManager.Instance.AddLoadedBundle(nHashCode, www.assetBundle);
                    if (callback != null)
                    {
                        ++callback.mDoneCnt;
                    }
                }
                www.Dispose();
            }
            mLoadingReqs.Remove(nHashCode);
        }

        yield return null;
    }

    private static StringBuilder m_strFullName = new StringBuilder(128);

    /// <summary>
    /// 获取ABResource目录下的Bundle目录
    /// </summary>
    /// <param name="bundleName"></param>
    /// <param name="bGroup"></param>
    /// <returns></returns>
    public static string GetBundleName(string bundleName, ELoadType loadType)
    {
        bundleName = bundleName.ToLower();
        m_strFullName.Length = 0;
        m_strFullName.Capacity = 128;
        AssetManageInfo info = AssetManageConfigManager.Instance.GetInfo(loadType);
        if (info != null && info.loadType == loadType)
        {
            if (loadType == ELoadType.Icon)
                m_strFullName.Append(ABPathHelper.ABResourcePrefix);
            else
            {
                m_strFullName.Append(info.path);
                m_strFullName.Append(ABPathHelper.Separator);
            }
            m_strFullName.Append(bundleName);
            m_strFullName.Append(ABPathHelper.Unity3dSuffix);
        }
        return m_strFullName.ToString();
    }

    public static string GetAssertName(string bundleName, string assetName, string suffixName, ELoadType loadType)
    {
        bundleName = bundleName.ToLower();
        m_strFullName.Length = 0;
        m_strFullName.Capacity = 128;

        m_strFullName.Append(ABPathHelper.AssetNoResourcePrefix);
        AssetManageInfo info = AssetManageConfigManager.Instance.GetInfo(loadType);
        m_strFullName.Append(info.path);
        m_strFullName.Append(ABPathHelper.Separator);
        m_strFullName.Append(bundleName);

        if (!string.IsNullOrEmpty(assetName))
        {
            m_strFullName.Append(ABPathHelper.Separator);
            m_strFullName.Append(assetName.ToLower());
        }
        m_strFullName.Append(suffixName);
        return m_strFullName.ToString();
    }
}
