using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ResourceManager : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Init(IntPtr l) {
		try {
			ResourceManager self=(ResourceManager)checkSelf(l);
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
	static public int LoadIcon(IntPtr l) {
		try {
			ResourceManager self=(ResourceManager)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			UnityEngine.GameObject a2;
			checkType(l,3,out a2);
			self.LoadIcon(a1,a2);
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
			ResourceManager self=(ResourceManager)checkSelf(l);
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
	static public int UnLoadIcon(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(string))){
				ResourceManager self=(ResourceManager)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				self.UnLoadIcon(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Object))){
				ResourceManager self=(ResourceManager)checkSelf(l);
				UnityEngine.Object a1;
				checkType(l,2,out a1);
				self.UnLoadIcon(a1);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int UnLoad(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==3){
				ResourceManager self=(ResourceManager)checkSelf(l);
				UnityEngine.GameObject a1;
				checkType(l,2,out a1);
				System.Boolean a2;
				checkType(l,3,out a2);
				self.UnLoad(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(argc==4){
				ResourceManager self=(ResourceManager)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				ELoadType a2;
				checkEnum(l,3,out a2);
				System.Boolean a3;
				checkType(l,4,out a3);
				self.UnLoad(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int RGet(IntPtr l) {
		try {
			ResourceManager self=(ResourceManager)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			ELoadType a3;
			checkEnum(l,4,out a3);
			UnityEngine.GameObject a4;
			checkType(l,5,out a4);
			var ret=self.RGet(a1,a2,a3,a4);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"ResourceManager");
		addMember(l,Init);
		addMember(l,LoadIcon);
		addMember(l,GC);
		addMember(l,UnLoadIcon);
		addMember(l,UnLoad);
		addMember(l,RGet);
		createTypeMetatable(l,null, typeof(ResourceManager),typeof(GameFramework.SingleTonGO<ResourceManager>));
	}
}
