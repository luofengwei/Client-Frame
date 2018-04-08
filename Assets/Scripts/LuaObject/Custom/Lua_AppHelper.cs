using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_AppHelper : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int constructor(IntPtr l) {
		try {
			AppHelper o;
			o=new AppHelper();
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
	static public int get_Branch(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,AppHelper.Branch);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Branch(IntPtr l) {
		try {
			System.String v;
			checkType(l,2,out v);
			AppHelper.Branch=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_isIOS(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,AppHelper.isIOS);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_isAndroid(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,AppHelper.isAndroid);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_GameChannel(IntPtr l) {
		try {
			pushValue(l,true);
			pushEnum(l,(int)AppHelper.GameChannel);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_version(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,AppHelper.version);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_version(IntPtr l) {
		try {
			string v;
			checkType(l,2,out v);
			AppHelper.version=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_versionBase(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,AppHelper.versionBase);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_verionChannelBase(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,AppHelper.verionChannelBase);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_versionChannelTag(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,AppHelper.versionChannelTag);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_isVersionBase(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,AppHelper.isVersionBase);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_isTrunk(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,AppHelper.isTrunk);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_isS0(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,AppHelper.isS0);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_isS1(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,AppHelper.isS1);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"AppHelper");
		addMember(l,"Branch",get_Branch,set_Branch,false);
		addMember(l,"isIOS",get_isIOS,null,false);
		addMember(l,"isAndroid",get_isAndroid,null,false);
		addMember(l,"GameChannel",get_GameChannel,null,false);
		addMember(l,"version",get_version,set_version,false);
		addMember(l,"versionBase",get_versionBase,null,false);
		addMember(l,"verionChannelBase",get_verionChannelBase,null,false);
		addMember(l,"versionChannelTag",get_versionChannelTag,null,false);
		addMember(l,"isVersionBase",get_isVersionBase,null,false);
		addMember(l,"isTrunk",get_isTrunk,null,false);
		addMember(l,"isS0",get_isS0,null,false);
		addMember(l,"isS1",get_isS1,null,false);
		createTypeMetatable(l,constructor, typeof(AppHelper));
	}
}
