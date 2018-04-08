using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PreLoadSys : CGameSystem
{
    public override bool SysEnter()
    {
        return true;
    }

    public override IEnumerator SysEnterCo()
    {
        LoadingUISys.Instance.LoadUI();

        //一步步的初始化进入游戏
        yield return StartCoroutine(EnterGameStepByStep());

        base.SysEnterCo();
    }

    private IEnumerator EnterGameStepByStep()
    {
        //把跟包的文件进行解压缩，使用的是这份解压缩了的文件
        if(isNeedDecompressAB)
        {
            //如果本地LocalAB目录存在,先删除本地LocalAB目录
            string localABPath = Path.GetDirectoryName(ABPathHelper.AssetsLocalABURL);
            if (Directory.Exists(localABPath))
                Directory.Delete(localABPath, true);
            Directory.CreateDirectory(localABPath);
            //如果本地LocalCache目录存在，也要删除本地LocalCache目录
            string localCachePath = Path.GetDirectoryName(ABPathHelper.AssetsURL);
            if (Directory.Exists(localCachePath))
                Directory.Delete(localCachePath, true);

            //进行跟包AB的解压操作
            yield return StartCoroutine(VersionFileSys.Instance.doDecompressAB());
        }

        //进行AB的初始化
        yield return StartCoroutine(PreLoadData());
    }

    public IEnumerator PreLoadData()
    {
        //对资源的minifest文件进行初始化
        ResourceManager.Instance.InitManifest();

        //预加载文件的读取与解析后，进行AB文件的预加载
        yield return StartCoroutine(doABPreLoad());

        //预加载lua文件
        yield return StartCoroutine(SLuaSys.Instance.preLoadLua());

        //预加载状态PreLoad切换至Login状态
        CGameRoot.SwitchToState(EStateType.Login);
    }

    /// <summary>
    /// 判断是否需要对跟包的AB进行解压缩,根据本地是否已有DecompressSuccess.txt文件
    /// </summary>
    private bool isNeedDecompressAB
    {
        get
        {
            string decompressSuccessFile = ABPathHelper.AssetsLocalABURL + ABPathHelper.DecompressSuccess;
            if (File.Exists(decompressSuccessFile))
            {
                //取出本地版本的版本号文件信息
                using (FileStream fs = new FileStream(decompressSuccessFile, FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        string line = sr.ReadLine();
                        if (line != null)
                        {
                            int successAppVer = int.Parse(line);
                            return successAppVer != AppVerConfig.curAppVer;
                        }
                    }
                }
                return false;
            }
            else
                return true;
        }
    }
    
    //需要预加载的文件列表
    private List<string> mPreloadList = new List<string>();
    //执行AB的预加载
    private IEnumerator doABPreLoad()
    {
        loadCfgFile();

        //判断当前的加载列表是否加载完成
        for (int i = 0; i < mPreloadList.Count; ++i)
        {
            BundleLoaderManager.Instance.LoadSyn(mPreloadList[i]);

            //if (i % 3 == 0)
            {
                yield return null;
            }
        }

        yield return null;
    }

    //读取预加载的配置文件
    private bool loadCfgFile()
    {
        mPreloadList.Clear();
        TextAsset config = ResourceManager.Instance.LoadTextAsset(ABPathHelper.GameConfigPath, ABPathHelper.GamePreLoadConfigFile);
        if (config != null)
        {
            using (MemoryStream configStream = new MemoryStream(config.bytes))
            {
                using (StreamReader str = new StreamReader(configStream))
                {
                    string line = str.ReadLine();
                    while (line != null)
                    {
                        if (!string.IsNullOrEmpty(line))
                        {
                            mPreloadList.Add(line);
                        }
                        line = str.ReadLine();
                    }
                }
            }
            Resources.UnloadAsset(config);
        }
        return true;
    }

    public override void SysLeave()
    {
        base.SysLeave();
    }

    public override void SysFinalize()
    {
        if (mPreloadList != null)
        {
            for (int i = 0; i < mPreloadList.Count; ++i)
            {
                BundleLoaderManager.Instance.UnloadAssetBundle(mPreloadList[i].GetHashCode(), false);
            }
            mPreloadList.Clear();
            mPreloadList = null;
        }

        base.SysFinalize();
    }
}
