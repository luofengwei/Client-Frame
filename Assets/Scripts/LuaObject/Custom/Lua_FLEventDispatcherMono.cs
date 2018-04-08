using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_FLEventDispatcherMono : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int passEvent(IntPtr l) {
		try {
			FLEventDispatcherMono self=(FLEventDispatcherMono)checkSelf(l);
			FLEventBase a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.passEvent(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int DispatchEvent(IntPtr l) {
		try {
			FLEventDispatcherMono self=(FLEventDispatcherMono)checkSelf(l);
			FLEventBase a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.DispatchEvent(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int AddEventListener(IntPtr l) {
		try {
			FLEventDispatcherMono self=(FLEventDispatcherMono)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			FLEventBase.FLEventHandler a2;
			LuaDelegation.checkDelegate(l,3,out a2);
			self.AddEventListener(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int RemoveEventListener(IntPtr l) {
		try {
			FLEventDispatcherMono self=(FLEventDispatcherMono)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			FLEventBase.FLEventHandler a2;
			LuaDelegation.checkDelegate(l,3,out a2);
			self.RemoveEventListener(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"FLEventDispatcherMono");
		addMember(l,passEvent);
		addMember(l,DispatchEvent);
		addMember(l,AddEventListener);
		addMember(l,RemoveEventListener);
		createTypeMetatable(l,null, typeof(FLEventDispatcherMono),typeof(BaseBehaviour));
	}
}
