using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using System;

public class XMLHelper
{
    public static T Deserialize<T>(byte[] bytes) 
    {
        object obj = null;
        using (MemoryStream stream = new MemoryStream(bytes))
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                obj = xs.Deserialize(stream);
            }
            catch (System.Exception ex)
            {
                Debug.Log("Deserialize Error:" + ex.ToString());
            }
        }

        return obj != null ? (T)obj : default(T);
    }  

    public static object Deserialize(byte[] bytes, Type type)
    {
        object obj = null;
        using (MemoryStream stream = new MemoryStream(bytes))
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(type);
                obj = xs.Deserialize(stream);
            }
            catch (System.Exception ex)
            {
                Debug.Log("Deserialize Error:" + ex.ToString());
            }
        }

        return obj;
    }  
    
    public static object DeSerializerObject(string path, Type type)
    {
        object obj = null;
        if (!File.Exists(path))
        {
            return null;
        }

        using (Stream streamFile = new FileStream(path, FileMode.Open))
        {
            if (streamFile == null)
            {
                Debug.LogError("OpenFile Erro");
                return obj;
            }

            try
            {
                if (streamFile != null)
                {
                    XmlSerializer xs = new XmlSerializer(type);
                    obj = xs.Deserialize(streamFile);
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("DeSerializerObject Erro:" + ex.ToString());
            }
        }
        
        return obj;
    }


    public static object DeSerializerObjectResourceLoad(string path, Type type)
    {
        object objRet = null;
        if (path.Contains(ABPathHelper.ResourcePrefix))
            path = path.Remove(0, 10);
                
        TextAsset textFile = Resources.Load(path) as TextAsset;
        if (textFile == null)
        {
            Debug.Log("<color=yellow>不存在该文件：" + path + "</color>");
            return null;
        }

        using (MemoryStream stream = new MemoryStream(textFile.bytes))
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(type);
                objRet = xs.Deserialize(stream);
            }
            catch (System.Exception ex)
            {
                objRet = null;
                Debug.Log("Deserialize Error:" + ex.ToString());
            }
        }
        Resources.UnloadAsset(textFile);
        return objRet;
    }

    public static void SerializerObject(string path, object obj)
    {
        if (File.Exists(path))
        { 
            File.Delete(path);
        }

        using (Stream streamFile = new FileStream(path, FileMode.OpenOrCreate))
        {
            if (streamFile == null)
            {
                Debug.LogError("OpenFile Erro");
                return;
            }

            try
            {
                string strDirectory = Path.GetDirectoryName(path);
                if (!Directory.Exists(strDirectory))
                {
                    Directory.CreateDirectory(strDirectory);
                }
                
                XmlSerializer xs = new XmlSerializer(obj.GetType());
                TextWriter writer = new StreamWriter(streamFile, Encoding.UTF8);
                xs.Serialize(writer, obj);
                writer.Close();
            }
            catch (System.Exception ex)
            {
                Debug.LogError("SerializerObject Erro:" + ex.ToString());
            }
        }
    }
}
