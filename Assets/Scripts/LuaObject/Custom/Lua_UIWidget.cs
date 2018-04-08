﻿using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_UIWidget : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetDimensions(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			self.SetDimensions(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetSides(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			UnityEngine.Transform a1;
			checkType(l,2,out a1);
			var ret=self.GetSides(a1);
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
	static public int CalculateFinalAlpha(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.CalculateFinalAlpha(a1);
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
	static public int Invalidate(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			System.Boolean a1;
			checkType(l,2,out a1);
			self.Invalidate(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CalculateCumulativeAlpha(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.CalculateCumulativeAlpha(a1);
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
	static public int SetRect(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			System.Single a1;
			checkType(l,2,out a1);
			System.Single a2;
			checkType(l,3,out a2);
			System.Single a3;
			checkType(l,4,out a3);
			System.Single a4;
			checkType(l,5,out a4);
			self.SetRect(a1,a2,a3,a4);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ResizeCollider(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			self.ResizeCollider();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CalculateBounds(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				UIWidget self=(UIWidget)checkSelf(l);
				var ret=self.CalculateBounds();
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==2){
				UIWidget self=(UIWidget)checkSelf(l);
				UnityEngine.Transform a1;
				checkType(l,2,out a1);
				var ret=self.CalculateBounds(a1);
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
	static public int SetDirty(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			self.SetDirty();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int RemoveFromPanel(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			self.RemoveFromPanel();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int MarkAsChanged(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			self.MarkAsChanged();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CreatePanel(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			var ret=self.CreatePanel();
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
	static public int CheckLayer(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			self.CheckLayer();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ParentHasChanged(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			self.ParentHasChanged();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int UpdateVisibility(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			System.Boolean a1;
			checkType(l,2,out a1);
			System.Boolean a2;
			checkType(l,3,out a2);
			var ret=self.UpdateVisibility(a1,a2);
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
	static public int UpdateTransform(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.UpdateTransform(a1);
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
	static public int UpdateGeometry(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.UpdateGeometry(a1);
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
	static public int WriteToBuffers(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			System.Collections.Generic.List<UnityEngine.Vector3> a1;
			checkType(l,2,out a1);
			System.Collections.Generic.List<UnityEngine.Vector2> a2;
			checkType(l,3,out a2);
			System.Collections.Generic.List<UnityEngine.Color> a3;
			checkType(l,4,out a3);
			System.Collections.Generic.List<UnityEngine.Vector3> a4;
			checkType(l,5,out a4);
			System.Collections.Generic.List<UnityEngine.Vector4> a5;
			checkType(l,6,out a5);
			System.Collections.Generic.List<UnityEngine.Vector4> a6;
			checkType(l,7,out a6);
			self.WriteToBuffers(a1,a2,a3,a4,a5,a6);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int MakePixelPerfect(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			self.MakePixelPerfect();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int OnFill(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
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
	static public int FullCompareFunc_s(IntPtr l) {
		try {
			UIWidget a1;
			checkType(l,1,out a1);
			UIWidget a2;
			checkType(l,2,out a2);
			var ret=UIWidget.FullCompareFunc(a1,a2);
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
	static public int PanelCompareFunc_s(IntPtr l) {
		try {
			UIWidget a1;
			checkType(l,1,out a1);
			UIWidget a2;
			checkType(l,2,out a2);
			var ret=UIWidget.PanelCompareFunc(a1,a2);
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
	static public int set_onChange(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			UIWidget.OnDimensionsChanged v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.onChange=v;
			else if(op==1) self.onChange+=v;
			else if(op==2) self.onChange-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_onPostFill(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			UIWidget.OnPostFillCallback v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.onPostFill=v;
			else if(op==1) self.onPostFill+=v;
			else if(op==2) self.onPostFill-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_mOnRender(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			UIDrawCall.OnRenderCallback v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.mOnRender=v;
			else if(op==1) self.mOnRender+=v;
			else if(op==2) self.mOnRender-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_autoResizeBoxCollider(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.autoResizeBoxCollider);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_autoResizeBoxCollider(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self.autoResizeBoxCollider=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_hideIfOffScreen(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.hideIfOffScreen);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_hideIfOffScreen(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self.hideIfOffScreen=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_keepAspectRatio(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.keepAspectRatio);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_keepAspectRatio(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			UIWidget.AspectRatioSource v;
			checkEnum(l,2,out v);
			self.keepAspectRatio=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_aspectRatio(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.aspectRatio);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_aspectRatio(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self.aspectRatio=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_hitCheck(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			UIWidget.HitCheck v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.hitCheck=v;
			else if(op==1) self.hitCheck+=v;
			else if(op==2) self.hitCheck-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_panel(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.panel);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_panel(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			UIPanel v;
			checkType(l,2,out v);
			self.panel=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_geometry(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.geometry);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_geometry(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			UIGeometry v;
			checkType(l,2,out v);
			self.geometry=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_fillGeometry(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.fillGeometry);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_fillGeometry(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self.fillGeometry=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_drawCall(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.drawCall);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_drawCall(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			UIDrawCall v;
			checkType(l,2,out v);
			self.drawCall=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_onRender(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			UIDrawCall.OnRenderCallback v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.onRender=v;
			else if(op==1) self.onRender+=v;
			else if(op==2) self.onRender-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_drawRegion(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.drawRegion);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_drawRegion(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			UnityEngine.Vector4 v;
			checkType(l,2,out v);
			self.drawRegion=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_pivotOffset(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.pivotOffset);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_width(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.width);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_width(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.width=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_height(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.height);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_height(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.height=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_color(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.color);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_color(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			UnityEngine.Color v;
			checkType(l,2,out v);
			self.color=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_alpha(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.alpha);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_alpha(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			float v;
			checkType(l,2,out v);
			self.alpha=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_isVisible(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.isVisible);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_hasVertices(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.hasVertices);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_rawPivot(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.rawPivot);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_rawPivot(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			UIWidget.Pivot v;
			checkEnum(l,2,out v);
			self.rawPivot=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_pivot(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.pivot);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_pivot(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			UIWidget.Pivot v;
			checkEnum(l,2,out v);
			self.pivot=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_depth(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.depth);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_depth(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.depth=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_raycastDepth(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.raycastDepth);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_localCorners(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.localCorners);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_localSize(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.localSize);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_localCenter(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.localCenter);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_worldCorners(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.worldCorners);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_worldCenter(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.worldCenter);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_drawingDimensions(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.drawingDimensions);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_material(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
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
			UIWidget self=(UIWidget)checkSelf(l);
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
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_mainTexture(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.mainTexture);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_mainTexture(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			UnityEngine.Texture v;
			checkType(l,2,out v);
			self.mainTexture=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_shader(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.shader);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_shader(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			UnityEngine.Shader v;
			checkType(l,2,out v);
			self.shader=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_hasBoxCollider(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.hasBoxCollider);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_minWidth(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.minWidth);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_minHeight(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.minHeight);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_border(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.border);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_border(IntPtr l) {
		try {
			UIWidget self=(UIWidget)checkSelf(l);
			UnityEngine.Vector4 v;
			checkType(l,2,out v);
			self.border=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"UIWidget");
		addMember(l,SetDimensions);
		addMember(l,GetSides);
		addMember(l,CalculateFinalAlpha);
		addMember(l,Invalidate);
		addMember(l,CalculateCumulativeAlpha);
		addMember(l,SetRect);
		addMember(l,ResizeCollider);
		addMember(l,CalculateBounds);
		addMember(l,SetDirty);
		addMember(l,RemoveFromPanel);
		addMember(l,MarkAsChanged);
		addMember(l,CreatePanel);
		addMember(l,CheckLayer);
		addMember(l,ParentHasChanged);
		addMember(l,UpdateVisibility);
		addMember(l,UpdateTransform);
		addMember(l,UpdateGeometry);
		addMember(l,WriteToBuffers);
		addMember(l,MakePixelPerfect);
		addMember(l,OnFill);
		addMember(l,FullCompareFunc_s);
		addMember(l,PanelCompareFunc_s);
		addMember(l,"onChange",null,set_onChange,true);
		addMember(l,"onPostFill",null,set_onPostFill,true);
		addMember(l,"mOnRender",null,set_mOnRender,true);
		addMember(l,"autoResizeBoxCollider",get_autoResizeBoxCollider,set_autoResizeBoxCollider,true);
		addMember(l,"hideIfOffScreen",get_hideIfOffScreen,set_hideIfOffScreen,true);
		addMember(l,"keepAspectRatio",get_keepAspectRatio,set_keepAspectRatio,true);
		addMember(l,"aspectRatio",get_aspectRatio,set_aspectRatio,true);
		addMember(l,"hitCheck",null,set_hitCheck,true);
		addMember(l,"panel",get_panel,set_panel,true);
		addMember(l,"geometry",get_geometry,set_geometry,true);
		addMember(l,"fillGeometry",get_fillGeometry,set_fillGeometry,true);
		addMember(l,"drawCall",get_drawCall,set_drawCall,true);
		addMember(l,"onRender",null,set_onRender,true);
		addMember(l,"drawRegion",get_drawRegion,set_drawRegion,true);
		addMember(l,"pivotOffset",get_pivotOffset,null,true);
		addMember(l,"width",get_width,set_width,true);
		addMember(l,"height",get_height,set_height,true);
		addMember(l,"color",get_color,set_color,true);
		addMember(l,"alpha",get_alpha,set_alpha,true);
		addMember(l,"isVisible",get_isVisible,null,true);
		addMember(l,"hasVertices",get_hasVertices,null,true);
		addMember(l,"rawPivot",get_rawPivot,set_rawPivot,true);
		addMember(l,"pivot",get_pivot,set_pivot,true);
		addMember(l,"depth",get_depth,set_depth,true);
		addMember(l,"raycastDepth",get_raycastDepth,null,true);
		addMember(l,"localCorners",get_localCorners,null,true);
		addMember(l,"localSize",get_localSize,null,true);
		addMember(l,"localCenter",get_localCenter,null,true);
		addMember(l,"worldCorners",get_worldCorners,null,true);
		addMember(l,"worldCenter",get_worldCenter,null,true);
		addMember(l,"drawingDimensions",get_drawingDimensions,null,true);
		addMember(l,"material",get_material,set_material,true);
		addMember(l,"mainTexture",get_mainTexture,set_mainTexture,true);
		addMember(l,"shader",get_shader,set_shader,true);
		addMember(l,"hasBoxCollider",get_hasBoxCollider,null,true);
		addMember(l,"minWidth",get_minWidth,null,true);
		addMember(l,"minHeight",get_minHeight,null,true);
		addMember(l,"border",get_border,set_border,true);
		createTypeMetatable(l,null, typeof(UIWidget),typeof(UIRect));
	}
}
