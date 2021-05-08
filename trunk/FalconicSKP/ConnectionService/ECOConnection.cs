using ConnectionService;
using Luthien;
using FieldAreaNetwork;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security.Permissions;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Falconic.Skp.Api.Client;

namespace ECO
{
    public class DataExportParams
    {
        #region Members

        public string Operator;
        public DateTime LastDataExport;
        public int ExportFrequency;
        public string FtpServer;
        public string FtpUser;
        public string FtpPassword;

#endregion
    }

    public class CustomerParams
    {
        #region Members

        public int CustomerId;
        public string CustomerNumber;
        public int SalutationId;
        public string Title; 
        public string FirstName;
        public string LastName;
        public string LogIn;
        public string Password;
        public int CardId;
        public int CardReleaseNumber;
        public int CardTypeId;
        public int PaymentId;
        public int FromAmount;
        public int ToAmount;
        public decimal PositiveCreditBalance;
        public DateTime CardReleaseDate;
        public DateTime LastTransaction;
        public string CompanyName;
        public string Street;
        public string ZIPCode;
        public string City;
        public string PhoneNumber;
        public string MobilePhone;
        public string Email;
        public string LanguageCode;
        public int LanguageId;
        public int PriceHundredKilo;
        public string LocationGroup;
        public int LocationGroupMask;
        public int AddressId;

#endregion

        #region Constructor

        public CustomerParams()
        {
            this.CustomerId = -1;
            this.CustomerNumber = "";
            this.SalutationId = -1;
            this.Title = "";
            this.FirstName = "";
            this.LastName = "";
            this.LogIn = "";
            this.Password = "";
            this.CardId = -1;
            this.CardReleaseNumber = -1;
            this.CardTypeId = -1;
            this.PaymentId = -1;
            this.FromAmount = -1;
            this.ToAmount = -1;
            this.PositiveCreditBalance = -1;
//            this.CardReleaseDate = null;
//            this.LastTransaction = null;
            this.CompanyName = "";
            this.Street = "";
            this.ZIPCode = "";
            this.City = "";
            this.PhoneNumber = "";
            this.MobilePhone = "";
            this.Email = "";
            this.LanguageId = -1;
            this.PriceHundredKilo = -1;
            this.LocationGroup = "";
            this.LocationGroupMask = 0;

            this.AddressId = -1;
            this.LanguageCode = "";
        }

#endregion
    }

    /// <summary>
    /// Class that represents a client
    /// </summary>
    [Serializable]
    public class Client
    {        
        #region Members

        private string _name;               // name of container
        private string _operator_name;      // operators name
        private string _device_number;      // device name
        private string _gsm_number;         // gsm number
        private int _container_id;          // container unique id
        private DateTime _t_went_offline;   // time since container is offline
        private DateTime _t_last_communication;   // time of last transaction
        private bool _bOffline;             // flag if container is offline
        private bool _bDisabled;            // communication start and end are out of range
        private string _ModemFWVersion;     // modem firmware version string
        private UInt16 _ModemSignalQuality; // modem signal quality information

        #endregion

        #region Properties

        /// <summary>
        /// Name of the client
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Operators name
        /// </summary>
        public string OperatorName
        {
            get { return _operator_name; }
            set { _operator_name = value; }
        }

        /// <summary>
        /// Device number
        /// </summary>
        public string DeviceNumber
        {
            get { return _device_number; }
            set { _device_number = value; }
        }

        /// <summary>
        /// GSM number
        /// </summary>
        public string GSMNumber
        {
            get { return _gsm_number; }
            set { _gsm_number = value; }
        }

        /// <summary>
        /// Container ID
        /// </summary>
        public int ContainerID
        {
            get { return _container_id; }
            set
            {
                _container_id = value;
                _name = String.Format("ContainerID {0}:", value);
            }
        }

        /// <summary>
        /// Time the container went offline
        /// </summary>
        public DateTime TimeWentOffline
        {
            get { return _t_went_offline; }
            set { _t_went_offline = value; }
        }

        public DateTime TimeLastCommunication
        {
            get { return _t_last_communication; }
            set { _t_last_communication = value; }
        }

        /// <summary>
        /// Wheter Container is offline or not
        /// </summary>
        public bool IsOffline
        {
            get { return _bOffline; }
            set { _bOffline = value; }
        }

        /// <summary>
        /// Wheter Containers communication start and end date matches
        /// </summary>
        public bool IsDisabled
        {
            get { return _bDisabled; }
            set { _bDisabled = value; }
        }

        /// <summary>
        /// Modem Firmware version string
        /// </summary>
        public string ModemFirmwareVersion
        {
            get { return _ModemFWVersion; }
            set { _ModemFWVersion = value; }
        }

        /// <summary>
        /// Modem Signal quality
        /// </summary>
        public UInt16 ModemSignalQuality
        {
            get { return _ModemSignalQuality; }
            set { _ModemSignalQuality = value; }
        }

#endregion

        #region Constructor

        public Client()
        {
            _bOffline = false;
            _bDisabled = false;
            _name = "";
            _ModemFWVersion = "unknown";
        }

#endregion
    }

    /// <summary>
    /// Class that represents a client connection
    /// </summary>
    [Serializable]
    public class ClientConnection : MarshalByRefObject
    {
        #region Private classes

        private class ServiceCommand
        {
            #region Members

            public uint command;    // 0001 PLC-Coldstart
                                    // 0002 PLC-Warmstart
                                    // 0003 Modem Restart
                                    // 0009 Switch to Online-Mode
            public uint delay_time; // time to wait till command should be executed
            public string reserved; // reserved

            #endregion
        }

#endregion

        #region Enums

        private enum _CLIENT_STATE
        {
            INIT,
            WAIT_CONFIG,
            WAIT_CONFIG_ACK,
            START_READ_JOURNAL,
            READ_JOURNAL,
            WAIT_READPOINTER,
            IDLE,
            WAIT_WRITEPOINTER,
            WAIT_NEW_CUSTOMERDATA_ACK,
            WAIT_TUNNEL,
            ERROR
        }

        #endregion

        #region Members

        private ContainerParamsDto _containerParams = null;                 // container parameters retrieved from rest api
        private OperatorParamsDto _operatorParams = null;                   // operator parameters retrieved from rest api
        private SkpLocationDto _locationParams = null;                      // location parameters retrieved from rest api
        private Client  _client = new Client();                             // client object
        private string  _name = "Unknown";                                  // client name
        private string  _operator_name = "Unknown";                         // operator name
        private int     _operator_id = 0;                                   // operator id
        private int     _location_id = 0;                                   // location_id
        private int     _location_group_id = 0;                             // location_group_id
        private int _currency_id = 0;                                       // currency id
        private string  _operator_language_code = "";                       // operator language code          
        private int _location_language_code = 0;                            // location language code
        private string _gsm_number = "";                                    // telephone number of gsm modem
        private DateTime _nightlock_start = new DateTime();                 // nightlock start time
        private DateTime _nightlock_stop = new DateTime();                  // nightlock stop time
        private DateTime _card_valid_date = new DateTime(2001, 1, 1);       // card valid date
        private int _gate_limit = 0;                                        // gate limit ??
        private int _container_id = -1;                                     // container id
        private int _price_hundred_kilo = -1;                               // price per hundred kilogramms
        private bool _bEmptying = false;                                         // entleerung wird gepflegt
        private int _container_type_id = -1;                                // container type identifier
        private string _container_type = "";                                // container type
        private string _actual_values = "";                                 // actual values transmitted with idle command
        private int _read_pointer = 0;                                      // journal read pointer
        private int _write_pointer = 0;                                     // journal write pointer
        private string _special_info = "0000";                              // reserved
        private uint _security_code = 0;
        private uint _protocol_version = 0;                                 // version of communication protocol
        private bool _bLogDetails = true;                                   // log detailed communication
        private int _numberOfExpectedEntries = 0;                           // readpointer admin
        private bool _bNewSeccodeCalculationNecessary = false;
        private bool _bJournalEntriesAlreadyStored = false;
        private bool _bAwaitCustomerAccessResponse = false;
        private int _MaxJournalEntries = 5000;                              // 5000 for protocol version 1 50000 for protocol version 2

        private UInt32 _numberOfBytesReceived = 0;                          // traffic counter in receive direction
        private UInt32 _numberOfBytesSent = 0;                              // traffic counter in send direction
        private DateTime _tLastCommandFromEco;                              // time when last command from ECO was received
        private DateTime _tLastCommandToEco;                                // time when last command was sent to ECO
        private DateTime _tLastWritePointerQuery;                           // last time the write pointers were read
        private DateTime _tLastConfigCheck;                                 // last time the configuration was checked
        private Queue _fromEco = new Queue();
        private Queue _serviceCommands = new Queue();                       // queue for service commands

        [NonSerialized] 
        private TcpClient _tcpClient;                       // tcp client class
        [NonSerialized]
        private ConnectionControl _controller;              // controller object who has created this instance
        [NonSerialized]
        private Thread _workerThread;                       // working thread
        [NonSerialized]
        private Thread _receiverThread;                     // receiving thread
        [NonSerialized]
        private AutoResetEvent _stopHandle = new AutoResetEvent(false);     // stop event
        [NonSerialized]
        private AutoResetEvent _receivedEvent = new AutoResetEvent(false);
        [NonSerialized]
        private const int BLOCK_SIZE = 1024;
        [NonSerialized]
        bool _bIdentified = false;
        [NonSerialized]
        AutoResetEvent _TunnelEstablished = new AutoResetEvent(false);
        [NonSerialized]
        SKP.Customer _actual_customer = new SKP.Customer();
        [NonSerialized]
        _CLIENT_STATE state = _CLIENT_STATE.INIT;
        [NonSerialized]
        DateTime _destTime;
        [NonSerialized]
        int _keepAliveInterval = 3 * 60; // [secs]

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
        /// Retrieve client object
        /// </summary>
        public Client Client
        {
            get { return _client; }
        }

        /// <summary>
        /// Clients name
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Operators name
        /// </summary>
        public string OperatorName
        {
            get { return _operator_name; }
            set { _operator_name = value; }
        }

        /// <summary>
        /// Container ID
        /// </summary>
        public int ContainerID
        {
            get { return _container_id; }
        }

        /// <summary>
        /// Container type
        /// </summary>
        public string ContainerType
        {
            get { return _container_type; }
        }


        /// <summary>
        /// Night lock start time
        /// </summary>
        public DateTime NightLockStartTime
        {
            get { return _nightlock_start; }
        }

        /// <summary>
        /// Night lock stop time
        /// </summary>
        public DateTime NightLockStopTime
        {
            get { return _nightlock_stop; }
        }

        /// <summary>
        /// GSM Number
        /// </summary>
        public string GSMNumber
        {
            get { return _gsm_number; }
        }

        /// <summary>
        /// Actual values transmitted with idle command
        /// </summary>
        public string ActualValues
        {
            get { return _actual_values; }
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

#endregion

        #region Public methods

        #region constructor

        public ClientConnection(string firstCommand)
        {
            if (firstCommand != "")
                _fromEco.Enqueue(firstCommand);
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
            _stopHandle.Set();
        }

        #endregion

        #region service commands

        public void DoColdRestart(uint minutes)
        {
            ServiceCommand srv_cmd = new ServiceCommand();

            srv_cmd.command = 1;    // PLC-Coldstart
            srv_cmd.delay_time = minutes;
            srv_cmd.reserved = "00000000";

            lock (_serviceCommands.SyncRoot)
            {
                _serviceCommands.Enqueue(srv_cmd);
            }
        }

        public void DoWarmRestart(uint minutes)
        {
            ServiceCommand srv_cmd = new ServiceCommand();

            srv_cmd.command = 2;    // PLC-Warmstart
            srv_cmd.delay_time = minutes;
            srv_cmd.reserved = "00000000";

            lock (_serviceCommands.SyncRoot)
            {
                _serviceCommands.Enqueue(srv_cmd);
            }
        }

        public void DoModemRestart(uint minutes)
        {
            ServiceCommand srv_cmd = new ServiceCommand();

            srv_cmd.command = 3;    // Modem-restart
            srv_cmd.delay_time = minutes;
            srv_cmd.reserved = "00000000";

            lock (_serviceCommands.SyncRoot)
            {
                _serviceCommands.Enqueue(srv_cmd);
            }
        }

        public void GoOnline(uint minutes)
        {
            ServiceCommand srv_cmd = new ServiceCommand();

            srv_cmd.command = 9;    // Start Online-Mode
            srv_cmd.delay_time = minutes;
            srv_cmd.reserved = "00000000";

            lock (_serviceCommands.SyncRoot)
            {
                _serviceCommands.Enqueue(srv_cmd);
            }
        }

#endregion

        #endregion

        #region Private methods

        #region Helpers

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
            else if (str.StartsWith("61"))
            {
                Console.WriteLine("Australia");
                isDrei = false;
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

        private void handle_customer(string frame)
        {
            if (_bAwaitCustomerAccessResponse)
            {
                _bAwaitCustomerAccessResponse = false;
                LogFile.WriteMessageToLogFile("{0} Received customer acknowledge", this.Name);
                return;
            }

            try
            {
                CustomerAccessCard card = new CustomerAccessCard();

                if (ConnectionService.ClientConnection.ParseCustomerData(frame, this.Name, ref card))
                {
                    CardInfoDto cardInfo = null;

                    if (card.CustomerNumber != 0)
                    {
                        // check if customer access is allowed
                       HasCardAccessToLocationFractionByCustomerNumber checkAccess = new HasCardAccessToLocationFractionByCustomerNumber((int)card.CustomerNumber, this._location_id, this._operator_id);
                       cardInfo = ConnectionControl.SkpApiClient.GetCardInfoByCustomerNumber(checkAccess);
                    }
                    else
                    {
                        string uuid = card.CardType + card.CardSerialNumber;
                        LogFile.WriteMessageToLogFile("{0} Got alphanumeric customer number: ({1})", this.Name, uuid);
                        HasCardAccessToLocationFractionByCardUuid checkAccess = new HasCardAccessToLocationFractionByCardUuid(uuid, this._location_id, this._operator_id);
                        cardInfo = ConnectionControl.SkpApiClient.GetCardInfoByCardUuid(checkAccess);
                    }

                    if (cardInfo != null && cardInfo.CardAccess == CardAccessResult.Ok)
                    {
                        LogFile.WriteMessageToLogFile("{0} Card: has Access!", this.Name);

                        if (card.CustomerNumber == 0)
                        {
                            UInt32 currentBalance = 0;
                            UInt32 pricePerOneHundredKilo = 0;

                            if (cardInfo.CurrentBalance.HasValue)
                                currentBalance = (UInt32)cardInfo.CurrentBalance;

                            if (_locationParams.PricePerOneHundredKilo.HasValue)
                                pricePerOneHundredKilo = (UInt32)_locationParams.PricePerOneHundredKilo;
                            else
                                LogFile.WriteMessageToLogFile("{0} - Warning! Location: {1} has no price per hundred kilos!", this.Name, _locationParams.LocationId);

                            LogFile.WriteMessageToLogFile("{0} Customeraccess on location: {1}, PricePerHundredKilos: {2}, Credit: {3}", this.Name, _locationParams.LocationId, pricePerOneHundredKilo, currentBalance);

                            // card is accepted so send locationgroup mask with all bits set
                            card.LocationGroupMask = 255;

                            string command = String.Format("#CUS={0:D8},{1:D3},{2:D8},{3:D5},{4},{5:D1},{6:D7},{7:D7},{8:D4},{9:D4},{10:D20}",
                                card.CustomerNumber, card.ReleaseNumber, card.LocationGroupMask, card.MinimumAmount,
                                card.ReleaseDate.ToString("ddMMyyyy"), (int)card.CardChargingType, currentBalance, pricePerOneHundredKilo,
                                cardInfo.LanguageId, card.CardType, card.CardSerialNumberUntrimmed);
                            
                            SendCommand(command);

                            _bAwaitCustomerAccessResponse = true;
                        }
                        else
                        {
                            SendCommand("#CUS!");
                            _bAwaitCustomerAccessResponse = false;
                        }
                    }
                    else
                    {
                        LogFile.WriteMessageToLogFile("{0} Card not accepted, reason: {1}", this.Name, cardInfo.CardAccess);

                        if (cardInfo.CardAccess == CardAccessResult.CustomerHasNotEnoughBalance)
                        {
                            UInt32 pricePerOneHundredKilo = 0;

                            if (_locationParams.PricePerOneHundredKilo.HasValue)
                                pricePerOneHundredKilo = (UInt32)_locationParams.PricePerOneHundredKilo;
                            else
                                LogFile.WriteMessageToLogFile("{0} - Warning! Location: {1} has no price per hundred kilos!", this.Name, _locationParams.LocationId);

                            string command = String.Format("#CUS={0:D8},{1:D3},{2:D8},{3:D5},{4},{5:D1},{6:D7},{7:D7},{8:D4},{9:D4},{10:D20}",
                                card.CustomerNumber, card.ReleaseNumber, card.LocationGroupMask, card.MinimumAmount,
                                card.ReleaseDate.ToString("ddMMyyyy"), (int)card.CardChargingType, 0, pricePerOneHundredKilo,
                                cardInfo.LanguageId, card.CardType, card.CardSerialNumberUntrimmed);

                            SendCommand(command);
                        }
                        else
                        {
                            // try to remove customer from whitelist in modem if it is a myfare card
                            if (card.CustomerNumber == 0)
                            {
                                string uuid = card.CardType + card.CardSerialNumber;
                                SendCommand(String.Format("-CUS={0}", uuid));
                            }
                            else
                            {
                                UInt32 currentBalance = 0;
                                UInt32 pricePerOneHundredKilo = 0;

                                if (cardInfo.CurrentBalance.HasValue)
                                    currentBalance = (UInt32)cardInfo.CurrentBalance;

                                if (_locationParams.PricePerOneHundredKilo.HasValue)
                                    pricePerOneHundredKilo = (UInt32)_locationParams.PricePerOneHundredKilo;
                                else
                                    LogFile.WriteMessageToLogFile("{0} - Warning! Location: {1} has no price per hundred kilos!", this.Name, _locationParams.LocationId);

                                // hitag -> send #CUS with LocationGroupMask = 0
                                string command = String.Format("#CUS={0:D8},{1:D3},{2:D8},{3:D5},{4},{5:D1},{6:D7},{7:D7},{8:D4},{9:D4},{10:D20}",
                                    card.CustomerNumber, card.ReleaseNumber, 0, card.MinimumAmount,
                                    card.ReleaseDate.ToString("ddMMyyyy"), (int)card.CardChargingType, currentBalance, pricePerOneHundredKilo,
                                    cardInfo.LanguageId, card.CardType, card.CardSerialNumberUntrimmed);

                                SendCommand(command);
                            }
                        }
                    }
                }
            }
            catch (Exception excp)
            {
                LogFile.WriteErrorToLogFile("{0} Exception ({1}) in handle_customer appeared", this.Name, excp.Message);
            }
        }

        /// <summary>
        /// Process messages from eco ( %MSG= )
        /// </summary>
        /// <param name="frame"></param>
        private void process_message(string frame)
        {
            try
            {
                string[] toks = frame.Split(new char[] { ',' });

                if (toks.GetLength(0) == 4)
                {
                    DateTime date = new DateTime();
                    DateTime time = new DateTime();

                    int code = Convert.ToInt32(toks[0]);

                    try
                    {
                        date = DateTime.ParseExact(toks[1], "ddMMyyyy", null);
                    }
                    catch (Exception)
                    {
                        LogFile.WriteErrorToLogFile("{0} Invalid date format in string: %MSG={1}", this.Name, frame);
                    }

                    try
                    {
                        time = DateTime.ParseExact(toks[2], "HHmmss", null);
                    }
                    catch (Exception)
                    {
                        LogFile.WriteErrorToLogFile("{0} Invalid time format in string: %MSG={1}", this.Name, frame);
                    }

                    date = date.Add(time.TimeOfDay);

                    try
                    {
                        List<StatusMessageDto> statusMsgList = new List<StatusMessageDto>();
                        statusMsgList.Add(new StatusMessageDto(code, null, true, false, date.ToUniversalTime()));
                        StoreContainerStatus containerStatus = new StoreContainerStatus(this._locationParams.LocationId, this._containerParams.GsmNumber, statusMsgList);
                        ConnectionControl.SkpApiClient.StoreContainerStatus(this.ContainerID, containerStatus);
                    }
                    catch (Exception excp)
                    {
                        LogFile.WriteErrorToLogFile("{0} Exception: {1} while trying to store container status", this.Name, excp.Message);
                    }

                    string message = "";

                    try
                    {
                        message = ConnectionControl.SkpApiClient.GetTranslationForStatusMessage(code, this._operatorParams.LanguageCode);
                        LogFile.WriteMessageToLogFile("{0} Got Translation for code: {1} -> {2}", this.Name, code, message);
                    }
                    catch (Exception excp)
                    {
                        LogFile.WriteErrorToLogFile("{0} Exception: {1} while trying to get translation for code: {2}, language code: {3}", this.Name, excp.Message, code, this._operator_language_code);
                    }

                    if (message != "")
                    {
                        string text = message;
 
                        if (this._operator_language_code == "DE")
                            text = String.Format("{0} {1} Standort:{2}-{3}", text, code, this._location_id, this._locationParams.Name + " - " + this._locationParams.FractionName);
                        else
                            text = String.Format("{0} {1} Location:{2}-{3}", text, code, this._location_id, this._locationParams.Name + " - " + this._locationParams.FractionName);

                        if (code == 40) // 75% full container
                        {
                            double actual_weight = 0;
                            
                            // in case of an container with emptying detection calculate actual weight
                            if ((this._bEmptying) && (this._container_type_id != 5))   // no eco underground
                            {
                                try
                                {
                                    CurrentWeightResult result = ConnectionControl.SkpApiClient.GetContainerCurrentWeight(this._container_id);
                                    actual_weight = (double)result.CurrentWeight;
                                }
                                catch (Exception excp)
                                {
                                    LogFile.WriteErrorToLogFile("{0} Exception ({1}) while trying to retrieve current weight!", excp.Message);
                                }

                                NumberFormatInfo nfi = new CultureInfo("de-DE", false).NumberFormat;

                                try
                                {
                                    LogFile.WriteMessageToLogFile("{0} Actual weight is: {1} kg", this.Name, actual_weight.ToString("N", nfi));

                                    if (actual_weight > 0.0f)
                                    {
                                        text += " kg: " + actual_weight.ToString("N", nfi);
                                    }
                                }
                                catch (Exception excp)
                                {
                                    LogFile.WriteMessageToLogFile("{0} Exception ({1}) while calculating actual weight", this.Name, excp.Message);
                                }
                            }
                        }

                        try
                        {
                            ArrayList users = new ArrayList();

                            foreach (var notification in ConnectionControl.SkpApiClient.GetNotificationContacts(this.ContainerID, code))
                            {
                                if (notification.ContactMethod == ContactMethod.Sms)
                                {
                                    FieldAreaNetwork.AlertingUser alertUser = new FieldAreaNetwork.AlertingUser();

                                    alertUser.ClientName = "SKP";
                                    alertUser.TelephoneNumber = notification.Contact;
                                    alertUser.Flags = (int)ALERTING_FLAGS.SMS_ENABLED;
                                    alertUser.EmailAddress = "";
                                    alertUser.Name = "Unknown";

                                    users.Add(alertUser);
                                    LogFile.WriteMessageToLogFile("{0} Send SMS: \'{1}\' to number: {2}", this.Name, text, alertUser.TelephoneNumber);
                                }
                            }

                            if (users.Count > 0)
                            {
                                if (text.Length > 160)
                                    text = text.Substring(0, 160);

                                FieldAreaNetwork.AlertingControl.AddAlarm("alarm.wallner-automation.com", users, "WIP", text, false, 0);
                            }
                            else
                            {
                                LogFile.WriteMessageToLogFile("{0} Got no SMS users for notification", this.Name);
                            }
                        }
                        catch (Exception excp)
                        {
                            LogFile.WriteErrorToLogFile("{0} Exception: {1} while trying to get notification contacts for code: {2}", this.Name, excp.Message, code);
                        }
                    }
                    else
                    {
                        LogFile.WriteMessageToLogFile("{0} No Translation found for message: ({1})", message);
                    }
                }
                else
                    LogFile.WriteErrorToLogFile("{0} Invalid number of fields in string: %MSG={1}", this.Name, frame);
            }
            catch (Exception excp)
            {
                LogFile.WriteErrorToLogFile("{0} Exception: {1} while processing message: {2}", this.Name, excp.Message, frame);
            }
        }

#endregion

#region Database methods

        private bool GetContainerId(string gsm_number)
        {
            bool retval = false;

            try
            {
                String fullString = gsm_number = gsm_number.Substring(5);
                int posComma = 0;
                // if we have a modem which sends it's softwareversion -> remove it
                if ((posComma = gsm_number.IndexOf(',')) != -1)
                {
                    try
                    {
                        string[] toks = gsm_number.Split(new char[] { ',' });
                        if (toks.GetLength(0) >= 2)
                        {
                            _client.ModemFirmwareVersion = toks[1].Trim();
                            _client.ModemSignalQuality = System.Convert.ToUInt16(toks[2].Trim());
                        }
                    }
                    catch
                    {
                    }

                    gsm_number = gsm_number.Substring(0, posComma);

#if false   // it's not necessary for new machines to trim before checking with full length
                    if (gsm_number.Length == 20 &&
                        (_client.ModemFirmwareVersion.IndexOf("ECO-EHS6T") != -1 ||
                        _client.ModemFirmwareVersion.IndexOf("ECO-TC65i") != -1)
                        )
                    {
                        LogFile.WriteMessageToLogFile("Trim last char from CCID because new modem firmware detected");
                        gsm_number = gsm_number.Substring(0, gsm_number.Length - 1);
                    }
#endif
                }

                for (int retry = 0; retry < 3; retry++)
                {
                    try
                    {
                        _containerParams = (ContainerParamsDto)ConnectionControl.SkpApiClient.GetContainerParams(gsm_number);

                        this._container_id = _containerParams.Id;

                        string new_name = String.Format("ContainerID {0}:", this._container_id);
                        _keepAliveInterval = Controller.GetKeepAliveInterval(_container_id);

                        LogFile.WriteMessageToLogFile("{0} Ident: {1}, Keepalive: {2} secs", new_name, fullString, _keepAliveInterval);

                        lock (_controller.ConnectedECOClients.SyncRoot)
                        {
                            for (int i = _controller.ConnectedECOClients.Count - 1; i >= 0; i--)
                            {
                                ClientConnection client = (ClientConnection)_controller.ConnectedECOClients[i];

                                if (client.Name == new_name)
                                {
                                    client.Stop();
                                }
                            }
                        }

                        this.Name = new_name;
                        retval = true;
                        break;
                    }
                    catch (Exception excp)
                    {
                        string msg = excp.Message;
                    }

                    if (_client.ModemFirmwareVersion.IndexOf("ECO-EHS6T") != -1 ||
                         _client.ModemFirmwareVersion.IndexOf("ECO-TC65i") != -1
                         )
                    {
                        LogFile.WriteMessageToLogFile("Trim last char from CCID maybe new (correct) firmware to read out ICCID");
                        gsm_number = gsm_number.Substring(0, gsm_number.Length - 1);
                    }
                    else
                        break;
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("{0} Exception: {1} while trying to get container id.", this.Name, e.Message);
                retval = false;
            }

            return retval;
        }

        private string TrimCustomerNumber(string custNumber)
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

                    return custNumber.Substring(i, remainder);
                }
            }

            return "";
        }

        /// <summary>
        /// Parse journal entries and store data to database
        /// </summary>
        /// <param name="str"></param>
        /// <param name="numberOfExpectedEntries"></param>
        /// <returns></returns>
        private bool ParseJournalEntries(string str, int numberOfExpectedEntries)
        {
            List<StatusMessageDto> statusMsgList = new List<StatusMessageDto>();

            try
            {
                // remove header
                str = str.Substring(5);
                // split in entries
                string[] str_entry = str.Split(new char[] { ';' });

                if (str_entry.GetLength(0) < numberOfExpectedEntries)
                {
                    LogFile.WriteErrorToLogFile("{0} ParseJournalEntries, unexpected number of entries ({1})!", this.Name, str_entry.GetLength(0));
                    return false;
                }

                // parse all entries
                for (int i = 0; i < numberOfExpectedEntries; i++)
                {
                    try
                    {
                        string[] toks = str_entry[i].Split(new char[] { ',' });

                        string alibiStoreNumber = "";
                        string cardType = "";
                        string cardSerialNumber = "";
                        DateTime transDate = DateTime.Now;    

                        bool bAlphanumericCustomerNumber = false;

                        if (toks.GetLength(0) != 11)
                        {
                            if (_protocol_version < 4)
                            {
                                LogFile.WriteErrorToLogFile("{0} ParseJournalEntries, unexpected number of fields in entry: {1}!", this.Name, str_entry[i]);
                                return false;
                            }
                        }

                        try
                        {
                            transDate = DateTime.ParseExact(toks[2], "ddMMyyyy", null);
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

                        transDate = transDate.Add(time.TimeOfDay);

                        int transNumber = Convert.ToInt32(toks[0]);
                        int transLocationId = Convert.ToInt32(toks[1]);
                        string tranCustomerNumber = toks[4];
                        // get rid of the leading zeros
                        int customerNumber = Convert.ToInt32(tranCustomerNumber);
                        tranCustomerNumber = Convert.ToString(customerNumber);

                        int emission_number = Convert.ToInt32(toks[5]);
                        int transStatusId = Convert.ToInt32(toks[6]);
                        int transDuration = Convert.ToInt32(toks[7]);
                        double transAmount = (double)Convert.ToInt32(toks[8]) / 100;
                        double transCurrentBalance = (double)Convert.ToInt32(toks[9]) / 100;
                        double transWeight = (double)Convert.ToInt32(toks[10]) / 10;

                        if (_protocol_version >= 4)
                        {
                            if (toks.GetLength(0) >= 14)
                            {
                                alibiStoreNumber = toks[11];
                                cardType = toks[12];
                                cardSerialNumber = toks[13];

                                // check if got an alphanumeric customer number
                                if (customerNumber == 0)
                                {
                                    if (cardSerialNumber != "" && cardSerialNumber != "00000000000000000000")
                                    {
                                        tranCustomerNumber = cardType + TrimCustomerNumber(cardSerialNumber);
                                        bAlphanumericCustomerNumber = true;
                                        LogFile.WriteMessageToLogFile("{0} Got alphanumeric customer number: {1}", this.Name, tranCustomerNumber);
                                    }
                                }
                            }
                        }
                        else
                            tranCustomerNumber = "";

                        // transactions with no customer number are normal container status messages
                        if (!bAlphanumericCustomerNumber && customerNumber == 0)
                        {
                            try
                            {
                                StatusMessageDto statusMessage = new StatusMessageDto(transStatusId, 0.0F, false, true, transDate.ToUniversalTime());
                                statusMsgList.Add(statusMessage);
                                LogFile.WriteMessageToLogFile("{0} Store status messagecode: {1}, Timestamp: {2}", this.Name, transStatusId);
                            }
                            catch (Exception e)
                            {
                                LogFile.WriteErrorToLogFile("{0} Exception: {1} while trying to add status code to list.", this.Name, e.Message);
                            }

                            continue;
                        }

                        if (bAlphanumericCustomerNumber)
                        {
                            try
                            {
                                CreateInsertionTransactionByCardUuid trans = new CreateInsertionTransactionByCardUuid((alibiStoreNumber == "") ? null : alibiStoreNumber, (tranCustomerNumber == "") ? null : tranCustomerNumber, this._container_id,
                                    transCurrentBalance, transDuration, transLocationId, this._operator_id, transStatusId, transDate.ToUniversalTime(), transWeight);
                                ConnectionControl.SkpApiClient.CreateInsertionByCardUuid(trans);
                            }
                            catch (Exception excp)
                            {
                                LogFile.WriteErrorToLogFile("{0} Exception: {1} while trying to create insertion by card uuid", this.Name, excp.Message);
                            }
                        }
                        else
                        {
                            try
                            { 
                                CreateInsertionTransactionByCustomerNumber trans = new CreateInsertionTransactionByCustomerNumber(alibiStoreNumber, tranCustomerNumber, this._container_id,
                                    transCurrentBalance, customerNumber, transDuration, transLocationId, this._operator_id, transStatusId,
                                    transDate.ToUniversalTime(), transWeight);
                                ConnectionControl.SkpApiClient.CreateInsertionByCustomerNumber(trans);
                            }
                            catch (Exception excp)
                            {
                                LogFile.WriteErrorToLogFile("{0} Exception: {1} while trying to create insertion by cuastomer number", this.Name, excp.Message);
                            }
                        }
                    }
                    catch (Exception excp)
                    {
                        LogFile.WriteErrorToLogFile("{0} Exception: {1}, while parsing entry {2}", this.Name, excp.Message, i + 1);
                    }
                }


                try
                {
                    StoreContainerStatus stat = new StoreContainerStatus(this._location_id, this._gsm_number, statusMsgList);

                    ConnectionControl.SkpApiClient.StoreContainerStatus(this._container_id, stat);

                    LogFile.WriteMessageToLogFile("{0} Stored {1} status codes", this.Name, statusMsgList.Count);
                }
                catch (Exception excp)
                {
                    LogFile.WriteErrorToLogFile("{0} Exception: {1}, while trying to store status messages", this.Name, excp.Message);
                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("{0} Exception: {1} in \'ParseJournalEntries\' appeared.", this.Name, e.Message);
            }

            return false;
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

        /// <summary>
        /// Try to get expected answer from queue
        /// If we have unsolicited frames in the queue, process them
        /// </summary>
        /// <param name="start_sequence"></param>
        /// <returns></returns>
        private string get_answer(string start_sequence)
        {
            string str = "";

            try
            {
                while (_fromEco.Count > 0)
                {
                    // get frame from queue
                    lock (_fromEco.SyncRoot)
                    {
                        str = (string)_fromEco.Dequeue();
                    }

                    if (str.StartsWith(start_sequence))
                        return str;
                    else
                    {
                        if (this.state <= _CLIENT_STATE.WAIT_CONFIG_ACK)
                        {
                            // we are not yet identified so put message back to queue and process it later
                            lock (_fromEco.SyncRoot)
                            {
                                _fromEco.Enqueue(str);
                            }
                            // if this was a customer request, remeber this to not calculate security code
//                            if (str.IndexOf("%CUS") != -1)
//                                _bSkipSecCodeCalcualtion = true;

                            return "";
                        }
                        else
                            process_frame(str);
                    }
                }
            }
            catch (Exception excp)
            {
                LogFile.WriteErrorToLogFile("{0} Exception ({1}) in get_answer appeared!", this.Name, excp.Message);
            }

            return "";
        }

        /// <summary>
        /// process unsolicited frames
        /// </summary>
        private void process_frame(string frame)
        {
            try
            {
                if (frame.StartsWith("%CUS="))
                {
                    try
                    {
                        handle_customer(frame);
                    }
                    catch (Exception excp)
                    {
                        LogFile.WriteErrorToLogFile("{0} Exception: {1} while trying to handle cutomer request.", this.Name, excp.Message);
                    }
                }
                else if (frame.StartsWith("%MSG="))
                {
                    try
                    {
                        process_message(frame.Substring(5));

                        if (_protocol_version >= 4)
                            SendCommand("#MSG!");
                    }
                    catch (Exception excp)
                    {
                        LogFile.WriteErrorToLogFile("{0} Exception: {1} while trying to process message: {2} from eco", this.Name, excp.Message, frame);
                    }
                }
                else if (frame.StartsWith("%SRV="))
                {
                    LogFile.WriteMessageToLogFile("{0} Received answer for service command. {1}", this.Name, frame);
                }
                else if (frame.StartsWith("%SMS="))
                {
                    string[] toks = frame.Substring(5).Split(new char[] { ',' });

                    if (toks.GetLength(0) >= 2)
                    {
                        string telNumber = toks[0];

                        // check for valid telephone number
                        if (telNumber.Length > 10 && telNumber.Length < 20)
                        {
                            char[] chars = telNumber.ToCharArray();
                            int i, startpos = 0;

                            if ((startpos = telNumber.IndexOf('+')) != -1)
                                startpos += 1;
                            else
                                startpos = 0;

                            for (i = startpos; i < telNumber.Length; i++)
                            {
                                if (!Char.IsDigit(chars[i]))
                                    break;
                            }

                            if (i == telNumber.Length)
                            {
                                bool bIsDrei = false;
                                bool retval = checkAreaCode(telNumber, ref bIsDrei);

                                _controller.SendSMS(toks[1], telNumber);
                            }
                            else
                            {
                                LogFile.WriteErrorToLogFile("{0} invalid phone number for sms detected!", this.Name);
                            }

                        }
                        else
                            LogFile.WriteErrorToLogFile("{0} invalid length of phone number detected!", this.Name);
                    }
                    else
                        LogFile.WriteErrorToLogFile("{0} invalid number of tokens ({1}) in send sms command detected!", this.Name, toks.GetLength(0));
                }
                else if (frame.StartsWith("%ERR!"))
                {
                    if (frame.IndexOf("SECCODE") != -1)
                    {
                        _bNewSeccodeCalculationNecessary = true;
                    }
                    else
                    {
                        LogFile.WriteErrorToLogFile("{0} Error received: {1}", this.Name, frame);
                    }
                }
                else if (frame.StartsWith("%CLI?"))
                {
                    string command = "#CLI=";
                    // retrieve list of available clients
                    foreach (ClientConnection client in _controller.ConnectedClients)
                    {
                        command += String.Format("{0};", client.Name);
                    }

                    SendCommand(command);
                }
            }
            catch (Exception excp)
            {
                LogFile.WriteErrorToLogFile("{0} Exception ({1}) in process_frame appeared!", this.Name, excp.Message);
            }

        }

        /// <summary>
        /// Checks if database settings have been changed
        /// </summary>
        /// <returns>true if values where changed</returns>
        private bool CheckConfiguration()
        {
            // TODO ??

            return false;

#if false
            try
            {
                int currency_id = this._currency_id;
                DateTime nightlock_start = this._nightlock_start;
                DateTime nightlock_stop = this._nightlock_stop;
                int gate_limit = this._gate_limit;
                int price_hundred_kilo = this._price_hundred_kilo;
                int location_language_code = this._location_language_code;
                int location_container_id = 0;

                // retrieve operator name currency id and language code from database
                if ((retval = _controller.DataAccess.GetOperatorParams(this._operator_id, ref this._operator_name, ref this._currency_id, ref this._operator_language_code)) == false)
                {
                    LogFile.WriteErrorToLogFile("{0} Error while trying to retrieve operator parameters!", this.Name);
                    return retval;
                }

                // retrieve location data
                if ((retval = _controller.DataAccess.GetLocationParams(this._location_id, ref this._location, ref this._nightlock_start, ref this._nightlock_stop,
                    ref this._gate_limit, ref location_container_id, ref this._price_hundred_kilo, ref this._bEmptying)) == false)
                {
                    LogFile.WriteErrorToLogFile("{0} Error while trying to retrieve location parameters!", this.Name);
                    return retval;
                }

                // retrieve location language code
                if ((retval = _controller.DataAccess.GetLocationLanguages(this._location_id, ref this._location_language_code)) == false)
                {
                    LogFile.WriteErrorToLogFile("{0} Error while trying to retrieve location language code!", this.Name);
                    return retval;
                }

                if (currency_id != this._currency_id ||
                    nightlock_start != this._nightlock_start ||
                    nightlock_stop != this._nightlock_stop ||
                    gate_limit != this._gate_limit ||
                    price_hundred_kilo != this._price_hundred_kilo ||
                    location_language_code != this._location_language_code)
                {
                    String strTimeZone = String.Empty;

                    LogFile.WriteMessageToLogFile("{0} Configuration has been changed, send new configuration!", this.Name);

                    // get time zone information
                    if (_controller.DataAccess.GetLocationTimeZone(this._location_id, ref strTimeZone))
                    {
                        try
                        {
                            DateTime timeUtc = DateTime.UtcNow;
                            TimeZoneInfo dstZone = TimeZoneInfo.FindSystemTimeZoneById(strTimeZone);
                            _destTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, dstZone);

                            LogFile.WriteMessageToLogFile("{0} Timezone for this location: {1}, actual Time: {2}", this.Name,
                                _destTime.IsDaylightSavingTime() ? dstZone.DaylightName : dstZone.StandardName, _destTime);
                        }
                        catch (TimeZoneNotFoundException)
                        {
                            LogFile.WriteErrorToLogFile("Timezone not found. Timezonename: {0}, location_id", strTimeZone, this._location_id);
                        }
                        catch (InvalidTimeZoneException)
                        {
                            LogFile.WriteErrorToLogFile("Registry data on the Central STandard Time zone has been corrupted.");
                        }
                    }
                    else
                    {
                        _destTime = DateTime.Now;
                    }

                    return SendNewConfiguration();
                }
            }
            catch (Exception excp)
            {
                LogFile.WriteErrorToLogFile("{0} Exception ({1}) in CheckConfiguration appeared!", this.Name, excp.Message);
            }

            return false;
#endif
        }

        /// <summary>
        /// Send new configuration if it has beeen changed
        /// </summary>
        /// <returns>true on success</returns>
        private bool SendNewConfiguration()
        {
            string command = null;

            try
            {
                if (_protocol_version < 4)
                {
                    command = String.Format("#CON={0:D2},{1:D5},{2:D5},{3:D8},{4:D7},{5:D2},{6},{7:D4},{8:D5},{9},{10:D4},{11},{12},{13},{14:D6}",
                        this._protocol_version, this._operator_id, this._location_id, this._location_group_id, this._price_hundred_kilo, this._currency_id,
                        this._nightlock_start.ToString("HHmm"), Convert.ToInt32(this._nightlock_stop.Subtract(this._nightlock_start).TotalMinutes),
                        this._gate_limit, this._card_valid_date.ToString("ddMMyyyy"), this._location_language_code, this._special_info,
                        _destTime.ToString("ddMMyyyy"), _destTime.ToString("HHmmss"), _security_code);
                }
                else
                {
                    command = String.Format("#CON={0:D2},{1:D5},{2:D5},{3:D8},{4:D7},{5:D2},{6},{7:D4},{8:D5},{9},{10:D4},{11},{12},{13}",
                        this._protocol_version, this._operator_id, this._location_id, this._location_group_id, this._price_hundred_kilo, this._currency_id,
                        this._nightlock_start.ToString("HHmm"), Convert.ToInt32(this._nightlock_stop.Subtract(this._nightlock_start).TotalMinutes),
                        this._gate_limit, this._card_valid_date.ToString("ddMMyyyy"), this._location_language_code, this._special_info,
                        _destTime.ToString("ddMMyyyy"), _destTime.ToString("HHmmss"));
                }

                LogFile.WriteMessageToLogFile("{0} Send new configuration: {1}", this.Name, command);

                if (!SendCommand(command)) return false;

            }
            catch (Exception excp)
            {
                LogFile.WriteErrorToLogFile("{0} Exception: {1} in \'SendNewConfiguration\' appeared.", this.Name, excp.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Split received configuration string in it's parts and compare to values stored in database.
        /// </summary>
        /// <param name="str">Received configuration string</param>
        /// <param name="bSendNewConfig">Boolean variable to signal if new configuration should be sent</param>
        /// <returns>true on success</returns>
        private bool AnalyzeConfiguration(string str, ref bool bSendNewConfig)
        {
            try
            {
                string[] toks = str.Split(new char[] { '=', ',' });

//                LogFile.WriteMessageToLogFile("Analyze configuration with string: {0}", str);

                if (toks.GetLength(0) >= 16)
                {
                    try
                    {
                        _protocol_version = Convert.ToUInt32(toks[1]);
                        if (_protocol_version >= 2)
                        {
                            int lastEntry = 0;

                            if (int.TryParse(toks[17], out lastEntry))
                                _MaxJournalEntries = lastEntry + 1;
                            else
                                _MaxJournalEntries = 50000;

                            if (_protocol_version >= 4 && toks.GetLength(0) >= 19)
                            {
                                string swVersionPLC = toks[18];
                                LogFile.WriteMessageToLogFile("{0} PLC Softwareverion: {1}", swVersionPLC);
                            }
                        }

                        this._operator_id = Convert.ToInt32(toks[2]);
                        this._location_id = Convert.ToInt32(toks[3]);
                        this._location_group_id = Convert.ToInt32(toks[4]);
                        int price_hundred_kilo = Convert.ToInt32(toks[5]);
                        int currency_id = Convert.ToInt32(toks[6]);
                        int gate_limit = Convert.ToInt32(toks[9]);
                        int location_language_code = Convert.ToInt32(toks[11]);
                        int write_pointer = Convert.ToInt32(toks[15]);
                        int read_pointer = Convert.ToInt32(toks[16]);

                        LogFile.WriteMessageToLogFile("{0} Journalsize is: {1}", this.Name, _MaxJournalEntries);

                        string special_info = toks[12];
                        string strTimeZone = "";
                        DateTime nightlock_start = new DateTime();
                        DateTime nightlock_stop = new DateTime();
                        DateTime card_valid_date = new DateTime();
                        DateTime actual_date = new DateTime();
                        DateTime actual_time = new DateTime();
                        _destTime = new DateTime();

                        try
                        {
                            nightlock_start = DateTime.ParseExact(toks[7], "HHmm", null);
                        }
                        catch (Exception)
                        {
                            LogFile.WriteErrorToLogFile("{0} Error while parsing \'Nigthlock start\'", this.Name);
                        }

                        try
                        {
                            nightlock_stop = nightlock_start.Add(new TimeSpan(0, Convert.ToInt32(toks[8]), 0));
                        }
                        catch (Exception)
                        {
                            LogFile.WriteErrorToLogFile("{0} Error while parsing \'Nigthlock duration\'", this.Name);
                        }

                        try
                        {
                            card_valid_date = DateTime.ParseExact(toks[10], "ddMMyyyy", null);
                        }
                        catch (Exception)
                        {
                            LogFile.WriteErrorToLogFile("{0} Error while parsing \'card_valid_date\'", this.Name);
                        }
                        try
                        {
                            actual_date = DateTime.ParseExact(toks[13], "ddMMyyyy", null);
                        }
                        catch (Exception)
                        {
                            LogFile.WriteErrorToLogFile("{0} Error while parsing \'actual_date\'", this.Name);
                        }
                        try
                        {
                            actual_time = DateTime.ParseExact(toks[14], "HHmmss", null);
                        }
                        catch (Exception)
                        {
                            LogFile.WriteErrorToLogFile("{0} Error while parsing \'actual_time\'", this.Name);
                        }

                        actual_date = actual_date.Add(actual_time.TimeOfDay);
                        LogFile.WriteMessageToLogFile("{0} Actual Time is: {1}", this.Name, actual_date);

                        try
                        {
                            // get operator name
                            if (this._operator_id == 13)
                                this._operator_id = 10161;

                            _operatorParams = ConnectionControl.SkpApiClient.GetOperatorParams(this._operator_id);
                            this.OperatorName = _operatorParams.OperatorName;

                            LogFile.WriteMessageToLogFile("{0} Operator: {1}, Language: {2}", this.Name, this.OperatorName, _operatorParams.LanguageCode);
                        }
                        catch (Exception excp)
                        {
                            LogFile.WriteErrorToLogFile("{0} Exception: {1}, while trying to retrieve operator params!", this.Name, excp.Message);
                            return false;
                        }

                        try
                        {
                            _locationParams = ConnectionControl.SkpApiClient.GetLocationById(this._location_id);

                            
                            this._location_language_code = _locationParams.Language.CalculatedLanguageCode;
                            this._nightlock_start = _locationParams.NightLockStart;
                            this._nightlock_stop = _locationParams.NightLockStop;

                            LogFile.WriteMessageToLogFile("{0} Location: {1} - {2}, Language: {3}, nightlock active: {4}", this.Name, _locationParams.Name, _locationParams.FractionName, _locationParams.Language.CalculatedLanguageCode, _locationParams.NightLockActive ? "true" : "false");

                            if (_locationParams.NightLockActive)
                                LogFile.WriteMessageToLogFile("{0} Nightlock Start: {1}, NightLockEnd: {2}", this.Name, _locationParams.NightLockStart, _locationParams.NightLockStop);
                        }
                        catch (Exception excp)
                        {
                            LogFile.WriteErrorToLogFile("{0} Exception: {1}, while trying to retrieve location params!", this.Name, excp.Message);
                            return false;
                        }

                        try
                        {
                            ConnectionControl.SkpApiClient.UpdateLocationContainer(this._location_id, new UpdateContainer(this._container_id));
                        }
                        catch (Exception excp)
                        {
                            LogFile.WriteErrorToLogFile("{0} Exception: {1}, while trying to update location container!", this.Name, excp.Message);
                            return false;
                        }

                        try
                        {
                            string strTimezoneId = _operatorParams.TimezoneId;

                            DateTime timeUtc = DateTime.UtcNow;
                            TimeZoneInfo dstZone = TimeZoneInfo.FindSystemTimeZoneById(strTimezoneId);
                            _destTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, dstZone);

                            LogFile.WriteMessageToLogFile("{0} Timezone for this location: {1}, actual Time: {2}", this.Name,
                                _destTime.IsDaylightSavingTime() ? dstZone.DaylightName : dstZone.StandardName, _destTime);
                        }
                        catch (TimeZoneNotFoundException)
                        {
                            LogFile.WriteErrorToLogFile("Timezone not found. Timezonename: {0}, location_id", strTimeZone, this._location_id);
                        }
                        catch (InvalidTimeZoneException)
                        {
                            LogFile.WriteErrorToLogFile("Registry data on the Central STandard Time zone has been corrupted.");
                        }

                        TimeSpan span = _destTime - actual_date;

                        if (_bLogDetails) LogFile.WriteMessageToLogFile("{0} Total time difference in seconds: {1}", this.Name, Math.Abs(span.TotalSeconds));

                        // calculate new security code
                        this._security_code = (uint)(514159 % ((2 * actual_time.Hour + 11) * (actual_time.Minute + 13) * (actual_time.Second + 17)) + this._location_id);

                        this._gsm_number = _containerParams.GsmNumber;
                        this._read_pointer = _containerParams.ReadPointer;
                        this._write_pointer = _containerParams.WritePointer;

                        this._price_hundred_kilo = (int)_locationParams.PricePerOneHundredKilo;
                        this._gate_limit = (int)_locationParams.AccessLimit;
//                        this._price_hundred_kilo = price_hundred_kilo;
//                        this._gate_limit = gate_limit;
                        this._currency_id = _operatorParams.CurrencyId;
                        this._special_info = special_info;
                        this._card_valid_date = card_valid_date;

                        // check if read pointer was modified from elsewhere
                        if (this._read_pointer != read_pointer)
                        {
                            // in case of securtiy error reported from machine
                            // we did change the readpointer and did a seconds config request
                            // so this mismatch was our fault 
                            if (!_bNewSeccodeCalculationNecessary)
                            {
                                string subject = String.Format("WIP: Container {0}: Pointer Error", this._container_id);
                                string text = String.Format("GSM Number: {0}\r\n\r\nContainer Pointers actual\r\nRead Pointer: {1}\r\nWrite Pointer: {2}\r\n\r\nContainer Pointers missing\r\nRead Pointer: {3}\r\nWrite Pointer: {4}",
                                    this.GSMNumber, read_pointer, write_pointer, this._read_pointer, this._write_pointer);

                                LogFile.WriteErrorToLogFile("{0} Pointer mismatch detected. Actual Read pointer: {1}, Write pointer: {2}. Database Read pointer: {3}, Write pointer: {4}",
                                    this.Name, read_pointer, write_pointer, this._read_pointer, this._write_pointer);

                                ArrayList recipients = new ArrayList();
                                ConnectionService.AlertingUser user = new ConnectionService.AlertingUser();
                                user.Name = "Admin";
                                user.EmailAddress = "wip.container@poettinger.at";
                                recipients.Add(user);
                                try
                                {
                                    ConnectionControl.SendMail(text, subject, recipients).Wait();
                                }
                                catch (Exception) { }
                            }
                            else
                            {
                                _bNewSeccodeCalculationNecessary = false;
                            }
                        }

                        // Email from JZ 14.06.2019: Take pointers from machine
                        //                        else
                        {
                            this._read_pointer = read_pointer;
                            this._write_pointer = write_pointer;
                        }

                        // compare values
                        if (price_hundred_kilo != this._price_hundred_kilo ||
                            currency_id != this._currency_id ||
                            location_language_code != this._location_language_code ||
                            nightlock_start.Hour != this._nightlock_start.Hour ||
                            nightlock_start.Minute != this._nightlock_start.Minute ||
                            nightlock_stop.Hour != this._nightlock_stop.Hour ||
                            nightlock_stop.Minute != this._nightlock_stop.Minute ||
                            gate_limit != this._gate_limit ||
                            card_valid_date != this._card_valid_date || 
                            special_info != this._special_info ||
                            actual_date.Date != _destTime.Date ||
                            Math.Abs(span.TotalSeconds) > 60)
                        {
                            bSendNewConfig = true;
                        }
                        else
                            bSendNewConfig = false;
                        
                    }
                    catch (Exception excp)
                    {
                        LogFile.WriteErrorToLogFile("{0} Exception: {1} in \'AnalyzeConfiguration\' while retrieving data from api appeared.", this.Name, excp.Message);
                        LogFile.WriteMessageToLogFile("{0}", excp.StackTrace);
                        return false;
                    }

                    LogFile.WriteMessageToLogFile("{0} Type: {1}, Operator: {2} at location: {3}",
                        this.Name, this._container_type, this._operator_name, this._locationParams.Name + " - " + this._locationParams.FractionName);
                    
                    return true;
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("{0} Exception: {1} in \'AnalyzeConfiguration\' appeared.", this.Name, e.Message);
            }

            return false;
        }

        private void SendNewReadpointer()
        {
            int new_rp = this._read_pointer + this._numberOfExpectedEntries;

            if (new_rp >= _MaxJournalEntries)
                new_rp = new_rp - _MaxJournalEntries;

            if (_protocol_version < 4)
                SendCommand(String.Format("#RDP={0},{1}", new_rp, this._security_code));
            else
                SendCommand(String.Format("#RDP={0}", new_rp));

            // store read pointer immediately because it sometimes happens that there is no answer from cntainer
            // though the value was stored -> to avoid double journal entries

            this._read_pointer = new_rp;

            try
            {
                ConnectionControl.SkpApiClient.UpdateReadWritePointer(this._container_id, new UpdateReadWritePointer(this._read_pointer, this._write_pointer));
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("{0} Exception: {1} in \'UpdateReadWritePointerMethod\' appeared.", this.Name, e.Message);
            }
        }

#endregion

#region CCS methods

        private void DoContainerAlerting(string str)
        {
            string[] toks = str.Split(new char[] { '=', ',' });
            int container_id = -1;
            int operator_id = -1;
            int blocking_time = -1;
            int delay_time = -1;
            string material = "";
            string operator_name = "";
            string container_name = "";
            string language_code = "";

            if (toks[1].ToLower() == "identify")
            {
                if (_controller.DataAccessCCS.StoreContainerData(toks[2].Trim(), toks[3].Trim()))
                {
                    LogFile.WriteMessageToLogFile("CCS: SIMCard ID: {0} was successfully stored in database!", toks[2]);

                    if (_controller.DataAccessCCS.GetContainerParams(toks[2].Trim(), ref container_id, ref container_name, ref operator_id, ref blocking_time, ref delay_time, ref material))
                    {
                        string strAnswer = String.Format("ACK: BlockingTime: {0}; DelayTime: {1}; Material: {2};", blocking_time, delay_time, material);
                        SendCommand(strAnswer);
                    }
                    else
                        SendCommand("NAK");
                }
                else
                {
                    LogFile.WriteErrorToLogFile("CCS: SIMCard ID: {0} could not be stored in database!", toks[2]);
                    SendCommand("NAK");
                }
            }
            else if (toks[1].ToLower() == "full" || toks[1].ToLower() == "error")
            {
                int number_of_users = 0;
                CCS.AlertingUser[] users = new CCS.AlertingUser[32];

                for (int i = 0; i < 32; i++)
                {
                    users[i] = new CCS.AlertingUser();
                }

                if (_controller.DataAccessCCS.GetContainerParams(toks[2].Trim(), ref container_id, ref container_name, ref operator_id, ref blocking_time, ref delay_time, ref material))
                {
                    string strAnswer = "ACK:";

                    if (!_controller.DataAccessCCS.GetOperatorParams(operator_id, ref operator_name, ref language_code))
                    {
                        LogFile.WriteErrorToLogFile("CCS: Error while retrieving operator parameters for id {0}", operator_id);
                    }

                    LogFile.WriteMessageToLogFile("CCS: {0}: Get alerting users for container: {1}", operator_name, container_name);
                    if (!_controller.DataAccessCCS.GetAlertingUsers(container_id, ref number_of_users, ref users))
                    {
                        LogFile.WriteErrorToLogFile("CCS: Error while retrieving alerting users for id {0}", container_id);
                    }

                    string subject = String.Format("Operator: {0}: Container: {1}", operator_name, container_name);
                    string body = "";

                    if (language_code.ToLower().Trim() == "nl")
                    {
                        if (toks[1].ToLower() == "full")
                            body = DateTime.Now.ToString("dd.MM.yyyy HH:mm :") + "pers is 75% vol gelieve deze in te plannen om te ledigen";
                        else
                            body = DateTime.Now.ToString("dd.MM.yyyy HH:mm :") + "perscontainer storing";
                    }
                    else if (language_code.ToLower().Trim() == "de")
                    {
                        if (toks[1].ToLower() == "full")
                            body = DateTime.Now.ToString("dd.MM.yyyy HH:mm :") + "Container 3/4 Voll";
                        else
                            body = DateTime.Now.ToString("dd.MM.yyyy HH:mm :") + "Container Stoerung";
                    }
                    else
                    {
                        if (toks[1].ToLower() == "full")
                            body = DateTime.Now.ToString("dd.MM.yyyy HH:mm :") + "Container 3/4 full";
                        else
                            body = DateTime.Now.ToString("dd.MM.yyyy HH:mm :") + "Error Container";
                    }

                    strAnswer += subject + "\r\n" + body + "\r\n;";

                    ArrayList emailRecipients = new ArrayList();

                    for (int i = 0; i < number_of_users; i++)
                    {
                        if (users[i].Email != "")
                        {
//                            _controller.SendEmail(subject, body, users[i].Email);

                            ConnectionService.AlertingUser user = new ConnectionService.AlertingUser();
                            user.EmailAddress = users[i].Email;
                            user.Name = users[i].LastName;

                            emailRecipients.Add(user);

                            LogFile.WriteMessageToLogFile("Send Email to {0}, Subject: {1}, Body: {2}", users[i].Email, subject, body);
                        }

                        if (users[i].GsmNumber != "")
                        {
                            strAnswer += users[i].GsmNumber + ";";
                            LogFile.WriteMessageToLogFile(strAnswer);
                        }
                    }

                    ConnectionControl.SendMail(body, subject, emailRecipients).Wait();

                    SendCommand(strAnswer);
                }
                else
                    SendCommand("NAK");
            }
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
            int retry = 0;
            bool bSendNewConfig = false;
            bool bBreak = false;

            waitHandles[0] = _receivedEvent;
            waitHandles[1] = _stopHandle;

            // initialize timeouts
            _tLastWritePointerQuery = DateTime.Now;
            _tLastCommandToEco = DateTime.Now;

            try
            {
                do
                {
                    switch (state)
                    {
                        case _CLIENT_STATE.INIT:
                            if (_fromEco.Count > 0 && (str = get_answer("%NUM")) != "")
                            {
                                if (str.IndexOf("%NUM1") != -1)
                                {
                                    // alerting from a container without a plc
                                    DoContainerAlerting(str);
                                    bBreak = true;
                                }
                                else if (GetContainerId(str))
                                {
                                    // aquire container configuration
                                    SendCommand("#CON?");
                                    state = _CLIENT_STATE.WAIT_CONFIG;
                                }
                                else
                                {
                                    LogFile.WriteErrorToLogFile("SIMCard ID: {0} was not found!", str);
                                    state = _CLIENT_STATE.ERROR;
                                    break;
                                }
                            }
                            else if (DateTime.Now > _tLastCommandToEco.AddSeconds(15))
                            {
                                LogFile.WriteErrorToLogFile("Didn't receive init string - Timeout.");
                                state = _CLIENT_STATE.ERROR;
                                break;
                            }
                            break;

                        case _CLIENT_STATE.WAIT_CONFIG:
                            if (_fromEco.Count > 0 && (str = get_answer("%CON")) != "")
                            {
                                if (AnalyzeConfiguration(str, ref bSendNewConfig) == false)
                                {
                                    LogFile.WriteErrorToLogFile("{0} Error while analyzing container configuration", this.Name);
                                    state = _CLIENT_STATE.ERROR;
                                    break;
                                }

                                _tLastConfigCheck = DateTime.Now;

                                if (bSendNewConfig)
                                {
                                    if (!SendNewConfiguration())
                                    {
                                        LogFile.WriteErrorToLogFile("{0} Error while trying to send new configuration", this.Name);
                                        state = _CLIENT_STATE.ERROR;
                                        break;
                                    }
                                    state = _CLIENT_STATE.WAIT_CONFIG_ACK;
                                }
                                else
                                {
                                    if (this._write_pointer == this._read_pointer)
                                        state = _CLIENT_STATE.IDLE;
                                    else
                                        state = _CLIENT_STATE.START_READ_JOURNAL;
                                }
                            }
                            else if (DateTime.Now > _tLastCommandToEco.AddSeconds(60))
                            {
                                LogFile.WriteErrorToLogFile("{0} Timeout while waiting for configuration", this.Name);
                                state = _CLIENT_STATE.ERROR;
                            }
                            break;

                        case _CLIENT_STATE.WAIT_CONFIG_ACK:

                            if (_fromEco.Count > 0 && get_answer("%CON") != "")
                            {
                                if (this._write_pointer == this._read_pointer)
                                    state = _CLIENT_STATE.IDLE;
                                else
                                {
                                    // if we have received an unsolicited frame, process it
                                    if (_fromEco.Count != 0)
                                    {
                                        while (_fromEco.Count != 0)
                                        {
                                            lock (_fromEco.SyncRoot)
                                            {
                                                str = (string)_fromEco.Dequeue();
                                            }
                                            process_frame(str);
                                        }
                                    }
                                 
                                    state = _CLIENT_STATE.START_READ_JOURNAL;
                                }
                            }
                            else if (DateTime.Now > _tLastCommandToEco.AddSeconds(60))
                            {
                                LogFile.WriteErrorToLogFile("{0} Timeout while waiting for configuration acknowledge", this.Name);
                                state = _CLIENT_STATE.ERROR;
                            }
                            break;

                        case _CLIENT_STATE.START_READ_JOURNAL:
                            if (this._write_pointer > this._read_pointer)
                                _numberOfExpectedEntries = this._write_pointer - this._read_pointer;
                            else
                            {
                                _numberOfExpectedEntries = this._write_pointer + (_MaxJournalEntries - this._read_pointer);
                            }

                            // plausibility check
                            if (_numberOfExpectedEntries > 10000)
                            {
                                DateTime tLastDataAccess = DateTime.Now;

                                try
                                {
                                    LastCommunicationResult res = ConnectionControl.SkpApiClient.GetContainerLastCommunication(_container_id);
                                    tLastDataAccess = (DateTime)res.LastCommunication;
                                }
                                catch (Exception excp)
                                {
                                    LogFile.WriteErrorToLogFile("{0}: Exception ({1}) while trying to get last communication time from container!", this.Name, excp.Message);
                                }

                                if (DateTime.Now.Subtract(tLastDataAccess).TotalDays < 15)
                                {
                                    string text = String.Format("{0}: Container hat aussergewöhnlich viele Einträge ({1}) im Journal. WritePointer: {2}, ReadPointer: {3}",
                                        this.Name, _numberOfExpectedEntries, _write_pointer, _read_pointer);

                                    ArrayList recipients = new ArrayList();
                                    ConnectionService.AlertingUser user = new ConnectionService.AlertingUser();
                                    user.Name = "Admin";
                                    user.EmailAddress = "johann.zehetner@poettinger.at";
                                    recipients.Add(user);
                                    ConnectionControl.SendMail(text, "Container Warnung", recipients).Wait();

                                    ConnectionControl.SkpApiClient.UpdateContainerLastCommunication(this._container_id, new UpdateLastCommunication(DateTime.Now));
                                    state = _CLIENT_STATE.IDLE;

                                    LogFile.WriteErrorToLogFile(text);
                                    break;
                                }
                            }

                            // maximum allowed in one frame is 13/8 depends on protocol version
                            if (_protocol_version >= 4)
                            {
                                if (_numberOfExpectedEntries > 8)
                                    _numberOfExpectedEntries = 8;
                            }
                            else
                            {
                                if (_numberOfExpectedEntries > 13)
                                    _numberOfExpectedEntries = 13;
                            }

                            String command = String.Format("#JOU?{0},{1}", this._read_pointer, _numberOfExpectedEntries);
                            SendCommand(command);

                            state = _CLIENT_STATE.READ_JOURNAL;
                            break;

                        case _CLIENT_STATE.READ_JOURNAL:

                            if (_fromEco.Count > 0)
                            {
                                if ((str = get_answer("%JOU")) != "")
                                {
                                    if (_bJournalEntriesAlreadyStored)
                                    {
                                        // in case we have got a SECCODE error while waiting for Readpointer acknowledge
                                        _bJournalEntriesAlreadyStored = false;
                                        LogFile.WriteMessageToLogFile("{0} Journalentries already stored -> Skip parsing", this.Name);
                                    }
                                    else
                                    {
                                        if (!ParseJournalEntries(str, _numberOfExpectedEntries))
                                        {
                                            LogFile.WriteErrorToLogFile("{0} Error while trying to parse journal entries", this.Name);
                                        }
                                    }
                                    retry = 0;
                                    SendNewReadpointer();
                                    state = _CLIENT_STATE.WAIT_READPOINTER;
                                }
                            }
                            else if (DateTime.Now > _tLastCommandToEco.AddSeconds(60))
                            {
                                LogFile.WriteErrorToLogFile("{0} Timeout while waiting for journal entries", this.Name);
                                state = _CLIENT_STATE.ERROR;
                            }
                            break;

                        case _CLIENT_STATE.WAIT_READPOINTER:

                            if (_fromEco.Count > 0)
                            {
                                if ((str = get_answer("%RDP")) != "")
                                {
//                                    if (!StoreReadPointer(str))
//                                    {
//                                        LogFile.WriteErrorToLogFile("{0} Error while trying to store readpointer", this.Name);
//                                        state = _CLIENT_STATE.ERROR;
//                                    }

                                    if (this._write_pointer == this._read_pointer)
                                    {
                                        ConnectionControl.SkpApiClient.UpdateContainerLastCommunication(this._container_id, new UpdateLastCommunication(DateTime.Now));
                                        state = _CLIENT_STATE.IDLE;
                                    }
                                    else
                                    {
                                        state = _CLIENT_STATE.START_READ_JOURNAL;
                                        _receivedEvent.Set();   // dont wait the timeout time
                                    }
                                }
                                else if (_bNewSeccodeCalculationNecessary)
                                {
                                    LogFile.WriteMessageToLogFile("{0} SECCODE invalid -> start config request", this.Name);
                                    SendCommand("#CON?");
                                    state = _CLIENT_STATE.WAIT_CONFIG;
                                    _bJournalEntriesAlreadyStored = true;
                                }
                            }
                            else if (DateTime.Now > _tLastCommandToEco.AddSeconds(60))
                            {
                                LogFile.WriteErrorToLogFile("{0} Timeout while waiting read pointer acknowledge", this.Name);
//                                if (++retry > 2)
                                    state = _CLIENT_STATE.ERROR;
//                                else
//                                    SendNewReadpointer();
                            }
                            break;

                        case _CLIENT_STATE.IDLE:
                            TimeSpan ts = TimeSpan.FromSeconds(_keepAliveInterval);

                            if (_tLastWritePointerQuery.Add(ts) < DateTime.Now)
                            {
                                SendCommand("#WRP?");
                                state = _CLIENT_STATE.WAIT_WRITEPOINTER;
                                break;
                            }

                            if (_tLastConfigCheck.AddSeconds(60) < DateTime.Now)
                            {
                                _tLastConfigCheck = DateTime.Now;
                                if (CheckConfiguration())
                                {
                                    state = _CLIENT_STATE.WAIT_CONFIG_ACK;
                                    break;
                                }
                            }

                            // check if we have to do some service commands
                            if (_serviceCommands.Count != 0)
                            {
                                ServiceCommand srv_cmd = null;

                                lock (_serviceCommands.SyncRoot)
                                {
                                    srv_cmd = (ServiceCommand)_serviceCommands.Dequeue();
                                }
                                string cmd;

                                if (_protocol_version < 4)
                                {
                                    cmd = String.Format("#SRV={0:D4},{1:D2},{2},{3}", srv_cmd.command, srv_cmd.delay_time,
                                    srv_cmd.reserved, this._security_code);
                                }
                                else
                                {
                                    cmd = String.Format("#SRV={0:D4},{1:D2},{2}", srv_cmd.command, srv_cmd.delay_time,
                                    srv_cmd.reserved);
                                }

                                SendCommand(cmd);
                            }

                            // if we have received an unsolicited frame, process it
                            if (_fromEco.Count != 0)
                            {
                                while (_fromEco.Count != 0)
                                {
                                    lock (_fromEco.SyncRoot)
                                    {
                                        str = (string)_fromEco.Dequeue();
                                    }
                                    process_frame(str);
                                }
                            }
                            break;

                        case _CLIENT_STATE.WAIT_WRITEPOINTER:
                            if (_fromEco.Count > 0)
                            {
                                if ((str = get_answer("%WRP")) != "")
                                {
                                    retry = 0;
                                    _tLastWritePointerQuery = DateTime.Now;
                                    str = str.Substring(5);
                                    this._write_pointer = Convert.ToInt32(str);
                                    if (this._write_pointer == this._read_pointer)
                                    {
                                        ConnectionControl.SkpApiClient.UpdateContainerLastCommunication(this._container_id, new UpdateLastCommunication(DateTime.Now));
                                        state = _CLIENT_STATE.IDLE;
                                    }
                                    else
                                        state = _CLIENT_STATE.START_READ_JOURNAL;
                                }
                            }
                            else if (DateTime.Now > _tLastCommandToEco.AddSeconds(60))
                            {
                                LogFile.WriteErrorToLogFile("{0} Timeout while waiting for write pointer", this.Name);
                                if (++retry > 1)
                                    state = _CLIENT_STATE.ERROR;
                                else
                                    state = _CLIENT_STATE.IDLE; // retry to query the write pointer
                            }
                            break;


                        case _CLIENT_STATE.WAIT_TUNNEL:

                            break;

                        case _CLIENT_STATE.ERROR:
                            LogFile.WriteErrorToLogFile("{0} Stop client due to error", this.Name);
                            Stop();
                            break;

                    }

                    // wait for either new frame recevied or someone wants to stop us
                    if ((index = WaitHandle.WaitAny(waitHandles, 100, false)) != WaitHandle.WaitTimeout)
                    {
                        if (index == 1)
                        {
                            SendQuitCommand();
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
            
            Controller.RemoveECOClient(this);

            _tcpClient.Close();

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
                                        lock (_fromEco.SyncRoot)
                                        {
                                            _fromEco.Enqueue(str);
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
                    else if (DateTime.Now > _tLastCommandFromEco.Add(TimeSpan.FromSeconds(_keepAliveInterval + 120)))
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
                string msg = excp.Message;
//                LogFile.WriteErrorToLogFile("{0} Exception ({1}) in !!Receive!! occured", this.Name, excp.Message);
            }

            LogFile.WriteMessageToLogFile("{0} disconnected", this.Name);
        }

#endregion

#endregion
    }
}
