using UnityEngine;
using GameFramework;
using System.Collections;
using SLua;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

[CustomLuaClassAttribute]
public class MyLuaSrv : SingletonO<MyLuaSrv>
{
	private LuaSvr _origin = null;
    private static bool printWithStack = false;
	private static Func<string, string> processFile;
	private static HashSet<string> ms_includedFiles = new HashSet<string> ();
	
//	[DllImport(PB, CallingConvention = CallingConvention.Cdecl)]
//	public static extern int luaopen_pb (IntPtr luaState);

//	[DllImport(PB, CallingConvention = CallingConvention.Cdecl)]
//	public static extern int luaopen_snapshot (IntPtr luaState);

//	[DllImport(PB, CallingConvention = CallingConvention.Cdecl)]
//	public static extern int luaopen_bitLib (IntPtr luaState);

	public LuaState luaState {
		get{ return _origin.luaState;}
	}

	public LuaSvr origin {
		get {
			return _origin;
		}
	}

	
	public static bool EnablePrint =
	#if UNITY_EDITOR || UNITY_EDITOR_OSX || UNITY_EDITOR_WIN
		true;
	#else
		false;
	#endif

	public void Dispose ()
	{
        //Debug.Log("<color=yellow> MyLuaSrv.Dispose \n" + "</color>");
        processFile = null;
        if (ms_includedFiles != null)
        {
            ms_includedFiles.Clear();
            ms_includedFiles = null;
        }
		if (_origin != null)
        {
            _origin.Dispose();
			_origin = null;
        }
        _instance = null;
        
        LuaLoadOverrider.Instance.Dispose();
	}
    
    public void GC()
    {
        if (_origin != null)
        {
            _origin.GC();
        }
    }
	
	public MyLuaSrv ()
	{
		_origin = new LuaSvr ();
		_origin.init (null, () => {
//			MyLuaSrv.luaopen_pb (_origin.luaState.L);
//			MyLuaSrv.luaopen_snapshot (_origin.luaState.L);
//			MyLuaSrv.luaopen_bitLib (_origin.luaState.L);
			LuaDLL.luaS_openextlibs(_origin.luaState.L);
			LuaDLL.lua_settop (_origin.luaState.L, 0);
			
			LuaDLL.lua_pushcfunction (_origin.luaState.L, import);
			LuaDLL.lua_setglobal (_origin.luaState.L, "using");
			
			LuaDLL.lua_pushcfunction (_origin.luaState.L, printStack);
			LuaDLL.lua_setglobal (_origin.luaState.L, "stack");

			LuaDLL.lua_pushcfunction (_origin.luaState.L, print);
			LuaDLL.lua_setglobal (_origin.luaState.L, "print");

			LuaDLL.lua_pushcfunction (_origin.luaState.L, buglyError);
			LuaDLL.lua_setglobal (_origin.luaState.L, "buglyError");
		});
	}

	public void SetGlobalImportFunction (Func<string,string> load)
	{
		processFile = load;
//		LuaDLL.lua_pushcfunction (_origin.luaState.L, autoImport);
//		LuaDLL.lua_setglobal (_origin.luaState.L, "autoImport");
	}

	public object this [string path] {
		get {
			return _origin.luaState.getObject (path);
		}
		set {
			_origin.luaState.setObject (path, value);
		}
	}

	public object DoFile (string fn)
	{
		return _origin.luaState.doFile (fn); 
	}

	public object DoString (string content)
	{
		return _origin.luaState.doString (content);
	}

	public LuaFunction GetFunction (string fn)
	{
		return _origin.luaState.getFunction (fn);
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	internal static int buglyError (IntPtr L)
	{
		LuaDLL.lua_getglobal (L, "debug");
		LuaDLL.lua_getfield (L, -1, "traceback");
		LuaDLL.lua_pushvalue (L, 1);
		LuaDLL.lua_pushnumber (L, 2);
		LuaDLL.lua_call (L, 2, 1);
		LuaDLL.lua_remove (L, -2);
		string error = LuaDLL.lua_tostring (L, -1);
        SLua.Logger.Log (error);
		LuaDLL.lua_pop (L, 1);
		
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	internal static int print (IntPtr L)
	{
		if (EnablePrint) {
            if (printWithStack)
            {
				return printStack (L);
			} 
            else
				return SLua.LuaState.print (L);
		}
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	internal static int printStack (IntPtr L)
	{
		if (EnablePrint) {
			LuaDLL.lua_getglobal (L, "debug");
			LuaDLL.lua_getfield (L, -1, "traceback");
			LuaDLL.lua_pushvalue (L, 1);
			LuaDLL.lua_pushnumber (L, 2);
			LuaDLL.lua_call (L, 2, 1);
			LuaDLL.lua_remove (L, -2);
			SLua.Logger.Log (LuaDLL.lua_tostring (L, -1));
			LuaDLL.lua_pop (L, 1);
		}
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	internal static int import (IntPtr l)
	{
		LuaDLL.luaL_checktype (l, 1, LuaTypes.LUA_TSTRING);
		string str = LuaDLL.lua_tostring (l, 1);
		if (ms_includedFiles.Contains (str)) {
			return 0;
		} else {
			ms_includedFiles.Add (str);
		}
		return LuaState.import (l);
	}

	public static string GetFullPath (string lua)
	{
		if (processFile != null)
			lua = processFile (lua);
		return lua;
	}

	public int LuaStateMemory {
		get {
			if (luaState != null) {
				return LuaDLL.lua_gc (luaState.L, LuaGCOptions.LUA_GCCOUNT, 0);
			}
			return -1;
		}
	}

	public void LuaManualGC ()
	{
		if (luaState != null) {
			LuaDLL.lua_gc (luaState.L, LuaGCOptions.LUA_GCCOLLECT, 0);
		}
	}

    public LuaTable GetTable(string fn)
    {
        return _origin.luaState.getTable(fn);
    }

    #region 有关游戏内特殊的lua函数调用相关
    public object CallRecvMsg(LuaFunction func,int arg1,int arg2,byte[] data,int length)
    {
        var state = _origin.luaState;
        int error = LuaObject.pushTry(state.L);

        LuaObject.pushEnum(state.L, arg1);
        LuaObject.pushEnum(state.L, arg2);
        LuaDLL.lua_pushlstring(state.L, data, length);

        bool ret = func.pcall(3, error);
        LuaDLL.lua_remove(state.L, error);

        if (ret)
            return state.topObjects(error - 1);
        return null;
    }
    #endregion

    //
    //	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    //	internal static int autoImport (IntPtr l)
    //	{
    //		LuaDLL.luaL_checktype (l, 1, LuaTypes.LUA_TSTRING);
    //		string str = LuaDLL.lua_tostring (l, 1);
    //		if (processFile != null)
    //			str = processFile (str);
    //		if (ms_includedFiles.Contains (str)) {
    //			return 0;
    //		} else {
    //			ms_includedFiles.Add (str);
    //		}
    //		LuaDLL.lua_remove(l,1);
    //		LuaDLL.lua_pushstring(l,str);
    //		return LuaState.loader (l);
    //	}
}
