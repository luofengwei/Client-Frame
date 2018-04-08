//Data         2018.02.06
///Description
///

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameFramework
{
    [CustomLuaClassAttribute]
    public enum NETWORKSTATE
    {
        NS_NONE = 0,
        NS_CONNECTING,          //开始连接
        NS_WAITINGLOGIN,        //等待连接
        NS_CONNECTED,           //已经连接
        NS_DISCONNECTED,        //断开连接
        NS_CONNECTSERVERERROR,  //服务器连接异常
        NS_SERVERFULL,          //服务器爆满
        NS_WAITTING,            //排队
        NS_RETRY,               //重试
    }

    public delegate void NetWorkCallBack(NETWORKSTATE netState);
}