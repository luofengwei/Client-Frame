///Data         2018.02.06
///Description
///

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public class TcpConConstDefine
    {
        public static int TcpClientReceiveBufferSize = 256 * 1024;               //TCP接收缓存大小是256字节
        public static int TcpClientReceiveTimeout = 10000;

        public static int TcpClientSendBufferSize = 256 * 1024;                  //TCP发送缓存大小是256字节
        public static int TcpClientSendTimeout = 10000;

        public static int KeepaliveFlag = 1;	//开关
        public static int KeepaliveTime = 5;	//存货检测次数
        public static int KeepaliveInterval = 1; //存活检测间隔

        public static float MaxReConnectedTime = 18.0f;                 //最大重连时间
        public static float SendReCTime = 3.0f;                         //重试的间隔时间

        public static float MaxHeartTime = 50.0f;
    }

    public class NetWorkConnection : SingletonO<NetWorkConnection>
    {
        private CSafeStreamQueue msgQueue = null;                  //消息队列
        private TcpSocket tcpSocket = null;
        private NetWorkCallBack connetCallback = null;

        public void Init()
        {
            msgQueue = new CSafeStreamQueue();
            tcpSocket = new TcpSocket();          
        }

        public void ConnectServer(string strIp, int nPort, NetWorkCallBack callBack)
        {
            if (tcpSocket == null)
                return;

            GDebug.Log("TCP开始连接服务器: strIp: " + strIp + " nPort:" + nPort);
            connetCallback = callBack;
            tcpSocket.Connect(strIp, nPort);
        }

        public void ReConnectServer()
        {
            if (tcpSocket == null)
                return;
            GDebug.Log("TCP连接超时，重新连接服务器");
            tcpSocket.ReConnect();
        }

        public void Destroy()
        {
            if (tcpSocket != null)
                tcpSocket.Destroy();
            tcpSocket = null;

            if (msgQueue != null)
                msgQueue.Clear();
            msgQueue = null;

            connetCallback = null;
        }

        public void Disconnect()
        {
            if (tcpSocket != null)           
                tcpSocket.Disconnect();           

            msgQueue.Clear();
        }

        public void SendMsg(byte[] msg)
        {
            if (tcpSocket != null)
                tcpSocket.SendMsg(msg);
        }     

        public void Update()
        {
            if (tcpSocket != null)
                tcpSocket.Update();

            TriggerEvent();
        }

        private void TriggerEvent()
        {
            CEventMgr eventMgr = CEventMgr.Instance;
            if (eventMgr == null)
                return;

            for (int i = 0; i < 200; ++i)
            {
                stMsg msg = msgQueue.Get();
                if (msg != null)
                {
                    eventMgr.HandleCommond(msg.buffer);                   
                    msgQueue.Pop();
                }              
            }
        }

        public void Push(byte[] pData, int size)
        {
            if (msgQueue != null)
                msgQueue.Push(pData, size);
        }

        public void ChangeNetState(NETWORKSTATE state)
        {
            if (connetCallback != null)
            {
                connetCallback(state);
            }
        }
    }
}
