///author       xuhan
///Data         2016.07.19
///Description

using System;
using System.Collections.Generic;
using UnityEngine;

// 下载器的状态
public enum ELoadState
{
    None,
    CacheLoading,       //本地加载
    WebInQueue,         //下载状态
    WebLoading,         //网络loading
    CacheLoadError,     //从本地缓存中加载失败
    WebDownLoadError,   //从网络下载失败        
    WebLoadError,       //从下载下来的数据中加载时失败   
    UserCancel,
    Done,
}

public enum ELoadReqError
{
    None,
    NotFound,           //404
    OutOfTime,          //超时
    UserCancel,         //用户取消
}

//资源引用的类型(同步还是异步)
public enum ELoadMode
{
    None = 0,
    Asyn = 1,       //同步
    Syn = 2,        //异步
}

//那种方式加载数据
public enum LoadDataType
{
    None = 0,
    ResourcesType = 1,                      //resource.load加载
    AssetBundleType = 2,                    //assetbundle.loadasset加载
    LoadAssetAtPath = 3,                    //AssetDatabase.LoadAssetAtPath加载
}

