///author       xuhan
///Data         2016.07.28
///Description

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DefaultABManager : IABManagement
{
    protected Dictionary<int, LoadedAssetBundle> m_Cache = null;
    protected BetterList<int> m_Removes = null;

    public DefaultABManager()
    {
        m_Cache = new Dictionary<int, LoadedAssetBundle>();
        m_Removes = new BetterList<int>();
    }

    public virtual bool IsLoaded(int nBundleId)
    {
        return m_Cache.ContainsKey(nBundleId);
    }

    public virtual LoadedAssetBundle AddLoadedBundle(int nBundleId, AssetBundle ab)
    {
        LoadedAssetBundle bundle = null;
        if (!m_Cache.TryGetValue(nBundleId, out bundle))       
        {
            bundle = CreateSAB(ab);
            m_Cache.Add(nBundleId, bundle); 
        }
        return bundle;
    }

    public virtual void UnLoadBundle(int nBundleId, bool unloadAllLoadedObjects)
    {
        LoadedAssetBundle bundle = null;
        if (m_Cache.TryGetValue(nBundleId, out bundle))      
        {
            if (unloadAllLoadedObjects)
            {
                //Debug.Log("<color=yellow> UnLoadBundle: " + name + " unloadAllLoadedObjects: " + unloadAllLoadedObjects + "\n" + "</color>");
                bundle.Dispose();
                m_Cache.Remove(nBundleId);
            }
            else
            {
                bundle.UnCount();
                //if (bundle.RefCount <= 0)
                //{
                //    bundle.Dispose();
                //    m_Cache.Remove(name);
                //}
            }  
        } 
    }

    public virtual LoadedAssetBundle GetLoadedBundle(int nBundleId)
    {
        LoadedAssetBundle bundle = null;
        m_Cache.TryGetValue(nBundleId, out bundle);
        return bundle;
    }
   
    public virtual void UnLoadUnReference()
    {
        if (m_Cache.Count <= 0)
            return;
        
        foreach (KeyValuePair<int, LoadedAssetBundle> kvp in m_Cache)
        {
            LoadedAssetBundle sab = kvp.Value;
            if (sab != null)
            {
                if (sab.RefCount <= 0)
                {
                    //Debug.Log("<color=blue> UnLoadUnReference: " + kvp.Key + "\n" + "</color>");
                    //if (sab.bundleName.Contains("data/sound/sound/matchvioce/cv_a"))
                    //{
                    //    int jjj = 0;
                    //}
                    sab.Dispose();
                    m_Removes.Add(kvp.Key);
                }
            }
        }

        for (int i = 0; i < m_Removes.size; ++i)
        {
            m_Cache.Remove(m_Removes[i]);
        }
        m_Removes.Clear();
    }

    public virtual void UnLoadAll()
    {
        if (m_Cache != null)
        {
            foreach (KeyValuePair<int, LoadedAssetBundle> kvp in m_Cache)
            {
                LoadedAssetBundle sab = kvp.Value;
                if (sab != null)
                {
                    sab.UnLoadAll();
                }
            }
            m_Cache.Clear();
            m_Cache = null;
        }
        if (m_Removes != null)
        {
            m_Removes.Release();
            m_Removes = null;
        }
    }

    public virtual int LoadedSAB
    {
        get
        {
            return m_Cache.Count;
        }
    }

    virtual protected LoadedAssetBundle CreateSAB(AssetBundle bundle)
    {
        return new LoadedAssetBundle(bundle);
    }
}

public class CustomABManager : DefaultABManager
{
    public override void UnLoadBundle(int nBundleId, bool unloadAllLoadedObjects)
    {
        LoadedAssetBundle cacheSource = GetLoadedBundle(nBundleId);
        if (cacheSource != null)
        {      
            if (cacheSource.RefCount <= 1)
            {
                //Debug.Log("CustomABManager<color=red>无引用自动释放</color>");
                base.UnLoadBundle(nBundleId, unloadAllLoadedObjects);
            }            
        }
    }   
}

public class NeverUnloadABManager : DefaultABManager
{
    public override void UnLoadBundle(int nBundleId, bool unloadAllLoadedObjects)
    {
        ////DebugLog.Logqs("NeverUnLoadABManager卸载: " + name);
    }

    public override void UnLoadUnReference()
    {
        
    }
}

public class UnLoadABImmdiatelyManager : DefaultABManager
{
   
}
