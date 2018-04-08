﻿using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_EventDelegate : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int constructor(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			EventDelegate o;
			if(argc==1){
				o=new EventDelegate();
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(argc==2){
				EventDelegate.Callback a1;
				LuaDelegation.checkDelegate(l,2,out a1);
				o=new EventDelegate(a1);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(argc==3){
				UnityEngine.MonoBehaviour a1;
				checkType(l,2,out a1);
				System.String a2;
				checkType(l,3,out a2);
				o=new EventDelegate(a1,a2);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			return error(l,"New object failed.");
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Set(IntPtr l) {
		try {
			EventDelegate self=(EventDelegate)checkSelf(l);
			UnityEngine.MonoBehaviour a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.Set(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Execute(IntPtr l) {
		try {
			EventDelegate self=(EventDelegate)checkSelf(l);
			var ret=self.Execute();
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
	static public int Clear(IntPtr l) {
		try {
			EventDelegate self=(EventDelegate)checkSelf(l);
			self.Clear();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Execute_s(IntPtr l) {
		try {
			System.Collections.Generic.List<EventDelegate> a1;
			checkType(l,1,out a1);
			EventDelegate.Execute(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int IsValid_s(IntPtr l) {
		try {
			System.Collections.Generic.List<EventDelegate> a1;
			checkType(l,1,out a1);
			var ret=EventDelegate.IsValid(a1);
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
	static public int Set_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(List<EventDelegate>),typeof(EventDelegate))){
				System.Collections.Generic.List<EventDelegate> a1;
				checkType(l,1,out a1);
				EventDelegate a2;
				checkType(l,2,out a2);
				EventDelegate.Set(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(List<EventDelegate>),typeof(EventDelegate.Callback))){
				System.Collections.Generic.List<EventDelegate> a1;
				checkType(l,1,out a1);
				EventDelegate.Callback a2;
				LuaDelegation.checkDelegate(l,2,out a2);
				var ret=EventDelegate.Set(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
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
	static public int Add_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(List<EventDelegate>),typeof(EventDelegate))){
				System.Collections.Generic.List<EventDelegate> a1;
				checkType(l,1,out a1);
				EventDelegate a2;
				checkType(l,2,out a2);
				EventDelegate.Add(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(List<EventDelegate>),typeof(EventDelegate.Callback))){
				System.Collections.Generic.List<EventDelegate> a1;
				checkType(l,1,out a1);
				EventDelegate.Callback a2;
				LuaDelegation.checkDelegate(l,2,out a2);
				var ret=EventDelegate.Add(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(List<EventDelegate>),typeof(EventDelegate),typeof(bool))){
				System.Collections.Generic.List<EventDelegate> a1;
				checkType(l,1,out a1);
				EventDelegate a2;
				checkType(l,2,out a2);
				System.Boolean a3;
				checkType(l,3,out a3);
				EventDelegate.Add(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(List<EventDelegate>),typeof(EventDelegate.Callback),typeof(bool))){
				System.Collections.Generic.List<EventDelegate> a1;
				checkType(l,1,out a1);
				EventDelegate.Callback a2;
				LuaDelegation.checkDelegate(l,2,out a2);
				System.Boolean a3;
				checkType(l,3,out a3);
				var ret=EventDelegate.Add(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
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
	static public int Remove_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(List<EventDelegate>),typeof(EventDelegate))){
				System.Collections.Generic.List<EventDelegate> a1;
				checkType(l,1,out a1);
				EventDelegate a2;
				checkType(l,2,out a2);
				var ret=EventDelegate.Remove(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(List<EventDelegate>),typeof(EventDelegate.Callback))){
				System.Collections.Generic.List<EventDelegate> a1;
				checkType(l,1,out a1);
				EventDelegate.Callback a2;
				LuaDelegation.checkDelegate(l,2,out a2);
				var ret=EventDelegate.Remove(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
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
	static public int get_oneShot(IntPtr l) {
		try {
			EventDelegate self=(EventDelegate)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.oneShot);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_oneShot(IntPtr l) {
		try {
			EventDelegate self=(EventDelegate)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self.oneShot=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_target(IntPtr l) {
		try {
			EventDelegate self=(EventDelegate)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.target);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_target(IntPtr l) {
		try {
			EventDelegate self=(EventDelegate)checkSelf(l);
			UnityEngine.MonoBehaviour v;
			checkType(l,2,out v);
			self.target=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_methodName(IntPtr l) {
		try {
			EventDelegate self=(EventDelegate)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.methodName);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_methodName(IntPtr l) {
		try {
			EventDelegate self=(EventDelegate)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.methodName=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_parameters(IntPtr l) {
		try {
			EventDelegate self=(EventDelegate)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.parameters);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_isValid(IntPtr l) {
		try {
			EventDelegate self=(EventDelegate)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.isValid);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_isEnabled(IntPtr l) {
		try {
			EventDelegate self=(EventDelegate)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.isEnabled);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"EventDelegate");
		addMember(l,Set);
		addMember(l,Execute);
		addMember(l,Clear);
		addMember(l,Execute_s);
		addMember(l,IsValid_s);
		addMember(l,Set_s);
		addMember(l,Add_s);
		addMember(l,Remove_s);
		addMember(l,"oneShot",get_oneShot,set_oneShot,true);
		addMember(l,"target",get_target,set_target,true);
		addMember(l,"methodName",get_methodName,set_methodName,true);
		addMember(l,"parameters",get_parameters,null,true);
		addMember(l,"isValid",get_isValid,null,true);
		addMember(l,"isEnabled",get_isEnabled,null,true);
		createTypeMetatable(l,constructor, typeof(EventDelegate));
	}
}
