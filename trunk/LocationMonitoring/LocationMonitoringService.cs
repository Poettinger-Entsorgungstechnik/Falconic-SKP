using System;
using System.ServiceProcess;
using System.Threading;

namespace LocationMonitoring
{
    public partial class LocationMonitoringService : ServiceBase
    {
        #region members

        private LocationMonitoring _LocationMonitoring;
        private Thread _workerThread;
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        #endregion


        public LocationMonitoringService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                _LocationMonitoring = new LocationMonitoring();

                ThreadStart threadStart = new ThreadStart(_LocationMonitoring.DoWork);
                // start worker thread
                _workerThread = new Thread(threadStart);
                _workerThread.Start();
            }
            catch (Exception exception)
            {
                Logger.Error("Exception: {0}", exception.Message);
            }
        }

        protected override void OnStop()
        {
            if (_workerThread != null)
            {
                _workerThread.Interrupt();
                _workerThread.Join(1000);

                Logger.Info("{0}: Stop", this.GetType().ToString());
            }
            else
                Logger.Error("workerThread == null");
        }
    }
}
