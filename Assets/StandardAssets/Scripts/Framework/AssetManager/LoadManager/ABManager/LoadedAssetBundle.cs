///author       xuhan
///Data         2016.07.28
///Description

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using Object = UnityEngine.Object;

public class LoadedAssetBundle : IDisposable
{  
    public AssetBundle mAssetBundle { get; private set; }
    private List<int> mDependences = null; 
    private int mRefCount = 0;
    public ELoadType mLoadType = ELoadType.Max;

    private SDictionary<int, UnityEngine.Object> mAssetMap = null;
    private UnityEngine.Object mMainAsset = null;

    public LoadedAssetBundle(AssetBundle bundle)
    {       
        mAssetBundle = bundle;      
        ++mRefCount;
    }

    public void AddDpendences(string [] dependencies)
    {
        if (dependencies == null)
            return;

        if (mDependences == null)
        {
            mDependences = new List<int>();
        }
        for (int i = 0; i < dependencies.Length; ++i)
        {
            mDependences.Add(dependencies[i].GetHashCode());         
        }        
    }

    public void MapAssets()
    {
        if (mAssetBundle != null && mAssetMap == null)
        {
            try
            {
                string[] names = mAssetBundle.GetAllAssetNames();
                mMainAsset = mAssetBundle.mainAsset;
                if (mMainAsset == null && names.Length > 0)
                    mMainAsset = mAssetBundle.LoadAsset(names[0]);
                if (names.Length > 0)
                {
                    mAssetMap = new SDictionary<int, UnityEngine.Object>();
                    for (int i = 0; i < names.Length; ++i)
                    {
                        //Debug.Log("<color=yellow> names: " + i + " " + names[i] + "</color>");
                        mAssetMap[names[i].GetHashCode()] = mAssetBundle.LoadAsset(names[i]);
                    }
                }
            } 
            catch
            {
                string[] names = mAssetBundle.GetAllAssetNames();
                if (names.Length > 0)
                    Debug.LogError("MapAssets:"+ names[0]);
            }
        }
    }

    public UnityEngine.Object LoadAsset(string assetName)
    {
        if (string.IsNullOrEmpty(assetName))
            return mMainAsset;
        if (mAssetMap != null)
            return mAssetMap[assetName.GetHashCode()];
        return null;
    }

    public SDictionary<int, UnityEngine.Object> GetAllAssets()
    {
        return mAssetMap;
    }

    public void SetMainAsset(UnityEngine.Object asset)
    {
        mMainAsset = asset;
    }

    public bool CanUnloadAfterLoad()
    {
        return mLoadType != ELoadType.Music; // && mLoadType != ELoadType.Sound;
    }

    public void UnLoadAssets()
    {       
        if (mAssetMap != null)
        {
            foreach(KeyValuePair<int, UnityEngine.Object> kvp in mAssetMap)
            {
                if (kvp.Value != null)
                {
                    UnLoadAsset(kvp.Value);
                }
            } 
            mAssetMap.Clear(); 
            mAssetMap = null;
        }      
       
        if (mMainAsset != null)
        {
            UnLoadAsset(mMainAsset);
        }
        mMainAsset = null;        
    }

    private void UnLoadAsset(UnityEngine.Object obj)
    {
        if((obj is GameObject) == false && (obj is Component) == false &&
            (obj is AssetBundle) == false)
        {
            Resources.UnloadAsset(obj);
        }
        else
        {
            ResourceManager.Instance.LateDestroyAsset(obj);
        }
    }
    
    public void UnLoadBundle(bool disposeUnLoadAllLoaded)
    {     
        if (mAssetBundle != null)
        {
            mAssetBundle.Unload(disposeUnLoadAllLoaded);
        }
        mAssetBundle = null;
    }

    public void Dispose()
    {       
        UnLoadDpendences();
        UnLoadBundle(true); 
        UnLoadAssets();       
        GC.SuppressFinalize(this);
    }

    public void UnLoadAll()
    {
        UnLoadBundle(true);
        UnLoadAssets(); 
    }

    private void UnLoadDpendences()
    {
        if (mDependences != null)
        {
            for (int i = 0; i < mDependences.Count; ++i)
            {
                ABManager.Instance.UnLoadBundle(mDependences[i], false);
            }
            mDependences.Clear();
            mDependences = null;
        }    
    }

    public void Count()
    {
        ++mRefCount;
    }

    public void UnCount()
    {
        --mRefCount;     
    }

    public int RefCount
    {
        get { return mRefCount; }
    }
}