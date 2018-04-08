using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SLuaSys : CGameSystem
{
    private static SLuaSys _Instance = null;
    public static SLuaSys Instance
    {
        get
        {
            if(_Instance == null)
            {
                _Instance = CGameRoot.GetGameSystem<SLuaSys>();
            }
            return _Instance;
        }
    }

    public override bool SysEnter()
    {
        return true;
    }

    public IEnumerator preLoadLua()
    {
        if(!mSystemInited)
        {
            LuaLoadOverrider.Instance.Init();
            startLua();

            mSystemInited = true;
            yield break;
        }
    }

    private bool mSystemInited = false;
    private LuaWorker worker = null;
    public bool startLua()
    {
        initLuaWorker();
        return true;
    }
    
    /// <summary>
    /// 初始化Lua
    /// </summary>
    private void initLuaWorker()
    {
        string luaText = "Script.Main.Main";
        worker = new LuaWorker(luaText);
        worker.DoFile();
    }

    public override void SysFinalize()
    {
        MyLuaSrv.Instance.Dispose();
        if (worker != null)
            worker.Dispose();
        base.SysFinalize();
    }
}
