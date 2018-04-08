using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//渠道的枚举
public enum ChannelType
{
    None = 0,
    Ios = 1,
    Android = 2,
    Window = 3,
    OSX = 4,
}

public delegate void DGameStateChangeEventHandler(EStateType newState, EStateType oldState);

public interface IGameSys { }

public class CGameRoot : MonoBehaviour {

    public const string cRootName = "_GameRoot";
    //加载脚本的模式
    public bool UseResourceScriptLoad = true;

    #region 对外接口
    //当前的渠道ID   根据当前的目标环境设置
    public static ChannelType CurChannel
    {
        get
        {           
            if (ABPathHelper.platformFolder.Equals("Android"))
                return ChannelType.Android;
            else if (ABPathHelper.platformFolder.Equals("iOS"))
                return ChannelType.Ios;
            else if (ABPathHelper.platformFolder.Equals("Windows"))
                return ChannelType.Window;
            else if (ABPathHelper.platformFolder.Equals("OSX"))
                return ChannelType.OSX;
            return ChannelType.Ios;
        }
    }

    static private UIRoot _UIRoot = null;
    static public UIRoot UIRoot
    {
        get
        {
            if (_UIRoot == null)
            {
                _UIRoot = GameObject.Find("UIRoot").GetComponent<UIRoot>();
            }
            return _UIRoot;
        }
    }

    private static GameObject _gameRootObject = null;
    public static GameObject rootObj
    {
        get
        {
            if(_gameRootObject == null)
            {
                _gameRootObject = GameObject.Find(cRootName);
            }
            return _gameRootObject;
        }
    }

    public static EGameRootCfgType ConfigType
    {
        get
        {
            if (instance != null)
                return instance.mConfigType;
            else
                return EGameRootCfgType.None;
        }
    }

    public static void SwitchToState(EStateType stateType)
    {
        if (stateType == EStateType.None)
            return;

        instance._SwitchToState(stateType);
    }

    static private GameObject _GameRootObject = null;
    static public GameObject GameRootObject
    {
        get
        {
            if (_GameRootObject == null)
                _GameRootObject = GameObject.Find(cRootName);
            return _GameRootObject;
        }
    }

    private static CGameRoot _gameRoot = null;
    public static CGameRoot instance
    {
        get
        {
            if(_gameRoot == null)
            {
                _gameRoot = rootObj.GetComponent<CGameRoot>();
            }
            return _gameRoot;
        }
    }

    static public T GetGameSystem<T>() 
        where T : CGameSystem
    {
        if (instance == null)
            return null;
        else
            return instance._GetGameSystem<T>();
    }

    static public CGameSystem GetGameSystem(Type type)
    {
        if (instance == null)
            return null;
        else
            return instance._GetGameSystem(type);
    }
    #endregion

    #region 生命周期函数

    private CGameRootCfg mConfig;
    [HideInInspector]
    private EGameRootCfgType mConfigType = EGameRootCfgType.Game;
    public void Awake()
    {
        //游戏生命期内不进行销毁
        DontDestroyOnLoad(this.gameObject);
        mConfig = CGameRootCfg.mGame;
    }

    public void Start()
    {
        //如果是编辑器模式，可以选择是否选择使用本地脚本和使用本地打包的bundle
#if (UNITY_EDITOR_OSX || UNITY_EDITOR)
        if (UseResourceScriptLoad)
            GameUtils.ScriptLoad = true;       //使用本地的脚本
        else
            GameUtils.ScriptLoad = false;      //使用bundle的脚本
#else
        GameUtils.ScriptLoad = false;
#endif

        ABPathHelper.InitCachePath();

        //此处要先读取本地的AssetManageConfig资源
        AssetManageConfigManager.Instance.Load();
        PoolManager.Instance.Init();
        ResourceManager.Instance.Init();

        //initial all system
        mSystems = mConfig.mInitialDelegate(this.transform);

        foreach(CGameSystem gameSys in mSystems)
        {
            mSystemMap.Add(gameSys.GetType(),gameSys);
        }
        foreach(CGameSystem gameSys in mSystems)
        {
            gameSys.SysInitial();
        }

        if (mFirstState != EStateType.None)
            SwitchToState(mFirstState);
        else
            _SwitchToState(EStateType.Root);
    }

    public void Update()
    {
        if (mSystems == null)
        {
            return;
        }

        for(int i=0;i<mSystems.Length;++i)
        {
            if(mSystems[i].SysEnabled)
            {
                mSystems[i].SysUpdate();
            }
        }
    }

    private void LateUpdate()
    {
        if (mSystems == null)
        {
            return;
        }

        for (int i=0;i<mSystems.Length;++i)
        {
            if(mSystems[i].SysEnabled)
            {
                mSystems[i].SysLateUpdate();
            }
        }
    }

    public void OnDestroy()
    {
        if(mSystems != null)
        {
            for(int i=mSystems.Length-1;i>=0;--i)
            {
                mSystems[i].SysFinalize();
                GameObject.DestroyImmediate(mSystems[i]);
            }
        }

        UnLoad();
        StopAllCoroutines();
        _gameRootObject = null;
        _gameRoot = null;
        mSystems = null;
        mConfig = null;
        mLeaveSystems.Clear();
        mEnterSystems.Clear();
        mSystemMap.Clear();
    }


    public void UnLoad()
    {
        AssetManageConfigManager.Instance.UnLoadAll();
        PoolManager.Instance.UnLoadAll();
        ResourceManager.Instance.UnLoadAll();
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
    }

    #endregion

    #region 系统流程构造/SwitchToState

    public static event DGameStateChangeEventHandler OnStateChange;
    public static event DGameStateChangeEventHandler OnPreStateChange;
    public static event DGameStateChangeEventHandler OnPostStateChange;

    [HideInInspector]
    private EStateType mFirstState = EStateType.PreLoad;

    private EStateType mCurState = EStateType.None;
    private EStateType mPreState = EStateType.None;
    private EStateType mNewState = EStateType.None;

    private List<CGameSystem> mLeaveSystems = new List<CGameSystem>();
    private List<CGameSystem> mEnterSystems = new List<CGameSystem>();

    private CGameSystem[] mSystems;
    private Dictionary<Type, CGameSystem> mSystemMap = new Dictionary<Type, CGameSystem>();

    bool running = false;
    private Queue<EStateType> switchQueue = new Queue<EStateType>();

    private void _SwitchToState(EStateType stateType)
    {
        switchQueue.Enqueue(stateType);
        if(running == false)
        {
            StartCoroutine(HandleSwitchQueue());
        }
    }
    
    static public EStateType PreState
    {
        get
        {
            if (instance != null)
                return instance.mPreState;
            else
                return EStateType.None;
        }
    }

    static public EStateType CurState
    {
        get
        {
            if (instance != null)
                return instance.mCurState;
            else
                return EStateType.None;
        }
    }

    //获取试图切换的新状态
    static public EStateType NewState
    {
        get
        {
            if (instance != null)
                return instance.mNewState;
            else
                return EStateType.None;
        }
    }


    private IEnumerator HandleSwitchQueue()
    {
        running = true;
        while(switchQueue.Count != 0)
        {
            yield return StartCoroutine(_SwitchToStateCo(switchQueue.Dequeue()));
        }
        running = false;
    }

    private IEnumerator _SwitchToStateCo(EStateType newState)
    {
        mNewState = newState;
        if(mCurState == newState)
        {
            yield break;
        }

        if (OnPreStateChange != null)
            OnPreStateChange(newState, mCurState);

        CGameState[] oldStates = mConfig.mStateMap[mCurState];
        CGameState[] newStates = mConfig.mStateMap[newState];

        int sameDepth = 0;
        while(sameDepth < newStates.Length && sameDepth < oldStates.Length
            && newStates[sameDepth] == oldStates[sameDepth])
        {
            ++sameDepth;
        }

        mLeaveSystems.Clear();
        mEnterSystems.Clear();

        for(int i=oldStates.Length - 1;i>=sameDepth;--i)
        {
            for(int j=0;j<oldStates[i].mSystems.Length;++j)
            {
                mLeaveSystems.Add(mSystemMap[oldStates[i].mSystems[j]]);
            }
        }

        for(int i=0;i<mLeaveSystems.Count;++i)
        {
            mLeaveSystems[i]._SysLeave();
        }

        if (OnStateChange != null)
            OnStateChange(newState,mCurState);

        for(int i=sameDepth;i<newStates.Length;++i)
        {
            for(int j=0;j<newStates[i].mSystems.Length;++j)
            {
                if (!mSystemMap.ContainsKey(newStates[i].mSystems[j]))
                    throw new Exception(string.Format("SystemMap.ContainKey({0})==false",newStates[i].mSystems[j].Name));

                mSystemMap[newStates[i].mSystems[j]].EnableInState = newStates[i].mStateType;
                mEnterSystems.Add(mSystemMap[newStates[i].mSystems[j]]);
            }
        }

        for(int i =0;i<mEnterSystems.Count;++i)
        {
            bool haveEnterCo = mEnterSystems[i]._SysEnter();
            if(haveEnterCo)
            {
                yield return StartCoroutine(mEnterSystems[i].SysEnterCo());
            }
            mEnterSystems[i]._EnterFinish();
        }

        //加入了新系统之后，再给旧系统一次清理的机会
        for(int i=0;i<mLeaveSystems.Count;++i)
        {
            mLeaveSystems[i].SysLastLeave();
        }

        for(int i =0;i<mEnterSystems.Count;++i)
        {
            mEnterSystems[i].OnStateChangeFinish();
        }

        mPreState = mCurState;
        mCurState = newState;

        if (OnPostStateChange != null)
            OnPostStateChange(newState,mPreState);
    }

    private T _GetGameSystem<T>() where T : CGameSystem
    {
        if (mSystemMap.ContainsKey(typeof(T)))
            return (T)mSystemMap[typeof(T)];
        else
            return null;
    }

    private CGameSystem _GetGameSystem(Type type)
    {
        if (mSystemMap.ContainsKey(type))
            return mSystemMap[type];
        else
            return null;
    }

    private bool _HaveSystemRegistered(Type type)
    {
        return mSystemMap.ContainsKey(type);
    }

    #endregion
}
