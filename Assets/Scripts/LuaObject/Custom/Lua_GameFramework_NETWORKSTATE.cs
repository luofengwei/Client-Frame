using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_NETWORKSTATE : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"GameFramework.NETWORKSTATE");
		addMember(l,0,"NS_NONE");
		addMember(l,1,"NS_CONNECTING");
		addMember(l,2,"NS_WAITINGLOGIN");
		addMember(l,3,"NS_CONNECTED");
		addMember(l,4,"NS_DISCONNECTED");
		addMember(l,5,"NS_CONNECTSERVERERROR");
		addMember(l,6,"NS_SERVERFULL");
		addMember(l,7,"NS_WAITTING");
		addMember(l,8,"NS_RETRY");
		LuaDLL.lua_pop(l, 1);
	}
}
