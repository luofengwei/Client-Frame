using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_UIScrollBar : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ForceUpdate(IntPtr l) {
		try {
			UIScrollBar self=(UIScrollBar)checkSelf(l);
			self.ForceUpdate();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_barSize(IntPtr l) {
		try {
			UIScrollBar self=(UIScrollBar)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.barSize);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_barSize(IntPtr l) {
		try {
			UIScrollBar self=(UIScrollBar)checkSelf(l);
			float v;
			checkType(l,2,out v);
			self.barSize=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"UIScrollBar");
		addMember(l,ForceUpdate);
		addMember(l,"barSize",get_barSize,set_barSize,true);
		createTypeMetatable(l,null, typeof(UIScrollBar),typeof(UISlider));
	}
}
