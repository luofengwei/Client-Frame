﻿using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_TweenScale : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetStartToCurrentValue(IntPtr l) {
		try {
			TweenScale self=(TweenScale)checkSelf(l);
			self.SetStartToCurrentValue();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetEndToCurrentValue(IntPtr l) {
		try {
			TweenScale self=(TweenScale)checkSelf(l);
			self.SetEndToCurrentValue();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Begin_s(IntPtr l) {
		try {
			UnityEngine.GameObject a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			UnityEngine.Vector3 a3;
			checkType(l,3,out a3);
			var ret=TweenScale.Begin(a1,a2,a3);
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
	static public int get_from(IntPtr l) {
		try {
			TweenScale self=(TweenScale)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.from);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_from(IntPtr l) {
		try {
			TweenScale self=(TweenScale)checkSelf(l);
			UnityEngine.Vector3 v;
			checkType(l,2,out v);
			self.from=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_to(IntPtr l) {
		try {
			TweenScale self=(TweenScale)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.to);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_to(IntPtr l) {
		try {
			TweenScale self=(TweenScale)checkSelf(l);
			UnityEngine.Vector3 v;
			checkType(l,2,out v);
			self.to=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_updateTable(IntPtr l) {
		try {
			TweenScale self=(TweenScale)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.updateTable);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_updateTable(IntPtr l) {
		try {
			TweenScale self=(TweenScale)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self.updateTable=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_cachedTransform(IntPtr l) {
		try {
			TweenScale self=(TweenScale)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.cachedTransform);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_value(IntPtr l) {
		try {
			TweenScale self=(TweenScale)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.value);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_value(IntPtr l) {
		try {
			TweenScale self=(TweenScale)checkSelf(l);
			UnityEngine.Vector3 v;
			checkType(l,2,out v);
			self.value=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"TweenScale");
		addMember(l,SetStartToCurrentValue);
		addMember(l,SetEndToCurrentValue);
		addMember(l,Begin_s);
		addMember(l,"from",get_from,set_from,true);
		addMember(l,"to",get_to,set_to,true);
		addMember(l,"updateTable",get_updateTable,set_updateTable,true);
		addMember(l,"cachedTransform",get_cachedTransform,null,true);
		addMember(l,"value",get_value,set_value,true);
		createTypeMetatable(l,null, typeof(TweenScale),typeof(UITweener));
	}
}
