﻿using System;
using System.Collections.Generic;
namespace SLua {
	[LuaBinder(3)]
	public class BindCustom {
		public static Action<IntPtr>[] GetBindList() {
			Action<IntPtr>[] list= {
				Lua_GameSysManager.reg,
				Lua_NetworkInterfaceManager.reg,
				Lua_AppHelper.reg,
				Lua_CUtility.reg,
				Lua_GameObjectUtils.reg,
				Lua_UIRect.reg,
				Lua_UIWidget.reg,
				Lua_UISpine.reg,
				Lua_Custom.reg,
				Lua_Deleg.reg,
				Lua_foostruct.reg,
				Lua_FloatEvent.reg,
				Lua_ListViewEvent.reg,
				Lua_SLuaTest.reg,
				Lua_System_Collections_Generic_List_1_int.reg,
				Lua_XXList.reg,
				Lua_AbsClass.reg,
				Lua_HelloWorld.reg,
				Lua_NewCoroutine.reg,
				Lua_ELoadType.reg,
				Lua_ResourceManager.reg,
				Lua_EStateType.reg,
				Lua_Fs_Config_FSLayerConfig.reg,
				Lua_FLEventBase.reg,
				Lua_BaseBehaviour.reg,
				Lua_FLEventDispatcherMono.reg,
				Lua_GameFramework_NETWORKSTATE.reg,
				Lua_LuaBehaviour.reg,
				Lua_LuaUIBehaviour.reg,
				Lua_LuaWorker.reg,
				Lua_MyLuaSrv.reg,
				Lua_System_Collections_Generic_Dictionary_2_int_string.reg,
				Lua_System_String.reg,
				Lua_UIPanel.reg,
				Lua_UIRoot.reg,
				Lua_UIBasicSprite.reg,
				Lua_UITexture.reg,
				Lua_UIInput.reg,
				Lua_UIAtlas.reg,
				Lua_UITextList.reg,
				Lua_UISprite.reg,
				Lua_UITweener.reg,
				Lua_TweenScale.reg,
				Lua_TweenAlpha.reg,
				Lua_UIScrollView.reg,
				Lua_UIPlayTween.reg,
				Lua_UIWidgetContainer.reg,
				Lua_UIButtonColor.reg,
				Lua_UIButton.reg,
				Lua_UICenterOnChild.reg,
				Lua_UIToggle.reg,
				Lua_UIGrid.reg,
				Lua_UIProgressBar.reg,
				Lua_UISlider.reg,
				Lua_UIScrollBar.reg,
				Lua_SpringPanel.reg,
				Lua_EventDelegate.reg,
				Lua_UIEventListener.reg,
				Lua_UILabel.reg,
				Lua_UIDragDropItem.reg,
			};
			return list;
		}
	}
}