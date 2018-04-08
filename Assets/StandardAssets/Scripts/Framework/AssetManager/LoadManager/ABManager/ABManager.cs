///author       xuhan
///Data         2016.07.28
///Description

using UnityEngine;
using GameFramework;
using System.Collections.Generic;

public interface IABManagement
{
    bool IsLoaded(int nBundleId);

    LoadedAssetBundle AddLoadedBundle(int nBundleId, AssetBundle ab);

    void UnLoadBundle(int nBundleId, bool unloadAllLoadedObjects);

    LoadedAssetBundle GetLoadedBundle(int nBundleId);

    void UnLoadUnReference();

    void UnLoadAll();

    int LoadedSAB { get; }
}

public class ABManager : SingletonO<ABManager>
{
    IABManagement[] m_ABFactory = null;

    public ABManager()
    {
        m_ABFactory = new IABManagement[(int)AssetManageMode.AMMMax];
        //m_ABFactory[(int)AssetManageMode.NeverUnLoad] = new NeverUnloadABManager();
        m_ABFactory[(int)AssetManageMode.UnLoadImmediately] = new UnLoadABImmdiatelyManager();
        //m_ABFactory[(int)AssetManageMode.Custom] = new CustomABManager();
        //m_ABFactory[(int)AssetManageMode.LRU] = new ABLRUManager();
        //m_ABFactory[(int)AssetManageMode.AutoUnloadNoDepends] = new AutoUnloadABManager();
    }

    public void AfterLoad(LoadedAssetBundle sab)
    {
        //AssetManageInfo info = AssetManageConfigManager.Instance.GetInfo(sab.bundleName);
        if (sab != null && sab.CanUnloadAfterLoad()) //&& info.manageBundleMode == AssetManageMode.UnLoadImmediately)
        {
            sab.UnLoadBundle(false);
        }
    }

    public void UnLoadUnReference()
    {
        if (m_ABFactory != null)
        {
            for (int i = 0; i < (int)AssetManageMode.AMMMax; ++i)
                m_ABFactory[i].UnLoadUnReference();
        }
    }

    public void UnLoadAll()
    {
        if (m_ABFactory != null)
        {
            for (int i = 0; i < (int)AssetManageMode.AMMMax; ++i)
            {
                m_ABFactory[i].UnLoadAll();
                m_ABFactory[i] = null;
            }
            m_ABFactory = null;
        }
        _instance = null;
    }

    public bool IsLoaded(int nBundleId)
    {
        //AssetManageInfo info = AssetManageConfigManager.Instance.GetInfo(name);
        return m_ABFactory[(int)AssetManageMode.UnLoadImmediately].IsLoaded(nBundleId);
    }

    public LoadedAssetBundle AddLoadedBundle(int nBundleId, AssetBundle ab)
    {
        //AssetManageInfo info = AssetManageConfigManager.Instance.GetInfo(name);
        return m_ABFactory[(int)AssetManageMode.UnLoadImmediately].AddLoadedBundle(nBundleId, ab);
    }

    public void UnLoadBundle(int nBundleId, bool unloadAllLoadedObjects)
    {
        //AssetManageInfo info = AssetManageConfigManager.Instance.GetInfo(name);
        m_ABFactory[(int)AssetManageMode.UnLoadImmediately].UnLoadBundle(nBundleId, unloadAllLoadedObjects);
    }

    public LoadedAssetBundle GetLoadedBundle(int nBundleId)
    {
        return m_ABFactory[(int)AssetManageMode.UnLoadImmediately].GetLoadedBundle(nBundleId);
    } 
}
