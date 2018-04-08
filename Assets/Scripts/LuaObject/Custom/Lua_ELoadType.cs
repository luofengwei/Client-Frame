using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ELoadType : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"ELoadType");
		addMember(l,0,"UI");
		addMember(l,1,"Animation");
		addMember(l,2,"Music");
		addMember(l,3,"Sound");
		addMember(l,4,"Avatar");
		addMember(l,5,"Effect");
		addMember(l,6,"Icon");
		addMember(l,7,"Model");
		addMember(l,8,"Font");
		addMember(l,9,"Shader");
		addMember(l,10,"ShaderVariant");
		addMember(l,11,"Spine2D");
		addMember(l,12,"TableConfig");
		addMember(l,13,"LuaScript");
		addMember(l,14,"Scene");
		addMember(l,15,"Config");
		addMember(l,16,"Max");
		LuaDLL.lua_pop(l, 1);
	}
}
