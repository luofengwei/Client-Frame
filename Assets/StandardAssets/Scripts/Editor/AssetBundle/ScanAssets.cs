using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.IO;

//扫描资源
public class ScanAssets
{
    //扫描依赖文件                       文件夹            标记符号队列                忽略标记队列          回调方法
    public static void ScanDependencies(string folder, List<string> extensionsLimit, List<string> excepts, Action<string> call)
    {
        ScanDependenciesWithSearchPattern(folder, extensionsLimit, excepts, "*", call);
    }

    //扫描多文件依赖                        文件列表             标记符号队列            是否是自己               回调方法
    public static void ScanDependciesByFiles(List<string> files, List<string> extensions, bool includeSelf, Action<string> call)
    {
        try
        {           
            for (int i = 0; i < files.Count; i++)
                files[i] = RetrimPath(files[i]);              
            string[] depends = AssetBundleDependency.GetDependenciesWithoutExtensions(files, extensions, includeSelf);
            if (call != null)
            {
                int Num = depends.Length;
                for (int i = 0; i < Num; i++)
                {
                    EditorUtility.DisplayProgressBar("Scanning Depends", string.Format("Scanned: {0}", depends[i]), (float)(i) / Num);
                    call(depends[i]);
                }
            }
            depends = null;
        }
        catch (System.Exception e)
        {
            throw e;
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }
    }

    public static void ScanDependenciesWithSearchPattern(string folder, List<string> extensionsLimit, List<string> excepts, string pattern = "*", Action<string> call = null)
    {
        try
        {
            if (folder.Contains("Assets/"))
                folder = folder.Replace("Assets/", "");
            folder = Path.Combine(Application.dataPath, folder);
            string[] files = null;
            if (string.IsNullOrEmpty(Path.GetExtension(folder)) == false)
                files = new string[] { folder };
            else
            {
                //获取所有筛选出来的文件列表
                files = Array.FindAll(Directory.GetFiles(folder, pattern, SearchOption.AllDirectories), (p) =>
                {
                    if (excepts != null)
                    {
                        for (int i = 0; i < excepts.Count; i++)
                        {
                            if (p.Contains(excepts[i]))
                                return false;
                        }
                    }
                    string extension = Path.GetExtension(p);
                    if (!string.IsNullOrEmpty(extension) && !(extension.Equals(".meta")))
                    {
                        if (extensionsLimit != null)
                            return extensionsLimit.Contains(extension);
                        return true;
                    }
                    return false;
                });
            }
            for (int i = 0; i < files.Length; ++i)
                files[i] = RetrimPath(files[i]);              
            if (call != null)
            {                 
                for (int i = 0; i < files.Length; ++i)
                {
                    EditorUtility.DisplayProgressBar("进度显示", string.Format("文件名: {0}", files[i]), (float)(i) / files.Length);
                    call(files[i]);
                }
            }
            files = null;
        }
        catch (System.Exception e)
        {
            throw e;
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }           
    }

    //重新整理路径
    static string RetrimPath(string path)
    {
        return path.Replace(Application.dataPath, "Assets");
    }
}
