using UnityEngine;
using GameFramework;
using System.Collections.Generic;
using System.IO;
using SLua;
using System;

public class LuaLoadOverrider : SingletonO<LuaLoadOverrider>
{ 
    public void Init()
    {
        SLua.LuaState.loaderDelegate += loadLuaFile;      
        MyLuaSrv.Instance.SetGlobalImportFunction(GetFullPath);
    }

    public void Dispose()
    {
        SLua.LuaState.loaderDelegate -= loadLuaFile;
        _instance = null;
    }

    string GetFullPath(string arg)
    {
        string res = arg;
        string find = string.Empty;
        if (ResourceID.pathData != null)
        {
            ResourceID.pathData.fileToFull.TryGetValue(res, out find);
            if (string.IsNullOrEmpty(find) == false)
                res = find;
        }

        return res;
    }
    
    byte[] loadLuaFile(string fn)
    {
        if (fn.EndsWith(".txt"))
            fn = fn.Replace(".txt", "");
        fn = fn.Replace(".", "/");
        byte[] res = null;
#if UNITY_EDITOR_OSX || UNITY_EDITOR
        if (fn.StartsWith("/") || fn.StartsWith("\\"))
            fn = fn.Remove(0, 1);
#endif
        TextAsset asset = null;
#if UNITY_EDITOR
        if (GameUtils.ScriptLoad)
        {
            fn = fn.ToLower();
            if (fn.IndexOf(ABPathHelper.AssetABPrefix) < 0)
            {
                fn = ABPathHelper.AssetABPrefix + fn;
            }
            if (fn.IndexOf(ABPathHelper.AssetTxtSuffix) < 0)
            {
                fn += ABPathHelper.AssetTxtSuffix;
            }
            //Debug.Log("<color=red>" + fn + "</color>");
            asset = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(fn);
        }
        else
#endif
        {
            //Debug.Log("<color=red>" + fn + "</color>");
            asset = ResourceManager.Instance.LoadScript(fn);
        }
    
        if (asset != null)
            res = asset.bytes;
        else
        {
            Debug.Log("<color=red>找不到："+fn+"</color>");
        }
        return res;
    }
}