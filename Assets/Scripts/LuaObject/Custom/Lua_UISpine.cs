using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_UISpine : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int OnFill(IntPtr l) {
		try {
			UISpine self=(UISpine)checkSelf(l);
			System.Collections.Generic.List<UnityEngine.Vector3> a1;
			checkType(l,2,out a1);
			System.Collections.Generic.List<UnityEngine.Vector2> a2;
			checkType(l,3,out a2);
			System.Collections.Generic.List<UnityEngine.Color> a3;
			checkType(l,4,out a3);
			self.OnFill(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_spine(IntPtr l) {
		try {
			UISpine self=(UISpine)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.spine);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_spine(IntPtr l) {
		try {
			UISpine self=(UISpine)checkSelf(l);
			NGUISpine v;
			checkType(l,2,out v);
			self.spine=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_spineMaterial(IntPtr l) {
		try {
			UISpine self=(UISpine)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.spineMaterial);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_spineMaterial(IntPtr l) {
		try {
			UISpine self=(UISpine)checkSelf(l);
			UnityEngine.Material v;
			checkType(l,2,out v);
			self.spineMaterial=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_material(IntPtr l) {
		try {
			UISpine self=(UISpine)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.material);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_material(IntPtr l) {
		try {
			UISpine self=(UISpine)checkSelf(l);
			UnityEngine.Material v;
			checkType(l,2,out v);
			self.material=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"UISpine");
		addMember(l,OnFill);
		addMember(l,"spine",get_spine,set_spine,true);
		addMember(l,"spineMaterial",get_spineMaterial,set_spineMaterial,true);
		addMember(l,"material",get_material,set_material,true);
		createTypeMetatable(l,null, typeof(UISpine),typeof(UIWidget));
	}
}
