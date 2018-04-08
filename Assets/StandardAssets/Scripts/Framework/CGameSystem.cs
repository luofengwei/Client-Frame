using UnityEngine;
using System.Collections;

public class CGameSystem : MonoBehaviour, IGameSys
{
    private string mName = null;
    public string Name
    {
        get
        {
            if (mName == null)
                mName = GetType().Name;
            return mName;
        }
    }

    public EStateType EnableInState
    {
        get{ return mEnableInState; }
        set{ mEnableInState = value; }
    }
    private EStateType mEnableInState;

    public virtual SEventData[] GetEventMap()
    {
        return new SEventData[] { };
    }

    #region Initial
    public virtual void SysInitial()
    {
        SEventData[] eventMap = GetEventMap();
        for(int i=0;i<eventMap.Length;++i)
        {

        }
    }
    #endregion

    #region Enter

    /// <summary>
    /// 系统是否已开始启用,在EnterFinish之后才算正在的启用了
    /// </summary>
    public bool SysEnabled { get { return mSysEnabled; } }
    protected bool mSysEnabled = false;

    public bool _SysEnter()
    {
        return SysEnter();
    }

    public virtual bool SysEnter()
    {
        return false;
    }

    public virtual IEnumerator SysEnterCo()
    {
        yield break;
    }

    public void _EnterFinish()
    {
        mSysEnabled = true;
        EnterFinish();
    }

    public virtual void EnterFinish()
    {

    }

    #endregion

    #region Leave

    public void _SysLeave()
    {
        mSysEnabled = false;
        EnableInState = EStateType.None;
        SysLeave();
    }

    public virtual void SysLeave()
    {

    }

    public virtual void SysLastLeave()
    {

    }

    #endregion

    #region Finalize

    public virtual void SysFinalize()
    {
        SEventData[] eventMap = GetEventMap();
        for(int i=0;i<eventMap.Length;++i)
        {

        }
    }

    #endregion

    public virtual void SysUpdate()
    {

    }

    public virtual void SysLateUpdate()
    {

    }

    public virtual void OnStateChangeFinish()
    {

    }
}

public struct SEventData
{
    public int eventKey;
    public DGameEventHandle eventHandle;
    
    public SEventData(int eventKey,DGameEventHandle eventHandle)
    {
        this.eventKey = eventKey;
        this.eventHandle = eventHandle;
    }
}
