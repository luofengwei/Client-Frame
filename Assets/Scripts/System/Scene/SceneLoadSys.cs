using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SLua;

public class SceneLoadSys : CGameSystem {

    LuaFunction _preSwitchStateFunc = null;
    private LuaFunction preSwitchStateFunc
    {
        get
        {
            if (_preSwitchStateFunc == null)
                _preSwitchStateFunc = MyLuaSrv.Instance.GetFunction("SceneSwitchState.PreSwitchState");
            return _preSwitchStateFunc;
        }
    }

    LuaFunction _switchStateFunc = null;
    private LuaFunction switchStateFunc
    {
        get
        {
            if (_switchStateFunc == null)
                _switchStateFunc = MyLuaSrv.Instance.GetFunction("SceneSwitchState.SwitchState");
            return _switchStateFunc;
        }
    }

    LuaFunction _postSwitchStateFunc = null;
    private LuaFunction postSwitchStateFunc
    {
        get
        {
            if (_postSwitchStateFunc == null)
                _postSwitchStateFunc = MyLuaSrv.Instance.GetFunction("SceneSwitchState.PostSwitchState");
            return _postSwitchStateFunc;
        }
    }

    public override void SysInitial()
    {
        base.SysInitial();

        CGameRoot.OnPreStateChange += EvtGamePreSwitchState;
        CGameRoot.OnStateChange += EvtGameSwitchState;
        CGameRoot.OnPostStateChange += EvtGamePostSwitchState;
    }

    public override bool SysEnter()
    {
        return true;
    }

    public override IEnumerator SysEnterCo()
    {
        base.SysEnterCo();

        yield return new WaitForSeconds(1.0f);
        yield return StartCoroutine(LoadGameRes());
    }

    private EStateType curResGameType = EStateType.None;
    public IEnumerator LoadGameRes()
    {
        if (EnableInState != curResGameType)
        {            
            curResGameType = EnableInState;
        }

        yield break;
    }

    private void EvtGamePreSwitchState(EStateType newState,EStateType curState)
    {
        if(preSwitchStateFunc != null)
        {
            preSwitchStateFunc.call(newState,curState);
        }
    }
    
    private void EvtGameSwitchState(EStateType newState,EStateType curState)
    {
        if (switchStateFunc != null)
        {
            switchStateFunc.call(newState,curState);
        }
    }

    private void EvtGamePostSwitchState(EStateType newState,EStateType curState)
    {
        if(postSwitchStateFunc != null)
        {
            postSwitchStateFunc.call(newState,curState);
        }
    }

    public override void SysLeave()
    {
        CGameRoot.OnPreStateChange -= EvtGamePreSwitchState;
        CGameRoot.OnStateChange -= EvtGameSwitchState;
        CGameRoot.OnPostStateChange -= EvtGamePostSwitchState;
    }

    public override void SysFinalize()
    {
        base.SysFinalize();
    }
}
