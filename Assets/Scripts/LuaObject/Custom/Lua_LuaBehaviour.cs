using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_LuaBehaviour : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Init(IntPtr l) {
		try {
			LuaBehaviour self=(LuaBehaviour)checkSelf(l);
			self.Init();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int setBehaviour(IntPtr l) {
		try {
			LuaBehaviour self=(LuaBehaviour)checkSelf(l);
			SLua.LuaTable a1;
			checkType(l,2,out a1);
			self.setBehaviour(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CallLuaMethod(IntPtr l) {
		try {
			LuaBehaviour self=(LuaBehaviour)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Object[] a2;
			checkParams(l,3,out a2);
			var ret=self.CallLuaMethod(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_LuaFile(IntPtr l) {
		try {
			LuaBehaviour self=(LuaBehaviour)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.LuaFile);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_LuaFile(IntPtr l) {
		try {
			LuaBehaviour self=(LuaBehaviour)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.LuaFile=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_LuaClassName(IntPtr l) {
		try {
			LuaBehaviour self=(LuaBehaviour)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.LuaClassName);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_worker(IntPtr l) {
		try {
			LuaBehaviour self=(LuaBehaviour)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.worker);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"LuaBehaviour");
		addMember(l,Init);
		addMember(l,setBehaviour);
		addMember(l,CallLuaMethod);
		addMember(l,"LuaFile",get_LuaFile,set_LuaFile,true);
		addMember(l,"LuaClassName",get_LuaClassName,null,true);
		addMember(l,"worker",get_worker,null,true);
		createTypeMetatable(l,null, typeof(LuaBehaviour),typeof(FLEventDispatcherMono));
	}
}
