using UnityEngine;
using System.Collections;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace GameFramework
{
    public class NetThread
    {
        private Thread m_thread;
        private bool m_terminateFlag = false;
        private System.Object m_terminateFlagMutex;

        public void Run()
        {
            if (m_thread == null)
                return;

            m_thread.Start(this);
        }

        protected virtual void ThreadProc(object obj)
        {
        }

        public virtual void UpdateStream() { }

        public void WaitTermination()
        {
            if (m_thread == null)
                return;

            m_thread.Join();
        }

        public void SetTerminateFlag()
        {
            if (m_terminateFlagMutex == null)
                return;

            lock (m_terminateFlagMutex)
            {
                m_terminateFlag = true;
            }
        }

        protected bool IsTerminateFlagSet()
        {
            if (m_terminateFlagMutex == null)
                return true;

            lock (m_terminateFlagMutex)
            {
                return m_terminateFlag;
            }
        }

        public NetThread()
        {
            m_thread = new Thread(ThreadProc);
            m_thread.IsBackground = true;
            m_terminateFlagMutex = new System.Object();
        }
    }
}
