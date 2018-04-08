//#define DEBUG_LOG
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using UnityEngine;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using ComponentAce.Compression.Libs.zlib;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

[CustomLuaClassAttribute]
public class CUtility
{
    public static string SHA1(string text)
    {
        byte[] cleanBytes = Encoding.Default.GetBytes(text);
        byte[] hashedBytes = System.Security.Cryptography.SHA1.Create().ComputeHash(cleanBytes);
        return BitConverter.ToString(hashedBytes).Replace("-", "");
    }

    public static IEnumerator GetGameObjAllChildEnumerator(GameObject go)
    {
        List<GameObject> ret = new List<GameObject>();
        GetChildGameObjects(go, ret);

        foreach (GameObject obj in ret)
        {
            yield return obj;
        }
    }

    public static IEnumerator GetGameObjTreeEnumerator(GameObject go)
    {
        List<GameObject> ret = new List<GameObject>();
        GetChildGameObjects(go, ret);

        yield return go;
        foreach (GameObject obj in ret)
        {
            yield return obj;
        }
    }

    public static void GetChildGameObjects(GameObject go, List<GameObject> ret)
    {
        foreach (Transform trans in go.transform)
        {
            ret.Add(trans.gameObject);
            GetChildGameObjects(trans.gameObject, ret);
        }
    }

    public static GameObject GetGameObjRoot(GameObject obj)
    {
        List<GameObject> paths = new List<GameObject>();
        return GetGameObjRoot(obj, paths);
    }

    public static GameObject GetGameObjRoot(GameObject obj, List<GameObject> paths)
    {
        paths.Clear();

        Transform rootTransform = obj.transform;

        while (rootTransform.parent != null)
        {
            paths.Add(rootTransform.gameObject);
            rootTransform = rootTransform.parent;
        }

        paths.Add(rootTransform.gameObject);
        paths.Reverse();

        return rootTransform.gameObject;
    }

    public static GameObject GetGameObjRoot(GameObject obj, out string path)
    {
        List<GameObject> pathObjs = new List<GameObject>();
        GameObject root = GetGameObjRoot(obj, pathObjs);
        path = "";
        foreach (GameObject go in pathObjs)
        {
            path += go.name + '/';
        }
        return root;
    }

    /// <summary>
    /// 通过名字查找物件
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static GameObject GetGameObjectForName(GameObject obj, string name)
    {
        GameObject link = null;
        Transform[] temparr = obj.GetComponentsInChildren<Transform>();

        foreach (Transform child in temparr)
        {
            if (child.name == name)
            {
                link = child.gameObject;
                break;
            }
        }
        return link;
    }

    public static void FileSize(string path,ref long fileSize)
    {
        if(!File.Exists(path))
        {
            return;
        }

        using (FileStream get_file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            fileSize = get_file.Length;
        }
    }

    public static string MD5Hash(string path,ref string fileMD5,ref long fileSize)
    {
        if (!File.Exists(path))
        {
            return "";
        }

        using (FileStream get_file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            MD5CryptoServiceProvider get_md5 = new MD5CryptoServiceProvider();
            byte[] hash_byte = get_md5.ComputeHash(get_file);
            fileSize = get_file.Length;
            get_file.Close();

            fileMD5 = System.BitConverter.ToString(hash_byte);
            fileMD5 = fileMD5.Replace("-", "");

            get_md5.Clear();
        }
        return fileMD5;
    }

    public static string MD5Hash(string path, ref string fileMD5)
    {
        using (FileStream get_file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            MD5CryptoServiceProvider get_md5 = new MD5CryptoServiceProvider();
            byte[] hash_byte = get_md5.ComputeHash(get_file);
            get_file.Close();

            fileMD5 = System.BitConverter.ToString(hash_byte);
            fileMD5 = fileMD5.Replace("-", "");

            get_md5.Clear();
        }
        return fileMD5;
    }

    public static string MD5Hash(string path)
    {
        string retFileMD5 = "";
        using (FileStream get_file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            MD5CryptoServiceProvider get_md5 = new MD5CryptoServiceProvider();
            byte[] hash_byte = get_md5.ComputeHash(get_file);
            //get_file.Length;
            get_file.Close();

            retFileMD5 = System.BitConverter.ToString(hash_byte);
            retFileMD5 = retFileMD5.Replace("-", "");

            get_md5.Clear();
        }
        return retFileMD5;
    }

    public static string MD5HashString(string str)
    {
        byte[] data = Encoding.UTF8.GetBytes(str);
        MD5CryptoServiceProvider get_md5 = new MD5CryptoServiceProvider();
        byte[] hash_byte = get_md5.ComputeHash(data);
        string result = System.BitConverter.ToString(hash_byte);
        result = result.Replace("-", "");
        return result;
    }

    public static string MD5HashString(byte[] data)
    {
        MD5CryptoServiceProvider get_md5 = new MD5CryptoServiceProvider();
        byte[] hash_byte = get_md5.ComputeHash(data);
        string result = System.BitConverter.ToString(hash_byte);
        result = result.Replace("-", "");
        return result;
    }

    public static string TdrBytesToString(byte[] bytes)
    {
        char[] array = Encoding.UTF8.GetChars(bytes);
        int len = 0;
        for (; len < array.Length; ++len)
            if (array[len] == 0) break;

        return new string(array, 0, len);
    }

    public static void StringToTdrBytes(byte[] bytes, string str)
    {
        if (bytes == null || str == null) return;
        Encoding.UTF8.GetBytes(str, 0, str.Length, bytes, 0);
    }

    public static void StringToTdrBytesDefault(byte[] bytes, string str)
    {
        if (bytes == null || str == null) return;
        //CUtility.DebugLog("StringToTdrBytesDefault = " + str);
        Encoding.UTF8.GetBytes(str, 0, str.Length, bytes, 0);
    }

    public static string TdrBytesToStringDefault(byte[] bytes)
    {
        //return Encoding.Default.GetString(bytes);
        char[] array = Encoding.UTF8.GetChars(bytes);
        int len = 0;
        for (; len < array.Length; ++len)
            if (array[len] == 0) break;
        string temp = new string(array, 0, len);

        return temp;
    }


    // 字符串可视长度，ASCII以内的算1个长度，以上的算两个长度 [10/27/2011 asokawu]
    public static int GetStringVisualLength(string str)
    {
        if (str == null) return 0;

        char[] array = str.ToCharArray();
        int len = 0;
        foreach (char c in array)
        {
            if (c == 0)
                break;
            else if (c <= 0xff)
                len += 1;
            else
                len += 2;
        }
        return len;
    }
    
    public static string ChangeIP(uint ip)
    {
        IPAddress ipaddress = IPAddress.Parse(ip.ToString());
        string strdreamduip = ipaddress.ToString();
        string iptemp;
        string[] vals = strdreamduip.Split('.');
        if (vals != null && vals.Length == 4)
        {
            iptemp = vals[3] + "." + vals[2] + "." + vals[1] + "." + vals[0];
        }
        else
        {
            iptemp = null;
        }
        return iptemp;
    }

    public static string ToHexColor(Color color)
    {
        return string.Format("#{0}{1}{2}", ((int)(color.r * 255)).ToString("x2"), ((int)(color.g * 255)).ToString("x2"), ((int)(color.b * 255)).ToString("x2"));
    }

    public static void MoveToLayer(Transform root, int layer)
    {
        Stack<Transform> moveTargets = new Stack<Transform>();
        moveTargets.Push(root);
        Transform currentTarget;
        while (moveTargets.Count != 0)
        {
            currentTarget = moveTargets.Pop();
            currentTarget.gameObject.layer = layer;
            foreach (Transform child in currentTarget)
                moveTargets.Push(child);
        }
    }

    public static string IdentityRoomNum(int num)
    {
        if (num > 9999)
        {
            return num.ToString();
        }
        else if (num > 999)
        {
            return "0" + num.ToString();
        }
        else if (num > 99)
        {
            return "00" + num.ToString();
        }
        else if (num > 9)
        {
            return "000" + num.ToString();
        }
        else
        {
            return "0000" + num.ToString();
        }
    }
    
    public static string GetKeyString(KeyCode key)
    {
        switch (key)
        {

            case KeyCode.KeypadPeriod:
                return "Keypad.";
            case KeyCode.KeypadDivide:
                return "Keypad/";
            case KeyCode.KeypadMultiply:
                return "Keypad*";
            case KeyCode.KeypadMinus:
                return "Keypad-";
            case KeyCode.KeypadPlus:
                return "Keypad+";
            case KeyCode.KeypadEquals:
                return "Keypad=";
            case KeyCode.UpArrow:
                return "↑";
            case KeyCode.DownArrow:
                return "↓";
            case KeyCode.LeftArrow:
                return "←";
            case KeyCode.RightArrow:
                return "→";
            case KeyCode.Alpha0:
                return "0";
            case KeyCode.Alpha1:
                return "1";
            case KeyCode.Alpha2:
                return "2";
            case KeyCode.Alpha3:
                return "3";
            case KeyCode.Alpha4:
                return "4";
            case KeyCode.Alpha5:
                return "5";
            case KeyCode.Alpha6:
                return "6";
            case KeyCode.Alpha7:
                return "7";
            case KeyCode.Alpha8:
                return "8";
            case KeyCode.Alpha9:
                return "9";
            case KeyCode.Exclaim:
                return "!";
            case KeyCode.DoubleQuote:
                return "\"";
            case KeyCode.Hash:
                return "#";
            case KeyCode.Dollar:
                return "$";
            case KeyCode.Ampersand:
                return "&";
            case KeyCode.Quote:
                return "'";
            case KeyCode.LeftParen:
                return "(";
            case KeyCode.RightParen:
                return ")";
            case KeyCode.Asterisk:
                return "*";
            case KeyCode.Plus:
                return "+";
            case KeyCode.Comma:
                return ",";
            case KeyCode.Minus:
                return "-";
            case KeyCode.Period:
                return ".";
            case KeyCode.Slash:
                return "/";
            case KeyCode.Colon:
                return ":";
            case KeyCode.Semicolon:
                return ";";
            case KeyCode.Less:
                return "<";
            case KeyCode.Equals:
                return "=";
            case KeyCode.Greater:
                return ">";
            case KeyCode.Question:
                return "?";
            case KeyCode.At:
                return "@";
            case KeyCode.LeftBracket:
                return "[";
            case KeyCode.Backslash:
                return "\\";
            case KeyCode.RightBracket:
                return "]";
            case KeyCode.Caret:
                return "^";
            case KeyCode.Underscore:
                return "_";
            case KeyCode.BackQuote:
                return "`";
            case KeyCode.RightShift:
                return "RShift";
            case KeyCode.LeftShift:
                return "LShift";
            case KeyCode.LeftControl:
                return "LCtrl";
            case KeyCode.RightAlt:
                return "RAlt";
            case KeyCode.LeftAlt:
                return "LAlt";
            case KeyCode.Mouse0:
                return "LeftMouse";
            case KeyCode.Mouse1:
                return "RightMouse";
            case KeyCode.Mouse2:
                return "MidMouse";
            default:
                return key.ToString();
        }
    }

    public static string RemoveTextColorTag(string s)
    {
        int startIndex = s.IndexOf('[');

        if (startIndex == -1)
        {
            return s;
        }

        int endIndex = s.IndexOf(']', startIndex);

        if (endIndex == -1)
        {
            return s;
        }

        return RemoveTextColorTag(s.Remove(startIndex, endIndex - startIndex));
    }

    public static DateTime ServerTimeToLocalTime(uint nTime)
    {
        //server 取的时间是从1970-1-1 8：00开始计时，Datetime从0000-1-1：00：00开始计时
        DateTime UnixEoich = new DateTime(1970, 1, 1, 8, 0, 0);
        //server计时单位是秒，Datetime是纳秒
        long uServerTime = UnixEoich.Ticks + (long)nTime * 10000000;

        return new DateTime(uServerTime);
    }

    public static uint LocalTimeToServerTime(DateTime LocalTime)
    {
        //server 取的时间是从1970-1-1 8：00开始计时，Datetime从0000-1-1：00：00开始计时
        DateTime UnixEoich = new DateTime(1970, 1, 1, 8, 0, 0);

        //server计时单位是秒，Datetime是纳秒
        uint uServerTime = (uint)((LocalTime.Ticks - UnixEoich.Ticks) / 10000000);

        return uServerTime;
    }

    public static long GetFileSize(string filePath)
    {
        if (File.Exists(filePath))
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                return stream.Length;
            }
        }
        return 0;
    }

    public static void ChangePlayerShaderLod(GameObject plyer, int lod)
    {
        int iShaderLod = 200;
        if (lod == 0)
        {
            iShaderLod = 100;
        }
        else if (lod == 1)
        {
            iShaderLod = 200;
        }
        else if (lod == 2)
        {
            iShaderLod = 300;
        }
        else
        {
            iShaderLod = 200;
        }

        Renderer[] skinrender = plyer.GetComponentsInChildren<Renderer>();

        foreach (Renderer r in skinrender)
        {
            if (r.sharedMaterial && r.sharedMaterial.shader)
            {
                // //Debug.Log(r.sharedMaterial.shader.name);
                r.sharedMaterial.shader.maximumLOD = iShaderLod;
            }
            else
            {
                //Debug.Log("no material in avatar");
            }
        }
    }

    /// <summary>
    /// //Debug.Log("FIND CRASH : " + CUtility.GetCodePos());
    /// </summary>
    /// <returns></returns>
    public static string GetCodePos()
    {
        System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(1, true);
        return string.Format("{0}: {1}", st.GetFrame(0).GetFileName(), st.GetFrame(0).GetFileLineNumber());
    }

    //行号
    public static int GetLineNum()
    {
        System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(1, true);
        return st.GetFrame(0).GetFileLineNumber();
    }

    //文件名
    public static string GetFileName()
    {
        System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(1, true);
        return st.GetFrame(0).GetFileName();
    }

    //函数
    public static string GetFuncName()
    {
        System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(1, true);
        return st.GetFrame(0).GetMethod().ToString();
    }

    public static string FormatPlayerName(string strName, int nMinShowCount)
    {
        if (strName == null)
        {
            return "";
        }

        if (strName.Length <= nMinShowCount)
        {
            return strName;
        }
        else
        {
            return strName.Substring(0, nMinShowCount - 1) + "...";
        }
    }

    public static string GetCutString(string inputstring)
    {
        if (inputstring == null)
            return "";
        string subname = inputstring;

        if (CUtility.GetStringVisualLength(subname) > 16)
        {
            while (true)
            {
                subname = inputstring.Substring(0, subname.Length - 1);
                int temp = CUtility.GetStringVisualLength(subname);
                if (temp <= 16)
                {
                    break;
                }
            }
        }
        return subname;
    }

    #region map相关

    public static int TERRAIN_TEX_WIDTH = 256;
    public static int TERRAIN_TEX_HEIGHT = 256;

    public static int GRID_WIDTH = 64;
    public static int GRID_HEIGHT = 32;
    public static int HALF_GRID_WIDTH = 32;
    public static int HALF_GRID_HEIGHT = 16;
    public static float GRID_WIDTH_INVERT = 1 / 64.0f;
    public static float GRID_HEIGHT_INVERT = 1 / 32.0f;

    public static int TILE_COL_GRID_CNT = TERRAIN_TEX_WIDTH / GRID_WIDTH;
    public static int TILE_ROW_GRID_CNT = TERRAIN_TEX_HEIGHT / GRID_HEIGHT;

    public static float PERSPECTIVE_SCALE()
    {
        return (float)GRID_HEIGHT / (float)GRID_WIDTH;
    }

    public static long ROUNDNUM2(long value, long num)
    {
        return (((value) + (num - 1)) & (~(num - 1)));
    }

    #endregion

    public static float Time2Speed(float a, float time_eslasped)
    {
        return ((float)(a) * ((float)(time_eslasped) / 1000.0f));

    }


    public static bool fgreat(float a, float b)
    {
        return (a - b) > -0.001f;
    }

    public static bool fless(float a, float b)
    {
        return (a - b) < +0.001f;
    }

    public static uint xtimeGetTime()
    {
        return (uint)(Time.time * 1000);
    }

    public static byte[] uintToBytes(uint value, byte[] src, int startidx = 0)
    {
        //byte[] src = new byte[4];
        src[startidx + 3] = (byte)((value >> 24) & 0xFF);
        src[startidx + 2] = (byte)((value >> 16) & 0xFF);
        src[startidx + 1] = (byte)((value >> 8) & 0xFF);
        src[startidx] = (byte)(value & 0xFF);
        return src;
    }

#region 数据存储到 list里面

    public static void ushortToBytesList(ushort value, List<byte> src, int startidx = 0)
    {
        src.Add((byte)(value & 0xFF));
        src.Add((byte)((value >> 8) & 0xFF));
    }

    public static void uintToBytesList(uint value, List<byte> src, int startidx = 0)
    {
        src.Add((byte)(value & 0xFF));
        src.Add((byte)((value >> 8) & 0xFF));
        src.Add((byte)((value >> 16) & 0xFF));
        src.Add((byte)((value >> 24) & 0xFF));
    }

    public static void ulongToBytesList(ulong value, List<byte> src, int startidx = 0)
    {
        src.Add((byte)(value & 0xFF));
        src.Add((byte)((value >> 8) & 0xFF));
        src.Add((byte)((value >> 16) & 0xFF));
        src.Add((byte)((value >> 24) & 0xFF));
        src.Add((byte)((value >> 32) & 0xFF));
        src.Add((byte)((value >> 40) & 0xFF));
        src.Add((byte)((value >> 48) & 0xFF));
        src.Add((byte)((value >> 56) & 0xFF));
    }

    public static void BytesToBytesList(byte[] bytes, List<byte> src)
    {
        for (int i = 0; i < bytes.Length; i++)
        {
            src.Add(bytes[i]);
        }
    }

    //public static void CharToBytesList(char[] bytes, List<byte> src)
    //{
    //    for (int i = 0; i < bytes.Length; i++)
    //    {
    //        byte temp = (byte)bytes[i];
    //        if (bytes[i] < 0)
    //        {
    //            temp = Mathf.Abs(bytes[i]) + 127; 
    //        }
    //        src.Add(temp);
    //    }
    //}

#endregion

    /**  
       * 将int数值转换为占四个字节的byte数组，本方法适用于(高位在前，低位在后)的顺序。  和bytesToInt2（）配套使用 
       */
    public static byte[] uintToBytes2(uint value, byte[] src, int startidx = 0)
    {
        src[startidx] = (byte)((value >> 24) & 0xFF);
        src[startidx + 1] = (byte)((value >> 16) & 0xFF);
        src[startidx + 2] = (byte)((value >> 8) & 0xFF);
        src[startidx + 3] = (byte)(value & 0xFF);
        return src;
    }


    public static uint BytesTouint(byte[] src, int startidx = 0)
    {
        uint temp = 0;
        temp = (uint)(src[startidx + 3]) << 24;
        temp += (uint)(src[startidx + 2]) << 16;
        temp += (uint)(src[startidx + 1]) << 8;
        temp += (uint)(src[startidx + 0]);
        return temp;
        //return BitConverter.ToUInt32(src, startidx);
    }

    public static int BytesToint(byte[] src, int startidx = 0)
    {
        return BitConverter.ToInt32(src, startidx);
    }

    public static Int16 BytesToint16(byte[] src, int startidx = 0)
    {
        return BitConverter.ToInt16(src, startidx);
    }

    public static uint BytesTouint(List<byte> src, int startidx = 0)
    {
        uint temp = 0;
        temp = (uint)(src[startidx + 3]) << 24;
        temp += (uint)(src[startidx + 2]) << 16;
        temp += (uint)(src[startidx + 1]) << 8;
        temp += (uint)(src[startidx + 0]);
        return temp;
    }

    public static void BytesToListBytes(List<Byte> des, Byte[] src, int startidx = 0)
    {
        //des.Clear();
        for(int i = startidx; i < src.Length; i++)
        {
            des.Add(src[i]);
        }
    }

    public static void BytesToListBytes(List<Byte> des, int desstartidx, Byte[] src, int srcstartidx = 0)
    {
        for (int i = srcstartidx; i < src.Length; i++)
        {
            des.Insert(desstartidx, src[srcstartidx]);
        }
    }

    //对应以前的writeByte
    public static void UInt8ToBytes(uint value, List<byte> src, int startidx = 0)
    {
        src.Add((byte)(value & 0xFF));
        //src.Add((byte)((value >> 8) & 0xFF));
        //src.Add((byte)((value >> 16) & 0xFF));
        //src.Add((byte)((value >> 24) & 0xFF));
    }

    public static void UInt16ToBytes(uint value, List<byte> src, int startidx = 0)
    {
        src.Add((byte)(value & 0xFF));
        src.Add((byte)((value >> 8) & 0xFF));
        //src.Add((byte)((value >> 16) & 0xFF));
        //src.Add((byte)((value >> 24) & 0xFF));
    }

    public static void UInt32ToBytes(uint value, List<byte> src, int startidx = 0)
    {
        src.Add((byte)(value & 0xFF));
        src.Add((byte)((value >> 8) & 0xFF));
        src.Add((byte)((value >> 16) & 0xFF));
        src.Add((byte)((value >> 24) & 0xFF));
    }
    
    public static void BytesToBytes(byte[] buff, int length, List<byte> data)
    {
        int i = 0;
        for (; i < buff.Length; i++)
        {
            data.Add(buff[i]);
        }
   
        for (; i < length; i++)
        {
            data.Add(0);
        }
    }

    public static string BytesToString(byte[] buff, int start, int length)
    {
        byte[] temp = new byte[length];
        Array.Copy(buff, start, temp, 0, length);
        return TdrBytesToStringDefault(temp);//(buff, start, length);
    }

    public static ushort BytesToUshort(byte[] src, int startidx = 0)
    {
        ushort temp = 0;
        temp += (ushort)((src[startidx + 1]) << 8);
        temp += (ushort)(src[startidx + 0]);
        return temp;
    }

    public static byte[] BytesToBytes(byte[] buff, int start, int length)
    {
        byte[] temp = new byte[length];
        Array.Copy(buff, start, temp, 0, length);
        return temp;
    }

    public static UInt64 BytesToULong(byte[] src, int startidx = 0)
    {
        UInt64 temp = 0;
        temp = (ulong)(src[startidx + 7]) << 56;
        temp += (ulong)(src[startidx + 6]) << 48;
        temp += (ulong)(src[startidx + 5]) << 40;
        temp += (ulong)(src[startidx + 4]) << 32;

        temp += (ulong)(src[startidx + 3]) << 24;
        temp += (ulong)(src[startidx + 2]) << 16;
        temp += (ulong)(src[startidx + 1]) << 8;
        temp += (ulong)(src[startidx + 0]);
        return temp;
    }

    /**
		* 读取一个ip地址
		*/
	public static String readIp(byte[] src, int startidx = 0)
	{
        uint ip1 = (uint)src[startidx + 0];
        uint ip2 = (uint)src[startidx + 1];
        uint ip3 = (uint)src[startidx + 2];
        uint ip4 = (uint)src[startidx + 3];
		return ip1 + "." + ip2 + "." + ip3 + "." + ip4;
	}

#region color
    
    public static uint COLOR_GET_ALPHA(uint x)
    {
        return (x & 0xff000000) >> 24;
    }
    public static uint COLOR_GET_R(uint x)
    {
        return  (x & 0x00ff0000) >> 16;

    }
    public static uint COLOR_GET_G(uint x)
    {
        return (x & 0xff00) >>8;
    }

    public static uint COLOR_GET_B(uint x)
    {
        return x & 0xff;
    }

    public static uint COLOR_ARGB(uint a, uint r, uint g, uint b)
    {
        return ((a & 0xff) << 24) | ((r & 0xff) << 16) | ((g & 0xff) << 8) | (b & 0xff);
    }

    public static Color ConvertColor(uint coloruint)
    {
        uint alpha = COLOR_GET_ALPHA(coloruint); 
        uint r = COLOR_GET_R(coloruint);
        uint g = COLOR_GET_G(coloruint);
        uint b = COLOR_GET_B(coloruint);
        return new Color(r / 255.0f, g / 255.0f, b /255.0f, alpha / 255.0f);
    }
#endregion

    public static Rect EnlargeRect(Rect lrect, Rect rrect)
	{
		Rect rect = new Rect(0, 0, 1, 1);
        if (lrect.width <= 0 || lrect.height <= 0) return rrect;
        if (rrect.width <= 0 || rrect.height <= 0) return lrect;
        rect.xMin = (lrect.xMin < rrect.xMin ? lrect.xMin : rrect.xMin);
        rect.yMin = (lrect.yMin < rrect.yMin ? lrect.yMin : rrect.yMin);
        rect.xMax = (lrect.xMax > rrect.xMax ? lrect.xMax : rrect.xMax);
        rect.yMax = (lrect.yMax > rrect.yMax ? lrect.yMax : rrect.yMax);
		return rect;
	}

    public static bool ByteMemcmp(byte[] byte1, byte[] byte2)
    {
        if (byte1.Length != byte2.Length)
        {
            return true;
        }
        else
        {
            for (int i = 0; i < byte2.Length; i++)
            {
                if (byte2[i] != byte1[i])
                {
                    return true;
                }
            }
        }
        return false;
    }

    static int ilog = 0;
    public static void DebugLog(string temp)
    {

#if DEBUG_LOG
        //Debug.Log(temp);

        if (ilog % 40 == 0)
        {
            TestDownLoad.staticloc = "";
        }
        ilog++;
        TestDownLoad.DebugLog(temp);
#endif
    }

    public static ushort RAND_MAX = 0x7fff;

    public static float RANDOM_OFFSET()
    {
        int rand0 = UnityEngine.Random.Range(0, RAND_MAX);
        int rand1 = UnityEngine.Random.Range(0, RAND_MAX);
        return ((float)rand0 - (float)rand1) / RAND_MAX;
    }

    /// <summary>
    /// 用CreateFromFile方式读取StreamingAssets目录下资源，路径规范如下
    /// </summary>
    /// <returns></returns>
    public static string GetLocalResPath()
    {
        string path = String.Empty;

        #region StreamingAssets目录下-CreateFromFile方式读取的路径格式

        if (Application.platform == RuntimePlatform.Android)
        {
            path = Application.dataPath + "!assets/";          
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            path = Application.dataPath + "/Raw/";             
        }
        else
        {
            path = Application.dataPath + "/StreamingAssets/";      
        }

        #endregion

        return path;
    }

    /// <summary>
    /// 用WWW方式读取StreamingAssets下的跟包资源，路径规范如下
    /// </summary>
    /// <returns></returns>
    public static string GetLocalResPathByWWW()
    {
        string path = String.Empty;

        #region StreamingAssets目录下-WWW的方式读取的路径格式

        if (Application.platform == RuntimePlatform.Android)
        {
            path = "jar:file://" + Application.dataPath + "!/assets/";       
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            path = "file://" + Application.dataPath + "/Raw/";               
        }
        else
        {
            path = "file://" + Application.dataPath + "/StreamingAssets/";        
        }

        #endregion

        return path;
    }

	/// <summary>
	/// 缓存基础路径
	/// </summary>
	public static string CacheBasePath
	{
		get
		{
			#if UNITY_EDITOR
			string ret = Application.dataPath;
			ret = ret.Substring(0, ret.Length - 6) + "LocalCache";
			return ret;
			#else
			return Application.persistentDataPath; 
			#endif
		}
	}

    static public Color ARGBToColor(int val)
    {
        float inv = 1f / 255f;
        Color c = Color.black;
        c.a = inv * ((val >> 24) & 0xFF);
        c.r = inv * ((val >> 16) & 0xFF);
        c.g = inv * ((val >> 8) & 0xFF);
        c.b = inv * (val & 0xFF);
        return c;
    }

    public static Vector3 GetLocalPostion(Camera cam, Vector3 pos, float currentadj)
    {
        Vector3 screenPoint = cam.WorldToScreenPoint(pos);

        float mHalfWidth = Screen.width / 2;
        float mHalfHeight = Screen.height / 2;
        screenPoint.x -= mHalfWidth;
        screenPoint.y -= mHalfHeight;
        float adjustment = currentadj;
        screenPoint.x *= adjustment;
        screenPoint.y *= adjustment;

        return screenPoint;
    }

    /// <summary>
    /// 复制流
    /// </summary>
    /// <param name="input">原始流</param>
    /// <param name="output">目标流</param>
    public static void CopyStream(System.IO.Stream input, System.IO.Stream output)
    {
        byte[] buffer = new byte[2000];
        int len;
        while ((len = input.Read(buffer, 0, 2000)) > 0)
        {
            output.Write(buffer, 0, len);
        }
        output.Flush();
    }

    /// <summary>
    /// 压缩字节数组
    /// </summary>
    /// <param name="sourceByte">需要被压缩的字节数组</param>
    /// <returns>压缩后的字节数组</returns>
    public static byte[] compressBytes(byte[] sourceByte)
    {
        MemoryStream inputStream = new MemoryStream(sourceByte);
        Stream outStream = compressStream(inputStream);
        byte[] outPutByteArray = new byte[outStream.Length];
        outStream.Position = 0;
        outStream.Read(outPutByteArray, 0, outPutByteArray.Length);
        outStream.Close();
        inputStream.Close();
        return outPutByteArray;
    }

    /// <summary>
    /// 解压缩字节数组
    /// </summary>
    /// <param name="sourceByte">需要被解压缩的字节数组</param>
    /// <returns>解压后的字节数组</returns>
    public static byte[] deCompressBytes(byte[] sourceByte)
    {
        MemoryStream inputStream = new MemoryStream(sourceByte);
        Stream outputStream = deCompressStream(inputStream);
        byte[] outputBytes = new byte[outputStream.Length];
        outputStream.Position = 0;
        outputStream.Read(outputBytes, 0, outputBytes.Length);
        outputStream.Close();
        inputStream.Close();
        return outputBytes;
    }

    /// <summary>
    /// 压缩流
    /// </summary>
    /// <param name="sourceStream">需要被压缩的流</param>
    /// <returns>压缩后的流</returns>
    private static Stream compressStream(Stream sourceStream)
    {
        MemoryStream streamOut = new MemoryStream();
        ZOutputStream streamZOut = new ZOutputStream(streamOut, zlibConst.Z_DEFAULT_COMPRESSION);
        CopyStream(sourceStream, streamZOut);
        streamZOut.finish();
        return streamOut;
    }

    /// <summary>
    /// 解压缩流
    /// </summary>
    /// <param name="sourceStream">需要被解压缩的流</param>
    /// <returns>解压后的流</returns>
    private static Stream deCompressStream(Stream sourceStream)
    {
        MemoryStream outStream = new MemoryStream();
        ZOutputStream outZStream = new ZOutputStream(outStream);
        CopyStream(sourceStream, outZStream);
        outZStream.finish();
        return outStream;
    }

    public static string[] GetDirAllFiles(string dir)
    {
        if (!Directory.Exists(dir))
            return null;
        string[] files = Directory.GetFiles(dir);
        return files;
    }

	public static string ArrayToStringWithSeparator<T>(string separator, T[] objs)
	{
		if (null == objs || 0 >= objs.Length)
		{
			return string.Empty;
		}
		var builder = new StringBuilder();
		builder.Append(objs[0]);
		for (int i = 1; i < objs.Length; ++i)
		{
			builder.Append(separator).Append(objs[i]);
		}
		return builder.ToString();
	}
	
	public static string ConnectToStringWithSeparator(string separator, params object[] objs)
	{
		return ArrayToStringWithSeparator(separator, objs);
	}

    public static void CacheLocalFile(string path, byte[] data)
    {
        string dir = Path.GetDirectoryName(path);
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);
        File.WriteAllBytes(path, data);
    }

	public static void CheckFileAndCreate(string path, string fileName)
	{
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}

		if (!File.Exists (path + "/" + fileName)) 
		{
			TextWriter writer = new StreamWriter (path + "/" + fileName);
			writer.Close ();
		}
	}

	public static string ReadCacheData(string path, string fileName)
	{
		string _accountID = "";
		if (!Directory.Exists (path)) 
		{
			Directory.CreateDirectory (path);
		}
		if (!File.Exists (path + "/" + fileName)) 
		{
			TextWriter writer = new StreamWriter (path + "/" + fileName);
			writer.Close ();
		}
		byte[] _data = File.ReadAllBytes (path + "/" + fileName);
		_accountID = TdrBytesToString (_data);
		return _accountID;
	}

	public static void WriteCacheData(string path, string fileName, string dataStr)
	{
		if (!Directory.Exists (path))
		{
			Directory.CreateDirectory (path);
		}
		if (!File.Exists (path + "/" + fileName)) 
		{
			TextWriter writer = new StreamWriter (path + "/" + fileName);
			writer.Close ();
		}
		byte[] data = Encoding.UTF8.GetBytes (dataStr);
		File.WriteAllBytes(path + "/" + fileName, data);
	}
    
    public static List<T> Clone<T>(object List)
    {
        using (Stream objectStream = new MemoryStream())
        {
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(objectStream, List);
            objectStream.Seek(0, SeekOrigin.Begin);
            return formatter.Deserialize(objectStream) as List<T>;
        }
    }
}

public static class PathUnity
{
	public static string Combine(string path1, string path2)
	{
		var path = Path.Combine(path1, path2);
		path = path.Replace(Path.DirectorySeparatorChar, '/');
		return path;
	}
}
