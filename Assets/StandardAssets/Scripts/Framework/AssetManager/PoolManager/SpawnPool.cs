///author       xuhan
///Data         2016.08.04
///Description

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoolCacheItem
{
    public GameObject objItem = null;
    public float fLeftTime = 0.0f;

    public PoolCacheItem(GameObject obj, float fTime)
    {
        objItem = obj;
        fLeftTime = fTime;
    }
}

public class SpawnPool : MonoBehaviour
{        
    public ELoadType loadType = ELoadType.Max;
    public int limitCount = 0;
    public float delayDelTime = 0.0f;

    private BetterList<PrefabPool> prefabPools = null;

    private BetterList<PoolCacheItem> m_DelayDespawnCache = null;
    private BetterList<PoolCacheItem> m_Removes = null;

    public bool dontDestroyOnLoad           // 切换场景不释放
    {
        get
        {
            return _dontDestroyOnLoad;
        }

        set
        {
            _dontDestroyOnLoad = value;
            //如果处于根节点，设置为不销毁
            if (gameObject.transform.parent == null)
                Object.DontDestroyOnLoad(gameObject);
        }
    }
    private bool _dontDestroyOnLoad = false;    
    public bool logMessages = false;

    public void Init()
    {
        prefabPools = new BetterList<PrefabPool>();
        m_DelayDespawnCache = new BetterList<PoolCacheItem>();
        m_Removes = new BetterList<PoolCacheItem>();
    }
   
    private void OnDestroy()
    {
        //Debug.Log("<color=yellow> SpawnPool.OnDestroy: " + loadType + "</color>");
        if (prefabPools != null)
        {
            prefabPools.Release();
            prefabPools = null;
        }
        if (m_DelayDespawnCache != null)
        {
            m_DelayDespawnCache.Release();
            m_DelayDespawnCache = null;
        }
        if (m_Removes != null)
        {
            m_Removes.Release();
            m_Removes = null;
        }
    }

    public void Destroy()
    {
        if (logMessages)
            Debug.Log(string.Format("SpawnPool {0}: Destroying...", loadType.ToString()));

        ClearCache();
        if (prefabPools != null)
        {
            for (int i = 0; i < prefabPools.size; ++i)
                prefabPools[i].Destroy();

            prefabPools.Clear();          
        }        
    }

    private void ClearCache()
    {
        if (m_DelayDespawnCache != null)
        {
            m_DelayDespawnCache.Clear();
        }
        if (m_Removes != null)
        {
            m_Removes.Clear();
        }
    }

    public void RemoveFromCache(GameObject instance)
    {
        for (int i = 0; i < m_DelayDespawnCache.size; ++i)
        {
            if (m_DelayDespawnCache[i].objItem == instance)
            {
                m_DelayDespawnCache.RemoveAt(i);
                break;
            }
        }            
    }

    public void Update()
    {
        if (m_DelayDespawnCache == null || m_DelayDespawnCache.size <= 0)
            return;

        for (int i = 0; i < m_DelayDespawnCache.size; ++i)
        {
            m_DelayDespawnCache[i].fLeftTime -= Time.deltaTime;
            if (m_DelayDespawnCache[i].fLeftTime < 0.0f)
            {
                m_Removes.Add(m_DelayDespawnCache[i]);
            }
        }

        for (int i = 0; i < m_Removes.size; ++i)
        {
            //Debug.Log("<color=yellow>资源：deleteobject: " + m_Removes[i].objItem.name + "</color>");

            PostDespawn(m_Removes[i].objItem);
            m_DelayDespawnCache.Remove(m_Removes[i]);
        }
        m_Removes.Clear();
    }

    ///	Spawns an instance or creates a new instance if none are available.
    ///	Either way, an instance will be set to the passed position and 
    ///	rotation.    
    public GameObject Spawn(Object prefab, int nBundleId, int nAssetId, Transform parent)
    { 
        PrefabPool pool = AddPrefabPool(prefab, nBundleId, nAssetId); 
        GameObject inst = pool.SpawnInstance();
     
        if (inst == null) 
            return null;

        if (parent != null)  // User explicitly provided a parent
        {
            inst.transform.parent = parent;
        }
        else
        {      
            inst.transform.parent = transform;
        }

        return inst;
    }     
       
    ///	If the passed object is managed by the SpawnPool, it will be 
    ///	deactivated and made available to be spawned again.    
    public void Despawn(GameObject instance)
    {   
        // Find the item and despawn it
        bool despawned = false;
        for (int i = 0; i < prefabPools.size; ++i)
        {
            despawned = prefabPools[i].Despawn(instance);
            if (despawned)
                break;
        }

        // If still false, then the instance wasn't found anywhere in the pool
        //if (!despawned)
        //{
            //Debug.LogError(string.Format("SpawnPool {0}: {1} not found in SpawnPool", loadType.ToString(), instance.name));
            //return;
        //}
    }

    public void Despawn(GameObject instance, float delayDelTime)
    {
        if (instance != null && instance.activeSelf)
        {
            PreDespawn(instance, delayDelTime); 
        }
    }

    private void PreDespawn(GameObject instance, float delayDelTime)
    {
        if (instance == null)
            return;

        // Find the item and despawn it
        bool despawned = false;
        for (int i = 0; i < prefabPools.size; ++i)
        {
            despawned = prefabPools[i].PreDespawn(instance);
            if (despawned)
            {
                m_DelayDespawnCache.Add(new PoolCacheItem(instance, delayDelTime));                
                break;
            }
        }

        // If still false, then the instance wasn't found anywhere in the pool
        //if (!despawned)
        //{
        //    //Debug.LogError(string.Format("SpawnPool {0}: {1} not found in SpawnPool", loadType.ToString(), instance.name));
        //    return;
        //}
    }

    private void PostDespawn(GameObject instance)
    {
        if (instance == null)
            return;

        // Find the item and despawn it
        bool despawned = false;
        for (int i = 0; i < prefabPools.size; ++i)
        {
            despawned = prefabPools[i].PostDespawn(instance);
            if (despawned)
                break;
        }

        // If still false, then the instance wasn't found anywhere in the pool
        //if (!despawned)
        //{
        //    //Debug.LogError(string.Format("SpawnPool {0}: {1} not found in SpawnPool", loadType.ToString(), instance.name));
        //    return;
        //}
    }
  
    /// Waits X seconds before despawning. See the docs for DespawnAfterSeconds()
    /// the argument useParent is used because a null parent is valid in Unity. It will 
    /// make the scene root the parent    
    private IEnumerator DoDespawnAfterSeconds(GameObject instance, float seconds)
    {
        PreDespawn(instance, seconds);
        while (seconds > 0)
        {
            yield return null;

            //// If the instance was deactivated while waiting here, just quit
            //if (!instance.activeInHierarchy)
            //    yield break;

            seconds -= Time.deltaTime;
        }

        PostDespawn(instance);              
    }

    /// Creates a new PrefabPool in this Pool and instances the requested 
    /// number of instances (set by PrefabPool.preloadAmount). If preload 
    /// amount is 0, nothing will be spawned and the return list will be empty.  
    public PrefabPool AddPrefabPool(Object prefab, int nBundleId, int nAssetId)
    {
        PrefabPool prefabPool = GetPrefabPool(nBundleId, nAssetId);
        if (prefabPool == null)
        {
            prefabPool = new PrefabPool(prefab, nBundleId, nAssetId);
            prefabPool.spawnPool = this;           
            prefabPools.Add(prefabPool); // prefabPools.Add(prefabPool.prefab.name, prefabPool);           
        }
        else
        {
            prefabPool.prefab = prefab;
        }
        return prefabPool;
    }
  
    public PrefabPool GetPrefabPool(int nBundleId, int nAssetId)
    {
        for (int i = 0; i < prefabPools.size; ++i)
        {
            //if (prefabPools[i].prefab == null)
            //Debug.LogError(string.Format("SpawnPool {0}: PrefabPool.prefab is null, {1} is resource name", loadType.ToString(), bundleName));

            if (prefabPools[i].nBundleId == nBundleId && prefabPools[i].nAssetId == nAssetId)
                return prefabPools[i];
        }
        return null;
    }

    public void RemovePrefabPool(int nBundleId, int nAssetId)
    {
        for (int i = 0; i < prefabPools.size; ++i)
        {
            if (prefabPools[i].nBundleId == nBundleId && prefabPools[i].nAssetId == nAssetId)
            {
                prefabPools[i].Destroy();
                prefabPools.RemoveAt(i);
                break;
            }
        }       
    }
}