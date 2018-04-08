﻿using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_UnityEngine_Experimental_Director_AnimatorControllerPlayable : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int constructor(IntPtr l) {
		try {
			UnityEngine.Experimental.Director.AnimatorControllerPlayable o;
			o=new UnityEngine.Experimental.Director.AnimatorControllerPlayable();
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
	static public int GetFloat(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(int))){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				var ret=self.GetFloat(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(string))){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				var ret=self.GetFloat(a1);
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
	static public int SetFloat(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(int),typeof(float))){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				self.SetFloat(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(string),typeof(float))){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				self.SetFloat(a1,a2);
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
	static public int GetBool(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(int))){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				var ret=self.GetBool(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(string))){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				var ret=self.GetBool(a1);
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
	static public int SetBool(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(int),typeof(bool))){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				System.Boolean a2;
				checkType(l,3,out a2);
				self.SetBool(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(string),typeof(bool))){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				System.Boolean a2;
				checkType(l,3,out a2);
				self.SetBool(a1,a2);
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
	static public int GetInteger(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(int))){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				var ret=self.GetInteger(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(string))){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				var ret=self.GetInteger(a1);
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
	static public int SetInteger(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(int),typeof(int))){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				self.SetInteger(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(string),typeof(int))){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				self.SetInteger(a1,a2);
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
	static public int SetTrigger(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(int))){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				self.SetTrigger(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(string))){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				self.SetTrigger(a1);
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
	static public int ResetTrigger(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(int))){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				self.ResetTrigger(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(string))){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				self.ResetTrigger(a1);
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
	static public int IsParameterControlledByCurve(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(int))){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				var ret=self.IsParameterControlledByCurve(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(string))){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				var ret=self.IsParameterControlledByCurve(a1);
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
	static public int GetLayerName(IntPtr l) {
		try {
			UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetLayerName(a1);
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
	static public int GetLayerIndex(IntPtr l) {
		try {
			UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.GetLayerIndex(a1);
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
	static public int GetLayerWeight(IntPtr l) {
		try {
			UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetLayerWeight(a1);
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
	static public int SetLayerWeight(IntPtr l) {
		try {
			UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Single a2;
			checkType(l,3,out a2);
			self.SetLayerWeight(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetCurrentAnimatorStateInfo(IntPtr l) {
		try {
			UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetCurrentAnimatorStateInfo(a1);
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
	static public int GetNextAnimatorStateInfo(IntPtr l) {
		try {
			UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetNextAnimatorStateInfo(a1);
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
	static public int GetAnimatorTransitionInfo(IntPtr l) {
		try {
			UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetAnimatorTransitionInfo(a1);
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
	static public int GetCurrentAnimatorClipInfo(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				var ret=self.GetCurrentAnimatorClipInfo(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				System.Collections.Generic.List<UnityEngine.AnimatorClipInfo> a2;
				checkType(l,3,out a2);
				self.GetCurrentAnimatorClipInfo(a1,a2);
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
	static public int GetNextAnimatorClipInfo(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				var ret=self.GetNextAnimatorClipInfo(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				System.Collections.Generic.List<UnityEngine.AnimatorClipInfo> a2;
				checkType(l,3,out a2);
				self.GetNextAnimatorClipInfo(a1,a2);
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
	static public int GetCurrentAnimatorClipInfoCount(IntPtr l) {
		try {
			UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetCurrentAnimatorClipInfoCount(a1);
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
	static public int GetNextAnimatorClipInfoCount(IntPtr l) {
		try {
			UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetNextAnimatorClipInfoCount(a1);
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
	static public int IsInTransition(IntPtr l) {
		try {
			UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.IsInTransition(a1);
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
	static public int GetParameter(IntPtr l) {
		try {
			UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetParameter(a1);
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
	static public int CrossFadeInFixedTime(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(int),typeof(float))){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				self.CrossFadeInFixedTime(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(string),typeof(float))){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				self.CrossFadeInFixedTime(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(float),typeof(int))){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				System.Int32 a3;
				checkType(l,4,out a3);
				self.CrossFadeInFixedTime(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(string),typeof(float),typeof(int))){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				System.Int32 a3;
				checkType(l,4,out a3);
				self.CrossFadeInFixedTime(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(float),typeof(int),typeof(float))){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				System.Int32 a3;
				checkType(l,4,out a3);
				System.Single a4;
				checkType(l,5,out a4);
				self.CrossFadeInFixedTime(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(string),typeof(float),typeof(int),typeof(float))){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				System.Int32 a3;
				checkType(l,4,out a3);
				System.Single a4;
				checkType(l,5,out a4);
				self.CrossFadeInFixedTime(a1,a2,a3,a4);
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
	static public int CrossFade(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(int),typeof(float))){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				self.CrossFade(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(string),typeof(float))){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				self.CrossFade(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(float),typeof(int))){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				System.Int32 a3;
				checkType(l,4,out a3);
				self.CrossFade(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(string),typeof(float),typeof(int))){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				System.Int32 a3;
				checkType(l,4,out a3);
				self.CrossFade(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(float),typeof(int),typeof(float))){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				System.Int32 a3;
				checkType(l,4,out a3);
				System.Single a4;
				checkType(l,5,out a4);
				self.CrossFade(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(string),typeof(float),typeof(int),typeof(float))){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				System.Int32 a3;
				checkType(l,4,out a3);
				System.Single a4;
				checkType(l,5,out a4);
				self.CrossFade(a1,a2,a3,a4);
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
	static public int PlayInFixedTime(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(int))){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				self.PlayInFixedTime(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(string))){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				self.PlayInFixedTime(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(int))){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				self.PlayInFixedTime(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(string),typeof(int))){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				self.PlayInFixedTime(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(int),typeof(float))){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				System.Single a3;
				checkType(l,4,out a3);
				self.PlayInFixedTime(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(string),typeof(int),typeof(float))){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				System.Single a3;
				checkType(l,4,out a3);
				self.PlayInFixedTime(a1,a2,a3);
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
	static public int Play(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(int))){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				self.Play(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(string))){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				self.Play(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(int))){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				self.Play(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(string),typeof(int))){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				self.Play(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(int),typeof(float))){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				System.Single a3;
				checkType(l,4,out a3);
				self.Play(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(string),typeof(int),typeof(float))){
				UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				System.Single a3;
				checkType(l,4,out a3);
				self.Play(a1,a2,a3);
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
	static public int HasState(IntPtr l) {
		try {
			UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			var ret=self.HasState(a1,a2);
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
	static public int get_layerCount(IntPtr l) {
		try {
			UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.layerCount);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_parameterCount(IntPtr l) {
		try {
			UnityEngine.Experimental.Director.AnimatorControllerPlayable self=(UnityEngine.Experimental.Director.AnimatorControllerPlayable)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.parameterCount);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"UnityEngine.Experimental.Director.AnimatorControllerPlayable");
		addMember(l,GetFloat);
		addMember(l,SetFloat);
		addMember(l,GetBool);
		addMember(l,SetBool);
		addMember(l,GetInteger);
		addMember(l,SetInteger);
		addMember(l,SetTrigger);
		addMember(l,ResetTrigger);
		addMember(l,IsParameterControlledByCurve);
		addMember(l,GetLayerName);
		addMember(l,GetLayerIndex);
		addMember(l,GetLayerWeight);
		addMember(l,SetLayerWeight);
		addMember(l,GetCurrentAnimatorStateInfo);
		addMember(l,GetNextAnimatorStateInfo);
		addMember(l,GetAnimatorTransitionInfo);
		addMember(l,GetCurrentAnimatorClipInfo);
		addMember(l,GetNextAnimatorClipInfo);
		addMember(l,GetCurrentAnimatorClipInfoCount);
		addMember(l,GetNextAnimatorClipInfoCount);
		addMember(l,IsInTransition);
		addMember(l,GetParameter);
		addMember(l,CrossFadeInFixedTime);
		addMember(l,CrossFade);
		addMember(l,PlayInFixedTime);
		addMember(l,Play);
		addMember(l,HasState);
		addMember(l,"layerCount",get_layerCount,null,true);
		addMember(l,"parameterCount",get_parameterCount,null,true);
		createTypeMetatable(l,constructor, typeof(UnityEngine.Experimental.Director.AnimatorControllerPlayable),typeof(UnityEngine.Experimental.Director.AnimationPlayable));
	}
}
