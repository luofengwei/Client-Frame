using UnityEngine;
using System;
using System.Collections.Generic;

[CustomLuaClassAttribute]
public enum  EStateType
{
    None,
    Root,
    PreLoad,
    Login,
    Init,
    Match,
}

class CGameState
{
    public EStateType mStateType;
    public Type[] mSystems;
    public CGameState[] mChildStates;

    public CGameState(EStateType stateType,Type[] systems,CGameState[] childState)
    {
        mStateType = stateType;
        mSystems = systems;
        mChildStates = childState;
    }
}

public enum EGameRootCfgType
{
    None,
    Game,
}

public delegate CGameSystem[] DInitialSysDelegate(Transform rootObj);

partial class CGameRootCfg
{
    public CGameState mRoot;
    public DInitialSysDelegate mInitialDelegate;
    public Dictionary<EStateType, CGameState[]> mStateMap = new Dictionary<EStateType, CGameState[]>();

    public CGameRootCfg(DInitialSysDelegate initialSysDelegate,CGameState root)
    {
        mInitialDelegate = initialSysDelegate;
        mRoot = root;

        List<CGameState> parentList = new List<CGameState>();
        mStateMap.Clear();
        mStateMap.Add(EStateType.None,new CGameState[] { });
        BuildStateMap(mRoot,parentList);
    }

    private void BuildStateMap(CGameState state,List<CGameState> parentList)
    {
        if(mStateMap.ContainsKey(state.mStateType))
        {
            throw new System.Exception();
        }

        parentList.Add(state);
        mStateMap.Add(state.mStateType, parentList.ToArray());

        if(state.mChildStates !=null)
        {
            foreach (CGameState childState in state.mChildStates)
            {
                BuildStateMap(childState,parentList);
            }
        }

        parentList.RemoveAt(parentList.Count - 1);
    }

    private static CGameSystem CreateGameSys<T>(Transform rootObj) where T :CGameSystem
    {
        GameObject go = new GameObject();
        go.transform.parent = rootObj;
        CGameSystem ret = go.AddComponent<T>();
        go.name = ret.Name;
        return ret;
    }
}