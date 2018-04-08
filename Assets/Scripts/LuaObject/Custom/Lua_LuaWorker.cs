﻿using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_LuaWorker : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int constructor(IntPtr l) {
		try {
			LuaWorker o;
			System.String a1;
			checkType(l,2,out a1);
			o=new LuaWorker(a1);
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
	static public int GetChunk(IntPtr l) {
		try {
			LuaWorker self=(LuaWorker)checkSelf(l);
			var ret=self.GetChunk();
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
	static public int SetEnv(IntPtr l) {
		try {
			LuaWorker self=(LuaWorker)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Object a2;
			checkType(l,3,out a2);
			System.Boolean a3;
			checkType(l,4,out a3);
			self.SetEnv(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int DoString(IntPtr l) {
		try {
			LuaWorker self=(LuaWorker)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.DoString(a1);
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
			LuaWorker self=(LuaWorker)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.DoFile(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CallLuaStaticMethod(IntPtr l) {
		try {
			LuaWorker self=(LuaWorker)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Object[] a2;
			checkParams(l,3,out a2);
			var ret=self.CallLuaStaticMethod(a1,a2);
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
	static public int CallLuaInstanceMethod(IntPtr l) {
		try {
			LuaWorker self=(LuaWorker)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Object[] a2;
			checkParams(l,3,out a2);
			var ret=self.CallLuaInstanceMethod(a1,a2);
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
	static public int Dispose(IntPtr l) {
		try {
			LuaWorker self=(LuaWorker)checkSelf(l);
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
	static public int FormatException_s(IntPtr l) {
		try {
			System.Exception a1;
			checkType(l,1,out a1);
			var ret=LuaWorker.FormatException(a1);
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
	static public int GetFieldInt_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(SLua.LuaTable),typeof(int))){
				SLua.LuaTable a1;
				checkType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				var ret=LuaWorker.GetFieldInt(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(SLua.LuaTable),typeof(string))){
				SLua.LuaTable a1;
				checkType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				var ret=LuaWorker.GetFieldInt(a1,a2);
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
	static public int GetFieldLong_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(SLua.LuaTable),typeof(int))){
				SLua.LuaTable a1;
				checkType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				var ret=LuaWorker.GetFieldLong(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(SLua.LuaTable),typeof(string))){
				SLua.LuaTable a1;
				checkType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				var ret=LuaWorker.GetFieldLong(a1,a2);
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
	static public int GetFieldFloat_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(SLua.LuaTable),typeof(int))){
				SLua.LuaTable a1;
				checkType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				var ret=LuaWorker.GetFieldFloat(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(SLua.LuaTable),typeof(string))){
				SLua.LuaTable a1;
				checkType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				var ret=LuaWorker.GetFieldFloat(a1,a2);
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
	static public int GetFieldString_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(SLua.LuaTable),typeof(int))){
				SLua.LuaTable a1;
				checkType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				var ret=LuaWorker.GetFieldString(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(SLua.LuaTable),typeof(string))){
				SLua.LuaTable a1;
				checkType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				var ret=LuaWorker.GetFieldString(a1,a2);
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
	static public int GetFieldTable_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(SLua.LuaTable),typeof(int))){
				SLua.LuaTable a1;
				checkType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				var ret=LuaWorker.GetFieldTable(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(SLua.LuaTable),typeof(string))){
				SLua.LuaTable a1;
				checkType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				var ret=LuaWorker.GetFieldTable(a1,a2);
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
	static public int set_DoInit(IntPtr l) {
		try {
			LuaWorker self=(LuaWorker)checkSelf(l);
			System.Action<SLua.LuaTable> v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.DoInit=v;
			else if(op==1) self.DoInit+=v;
			else if(op==2) self.DoInit-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_table(IntPtr l) {
		try {
			LuaWorker self=(LuaWorker)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.table);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_LuaFile(IntPtr l) {
		try {
			LuaWorker self=(LuaWorker)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.LuaFile);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_LuaFile(IntPtr l) {
		try {
			LuaWorker self=(LuaWorker)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.LuaFile=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_isLuaReady(IntPtr l) {
		try {
			LuaWorker self=(LuaWorker)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.isLuaReady);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_LuaClassName(IntPtr l) {
		try {
			LuaWorker self=(LuaWorker)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.LuaClassName);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"LuaWorker");
		addMember(l,GetChunk);
		addMember(l,SetEnv);
		addMember(l,DoString);
		addMember(l,DoFile);
		addMember(l,CallLuaStaticMethod);
		addMember(l,CallLuaInstanceMethod);
		addMember(l,Dispose);
		addMember(l,FormatException_s);
		addMember(l,GetFieldInt_s);
		addMember(l,GetFieldLong_s);
		addMember(l,GetFieldFloat_s);
		addMember(l,GetFieldString_s);
		addMember(l,GetFieldTable_s);
		addMember(l,"DoInit",null,set_DoInit,true);
		addMember(l,"table",get_table,null,true);
		addMember(l,"LuaFile",get_LuaFile,set_LuaFile,true);
		addMember(l,"isLuaReady",get_isLuaReady,null,true);
		addMember(l,"LuaClassName",get_LuaClassName,null,true);
		createTypeMetatable(l,constructor, typeof(LuaWorker));
	}
}
