///Data         2018.02.06
///Description
///

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameFramework
{       
    public delegate void OnEnterState();  //进入状态回调
    public delegate void OnExitState();  //离开状态回调

    public class NetBaseState
    {
        public NETWORKSTATE netState = NETWORKSTATE.NS_NONE;
        protected TcpSocket parent = null;
        private OnEnterState onEnter = null;
        private OnExitState onExit = null;

        public NetBaseState(TcpSocket parent, NETWORKSTATE netState = NETWORKSTATE.NS_NONE, OnEnterState enter = null, OnExitState exit = null)
        {
            this.parent = parent;
            this.netState = netState;
            onEnter = enter;
            onExit = exit;
        }

        public virtual void Enter()
        {
            GDebug.Log("Enter : " + netState);                 
            if (onEnter != null)
                onEnter();
        }

        public virtual void Update()
        {

        }

        public virtual void Exit()
        {
            GDebug.Log("Exit : " + netState);
            if (onExit != null)
                onExit();
        }
    }   

    public class NetWorkStateManager
    {
        private TcpSocket tcpSocket = null;
        private NetBaseState curState = null;
        private Dictionary<NETWORKSTATE, NetBaseState> stateDict = new Dictionary<NETWORKSTATE, NetBaseState>();
    
        public NetWorkStateManager(TcpSocket socket)
        {
            tcpSocket = socket;
            AddState(new NetBaseState(tcpSocket, NETWORKSTATE.NS_NONE));         
            AddState(new ConnectingNetWorkState(tcpSocket, 5000, NETWORKSTATE.NS_CONNECTING));
            AddState(new NetBaseState(tcpSocket, NETWORKSTATE.NS_CONNECTED));
            AddState(new DisConnectNetWorkState(tcpSocket, NETWORKSTATE.NS_DISCONNECTED));
            AddState(new NetBaseState(tcpSocket, NETWORKSTATE.NS_CONNECTSERVERERROR));
            AddState(new RetryNetWorkState(tcpSocket, NETWORKSTATE.NS_RETRY));

            Start(NETWORKSTATE.NS_NONE);
        }     

        public NETWORKSTATE GetNetState()
        {
            return curState != null ? curState.netState : NETWORKSTATE.NS_NONE;
        }
       
        private void AddState(NetBaseState state)
        {
            stateDict[state.netState] = state;
        }

        private void Start(NETWORKSTATE state)
        {
            curState = stateDict[state];
            if (curState!= null)
                curState.Enter();
            GDebug.Log("NetWorkStateManager Start : " + state);
        }

        public void Update()
        {
            if (curState != null)
                curState.Update();
        }

        public void ChangeState(NETWORKSTATE state)
        {
            if (curState != null && curState.netState != state)
            {
                curState.Exit();
                curState = stateDict[state];
                if (curState != null)
                    curState.Enter();    
                NetWorkConnection.Instance.ChangeNetState(state);
            }
        }
    }
}