///Data         2018.02.01
///Description
///

using UnityEngine;
using SLua;

namespace GameFramework
{
    [CustomLuaClassAttribute]
    public class SingletonO<T> where T : new()
    {
        protected static T _instance = default(T);

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                }
                return _instance;
            }
        }
    }

    public class SingleTonGO<T> : MonoBehaviour
        where T : SingleTonGO<T>, new()
    {
        protected static T _instance = default(T);
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject(typeof(T).FullName);
                    _instance = go.AddComponent<T>();
                    if (CGameRoot.GameRootObject != null)
                        go.transform.SetParent(CGameRoot.GameRootObject.transform);
                }
                return _instance;
            }            
        }       

        protected virtual void OnEnable()
        {
          
        }

        protected virtual void OnDestroy()
        {
           
        }
    }

    public class Singleton<T> : MonoBehaviour where T : Component
    {       
        public static T Instance { get; private set; }
      
        public string TypeName { get; private set; }
     
        public static bool IsActive
        {
            get
            {
                return Instance != null;
            }
        }

        void Awake()
        {
            TypeName = typeof(T).FullName;
            GDebug.Log(TypeName + ": Awake");

            if (Instance != null)
            {
                if (Instance != this)
                    Destroy(gameObject);
                return;            
            }
          
            Instance = this as T;
          
            GameSetup();
        }

        void OnDestroy()
        {
            GDebug.Log(TypeName + ": OnDestroy");

            if (Instance == this)
            {
                SaveState();
                GameDestroy();
            }
        }

        /// <summary>
        /// Save any state when the application quits.
        /// </summary>
        /// Note that iOS applications are usually suspended and do not quit. You should tick "Exit on Suspend" in Player settings 
        /// for iOS builds to cause the game to quit and not suspend, otherwise you may not see this call. If "Exit on Suspend" is 
        /// not ticked then you will see calls to OnApplicationPause instead.
        protected virtual void OnApplicationQuit()
        {
            GDebug.Log(TypeName + ": OnApplicationQuit");
            SaveState();
        }

        protected virtual void OnApplicationPause(bool pauseStatus)
        {
            GDebug.Log(TypeName + ": OnApplicationPause");
            SaveState();
        }        
      
        protected virtual void GameSetup()
        {
        }

        public virtual void SaveState()
        {
        }
       
        protected virtual void GameDestroy()
        {
        }
    }
}