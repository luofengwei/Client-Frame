///Data         2018.02.06
///Description
///

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public class NetworkManager : SingletonO<NetworkManager>
    {
        public void Init()
        {
            NetWorkConnection.Instance.Init();
        }

        public void GameDestroy()
        {
            NetWorkConnection.Instance.Destroy();
        }

        public void ConnectServer(string ip, int nPort, NetWorkCallBack callBack = null)
        {
            NetWorkConnection.Instance.ConnectServer(ip, nPort, callBack); 
        }

        public void ReConnectServer()
        {
            NetWorkConnection.Instance.ReConnectServer();
        }

        public void Disconnect()
        {
            NetWorkConnection.Instance.Disconnect();
        }   

        private uint dwSendPackCount = 0;
        private List<byte> sendCache = new List<byte>();
        public void SendCommand<T>(uint id1, uint id2, T cmd)
        {
            byte[] pbdata = ProtoBufSerialize.Serialize(cmd);
            sendCache.Clear();

            CUtility.UInt8ToBytes(id1, sendCache);
            CUtility.UInt8ToBytes(id2, sendCache);
            CUtility.UInt32ToBytes(AddSendPackCount(), sendCache);
            CUtility.BytesToBytes(pbdata, pbdata.Length, sendCache);
       
            Send(sendCache.ToArray());
        }

        private void Send(byte[] msg)
        {
            NetWorkConnection.Instance.SendMsg(Packet.Wrap(msg));
        }

        private uint AddSendPackCount()
        {
            return dwSendPackCount++;
        }  
    }
}