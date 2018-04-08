using UnityEngine;
using System.Collections;
using System;

[CustomLuaClassAttribute]
public class FLEventBase
{
    //委托
    public delegate void FLEventHandler(FLEventBase e);

    private string _type;

    public string Type
    {
        set { _type = value; }
        get { return _type; }
    }

    public IFLEventDispatcher target;

    public FLEventBase()
    {
    }

    public FLEventBase(string type)
    {
        _type = type;
    }

    public virtual FLEventBase Clone()
    {
        FLEventBase e = (Activator.CreateInstance(this.GetType()) as FLEventBase);
        e.Type = this._type;
        return e;       
    }

    public void Clear()
    {
        selfClear();
    }

    protected virtual void selfClear()
    {
        target = null;
    }
}
