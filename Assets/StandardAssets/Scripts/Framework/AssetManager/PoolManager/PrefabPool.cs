///author       xuhan
///Data         2016.08.04
///Description

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Fs.Config;

public class PrefabPool
{
    public SpawnPool spawnPool = null;
    public Object prefab = null;
    public int nBundleId = 0;
    public int nAssetId = 0;
  
    private BetterList<GameObject> spawned = new BetterList<GameObject>();
    private BetterList<GameObject> despawned = new BetterList<GameObject>();
        
    public PrefabPool(Object prefab, int nBundleId, int nAssetId)
    {
        this.prefab = prefab;
        this.nBundleId = nBundleId;
        this.nAssetId = nAssetId;
    }

    public bool logMessages
    {
        get
        {
            return this.spawnPool.logMessages;
        }
    }

    public void Destroy()
    {    
        // Go through both lists and destroy everything
        for (int i = 0; i < despawned.size; ++i)
        {
            if (despawned[i] != null)
            {             
                GameObject.Destroy(despawned[i]);
                despawned[i] = null;          
            }
        }

        for (int i = 0; i < spawned.size; ++i)
        {
            if (spawned[i] != null)
            {  
                GameObject.Destroy(spawned[i]);
                spawned[i] = null;            
            }
        }
        
        spawned.Release();
        despawned.Release();

        this.prefab = null;
        this.spawnPool = null;  

        BundleLoaderManager.Instance.UnloadAssetBundle(nBundleId, false);               
    }
  
    public int totalCount
    {
        get
        {
            // Add all the items in the pool to get the total count
            int count = 0;
            count += spawned.size;
            count += despawned.size;
            return count;
        }
    }
 
    public void RealDestroyObject(GameObject Instance)
    {
        if (Instance != null)
        {         
            this.despawned.Remove(Instance);    
            GameObject.Destroy(Instance);
            Instance = null;

            if (totalCount == 0)
            {
                this.spawnPool.RemovePrefabPool(nBundleId, nAssetId);
            }
        }
    }

    public bool PreDespawn(GameObject Instance)
    {
        bool despawned = false;

        if (this.despawned.Contains(Instance))
        {
            despawned = false;
        }
        else if (this.spawned.Contains(Instance))
        {
            this.spawned.Remove(Instance);
            this.despawned.Add(Instance);
            // Deactivate the instance and all children
            PoolManagerUtils.SetActive(Instance, false, this.spawnPool.loadType);       
            despawned = true;
        }

        return despawned;
    }

    public bool PostDespawn(GameObject Instance)
    {
    //    Debug.Log("<color=red> PostDespawn: " + bundleName + "\n" + "</color>");
        bool despawned = false;
        if (this.despawned.Contains(Instance))
        {
            this.RealDestroyObject(Instance);
            despawned = true;     
        }
        return despawned;
    }

    public bool Despawn(GameObject Instance)
    {
        bool despawned = false;
        if (this.spawned.Contains(Instance))
        {
            despawned = this.DespawnInstance(Instance);            
        }
        else if (this.despawned.Contains(Instance))
        {
            despawned = true;
            //Debug.LogError(string.Format("SpawnPool {0}: {1} has already been despawned. " + "You cannot despawn something more than once!", this.spawnPool.loadType.ToString(), Instance.name));            
        }
        return despawned;
    }

    /// Move an instance from despawned to spawned, set the position and 
    /// rotation, activate it and all children and return the GameObject   
    public bool DespawnInstance(GameObject Instance)
    {
        if (this.logMessages)
        {
            //Debug.Log(string.Format("SpawnPool {0} ({1}): Despawning '{2}'",this.spawnPool.loadType.ToString(),this.prefab.name,Instance.name));
        }
     
        // Switch to the despawned list
        spawned.Remove(Instance);
        despawned.Add(Instance);      

        RealDestroyObject(Instance);    
        
        return true;
    }
    
    /// Move an instance from despawned to spawned, set the position and 
    /// rotation, activate it and all children and return the GameObject.    
    public GameObject SpawnInstance()
    {
        GameObject inst = null;
    
        // If nothing is available, create a new instance
        if (despawned.size == 0)
        {
            // This will also handle limiting the number of NEW instances
            if (spawnPool.limitCount > 0 && totalCount >= spawnPool.limitCount)
            {
                inst = spawned[0];
            }
            else
            {
                inst = this.SpawnNew();
            }
        }
        else
        {
            // Switch the instance we are using to the spawned list
            // Use the first item in the list for ease
            inst = despawned[0];
            spawnPool.RemoveFromCache(inst);
            despawned.RemoveAt(0);
            spawned.Add(inst);

            // This came up for a user so this was added to throw a user-friendly error
            if (inst == null)
            {
                var msg = "Make sure you didn't delete a despawned instance directly.";
                throw new MissingReferenceException(msg);
            }

            if (this.logMessages)
            {
                //Debug.Log(string.Format("SpawnPool {0} ({1}): respawning '{2}'.", this.spawnPool.loadType.ToString(), this.prefab.name,inst.name));
            }

            // Get an instance and set position, rotation and then Reactivate the instance and all children
            inst.transform.position = Vector3.zero;
            inst.transform.rotation = Quaternion.identity;
            PoolManagerUtils.SetActive(inst, true, this.spawnPool.loadType);
        }

        return inst;
    }

    public GameObject SpawnNew()
    {
        GameObject inst = (GameObject)Object.Instantiate(this.prefab);
        this.nameInstance(inst);  // Adds the number to the end
      
        // Start tracking the new instance
        spawned.Add(inst);

        if (this.logMessages)
        {
            //Debug.Log(string.Format("SpawnPool {0} ({1}): Spawned new instance '{2}'.",this.spawnPool.loadType.ToString(),this.prefab.name,inst.name));
        }
        return inst;
    }  

    private void nameInstance(GameObject instance)
    {
        if (this.spawnPool.loadType == ELoadType.UI)
            instance.name = instance.name.Replace("(Clone)", "");
        else
        {
            instance.name += (this.totalCount + 1);
            //instance.name += (this.totalCount + 1).ToString("#000");
        }
    }
}