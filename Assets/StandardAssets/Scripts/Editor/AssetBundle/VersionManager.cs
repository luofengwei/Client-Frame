using System.IO;
using UnityEditor;
using UnityEngine;

public class VerNo
{
    public int mAppNo = 0;
    public int mResNo;

    public VerNo(int appNo, int resNo)
    {
        mAppNo = appNo;
        mResNo = resNo;
    }

    public string strVerNo
    {
        get
        {
            return AppHelper.versionChannelTag + "." + mAppNo.ToString() + "." + mResNo.ToString();
        }
    }
}

public class VersionManager
{
    /// 总文件数量
    private static int allFileCount = 0;
    private static int curFileIndex = 0;

    /// 文件的后缀名
    private static string fileExt = "";
    /// 文件名
    private static string curFileName = "";
    /// 输出文件的名字
    private static string outFileName = "";
    public static void CopyDir(string srcPath, string aimPath)
    {
        try
        {
            // 检查目标目录是否以目录分割字符结束如果不是则添加之
            if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
                aimPath += Path.DirectorySeparatorChar;
            // 判断目标目录是否存在如果不存在则新建之
            if (!Directory.Exists(aimPath))
                Directory.CreateDirectory(aimPath);
            // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
            // 如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法
            // string[] fileList = Directory.GetFiles(srcPath);
            string[] fileList = Directory.GetFileSystemEntries(srcPath);
            // 遍历所有的文件和目录
            foreach (string file in fileList)
            {
                // 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                if (Directory.Exists(file))
                    CopyDir(file, aimPath + Path.GetFileName(file));
                // 否则直接Copy文件
                else
                {
                    fileExt = Path.GetExtension(file);
                    curFileName = Path.GetFileName(file);
                    if (fileExt.Equals(".manifest"))
                        continue;

                    outFileName = aimPath + curFileName;
                    if (fileExt.Equals(ABPathHelper.Unity3dSuffix))
                    {
                        string fileName = file.Replace("\\", "/");
                        byte[] fileData = null;
                        using (FileStream fs = new FileStream(file, FileMode.Open))
                        {
                            fileData = new byte[fs.Length];
                            fs.Read(fileData, 0, (int)fs.Length);
                        }

                        ABHelper.CacheCompressFile(outFileName, fileData);

                        curFileIndex++;
                        ShowProgress();
                    }
                    else
                    {
                        File.Copy(file, outFileName, true);
                    }
                }
            }
        }
        catch
        {
            Debug.LogError("无法复制!" + srcPath + "  到   " + aimPath);
        }
    }
    
    /// <summary>
    /// 显示压缩进度
    /// </summary>
    private static void ShowProgress()
    {
        if (allFileCount != 0)
        {
            EditorUtility.DisplayProgressBar("请喝杯咖啡稍等片刻，文件正在压缩比对中..", string.Format("FileName: {0}", curFileName), (float)(curFileIndex) / allFileCount);
        }
    }
}
