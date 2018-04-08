using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_BaseBehaviour : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Init(IntPtr l) {
		try {
			BaseBehaviour self=(BaseBehaviour)checkSelf(l);
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
	static public int GetComponentInMy(IntPtr l) {
		try {
			BaseBehaviour self=(BaseBehaviour)checkSelf(l);
			var ret=self.GetComponentInMy<UnityEngine.Component>();
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
	static public int Destory(IntPtr l) {
		try {
			BaseBehaviour self=(BaseBehaviour)checkSelf(l);
			self.Destory();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_trans(IntPtr l) {
		try {
			BaseBehaviour self=(BaseBehaviour)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.trans);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"BaseBehaviour");
		addMember(l,Init);
		addMember(l,GetComponentInMy);
		addMember(l,Destory);
		addMember(l,"trans",get_trans,null,true);
		createTypeMetatable(l,null, typeof(BaseBehaviour),typeof(UnityEngine.MonoBehaviour));
	}
}
