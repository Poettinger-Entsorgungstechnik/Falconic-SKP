using Luthien;
using FieldAreaNetwork;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Spatial;
using System.Data.SqlClient;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Net.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;
using System.Security.Cryptography;

using Falconic.Skp.Api.Client;
using Falconic.Messaging.DTOs;
using Falconic.Messaging.Exceptions;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System.Diagnostics;
using Mailjet.Client;
using Mailjet.Client.Resources;
using System.Net.Http;
using softaware.Authentication.Hmac.Client;
using System.Xml;

//   Version History:

//  1.2.0.06         -       25.01.2021

//                   -       Handle actual fillinglevel when permanent online is selected                     

//  1.2.0.05         -       01.03.2020

//                   -       Eco Clients - New API for CreditBalance and LanguageId                    

//  1.2.0.04         -       31.01.2020

//                   -       Finished ECO Clients

//  1.2.0.03         -       11.01.2020

//                            Added SMS Remote Support

//  1.2.0.02         -       07.12.2019

//                          - Add ECO Clients

//  1.2.0.01         -       30.11.2019

//                          - Added SMS Boxes Types to send SMS messages out with these boxes      

//  1.2.0.00         -       24.11.2019

//                          - Merged WIP SKP to Project      

//  1.1.0.00         -       12.11.2019 

//                           - Customer handling for machines with cardstart

//  1.0.1.17         -       20.04.2019 
//                          - On event code 2727, 2728,2729, 2730 and 4360 do no more call REST api 

//  1.0.1.16         -       05.04.2019 
//                          - Feature #731: Betriebsstundenthematik bei Modultausch oder Fehlerhaften Werten
//                          - Corrected error with external start (Bedienung_Lichtschranke)

//  1.0.1.15         -       18.03.2019 
//                          - # 673: Raised search area from 0.002F to 0.003F.

//  1.0.1.14         -       28.02.2019 
//                          - # 638: 2.0 some additional changes.

//  1.0.1.13         -       23.02.2019 
//                          - # 638: 2.0 Maschinen verlieren Maschinenausstattung.
//                          - New Api from Softaware


//  1.0.1.12         -       06.02.2019 
//                          - # 612: Machine Firmwareupdate triggered by server.


//  1.0.1.11         -       03.01.2019 
//                          - # 565: Store 2-0 Events also.
//                          - # 569: Removed Version information in message to container to avoid automatic firmware downgrade. 


namespace ConnectionService
{
    [Serializable]
    public class ConnectionControl : MarshalByRefObject
    {
        #region internal classes

        class DataImportExportClient
        {
            public string name;                 // operator name
            public int operatorId;             // operator identifier
            public string exportFormat;        // data export format .csv, .xml, .csv .xml
            public Thread workerThread;         // worker thread
            public SKP.CustomerImportExport.DataImportExport client;
        }

        class ContainerSettings
        {
            public int keepAliveInterval;
        }

        #endregion

        #region constants

        #endregion

        #region static objects

        bool _bIsDevelopmentService = false;
        static string _apiId = "d760404a-5ad1-4227-b885-62c5dff69368";
//        static string _apiIdDev = "SKP-API-Client-Dev";
        static string _apiIdTest = "SKP-API-Client-Test";
        static string _apiKey = "31w0XJzAVP3P6IeyoFNZxYF2Ll8UVmdeo/WeiVq5AMY=";
//        static string _apiKeyDev= "FI4SpZav02A2F0oPIp9AQa53Ge+wJP3BVTs2P12VghE=";
        static string _apiKeyTest = "G7FuovOAdd5IQ3XPz4m/LXeR9GiPX3xMcX/kdhVas/s=";

        static string _apiUrl = "https://falconic-skp-api.azurewebsites.net";
//        static string _apiUrlDev = "https://falconic-skp-api-dev.azurewebsites.net";
        static string _apiUrlTest = "https://falconic-skp-api-test.azurewebsites.net";


        public static string ActualModemFirmwareVersion = "unknown";

        public static ISkpApiClient SkpApiClient = null; // new SkpAPIv10(new Uri(_apiUrl), new ApiKeyDelegatingHandler(_apiId, _apiKey));

        #endregion

        #region Members

        private ArrayList _clients = new ArrayList(100);	        // list of actual connected clients, allow 100 clients as default, can be increased
        private ArrayList _ECOClients = new ArrayList(100);	        // list of actual connected clients, allow 100 clients as default, can be increased
        private ArrayList _SMSClients = new ArrayList(100);         // list of actual connected clients, allow 100 clients as default, can be increased
        private ArrayList _imexport = new ArrayList();
        private Hashtable _containerSettingsOverride = new Hashtable();

        private Mutex _emailMutex = new Mutex();
        private DateTime _tLastLocationCheck = DateTime.Now.Subtract(new TimeSpan(0,1,0,0,0));
        private Hashtable _lastMonitoringMessage = new Hashtable();
        private FileSystemWatcher _fsWatcher = new FileSystemWatcher();
        private FileSystemWatcher _fsWatcherFirmwareELS61TLinux = new FileSystemWatcher();
        private FileSystemWatcher _fsWatcherContainerSettingsOverride = new FileSystemWatcher();

        private String _fwVersionELS61TLinux;

        private Dictionary<int, Location> _2DotZero_Containers = new Dictionary<int, Location>();
        private Mutex _2DotZeroContainerMutex = new Mutex();
        private Mutex _ContainerSettingsOverridMutex = new Mutex();

        private DateTime _lastTimeFsWatcherCalled;
        private Dictionary<string, Dictionary<string, string>> _lngDictionary = new Dictionary<string, Dictionary<string, string>>();
        /// <summary>
        /// WIP SKP service Methods can be removed after wip is merged to falconic
        /// </summary>
        private SKP.IWIPDataAccess _data_access = null;
        private CCS.CCSDataAccess _data_access_ccs = null;
        private ArrayList _WIPClients = new ArrayList(100);         // list of actual connected WIP clients, allow 100 clients as default, can be increased
        /// <summary>
        /// End WIP SKP service Methods
        /// </summary>

        #endregion

        #region Azure Message Queue

        private static QueueClient queueClient;

        private static void RegisterOnMessageHandlerAndReceiveMessages()
        {
            // Configure the message handler options in terms of exception handling, number of concurrent messages to deliver, etc.
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                // Maximum number of concurrent calls to the callback ProcessMessagesAsync(), set to 1 for simplicity.
                // Set it according to how many messages the application wants to process in parallel.
                MaxConcurrentCalls = 1,

                // Indicates whether the message pump should automatically complete the messages after returning from user callback.
                // False below indicates the complete operation is handled by the user callback as in ProcessMessagesAsync().
                AutoComplete = false
            };

            // Register the function that processes messages.
            queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        private static async Task ProcessMessagesAsync(Microsoft.Azure.ServiceBus.Message message, CancellationToken token)
        {
            try
            {
                // Process the message.
                var messageBody = Encoding.UTF8.GetString(message.Body);

                // Console.WriteLine($"Received message: SequenceNumber: {message.SystemProperties.SequenceNumber} Body: {messageBody}");
                var queueMessage = JsonConvert.DeserializeObject<QueueMessage>(messageBody);
                if (queueMessage == null)
                {
                    throw new UnknownMessageContentException("Content: " + messageBody);
                }

                if (queueMessage.Type == QueueMessageType.ContainerLocationAssignmentChangedMessage)
                {
                    var locationChangedMessage = JsonConvert.DeserializeObject<ContainerLocationAssignmentChangedMessage>(queueMessage.InnerMessage);
                    LogFile.WriteMessageToLogFile(
                        $"[{locationChangedMessage.UtcTimestamp}]: " +
                        $"Container location assignment changed for containerId {locationChangedMessage.ContainerId} " +
                        $"(GSMNumber: {locationChangedMessage.GsmNumber}) to locationId {locationChangedMessage.LocationId}");

                    ArrayList users = new ArrayList();
                    FieldAreaNetwork.AlertingUser alertUser = new FieldAreaNetwork.AlertingUser();
                        
                    alertUser.ClientName = "SKP";
                    alertUser.TelephoneNumber = locationChangedMessage.GsmNumber;
                    alertUser.Flags = (int)ALERTING_FLAGS.SMS_ENABLED;
                    alertUser.EmailAddress = "";
                    alertUser.Name = "Unknown";
                    users.Add(alertUser);

                    FieldAreaNetwork.AlertingControl.AddAlarm("alarm.wallner-automation.com", users, "WIP", "Report", false, 0);
                }

                // Complete the message so that it is not received again.
                // This can be done only if the queue Client is created in ReceiveMode.PeekLock mode (which is the default).
                await queueClient.CompleteAsync(message.SystemProperties.LockToken);

                // Note: Use the cancellationToken passed as necessary to determine if the queueClient has already been closed.
                // If queueClient has already been closed, you can choose to not call CompleteAsync() or AbandonAsync() etc.
                // to avoid unnecessary exceptions.
            }
            catch (Exception excp)
            {
                LogFile.WriteErrorToLogFile("Exception ({0} while trying to process message", excp.Message);
            }
        }

        // Use this handler to examine the exceptions received on the message pump.
        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            LogFile.WriteErrorToLogFile($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            LogFile.WriteErrorToLogFile("Exception context for troubleshooting:");
            LogFile.WriteErrorToLogFile($"- Endpoint: {context.Endpoint}");
            LogFile.WriteErrorToLogFile($"- Entity Path: {context.EntityPath}");
            LogFile.WriteErrorToLogFile($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }

        #endregion

        #region Properties

        /// <summary>
        /// List of connected clients
        /// </summary>
        public ArrayList ConnectedClients
        {
            get { return _clients; }
        }

        public ArrayList ConnectedWIPClients
        {
            get { return _WIPClients; }
        }

        public ArrayList ConnectedECOClients
        {
            get { return _ECOClients; }
        }

        public CCS.CCSDataAccess DataAccessCCS
        {
            get { return _data_access_ccs;  }
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Override of MarshalByRefObject's implementation. Specify to leave the singelton remoting object in memory till the appdomain shuts down.
        /// </summary>
        /// <returns></returns>
        public override Object InitializeLifetimeService()
        {
            return null;
        }

        #endregion

        #region Constructor

        public ConnectionControl()
        {
            int processId = System.Diagnostics.Process.GetCurrentProcess().Id;
            String query = "SELECT * FROM Win32_Service where ProcessId = " + processId;
            String serviceName = "";
            String serviceBusConnectionString = "Endpoint=sb://falconic.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=s3/BE9xHzVzMaG91Omzay9+erI8v3fnI3iY21rkf5go=";
            String queueName = "skp-notification";

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;

            LogFile.WriteMessageToLogFile("ConnectionControl Version: {0}, ProcessId: {1}", version, processId);

            System.Management.ManagementObjectSearcher searcher =
                    new System.Management.ManagementObjectSearcher(query);

            foreach (System.Management.ManagementObject queryObj in searcher.Get())
            {
                serviceName =  queryObj["Name"].ToString();
            }

            if (serviceName.IndexOf("Dev") != -1)
            {
//                _apiUrl = _apiUrlDev;
                _apiUrl = _apiUrlTest;

//                _apiId = _apiIdDev;
                _apiId = _apiIdTest;

//                _apiKey = _apiKeyDev;
                _apiKey = _apiKeyTest;

                queueName = "skp-notification-test";

                _bIsDevelopmentService = true;
            }


            try
            {
                LogFile.WriteMessageToLogFile("Create Message Queue: {0}", queueName);

                queueClient = new QueueClient(serviceBusConnectionString, queueName);

                // see https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-dotnet-get-started-with-queues#4-receive-messages-from-the-queue
                RegisterOnMessageHandlerAndReceiveMessages();
            }
            catch (Exception excp)
            {
                LogFile.WriteErrorToLogFile("Exception: {0} while trying to connect service bus queue: {1}", excp.Message, queueName);
            }

            LogFile.WriteMessageToLogFile("Create SkpAPIv10 object to: {0}", _apiUrl);

            try
            {
                var httpClient = new HttpClient(new ApiKeyDelegatingHandler(_apiId, _apiKey, new HttpClientHandler()));
                SkpApiClient = new SkpApiClient(_apiUrl, httpClient);
            }
            catch (Exception excp)
            {
                LogFile.WriteErrorToLogFile("Exception: {0} while trying to establish connection to SkpAPIv10 on: {1}", excp.Message, _apiUrl);
            }

            LogFile.WriteMessageToLogFile("{0} Start file system Watcher", this.ToString());

            // start file watcher for offline machines
            _fsWatcher.Path = "c:\\SKP\\2.0 Machines\\";
            _fsWatcher.NotifyFilter = NotifyFilters.LastWrite;
            _fsWatcher.Filter = "*.txt";
            _fsWatcher.Changed += FsWatcher_Changed;
            _fsWatcher.EnableRaisingEvents = true;

            _lastTimeFsWatcherCalled = DateTime.Now - new TimeSpan(0, 0, 10);
            // read file
            FsWatcher_Changed(this, new FileSystemEventArgs(WatcherChangeTypes.Changed, _fsWatcher.Path, "database.txt"));

            try
            {
                _fsWatcherFirmwareELS61TLinux.Path = "C:\\SKP\\Firmware\\ECO-ELS61T-Linux\\";
                _fsWatcherFirmwareELS61TLinux.NotifyFilter = NotifyFilters.LastWrite;
                _fsWatcherFirmwareELS61TLinux.Filter = "*.txt";
                _fsWatcherFirmwareELS61TLinux.Changed += _fsWatcherFirmwareELS61TLinux_Changed;
                _fsWatcherFirmwareELS61TLinux.EnableRaisingEvents = true;
                // read file
                _fsWatcherFirmwareELS61TLinux_Changed(this, new FileSystemEventArgs(WatcherChangeTypes.Changed, _fsWatcherFirmwareELS61TLinux.Path, "ActualVersion.txt"));
            }
            catch (Exception excp)
            {
                LogFile.WriteErrorToLogFile("!! Exception: {0} !!", excp.Message);
                LogFile.WriteErrorToLogFile("{0}", excp.StackTrace);
            }

            try
            {
                _fsWatcherContainerSettingsOverride.Path = "C:\\SKP\\";
                _fsWatcherContainerSettingsOverride.NotifyFilter = NotifyFilters.LastWrite;
                _fsWatcherContainerSettingsOverride.Filter = "*.txt";
                _fsWatcherContainerSettingsOverride.Changed += _fsWatcherContainerSettingsOverride_Changed;
                _fsWatcherContainerSettingsOverride.EnableRaisingEvents = true;
                // read file
                _fsWatcherContainerSettingsOverride_Changed(this, new FileSystemEventArgs(WatcherChangeTypes.Changed, _fsWatcherContainerSettingsOverride.Path, "ContainerSettingsOverride.txt"));
            }
            catch (Exception excp)
            {
                LogFile.WriteErrorToLogFile("!! Exception: {0} !!", excp.Message);
                LogFile.WriteErrorToLogFile("{0}", excp.StackTrace);
            }

            /// <summary>
            /// WIP SKP service Methods can be removed after wip is merged to falconic
            /// </summary>
            string WIP_ConnectionString = "Server=tcp:poettingerwip.database.windows.net,1433;Initial Catalog=WIP;Persist Security Info=False;User ID=wip;Password=9xWQyoatmZ3eH64ef5iS;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";
            string CCS_ConnectionString = "Server=tcp:poettingerwip.database.windows.net,1433;Initial Catalog=CCS;Persist Security Info=False;User ID=wip;Password=9xWQyoatmZ3eH64ef5iS;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";

            LogFile.WriteMessageToLogFile("{0} Start data access", this.ToString());

            try
            {
                _data_access = new SKP.WIPLocalDataAccess(WIP_ConnectionString);
            }
            catch (Exception excp)
            {
                LogFile.WriteErrorToLogFile("Exception: {0} while trying to instantiate WIP data access!", excp.Message);
            }

            try
            {
                _data_access_ccs = new CCS.CCSDataAccess(CCS_ConnectionString);
            }
            catch (Exception excp)
            {
                LogFile.WriteErrorToLogFile("Exception: {0} while trying to instantiate WIP data access!", excp.Message);
            }

            LogFile.WriteMessageToLogFile("{0} Send Start Email", this.ToString());

            InitDataImportExport();

            try
            {
                ArrayList emailRecipients = new ArrayList();
                AlertingUser user = new AlertingUser();
                user.EmailAddress = "a.erler@weitblick-resources.at";
                user.Name = "Andreas Erler";

                emailRecipients.Add(user);

                SendMail("Falconic Service Started", "Falconic", emailRecipients).Wait();
            }
            catch (Exception excp)
            {
                LogFile.WriteErrorToLogFile("{0} Exception ({1}) while trying to send email!", excp.Message);
            }

            LogFile.WriteMessageToLogFile("{0} Startup complete :-)", this.ToString());
        }

        private void _fsWatcherContainerSettingsOverride_Changed(object sender, FileSystemEventArgs e)
        {
            LogFile.WriteMessageToLogFile("ContainerSettingsOverride changed: {0}, {1}, {2}", e.FullPath, e.Name, e.ChangeType);

            if (e.Name == "ContainerSettingsOverride.txt" && e.ChangeType == WatcherChangeTypes.Changed)
            {
                // wait a bit till editor closed all resources
                Thread.Sleep(100);

                _ContainerSettingsOverridMutex.WaitOne();
                _containerSettingsOverride.Clear();

                try
                {
                    // read the whole file into a string array                        
                    StreamReader sr = new StreamReader(e.FullPath);
                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        try
                        {
                            string[] lineToks = line.Split(new char[] { ',' });

                            if (line == "") continue;
                            if (line.StartsWith("#"))
                                continue;

                            int containerId = Convert.ToInt32(lineToks[0].Trim());
                            int keepAliveInterval = Convert.ToInt32(lineToks[1].Trim());

                            ContainerSettings containerSettings = new ContainerSettings();
                            containerSettings.keepAliveInterval = keepAliveInterval;

                            _containerSettingsOverride[containerId] = containerSettings;

                            LogFile.WriteMessageToLogFile("{0}: Set KeepAliveinterval: {1}", String.Format("ContainerID {0}", containerId), keepAliveInterval);

                        }
                        catch (Exception excp)
                        {
                            LogFile.WriteErrorToLogFile("Exception ({0}) while trying to parse line: ({1})", excp.Message, line);
                        }
                    }

                    sr.Close();
                }
                catch (Exception excp)
                {
                    LogFile.WriteErrorToLogFile("Exception ({0}) while trying to read container settings override", excp.Message);
                }
                finally
                {
                    _ContainerSettingsOverridMutex.ReleaseMutex();
                }
            }
        }

        private void _fsWatcherFirmwareELS61TLinux_Changed(object sender, FileSystemEventArgs e)
        {
            LogFile.WriteMessageToLogFile("Firmwareversion for ELS61T (Linux) changed: {0}, {1}, {2}", e.FullPath, e.Name, e.ChangeType);

            if (e.Name == "ActualVersion.txt" && e.ChangeType == WatcherChangeTypes.Changed)
            {
                // wait a bit till editor closed all resources
                Thread.Sleep(100);

                try
                {
                    // read the whole file into a string array                        
                    StreamReader sr = new StreamReader(e.FullPath);
                    FwVersionELS61TLinux = sr.ReadLine().Trim();
                    sr.Close();

                    LogFile.WriteMessageToLogFile("Actual firmwareversion which should be installed on ELS61T (Linux) modems: ({0})", FwVersionELS61TLinux);
                }
                catch (Exception excp)
                {
                    LogFile.WriteErrorToLogFile("Exception ({0}) while trying to access firmware version file", excp.Message);
                }
            }
        }

        private void FsWatcher_Changed(object sender, System.IO.FileSystemEventArgs e)
        {
            TimeSpan span = DateTime.Now - _lastTimeFsWatcherCalled;

            if (span.TotalMilliseconds < 1000)
            {
                LogFile.WriteMessageToLogFile("Ignore multiple filewatcher events");
                return;

            }

            _lastTimeFsWatcherCalled = DateTime.Now;

            LogFile.WriteMessageToLogFile("2.0 Database changed: {0}, {1}, {2}", e.FullPath, e.Name, e.ChangeType);

            if (e.Name == "database.txt" && e.ChangeType == WatcherChangeTypes.Changed)
            {
                // wait a bit till editor closed all resources
                Thread.Sleep(100);

                try
                {
                    _2DotZeroContainerMutex.WaitOne();
                    _2DotZero_Containers.Clear();

                    try
                    {
                        // read the whole file into a string array                        
                        StreamReader sr = new StreamReader(e.FullPath);
                        string line;

                        while ((line = sr.ReadLine()) != null)
                        {
                            try
                            {
                                string[] lineToks = line.Split(new char[] { ',' });

                                if (line == "") continue;
                                if (line.StartsWith("#"))
                                {
                                    // we also set here for the moment the firmwareversion which should be installed on all machines
                                    int pos = line.IndexOf("ActualFirmwareVersion");
                                    if (pos != -1)
                                    {
                                        pos = line.IndexOf(":", pos);
                                        if (pos != -1)
                                        {
                                            ActualModemFirmwareVersion = line.Substring(pos + 1).Trim();
                                        }
                                    }
                                    continue;
                                }

                                int containerId = Convert.ToInt32(lineToks[0].Trim());
                                int pressStrokes = Convert.ToInt32(lineToks[1].Trim());
                                int pressStopAtFront = Convert.ToInt32(lineToks[2].Trim());
                                int nearlyFull = Convert.ToInt32(lineToks[3].Trim());
                                int full = Convert.ToInt32(lineToks[4].Trim());
                                int workload = Convert.ToInt32(lineToks[5].Trim());

                                Location loc = new Location();

                                loc.PressStrokes = pressStrokes;
                                loc.PressPosition = (pressStopAtFront == 1);
                                loc.FullWarningLevel = nearlyFull;
                                loc.FullErrorLevel = full;
                                loc.MachineUtilization = workload;
                                loc.IsLiftTiltEquipped = false;
                                loc.IsRetroKitEquipped = false;

                                LogFile.WriteMessageToLogFile("{0}: Add data from 2.0 database: {1}, {2}, {3}, {4}, {5}", String.Format("ContainerID {0}", containerId),
                                    loc.PressStrokes, loc.PressPosition, loc.FullWarningLevel, loc.FullErrorLevel,
                                    loc.MachineUtilization);

                                _2DotZero_Containers.Add(containerId, loc);
                            }
                            catch (Exception excp)
                            {
                                LogFile.WriteErrorToLogFile("Exception ({0}) while trying to parse line: ({1})", excp.Message, line);
                            }
                        }

                        sr.Close();

                        LogFile.WriteMessageToLogFile("Actual firmwareversion which should be installed on machines: ({0})", ActualModemFirmwareVersion);
                    }
                    catch (Exception excp)
                    {
                        LogFile.WriteErrorToLogFile("Exception ({0}) while trying to read 2.0 database", excp.Message);
                    }
                }
                catch (Exception excp)
                {
                    LogFile.WriteErrorToLogFile("Exception ({0}) while trying to access 2.0 dictionary", excp.Message);
                }
                finally
                {
                    _2DotZeroContainerMutex.ReleaseMutex();
                }
            }
        }

        public Location Get2DotZeroLocation(int containerId)
        {
            Location retval = null;

            try
            {
                _2DotZeroContainerMutex.WaitOne();

                if (_2DotZero_Containers.ContainsKey(containerId))
                    retval = _2DotZero_Containers[containerId];
            }
            catch (Exception excp)
            {
                LogFile.WriteErrorToLogFile("Exception ({0}) while trying to access 2.0 dictionary", excp.Message);
            }
            finally
            {
                _2DotZeroContainerMutex.ReleaseMutex();
            }

            return retval;
        }

        public int GetKeepAliveInterval(int containerId)
        {
            int retval = 3 * 60;

            try
            {
                _ContainerSettingsOverridMutex.WaitOne();

                if (_containerSettingsOverride.ContainsKey(containerId))
                {
                    ContainerSettings contSettings = (ContainerSettings)_containerSettingsOverride[containerId];

                    retval = contSettings.keepAliveInterval;
                }
            }
            catch (Exception excp)
            {
                LogFile.WriteErrorToLogFile("Exception ({0}) while trying to access container settings dictionary", excp.Message);
            }
            finally
            {
                _ContainerSettingsOverridMutex.ReleaseMutex();
            }

            return retval;
        }

        #endregion

        #region static methods

        public static async Task SendMail(string message, string subject, ArrayList recipients)
        {
            try
            {
//                MailjetClient client = new MailjetClient("c0719838f86777631495b01e3d9fb47f", "83cfc1e5f8c84cb2a549f2e9c386c3fc");
                MailjetClient client = new MailjetClient("c0719838f86777631495b01e3d9fb47f", "50a455c21f8903cb3b0d754e3f44c49c");
                JArray jRecepients = new JArray();

                for (int i = 0; i < recipients.Count; i++)
                {
                    AlertingUser user = (AlertingUser)recipients[i];

                    jRecepients.Add(new JObject { { "Email", user.EmailAddress } });
                }

                MailjetRequest request = new MailjetRequest
                {
                    Resource = Send.Resource,
                }
                   //                   .Property(Send.FromEmail, "elch@aon.at")
                   .Property(Send.FromEmail, "falconic@poettinger.at")
                   .Property(Send.FromName, "FALCONIC")
                   .Property(Send.Subject, subject)
                   .Property(Send.TextPart, message)
                    //               .Property(Send.HtmlPart, "<h3>Dear passenger, welcome to Mailjet!</h3><br />May the delivery force be with you!")
                    //.Property(Send.Recipients, new JArray {
                    //    new JObject {
                    //        {"Email", "andreas.erler@ocilion.com"}
                    //        }
                    //    });

                    .Property(Send.Recipients, jRecepients);

                MailjetResponse response = await client.PostAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    LogFile.WriteMessageToLogFile("Total: {0}, Count: {1}\n", response.GetTotal(), response.GetCount());
                }
                else
                {
                    LogFile.WriteErrorToLogFile("StatusCode: {0}\n", response.StatusCode);
                    LogFile.WriteErrorToLogFile("ErrorInfo: {0}\n", response.GetErrorInfo());
                    LogFile.WriteErrorToLogFile("ErrorMessage: {0}\n", response.GetErrorMessage());
                }
            }
            catch (Exception excp)
            {
                LogFile.WriteErrorToLogFile(excp.Message);
            }
        }

        // since 10.08.2018 only SMS Messages are sent by us
        public static void DoAlerting(int containerId, int type, string smsMessage, string emailMessage, string subject)
        {
            ArrayList users = new ArrayList();

            foreach (var notification in ConnectionControl.SkpApiClient.GetNotificationContacts(containerId, type))
            {
                FieldAreaNetwork.AlertingUser alertUser = new FieldAreaNetwork.AlertingUser();

                alertUser.ClientName = "SKP";
                alertUser.TelephoneNumber = notification.Contact;
                alertUser.Flags = (int)ALERTING_FLAGS.SMS_ENABLED;
                alertUser.EmailAddress = "";
                alertUser.Name = "Unknown";

                users.Add(alertUser);
                LogFile.WriteMessageToLogFile("ContainerID {0}: Send SMS for user: {1} to number: {2}", containerId, alertUser.Name, alertUser.TelephoneNumber);
            }

            if (users.Count > 0)
            {
                if (smsMessage.Length > 160)
                    smsMessage = smsMessage.Substring(0, 160);

                FieldAreaNetwork.AlertingControl.AddAlarm("alarm.wallner-automation.com", users, "WIP", smsMessage, false, 0);
            }
        }

        #endregion

        #region Methods

        private bool InitDataImportExport()
        {
            bool retval = false;
            // read configuration
            string fileName = System.AppDomain.CurrentDomain.BaseDirectory + "DataImportExport.config";

            XmlTextReader reader = null;
            string elementName = "";
            DataImportExportClient client = null;

            try
            {
                // Load the reader with the configuration file and ignore all white space nodes.         
                reader = new XmlTextReader(fileName);
                reader.WhitespaceHandling = WhitespaceHandling.None;

                // Parse the file and display each of the nodes.
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (reader.Name == "plugin")
                            {
                                client = new DataImportExportClient();
                                _imexport.Add(client);
                            }
                            else
                                elementName = reader.Name;
                            break;

                        case XmlNodeType.Text:
                            if (elementName == "NAME")
                                client.name = reader.Value;
                            else if (elementName == "OPERATOR_ID")
                                client.operatorId = System.Convert.ToInt32(reader.Value);
                            else if (elementName == "EXPORT_FORMAT")
                                client.exportFormat = reader.Value;
                            break;

                        case XmlNodeType.EndElement:
                            if (reader.Name == "plugin")
                            {
                                // start data import export module thread with operator id 
                                client.client = new SKP.CustomerImportExport.DataImportExport(this, client.operatorId, client.exportFormat);
                                ThreadStart threadStart = new ThreadStart(client.client.DoWork);
                                client.workerThread = new Thread(threadStart);
                                client.workerThread.Start();

                                LogFile.WriteMessageToLogFile("{0}: Start data import export workerthread for client: {1}", this.GetType().ToString(), client.name);
                            }
                            break;
                    }
                }
            }
            catch (Exception excp)
            {
                LogFile.WriteErrorToLogFile("Exception {1} while trying to read data import export configuration", excp.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return retval;
        }

        public void SendSMS(string text, string telnumber)
        {
            ArrayList users = new ArrayList();

            FieldAreaNetwork.AlertingUser alertUser = new FieldAreaNetwork.AlertingUser();
            alertUser.ClientName = "SKP";
            alertUser.TelephoneNumber = telnumber;
            alertUser.Flags = (int)ALERTING_FLAGS.SMS_ENABLED;
            alertUser.EmailAddress = "";
            alertUser.Name = "Unknown";

            users.Add(alertUser);
            LogFile.WriteMessageToLogFile("Send SMS for user: {0} to number: {1}", alertUser.Name, alertUser.TelephoneNumber);
            if (text.Length > 160)
                text = text.Substring(0, 160);
            FieldAreaNetwork.AlertingControl.AddAlarm("alarm.wallner-automation.com", users, "WIP", text, false, 0);
        }

        /// <summary>
        /// WIP data access object
        /// </summary>
        public SKP.IWIPDataAccess DataAccess
        {
            get { return _data_access; }
        }

        public string FwVersionELS61TLinux { get => _fwVersionELS61TLinux; set => _fwVersionELS61TLinux = value; }

        /// <summary>
        /// Remove given client connection
        /// </summary>
        /// <param name="client"></param>
        public void RemoveClient(ClientConnection client)
        {
            try
            {
                lock (_clients.SyncRoot)
                {
                    _clients.Remove(client);
                }

                LogFile.WriteMessageToLogFile("{0} Remove Client. Clients left: {1}.", client.Name, _clients.Count);
            }
            catch (Exception excp)
            {
                LogFile.WriteErrorToLogFile("{0}: Error while removing client {1}\nException: {2}", this.GetType().ToString(),
                    client.Name, excp.Message);
            }
        }

        public void RemoveWIPClient(SKP.ClientConnection client)
        {
            try
            {
                lock (_WIPClients.SyncRoot)
                {
                    _WIPClients.Remove(client);
                }

                LogFile.WriteMessageToLogFile("{0} Remove WIP Client. WIP Clients left: {1}.", client.Name, _WIPClients.Count);
            }
            catch (Exception excp)
            {
                LogFile.WriteErrorToLogFile("{0}: Error while removing WIP client {1}\nException: {2}", this.GetType().ToString(),
                    client.Name, excp.Message);
            }
        }

        public void RemoveSMSClient(SMSClientConnection client)
        {
            try
            {
                lock (_SMSClients.SyncRoot)
                {
                    _SMSClients.Remove(client);
                }

                LogFile.WriteMessageToLogFile("{0} Remove SMS Client. SMS Clients left: {1}.", client.Name, _clients.Count);
            }
            catch (Exception excp)
            {
                LogFile.WriteErrorToLogFile("{0}: Error while removing SMS client {1}\nException: {2}", this.GetType().ToString(),
                    client.Name, excp.Message);
            }
        }

        public void RemoveECOClient(ECO.ClientConnection client)
        {
            try
            {
                lock (_ECOClients.SyncRoot)
                {
                    _ECOClients.Remove(client);
                }

                LogFile.WriteMessageToLogFile("{0} Remove ECO Client. ECO Clients left: {1}.", client.Name, _clients.Count);
            }
            catch (Exception excp)
            {
                LogFile.WriteErrorToLogFile("{0}: Error while removing ECO client {1}\nException: {2}", this.GetType().ToString(),
                    client.Name, excp.Message);
            }
        }

        public string GetTranslation(string message, string language)
        {
            // check for language
            if (!_lngDictionary.ContainsKey(language))
                _lngDictionary[language] = new Dictionary<string, string>();

            if (_lngDictionary[language].ContainsKey(message))
                return _lngDictionary[language][message];
            else
                _lngDictionary[language][message] = SkpApiClient.GetTranslation(message, language);

            return _lngDictionary[language][message];
        }

        /// <summary>
        /// Do the service's job
        /// </summary>
        public void DoWork()
        {
            TcpListener tcpl = null;

            LogFile.WriteMessageToLogFile("{0}: Listener thread started", this.GetType().ToString());

            try
            {
                int port = 820;
                if (_bIsDevelopmentService)
                    port = 821;

                tcpl = new TcpListener(IPAddress.Any, port);
                tcpl.Start();

                try
                {
                    LogFile.WriteMessageToLogFile("{0}: Wait for clients to connect", this.GetType().ToString());

                    do
                    {
                        // check for new clients
                        if (tcpl.Pending())
                        {
                            TcpClient tc = tcpl.AcceptTcpClient();
                            Socket socket = tc.Client;
                            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true); // not sure but i think this does not work
                            IPEndPoint ipEP = (IPEndPoint)socket.RemoteEndPoint;
                            int length = 0;
                            int offset = 0;
                            byte[] frame = null;
                            string firstLineReceived = "";

                            for (int i = 0; i < 30; i++)
                            {
                                if (socket.Poll(100000, SelectMode.SelectRead))
                                {
                                    if (socket.Available > 0)
                                    {
                                        if ((length = socket.Available) > 0)
                                        {
                                            byte[] bytes = new byte[length + offset];

                                            if (offset > 0)
                                                Array.Copy(frame, 0, bytes, 0, offset);

                                            // get new bytes
                                            socket.Receive(bytes, offset, length, SocketFlags.None);

                                            frame = bytes;
                                            offset += length;

                                            if (offset >= 4)
                                            {
                                                short frame_length = (short)(frame[2] << 8 | frame[3] + 6);
                                                // additionally we can do a comparision of checksums,
                                                // but in this case a higher level is done already by the sockets layer
                                                // so if last char is an ETX and the frame_length matches we consider a valid frame
                                                if (offset == frame_length && frame[offset - 1] == 0x03)
                                                {
                                                    offset = 0;
                                                    firstLineReceived = Encoding.ASCII.GetString(frame, 4, frame_length - 6);
                                                    LogFile.WriteMessageToLogFile("{0} Received: {1}", this.GetType().ToString(), firstLineReceived);
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        LogFile.WriteMessageToLogFile("{0} Client connection has been closed remotely", this.GetType().ToString());
                                        break;
                                    }
                                }
                            }

                            // check for ECO-WIP clients
                            if (firstLineReceived.IndexOf("ECO-") != -1 || firstLineReceived.IndexOf("SMS-Remote") != -1 || firstLineReceived.IndexOf("BWASTE-") != -1)
                            {
                                bool bIsInFalconic = false;
                                string iccId = firstLineReceived.Substring(5);
                                int posComma = iccId.IndexOf(',');
                                if (posComma != -1)
                                {
                                    iccId = iccId.Substring(0, posComma);
                                    LogFile.WriteMessageToLogFile("Try to get container parameters for IccId: {0}", iccId);
                                    try
                                    {
                                        ContainerParamsDto contParams = (ContainerParamsDto)ConnectionControl.SkpApiClient.GetContainerParams(iccId);
                                        bIsInFalconic = true;
                                    }
                                    catch (Exception)
                                    {
//                                        LogFile.WriteErrorToLogFile("{0} Exception ({1}) while tring to ...", this.ToString(), excp.Message);
//                                        LogFile.WriteMessageToLogFile("{0}", excp.StackTrace);
                                    }
                                }

                                if (bIsInFalconic)
                                {
                                    ECO.ClientConnection conn = new ECO.ClientConnection(firstLineReceived);
                                    conn.TcpClient = tc;
                                    conn.Controller = this;
                                    lock (_ECOClients.SyncRoot)
                                    {
                                        _ECOClients.Add(conn);
                                    }
                                    conn.Start();
                                    LogFile.WriteMessageToLogFile("{0}: ECO (Falconic) Client Nr: {1} with ip {2} want to connect.", this.GetType().ToString(), _ECOClients.Count, ipEP.Address.ToString());
                                }
                                else
                                {
                                    SKP.ClientConnection conn = new SKP.ClientConnection(firstLineReceived);
                                    conn.TcpClient = tc;
                                    conn.Controller = this;
                                    lock (_WIPClients.SyncRoot)
                                    {
                                        _WIPClients.Add(conn);
                                    }
                                    conn.Start();
                                    LogFile.WriteMessageToLogFile("{0}: WIP Client Nr: {1} with ip: {2} want to connect.", this.GetType().ToString(), _WIPClients.Count, ipEP.Address.ToString());
                                }
                            }
                            else if (firstLineReceived.IndexOf("SMS-Machine") != -1)
                            {
                                SMSClientConnection conn = new SMSClientConnection(firstLineReceived);
                                conn.TcpClient = tc;
                                conn.Controller = this;
                                lock (_SMSClients.SyncRoot)
                                {
                                    _SMSClients.Add(conn);
                                }
                                conn.Start();
                                LogFile.WriteMessageToLogFile("{0}: SMS Client Nr: {1} with ip: {2} want to connect.", this.GetType().ToString(), _SMSClients.Count, ipEP.Address.ToString());       
                            }
                            else
                            {
                                ClientConnection conn = new ClientConnection(firstLineReceived);
                                conn.TcpClient = tc;
                                conn.Controller = this;
                                lock (_clients.SyncRoot)
                                {
                                    _clients.Add(conn);
                                }
                                conn.Start();
                                LogFile.WriteMessageToLogFile("{0}: Client Nr: {1} with ip: {2} want to connect.", this.GetType().ToString(), _clients.Count, ipEP.Address.ToString());
                            }
                        }

//                        LocationMonitoring();

                        Thread.Sleep(10);

                    } while (true);
                }
                catch (Exception)
                {
                    LogFile.WriteMessageToLogFile("{0}: Listeningthread mainroutine stopped", this.GetType().ToString());
                }
            }
            catch (SocketException socketError)
            {
                if (socketError.ErrorCode == 10048)
                {
                    LogFile.WriteErrorToLogFile("{0}: Connection to this port failed. There is another server is listening on this port", this.GetType().ToString());
                }
                else
                    LogFile.WriteErrorToLogFile(socketError.Message);
            }
            finally
            {
                // stop listener
                if (tcpl != null) tcpl.Stop();

                // stop all client threads
                foreach (object x in _clients)
                {
                    ((ClientConnection)x).Stop();
                }

                // stop all wip client threads
                foreach (object x in _WIPClients)
                {
                    ((SKP.ClientConnection)x).Stop();
                }

                // stop all eco client threads
                foreach (object x in _ECOClients)
                {
                    ((ECO.ClientConnection)x).Stop();
                }

                // stop all sms client threads
                foreach (object x in _SMSClients)
                {
                    ((SMSClientConnection)x).Stop();
                }
            }
        }

        #endregion
    }

    [Serializable]
    public class AlertingUser
    {
        #region members

        private int _id;
        private string _name;                   // name of alerted person
        private string _telephoneNumber;        // telephone number of alerted person
        private string _emailAddress;           // email address of alerted person
        private int _flags;                     // different alerting options (SMS,EMAIL,SPEECH)

#endregion

        #region properties

        /// <summary>
        /// name of alerted person
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Telephone number of alerted person
        /// </summary>
        public string TelephoneNumber
        {
            get { return _telephoneNumber; }
            set { _telephoneNumber = value; }
        }

        /// <summary>
        /// email address of alerted person
        /// </summary>
        public string EmailAddress
        {
            get { return _emailAddress; }
            set { _emailAddress = value; }
        }

        /// <summary>
        /// different alerting options (SMS,EMAIL,SPEECH)
        /// </summary>
        public int Flags
        {
            get { return _flags; }
            set { _flags = value; }
        }

        public int Id { get => _id; set => _id = value; }

#endregion
    }

    public enum eCARD_TYPE
    {
        NONE,
        PREPAID,
        INVOICE,
    }

    [Serializable]
    public class CustomerAccessCard
    {
        #region members

        private UInt32 _customerNumber;             // Legacy: can be zero. If zero cardserialnumber should be set
        private UInt32 _releaseNumber;              // Legacy: card release number?? not sure about this
        private UInt32 _locationGroupMask;          // Legacy: for check 
        private UInt32 _minimumAmount;              // Minimum amount [1/100] that should be on a prepaid card for successful access
        private DateTime _releaseDate;              // Date at which card was handed out
        private eCARD_TYPE _cardChargingType;       // 1 .. Prepaid, 2 .. Invoice
        private UInt32 _positiveBalance;
        private UInt32 _pricePerHundred;            // Price per hundred kilogramm [1/100]
        private UInt32 _languageId;                 // language index
        private DateTime _accessTime;               // Timepoint of machine access
        private string _cardType;                   // Type of card 0041 HITAG, 0080 MyFare
        private string _cardSerialNumber;           // hex formatted card serial number
        private string _cardSerialNumberUntrimmed;  // hex formatted card serial number

        #endregion

        #region properties

        public uint CustomerNumber { get => _customerNumber; set => _customerNumber = value; }
        public uint ReleaseNumber { get => _releaseNumber; set => _releaseNumber = value; }
        public uint LocationGroupMask { get => _locationGroupMask; set => _locationGroupMask = value; }
        public uint MinimumAmount { get => _minimumAmount; set => _minimumAmount = value; }
        public DateTime ReleaseDate { get => _releaseDate; set => _releaseDate = value; }
        public eCARD_TYPE CardChargingType { get => _cardChargingType; set => _cardChargingType = value; }
        public uint PositiveBalance { get => _positiveBalance; set => _positiveBalance = value; }
        public uint PricePerHundred { get => _pricePerHundred; set => _pricePerHundred = value; }
        public uint LanguageId { get => _languageId; set => _languageId = value; }
        public DateTime AccessTime { get => _accessTime; set => _accessTime = value; }
        public string CardType { get => _cardType; set => _cardType = value; }
        public string CardSerialNumber { get => _cardSerialNumber; set => _cardSerialNumber = value; }
        public string CardSerialNumberUntrimmed { get => _cardSerialNumberUntrimmed; set => _cardSerialNumberUntrimmed = value; }

        #endregion

        #region constructor

        public CustomerAccessCard()
        {
            _customerNumber = 0;
            _releaseNumber = 0;
            _locationGroupMask = 0;
            _minimumAmount = 0;
            _releaseDate = new DateTime(2000, 1, 1, 0, 0, 0);
            _cardChargingType = eCARD_TYPE.NONE;
            _positiveBalance = 0;
            _pricePerHundred = 0;
            _languageId = 0;
            _accessTime = new DateTime(2000, 1, 1, 0, 0, 0);
            _cardType = "0000";
            _cardSerialNumber = "";
        }

        #endregion
    }

    /// <summary>
    /// Class that represents a container object
    /// </summary>
    [Serializable]
    public class Container
    {
        #region Database Members

        private string _iccId = "";                 // simcard id
        private string _mobileNumber;               // simcard phone number
        private int _containerId;                   // container id
        private int _containerTypeId;               // containertype id
        private int _operatorId;                    // operators id
        private string _identString;                // identification string
        private string _deviceNumber;               // machine number
        private string _operatorName;
        private string _operatorLanguage;           
        private string _containerType;
        private string _firmwareVersion;            // firmwareversion which should be installed
        private int _numberOfStoredEvents;
        private bool _bIsRetroFit;
        private bool _bIsLiftTiltEquipped;
        private bool _bIsExternalStartEquipped;
        private bool _bIsIdentSystemEquipped;
        private bool _bIs2DotZero;
        private bool _pressPosition;                // position where press should stop 0 ... back, 1 ... front 

        #endregion

        #region Objects Members

        private string _modemFirmwareVersion;       // modem firmware version string
        private string _controllerFirmwareVersion;  // controller firmware version string
        private int _ModemSignalQuality;            // controller signal quality information
        private int _actualFillingLevel;            // actual calculated filling level
        private int _supplyVoltage;                 // actual supply voltage value (raw value)
        private DbGeography _location;              // gps coordinates where container is located
        private DateTime _tLastCommunication;       // time of last transaction
        private int _journalSize = 0;               // max num entries in journal  
        private int _writePointer = 0;              // actual write pointer of journal
        private int _readPointer = 0;               // actual read pointer of journal
        private bool _configrationInvalid = false;       // when configuration stored in modules flash is invalid

        #endregion

        #region Properties

        public string IccId { get => _iccId; set => _iccId = value; }
        public string MobileNumber { get => _mobileNumber; set => _mobileNumber = value; }
        public int ContainerId { get => _containerId; set => _containerId = value; }
        public int OperatorId { get => _operatorId; set => _operatorId = value; }

        public string ModemFirmwareVersion { get => _modemFirmwareVersion; set => _modemFirmwareVersion = value; }
        public string ControllerFirmwareVersion { get => _controllerFirmwareVersion; set => _controllerFirmwareVersion = value; }
        public int ModemSignalQuality { get => _ModemSignalQuality; set => _ModemSignalQuality = value; }
        public int ActualFillingLevel { get => _actualFillingLevel; set => _actualFillingLevel = value; }
        public DbGeography Location { get => _location; set => _location = value; }
        public DateTime TLastCommunication { get => _tLastCommunication; set => _tLastCommunication = value; }
        public int NumberOfStoredEvents { get => _numberOfStoredEvents; set => _numberOfStoredEvents = value; }
        public string IdentString { get => _identString; set => _identString = value; }
        public string DeviceNumber { get => _deviceNumber; set => _deviceNumber = value; }
        public string OperatorName { get => _operatorName; set => _operatorName = value; }
        public string ContainerType { get => _containerType; set => _containerType = value; }
        public int ContainerTypeId { get => _containerTypeId; set => _containerTypeId = value; }
        public string FirmwareVersion { get => _firmwareVersion; set => _firmwareVersion = value; }
        public int SupplyVoltage { get => _supplyVoltage; set => _supplyVoltage = value; }
        public int WritePointer { get => _writePointer; set => _writePointer = value; }
        public int ReadPointer { get => _readPointer; set => _readPointer = value; }
        public int JournalSize { get => _journalSize; set => _journalSize = value; }
        public string OperatorLanguage { get => _operatorLanguage; set => _operatorLanguage = value; }
        public bool IsRetroFit { get => _bIsRetroFit; set => _bIsRetroFit = value; }
        public bool IsLiftTiltEquipped { get => _bIsLiftTiltEquipped; set => _bIsLiftTiltEquipped = value; }
        public bool IsExternalStartEquipped { get => _bIsExternalStartEquipped; set => _bIsExternalStartEquipped = value; }
        public bool IsIdentSystemEquipped { get => _bIsIdentSystemEquipped; set => _bIsIdentSystemEquipped = value; }
        public bool PressPosition { get => _pressPosition; set => _pressPosition = value; }
        public bool Is2DotZero { get => _bIs2DotZero; set => _bIs2DotZero = value; }
        public bool IsConfigrationInvalid { get => _configrationInvalid; set => _configrationInvalid = value; }

        #endregion

        #region Constructor

        public Container()
        {
            NumberOfStoredEvents = 0;
            ModemFirmwareVersion = "unknown";
            ControllerFirmwareVersion = "unknown";
            IsRetroFit = false;
            IsLiftTiltEquipped = false;
            Is2DotZero = false;
        }

#endregion
    }

    [Serializable]
    public class Location
    {
        #region members

        private int _locationId;                    // location id
        private int _locationGroupId;               // location group to which this location belongs
        private string _name;                       // name of location
        private string _materialName;               // name of material
        private string _strHash;
        private int _materialId;                    // kind of material
        private int _pressStrokes;                  // number of strokes to perform in one cycle
        private bool _pressPosition;                // position where press should stop 0 ... back, 1 ... front 
        private int _fullWarningLevel;              // at this level full warning alerting will be done
        private int _fullErrorLevel;                // at this level full alerting will be done
        private double _latitude;                   // real coordinates from database
        private double _longitude;                  // real coordinates from database
        private int _nightLockStart;                // begin of nightlock (minutes after midnight)
        private int _nightLockEnd;                  // end of nightlock (minutes after midnight)
        private int _nightLockDuration;             // minutes
        private bool _isWatchdogActive;
        private int _WatchdogDuration;
        private bool _bValid;                       // is location valid (operatorId =)
        private int _machineUtilization;
        private Guid? _preferredFractionId;      

        // members only for 2.0 containers
        private bool _bLiftTiltEquipped;
        private bool _bRetroKitEquipped;

#endregion

        #region properties

        public string Name { get => _name; set => _name = value; }
        public int MaterialId { get => _materialId; set => _materialId = value; }
        public int PressStrokes { get => _pressStrokes; set => _pressStrokes = value; }
        public int FullWarningLevel { get => _fullWarningLevel; set => _fullWarningLevel = value; }
        public int FullErrorLevel { get => _fullErrorLevel; set => _fullErrorLevel = value; }
        public int LocationId { get => _locationId; set => _locationId = value; }
        public bool PressPosition { get => _pressPosition; set => _pressPosition = value; }
        public double Latitude { get => _latitude; set => _latitude = value; }
        public double Longitude { get => _longitude; set => _longitude = value; }
        public string MaterialName { get => _materialName; set => _materialName = value; }
        public string StrHash { get => _strHash; set => _strHash = value; }
        public int NightLockStart { get => _nightLockStart; set => _nightLockStart = value; }
        public int NightLockEnd { get => _nightLockEnd; set => _nightLockEnd = value; }
        public int NightLockDuration { get => _nightLockDuration; set => _nightLockDuration = value; }
        public bool IsWatchdogActive { get => _isWatchdogActive; set => _isWatchdogActive = value; }
        public int WatchdogDuration { get => _WatchdogDuration; set => _WatchdogDuration = value; }
        public int LocationGroupId { get => _locationGroupId; set => _locationGroupId = value; }
        public bool IsValid { get => _bValid; set => _bValid = value; }
        public int MachineUtilization { get => _machineUtilization; set => _machineUtilization = value; }
        public bool IsLiftTiltEquipped { get => _bLiftTiltEquipped; set => _bLiftTiltEquipped = value; }
        public bool IsRetroKitEquipped { get => _bRetroKitEquipped; set => _bRetroKitEquipped = value; }
        public Guid? PreferredFractionId { get => _preferredFractionId; set => _preferredFractionId = value; }

#endregion

        #region constructor

        public Location()
        {
            Name = "Not allocated";
            MaterialId = 2; // Karton
            MaterialName = "unknown";
            PressStrokes = 3;
            FullWarningLevel = 75;
            FullErrorLevel = 95;
            LocationId = 0;
            _pressPosition = false;
            _machineUtilization = 100;

            _bLiftTiltEquipped = false;
            _bRetroKitEquipped = false;

        }
#endregion
    }

    /// <summary>
    /// Class that represents a client connection
    /// </summary>
    [Serializable]
    public class ClientConnection : MarshalByRefObject
    {
        #region Enums

        private enum _CLIENT_STATE
        {
            INIT,
            WAIT_CONFIG,
            WAIT_CONFIG_ACK,
            READ_EVENTS,
            START_READ_JOURNAL,
            READ_JOURNAL,
            WAIT_READPOINTER_ACK,
            STOP,
            ONLINE,
            ERROR,
        }

#endregion

        #region Members

        private bool _bLogDetails = true;
        private Container _container = new Container();                     // container object
        private Location _location = new Location();
        private string _name = "unknown";
        private UInt32 _numberOfBytesReceived = 0;                          // traffic counter in receive direction
        private UInt32 _numberOfBytesSent = 0;                              // traffic counter in send direction
        private DateTime _tLastCommandFromEco;                              // time when last command from ECO was received
        private DateTime _tLastCommandToEco;                                // time when last command was sent to ECO
        private DateTime _tLastWritePointerQuery;                           // last time the write pointers were read
        private Queue _receivedFrames = new Queue();
        private Queue _serviceCommands = new Queue();                       // queue for service commands

        private DateTime _destTime;

        [NonSerialized]
        private TcpClient _tcpClient;                                       // tcp client class
        [NonSerialized]
        private ConnectionControl _controller;                              // controller object who has created this instance
        [NonSerialized]
        private Thread _workerThread;                                       // working thread
        [NonSerialized]
        private Thread _receiverThread;                                     // receiving thread
        [NonSerialized]
        private AutoResetEvent _stopHandle = new AutoResetEvent(false);     // stop event
        [NonSerialized]
        private AutoResetEvent _receivedEvent = new AutoResetEvent(false);
        [NonSerialized]
        private const int BLOCK_SIZE = 1024;
        [NonSerialized]
        bool _bIdentified = false;
        [NonSerialized]
        _CLIENT_STATE state = _CLIENT_STATE.INIT;
        [NonSerialized]
        int _keepAliveInterval = 5;
        [NonSerialized]
        int _numberOfStartingsLastCycle = 0;
        [NonSerialized]
        int _signalQualityLastCycle = 0;
        [NonSerialized]
        string _cellInfoLastCycle = "";

        private bool _bPermantOnline = false;

#endregion

        #region Overrides

        /// <summary>
        /// Override of MarshalByRefObject's implementation. Specify to leave the singelton remoting object in memory till the appdomain shuts down.
        /// </summary>
        /// <returns></returns>
        public override Object InitializeLifetimeService()
        {
            return null;
        }

#endregion

        #region Properties

        /// <summary>
        /// Client already full accepted
        /// </summary>
        public bool Identified
        {
            get { return _bIdentified; }
        }

        /// <summary>
        /// Switch detailed logging on or off
        /// </summary>
        public bool LogDetails
        {
            get { return _bLogDetails; }
            set { _bLogDetails = value; }
        }

        /// <summary>
        /// Time when last command was received from client
        /// </summary>
        public DateTime LastCommandReceived
        {
            get { return _tLastCommandFromEco; }
        }

        /// <summary>
        /// Time when last command was sent to eco
        /// </summary>
        public DateTime LastCommandSent
        {
            get { return _tLastCommandToEco; }
        }

        /// <summary>
        /// TCP Socket connection
        /// </summary>
        public TcpClient TcpClient
        {
            get { return _tcpClient; }
            set { _tcpClient = value; }
        }

        /// <summary>
        /// Connection control service
        /// </summary>
        public ConnectionControl Controller
        {
            get { return _controller; }
            set { _controller = value; }
        }

        /// <summary>
        /// Counter for data traffic in receive direction
        /// </summary>
        public UInt32 NumberOfBytesReceived
        {
            get { return _numberOfBytesReceived; }
            set { _numberOfBytesReceived = value; }
        }

        /// <summary>
        /// Counter for data traffic in send direction
        /// </summary>
        public UInt32 NumberOfBytesSent
        {
            get { return _numberOfBytesSent; }
            set { _numberOfBytesSent = value; }
        }

        public string Name { get => _name; set => _name = value; }

#endregion

        #region Public methods

#region constructor

        public ClientConnection(string firstFrame)
        {
            if (firstFrame != "")
                _receivedFrames.Enqueue(firstFrame);
        }

#endregion

    #region start & stop

        public void Start()
        {
            // start receiver thread
            _receiverThread = new Thread(new ThreadStart(Receive));
            _receiverThread.Start();

            // start worker thread
            _workerThread = new Thread(new ThreadStart(DoWork));
            _workerThread.Start();
        }

        public void Stop()
        {
            SendQuitCommand();

            _stopHandle.Set();
        }

#endregion

#endregion

        #region Helpers

        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime FromUnixTime(long unixTime)
        {
            return epoch.AddSeconds(unixTime);
        }

        private string GetMaterialName(int materialId, string language)
        {
            if (materialId == 1)
            {
                if (language == "DE")
                    return "Gewerbemüll";
                else
                    return "Commercial waste";
            }
            else if (materialId == 2)
            {
                if (language == "DE")
                    return "Karton";
                else
                    return "Cardboard";
            }
            else if (materialId == 3)
            {
                if (language == "DE")
                    return "Kunststoff";
                else
                    return "Plastic";
            }
            else if (materialId == 4)
            {
                if (language == "DE")
                    return "Mischpapier";
                else
                    return "Mixed paper";
            }
            else if (materialId == 5)
            {
                if (language == "DE")
                    return "Mischfolie";
                else
                    return "Mixed foil";
            }
            else if (materialId == 6)
            {
                if (language == "DE")
                    return "Gemischte Siedlungsabfälle";
                else
                    return "Mixed waste";
            }
            else
                return "Nicht bekannt";
        }

#region AreaCode

        private bool checkAreaCode(string phoneNumber, ref bool isDrei)
        {
            string str = phoneNumber.TrimStart();
            str.TrimEnd();

            isDrei = false;

            if (str.StartsWith("+"))
            {
                str = str.Substring(1);
            }
            else if (str.StartsWith("00"))
            {
                str = str.Substring(2);
            }
            else
            {
                Console.WriteLine("Error: Invalid phoneNumber");
                return false;
            }

            if (str.StartsWith("30"))
            {
                Console.WriteLine("Greece");
                isDrei = true;
                return true;
            }
            else if (str.StartsWith("31"))
            {
                Console.WriteLine("Netherland");
                isDrei = true;
                return true;
            }
            else if (str.StartsWith("32"))
            {
                Console.WriteLine("Belgium");
                isDrei = true;
                return true;
            }
            else if (str.StartsWith("33"))
            {
                Console.WriteLine("France");
                isDrei = true;
                return true;
            }
            else if (str.StartsWith("35"))
            {
                str = str.Substring(2);
                if (str.StartsWith("1"))
                {
                    Console.WriteLine("Portugal");
                    isDrei = true;
                    return true;
                }
                else if (str.StartsWith("2"))
                {
                    Console.WriteLine("Luxembourg");
                    isDrei = true;
                    return true;
                }
                else if (str.StartsWith("3"))
                {
                    Console.WriteLine("Ireland");
                    isDrei = true;
                    return true;
                }
                else if (str.StartsWith("4"))
                {
                    Console.WriteLine("Iceland");
                    isDrei = true;
                    return true;
                }
                else if (str.StartsWith("6"))
                {
                    Console.WriteLine("Malta");
                    isDrei = true;
                    return true;
                }
                else if (str.StartsWith("7"))
                {
                    Console.WriteLine("Cyprus");
                    isDrei = true;
                    return true;
                }
                else if (str.StartsWith("8"))
                {
                    Console.WriteLine("Finland");
                    isDrei = true;
                    return true;
                }
                else if (str.StartsWith("9"))
                {
                    Console.WriteLine("Bulgaria");
                    isDrei = true;
                    return true;
                }
                else
                {
                    return true;
                }
            }
            else if (str.StartsWith("36"))
            {
                Console.WriteLine("Hungary");
                isDrei = true;
                return true;
            }
            else if (str.StartsWith("37"))
            {
                str = str.Substring(2);

                if (str.StartsWith("1"))
                {
                    Console.WriteLine("Latvia");
                    isDrei = true;
                    return true;
                }
                else if (str.StartsWith("2"))
                {
                    Console.WriteLine("Estonia");
                    isDrei = true;
                    return true;
                }
                else
                {
                    return true;
                }
            }
            else if (str.StartsWith("38"))
            {
                str = str.Substring(2);

                if (str.StartsWith("6"))
                {
                    Console.WriteLine("Slovenia");
                    isDrei = true;
                    return true;
                }
                else
                {
                    return true;
                }
            }
            else if (str.StartsWith("39"))
            {
                Console.WriteLine("Italy");
                isDrei = true;
                return true;
            }
            else if (str.StartsWith("40"))
            {
                Console.WriteLine("Romania");
                isDrei = true;
                return true;
            }
            else if (str.StartsWith("41"))
            {
                Console.WriteLine("Switzerland");
                isDrei = true;
                return true;
            }
            else if (str.StartsWith("42"))
            {
                str = str.Substring(2);

                if (str.StartsWith("0"))
                {
                    Console.WriteLine("Czech");
                    isDrei = true;
                    return true;
                }
                else if (str.StartsWith("1"))
                {
                    Console.WriteLine("Slovakia");
                    isDrei = true;
                    return true;
                }
                else if (str.StartsWith("3"))
                {
                    Console.WriteLine("Liechtenstein");
                    isDrei = true;
                    return true;
                }
                else
                {
                    return true;
                }
            }
            else if (str.StartsWith("43"))
            {
                Console.WriteLine("Austria");
                isDrei = true;
                return true;
            }
            else if (str.StartsWith("44"))
            {
                Console.WriteLine("Britain");
                isDrei = true;
                return true;
            }
            else if (str.StartsWith("45"))
            {
                Console.WriteLine("Denmark");
                isDrei = true;
                return true;
            }
            else if (str.StartsWith("46"))
            {
                Console.WriteLine("Sweden");
                isDrei = true;
                return true;
            }
            else if (str.StartsWith("47"))
            {
                Console.WriteLine("Norway");
                isDrei = true;
                return true;
            }
            else if (str.StartsWith("48"))
            {
                Console.WriteLine("Polen");
                isDrei = true;
                return true;
            }
            else if (str.StartsWith("49"))
            {
                Console.WriteLine("Germany");
                isDrei = true;
                return true;
            }
            else if (str.StartsWith("59"))
            {
                str = str.Substring(2);

                if (str.StartsWith("0"))
                {
                    Console.WriteLine("Guadeloupe");
                    isDrei = true;
                    return true;
                }
                else
                {
                    return true;
                }
            }
            else if (str.StartsWith("7012"))
            {
                Console.WriteLine("Guadeloupe");
                isDrei = true;
                return true;
            }

            return false;
        }

#endregion

        private bool analyseStatus(string frame)
        {
            try
            {
                ConnectionControl.SkpApiClient.UpdateContainerLastCommunication(_container.ContainerId, new UpdateLastCommunication(DateTime.Now));
            }
            catch (Exception excp)
            {
                LogFile.WriteErrorToLogFile("Exception: {0} while trying to store last communication date", excp.Message);
            }

            try
            {
                string[] toks = frame.Split(new char[] { '=', ',' });

                if (toks.GetLength(0) > 4)
                {
                    try
                    {
                        ////////////////////////////////////////////////////
                        // [1] Protocol version

                        uint protocolVersion = Convert.ToUInt32(toks[1]);

                        //
                        ////////////////////////////////////////////////////

                        ////////////////////////////////////////////////////
                        // [2] Number of events stored

                        _container.NumberOfStoredEvents = Convert.ToInt32(toks[2]);
                        
                        //
                        ////////////////////////////////////////////////////

                        if (protocolVersion == 2)
                        {
                            _container.WritePointer = 0;
                            _container.ReadPointer = 0;
                           
                        }
                        else if (protocolVersion >= 3)
                        {
                            _container.JournalSize = Convert.ToInt32(toks[3]);
                            _container.WritePointer = Convert.ToInt32(toks[4]);
                            _container.ReadPointer = Convert.ToInt32(toks[5]);

                            // network info since firmware version 1.2.00
                            if (toks.GetLength(0) > 10)
                            {
                                try
                                {
                                    int numberOfStartings = Convert.ToInt32(toks[6]);
                                    int minutesOfOperation = Convert.ToInt32(toks[7]);
                                    int mobileCountryCode = 0;
                                    int mobileNetworkCode = 0;
                                    int locationAreaCode = 0;
                                    int cellId = 0;
                                    int signaldB = 0;

                                    String networkInfo = toks[8] + " - " + toks[9];

                                    if (toks[10] != "SEARCH")
                                    {
                                        try
                                        {
                                            if (toks[9] == "2G")
                                            {
                                                signaldB = Convert.ToInt32(toks[11]);
                                                mobileCountryCode = Convert.ToInt32(toks[12]);
                                                mobileNetworkCode = Convert.ToInt32(toks[13]);
                                                locationAreaCode = Convert.ToInt32(toks[14], 16);
                                                cellId = Convert.ToInt32(toks[15], 16);
                                            }
                                            else if (toks[9] == "3G")
                                            {
                                                signaldB = Convert.ToInt32(toks[13]);
                                                mobileCountryCode = Convert.ToInt32(toks[14]);
                                                mobileNetworkCode = Convert.ToInt32(toks[15]);
                                                locationAreaCode = Convert.ToInt32(toks[16], 16);
                                                cellId = Convert.ToInt32(toks[17], 16);
                                            }
                                            else if (toks[9] == "4G")
                                            {
                                                signaldB = Convert.ToInt32(toks[21]);
                                                mobileCountryCode = Convert.ToInt32(toks[15]);
                                                mobileNetworkCode = Convert.ToInt32(toks[16]);
                                                locationAreaCode = Convert.ToInt32(toks[17], 16);
                                                cellId = Convert.ToInt32(toks[18], 16);
                                            }
                                        }
                                        catch (Exception excp)
                                        {
                                            LogFile.WriteMessageToLogFile("{0} ({1}), while trying to get Networkinfo", this.Name, excp.Message);
                                        }
                                    }

                                    networkInfo += String.Format(" ({0} dBm)", signaldB);
                                    networkInfo += " - MCC: " + mobileCountryCode;
                                    networkInfo += " - MNC: " + mobileNetworkCode;
                                    networkInfo += " - LAC: " + locationAreaCode;
                                    networkInfo += " - CELLID: " + cellId;

                                    FirmwareType ft = FirmwareType.Presscontrol;

                                    if (_container.ModemFirmwareVersion.IndexOf("SSC-") != -1)
                                    {
                                        ft = FirmwareType.Ssc;
                                        int pos1 = _container.ModemFirmwareVersion.LastIndexOf("-");
                                        if (pos1 != -1)
                                            _container.ModemFirmwareVersion = _container.ModemFirmwareVersion.Substring(pos1 + 1);

                                        LogFile.WriteMessageToLogFile("{0} - SSC with firmware version: {1}", this.Name, _container.ModemFirmwareVersion);
                                    }
                                    else if (_container.ModemFirmwareVersion.IndexOf("3RD-") != -1)
                                    {
                                        ft = FirmwareType.ThirdPartyProduct;
                                        int pos1 = _container.ModemFirmwareVersion.LastIndexOf("-");
                                        if (pos1 != -1)
                                            _container.ModemFirmwareVersion = _container.ModemFirmwareVersion.Substring(pos1 + 1);

                                        LogFile.WriteMessageToLogFile("{0} - 3rd Party product with firmware version: {1}", this.Name, _container.ModemFirmwareVersion);
                                    }

                                    StoreContainerHardwareInformation info = new StoreContainerHardwareInformation(networkInfo, ft, _container.ModemFirmwareVersion, _container.ModemSignalQuality,
                                        numberOfStartings, minutesOfOperation, DateTime.Now);

                                    if (_numberOfStartingsLastCycle != info.NumberOfStartings ||
                                        _signalQualityLastCycle != info.GsmSignalStrength ||
                                        _cellInfoLastCycle != networkInfo)
                                    {
                                        _numberOfStartingsLastCycle = info.NumberOfStartings;
                                        _signalQualityLastCycle = info.GsmSignalStrength;
                                        _cellInfoLastCycle = networkInfo;

                                        LogFile.WriteMessageToLogFile("{0} Store Hardwareinfo: {1}, {2}, {3}, {4}, {5}", this.Name, info.FirmwareVersion, info.GsmSignalStrength,
                                            info.NumberOfStartings, info.OperatingMinutes, info.DataConnection);

                                        ConnectionControl.SkpApiClient.StoreContainerHardwareInformation(_container.ContainerId, info);
                                    }
                                }
                                catch (Exception excp)
                                {
                                    LogFile.WriteErrorToLogFile("{0} Exception ({1}) while trying to parse network info", this.Name, excp.Message);
                                }
                            }
                        }
                    }
                    catch (Exception excp)
                    {
                        LogFile.WriteErrorToLogFile("{0} Exception: {1} in \'AnalyzeStatus\' while parsing data appeared.", this.Name, excp.Message);
                        return false;
                    }

                    return true;
                }
                else
                {
                    LogFile.WriteErrorToLogFile("{0} Invalid Status string received: ({1})", Name, frame);
                    return false;
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("{0} Exception: {1} in \'AnalyzeStatus\' appeared.", this.Name, e.Message);
            }

            return false;
        }

        private string ByteArrayToString(byte[] arrInput)
        {
            int i;

            StringBuilder sOutput = new StringBuilder(arrInput.Length);

            for (i = 0; i < arrInput.Length - 1; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }

            return sOutput.ToString();
        }

        private bool ParseJournalEntries(string str, int numberOfExpectedEntries)
        {
            try
            {
                // since firmware minor version 3 we got three additional fields: AlibiStorageNumber, CardType and CardSerialnumber in journal entries
                UInt32 numFields = 11;
                string[] fwParts = _container.ModemFirmwareVersion.Split(new char[] { '.' });
                UInt32 minor;
                try
                {
                     minor = Convert.ToUInt32(fwParts[1]);
                    if (minor >= 3)
                        numFields = 14;
                }
                catch (Exception)
                {
                    LogFile.WriteErrorToLogFile("{0} Invalid firmwarestring: {1}", this.Name, fwParts[1]);
                }

                // remove header
                str = str.Substring(5);
                // split in entries
                string[] str_entry = str.Split(new char[] { ';' });

                if (str_entry.GetLength(0) < numberOfExpectedEntries)
                {
                    LogFile.WriteErrorToLogFile("{0} ParseJournalEntries, unexpected number of entries ({1})!", this.Name, str_entry.GetLength(0));
                    return false;
                }

                List<StatusMessageDto> statusMsgList = new List<StatusMessageDto>();

                // parse all entries
                for (int i = 0; i < numberOfExpectedEntries; i++)
                {
                    try
                    {
                        string[] toks = str_entry[i].Split(new char[] { ',' });

                        if (toks.GetLength(0) != numFields)
                        {
                            LogFile.WriteErrorToLogFile("{0} ParseJournalEntries, unexpected number of fields in entry: {1}!", this.Name, str_entry[i]);
                            return false;
                        }

                        DateTime date = new DateTime();

                        try
                        {
                            date = DateTime.ParseExact(toks[2], "ddMMyyyy", null);
                        }
                        catch (Exception)
                        {
                            LogFile.WriteErrorToLogFile("{0} Invalid date format in ParseJournalEntry: {1}", this.Name, str_entry[i]);
                        }

                        DateTime time = new DateTime();
                        try
                        {
                            time = DateTime.ParseExact(toks[3], "HHmmss", null);
                        }
                        catch (Exception)
                        {
                            LogFile.WriteErrorToLogFile("{0} Invalid time format in ParseJournalEntry: {1}", this.Name, str_entry[i]);
                        }

                        date = date.Add(time.TimeOfDay);

                        int entry_address = Convert.ToInt32(toks[0]);
                        int locationId = Convert.ToInt32(toks[1]);
                        string customerNumber = toks[4];
                        int code = Convert.ToInt32(toks[6]);
                        int duration = Convert.ToInt32(toks[7]);
                        int amount = Convert.ToInt32(toks[8]);
                        double positiveCreditBalance = Convert.ToDouble(toks[9]) / 100;
                        int weight = Convert.ToInt32(toks[10]);
                        String alibiStorageNumber = "";
                        String cardType = "";
                        String cardSerialNumber = "";

                        if (numFields >= 13)
                        {
                            alibiStorageNumber = toks[11];
                            cardType = toks[12];
                            cardSerialNumber = toks[13].Trim();
                        }

                        LogFile.WriteMessageToLogFile("{0} Stored Event: {1}, time: {2}", this.Name, code, date);

                        string message = Controller.GetTranslation("Message", _container.OperatorLanguage);
                        message += ": ";

                        try
                        {
                            message += ConnectionControl.SkpApiClient.GetTranslationForStatusMessage(code, _container.OperatorLanguage);
                            LogFile.WriteMessageToLogFile("{0} Got Translation for code: {1} -> {2}", this.Name, code, message);
                        }
                        catch (Exception excp)
                        {
                            LogFile.WriteErrorToLogFile("{0} Exception: {1} while trying to get translation for code: {2}, language code: {3}", this.Name, excp.Message, code, _container.OperatorLanguage);
                        }

                        string uuid = cardType + cardSerialNumber;

                        if (uuid != "")
                        {
                            int cusNumber = 0;
                            try
                            {
                                cusNumber = Convert.ToInt32(customerNumber);
                            }
                            catch (Exception) { };

                            if (cusNumber == 0)
                            {
                                try
                                {
                                    CreateInsertionTransactionByCardUuid trans = new CreateInsertionTransactionByCardUuid(alibiStorageNumber, uuid, 
                                        _container.ContainerId, positiveCreditBalance, duration, _location.LocationId, _container.OperatorId,
                                        code, date.ToUniversalTime(), null);

                                    LogFile.WriteMessageToLogFile("{0} Store transaction for customer: {1}, duration: {2}", this.Name, trans.CardUuid, trans.DurationInSeconds);
                                    ConnectionControl.SkpApiClient.CreateInsertionByCardUuid(trans);
                                }
                                catch (Exception excp)
                                {
                                    LogFile.WriteErrorToLogFile("{0} Exception: {1} while trying to store transaction!", this.Name, excp.Message);
                                }
                            }
                            else
                            {
                                try
                                {
                                    double currentBalance = 0.0F;
                                    double weightInKilo = 0.0F;

                                    CreateInsertionTransactionByCustomerNumber trans = new CreateInsertionTransactionByCustomerNumber(alibiStorageNumber, cardSerialNumber,
                                        _container.ContainerId, currentBalance, cusNumber, duration, _location.LocationId, _container.OperatorId,
                                        code, date.ToUniversalTime(), weightInKilo);

                                    ConnectionControl.SkpApiClient.CreateInsertionByCustomerNumber(trans);
                                }
                                catch (Exception excp)
                                {
                                    LogFile.WriteErrorToLogFile("{0} Exception: {1} while trying to store transaction!", this.Name, excp.Message);
                                }
                            }
                            continue;
                        }
                        else if (code == 20)  // Power On Event
                        {
                            if (_container.OperatorId == 10008) // no power on messages to wong fong
                                continue;
                        }
                        else if (code == 42 && _bPermantOnline)    // actual fillinglevel changed
                        {
                            // Fillinglevel is only transmitted on new connection
                            // when permant online is selected (customer card reader)
                            // we have to handle actual fillinglevel with message
                            // FILLING_LEVEL_CHANGED
                            _container.ActualFillingLevel += 5;
                            LogFile.WriteMessageToLogFile("{0} Increased actual filling level to: {1}", this.Name, _container.ActualFillingLevel);
                        }
                        else if (code == 2727) // Geodata changed
                        {
                            continue;
                        }
                        else if (code == 2728) // Goto Standby failed 1
                        {
                            LogFile.WriteErrorToLogFile("Goto standby (1) failed!");
                            continue;
                        }
                        else if (code == 2729) // Goto Standby failed 2
                        {
                            LogFile.WriteErrorToLogFile("Goto standby (2) failed!");
                            continue;
                        }
                        else if (code == 2730) // Emptying fallback triggered
                        {
                            LogFile.WriteErrorToLogFile("Emptying fallback triggered!");
                            continue;
                        }
                        else if (code == 4360) // NO_GSM_CONNECTION
                        {
                            continue;
                        }

                        bool bShouldNotfiyUser = true;
                        bool bStoreInHistory = true;

                        statusMsgList.Add(new StatusMessageDto(code, _container.ActualFillingLevel, bShouldNotfiyUser, bStoreInHistory, date.ToUniversalTime()));

                        String smsMessage = "\nIdent Nr.: " + _container.IdentString + "\n";
                        smsMessage += message + "\n";

                        smsMessage += Controller.GetTranslation("Material", _container.OperatorLanguage) + ": " + _location.MaterialName + "\n";

                        smsMessage += Controller.GetTranslation("Operator", _container.OperatorLanguage) +  ": " + _container.OperatorName + "\n";

                        smsMessage += Controller.GetTranslation("Location", _container.OperatorLanguage) + ": " + _location.Name + "\n";

                        double lat = (double)_location.Latitude;
                        double lng = (double)_location.Longitude;

                        ConnectionControl.DoAlerting(_container.ContainerId, code, smsMessage, "", "");
                    }
                    catch (Exception excp)
                    {
                        LogFile.WriteErrorToLogFile("{0} Exception: {1}, while parsing entry {2}", this.Name, excp.Message, i + 1);
                    }
                }

                StoreContainerStatus containerStatus = new StoreContainerStatus(_location.LocationId, _container.IccId, statusMsgList);
                ConnectionControl.SkpApiClient.StoreContainerStatus(_container.ContainerId, containerStatus);

                return true;
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("{0} Exception: {1} in \'ParseJournalEntries\' appeared.", this.Name, e.Message);
            }

            return false;
        }

        private bool ReadEvents(string frame)
        {
            try
            {
                string[] toks = frame.Split(new char[] { '=', ',' });

                if (toks.Length > 0)
                {
                    int numberOfEvents = (toks.Length - 1) / 2;

                    LogFile.WriteMessageToLogFile("Read ({0}) events: {1}", numberOfEvents, frame);

                    List<StatusMessageDto> statusMsgList = new List<StatusMessageDto>();

                    for (int i = 0; i < numberOfEvents; i++)
                    {
                        uint type = System.Convert.ToUInt32(toks[i * 2 + 1]);
                        uint time = System.Convert.ToUInt32(toks[i * 2 + 2]);
                        DateTime date = FromUnixTime(time);
                        int code = 0;

                        LogFile.WriteMessageToLogFile("Event with type: {0} and time: {1}", type, time);

                        bool bShouldNotfiyUser = true;
                        bool bStoreInHistory = true;

                        statusMsgList.Add(new StatusMessageDto(code, _container.ActualFillingLevel, bShouldNotfiyUser, bStoreInHistory, date.ToUniversalTime()));

                        string message = Controller.GetTranslation("Message", _container.OperatorLanguage);
                        message += ": ";

                        if (type == 0)  // Power On Event
                        {
                            message += Controller.GetTranslation("PowerOn", _container.OperatorLanguage);
                            code = 20;
                        }
                        else if (type == 1)
                        {
                            message += Controller.GetTranslation("Emptying", _container.OperatorLanguage);
                            code = 10;
                        }
                        else if (type == 2) // Nearly full Event
                        {
                            // nearly full
                            message += Controller.GetTranslation("PreFull", _container.OperatorLanguage);
                            code = 40;
                        }
                        else if (type == 3) // Full Event
                        {
                            // full
                            message += Controller.GetTranslation("Full", _container.OperatorLanguage);
                            code = 41;
                        }
                        else if (type == 4) // Emergency stop
                        {
                            message += Controller.GetTranslation("EmergencyStop", _container.OperatorLanguage);
                            code = 4025;
                        }
                        else if (type == 5) // Motorschutz
                        {
                            message += Controller.GetTranslation("MotorProtection", _container.OperatorLanguage);
                            code = 4030;
                        }
                        else if (type == 6) // Störung
                        {
                            message += Controller.GetTranslation("Malfunction", _container.OperatorLanguage);
                            code = 3492;
                        }
                        else if (type == 10)
                        {
                            // error free
                            code = 21;
                            // reset emergeny stop and motor protection prophylactic
//                            StoreContainerStatus status = new StoreContainerStatus(_container.IccId, _location.LocationId, 4026, _container.ActualFillingLevel, date.ToUniversalTime());
//                            ConnectionControl.SkpApiClient.StoreContainerStatusMethod(_container.ContainerId, status);
//                            status = new StoreContainerStatus(_container.IccId, _location.LocationId, 4031, _container.ActualFillingLevel, date.ToUniversalTime());
//                            ConnectionControl.SkpApiClient.StoreContainerStatusMethod(_container.ContainerId, status);
                        }

                        LogFile.WriteMessageToLogFile("{0} Stored Event: {1}, time: {2}", this.Name, code, date);

                        String smsMessage = "\nIdent Nr.: " + _container.IdentString + "\n";
                        smsMessage += message + "\n";

                        smsMessage += Controller.GetTranslation("Material", _container.OperatorLanguage) + ": " + _location.MaterialName + "\n";

                        smsMessage += Controller.GetTranslation("Operator", _container.OperatorLanguage) + ": " + _container.OperatorName + "\n";

                        smsMessage += Controller.GetTranslation("Location", _container.OperatorLanguage) + ": " + _location.Name + "\n";

                        double lat = (double)_location.Latitude;
                        double lng = (double)_location.Longitude;

                        ConnectionControl.DoAlerting(_container.ContainerId, code, smsMessage, "", "");
                    }

                    StoreContainerStatus containerStatus = new StoreContainerStatus(_location.LocationId, _container.IccId, statusMsgList);
                    ConnectionControl.SkpApiClient.StoreContainerStatus(_container.ContainerId, containerStatus);
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("{0} Exception: {1} in \'ReadEvents\' appeared.", this.Name, e.Message);
            }

            return false;
        }

        static string TrimCustomerNumber(string custNumber, string dbgName)
        {
            for (int i = 0; i < custNumber.Length; i++)
            {
                if (custNumber[i] != '0')
                {
                    int remainder = custNumber.Length - i;
                    if (remainder % 2 != 0)
                    {
                        remainder++;
                        i--;
                    }

                    LogFile.WriteMessageToLogFile("{0} Trimmed customer number: {1}", dbgName, custNumber.Substring(i, remainder));
                    return custNumber.Substring(i, remainder);
                }
            }

            return "";
        }

        static public bool ParseCustomerData(string str, string dbgName, ref CustomerAccessCard card)
        {
            try
            {
                string[] toks = str.Split(new char[] { '=', ',' });

                if (toks.GetLength(0) >= 12)
                {
                    try
                    {
                        try { card.CustomerNumber = Convert.ToUInt32(toks[1]); } catch { };
                        try { card.ReleaseNumber = Convert.ToUInt32(toks[2]); } catch { };
                        try { card.LocationGroupMask = Convert.ToUInt32(toks[3]); } catch { };
                        try { card.MinimumAmount = Convert.ToUInt32(toks[4]); } catch { };
                        try { card.ReleaseDate = DateTime.ParseExact(toks[5], "ddMMyyyy", null); } catch { };
                        try { card.CardChargingType = (eCARD_TYPE)Convert.ToUInt32(toks[6]); } catch { };
                        try { card.PositiveBalance = Convert.ToUInt32(toks[7]); } catch { };
                        try { card.PricePerHundred = Convert.ToUInt32(toks[8]); } catch { };
                        try { card.LanguageId = Convert.ToUInt32(toks[9]); } catch { };

                        try { card.AccessTime = DateTime.ParseExact(toks[10], "ddMMyyyy", null); }
                        catch (Exception)
                        {
                            LogFile.WriteErrorToLogFile("{0} Invalid access date format in customer access string: {1}", dbgName, str);
                        }

                        try
                        {
                            DateTime actualTime = DateTime.ParseExact(toks[11], "HHmmss", null);
                            card.AccessTime = card.AccessTime.Add(actualTime.TimeOfDay);
                        }
                        catch (Exception)
                        {
                            LogFile.WriteErrorToLogFile("{0} Invalid access time format in customer access string: {1}", dbgName, str);
                        }

                        if (toks.GetLength(0) >= 14)
                        {
                            try { card.CardType = toks[12]; } catch { };
                            try { card.CardSerialNumber = toks[13]; } catch { };
                            card.CardSerialNumberUntrimmed = card.CardSerialNumber;
                            card.CardSerialNumber = TrimCustomerNumber(card.CardSerialNumber.Trim(), dbgName);

                            if ((card.CustomerNumber == 0) && (card.CardType == "") && (card.CardSerialNumber == ""))
                            {
                                LogFile.WriteErrorToLogFile("{0} Invalid customer card data!", dbgName);
                                return false;
                            }
                        }
                    }
                    catch (Exception excp)
                    {
                        LogFile.WriteErrorToLogFile("{0} Exception ({1}) while trying to parse customer data. ({2})", dbgName, excp.Message, str);
                        return false;
                    }
                }
                else
                {
                    LogFile.WriteErrorToLogFile("{0} customer access string. Invalid format: ({1})", dbgName, str);
                    return false;
                }
            }
            catch (Exception excp)
            {
                LogFile.WriteErrorToLogFile("{0} Exception ({1}) while trying to split customer data. ({2})", dbgName, excp.Message, str);
                return false;
            }

            return true;
        }

        private bool processFrame(string frame)
        {
            LogFile.WriteMessageToLogFile("{0} process frame: ({1})", Name, frame);

            if (frame.StartsWith("%CUS"))
            {
                CustomerAccessCard card = new CustomerAccessCard();

                if (ParseCustomerData(frame, this.Name, ref card))
                {
                    CardInfoDto cardInfo = null;

                    if (card.CustomerNumber != 0)
                    {
                        // check if customer access is allowed
                        HasCardAccessToLocationFractionByCustomerNumber checkAccess = new HasCardAccessToLocationFractionByCustomerNumber((int)card.CustomerNumber, this._location.LocationId, this._container.OperatorId);
                        cardInfo = ConnectionControl.SkpApiClient.GetCardInfoByCustomerNumber(checkAccess);
                        LogFile.WriteMessageToLogFile($"Has Access {card.CustomerNumber}: {cardInfo.CardAccess}");
                    }
                    else
                    {
                        string uuid = card.CardType + card.CardSerialNumber;
                        HasCardAccessToLocationFractionByCardUuid checkAccess = new HasCardAccessToLocationFractionByCardUuid(uuid, this._location.LocationId, this._container.OperatorId);
                        cardInfo = ConnectionControl.SkpApiClient.GetCardInfoByCardUuid(checkAccess);
                        LogFile.WriteMessageToLogFile($"Has Access {uuid}: {cardInfo.CardAccess}");
                    }

                    if (cardInfo != null && cardInfo.CardAccess == CardAccessResult.Ok)
                    {
                        SendCommand("#CUS!");
                    }
                    else
                    {
                        string answer = String.Format("#CUS: {0};", (int)cardInfo.CardAccess); 
                        SendCommand(answer);
                    }
                }
            }
            else if (frame.StartsWith("%STAT="))
            {
                if (analyseStatus(frame))
                {
                    state = _CLIENT_STATE.START_READ_JOURNAL;
                }
            }


            return true;
        }

        private string getFrame(string start_sequence)
        {
            string str = "";

            try
            {
                while (_receivedFrames.Count > 0)
                {
                    // get frame from queue
                    lock (_receivedFrames.SyncRoot)
                    {
                        str = (string)_receivedFrames.Dequeue();
                    }

                    if (str.StartsWith(start_sequence))
                        return str;
                    else
                    {
                        if (this.state <= _CLIENT_STATE.WAIT_CONFIG_ACK)
                        {
                            // we are not yet identified so put message back to queue and process it later
                            lock (_receivedFrames.SyncRoot)
                            {
                                _receivedFrames.Enqueue(str);
                            }
                            return "";
                        }
                        else
                        {
                            processFrame(str);
                        }
                    }
                }
            }
            catch (Exception excp)
            {
                LogFile.WriteErrorToLogFile("{0} Exception ({1}) in get_answer appeared!", this.Name, excp.Message);
            }

            return "";
        }

#endregion

        #region Protocol methods

        /// <summary>
        /// send quit connection command
        /// </summary>
        /// <param name="command">Command string to send to container</param>
        /// <returns>true if successfully sent</returns>
        private bool SendQuitCommand()
        {
            try
            {
                byte[] send_data = new byte[6];

                send_data[0] = 0x02;    // STX
                send_data[1] = 0x40;    // CMD_QUIT
                send_data[2] = 0;
                send_data[3] = 0;
                send_data[4] = 0;
                send_data[5] = 0x03;   // ETX

                _tcpClient.Client.Send(send_data);

                if (_bLogDetails) LogFile.WriteMessageToLogFile("Send: Quitcommand");
            }
            catch (Exception excp)
            {
                LogFile.WriteErrorToLogFile("{0} Exception: {1} in function SendQuitCommand.", this.Name, excp.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Embed command string into a frame and send it to container
        /// </summary>
        /// <param name="command">Command string to send to container</param>
        /// <returns>true if successfully sent</returns>
        private bool SendCommand(string command)
        {
            try
            {
                int data_length = command.Length;
                byte chk = 0;
                byte[] send_data = new byte[data_length + 6];

                send_data[0] = 0x02;    // STX
                send_data[1] = 0x41;    // WIP2ECO
                send_data[2] = (byte)((data_length & 0xff00) >> 8);
                send_data[3] = (byte)(data_length & 0x00ff);

                Array.Copy(Encoding.ASCII.GetBytes(command), 0, send_data, 4, data_length);

                for (int i = 0; i < data_length + 4; i++)
                    chk += send_data[i];

                send_data[data_length + 4] = chk;
                send_data[data_length + 5] = 0x03;   // ETX

                _tcpClient.Client.Send(send_data);
                _tLastCommandToEco = DateTime.Now;

                if (_bLogDetails) LogFile.WriteMessageToLogFile("{0} Send: {1}", this.Name, command);
            }
            catch (Exception excp)
            {
                LogFile.WriteErrorToLogFile("{0} Exception: {1} in function SendCommand.", this.Name, excp.Message);
                return false;
            }

            return true;
        }

#endregion

        #region Database methods

        private bool GetLocationDefaultParameters(Container cont)
        {

            return false;
        }

        private bool ProcessEmptying(string strIdent)
        {
            strIdent = strIdent.Substring(6);
            double lat = 0.0F, lng = 0.0F;
            int posComma = 0;
            int preferedMaterialId = -1;

            if ((posComma = strIdent.IndexOf(',')) != -1)
            {
                try
                {
                    string[] toks = strIdent.Split(new char[] { ',' });
                    if (toks.GetLength(0) > 2)
                    {
                        _container.ModemFirmwareVersion = toks[1].Trim();
                        _container.ControllerFirmwareVersion = toks[2].Trim();
                        _container.ModemSignalQuality = System.Convert.ToUInt16(toks[3].Trim());
                        lat = System.Convert.ToDouble(toks[4].Trim(), CultureInfo.InvariantCulture);
                        lng = System.Convert.ToDouble(toks[5].Trim(), CultureInfo.InvariantCulture);

                        if (toks.GetLength(0) > 6)
                        {
                            _container.SupplyVoltage = System.Convert.ToInt32(toks[6].Trim());
                        }

                        if (toks.GetLength(0) > 7)
                        {
                            preferedMaterialId = System.Convert.ToInt32(toks[7].Trim());
                        }

                        if (toks.GetLength(0) > 8)
                        {
                            _container.ActualFillingLevel = 0;
                        }
                    }
                }
                catch (Exception excp)
                {
                    LogFile.WriteErrorToLogFile("Exception: {0}, while trying to parse identify string", excp.Message);
                }

                _container.IccId = strIdent.Substring(0, posComma);

                LogFile.WriteMessageToLogFile("Get container parameters for IccId: {0}", _container.IccId);
                ContainerParamsDto contParams = (ContainerParamsDto)ConnectionControl.SkpApiClient.GetContainerParams(_container.IccId);

                _container.ContainerId = (int)contParams.Id;

                try
                {
                    UpdateGeoPosition newGeoPos = new UpdateGeoPosition(lat, lng);
                    ConnectionControl.SkpApiClient.UpdateContainerGeoPosition(_container.ContainerId, newGeoPos);
                }
                catch (Exception excp)
                {
                    LogFile.WriteErrorToLogFile("{0} Exception: {1}, while trying to update container geo position: {2}, {3}", this.Name, excp.Message, lat, lng);
                }

                try
                {
                    ConnectionControl.SkpApiClient.RemoveContainerFromAllLocations(_container.ContainerId);
                }
                catch (Exception excp)
                {
                    LogFile.WriteErrorToLogFile("{0} Exception: {1}, while trying to update container geo position: {2}, {3}", this.Name, excp.Message, lat, lng);
                }
            }
            else
            {
                LogFile.WriteErrorToLogFile("Invalid identification string: {0}", strIdent);
                return false;
            }

            return true;
        }

        private bool GetContainerAndLocation(string strIdent)
        {
            bool retval = true;
            double lat = 0.0F, lng = 0.0F;
            int preferedMaterialId = -1;

            try
            {
                strIdent = strIdent.Substring(5);
                int posComma = 0;

                if ((posComma = strIdent.IndexOf(',')) != -1)
                {
                    try
                    {
                        string[] toks = strIdent.Split(new char[] { ',' });
                        if (toks.GetLength(0) > 2)
                        {
                            _container.ModemFirmwareVersion = toks[1].Trim();
                            _container.ControllerFirmwareVersion = toks[2].Trim();
                            _container.ModemSignalQuality = System.Convert.ToUInt16(toks[3].Trim());
                            lat = System.Convert.ToDouble(toks[4].Trim(), CultureInfo.InvariantCulture);
                            lng = System.Convert.ToDouble(toks[5].Trim(), CultureInfo.InvariantCulture);

                            if (toks.GetLength(0) > 6)
                            {
                                _container.SupplyVoltage = System.Convert.ToInt32(toks[6].Trim());
                            }

                            if (toks.GetLength(0) > 7)
                            {
                                preferedMaterialId = System.Convert.ToInt32(toks[7].Trim());
                            }

                            if (toks.GetLength(0) > 8)
                            {
                                _container.ActualFillingLevel = System.Convert.ToInt32(toks[8].Trim());
                            }

                            if (toks.GetLength(0) > 9)
                            {
                                _container.IsConfigrationInvalid = toks[9].Trim() == "true";
                                LogFile.WriteMessageToLogFile("{0} Configuration invalid: {1}, {2}", this.Name, _container.IsConfigrationInvalid, toks[9].Trim());
                            }

                        }
                    }
                    catch (Exception excp)
                    {
                        LogFile.WriteErrorToLogFile("Exception: {0}, while trying to parse identify string", excp.Message);
                        return false;
                    }

                    _container.IccId = strIdent.Substring(0, posComma);
                }
                else
                {
                    LogFile.WriteErrorToLogFile("Invalid identification string: {0}", strIdent);
                    return false;
                }

                List<Location> locations = new List<Location>();

                try
                {
                    Location virtLocation = new Location();     // virtual Location object if no location for container was found

                    LogFile.WriteMessageToLogFile("Get container parameters for IccId: {0}", _container.IccId);
                    ContainerParamsDto contParams = (ContainerParamsDto)ConnectionControl.SkpApiClient.GetContainerParams(_container.IccId);
                    SkpContainerFeaturesDto contFeatures = (SkpContainerFeaturesDto)ConnectionControl.SkpApiClient.GetContainerMachineData((int)contParams.Id);

                    this.Name = String.Format("ContainerID {0}:", contParams.Id);
                    LogFile.WriteMessageToLogFile("{0} Ident: {1}", this.Name, strIdent);

                    try
                    {
                        ConnectionControl.SkpApiClient.UpdateContainerLastCommunication(contParams.Id, new UpdateLastCommunication(DateTime.Now));
                    }
                    catch (Exception excp)
                    {
                        LogFile.WriteErrorToLogFile("Exception: {0} while trying to store last communication date", excp.Message);
                    }

                    var singlefeatureList =  contFeatures.SingleValue;

                    LogFile.WriteMessageToLogFile("{0} SingleFeatures: {1}", this.Name, singlefeatureList.Count);

                    foreach (string feature in singlefeatureList.Values)
                    {
                        LogFile.WriteMessageToLogFile("{0}  Feature: {1}", this.Name, feature);

                        if (feature.IndexOf("Hubkipp Grundausstattung") != -1)
                        {
                            _container.IsLiftTiltEquipped = true;
                        }
                        else if (feature.IndexOf("Lichtschrankensteuerung") != -1)
                        {
                            _container.IsExternalStartEquipped = true;
                        }
                        else if (feature.IndexOf("Pressenstellung") != -1)
                        {
                            if (feature.IndexOf("offen") != -1)
                            {
                                _container.PressPosition = true; // is inverted
                            }
                            else
                            {
                                _container.PressPosition = false; // is inverted
                            }
                        }
                        else if (feature.IndexOf("Schaltschrank 2.0") != -1)
                        {
                            _container.Is2DotZero = true;
                        }
                    }

                    var multifeatureList = contFeatures.MultipleValue;

                    LogFile.WriteMessageToLogFile("MultiFeatures: {0}", multifeatureList.Count);

                    foreach (List<string> featureList in multifeatureList.Values)
                    {
                        foreach (string feature in featureList)
                        {
                            LogFile.WriteMessageToLogFile("{0}  Feature: {1}", this.Name, feature);

                            if (feature.IndexOf("Hubkipp Grundausstattung") != -1)
                            {
                                _container.IsLiftTiltEquipped = true;
                            }
                            else if (feature.IndexOf("Lichtschrankensteuerung") != -1)
                            {
                                _container.IsExternalStartEquipped = true;
                            }
                            else if (feature.IndexOf("Start mit Identsystem") != -1)
                            {
                                _container.IsIdentSystemEquipped = true;
                            }
                            else if (feature.IndexOf("Druckeinst.") != -1)
                            {
                                if (feature.IndexOf("hohe Dichte") != -1)
                                {
                                    virtLocation.FullWarningLevel = 75;
                                    virtLocation.FullErrorLevel = 90;
                                    virtLocation.MachineUtilization = 70;
                                }
                                else
                                {
                                    virtLocation.FullWarningLevel = 60;
                                    virtLocation.FullErrorLevel = 60;
                                }
                            }
                        }
                    }

                    _container.IsRetroFit = (bool)contParams.Retrofit;

                    _container.ContainerId = (int)contParams.Id;
                    this.Name = String.Format("ContainerID {0}:", _container.ContainerId);
                    _container.MobileNumber = contParams.GsmNumber;
                    _container.IdentString = contParams.InternalIdentNumber;
                    _container.DeviceNumber = contParams.DeviceNumber;
                    _container.FirmwareVersion = ConnectionControl.ActualModemFirmwareVersion; // contParams.FirmwareVersion; // 19.12.2018 - at the moment not used
                    _container.ReadPointer = (int)contParams.ReadPointer;
                    _container.WritePointer = (int)contParams.WritePointer;
                    _container.OperatorId = (int)contParams.OperatorId;
                    
                    LogFile.WriteMessageToLogFile("{0} Found parameters: {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", this.Name, _container.MobileNumber, _container.IdentString, _container.DeviceNumber,
                        _container.FirmwareVersion, _container.OperatorId, _container.WritePointer, _container.ReadPointer, contParams.Retrofit);

                    // get operator name
                    OperatorParamsDto opParams = ConnectionControl.SkpApiClient.GetOperatorParams(_container.OperatorId);
                    _container.OperatorName = opParams.OperatorName;
                    _container.OperatorLanguage = opParams.LanguageCode;

                    LogFile.WriteMessageToLogFile("{0} Operator: {1}, Language: {2}", this.Name, _container.OperatorName, opParams.LanguageCode);

                    try
                    {
                        UpdateGeoPosition newGeoPos = new UpdateGeoPosition(lat, lng);
                        ConnectionControl.SkpApiClient.UpdateContainerGeoPosition(_container.ContainerId, newGeoPos);
                    }
                    catch (Exception excp)
                    {
                        LogFile.WriteErrorToLogFile("{0} Exception: {1}, while trying to update container geo position: {2}, {3}", this.Name, excp.Message, lat, lng);
                    }

                    if (_container.Is2DotZero)
                    {
                        LogFile.WriteMessageToLogFile("{0} Machine is of type: Schaltschrank 2.0", this.Name);
                    }
                    else
                    {

                        LogFile.WriteMessageToLogFile("{0} Get locations for this geopos", this.Name);

                        GetSkpLocations getLoc = new GetSkpLocations(lat + 0.0030F, lng + 0.0030F, lat - 0.0030F, lng - 0.0030F);

                        foreach (var location in ConnectionControl.SkpApiClient.GetLocationsForOperator(_container.OperatorId, getLoc))
                        {
                            Location loc = new Location();

                            loc.LocationId = (int)location.LocationId;
                            loc.Name = location.Name;
                            //                        loc.MaterialId = (int)location.LocationTypeId;


                            loc.Latitude = (double)location.Latitude;
                            loc.Longitude = (double)location.Longitude;
                            //                            loc.IsWatchdogActive = (bool)location.LocationMonitoringActive;
                            if (contFeatures.MachineSettings != null)
                            {
                                loc.PressStrokes = (int)contFeatures.MachineSettings.NumberOfPresses;
                                loc.PressPosition = (bool)contFeatures.MachineSettings.PressPosition;
                                loc.MachineUtilization = (int)contFeatures.MachineSettings.MachineUtilization;
                                loc.FullErrorLevel = (int)contFeatures.MachineSettings.PercentFullMessage;
                                loc.FullWarningLevel = (int)contFeatures.MachineSettings.PercentPreFullMessage;
                            }
                            else
                            {
                                LogFile.WriteErrorToLogFile("{0} MachineSettings == null -> use default settings", this.Name);
                                loc.PressStrokes = 3;
                                loc.PressPosition = false;
                                loc.MachineUtilization = 100;
                                loc.FullErrorLevel = 95;
                                loc.FullWarningLevel = 75;
                            }

                            loc.PreferredFractionId = location.FractionId;

                            if (location.NightLockActive)
                            {
                                DateTime nlStart = (DateTime)location.NightLockStart;
                                DateTime nlStop = (DateTime)location.NightLockStop;
                                if (nlStop < nlStart)
                                {
                                    nlStop = nlStop.AddDays(1);
                                }

                                TimeSpan ts = nlStop.Subtract(nlStart);

                                loc.NightLockStart = nlStart.Hour * 60 + nlStart.Minute;
                                loc.NightLockEnd = nlStop.Hour * 60 + nlStop.Minute;
                                loc.NightLockDuration = (int)ts.TotalMinutes;
                            }
                            else
                            {
                                loc.NightLockStart = 0;
                                loc.NightLockDuration = 0;
                            }

                            locations.Add(loc);
                        }


                        LogFile.WriteMessageToLogFile("{0} Found {1} locations within search area.", this.Name, locations.Count);
                    }

                    //double minDevLat = 90.0F;
                    //double minDevLong = 180.0F;
                    Location bestLocation = null;


                    // check for preferred location if available
                    if (contParams.PreferredLocation != null)
                    {
                        foreach (Location loc in locations)
                        {
                            if (loc.LocationId == contParams.PreferredLocation.LocationId)
                            {
                                SkpLocationDto locationDto = ConnectionControl.SkpApiClient.GetLocationById(loc.LocationId);
                                loc.MaterialName = locationDto.FractionName;

                                bestLocation = loc;

                                LogFile.WriteMessageToLogFile("{0} Found preferred location ({1}:{2}:{3})", this.Name, loc.LocationId, loc.Name, loc.MaterialName);
                                break;
                            }
                        }

                        // check for preferred fractionId if available if still no location is valid
                        if (bestLocation == null && contParams.PreferredLocation.FractionId != null)
                        {
                            foreach (Location loc in locations)
                            {
                                if (loc.PreferredFractionId == contParams.PreferredLocation.FractionId)
                                {
                                    SkpLocationDto locationDto = ConnectionControl.SkpApiClient.GetLocationById(loc.LocationId);
                                    loc.MaterialName = locationDto.FractionName;

                                    bestLocation = loc;

                                    LogFile.WriteMessageToLogFile("{0} Found preferred fraction on location ({1}:{2}:{3}:{4})", this.Name, loc.LocationId, loc.Name, loc.MaterialName,
                                        loc.PreferredFractionId);

                                    break;
                                }
                            }
                        }
                    }


                    if (bestLocation == null && contParams.LastLocation != null)
                    {
                        foreach (Location loc in locations)
                        {
                            if (loc.LocationId == contParams.LastLocation.LocationId)
                            {
                                SkpLocationDto locationDto = ConnectionControl.SkpApiClient.GetLocationById(loc.LocationId);
                                loc.MaterialName = locationDto.FractionName;

                                bestLocation = loc;

                                LogFile.WriteMessageToLogFile("{0} Found last location ({1}:{2}:{3})", this.Name, loc.LocationId, loc.Name, loc.MaterialName);
                                break;
                            }
                        }

                        // check for preferred fractionId if available if still no location is valid
                        if (bestLocation == null && contParams.LastLocation.FractionId != null)
                        {
                            foreach (Location loc in locations)
                            {
                                if (loc.PreferredFractionId == contParams.LastLocation.FractionId)
                                {
                                    SkpLocationDto locationDto = ConnectionControl.SkpApiClient.GetLocationById(loc.LocationId);
                                    loc.MaterialName = locationDto.FractionName;

                                    bestLocation = loc;

                                    LogFile.WriteMessageToLogFile("{0} Found last fraction on location ({1}:{2}:{3}:{4})", this.Name, loc.LocationId, loc.Name, loc.MaterialName,
                                        loc.PreferredFractionId);

                                    break;
                                }
                            }
                        }
                    }

                    if (bestLocation == null && !_container.Is2DotZero)
                    {
                        double minDevLat = 90.0F;
                        double minDevLong = 180.0F;

                        foreach (Location loc in locations)
                        {
                            double devLat = Math.Abs(lat - loc.Latitude);
                            double devLng = Math.Abs(lng - loc.Longitude);

                            LogFile.WriteMessageToLogFile("{0} Location: {1}, deviation is lat: {2}, long: {3}", this.Name, loc.LocationId, devLat, devLng);

                            if (devLat < minDevLat && devLng < minDevLong)
                            {
                                minDevLat = devLat;
                                minDevLong = devLng;
                                bestLocation = loc;
                            }
                        }
                    }

                    if (bestLocation != null)
                    {
                        this._location = bestLocation;

                        LogFile.WriteMessageToLogFile("{0} Selected Location: {1}, {2}", this.Name, bestLocation.LocationId, bestLocation.Name);
                        LogFile.WriteMessageToLogFile("{0} Nightlockstart: {1}, End: {2}, Duration: {3}", this.Name, bestLocation.NightLockStart, bestLocation.NightLockEnd, bestLocation.NightLockDuration);

//                        _location.MaterialName = GetMaterialName(_location.MaterialId, _container.OperatorLanguage);

                        UpdateContainer updateContainer = new UpdateContainer(_container.ContainerId);
                        ConnectionControl.SkpApiClient.UpdateLocationContainer(_location.LocationId, updateContainer);

                        _location.IsRetroKitEquipped = _container.IsRetroFit;
                        _location.IsLiftTiltEquipped = _container.IsLiftTiltEquipped;
                    }
                    else
                    {
                        LogFile.WriteMessageToLogFile("{0} No location found within specified area!", this.Name);
                        ConnectionControl.SkpApiClient.RemoveContainerFromAllLocations(_container.ContainerId);

                        _location.PressStrokes = (int)contFeatures.MachineSettings.NumberOfPresses;
                        _location.PressPosition = contFeatures.MachineSettings.PressPosition;
                        _location.FullErrorLevel = contFeatures.MachineSettings.PercentFullMessage;
                        _location.FullWarningLevel = contFeatures.MachineSettings.PercentPreFullMessage;
                        _location.MachineUtilization = contFeatures.MachineSettings.MachineUtilization;
                        _location.IsLiftTiltEquipped = _container.IsLiftTiltEquipped;
                        _location.IsRetroKitEquipped = _container.IsRetroFit;

#if false
                        if (_container.Is2DotZero)
                        {
                            // try to get standard parameters for container from 2.0 machine configuration database
                            // there are containers listet which do not have falconic website support
                            // these are configured with a simple text file
                            Location loc = _controller.Get2DotZeroLocation(_container.ContainerId);
                            if (loc != null)
                            {
                                _location.FullErrorLevel = loc.FullErrorLevel;
                                _location.FullWarningLevel = loc.FullWarningLevel;

                                _location.IsLiftTiltEquipped = _container.IsLiftTiltEquipped;   // do no more take this settings from database because it is comming from system features
                                _location.IsRetroKitEquipped = _container.IsRetroFit;  // do no more take this settings from database because it is comming from system features
                                _location.PressPosition = _container.PressPosition;

                                _location.MachineUtilization = loc.MachineUtilization;
                                _location.PressStrokes = loc.PressStrokes;

                                LogFile.WriteMessageToLogFile("{0} Take data from 2.0 database: {1}, {2}, {3}, {4}, {5}, {6}, {7}", this.Name,
                                    _location.PressStrokes, _location.PressPosition, _location.FullWarningLevel, _location.FullErrorLevel,
                                    _location.MachineUtilization, _location.IsLiftTiltEquipped, _location.IsRetroKitEquipped);
                            }
                            else
                            {
                                _location.FullErrorLevel = 90;
                                _location.FullWarningLevel = 75;

                                _location.IsLiftTiltEquipped = _container.IsLiftTiltEquipped;
                                _location.IsRetroKitEquipped = _container.IsRetroFit;
                                _location.PressPosition = _container.PressPosition;

                                _location.MachineUtilization = 100;
                                _location.PressStrokes = 3;

                                LogFile.WriteMessageToLogFile("{0} Not found in 2.0 database take default settings: {1}, {2}, {3}, {4}, {5}, {6}, {7}", this.Name,
                                    _location.PressStrokes, _location.PressPosition, _location.FullWarningLevel, _location.FullErrorLevel,
                                    _location.MachineUtilization, _location.IsLiftTiltEquipped, _location.IsRetroKitEquipped);
                            }
                        }
                        else
                        { 
                            _location = virtLocation;
                            _location.IsLiftTiltEquipped = _container.IsLiftTiltEquipped;
                            _location.IsRetroKitEquipped = _container.IsRetroFit;
                            _location.PressPosition = _container.PressPosition;
                        }
#endif
                    }

                    UpdateGeoPosition x = new UpdateGeoPosition(lat, lng);
                    ConnectionControl.SkpApiClient.UpdateContainerGeoPosition(_container.ContainerId, x);
                }
                catch (Exception e)
                {
                    LogFile.WriteErrorToLogFile("{0} {1} in \'GetContainerAndLocation\' appeared.", this.Name, e.Message);
                    retval = false;
                }
                finally
                {
                }

            }
            catch (Exception excp)
            {
                LogFile.WriteErrorToLogFile("{0} Exception: {1} while trying to get container parameters", this.Name, excp.Message);
            }

            // remove any possible zombie clients with the same name
            lock (_controller.ConnectedClients.SyncRoot)
            {
                for (int i = _controller.ConnectedClients.Count - 1; i >= 0; i--)
                {
                    ClientConnection client = (ClientConnection)_controller.ConnectedClients[i];

                    if (client != this && client.Name == this.Name)
                    {
                        client.Stop();
                    }
                }
            }

            return retval;
        }

#endregion

#region Thread routines

        /// <summary>
        /// SKP connection state machine
        /// </summary>
        private void DoWork()
        {
            string str;
            WaitHandle[] waitHandles = new WaitHandle[2];
            int index;
            int numberOfExpectedEntries = 0;
            bool bBreak = false;

            waitHandles[0] = _receivedEvent;
            waitHandles[1] = _stopHandle;

            // initialize timeouts
            _tLastWritePointerQuery = DateTime.Now;
            _destTime = DateTime.Now;

            try
            {
                do
                {
                    switch (state)
                    {
                        case _CLIENT_STATE.INIT:
                            if (_receivedFrames.Count > 0 && (str = getFrame("%NUM")) != "")
                            {
                                if (str.IndexOf("%NUME") != -1)
                                {
                                    ProcessEmptying(str);
                                    state = _CLIENT_STATE.STOP;
                                }
                                else if (GetContainerAndLocation(str))
                                {
                                    int? numberOfStartings = 0;
                                    int? operationMinutes = 0;
                                    var hwInformation = ConnectionControl.SkpApiClient.GetContainerHardwareInformation(_container.ContainerId);

                                    _bIdentified = true;

                                    LogFile.WriteMessageToLogFile("{0} Found entry for container: {1}, ContainerId: {2}, on Location: {3}, Material: {4}, MobileNumber: {5}, PressStrokes: {6}, PreFullLevel: {7}, FullLevel: {8}",
                                        this.Name, _container.IdentString, _container.ContainerId, _location.LocationId, _location.MaterialName,
                                        _container.MobileNumber, _location.PressStrokes, _location.FullWarningLevel, _location.FullErrorLevel);

                                    if (_container.IsConfigrationInvalid)
                                    {

                                        numberOfStartings = hwInformation.NumberOfStartings;
                                        operationMinutes = hwInformation.OperatingMinutes;

                                        LogFile.WriteMessageToLogFile("{0} Container configuration seems to be corrupted so take infos from database. OperationMinutes: {1}, NumStartings: {2}", this.Name,
                                            operationMinutes, numberOfStartings);
                                    }

                                    if (hwInformation.TargetFirmwareUpdateDate >= DateTime.UtcNow && hwInformation.TargetFirmwareVersion != _container.FirmwareVersion)
                                    {
                                        LogFile.WriteMessageToLogFile("{0} Time to update to version: {1}, actual: {2}", this.Name, hwInformation.TargetFirmwareVersion, hwInformation.FirmwareVersion);
                                        _container.FirmwareVersion = hwInformation.TargetFirmwareVersion;
                                    }

                                    string strCommand = String.Format("#CON={0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},", _destTime.ToString("ddMMyyyy"), _destTime.ToString("HHmmss"),
                                        _container.ContainerId, _container.OperatorId, _location.MaterialId, _container.MobileNumber, _location.PressStrokes, _location.PressPosition ? 1 : 0, _location.FullWarningLevel, _location.FullErrorLevel,
                                        _container.FirmwareVersion, _location.NightLockStart, _location.NightLockDuration, _container.ContainerTypeId, _location.MachineUtilization,
                                        _location.IsLiftTiltEquipped ? 1 : 0, _location.IsRetroKitEquipped ? 1 : 0, _container.IsExternalStartEquipped ? 1: 0, operationMinutes, numberOfStartings, _container.IsIdentSystemEquipped ? 1 : 0);

                                    if (SendCommand(strCommand))
                                        state = _CLIENT_STATE.WAIT_CONFIG_ACK;
                                    else
                                        state = _CLIENT_STATE.ERROR;
                                }
                                else
                                {
                                    LogFile.WriteErrorToLogFile("SIMCard ID: {0} was not found!", str);
                                    state = _CLIENT_STATE.ERROR;
                                    break;
                                }
                            }
                            break;

                        case _CLIENT_STATE.WAIT_CONFIG_ACK:
                            if (_receivedFrames.Count > 0 && (str = getFrame("%STAT")) != "")
                            {
                                if (analyseStatus(str))
                                {
                                    if (_container.NumberOfStoredEvents == 0)
                                    {
                                        if (_container.WritePointer != _container.ReadPointer)
                                        {
                                            state = _CLIENT_STATE.START_READ_JOURNAL;
                                        }
                                        else
                                        {
                                            if (_container.IsIdentSystemEquipped)
                                                state = _CLIENT_STATE.ONLINE;
                                            else
                                                state = _CLIENT_STATE.STOP;
                                        }
                                    }
                                    else
                                    {
                                        state = _CLIENT_STATE.READ_EVENTS;
                                        SendCommand("#EVT?");
                                    }
                                }
                                else
                                    state = _CLIENT_STATE.ERROR;
                            }
                            else if (DateTime.Now.Subtract(_tLastCommandToEco).TotalSeconds > 15)
                            {
                                LogFile.WriteMessageToLogFile("{0} Timeout while waiting for config acknowledge", Name);
                                state = _CLIENT_STATE.ERROR;
                            }
                            break;

                        case _CLIENT_STATE.READ_EVENTS:
                            if (_receivedFrames.Count > 0 && (str = getFrame("%EVT=")) != "")
                            {
                                ReadEvents(str);

                                if (_container.WritePointer != _container.ReadPointer)
                                {
                                    state = _CLIENT_STATE.START_READ_JOURNAL;
                                }
                                else
                                {
                                    if (_container.IsIdentSystemEquipped)
                                    {
                                        state = _CLIENT_STATE.ONLINE;
                                        _bPermantOnline = true;
                                    }
                                    else
                                        state = _CLIENT_STATE.STOP;
                                }
                            }
                            else if (DateTime.Now.Subtract(_tLastCommandToEco).TotalSeconds > 15)
                            {
                                LogFile.WriteMessageToLogFile("{0} Timeout while waiting for stored events", Name);
                                state = _CLIENT_STATE.ERROR;
                            }
                            break;

                        case _CLIENT_STATE.START_READ_JOURNAL:
                            {
                                if (_container.WritePointer > _container.ReadPointer)
                                    numberOfExpectedEntries = _container.WritePointer - _container.ReadPointer;
                                else
                                    numberOfExpectedEntries = _container.WritePointer + (_container.JournalSize - _container.ReadPointer);

                                // maximum allowed in one frame is 30
                                if (numberOfExpectedEntries > 30)
                                    numberOfExpectedEntries = 30;

                                String command = String.Format("#JOU?{0},{1}", _container.ReadPointer, numberOfExpectedEntries);
                                SendCommand(command);

                                state = _CLIENT_STATE.READ_JOURNAL;

                                break;
                            }
                        case _CLIENT_STATE.READ_JOURNAL:
                            if (_receivedFrames.Count > 0 && (str = getFrame("%JOU=")) != "")
                            {
//                                if (!_container.Is2DotZero)   // removed 03.01.2019 see #565
                                    ParseJournalEntries(str, numberOfExpectedEntries);
                                int newReadPointer = _container.ReadPointer + numberOfExpectedEntries;

                                if (newReadPointer >= _container.JournalSize)
                                    newReadPointer = newReadPointer - _container.JournalSize;

                                SendCommand(String.Format("#RDP={0},{1}", newReadPointer, "xyz"));  // no security code

                                state = _CLIENT_STATE.WAIT_READPOINTER_ACK;
                            }
                            else if (DateTime.Now.Subtract(_tLastCommandToEco).TotalSeconds > 15)
                            {
                                LogFile.WriteMessageToLogFile("{0} Timeout while waiting for stored events", Name);
                                state = _CLIENT_STATE.ERROR;
                            }
                            break;

                        case _CLIENT_STATE.WAIT_READPOINTER_ACK:

                            if (_receivedFrames.Count > 0 && (str = getFrame("%RDP=")) != "")
                            {
                                _container.ReadPointer = Convert.ToInt32(str.Substring(5).Trim());

                                if (_container.WritePointer == _container.ReadPointer)
                                {
                                    if (_container.IsIdentSystemEquipped)
                                    {
                                        state = _CLIENT_STATE.ONLINE;
                                        _bPermantOnline = true;
                                    }
                                    else
                                        state = _CLIENT_STATE.STOP;
                                }
                                else
                                {
                                    state = _CLIENT_STATE.START_READ_JOURNAL;
                                }
                            }
                            else if (DateTime.Now > _tLastCommandToEco.AddSeconds(60))
                            {
                                LogFile.WriteErrorToLogFile("{0} Timeout while waiting read pointer acknowledge", this.Name);
                                state = _CLIENT_STATE.ERROR;
                            }
                            break;

                        case _CLIENT_STATE.STOP:
                            ConnectionControl.SkpApiClient.UpdateContainerLastCommunication(_container.ContainerId, new UpdateLastCommunication(DateTime.Now));
                            LogFile.WriteMessageToLogFile("{0} Stop client -> up to date", Name);
                            Stop();
                            break;

                        case _CLIENT_STATE.ONLINE:
                            if (_receivedFrames.Count > 0)
                            {
                                // events are no more supported
                                // but we use this here to check for customer access
                                // or status reports
                                if ((str = getFrame("%EVT=")) != "")
                                    ReadEvents(str);

                                // update last communication here
                                try
                                {
                                    ConnectionControl.SkpApiClient.UpdateContainerLastCommunication(_container.ContainerId, new UpdateLastCommunication(DateTime.Now));
                                }
                                catch (Exception excp)
                                {
                                    LogFile.WriteErrorToLogFile("Exception: {0} while trying to store last communication date", excp.Message);
                                }
                            }
                            else if (DateTime.Now > _tLastCommandToEco.AddMinutes(3))
                            {
                                // do every 3 minutes a status request
                                // to get current read/write pointers
                                // and to keep socket alive
                                SendCommand("#STAT");
                                state = _CLIENT_STATE.WAIT_CONFIG_ACK;
                            }
                            break;

                        case _CLIENT_STATE.ERROR:
                            LogFile.WriteErrorToLogFile("{0} Stop client due to error", Name);
                            Stop();
                            break;

                    }

                    // wait for either new frame received or someone wants to stop us
                    if ((index = WaitHandle.WaitAny(waitHandles, 100, false)) != WaitHandle.WaitTimeout)
                    {
                        if (index == 1)
                        {
                            break;
                        }
                    }
                } while (!bBreak);

                _stopHandle.Set(); // set stop handle also for receiver thread
            }
            catch (Exception excp)
            {
                LogFile.WriteErrorToLogFile("{0} Exception ({1}) in !!DoWork!! occured", this.Name, excp.Message);
            }

            LogFile.WriteMessageToLogFile("{0} Workerthread stopped.", this.Name);

            Controller.RemoveClient(this);

            return;
        }

        /// <summary>
        /// Receiver thread
        /// </summary>
        private void Receive()
        {
            Socket socket = TcpClient.Client;
            _tLastCommandFromEco = DateTime.Now;
            byte[] frame = null;
            int length = 0, offset = 0;

            try
            {
                do
                {
                    // normal operation
                    if (socket.Poll(100000, SelectMode.SelectRead))
                    {
                        if (socket.Available > 0)
                        {
                            if ((length = socket.Available) > 0)
                            {
                                byte[] bytes = new byte[length + offset];

                                if (offset > 0)
                                    Array.Copy(frame, 0, bytes, 0, offset);

                                // get new bytes
                                socket.Receive(bytes, offset, length, SocketFlags.None);

                                frame = bytes;
                                offset += length;

                                if (offset >= 4)
                                {
                                    short frame_length = (short)(frame[2] << 8 | frame[3] + 6);
                                    // additionally we can do a comparision of checksums,
                                    // but in this case a higher level is done already by the sockets layer
                                    // so if last char is an ETX and the frame_length matches we consider a valid frame
                                    if (offset == frame_length && frame[offset - 1] == 0x03)
                                    {
                                        offset = 0;
                                        string str = Encoding.ASCII.GetString(frame, 4, frame_length - 6);
                                        if (_bLogDetails) LogFile.WriteMessageToLogFile("{0} Received: {1}", this.Name, str);
                                        lock (_receivedFrames.SyncRoot)
                                        {
                                            _receivedFrames.Enqueue(str);
                                            _receivedEvent.Set();
                                        }
                                    }
                                }
                            }

                            _tLastCommandFromEco = DateTime.Now;
                        }
                        else
                        {
                            LogFile.WriteMessageToLogFile("{0} Client connection has been closed remotely", this.Name);
                            Stop();
                            break;
                        }
                    }
                    else if (offset != 0 && DateTime.Now > _tLastCommandFromEco.AddSeconds(2))
                    {
                        offset = 0;
                    }
                    else if (DateTime.Now > _tLastCommandFromEco.Add(new TimeSpan(0, _keepAliveInterval + 2, 0)))
                    {
                        LogFile.WriteMessageToLogFile("{0} End Client connection due to timeout", this.Name);
                        Stop();
                        break;
                    }

                    if (_stopHandle.WaitOne(3, false))
                    {
                        // set stop handle also for worker thread
                        _stopHandle.Set();
                        break;
                    }
                } while (true);
            }
            catch (Exception excp)
            {
                LogFile.WriteErrorToLogFile("{0} Exception ({1}) in !!Receive!! occured", this.Name, excp.Message);
            }

            LogFile.WriteMessageToLogFile("{0} disconnected", this.Name);

            if (_tcpClient != null)
                _tcpClient.Close();
        }

#endregion
    }
}
