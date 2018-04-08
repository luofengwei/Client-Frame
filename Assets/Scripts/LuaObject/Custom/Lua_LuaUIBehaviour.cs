using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_LuaUIBehaviour : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_lua(IntPtr l) {
		try {
			LuaUIBehaviour self=(LuaUIBehaviour)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.lua);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_lua(IntPtr l) {
		try {
			LuaUIBehaviour self=(LuaUIBehaviour)checkSelf(l);
			UnityEngine.TextAsset v;
			checkType(l,2,out v);
			self.lua=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"LuaUIBehaviour");
		addMember(l,"lua",get_lua,set_lua,true);
		createTypeMetatable(l,null, typeof(LuaUIBehaviour),typeof(LuaBehaviour));
	}
}
