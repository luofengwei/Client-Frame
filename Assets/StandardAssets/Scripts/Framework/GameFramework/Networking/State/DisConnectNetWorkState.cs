///Data         2018.02.08
///Description
///

using System;
using System.Net.Sockets;
using System.Threading;

namespace GameFramework
{
    public class DisConnectNetWorkState : NetBaseState
    {
        public DisConnectNetWorkState(TcpSocket parent, NETWORKSTATE state = NETWORKSTATE.NS_DISCONNECTED)
            : base(parent, state)
        {
           
        }

        public override void Enter()
        { 
            base.Enter();  
            parent.Disconnect();            
        }

        public override void Exit()
        {
            base.Exit();          
        }      
    }
}