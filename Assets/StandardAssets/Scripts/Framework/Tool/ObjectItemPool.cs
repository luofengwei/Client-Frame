///author       xuhan
///Data         2016.12.21
///Description

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectItemPool<T> where T : new()
{
    private List<T> mActive = new List<T>();
    private List<T> mInActive = new List<T>();

    public T GetObject()
    {
        T item = default(T);
        if (mInActive.Count > 0)
        {
            item = mInActive[0];           
            mInActive.RemoveAt(0);
            mActive.Add(item);
        }
        else 
        {
            item = new T();
            mActive.Add(item);
        }

        return item;
    }

    public void RemoveObject(T item)
    {
        if (item != null && mActive.Contains(item))
        {
            mActive.Remove(item);
            mInActive.Add(item);
        }
    }

    public void PopFront()
    {
        if (mActive.Count > 0)
        {
            T item = mActive[0];
            mInActive.Add(item);
            mActive.RemoveAt(0);
        }
    }

    public int GetCount()
    {
        return mActive.Count;
    }

    public T GetItem(int nIdx)
    {
        return mActive.Count < nIdx ? mActive[nIdx] : default(T);
    }

    public void UnLoadAll()
    {
        if (mActive != null)
        {
            mActive.Clear();
            mActive = null;
        }

        if (mInActive != null)
        {
            mInActive.Clear();
            mInActive = null;
        }
    }
}