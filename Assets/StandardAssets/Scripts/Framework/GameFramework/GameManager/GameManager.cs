///Data         2018.02.06
///Description
///

using GameFramework;
using UnityEngine;

namespace GameFramework
{
    public class GameManager 
    {
        static private CGameRoot _gameRoot = null;
        static public CGameRoot gameRoot
        {
            get
            {
                if (_gameRoot == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "GameRoot";
                    _gameRoot = obj.AddComponent<CGameRoot>();
                    Object.DontDestroyOnLoad(obj);
                    //_gameClient.Init();
                }
                return _gameRoot;
            }
        }       
    }
}