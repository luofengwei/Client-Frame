///Data         2018.02.08
///Description
///

using UnityEngine;
using System;
using System.Net.Sockets;
using System.Threading;

namespace GameFramework
{
    public class RetryNetWorkState : ConnectingNetWorkState
    {
        private int nRetryTimes = 0;
        private float curTime = 0.0f;
        private float fInterval = 5.0f;

        public RetryNetWorkState(TcpSocket parent, NETWORKSTATE state = NETWORKSTATE.NS_RETRY)
            : base(parent, 5000, state)
        {
           
        }

        public override void Enter()
        {
            base.Enter();
            nRetryTimes = (int)(TcpConConstDefine.MaxReConnectedTime / TcpConConstDefine.SendReCTime);
            curTime = 0.0f;
            fInterval = TcpConConstDefine.SendReCTime;
            Connect();
            --nRetryTimes;
        }

        public override void Update()
        {
            if (nRetryTimes <= 0)
            {
                parent.ChangeState(NETWORKSTATE.NS_DISCONNECTED);
                return;
            }

            base.Update();
            curTime += Time.deltaTime;
            if (curTime > fInterval)
            {
                Connect();
                curTime -= fInterval;
                --nRetryTimes;               
            }
        }

        public override void Exit()
        {    
            base.Exit();           
        }      
    }
}