///Data         2018.02.08
///Description
///

using System;
using System.Net.Sockets;
using System.Threading;

namespace GameFramework
{
    public class ConnectingNetWorkState : NetBaseState
    {  
        protected TcpClient tcpClient;
        protected int timeout; // milliseconds
        protected bool connected;

        public ConnectingNetWorkState(TcpSocket parent, int timeout = 5000, NETWORKSTATE state = NETWORKSTATE.NS_CONNECTING)
            : base(parent, state)
        { 
            this.timeout = timeout;
        }

        public override void Enter()
        {
            base.Enter();            
            Connect();
        }

        public override void Update()
        {           
            base.Update();
        }

        public override void Exit()
        {
            base.Exit();
            tcpClient = null;
        }

        protected void Connect()
        {
            // kick off the thread that tries to connect
            connected = false;         
            Thread thread = null;
            try
            {
                thread = new Thread(new ThreadStart(BeginConnect));
                thread.IsBackground = true; // 作为后台线程处理
               
                thread.Start();              
                thread.Join(timeout);                            
            }
            catch (ArgumentNullException ex)
            {
                GDebug.LogError("Connect:new TcpClient: host name is null, " + ex.Message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                GDebug.LogError("Connect:new TcpClient: port error, " + ex.Message);
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode == SocketError.NetworkUnreachable)
                {
                    GDebug.LogError("-----客户端连接不上网络,断线重连处理-----");
                }
                else if (ex.SocketErrorCode == SocketError.ConnectionRefused)
                {
                    GDebug.LogError("-----服务器重启中,客户端返回登录界面------");
                    if (netState != NETWORKSTATE.NS_RETRY)
                        parent.ChangeState(NETWORKSTATE.NS_DISCONNECTED);
                }
                else
                {
                    GDebug.LogError("-----其它网络连接失败错误码------ : " + ex.SocketErrorCode);
                }
                GDebug.Log("Connect:new TcpClient: socket error1, " + ex.Message);
            }
            finally
            {
                thread.Abort();
                if (connected)
                {
                    parent.SetClient(tcpClient);  
                    parent.OnConnected();
                }  
                else
                {
                    if (netState != NETWORKSTATE.NS_RETRY)
                        parent.SetConnectErrState("连接失败");
                }
            }          
        }

        private void BeginConnect()
        {
            try
            {
                if (tcpClient != null)
                {
                    tcpClient.Close();
                    tcpClient = null;
                }
                tcpClient = new TcpClient(parent.GetIP(), parent.GetPort());
              
                connected = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }      
    }
}