///Data         2018.02.06
///Description
///

using UnityEngine;
using System.Collections;
using System.IO;
using System;
//using ProtoBuf;

namespace GameFramework
{
    public class ProtoBufSerialize
    {      
        static public byte[] Serialize<T>(T msg)
        {
            byte[] result = null;
            if (msg != null)
            {
                using (var stream = new MemoryStream())
                {
                    //Serializer.Serialize<T>(stream, msg);
                    result = stream.ToArray();
                }
            }
            return result;
        }

        static public T Deserialize<T>(byte[] message)
        {
            T result = default(T);
            if (message != null)
            {
                using (var stream = new MemoryStream(message))
                {
                    //result = Serializer.Deserialize<T>(stream);
                }
            }
            return result;
        }
    }
}
