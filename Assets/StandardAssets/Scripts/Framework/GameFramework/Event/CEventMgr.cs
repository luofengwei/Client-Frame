#define LOG_EVENT
//#define PROFILE

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class CEventMgr : CGameSystem
{
    private static CEventMgr _Instance = null;
    public static CEventMgr Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = CGameRoot.GetGameSystem<CEventMgr>();
            }
            return _Instance;
        }
    }

    readonly string[] c_szErrorMessage = new string[]{
        "未知错误",
        "数据库出错",
        "帐号正在被使用中",
        "帐号已被禁用",
        "服务器维护中，请稍后登陆",
        "用户已满，请选择另一组服务器",
        "帐号待激活",

        "未知错误",
        "未知错误",
        "未知错误",
        "未知错误",
        "未知错误",

        "ForceChangeUserPos色名重复",
        "用户档案不存在",
    };


    enum RegRet
    {
        REG_SUCC = 0,  // 登陆成功
        REG_FAIL = 1,  // 未定义错误
        REG_SHA1_ERROR = 2,
        REG_LOGIN_OVERTIME = 3,  //登录超时
        REG_SNAP_SHOT_FAIL = 4,  //获取快照失败
        REG_WRONG_ZONE = 5,  //大厅错误
        REG_WRONG_NAME = 6,  //名字错误
        REG_RELOGIN = 10,  //重复登录
        REG_RELOGIN_DIFF_GATE = 11,  //不同网关重复登录
        REG_RELOGIN_SCENE = 12,  //场景重复登录
        REG_SESSION_CREATE_FAILD = 15,  //会话创建角色失败
        REG_NOTIFY_SCENE = 16,  //未找到对应场景
        REG_SCENE_CREATE_FAILD = 17,  //场景创建角色失败
        REG_ERR_FIND_GATE = 18,  //找不到登录网关
        REG_ERR_REQ_DATA_FROM_RECORD = 19,  //从record请求数据错误
        REG_ERR_SET_USER_DATA_SCENE = 20,  //设置玩家数据失败
        REG_ERR_FORBID_REG,  //被封号
        REG_ERR_ENTER_SCENE,  //进入场景失败
        REG_ERR_GET_USER_DATA,  //读档失败
        REG_ERR_SET_USER_DATA,  //设置玩家数据失败
        REG_ERR_ADD_TO_MANAGER,
    };


    class CEventData
    {
        public bool mObjRelate;
        public DGameEventHandle mHandle;
        public GameObject mGameObj;

        public CEventData(DGameEventHandle handle)
        {
            mObjRelate = false;
            mHandle = handle;
            mGameObj = null;
        }

        public CEventData(DGameEventHandle handle, GameObject gameObj)
        {
            mObjRelate = true;
            mHandle = handle;
            mGameObj = gameObj;
        }
    }

    public bool mLimitQueueProcesing = false;
    public float mQueueProcessTime = 0.0f;

    private Dictionary<int, List<CEventData>> mListenerTable = new Dictionary<int, List<CEventData>>();
    private Queue mEventQueue = new Queue();

    public override bool SysEnter()
    {
        return false;
    }

    public bool AddListener(int eventkey, DGameEventHandle eventHandle)
    {
        if (eventHandle == null)
        {
            return false;
        }

        if (!mListenerTable.ContainsKey(eventkey))
            mListenerTable.Add(eventkey, new List<CEventData>());

        List<CEventData> listenerList = mListenerTable[eventkey];

        listenerList.Add(new CEventData(eventHandle));

        return true;
    }

    /// <summary>
    /// 为避免回调时被调方是一个已经被删除的GameObj上的组件，造成异常，组件需要使用此函数注册
    /// </summary>
    /// <param name="eventkey"></param>
    /// <param name="eventHandle"></param>
    /// <param name="gameObj"></param>
    /// <returns></returns>
    public bool AddListener(int eventkey, DGameEventHandle eventHandle, GameObject gameObj)
    {
        if (eventHandle == null)
        {
            return false;
        }

        if (!mListenerTable.ContainsKey(eventkey))
            mListenerTable.Add(eventkey, new List<CEventData>());

        List<CEventData> listenerList = mListenerTable[eventkey];

        listenerList.Add(new CEventData(eventHandle, gameObj));

        return true;
    }

    public bool DetachListener(int eventKey, DGameEventHandle eventHandle)
    {
        if (!mListenerTable.ContainsKey(eventKey))
            return false;

        List<CEventData> listenerList = mListenerTable[eventKey];

        CEventData find = null;
        foreach (CEventData evtData in listenerList)
        {
            if (evtData.mHandle == eventHandle)
            {
                find = evtData;
                break;
            }
        }

        if (find != null)
            listenerList.Remove(find);

        return true;
    }

    /// <summary>
    /// 同步事件触发
    /// </summary>
    /// <param name="evt"></param>
    /// <returns></returns>
    public void TriggerEvent(IEvent evt)
    {
        int eventKey = evt.GetKey();
        List<CEventData> listenerList = null;
        if (mListenerTable.TryGetValue(eventKey, out listenerList))
        {
            //防止在事件处理流程中改变事件列表
            List<CEventData> tmpList = new List<CEventData>(listenerList);
            for (int i = 0; i < tmpList.Count; ++i)
            {
                CEventData evtData = tmpList[i];

                if (evtData.mHandle != null && (!evtData.mObjRelate || evtData.mGameObj != null))
                {
                    evtData.mHandle(evt);
                }
            }

            for (int i = 0; i < tmpList.Count; ++i)
            {
                if (tmpList[i].mObjRelate && tmpList[i].mGameObj == null)
                {
                    listenerList.Remove(tmpList[i]);
                }
            }
        }

        //改变事件等待协程的状态
        if (mEvtWaiterMap.ContainsKey(eventKey))
        {
            foreach (CEvtWaiter waiter in mEvtWaiterMap[eventKey])
            {
                waiter.mEvt = evt;
            }
            mEvtWaiterMap[eventKey].Clear();
            mEvtWaiterMap[eventKey] = null;
            mEvtWaiterMap.Remove(eventKey);
        }
    }

    public bool QueueEvent(IEvent evt)
    {
        mEventQueue.Enqueue(evt);
        return true;
    }

    public override void SysUpdate()
    {
        float timer = 0.0f;
        while (mEventQueue.Count > 0)
        {
            if (mLimitQueueProcesing)
            {
                if (timer > mQueueProcessTime)
                    return;
            }

            IEvent evt = mEventQueue.Dequeue() as IEvent;
            TriggerEvent(evt);

            if (mLimitQueueProcesing)
                timer += Time.deltaTime;
        }
    }

    public override void SysFinalize()
    {
        if (mListenerTable != null)
        {
            mListenerTable.Clear();
            mListenerTable = null;
        }
        if (mEventQueue != null)
        {
            mEventQueue.Clear();
            mEventQueue = null;
        }
        if (mEvtWaiterMap != null)
        {
            mEvtWaiterMap.Clear();
            mEvtWaiterMap = null;
        }
        base.SysFinalize();
    }

    Dictionary<int, List<CEvtWaiter>> mEvtWaiterMap = new Dictionary<int, List<CEvtWaiter>>();

    public IEnumerator WaitEvent(CEvtWaiter waiter)
    {
        if (mEvtWaiterMap.ContainsKey(waiter.mEvtKey))
        {
            mEvtWaiterMap[waiter.mEvtKey].Add(waiter);
        }
        else
        {
            mEvtWaiterMap.Add(waiter.mEvtKey, new List<CEvtWaiter>());
            mEvtWaiterMap[waiter.mEvtKey].Add(waiter);
        }

        float waitTime = waiter.mWaitTime;
        float startTime = Time.time;

        while (waiter.mEvt == null)
        {
            if (waiter.mWaitTime > 0
                && Time.time - startTime > waitTime)
            {
                waiter.mState = EEvtWaiterState.OutOfTime;
                yield break;
            }
            yield return null;
        }
        waiter.mState = EEvtWaiterState.Received;
    }


    public void HandleCommond(byte[] msg, bool bFromServer = true)
    {
        if (msg.Length < 6)
        {
            //throw new IndexOutOfRangeException("msg error, recieve msg length lower 3");
            return;
        }

        uint gsCmd = (uint)msg[0];					//大消息号
        uint gsParamType = (uint)msg[1];				//小消息号
        ////Debug.LogError("gsCmd : " + gsCmd + "   gsParamType : " + gsParamType + "     msg.Length : " + msg.Length);
        byte[] data = CUtility.BytesToBytes(msg, 6, msg.Length - 6);    //取出剩余数据包
 
        switch (gsCmd)
        {           
            default:
                 break;
        }      
    } 
  
    bool StartGame()
    {
        return true;
    }

}

public enum EEvtWaiterState
{
    Waiting,
    Received,
    OutOfTime,
}


public class CEvtWaiter
{
    public const float cDefaultWaitTime = 10f;//20f

    /// <summary>
    /// 使用默认超时时间
    /// </summary>
    /// <param name="evtKey"></param>
    public CEvtWaiter(int evtKey)
    {
        mEvtKey = evtKey;
        mWaitTime = cDefaultWaitTime;
        mEvt = null;
    }

    /// <summary>
    /// 自定义超时时间，-1表示不使用超时机制
    /// </summary>
    /// <param name="evtKey"></param>
    /// <param name="waitTime"></param>
    public CEvtWaiter(int evtKey, float waitTime)
    {
        mEvtKey = evtKey;
        mWaitTime = waitTime;
        mEvt = null;
    }

    public int mEvtKey;
    public float mWaitTime;
    public IEvent mEvt;
    public EEvtWaiterState mState = EEvtWaiterState.Waiting;
}
