using Luthien;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConnectionService
{
    public partial class ConnectionControlService : ServiceBase
    {
        #region members

        Thread _workerThread;
        private ConnectionControl _controller;

        #endregion


        public ConnectionControlService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                _controller = new ConnectionControl();

                ThreadStart threadStart = new ThreadStart(_controller.DoWork);
                // start worker thread
                _workerThread = new Thread(threadStart);
                _workerThread.Start();
            }
            catch (Exception exception)
            {
                throw new Exception("Abort", exception);
            }
        }

        protected override void OnStop()
        {
            if (_workerThread != null)
            {
                _workerThread.Interrupt();
                _workerThread.Join(1000);

                LogFile.WriteMessageToLogFile("{0}: Stop", this.GetType().ToString());
            }
            else
                LogFile.WriteMessageToLogFile("workerThread == null");
        }
    }
}
