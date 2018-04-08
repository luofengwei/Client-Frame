using UnityEngine;
using System.Collections;

public delegate bool DGameEventHandle(IEvent evt);

public interface IEvent
{
    int GetKey();
    object GetParam1();
    object GetParam2();
}

public class CSystemEvent : IEvent
{
    public ESysEvent mSysEvt;
    public object mParam1;
    public object mParam2;

    public CSystemEvent(ESysEvent sysEvt)
    {
        mSysEvt = sysEvt;
    }

    public CSystemEvent(ESysEvent sysEvt, object param1, object param2)
    {
        this.mSysEvt = sysEvt;
        this.mParam1 = param1;
        this.mParam2 = param2;
    }

    #region IEvent ≥…‘±
    public int GetKey()
    {
        return (int)mSysEvt;
    }

    public object GetParam1()
    {
        return mParam1;
    }

    public object GetParam2()
    {
        return mParam2;
    }
    #endregion
}


public class CGameEvent : IEvent
{
    private int key;
    private object param1;
    private object param2;

    public CGameEvent(int k, object p1)
    {
        key = k;
        param1 = p1;
        param2 = null;
    }

    public CGameEvent(int k, object p1, object p2)
    {
        key = k;
        param1 = p1;
        param2 = p2;
    }

    public CGameEvent(int k)
    {
        key = k;
        param1 = null;
        param2 = null;
    }

    public int GetKey()
    {
        return key;
    }

    public object GetParam1()
    {
        return param1;
    }

    public object GetParam2()
    {
        return param2;
    }
}
