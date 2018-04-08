using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_EStateType : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"EStateType");
		addMember(l,0,"None");
		addMember(l,1,"Root");
		addMember(l,2,"PreLoad");
		addMember(l,3,"Login");
		addMember(l,4,"Init");
		addMember(l,5,"Match");
		LuaDLL.lua_pop(l, 1);
	}
}
