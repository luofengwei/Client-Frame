///author       xuhan
///Data         2016.07.23
///Description

using UnityEngine;
using GameFramework;
using System.Collections.Generic;
using System.Collections;
using System.Text;

[CustomLuaClassAttribute]
public class ResourceManager : SingleTonGO<ResourceManager>
{  
    private AssetDestroyer m_AssetDestroyer = null;   
    private Dictionary<GameObject, int> m_AllGameObjects = null;  

    public void Init()
    {
        if (m_AllGameObjects == null)
            m_AllGameObjects = new Dictionary<GameObject, int>();        
    }

    [DoNotToLua]
    public void InitManifest()
    {
        m_AssetDestroyer = gameObject.AddComponent<AssetDestroyer>();
        BundleLoaderManager.Instance.Init();
    }

    [DoNotToLua]
    public TextAsset LoadTextAsset(string bundleName, string assetName = "")
    {
        return BundleLoaderManager.Instance.Load<TextAsset>(bundleName, assetName);        
    }

    [DoNotToLua]
    public TextAsset LoadTextAssetForAsyn(string bundleName, string assetName, LoadDataType load = LoadDataType.AssetBundleType)
    {
        TextAsset textFile = null;
        if (load == LoadDataType.ResourcesType)
        {
            string path = bundleName;
            if (path.Contains(ABPathHelper.ResourcePrefix))
                path = path.Remove(0, 10);
            path += assetName;
            textFile = Resources.Load(path) as TextAsset;
        }
#if UNITY_EDITOR
        else if (load == LoadDataType.LoadAssetAtPath)
        {
            if (!bundleName.StartsWith("Assets/ABResources/"))
                bundleName = "Assets/ABResources/" + bundleName + "/" + assetName + ABPathHelper.AssetXmlSuffix;
            else
                bundleName = bundleName + "/" + assetName + ABPathHelper.AssetXmlSuffix;

            textFile = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(bundleName);
        }
#endif
        else
        {
            textFile = (TextAsset)ResourceManager.Instance.Loadobject(bundleName, assetName);
        }

        return textFile;
    }
    
    [DoNotToLua]
    public TextAsset LoadScript(string bundleName)
    {
        return BundleLoaderManager.Instance.LoadScript(bundleName);
    }

    [DoNotToLua]
    public object Loadobject(string bundleName, string assetName)
    {
        return BundleLoaderManager.Instance.Loadobject(bundleName, assetName);
    }

    private Dictionary<int, int> m_AllIcons = new Dictionary<int, int>();

    [DoNotToLua]
    public void AddIcon(int nID, string strFileName)
    {
        m_AllIcons[nID] = strFileName.GetHashCode();
    }

    public void LoadIcon(string bundleName, GameObject target) 
    {
        BundleLoaderManager.Instance.LoadIcon(bundleName, target);     
    }

    private float m_fTime = 0.0f;
    [DoNotToLua]
    public void Update()
    {
        m_fTime += Time.deltaTime;
        if (m_fTime > 30.0f)
        {
            GC();
            m_fTime = 0.0f;
        }
    }

    public void GC()
    {
        //Debug.Log("<color=red> GC: m_AllGameObjects.size = " + m_AllGameObjects.Count + "</color>");
        //foreach (var item in m_AllGameObjects)
        //{
        //    Debug.Log("<color=red> GC: Item: " + item.Value + "</color>");
        //}
        ABManager.Instance.UnLoadUnReference();
        ABManager.Instance.UnLoadUnReference(); //收集倚赖
        if (m_AssetDestroyer != null)
        {
            m_AssetDestroyer.DestroyRightNow();
        }
    }

    public void UnLoadIcon(Object obj)
    {
        if (obj == null)
            return;

        if (m_AllIcons != null && m_AllIcons.Count > 0)
        {
            int nBundleId = 0;
            if (m_AllIcons.TryGetValue(obj.GetInstanceID(), out nBundleId))
            {
                BundleLoaderManager.Instance.UnloadAssetBundle(nBundleId, false);
            }
        }
    }

    public void UnLoadIcon(string bundleName)
    {
        string strFullBundleName = BundleLoaderManager.GetBundleName(bundleName, ELoadType.Icon);
        //Debug.Log("<color=yellow>资源：bundleName: " + strFullBundleName + "</color>");
        int nBundleId = strFullBundleName.GetHashCode();
        BundleLoaderManager.Instance.UnloadAssetBundle(nBundleId, false);
    }

    [DoNotToLua]
    private void UnLoadAllIcon()
    {
        if (m_AllIcons != null)
        {
            foreach (KeyValuePair<int, int> kvp in m_AllIcons)
            {
                BundleLoaderManager.Instance.UnloadAssetBundle(kvp.Value, true);
            }
            m_AllIcons.Clear();
            m_AllIcons = null;  
        }
    }

    public void UnLoad(string bundleName, ELoadType loadType, bool unloadAllLoadedObjects = false) // ELoadType
    {
        string strFullBundleName = BundleLoaderManager.GetBundleName(bundleName, loadType);
        int nBundleId = strFullBundleName.GetHashCode();
        //Debug.Log("<color=yellow>资源：bundleName: " + strFullBundleName + "</color>");
        BundleLoaderManager.Instance.UnloadAssetBundle(nBundleId, unloadAllLoadedObjects);
    }

    public void UnLoad(GameObject gameObj, bool unloadAllLoadedObjects = false)
    {
        if (gameObj != null && m_AllGameObjects != null)
        {           
            //PoolManager.Instance.DeSpawn(gameObj);
            int nBundleId = 0;
            if (m_AllGameObjects.TryGetValue(gameObj, out nBundleId))
            {              
                m_AllGameObjects.Remove(gameObj);
                GameObject.DestroyImmediate(gameObj);
                gameObj = null;
                BundleLoaderManager.Instance.UnloadAssetBundle(nBundleId, unloadAllLoadedObjects);               
            }           
        }
    }   

    [DoNotToLua]
    public void UnLoadAll()
    {
        DestoryAllGameObjects();
        UnLoadAllIcon();
        BundleLoaderManager.Instance.UnLoadAll();
        if (m_AssetDestroyer != null)
        {
            m_AssetDestroyer.DestroyRightNow();
            Component.DestroyImmediate(m_AssetDestroyer);
            m_AssetDestroyer = null;
        }  
    }

    [DoNotToLua]
    public void DestoryAllGameObjects()
    {
        if (m_AllGameObjects != null)
        {
            foreach (var item in m_AllGameObjects)
            {
                if (item.Key != null)
                { 
                    GameObject.DestroyImmediate(item.Key);
                    BundleLoaderManager.Instance.UnloadAssetBundle(item.Value, true);                   
                }
            }
            m_AllGameObjects.Clear();
            m_AllGameObjects = null;
        }
    }

    [DoNotToLua]
    public void LateDestroyAsset(Object asset)
    {
        if (m_AssetDestroyer != null)
            m_AssetDestroyer.AddToDestroy(asset);
    } 
 
    [DoNotToLua]
    public SDictionary<int, Object> GetBundleObjects(string bundleName, ELoadType loadType)
    {
        return BundleLoaderManager.Instance.LoadAll(bundleName, loadType);
    }

    //获取Bundle资源，没有实例化
    [DoNotToLua]
    public Object RGetNoInstance(string bundleName, string assetName = "", string suffix = ".prefab", ELoadType loadType = ELoadType.Max)
    {
        string strFullBundleName = BundleLoaderManager.GetBundleName(bundleName, loadType);
        string strFullAssetName = BundleLoaderManager.GetAssertName(bundleName, assetName, suffix, loadType);
        Object source = BundleLoaderManager.Instance.Load(strFullBundleName, strFullAssetName, loadType);        
        return source;       
    }

    //获取实例化资源的地方(同步方式加载预设)
    public GameObject RGet(string bundleName, string assetName, ELoadType loadType, GameObject parent = null)
    {     
        if (string.IsNullOrEmpty(bundleName))
            return null;

        string strFullBundleName = BundleLoaderManager.GetBundleName(bundleName, loadType);
        string strFullAssetName = BundleLoaderManager.GetAssertName(bundleName, assetName, ABPathHelper.PrefabSuffix, loadType);
        //Debug.Log("<color=yellow>资源：bundleName: " + strFullBundleName + " assetName: " + strFullAssetName + "</color>");

        Object source = null;
#if UNITY_EDITOR
        if (GameUtils.ScriptLoad && loadType == ELoadType.UI)
        {
            source = UnityEditor.AssetDatabase.LoadAssetAtPath(strFullAssetName, typeof(UnityEngine.Object));
        }
        else
#endif
        {
            source = BundleLoaderManager.Instance.Load(strFullBundleName, strFullAssetName, loadType);
        }

        if (source is GameObject)
        {
            GameObject go = GameObject.Instantiate(source) as GameObject;
            MaterialUtils.ResetGameObjectMaterials(go);

            if (loadType == ELoadType.UI)
            {
                AutoDestroy autoDestroyCom = go.GetComponent<AutoDestroy>();
                if (autoDestroyCom == null)
                {
                    autoDestroyCom = go.AddComponent<AutoDestroy>();
                }
            }
            go.name = go.name.Replace("(Clone)", "");

            int nBundleId = strFullBundleName.GetHashCode();
            m_AllGameObjects.Add(go, nBundleId);
            if (parent != null)
            {
                go.transform.parent = parent.transform;
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;
                go.transform.localRotation = Quaternion.identity;
            }
            return go;
        }
        else
        {
            Debug.Log("<color=red> object is null: " + strFullAssetName + "\n" + "</color>");
        }
        return null;
    }
}
