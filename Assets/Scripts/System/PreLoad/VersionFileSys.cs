using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class VersionFileSys : CGameSystem
{
    private static VersionFileSys _Instance = null;
    public static VersionFileSys Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = CGameRoot.GetGameSystem<VersionFileSys>();
            }
            return _Instance;
        }
    }

    public override bool SysEnter()
    { 
        return true;
    }

    public static readonly string DecompressInfo = "首次进入游戏需要解压本地资源，不消耗流量，先热身一下准备上场吧!";
    /// <summary>
    /// 解压缩的分段次数
    /// </summary>
    public static int DecompressCount = 3;
    /// <summary>
    /// 跟包AB文件的MD5s信息
    /// </summary>
    private List<string> mWithPakMD5s = null;
    /// <summary>
    /// 最大同时解压数量
    /// </summary>
    private const int MAX_DECOMPRESS_HANDLE = 4;
    /// <summary>
    /// 进行跟包资源的解压缩
    /// </summary>
    /// <returns></returns>
    public IEnumerator doDecompressAB()
    {
        //读取并存储跟包的MD5s
        yield return StartCoroutine(loadWithPakMD5s());

        //读取并存储跟包的Manifest
        string strManifestPath = ABPathHelper.AssetsWWWStreamURL + ABPathHelper.platformFolder;
        WWW wwwManifest = new WWW(strManifestPath);
        if (wwwManifest != null)
        {
            while (!wwwManifest.isDone && string.IsNullOrEmpty(wwwManifest.error))
            {
                yield return null;
            }

            strManifestPath = ABPathHelper.AssetsLocalABURL + ABPathHelper.platformFolder;
            File.WriteAllBytes(strManifestPath, wwwManifest.bytes);

            wwwManifest.Dispose();
        }

        int curProCompressStep = -1;
        int curWithPakIndex = 0;
        int decompressCount = DecompressCount;
        if (decompressCount <= 0) decompressCount = 3;
        int clipWithPakCount = mWithPakMD5s.Count / decompressCount;
        string showDecompressInfo = DecompressInfo;
        for (int i = 0; i < mWithPakMD5s.Count;)
        {
            if (DecompressWhileRef.GetCurRefs() < MAX_DECOMPRESS_HANDLE)
            {
                StartCoroutine(loadForLocalABPath(mWithPakMD5s[i]));
                curWithPakIndex++;
                int proCompressStep = curWithPakIndex / clipWithPakCount;
                int proCompressIndex = curWithPakIndex % clipWithPakCount;

                //if (Updating != null)
                //{
                //    if (curProCompressStep != proCompressStep)
                //    {
                //        curProCompressStep = proCompressStep;
                //        showDecompressInfo = LoadingUIManager.DecompressInfo + " [" + (proCompressStep + 1) + "/" + decompressCount + "] ";
                //        LoadingUIManager.Instance.ResetUI();
                //    }
                //    Updating(showDecompressInfo, proCompressIndex / (float)clipWithPakCount);
                //}

                i++;
            }
            else
            {
                yield return null;
            }
        }

        while (DecompressWhileRef.GetCurRefs() > 0)
            yield return null;

        //全部解压缩完成啦啦啦
        string strDecompressSuccess = ABPathHelper.AssetsLocalABURL + ABPathHelper.DecompressSuccess;
        using (FileStream fs = new FileStream(strDecompressSuccess, FileMode.OpenOrCreate))
        {
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.Write(AppVerConfig.curAppVer);
            }
        }
    }
    
    /// <summary>
    /// 读取跟包的VersionMD5.txt
    /// </summary>
    /// <returns></returns>
    private IEnumerator loadWithPakMD5s()
    {        
        mWithPakMD5s = new List<string>();
        string strWithPakMD5sPath = ABPathHelper.AssetsWWWStreamURL + ABPathHelper.localVerMD5File;

        WWW wwwWithPakMD5s = new WWW(strWithPakMD5sPath);
        if (wwwWithPakMD5s != null)
        {
            while (!wwwWithPakMD5s.isDone && string.IsNullOrEmpty(wwwWithPakMD5s.error))
            {
                yield return null;
            }

            string splitSign = "\r\n";
            string[] listMD5s = wwwWithPakMD5s.text.Split(splitSign.ToCharArray());
            for (int i = 0; i < listMD5s.Length; i++)
            {
                string line = listMD5s[i];
                if (!string.IsNullOrEmpty(line))
                {
                    string[] tmp = line.Split('*');
                    if (tmp.Length > 2 && !tmp[0].EndsWith(ABPathHelper.ManifestSuffix) && !tmp[0].EndsWith(ABPathHelper.MP4Suffix))
                    {
                        mWithPakMD5s.Add(tmp[0].Replace("\\", "/"));
                    }
                }
            }

            //把streamingAssets里的MD5文件拷贝到localAB目录
            strWithPakMD5sPath = ABPathHelper.AssetsLocalABURL + ABPathHelper.localVerMD5File;
            File.WriteAllBytes(strWithPakMD5sPath, wwwWithPakMD5s.bytes);

            wwwWithPakMD5s.Dispose();
        }
    }

    private IEnumerator loadForLocalABPath(string keyPath)
    {
        using (DecompressWhileRef counter = new DecompressWhileRef())
        {
            string abFilePath = ABPathHelper.AssetsWWWStreamURL + keyPath;
            if (abFilePath.IndexOf(ABPathHelper.MP4Suffix) < 0)
            {
                WWW wwwWithPakFile = new WWW(abFilePath);
                if (wwwWithPakFile != null)
                {
                    while (!wwwWithPakFile.isDone)
                    {
                        yield return null;
                    }
                    abFilePath = ABPathHelper.AssetsLocalABURL + keyPath;
                    ABHelper.CacheFile(abFilePath, wwwWithPakFile.bytes);
                    wwwWithPakFile.Dispose();
                }
            }
        }
    }

    public override void SysLeave()
    {
        base.SysLeave();
    }

    public override void SysFinalize()
    {
        base.SysFinalize();
    }
}

/// <summary>
/// 只是用于此处的ref计数
/// </summary>
public class DecompressWhileRef : IDisposable
{
    static int count = 0;
    public DecompressWhileRef() { ++count; }

    public void Dispose() { --count; }

    public static int GetCurRefs() { return count; }
}