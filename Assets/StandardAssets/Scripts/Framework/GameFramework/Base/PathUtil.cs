using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PathUtil
{
    //根据指定的Assets下的文件夹路径 返回这个路径下的指定后缀所有文件全路径
	public static void GetAllFilesWithSuffix(string path, string suffix, ref List<string> nameObjs)
    {
        try
        {
            //返回指定的目录中文件和子目录的名称的数组或空数组
            string[] directoryEntries = Directory.GetFileSystemEntries(path);

            for (int i = 0; i < directoryEntries.Length; ++i)
            {
                string p = directoryEntries[i];
                if (p.EndsWith(".meta"))
                    continue;
                string[] pathSplit = p.Split('.');
                //文件
                if (pathSplit.Length > 1)
                {
                    if (p.EndsWith(suffix))
						nameObjs.Add(p);
                }
                else
                {
                    GetAllFilesWithSuffix(pathSplit[0] + ABPathHelper.Separator, suffix, ref nameObjs);
                    continue;
                }
            }
        }
        catch (DirectoryNotFoundException)
        {
            //Debug.LogError(path + "路径不存在");
        }
    }

    public static bool ChangeFileNameExtension(string path, string newExt,out string newPath)
    {
        newPath = path;
        if (path == null || path == string.Empty) return false;
        if (newExt == null || newExt == string.Empty) return false;

        try
        {
            string suffixPath = Path.GetDirectoryName(path);
            string fileName = Path.GetFileNameWithoutExtension(path);

            if (fileName == string.Empty) return false;
            newPath = Path.Combine(suffixPath,fileName) + "." + newExt;
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
  
    public static void GetAllDirectories(string path, ref List<string> files)
    {
        if (Directory.Exists(path))
        {           
            path = path.Replace("\\", "/");
            files.Add(path);
            string[] dirs = Directory.GetDirectories(path);
            if (dirs == null || dirs.Length == 0) return;
            for (int i = 0; i < dirs.Length; ++i)
            {
                GetAllDirectories(dirs[i], ref files);
            }            
        }       
    }

    public static void GetAllFiles(string path, ref List<string> files)
    {
        if (Directory.Exists(path))
        {
            string[] allFiles = Directory.GetFiles(path);
            if (allFiles != null)
            {
                for (int i = 0; i < allFiles.Length; ++i)
                {
                    string p = allFiles[i];
                    if (p.EndsWith(".meta"))
                        continue;
                    p = p.Replace("\\", "/");
                    p = p.ToLower();
                    files.Add(p);
                }
            }
            string[] dirs = Directory.GetDirectories(path);
            if (dirs == null || dirs.Length == 0) return;
            for (int i = 0; i < dirs.Length; ++i)
            {
                GetAllFiles(dirs[i], ref files);
            }              
        }      
    }

    public static void GetAllFiles(string path, string suffix, ref List<string> files)
    {
        if (Directory.Exists(path))
        {
            string[] allFiles = Directory.GetFiles(path, suffix, SearchOption.AllDirectories); //"*.*" "*.unity3d"
            if (allFiles != null)
            {
                for (int i = 0; i < allFiles.Length; ++i)
                {
                    string p = allFiles[i];
                    p = p.Replace("\\", "/");
                    //p = p.ToLower();
                    files.Add(p);
                }
            }
        }
    }
  
    public static void CreateDirectory(string fileName)
    {
        string strFolder = Path.GetDirectoryName(fileName);
        if (!Directory.Exists(strFolder))
            Directory.CreateDirectory(strFolder);
    }  

    public static void DeleteFile(string fileName)
    {
        if (File.Exists(fileName))
            File.Delete(fileName);
    }

    public static void DeleteSubDirectory(string path)
    {
        if (Directory.Exists(path))
        {            
            string[] dirs = Directory.GetDirectories(path);
            if (dirs == null || dirs.Length == 0) 
                return;
            for (int i = 0; i < dirs.Length; ++i)
            {
                PathUtil.ReCreatePath(dirs[i]);               
            }
        }       
    }

    public static void ReCreatePath(string path)
    {
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
        Directory.CreateDirectory(path);
    }

    public static void DeleteFiles(string path, string suffix)
    {
        if (Directory.Exists(path))
        {
            string[] allFiles = Directory.GetFiles(path, suffix, SearchOption.AllDirectories);
            if (allFiles != null)
            {
                for (int i = 0; i < allFiles.Length; ++i)
                {
                    File.Delete(allFiles[i]);
                }
            }
        }
    }

    public static void CopyFiles(string srcPath, string tgtPath, string suffix)
    {
        List<string> files = new List<string>();
        PathUtil.GetAllFiles(srcPath, suffix, ref files);
        for (int i = 0; i < files.Count; ++i)
        {
            string fileName = files[i];
            fileName = fileName.Replace(srcPath, tgtPath);
            File.Copy(files[i], fileName, true);	
        }
    }

    public static void CopyFile(string srcPath, string tgtPath, string fileName)
    {    
        string srcFileName = srcPath;
        srcFileName += ABPathHelper.Separator;
        srcFileName += fileName;

        string tgtFileName = tgtPath;
        tgtFileName += ABPathHelper.Separator;
        tgtFileName += fileName;
        CreateDirectory(tgtFileName);
        File.Copy(srcFileName, tgtFileName, true);
    }

    public static void CopyDirectory(string sourcePath, string destinationPath)
    {
        DirectoryInfo info = new DirectoryInfo(sourcePath);
        Directory.CreateDirectory(destinationPath);
        foreach (FileSystemInfo fsi in info.GetFileSystemInfos())
        {
            string destName = Path.Combine(destinationPath, fsi.Name);
            if (fsi is FileInfo)
                File.Copy(fsi.FullName, destName);
            else
            {
                Directory.CreateDirectory(destName);
                CopyDirectory(fsi.FullName, destName);
            }
        }
    }
}
