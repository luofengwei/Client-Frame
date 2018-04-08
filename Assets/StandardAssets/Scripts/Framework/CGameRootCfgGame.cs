using UnityEngine;
using System;

partial class CGameRootCfg
{
    public static CGameRootCfg mGame = new CGameRootCfg(

        delegate (Transform rootObj)
        {
            return new CGameSystem[]
            {
                CreateGameSys<SLuaSys>(rootObj),
                CreateGameSys<VersionFileSys>(rootObj),
                CreateGameSys<LoadingUISys>(rootObj),
                CreateGameSys<PreLoadSys>(rootObj),
                CreateGameSys<SceneLoadSys>(rootObj),
            };
        },

        new CGameState(EStateType.Root,
            new Type[]
            {
                typeof(VersionFileSys),
                typeof(LoadingUISys),
                typeof(PreLoadSys),
                typeof(SLuaSys),
            },
            new CGameState[]
            {
                new CGameState(EStateType.PreLoad,
                    new Type[]
                    {

                    },null),
                new CGameState(EStateType.Login,
                    new Type[]
                    {
                        typeof(SceneLoadSys),
                    },null),
                new CGameState(EStateType.Init,
                    new Type[]
                    {
                        typeof(SceneLoadSys),
                    },null),
                new CGameState(EStateType.Match,
                    new Type[]
                    {
                        typeof(SceneLoadSys),
                    },null),
            })

        );
}