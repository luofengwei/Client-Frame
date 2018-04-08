using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Security.Cryptography;

class AES
{
    public const string strEncriptKey = "kuaidianlaidaoxindongwangluolanqiu";
    public const string strIv = "Rkb4jvUy/ye7Cd7k89QQgQ==";

    /// <summary>
    /// AES 加密(高级加密标准，是下一代的加密算法标准，速度快，安全级别高，目前 AES 标准的一个实现是 Rijndael 算法)
    /// </summary>
    /// <param name="EncryptString">待加密密文</param>
    /// <param name="EncryptKey">加密密钥</param>
    /// <returns></returns>
    public static byte[] AESEncrypt(byte[] EncryptedData, string EncryptKey)
    {
        if (EncryptedData == null || EncryptedData.Length <= 0) { throw (new Exception("密文不得为空")); }
        if (string.IsNullOrEmpty(EncryptKey)) { throw (new Exception("密钥不得为空")); }

        byte[] plainBytes = Encoding.Default.GetBytes(EncryptKey);
        Byte[] bKey = new Byte[16];
        Array.Copy(Encoding.Default.GetBytes(EncryptKey.PadRight(bKey.Length)), bKey, bKey.Length);
            
        string m_strEncrypt = string.Empty;
        byte[] m_btIV = Convert.FromBase64String(strIv);
        Rijndael m_AESProvider = Rijndael.Create();
        try
        {
            byte[] m_btEncryptString = Encoding.Default.GetBytes(Convert.ToBase64String(EncryptedData));
            MemoryStream m_stream = new MemoryStream();
            CryptoStream m_csstream = new CryptoStream(m_stream, m_AESProvider.CreateEncryptor(bKey, m_btIV), CryptoStreamMode.Write);
            m_csstream.Write(m_btEncryptString, 0, m_btEncryptString.Length);
            m_csstream.FlushFinalBlock();
            m_strEncrypt = Convert.ToBase64String(m_stream.ToArray());
            m_stream.Close(); m_stream.Dispose();
            m_csstream.Close(); m_csstream.Dispose();
        }
        catch (IOException ex) { throw ex; }
        catch (CryptographicException ex) { throw ex; }
        catch (ArgumentException ex) { throw ex; }
        catch (Exception ex) { throw ex; }
        finally { m_AESProvider.Clear(); }
        return Convert.FromBase64String(m_strEncrypt);
    }

    /// <summary>
    /// AES 解密(高级加密标准，是下一代的加密算法标准，速度快，安全级别高，目前 AES 标准的一个实现是 Rijndael 算法)
    /// </summary>
    /// <param name="DecryptString">待解密密文</param>
    /// <param name="DecryptKey">解密密钥</param>
    /// <returns></returns>
    public static byte[] AESDecrypt(byte[] DecryptedData, string DecryptKey)
    {
        if (DecryptedData == null || DecryptedData.Length <= 0) { throw (new Exception("密文不得为空")); }
        if (string.IsNullOrEmpty(DecryptKey)) { throw (new Exception("密钥不得为空")); }

        byte[] plainBytes = Encoding.Default.GetBytes(DecryptKey);
        Byte[] bKey = new Byte[16];
        Array.Copy(Encoding.Default.GetBytes(DecryptKey.PadRight(bKey.Length)), bKey, bKey.Length);
     
        string m_strDecrypt = string.Empty;
        byte[] m_btIV = Convert.FromBase64String(strIv);
        Rijndael m_AESProvider = Rijndael.Create();
        try
        {
            byte[] m_btDecryptString = Convert.FromBase64String(Convert.ToBase64String(DecryptedData));
            MemoryStream m_stream = new MemoryStream();
            CryptoStream m_csstream = new CryptoStream(m_stream, m_AESProvider.CreateDecryptor(bKey, m_btIV), CryptoStreamMode.Write);
            m_csstream.Write(m_btDecryptString, 0, m_btDecryptString.Length); 
            m_csstream.FlushFinalBlock();
            m_strDecrypt = Encoding.Default.GetString(m_stream.ToArray());
            m_stream.Close(); m_stream.Dispose();
            m_csstream.Close(); m_csstream.Dispose();
        }
        catch (IOException ex) { throw ex; }
        catch (CryptographicException ex) { throw ex; }
        catch (ArgumentException ex) { throw ex; }
        catch (Exception ex) { throw ex; }
        finally { m_AESProvider.Clear(); }
        return Convert.FromBase64String(m_strDecrypt);
    }

    public static byte[] AESEncrypt1(byte[] EncryptedData, string EncryptKey)
    {
        if (EncryptedData == null || EncryptedData.Length <= 0) { throw (new Exception("密文不得为空")); }
        if (string.IsNullOrEmpty(EncryptKey)) { throw (new Exception("密钥不得为空")); }

        byte[] plainBytes = Encoding.Default.GetBytes(EncryptKey);
        Byte[] bKey = new Byte[16];
        Array.Copy(Encoding.Default.GetBytes(EncryptKey.PadRight(bKey.Length)), bKey, bKey.Length);

        //string m_strEncrypt = string.Empty;
        byte[] EncryptData = null;
        byte[] m_btIV = Convert.FromBase64String(strIv);
        Rijndael m_AESProvider = Rijndael.Create();
        try
        {
            //byte[] m_btEncryptString = Encoding.Default.GetBytes(Convert.ToBase64String(EncryptedData));
            MemoryStream m_stream = new MemoryStream();
            CryptoStream m_csstream = new CryptoStream(m_stream, m_AESProvider.CreateEncryptor(bKey, m_btIV), CryptoStreamMode.Write);
            m_csstream.Write(EncryptedData, 0, EncryptedData.Length);
            m_csstream.FlushFinalBlock();
            EncryptData = m_stream.ToArray();
            m_stream.Close(); m_stream.Dispose();
            m_csstream.Close(); m_csstream.Dispose();
        }
        catch (IOException ex) { throw ex; }
        catch (CryptographicException ex) { throw ex; }
        catch (ArgumentException ex) { throw ex; }
        catch (Exception ex) { throw ex; }
        finally { m_AESProvider.Clear(); }
        return EncryptData;
    }
   
    public static byte[] AESDecrypt1(byte[] DecryptedData, string DecryptKey)
    {
        if (DecryptedData == null || DecryptedData.Length <= 0) { throw (new Exception("密文不得为空")); }
        if (string.IsNullOrEmpty(DecryptKey)) { throw (new Exception("密钥不得为空")); }

        byte[] plainBytes = Encoding.Default.GetBytes(DecryptKey);
        Byte[] bKey = new Byte[16];
        Array.Copy(Encoding.Default.GetBytes(DecryptKey.PadRight(bKey.Length)), bKey, bKey.Length);

        //string m_strDecrypt = string.Empty;
        byte[] DecryptData = null;
        byte[] m_btIV = Convert.FromBase64String(strIv);
        Rijndael m_AESProvider = Rijndael.Create();
        try
        {
            //byte[] m_btDecryptString = Convert.FromBase64String(Convert.ToBase64String(DecryptedData));
            MemoryStream m_stream = new MemoryStream();
            CryptoStream m_csstream = new CryptoStream(m_stream, m_AESProvider.CreateDecryptor(bKey, m_btIV), CryptoStreamMode.Write);
            m_csstream.Write(DecryptedData, 0, DecryptedData.Length); 
            m_csstream.FlushFinalBlock();
            DecryptData = m_stream.ToArray();
            m_stream.Close(); m_stream.Dispose();
            m_csstream.Close(); m_csstream.Dispose();
        }
        catch (IOException ex) { throw ex; }
        catch (CryptographicException ex) { throw ex; }
        catch (ArgumentException ex) { throw ex; }
        catch (Exception ex) { throw ex; }
        finally { m_AESProvider.Clear(); }
        return DecryptData;
    }

    public static byte[] AESEncrypt2(byte[] EncryptedData, String EncryptKey)
    {
        if (EncryptedData == null || EncryptedData.Length <= 0) { throw (new Exception("密文不得为空")); }
        if (string.IsNullOrEmpty(EncryptKey)) { throw (new Exception("密钥不得为空")); }

        Byte[] bKey = new Byte[16];
        Array.Copy(Encoding.UTF8.GetBytes(EncryptKey.PadRight(bKey.Length)), bKey, bKey.Length);

        //MemoryStream mStream = new MemoryStream();
        RijndaelManaged aes = new RijndaelManaged();
        aes.Mode = CipherMode.ECB;
        //aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        aes.KeySize = 128;
        //aes.BlockSize = 128;
        aes.Key = bKey;
        //aes.IV = Convert.FromBase64String(strIv); 
        ICryptoTransform cTransform = aes.CreateEncryptor();
        //byte[] plainText = Encoding.UTF8.GetBytes(EncryptedData);
        byte[] cipherBytes = cTransform.TransformFinalBlock(EncryptedData, 0, EncryptedData.Length);
        //return Convert.ToBase64String(cipherBytes);
        return cipherBytes;
        //CryptoStream cryptoStream = new CryptoStream(mStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
        //try
        //{
        //    cryptoStream.Write(EncryptedData, 0, EncryptedData.Length);
        //    cryptoStream.FlushFinalBlock();
        //    return mStream.ToArray();
        //}
        //finally
        //{
        //    cryptoStream.Close();
        //    mStream.Close();
        //    aes.Clear();
        //}
    }

    public static byte[] AESDecrypt2(byte[] DecryptedData, String DecryptKey)
    {
        if (DecryptedData == null || DecryptedData.Length <= 0) { throw (new Exception("密文不得为空")); }
        if (string.IsNullOrEmpty(DecryptKey)) { throw (new Exception("密钥不得为空")); }

        Byte[] bKey = new Byte[16];
        Array.Copy(Encoding.UTF8.GetBytes(DecryptKey.PadRight(bKey.Length)), bKey, bKey.Length);

        MemoryStream mStream = new MemoryStream();
        RijndaelManaged aes = new RijndaelManaged();
        aes.Mode = CipherMode.ECB;
        //aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        aes.KeySize = 128;
        //aes.BlockSize = 128;
        aes.Key = bKey;
        //aes.IV = Convert.FromBase64String(strIv); 

        CryptoStream cryptoStream = new CryptoStream(mStream, aes.CreateDecryptor(), CryptoStreamMode.Write);
        try
        {
            //byte[] tmp = new byte[DecryptedData.Length + 32];
            //int len = cryptoStream.Read(tmp, 0, DecryptedData.Length + 32);
            //byte[] ret = new byte[len];
            //Array.Copy(tmp, 0, ret, 0, len);
            //return ret;

            cryptoStream.Write(DecryptedData, 0, DecryptedData.Length);
            cryptoStream.FlushFinalBlock();
            return mStream.ToArray();           
        }
        finally
        {
            cryptoStream.Close();
            mStream.Close();
            aes.Clear();
        }
    }
}