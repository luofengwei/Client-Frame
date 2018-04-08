using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//回头这块要优化消息存储，使用队列流，存储在一块连续的内存上,不要每次重新分配 by xuhan

//基础的消息结构
public class stMsg
{
    public byte[] buffer;
    public int size;
}

public class CStreamQueue
{
    public List<stMsg> m_queue = new List<stMsg>();    //队列

    //错误枚举
    public enum enumError
    {
        eErrorOk = 0,           //正常
        eErrorNoData,           //没有数据
        eErrorMsgSize,          //消息大小有问题
    }

    //默认构造函数(初始化数据)
    public CStreamQueue()
    {
    }

    //清除
    public void Clear()
    {
        lock (m_queue)
        {
            m_queue.Clear();
        }
    }

    //判断当前是否为空
    public bool Empty()
    {
        lock (m_queue)
        {
            return m_queue.Count <= 0;
        }
    }

    //添加大小为size的字节数组pData
    public void Push(byte[] pData, int size)
    {
        if (pData == null || size == 0)
            return;

        stMsg msg = new stMsg();
        msg.buffer = new byte[size];
        msg.size = size;
        Array.Copy(pData, 0, msg.buffer, 0, size);       //新加入的消息设置数据

        lock (m_queue)
        {
            m_queue.Add(msg);
        }
    }

    //获取当前队列的数量
    public int GetSize()
    {
        lock (m_queue)
        {
            return m_queue.Count;
        }
    }

    //取出消息
    public void Pop()
    {
        lock (m_queue)
        {
            if (m_queue.Count <= 0)
            {
                return;
            }

            //队列中移除该消息
            m_queue.Remove(m_queue[0]);
        }
    }

    public stMsg Get()
    {
        lock (m_queue)
        {
            if (m_queue.Count <= 0)
                return null;

            return m_queue[0];
        }
    }
};

public class CSafeStreamQueue : CStreamQueue
{
    //CLock m_lock;
    public CSafeStreamQueue() { }
}
