using Luthien;
using FieldAreaNetwork;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Spatial;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Net.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;
using System.Security.Cryptography;

namespace ConnectionService
{
    [Serializable]
    public class ConnectionControl : MarshalByRefObject
    {
        #region Members

        private ArrayList _clients = new ArrayList(100);	        // list of actual connected clients, allow 100 clients as default, can be increased

        private Mutex _emailMutex = new Mutex();

        #endregion

        #region Properties

        /// <summary>
        /// List of connected clients
        /// </summary>
        public ArrayList ConnectedClients
        {
            get { return _clients; }
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
        }

        #endregion

        #region Methods

        public void SendEmail(string subject, string body, string contact)
        {
            MailMessage mail = new MailMessage("wip.container@poettinger.at", contact);

            mail.Subject = subject;
            mail.Body = body;

            try
            {
                _emailMutex.WaitOne();

                SmtpClient smtp_client = new SmtpClient("10.10.1.1", 25);
            
                smtp_client.UseDefaultCredentials = true;
                smtp_client.Send(mail);
            }
            catch (Exception excp)
            {
                LogFile.WriteErrorToLogFile("Exception: {0}, while sending email", excp.Message);
            }
            finally
            {
                _emailMutex.ReleaseMutex();
            }
        }
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

                LogFile.WriteMessageToLogFile("{0}: Remove Client. Clients left: {1}.", client.Name, _clients.Count);
            }
            catch (Exception excp)
            {
                LogFile.WriteErrorToLogFile("{0}: Error while removing client {1}\nException: {2}", this.GetType().ToString(),
                    client.Name, excp.Message);
            }
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
                tcpl = new TcpListener(IPAddress.Any, 849);
                tcpl.Start();

                try
                {
                    LogFile.WriteMessageToLogFile("{0}: Wait for clients to connect", this.GetType().ToString());

                    do
                    {
                        // check for new clients
                        if (tcpl.Pending())
                        {
                            ClientConnection conn = new ClientConnection();
                            conn.TcpClient = tcpl.AcceptTcpClient();
                            conn.Controller = this;
                            lock (_clients.SyncRoot)
                            {
                                _clients.Add(conn);
                            }

                            Socket socket = conn.TcpClient.Client;
                            IPEndPoint ipEP = (IPEndPoint)socket.RemoteEndPoint;

                            LogFile.WriteMessageToLogFile("{0}: Client Nr: {1} with ip: {2} want to connect.", this.GetType().ToString(), _clients.Count, ipEP.Address.ToString());

                            conn.Start();
                        }

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
        private string _containerType;
        private int _numberOfStoredEvents;          

        #endregion

        #region Objects Members

        private string _modemFirmwareVersion;       // modem firmware version string
        private string _controllerFirmwareVersion;  // controller firmware version string
        private int _ModemSignalQuality;            // controller signal quality information
        private int _actualFillingLevel;            // actual calculated filling level
        private DbGeography _location;              // gps coordinates where container is located
        private DateTime _tLastCommunication;       // time of last transaction

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

        #endregion

        #region Constructor

        public Container()
        {
            NumberOfStoredEvents = 0;
            ModemFirmwareVersion = "unknown";
            ControllerFirmwareVersion = "unknown";
        }

        #endregion
    }

    [Serializable]
    public class Location
    {
        #region members

        private int _locationId;                    // location id
        private string _name;                       // name of location
        private string _materialName;               // name of material
        private int _materialId;                    // kind of material
        private int _pressStrokes;                  // number of strokes to perform in one cycle
        private bool _pressPosition;                // position where press should stop 0 ... back, 1 ... front 
        private int _fullWarningLevel;              // at this level full warning alerting will be done
        private int _fullErrorLevel;                // at this level full alerting will be done
        private double _latitude;                   // real coordinates from database
        private double _longitude;                  // real coordinates from database

        #endregion

        #region members

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

        #endregion

        #region constructor

        public Location()
        {
            Name = "";
            MaterialId = 2; // Karton
            PressStrokes = 3;
            FullWarningLevel = 75;
            FullWarningLevel = 95;
            LocationId = 0;
            _pressPosition = false;
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
            STOP,
            ERROR
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

//        private string _DatabaseConnectionString = "Data Source=LocalHost\\PCONTRAC;Initial Catalog=CCS;Pooling=False;User Id=sa;Password=ikop";
        private string _DatabaseConnectionString = "Data Source=172.22.103.8;Initial Catalog=WIP;Pooling=True;Persist Security Info=False;User Id=sa;Password=Ikopsql01";

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

        public ClientConnection()
        {
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
                string[] toks = frame.Split(new char[] { '=', ',' });

                LogFile.WriteMessageToLogFile("Analyze status with string: {0}", frame);

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

                        ////////////////////////////////////////////////////
                        // [3] GSM Longitude
                        // [4] GSM Latitude

                        //string strGeo = string.Format(CultureInfo.InvariantCulture.NumberFormat, "POINT({0} {1})", toks[3], toks[4]);
                        //// 4326 is most common coordinate system used by GPS/Maps
                        //_container.Location = DbGeography.PointFromText(strGeo, 4326);

                        //LogFile.WriteMessageToLogFile("{0} Location: {1}", this.Name, strGeo);

                        //
                        ////////////////////////////////////////////////////
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

        async Task SendMail(string message, ArrayList recipients)
        {
            try
            {
//                MailjetClient client = new MailjetClient("f45a3bcc36c321165eeeb8061abc1fcf", "0feec0515e1669c976034c6f6dcfc887");
                MailjetClient client = new MailjetClient("c0719838f86777631495b01e3d9fb47f", "83cfc1e5f8c84cb2a549f2e9c386c3fc");
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
                   .Property(Send.Subject, "Container Alerting")
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

        private bool ReadEvents(string frame)
        {
            try
            {
                string[] toks = frame.Split(new char[] { '=', ',' });

                if (toks.Length > 0)
                {
                    int numberOfEvents = (toks.Length - 1) / 2;

                    byte[] tmpHash = new MD5CryptoServiceProvider().ComputeHash(ASCIIEncoding.ASCII.GetBytes(_location.Name));
                    string strHash = ByteArrayToString(tmpHash).Substring(0, 8);

                    SetMapviewHash(strHash, DateTime.Now.AddHours(10));

                    LogFile.WriteMessageToLogFile("Read ({0}) events: {1}", numberOfEvents, frame);

                    for (int i = 0; i < numberOfEvents; i++)
                    {
                        uint type = System.Convert.ToUInt32(toks[i * 2 + 1]);
                        uint time = System.Convert.ToUInt32(toks[i * 2 + 2]);

                        LogFile.WriteMessageToLogFile("Event with type: {0} and time: {1}", type, time);

                        string message = "Meldung: ";
                        
                        if (type == 0)  // Power On Event
                        {
                            message += "HAUPTSCHALTER EIN";
                        }
                        else if (type == 1) // Nearly full Event
                        {
                            message += "VOR VOLL" + String.Format("({0}) %", _container.ActualFillingLevel);
                        }
                        else if (type == 2) // Full Event
                        {
                            message += "VOLL";
                        }
                        else if (type == 3) // Emergency stop
                        {
                            message += "NOTHALT";
                        }
                        else if (type == 4) // Motorschutz
                        {
                            message += "MOTORSCHUTZ";
                        }
                        else if (type == 5) // Störung
                        {
                            message += "STOERUNG";
                        }

                        String smsMessage = "\nIdent Nr.: " + _container.IdentString + "\n";
                        smsMessage += message + "\n";
                        smsMessage += "Fraktion: " + _location.MaterialName + "\n";
                        smsMessage += "SerialNr: " + _container.DeviceNumber + "\n";
                        smsMessage += "Betreiber: " + _container.OperatorName + "\n";
                        smsMessage += "Location: " + _location.Name + "\n";

                        String emailMessage = smsMessage;
                        emailMessage += "Typ: " + _container.ContainerType + "\n";

                        double lat = (double)_location.Latitude;
                        double lng = (double)_location.Longitude;

                        emailMessage += "https://www.entsorgungstechnik.com/Standorte/Infomap?locationid=" + _location.LocationId.ToString() + "&code=";
                        emailMessage += strHash + "\n";

                        LogFile.WriteMessageToLogFile("Alertingmessage: {0}", emailMessage);

                        ArrayList users = new ArrayList();

                        GetAlertingUsers(_location.LocationId, ref users);

                        for (int j = 0; j < users.Count; j++)
                        {
                            AlertingUser user = (AlertingUser)users[j];

                            // do sms alerting over wallner server and email alerting over MailJet
                            if ((user.Flags & (int)ALERTING_FLAGS.SMS_ENABLED) == 0x01)    // do sms alerting
                            {
                                ArrayList users1 = new ArrayList();
                                FieldAreaNetwork.AlertingUser alertUser = new FieldAreaNetwork.AlertingUser();

                                alertUser.ClientName = "SKP";
                                alertUser.TelephoneNumber = user.TelephoneNumber;
                                alertUser.Flags = (int)ALERTING_FLAGS.SMS_ENABLED;
                                alertUser.EmailAddress = "";
                                alertUser.Name = user.Name;

                                users1.Add(alertUser);

                                LogFile.WriteMessageToLogFile("Send SMS for user: {0} to number: {1}", alertUser.Name, alertUser.TelephoneNumber);

                                FieldAreaNetwork.AlertingControl.AddAlarm("alarm.wallner-automation.com", users1, "WIP", smsMessage, false, 0);
                            }
                            if ((user.Flags & 0x02) == 0x02)    // do email alerting
                            {
                                ArrayList users1 = new ArrayList();
                                LogFile.WriteMessageToLogFile("Send Email for user: {0} to address: {1}", user.Name, user.EmailAddress);

                                AlertingUser alertUser = new AlertingUser();
                                alertUser.EmailAddress = user.EmailAddress;
                                alertUser.Name = user.Name;

                                users1.Add(alertUser);

                                SendMail(emailMessage, users1).Wait();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("{0} Exception: {1} in \'ReadEvents\' appeared.", this.Name, e.Message);
            }

            return false;
        }

        private bool processFrame(string frame)
        {
            LogFile.WriteMessageToLogFile("{0} process frame: ({1})", Name, frame);

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

        private bool SetMapviewHash(string strHash, DateTime dueDate)
        {
            bool retval = false;

            LogFile.WriteMessageToLogFile("Set mapview hast to: ({0}), DueDate: ({1})", strHash, dueDate);

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_DatabaseConnectionString))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd_update = new SqlCommand("UPDATE LOCATION SET CONTAINER_ID=@ContainerId, MAPVIEW_HASH=@MapViewHash, MAPVIEW_VALID_TO=@DueDate WHERE LOCATION_ID=@LocationId", sqlConnection))
                    {
                        SqlParameter iContainerId = new SqlParameter("@ContainerId", SqlDbType.Int);
                        iContainerId.Value = _container.ContainerId;

                        SqlParameter strMapViewHash = new SqlParameter("@MapViewHash", SqlDbType.VarChar);
                        strMapViewHash.Value = strHash;

                        SqlParameter tDueDate = new SqlParameter("@DueDate", SqlDbType.DateTime);
                        tDueDate.Value = dueDate;

                        SqlParameter iLocationId = new SqlParameter("@LocationId", SqlDbType.Int);
                        iLocationId.Value = _location.LocationId;

                        cmd_update.Parameters.Add(iContainerId);
                        cmd_update.Parameters.Add(strMapViewHash);
                        cmd_update.Parameters.Add(tDueDate);
                        cmd_update.Parameters.Add(iLocationId);

                        if (cmd_update.ExecuteNonQuery() != 1)
                        {
                            LogFile.WriteErrorToLogFile("ContainerID {0}: Error while trying to map view gash for location: {1}", _container.ContainerId, _location.LocationId);
                        }
                        else
                            retval = true;
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception excp)
            {
                LogFile.WriteErrorToLogFile("Exception: {0} in \'SetMapviewHash\' appeared.", excp.Message);
            }
            finally
            {
            }

            return retval;
        }


        private bool GetOperatorParams(int operatorId)
        {
            bool retval = false;

            string sqlStatement = "SELECT OPERATOR_NAME1, CURRENCY_ID, LANGUAGE_CODE FROM OPERATOR WHERE OPERATOR_ID=@Operator_Id";

            try
            {

                using (SqlConnection sqlConnection = new SqlConnection(_DatabaseConnectionString))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter param_opid = new SqlParameter("@Operator_Id", SqlDbType.Int);
                        param_opid.Value = _container.OperatorId;

                        cmd.Parameters.Add(param_opid);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                _container.OperatorName = (string)reader[0];
//                                currency_id = (int)reader[1];
//                                language_code = (string)reader[2];
                            }
                            else
                            {
                                LogFile.WriteErrorToLogFile("Error while excuting sql statement: {0}", sqlStatement);
                                retval = false;
                            }

                            reader.Close();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("{0} in \'GetOperatorParams\' appeared.", e.Message);
                retval = false;
            }
            finally
            {
            }

            return retval;
        }

        private bool GetMaterialName(int materialId)
        {
            bool retval = false;

            string sqlStatement = "SELECT LOCATIONTYPE FROM LOCATIONTYPE WHERE LOCATIONTYPE_ID=@LocationTypeId";

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_DatabaseConnectionString))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter locTypeId = new SqlParameter("@LocationTypeId", SqlDbType.Int);
                        locTypeId.Value = materialId;

                        cmd.Parameters.Add(locTypeId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                _location.MaterialName = (string)reader[0];
                            }
                            else
                            {
                                LogFile.WriteErrorToLogFile("Error while excuting sql statement: {0}", sqlStatement);
                                retval = false;
                            }

                            reader.Close();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("{0} in \'GetMaterialName\' appeared.", e.Message);
                retval = false;
            }
            finally
            {
            }

            return retval;
        }

        private bool GetContainerType(int containerTypeId)
        {
            bool retval = false;

            string sqlStatement = "SELECT CONTAINER_TYPE FROM CONTAINER_TYPE WHERE CONTAINER_TYPE_ID=@ContainerTypeId";

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_DatabaseConnectionString))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter contTypeId = new SqlParameter("@ContainerTypeId", SqlDbType.Int);
                        contTypeId.Value = containerTypeId;

                        cmd.Parameters.Add(contTypeId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                _container.ContainerType = (string)reader[0];
                            }
                            else
                            {
                                LogFile.WriteErrorToLogFile("Error while excuting sql statement: {0}", sqlStatement);
                                retval = false;
                            }

                            reader.Close();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("{0} in \'GetContainerType\' appeared.", e.Message);
                retval = false;
            }
            finally
            {
            }

            return retval;
        }

        private bool GetAlertingUsers(int location_id, ref ArrayList users)
        {
            string sqlStatement = "SELECT REMOTE_CONTROL.REMOTE_CONTROL_ID, REMOTE_CONTROL_TYPE_ID, MEMO, CONTACT FROM REMOTE_CONTROL INNER JOIN ERROR ON REMOTE_CONTROL.REMOTE_CONTROL_ID=ERROR.REMOTE_CONTROL_ID WHERE ERROR.LOCATION_ID=@LocationId";

            bool retval = true;

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_DatabaseConnectionString))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter p_locationId = new SqlParameter("@LocationId", SqlDbType.Int);
                        p_locationId.Value = location_id;

                        cmd.Parameters.Add(p_locationId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                AlertingUser user = new AlertingUser();
                                user.Id = (int)reader[0];
                                int type = (int)reader[1];

                                user.Name = (string)reader[2];

                                if (type == 2)  // email
                                {
                                    user.Flags = (int)ALERTING_FLAGS.EMAIL_ENABLED;
                                    user.EmailAddress = (string)reader[3];
                                }
                                else if (type == 3) // sms
                                {
                                    user.Flags = (int)ALERTING_FLAGS.SMS_ENABLED;
                                    user.TelephoneNumber = (string)reader[3];
                                }

                                users.Add(user);
                            }
                        }
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("{0} in \'GetAlertingUsers\' appeared.", e.Message);
                retval = false;
            }
            finally
            {
            }

            return retval;
        }

        private bool GetContainerAndLocation(string strIdent)
        {
            bool retval = true;
            double lat = 0.0F, lng = 0.0F; 

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
                        }
                    }
                    catch (Exception excp)
                    {
                        LogFile.WriteErrorToLogFile("Exception: {0}, while trying to parse identify string", excp.Message);
                    }

                    _container.IccId = strIdent.Substring(0, posComma);
                }
                else
                {
                    LogFile.WriteErrorToLogFile("Invalid identification string: {0}", strIdent);
                    return false;
                }

                string sqlStatement = "SELECT * FROM CONTAINER WHERE GSM_NUMBER=@ICCID";

                try
                {
                    using (SqlConnection sqlConnection = new SqlConnection(_DatabaseConnectionString))
                    {
                        sqlConnection.Open();

                        using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                        {
                            SqlParameter param_gsm_number = new SqlParameter("@ICCID", SqlDbType.VarChar);
                            param_gsm_number.Value = _container.IccId;

                            cmd.Parameters.Add(param_gsm_number);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    _container.ContainerId = (int)reader["CONTAINER_ID"];
                                    _container.OperatorId = (int)reader["OPERATOR_ID"];
                                    _container.MobileNumber = (string)reader["GSM_NR2"];
                                    _container.IdentString = (string)reader["INT_IDENTNR"];
                                    _container.DeviceNumber = (string)reader["DEVICE_NUMBER"];
                                    _container.ContainerTypeId = (int)reader["CONTAINER_TYPE_ID"];

                                    GetOperatorParams(_container.OperatorId);
                                    GetContainerType(_container.ContainerTypeId);
                                }
                                else
                                {
                                    LogFile.WriteErrorToLogFile("Error while excuting sql statement: {0}", sqlStatement);
                                    retval = false;
                                }

                                reader.Close();
                            }
                        }

                        sqlStatement = "SELECT * FROM LOCATION WHERE FALCONICLOCATION=1 AND LAT >= @LAT_MIN AND LAT <= @LAT_MAX AND LNG >= @LNG_MIN AND LNG <= @LNG_MAX";

                        using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                        {
                            SqlParameter latMin = new SqlParameter("@LAT_MIN", SqlDbType.Float);
                            latMin.Value = lat - 0.0010F;
                            SqlParameter latMax = new SqlParameter("@LAT_MAX", SqlDbType.Float);
                            latMax.Value = lat + 0.0010F;
                            SqlParameter lngMin = new SqlParameter("@LNG_MIN", SqlDbType.Float);
                            lngMin.Value = lng - 0.0010F;
                            SqlParameter lngMax = new SqlParameter("@LNG_MAX", SqlDbType.Float);
                            lngMax.Value = lng + 0.0010F;

                            LogFile.WriteMessageToLogFile("Location area: {0}, {1}, {2}, {3}", latMin.Value, latMax.Value, lngMin.Value, lngMax.Value);

                            cmd.Parameters.Add(latMin);
                            cmd.Parameters.Add(latMax);
                            cmd.Parameters.Add(lngMin);
                            cmd.Parameters.Add(lngMax);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    _location.LocationId = (int)reader["LOCATION_ID"];
                                    _location.Name = (string)reader["LOCATION"];
                                    _location.MaterialId = (int)reader["LOCATIONTYPE_ID"];
                                    _location.Latitude = (double)reader["LAT"];
                                    _location.Longitude = (double)reader["LNG"];

                                    GetMaterialName(_location.MaterialId);
                                }
                                else
                                {
                                    LogFile.WriteErrorToLogFile("Error while excuting sql statement: {0}", cmd.CommandText);
                                    retval = false;
                                }

                                reader.Close();
                            }
                        }

                        if (_location.LocationId != 0)
                        {
                            sqlStatement = "SELECT * FROM LOCATION_CONFIG WHERE LOCATION_ID=@LOCATIONID";

                            using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                            {
                                SqlParameter locId = new SqlParameter("@LOCATIONID", SqlDbType.Int);
                                locId.Value = _location.LocationId;

                                cmd.Parameters.Add(locId);

                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        _location.PressStrokes = (int)reader["PRESS_STROKES"];
                                        _location.PressPosition = (bool)reader["PRESS_POSITION"];
                                        _location.FullWarningLevel = (int)reader["FULL_WARNING_LEVEL"];
                                        _location.FullErrorLevel = (int)reader["FULL_ERROR_LEVEL"];
                                    }
                                    else
                                    {
                                        LogFile.WriteErrorToLogFile("Error while excuting sql statement: {0}", cmd.CommandText);
                                        retval = false;
                                    }

                                    reader.Close();
                                }
                            }
                        }

                        sqlConnection.Close();
                    }
                }
                catch (Exception e)
                {
                    LogFile.WriteErrorToLogFile("{0} in \'GetContainerAndLocation\' appeared.", e.Message);
                    retval = false;
                }
                finally
                {
                }

            }
            catch (Exception excp)
            {
                LogFile.WriteErrorToLogFile("Exception: {0} while trying to get container parameters", excp.Message);
            }

            string new_name = String.Format("ContainerID {0}:", _container.ContainerId);

            // remove any possible zombie clients with the same name
            lock (_controller.ConnectedClients.SyncRoot)
            {
                for (int i = _controller.ConnectedClients.Count - 1; i >= 0; i--)
                {
                    ClientConnection client = (ClientConnection)_controller.ConnectedClients[i];

                    if (client.Name == new_name)
                    {
                        client.Stop();
                    }
                }
            }

            this.Name = new_name;

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
                                if (GetContainerAndLocation(str))
                                {
                                    _bIdentified = true;

                                    LogFile.WriteMessageToLogFile("{0} Found entry for container: {1}, OperatorId: {2}, on Loaction: {3}, MaterialId: {3}, MobileNumber: {4}, PressStrokes: {5}, WarningLevel: {6}, ErrorLevel: {7}",
                                        this.Name, _container.IdentString, _container.OperatorId, _location.MaterialId, _container.MobileNumber, _location.PressStrokes, _location.FullWarningLevel, _location.FullErrorLevel);

                                    string strCommand = String.Format("#CON={0},{1},{2},{3},{4},{5},{6},{7},{8},{9},", _destTime.ToString("ddMMyyyy"), _destTime.ToString("HHmmss"),
                                        _container.ContainerId, _container.OperatorId, _location.MaterialId, _container.MobileNumber, _location.PressStrokes, _location.PressPosition ? 1 : 0, _location.FullWarningLevel, _location.FullErrorLevel);

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
                                LogFile.WriteMessageToLogFile("{0} Acknowledge configuration received", Name);

                                if (analyseStatus(str))
                                {
                                    if (_container.NumberOfStoredEvents == 0)
                                        state = _CLIENT_STATE.STOP;
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
                                state = _CLIENT_STATE.STOP;
                            }
                            else if (DateTime.Now.Subtract(_tLastCommandToEco).TotalSeconds > 15)
                            {
                                LogFile.WriteMessageToLogFile("{0} Timeout while waiting for stored events", Name);
                                state = _CLIENT_STATE.ERROR;
                            }
                            break;

                        case _CLIENT_STATE.STOP:
                            LogFile.WriteMessageToLogFile("{0} Stop client -> up to date", Name);
                            Stop();
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
