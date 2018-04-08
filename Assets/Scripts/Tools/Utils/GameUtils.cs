using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Text.RegularExpressions;

public class GameUtils
{
    public static bool OnlyShowInInner = false;  //只在内部玩家(登录账号白名单or点出debug按钮的手机)下显示的控制

    [DoNotToLua]
    public static string ForceUpdateUrl = "http://www.baidu.com"; //当前强制更新的地址
    [DoNotToLua]
    public static bool ScriptLoad = false;  //当前脚本的运行模式  
    [DoNotToLua]
    public static bool UseSelfBundle = false;   //当前使用自己生成的bundle运行
    [DoNotToLua]
    public static bool IsForceUpdate = false;//当前是否是强制更新   
    [DoNotToLua]
    public static bool UseAsynSolve = true;  //使用异步加载
    [DoNotToLua]
    public static bool UseReporter = true;   //使用Reporter   

    //拼接地址
    [DoNotToLua]
    public static string GetCombinePath(string path1, string path2, params string[] path3)
    {
        StringBuilder back = new StringBuilder();
        back.Append(path1);
        back.Append(ABPathHelper.Separator);
        back.Append(path2);
        if (path3 != null)
        {
            for (int i = 0; i < path3.Length; ++i)
            {
                back.Append(ABPathHelper.Separator);
                back.Append(path3[i]);
            }
        }
        return back.ToString();
    }

    [DoNotToLua]
    public static void ShowMsg(string msg)
    {
        SLua.LuaFunction luaFunc = MyLuaSrv.Instance.GetFunction("BridgingClass.ShowMsg");
        if (null != luaFunc)
        {
            luaFunc.call(msg);
        }
    }
    //检测玩家输入的名字是否正确 
    //名字只接受数字、英文字母、中文汉字、下划线，其他字符不接受
    public static bool isRightName(string name)
    {
        Regex reg = new Regex(@"^[a-zA-Z0-9_\u4e00-\u9fa5]+$");
        Match m = reg.Match(name);
        if (m.Success)
            return true;
        else
            return false;
    }

    /// <summary>
    /// 判断输入的字符串是否只包含数字和英文字母
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool IsNumAndEn(string input)
    {
        string pattern = @"^[A-Za-z0-9]+$";
        Regex regex = new Regex(pattern);
        return regex.IsMatch(input);
    }
}
