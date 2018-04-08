///author       xuhan
///Data         2016.08.04
///Description

using UnityEngine;
using GameFramework;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Fs.Config;

public static class PoolManagerUtils
{
    public static Vector3 InvalidPosition = new Vector3(-200000.0f, -200000.0f, -200000.0f);
    public static void SetActive(GameObject obj, bool state, ELoadType loadType = ELoadType.Max)
    {
        //obj.SetActive(state);
        switch (loadType)
        {
            case ELoadType.Sound:
                obj.SetActive(state);
                break;
            case ELoadType.Effect:
                obj.SetActive(state);
                break;
            default:
                if (state)
                    obj.transform.position = Vector3.zero;
                else
                    obj.transform.position = InvalidPosition;
                break;
        }
    }
}

public class PoolManager : SingleTonGO<PoolManager>
{
    private Dictionary<ELoadType, SpawnPool> m_Pools = null;  
  
    public void Init()
    {
        if (m_Pools == null)       
            m_Pools = new Dictionary<ELoadType, SpawnPool>();        
    }

    public GameObject Spawn(string bundleName, string assetName, ELoadType loadType, Transform parent = null)
    {
        if (string.IsNullOrEmpty(bundleName))
            return null;

        //Debug.Log("<color=red> Spawn: " + bundleName + "/" + fileName + "\n" + "</color>");
        
        SpawnPool pool = null;
        if (!m_Pools.TryGetValue(loadType, out pool))
        {
            AssetManageInfo info = AssetManageConfigManager.Instance.GetInfo(loadType);
            pool = CreateSpawnPool(loadType, info.delayDelTime);
            pool.limitCount = info.limitCount;
            if (loadType == ELoadType.Effect)
            {
                pool.transform.parent = CGameRoot.UIRoot.transform;
            }
        }

        string strFullBundleName = BundleLoaderManager.GetBundleName(bundleName, loadType);
        string strFullAssetName = BundleLoaderManager.GetAssertName(bundleName, assetName, ABPathHelper.PrefabSuffix, loadType);

        //Debug.Log("<color=yellow>资源：bundleName: " + strFullBundleName + " assetName: " + strFullAssetName + "</color>");

        int nBundleId = strFullBundleName.GetHashCode();
        int nAssetId = strFullAssetName.GetHashCode();

        PrefabPool prefabPool = pool.GetPrefabPool(nBundleId, nAssetId);       
        GameObject inst = null;
        if (prefabPool != null && prefabPool.prefab != null)
        {
            inst = pool.Spawn(prefabPool.prefab, nBundleId, nAssetId, parent);      
        }
        else
        {
            Object source = BundleLoaderManager.Instance.Load(strFullBundleName, strFullAssetName, loadType);
            if (source != null)
            {
                inst = pool.Spawn(source, nBundleId, nAssetId, parent);
            }
            else
            {
                Debug.Log("<color=red> object is null: " + strFullAssetName + "\n" + "</color>");  
            }           
        }       
        return inst;
    }
  
    public void DeSpawn(ELoadType loadType, GameObject Instance, bool bImediate = false)
    {  
        SpawnPool pool = null;
        if (m_Pools == null)
        {
            Debug.Log("Despaw() Instance.name:" + Instance.name + " m_Pools had destroyed");
            return;
        }            
        if (m_Pools.TryGetValue(loadType, out pool))
        {
            if (pool != null && pool.isActiveAndEnabled && Instance != null)
            {
                Instance.transform.parent = pool.transform;
                if (!bImediate && pool.delayDelTime > 0.0f)
                    pool.Despawn(Instance, pool.delayDelTime);
                else
                    pool.Despawn(Instance);
            }
        }     
    }

    public SpawnPool CreateSpawnPool(ELoadType loadType, float fDelayDelTime)
    {
        if (!m_Pools.ContainsKey(loadType))
        {
            GameObject owner = new GameObject(loadType.ToString());
            owner.transform.parent = this.transform;
            m_Pools[loadType] = owner.AddComponent<SpawnPool>();
            m_Pools[loadType].loadType = loadType;
            m_Pools[loadType].delayDelTime = fDelayDelTime;
            m_Pools[loadType].dontDestroyOnLoad = true;
            m_Pools[loadType].Init();
        }
        
        return m_Pools[loadType];
    }
    
    public bool DestroySpawnPool(ELoadType type)
    {      
        SpawnPool spawnPool = null;
        if (m_Pools.TryGetValue(type, out spawnPool))
        {
            // The rest of the logic will be handled by OnDestroy() in SpawnPool
            if (spawnPool != null)
            {
                spawnPool.Destroy();
            }
            return true;
        }

        //Debug.Log(string.Format("PoolManager: Unable to destroy '{0}'. Not in PoolManager", type.ToString()));
        return false;
    }   
  
    public void UnLoadAll()
    {
        //Debug.Log("<color=yellow> PoolManager.UnLoadAll \n" + "</color>");
        if (m_Pools != null)
        {
            foreach (KeyValuePair<ELoadType, SpawnPool> pair in m_Pools)
            {
                if (pair.Value != null)
                {
                    pair.Value.Destroy();
                    GameObject.Destroy(pair.Value.gameObject);
                }
            }

            m_Pools.Clear();
            m_Pools = null;
        }      
    }
}
