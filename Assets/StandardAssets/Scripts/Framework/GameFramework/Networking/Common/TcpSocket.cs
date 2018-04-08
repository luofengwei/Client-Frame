///Data         2018.02.08
///Description
///

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.IO;
using System;
using System.Diagnostics;
using System.Threading;
using System.Net;
using System.Net.NetworkInformation;

namespace GameFramework
{
    public class TcpSocket : NetThread
    {       
        private TcpClient tcpClient = null;
        private NetworkStream netStream = null;
        private NetWorkStateManager stateMgr = null;
      
        private string strIp = string.Empty;
        private int nPort = 0;

        //接收缓存
        private byte[] m_recBuf = null;
        private byte[] m_recvBuffer = null;
        private int m_recvBufferLen = 0;
        private int m_recBufOffset = 0;

	    //发送缓存
        private byte[] m_sendBuffer = null;
        private int m_sendBufferLen = 0;
	      
        public TcpSocket()
        {
            stateMgr = new NetWorkStateManager(this);

            int nMaxPacketSize = PacketConstDefine.MaxPacketSize;
            m_recBuf = new byte[nMaxPacketSize];
		    m_recvBuffer = new byte[nMaxPacketSize];
		    m_sendBuffer = new byte[nMaxPacketSize];        
        }

        public string GetIP()
        {
            return strIp;
        }

        public int GetPort()
        {
            return nPort;
        }

        public TcpClient GetClient()
        {
            return tcpClient;
        } 

        public void SetClient(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
        }

        public void CloseClient()
        {
            if (netStream != null)
                netStream.Close();
            netStream = null;

            if (tcpClient != null)
                tcpClient.Close();
            tcpClient = null;     
        }

        public NETWORKSTATE GetNetState()
        {
            return stateMgr != null ? stateMgr.GetNetState() : NETWORKSTATE.NS_NONE;
        }

        public void ChangeState(NETWORKSTATE state)
        {
            if (stateMgr != null)
            {
                stateMgr.ChangeState(state);               
            }
        }

        public void SetConnectErrState(string log)
        {            
            GDebug.LogError("连接错误: " + log);
            if (GetNetState() != NETWORKSTATE.NS_RETRY)
            {
                //ChangeState(NETWORKSTATE.NS_CONNECTSERVERERROR);
                ChangeState(NETWORKSTATE.NS_DISCONNECTED);
            }
        }

        public bool IsUseThread()
        {
            return true;
        }

        public bool IsConnected()
        {
            return tcpClient != null && tcpClient.Connected;
        }  

        public void Connect(string strIp, int nPort)
        {
            this.strIp = strIp;
            this.nPort = nPort;

            ChangeState(NETWORKSTATE.NS_CONNECTING);
        }

        public void ReConnect()
        {
            ChangeState(NETWORKSTATE.NS_RETRY);
        }

        public void OnConnected()
        {
            if (!IsConnected())
                return;

            GDebug.Log("TCP连接成功");
            bool success = false;
            try
            {
                //成功, 设置属性
                SetProperty();
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
                if (ex is SocketException)
                {
                    SocketException socketExcept = (SocketException)ex;
                    if (socketExcept.ErrorCode == 10055)//No buffer space available. iphone4上会出这个异常，但实际连接还是有效
                    {
                        success = true;
                    }
                }         
            }
            finally
            {      
                if (success)
                {
                    ChangeState(NETWORKSTATE.NS_CONNECTED);
                    netStream = tcpClient.GetStream();
                    if (IsUseThread())
                    {
                        Run();
                    }
                }
                else
                {
                    SetConnectErrState("设置属性失败");
                }
            }
        }

        private void SetProperty()
        {
            if (!IsConnected())
                return;
            tcpClient.NoDelay = true;

            tcpClient.ReceiveBufferSize = TcpConConstDefine.TcpClientReceiveBufferSize;
            tcpClient.ReceiveTimeout = TcpConConstDefine.TcpClientReceiveTimeout;

            tcpClient.SendBufferSize = TcpConConstDefine.TcpClientSendBufferSize;
            tcpClient.SendTimeout = TcpConConstDefine.TcpClientSendTimeout;

            byte[] buffer = new byte[12];
            BitConverter.GetBytes(TcpConConstDefine.KeepaliveFlag).CopyTo(buffer, 0);
            BitConverter.GetBytes(TcpConConstDefine.KeepaliveTime).CopyTo(buffer, 4);
            BitConverter.GetBytes(TcpConConstDefine.KeepaliveInterval).CopyTo(buffer,8);
            tcpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, buffer);
        }

        public void Disconnect()
        {
            if (IsUseThread())
            {
                SetTerminateFlag();
            }
            else
            {
                CloseClient();
            }
        }

        public void Destroy()
        {
            Disconnect();
            stateMgr = null;
		    m_recBuf = null;
            m_recvBuffer = null;
		    m_sendBuffer = null;
	    }

        public void SendMsg(byte[] msg) 
        {
            if (null != msg && msg.Length > 0)
            {
                while ((m_sendBufferLen + msg.Length) > m_sendBuffer.Length)
                {
                    Array.Resize(ref m_sendBuffer, m_sendBuffer.Length + PacketConstDefine.MaxPacketSize);
                }
                Array.Copy(msg, 0, m_sendBuffer, m_sendBufferLen, msg.Length);
                m_sendBufferLen += msg.Length;
                SendMsg();
            }
        }

        private void SendMsg()
        {
            if (netStream == null)
                return;

            if (!IsConnected())
            {
                SetConnectErrState("消息发送失败，重新连接服务器");
                return;
            }

            int offset = 0;
            while (offset < m_sendBufferLen)
            {
                if (netStream.CanWrite != true)
                    break;

                try
                {
                    int len = (m_sendBufferLen - offset) < PacketConstDefine.MaxPacketSize ? (m_sendBufferLen - offset) : PacketConstDefine.MaxPacketSize;
                    netStream.Write(m_sendBuffer, offset, len);
                    offset += len;
                }
                catch (IOException e)
                {
                    if (e.InnerException is SocketException)
                    {
                        SetConnectErrState("消息发送失败，重新连接服务器");
                    }
                    break;
                }
            }

            if (offset > 0)
            {
                m_sendBufferLen -= offset;
                if (m_sendBufferLen > 0)
                {
                    Array.Copy(m_sendBuffer, offset, m_sendBuffer, 0, m_sendBufferLen);
                }
            }
        }

        public void Update()
        {
            if (stateMgr != null)
                stateMgr.Update();
        }

        public void UpdateStream()
        { 
            while (ReadFromStream())
            {
                Packet.UnWrap(ref m_recvBuffer, ref m_recvBufferLen);
            }
        }   

        private bool ReadFromStream()
        {
            if (!IsConnected())
            {
                return false;
            }

            if (netStream != null && netStream.DataAvailable)
            {
                //if no use thread
                if (!IsUseThread())
                {
                    netStream.BeginRead(m_recBuf, 0, m_recBuf.Length, new AsyncCallback(CallBackReadMethod), netStream);
                    return false;
                }
                else
                {
                    try
                    {
                        m_recBufOffset = netStream.Read(m_recBuf, 0, m_recBuf.Length);
                        if (m_recBufOffset <= 0)
                        {
                            return false;
                        }
                        while ((m_recvBufferLen + m_recBufOffset) > m_recvBuffer.Length)
                        {
                            Array.Resize(ref m_recvBuffer, m_recvBuffer.Length + PacketConstDefine.MaxPacketSize);
                        }
                        Array.Copy(m_recBuf, 0, m_recvBuffer, m_recvBufferLen, m_recBufOffset);
                        m_recvBufferLen += m_recBufOffset;
                    }
                    catch (System.Exception)
                    {
                        NGUIDebug.Log("receive bug");
                    }
                    return true;
                }
            }
            return false;
        }

        private void CallBackReadMethod(IAsyncResult ar)
        {
            try
            {
                NetworkStream state = (NetworkStream)ar.AsyncState;
                m_recBufOffset = state.EndRead(ar);
                if (m_recBufOffset > 0)
                {
                    while ((m_recvBufferLen + m_recBufOffset) > m_recvBuffer.Length)
                    {
                        Array.Resize(ref m_recvBuffer, m_recvBuffer.Length + PacketConstDefine.MaxPacketSize);
                    }
                    Array.Copy(m_recBuf, 0, m_recvBuffer, m_recvBufferLen, m_recBufOffset);
                    m_recvBufferLen += m_recBufOffset;

                    Packet.UnWrap(ref m_recvBuffer, ref m_recvBufferLen);
                }
                else
                {
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        protected override void ThreadProc(object obj)
        {
            TcpSocket me = (TcpSocket)obj;
            var sw = Stopwatch.StartNew();
       
            while (!me.IsTerminateFlagSet())
            {
                me.UpdateStream();
                if (sw.ElapsedMilliseconds < 10)
                {
                    Thread.Sleep((int)(10 - sw.ElapsedMilliseconds));
                }
                sw.Reset();
            }
            me.CloseClient();
        }      	       
	}
}