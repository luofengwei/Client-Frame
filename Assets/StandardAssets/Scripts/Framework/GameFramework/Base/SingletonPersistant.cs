///Data         2018.02.01
///Description
///

using UnityEngine;

namespace GameFramework
{
    public class SingletonPersistant<T> : Singleton<T> where T : Component
    {    
        protected override void GameSetup()
        {
            DontDestroyOnLoad(gameObject);
            base.GameSetup();
        }
    }
}