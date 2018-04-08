using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_MyLuaSrv : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int constructor(IntPtr l) {
		try {
			MyLuaSrv o;
			o=new MyLuaSrv();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Dispose(IntPtr l) {
		try {
			MyLuaSrv self=(MyLuaSrv)checkSelf(l);
			self.Dispose();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GC(IntPtr l) {
		try {
			MyLuaSrv self=(MyLuaSrv)checkSelf(l);
			self.GC();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetGlobalImportFunction(IntPtr l) {
		try {
			MyLuaSrv self=(MyLuaSrv)checkSelf(l);
			System.Func<System.String,System.String> a1;
			LuaDelegation.checkDelegate(l,2,out a1);
			self.SetGlobalImportFunction(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int DoFile(IntPtr l) {
		try {
			MyLuaSrv self=(MyLuaSrv)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.DoFile(a1);
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
	static public int DoString(IntPtr l) {
		try {
			MyLuaSrv self=(MyLuaSrv)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.DoString(a1);
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
	static public int GetFunction(IntPtr l) {
		try {
			MyLuaSrv self=(MyLuaSrv)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.GetFunction(a1);
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
	static public int LuaManualGC(IntPtr l) {
		try {
			MyLuaSrv self=(MyLuaSrv)checkSelf(l);
			self.LuaManualGC();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetTable(IntPtr l) {
		try {
			MyLuaSrv self=(MyLuaSrv)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.GetTable(a1);
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
	static public int CallRecvMsg(IntPtr l) {
		try {
			MyLuaSrv self=(MyLuaSrv)checkSelf(l);
			SLua.LuaFunction a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			System.Byte[] a4;
			checkArray(l,5,out a4);
			System.Int32 a5;
			checkType(l,6,out a5);
			var ret=self.CallRecvMsg(a1,a2,a3,a4,a5);
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
	static public int GetFullPath_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=MyLuaSrv.GetFullPath(a1);
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
	static public int get_EnablePrint(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,MyLuaSrv.EnablePrint);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_EnablePrint(IntPtr l) {
		try {
			System.Boolean v;
			checkType(l,2,out v);
			MyLuaSrv.EnablePrint=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_luaState(IntPtr l) {
		try {
			MyLuaSrv self=(MyLuaSrv)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.luaState);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_origin(IntPtr l) {
		try {
			MyLuaSrv self=(MyLuaSrv)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.origin);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_LuaStateMemory(IntPtr l) {
		try {
			MyLuaSrv self=(MyLuaSrv)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.LuaStateMemory);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int getItem(IntPtr l) {
		try {
			MyLuaSrv self=(MyLuaSrv)checkSelf(l);
			string v;
			checkType(l,2,out v);
			var ret = self[v];
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
	static public int setItem(IntPtr l) {
		try {
			MyLuaSrv self=(MyLuaSrv)checkSelf(l);
			string v;
			checkType(l,2,out v);
			System.Object c;
			checkType(l,3,out c);
			self[v]=c;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"MyLuaSrv");
		addMember(l,Dispose);
		addMember(l,GC);
		addMember(l,SetGlobalImportFunction);
		addMember(l,DoFile);
		addMember(l,DoString);
		addMember(l,GetFunction);
		addMember(l,LuaManualGC);
		addMember(l,GetTable);
		addMember(l,CallRecvMsg);
		addMember(l,GetFullPath_s);
		addMember(l,getItem);
		addMember(l,setItem);
		addMember(l,"EnablePrint",get_EnablePrint,set_EnablePrint,false);
		addMember(l,"luaState",get_luaState,null,true);
		addMember(l,"origin",get_origin,null,true);
		addMember(l,"LuaStateMemory",get_LuaStateMemory,null,true);
		createTypeMetatable(l,constructor, typeof(MyLuaSrv),typeof(GameFramework.SingletonO<MyLuaSrv>));
	}
}
