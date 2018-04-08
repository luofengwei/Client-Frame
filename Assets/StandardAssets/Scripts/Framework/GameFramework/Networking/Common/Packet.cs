///Data         2018.02.07
///Description
///

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Security.Cryptography;

namespace GameFramework
{
    public enum SerializeType
    {
        None = 0,
        Encript = 1 << 0,
        Compress = 1 << 1,
        Max = 0xFF,
    }

    public class PacketConstDefine
    {
        public static int PacketHeadLen = 3;
        public static int MaxPacketSize = 64 * 1024;                            //最大包体大小64字节
        public static SerializeType SendSerializeType = SerializeType.None;
        public static SerializeType RecvSerializeType = SerializeType.None;
    }

    public class Packet
    {
        private static TripleDESCryptoServiceProvider des = null;
        private static ICryptoTransform dec = null;

        private static void Init()
        {
            if (dec == null)
            {
                des = new TripleDESCryptoServiceProvider();
                byte[] key = { 95, 27, 5, 20, 111, 4, 8, 88, 2, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0 };
                des.Key = key;
                des.Mode = CipherMode.ECB;
                des.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
                dec = des.CreateDecryptor();
            }
        }

        private static List<byte> dataCache = new List<byte>();
        public static byte[] Wrap(byte[] message, int packHeadLen = 3)
        {
            if (message == null)
                return null;

            uint msgLength = (uint)message.Length;

            if ((PacketConstDefine.SendSerializeType & SerializeType.Compress) != 0)
            {

            }
            if ((PacketConstDefine.SendSerializeType & SerializeType.Encript) != 0)
            {

            }

            dataCache.Clear();
            uint flag = 0;

            CUtility.UInt8ToBytes(flag, dataCache);
            CUtility.UInt16ToBytes(msgLength, dataCache);
            CUtility.BytesToBytes(message, (int)msgLength, dataCache);
            return dataCache.ToArray();
        }

        public static void UnWrap(ref byte[] recvBuffer, ref int nRecvLen)
        {
            int offset = 0;
            bool bCompress = false;
            bool bEncrypt = false;
            int nMessageLen = 0;
            int datasize = nRecvLen;
            int nPacketHeadLen = PacketConstDefine.PacketHeadLen;

            //尝试读取包头
            while (offset + nPacketHeadLen < datasize)
            {
                if (nMessageLen == 0)
                {
                    //包头还没有读取过
                    //第一个字节 是否压缩
                    //第二个 + 第三个 代表包体长度 
                    bCompress = (recvBuffer[offset] & 0x01) > 0;
                    bEncrypt = (recvBuffer[offset] & 0x02) > 0;
                    nMessageLen = CUtility.BytesToUshort(recvBuffer, offset + 1);
                }

                if (nMessageLen <= 0)
                {
                    //GDebug.LogError("<color=red> 消息长度小于等于0 </color>");
                    offset += nPacketHeadLen;
                    nMessageLen = 0;
                    continue;
                }
                else if (offset + nMessageLen + nPacketHeadLen > datasize)
                {
                    //byte offset0 = recvBuffer[offset];
                    //UInt16 offset12 = CUtility.BytesToUshort(recvBuffer, offset+1);
                    //byte offset3 = recvBuffer[offset + 3];    //大消息号 
                    //byte offset4 = recvBuffer[offset + 4];    //小消息号
                    //GDebug.LogError("<color=red> 被拆分的包体 offset0 = " + offset0 + " offset12 = " + offset12 + " offset3 = " + offset3 + " offset4 = " + offset4 + " iscompress = " + m_bCompress + " m_nMessageLen = " + m_nMessageLen + " offset  " + offset + "  datasize " + datasize + "</color>");
                    break;
                }

                //尝试读取包体
                if ((offset + nMessageLen + nPacketHeadLen) <= datasize)
                {
                    //消息包头已经读取，并且剩下的数据足够读包体
                    //先解密再解压
                    byte[] bytes = CUtility.BytesToBytes(recvBuffer, offset + nPacketHeadLen, nMessageLen);
                    byte[] desdata;
                    if (bEncrypt)
                    {
                        Init();
                        desdata = dec.TransformFinalBlock(bytes, 0, bytes.Length);
                    }
                    else
                    {
                        desdata = bytes;
                    }

                    byte[] uncompressData;
                    // 判断是否需要解压
                    if (bCompress)
                        uncompressData = CUtility.deCompressBytes(desdata);
                    else
                        uncompressData = desdata;

                    NetWorkConnection.Instance.Push(uncompressData, (int)uncompressData.Length);

                    offset += nMessageLen + nPacketHeadLen;
                    nMessageLen = 0;
                }
            }

            nRecvLen = datasize - offset;
            if (nRecvLen > 0)
            {
                Array.Copy(recvBuffer, offset, recvBuffer, 0, nRecvLen);
            }           
        }
    }
}
