using UnityEngine;
using System;
using System.IO;
using ICSharpCode.SharpZipLib.GZip;

public class ABHelper
{
    public static void Write2File(string filePathName, byte[] bytes)
    {
        if (File.Exists(filePathName))
        {
            File.Delete(filePathName);
        }

        string pathName = Path.GetDirectoryName(filePathName);
        if (!Directory.Exists(pathName))
            Directory.CreateDirectory(pathName);

        using (FileStream stream = new FileStream(filePathName, FileMode.Create))
        {
            stream.Write(bytes, 0, bytes.Length);
        }
    }

    public static bool ReadFile(string filePathName, out byte[] bytes)
    {
        FileStream stream = new FileStream(filePathName, FileMode.Open);
        bool ret = false;
        bytes = null;
        if (null != stream)
        {
            int len = (int)stream.Length;
            bytes = new byte[len];
            int readLend = stream.Read(bytes, 0, len);
            stream.Flush();
            stream.Close();
            ret = readLend == len;
        }
        return ret;
    }

    public static bool FileInCache(string path)
    {
        if (string.IsNullOrEmpty(path))
            return false;

        string cachePath = ABPathHelper.AssetsURL;
        cachePath += path;

        if (File.Exists(cachePath))
            return true;
        else
            return false;
    }

    /// <summary>
    /// 解压Byte流
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static byte[] GetDecompressData(byte[] data)
    {
        if (data == null || data.Length == 0)
            return null;

        GZipInputStream gzi = new GZipInputStream(new MemoryStream(data));

        MemoryStream re = new MemoryStream();
        try
        {
            int count = 0;
            byte[] midData = new byte[4096];
            while ((count = gzi.Read(midData, 0, midData.Length)) > 0)
            {
                re.Write(midData, 0, count);
                re.Flush();
            }
        }
        finally
        {
            gzi.Close();
            re.Close();
        }

        return re.ToArray();
    }

    /// <summary>
    /// 压缩Byte流
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static byte[] GetCompressData(byte[] data)
    {
        if (data == null || data.Length == 0)
            return null;

        MemoryStream re = new MemoryStream();
        GZipOutputStream gzi = new GZipOutputStream(re);
        try
        {
            gzi.SetLevel(9);
            gzi.Write(data, 0, data.Length);
        }
        finally
        {
            gzi.Close();
            re.Close();
        }

        return re.ToArray();
    }
    
    private static string fileDicPath = "";
    private static string fileExt = "";
    /// <summary>
    /// 存储解压缩后的ab文件数据
    /// </summary>
    /// <param name="path"></param>
    /// <param name="data"></param>
    public static void CacheFile(/*ref */string path, byte[] data)
    {
        try
        {
            if (string.IsNullOrEmpty(path))
                return;

            fileDicPath = Path.GetDirectoryName(path);
            fileExt = Path.GetExtension(path);
            if (!Directory.Exists(fileDicPath))
                Directory.CreateDirectory(fileDicPath);

            if (fileExt.Equals(ABPathHelper.Unity3dSuffix))
            {
                File.WriteAllBytes(path, GetDecompressData(data));
            }
            else
            {
                File.WriteAllBytes(path, data);
            }
        }
        catch (Exception e)
        {
            Debug.Log("本地缓存失败" + e.Message);
        }
    }

    // 只判断是否是被压缩过的
    public static bool CheckIfIsCompressed(string path)
    {
        if (string.IsNullOrEmpty(path)) return false;
        //fileDicPath = Path.GetDirectoryName(path);
        fileExt = Path.GetExtension(path);

        return fileExt.Equals(ABPathHelper.Unity3dSuffix);
    }

    // 直接缓存已经处理过的字节流，确保是已经解压过的
    public static void CacheUnCompressFile(string path, byte[] data)
    {
        try
        {
            if (string.IsNullOrEmpty(path)) return;
            fileDicPath = Path.GetDirectoryName(path);
            //fileExt = Path.GetExtension(path);
            if (!Directory.Exists(fileDicPath)) Directory.CreateDirectory(fileDicPath);

            File.WriteAllBytes(path, data);
        }
        catch (Exception e)
        {
            Debug.Log("本地缓存失败" + e.Message);
        }
    }

    /// <summary>
    /// 存储压缩后的ab文件数据
    /// </summary>
    /// <param name="path"></param>
    /// <param name="data"></param>
    public static void CacheCompressFile(string path, byte[] data)
    {
        try
        {
            if (string.IsNullOrEmpty(path))
                return;

            fileDicPath = Path.GetDirectoryName(path);
            fileExt = Path.GetExtension(path);
            if (!Directory.Exists(fileDicPath))
                Directory.CreateDirectory(fileDicPath);

            if (fileExt.Equals(ABPathHelper.Unity3dSuffix))
            {
                File.WriteAllBytes(path, GetCompressData(data));
            }
            else
            {
                File.WriteAllBytes(path, data);
            }
        }
        catch (Exception e)
        {
            Debug.Log("本地缓存失败" + e.Message);
        }
    }

    /// <summary>
    /// 存储为压缩文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="data"></param>
    public static void SaveCompressFile(string path, byte[] data)
    {
        try
        {
            if (string.IsNullOrEmpty(path))
                return;

            fileDicPath = Path.GetDirectoryName(path);
            fileExt = Path.GetExtension(path);
            if (!Directory.Exists(fileDicPath))
                Directory.CreateDirectory(fileDicPath);

            File.WriteAllBytes(path, GetCompressData(data));
        }
        catch (Exception e)
        {
            Debug.Log("本地缓存失败" + e.Message);
        }
    }

    /// <summary>
    /// 存储为非压缩的文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="data"></param>
    public static void SaveDecompressFile(string path, byte[] data)
    {
        try
        {
            if (string.IsNullOrEmpty(path))
                return;

            fileDicPath = Path.GetDirectoryName(path);
            fileExt = Path.GetExtension(path);
            if (!Directory.Exists(fileDicPath))
                Directory.CreateDirectory(fileDicPath);

            File.WriteAllBytes(path, GetDecompressData(data));
        }
        catch (Exception e)
        {
            Debug.Log("本地缓存失败" + e.Message);
        }
    }    

    public static void DeleteFile(string path)
    {
        if (string.IsNullOrEmpty(path))
            return;

        string cachePath = ABPathHelper.AssetsURL;
        cachePath += path;

        if (File.Exists(cachePath))
        {
            File.Delete(cachePath);
        }
    }    
}
