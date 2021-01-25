#undef TRACE_PERFORMANCE

using Luthien;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Threading;

namespace SKP
{
    public class AlertingUser
    {
        private int remote_control_idField;
        private int remote_control_typeField;
        private string memoField;
        private string contactField;

        /// <remarks/>
        public int remote_control_id
        {
            get
            {
                return this.remote_control_idField;
            }
            set
            {
                this.remote_control_idField = value;
            }
        }

        /// <remarks/>
        public int remote_control_type
        {
            get
            {
                return this.remote_control_typeField;
            }
            set
            {
                this.remote_control_typeField = value;
            }
        }

        /// <remarks/>
        public string memo
        {
            get
            {
                return this.memoField;
            }
            set
            {
                this.memoField = value;
            }
        }

        /// <remarks/>
        public string contact
        {
            get
            {
                return this.contactField;
            }
            set
            {
                this.contactField = value;
            }
        }
    }

    public class Transaction
    {
        private int _customer_idField;
        private string _customer_numberField;
        private int _location_idField;
        private int _container_idField;
        private int _transaction_status_idField;
        private System.DateTime _dateField;
        private int _weightField;
        private int _durationField;
        private int _amountField;
        private decimal _positive_credit_balanceField;
        private string _gsm_numberField;
        private eCARD_TYPE _card_type_idField;

        /// <remarks/>
        public int _customer_id
        {
            get
            {
                return this._customer_idField;
            }
            set
            {
                this._customer_idField = value;
            }
        }

        /// <remarks/>
        public string _customer_number
        {
            get
            {
                return this._customer_numberField;
            }
            set
            {
                this._customer_numberField = value;
            }
        }

        /// <remarks/>
        public int _location_id
        {
            get
            {
                return this._location_idField;
            }
            set
            {
                this._location_idField = value;
            }
        }

        /// <remarks/>
        public int _container_id
        {
            get
            {
                return this._container_idField;
            }
            set
            {
                this._container_idField = value;
            }
        }

        /// <remarks/>
        public int _transaction_status_id
        {
            get
            {
                return this._transaction_status_idField;
            }
            set
            {
                this._transaction_status_idField = value;
            }
        }

        /// <remarks/>
        public System.DateTime _date
        {
            get
            {
                return this._dateField;
            }
            set
            {
                this._dateField = value;
            }
        }

        /// <remarks/>
        public int _weight
        {
            get
            {
                return this._weightField;
            }
            set
            {
                this._weightField = value;
            }
        }

        /// <remarks/>
        public int _duration
        {
            get
            {
                return this._durationField;
            }
            set
            {
                this._durationField = value;
            }
        }

        /// <remarks/>
        public int _amount
        {
            get
            {
                return this._amountField;
            }
            set
            {
                this._amountField = value;
            }
        }

        /// <remarks/>
        public decimal _positive_credit_balance
        {
            get
            {
                return this._positive_credit_balanceField;
            }
            set
            {
                this._positive_credit_balanceField = value;
            }
        }

        /// <remarks/>
        public string _gsm_number
        {
            get
            {
                return this._gsm_numberField;
            }
            set
            {
                this._gsm_numberField = value;
            }
        }

        /// <remarks/>
        public eCARD_TYPE _card_type_id
        {
            get
            {
                return this._card_type_idField;
            }
            set
            {
                this._card_type_idField = value;
            }
        }
    }

    public enum eCARD_TYPE
    {
        /// <remarks/>
        NONE,
        /// <remarks/>
        PREPAID,
        /// <remarks/>
        INVOICE,
    }

    public partial class Customer
    {
        private string _customer_numberField;
        private int _customer_idField;
        private int _card_extensionField;
        private int _location_group_idField;
        private int _min_amountField;
        private int _card_type_idField;
        private int _positive_balanceField;
        private int _price_hundred_kiloField;
        private int _language_idField;
        private int _card_numberField;
        private System.DateTime _releaseDateField;
        private bool _bRechargeField;
        private bool _b_location_group_changedField;
        private bool _b_language_changedField;
        private bool _b_min_amount_changedField;
        private string _cardType;
        private string _cardSerialNumber;

        /// <remarks/>
        public string _customer_number
        {
            get
            {
                return this._customer_numberField;
            }
            set
            {
                this._customer_numberField = value;
            }
        }

        /// <remarks/>
        public int _customer_id
        {
            get
            {
                return this._customer_idField;
            }
            set
            {
                this._customer_idField = value;
            }
        }

        /// <remarks/>
        public int _card_extension
        {
            get
            {
                return this._card_extensionField;
            }
            set
            {
                this._card_extensionField = value;
            }
        }

        /// <remarks/>
        public int _location_group_id
        {
            get
            {
                return this._location_group_idField;
            }
            set
            {
                this._location_group_idField = value;
            }
        }

        /// <remarks/>
        public int _min_amount
        {
            get
            {
                return this._min_amountField;
            }
            set
            {
                this._min_amountField = value;
            }
        }

        /// <remarks/>
        public int _card_type_id
        {
            get
            {
                return this._card_type_idField;
            }
            set
            {
                this._card_type_idField = value;
            }
        }

        /// <remarks/>
        public int _positive_balance
        {
            get
            {
                return this._positive_balanceField;
            }
            set
            {
                this._positive_balanceField = value;
            }
        }

        /// <remarks/>
        public int _price_hundred_kilo
        {
            get
            {
                return this._price_hundred_kiloField;
            }
            set
            {
                this._price_hundred_kiloField = value;
            }
        }

        /// <remarks/>
        public int _language_id
        {
            get
            {
                return this._language_idField;
            }
            set
            {
                this._language_idField = value;
            }
        }

        /// <remarks/>
        public int _card_number
        {
            get
            {
                return this._card_numberField;
            }
            set
            {
                this._card_numberField = value;
            }
        }

        /// <remarks/>
        public System.DateTime _releaseDate
        {
            get
            {
                return this._releaseDateField;
            }
            set
            {
                this._releaseDateField = value;
            }
        }

        /// <remarks/>
        public bool _bRecharge
        {
            get
            {
                return this._bRechargeField;
            }
            set
            {
                this._bRechargeField = value;
            }
        }

        /// <remarks/>
        public bool _b_location_group_changed
        {
            get
            {
                return this._b_location_group_changedField;
            }
            set
            {
                this._b_location_group_changedField = value;
            }
        }

        /// <remarks/>
        public bool _b_language_changed
        {
            get
            {
                return this._b_language_changedField;
            }
            set
            {
                this._b_language_changedField = value;
            }
        }

        /// <remarks/>
        public bool _b_min_amount_changed
        {
            get
            {
                return this._b_min_amount_changedField;
            }
            set
            {
                this._b_min_amount_changedField = value;
            }
        }

        public string CardType { get => _cardType; set => _cardType = value; }
        public string CardSerialNumber { get => _cardSerialNumber; set => _cardSerialNumber = value; }
    }

    public interface IWIPDataAccess
    {
        /// <summary>
        /// Get all containers with SPS_RELEASE > 3 which should be online
        /// </summary>
        /// <param name="container_ids"></param>
        /// <returns></returns>
        bool GetConfiguredContainers(ref ArrayList containers);

        /// <summary>
        /// Get the essential operator parameters
        /// </summary>
        /// <param name="operator_id"></param>
        /// <param name="operator_name"></param>
        /// <param name="currency_id"></param>
        /// <param name="language_code"></param>
        /// <returns></returns>
        bool GetOperatorParams(int operator_id, ref string operator_name, ref int currency_id, ref string language_code);

        /// <summary>
        /// Get Parameters for customer data import and export
        /// </summary>
        /// <param name="operator_id"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        bool GetDataExportParams(int operator_id, ref DataExportParams parameters);

        /// <summary>
        /// Get all customers data from a specified operator
        /// </summary>
        /// <param name="customers"></param>
        /// <returns></returns>
        bool GetOperatorsCustomers(int operator_id, ref ArrayList customers);

        /// <summary>
        /// Get the essential customer parameters
        /// </summary>
        /// <param name="customer_number"></param>
        /// <param name="customer_id"></param>
        /// <param name="card_type_id"></param>
        /// <returns></returns>
        bool GetCustomerData(string customer_number, int operator_id, ref int customer_id, ref eCARD_TYPE card_type_id, ref string language_code);

        /// <summary>
        /// Get all necessary customer data
        /// </summary>
        /// <param name="customer_number"></param>
        /// <param name="operator_id"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        bool GetCustomerDataEx(string customer_number, int operator_id, ref CustomerParams customer);

        /// <summary>
        /// Get location time zone
        /// </summary>
        /// <param name="location_id"></param>
        /// <param name="operator_id"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        bool GetLocationTimeZone(int location_group_id, ref string strTimeZone);

        /// <summary>
        /// Get time of last communication
        /// </summary>
        /// <param name="container_id"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        bool GetContainerLastCommunication(int container_id, ref DateTime date);

        /// <summary>
        /// Modify existing customer's data
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        bool ModifyCustomerData(int operator_id, CustomerParams existing_customer, CustomerParams imported_customer);

        /// <summary>
        /// Add an imported customer
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        bool AddNewCustomer(int operator_id, CustomerParams customer);

        /// <summary>
        /// Get customers new credits
        /// </summary>
        /// <param name="customer_id"></param>
        /// <param name="new_credit"></param>
        /// <returns></returns>
        bool GetCustomerRechargeCredit(int customer_id, ref int new_credit);

        /// <summary>
        /// Retrieve customers locationgroup bitmask
        /// </summary>
        /// <param name="customer_id"></param>
        /// <param name="customer_locationgroup_mask"></param>
        /// <returns></returns>
        bool GetCustomerLocationgroupMask(int customer_id, ref int customer_locationgroup_mask);

        /// <summary>
        /// Retrieve csutomers location group information
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        bool GetCustomerLocationGroups(ref CustomerParams customer);

        /// <summary>
        /// Retrieve customer minimum and maximum amount for credit recharges
        /// </summary>
        /// <param name="customer_id"></param>
        /// <param name="min_amount"></param>
        /// <param name="max_amount"></param>
        /// <returns></returns>
        bool GetCustomerMinMaxAmount(int customer_id, ref int payment_id, ref int min_amount, ref int max_amount);

        /// <summary>
        /// Retrieve customer address information
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        bool GetCustomerAddress(ref CustomerParams customer);

        /// <summary>
        /// Get the essential location parameters
        /// </summary>
        /// <param name="location_id"></param>
        /// <param name="location"></param>
        /// <param name="nightlock_start"></param>
        /// <param name="nightlock_stop"></param>
        /// <param name="gate_limit"></param>
        /// <param name="container_id"></param>
        /// <param name="price_hundred_kilo"></param>
        /// <param name="bEmptying"></param>
        /// <returns></returns>
        bool GetLocationParams(int location_id, ref string location, ref DateTime nightlock_start, ref DateTime nightlock_stop,
                ref int gate_limit, ref int container_id, ref int price_hundred_kilo, ref bool bEmptying);

        /// <summary>
        /// Translate language code to language id
        /// </summary>
        /// <param name="language_code"></param>
        /// <param name="language_id"></param>
        /// <returns></returns>
        bool GetLocationLanguageId(string language_code, ref int language_id);

        /// <summary>
        /// Get the selected languages for specified location
        /// </summary>
        /// <param name="location_id"></param>
        /// <param name="language_code"></param>
        /// <returns></returns>
        bool GetLocationLanguages(int location_id, ref int language_code);

        /// <summary>
        /// Get container id from simcard gsm number
        /// </summary>
        /// <param name="gsm_number"></param>
        /// <param name="container_id"></param>
        /// <returns></returns>
        bool GetContainerId(string gsm_number, ref int container_id);

        /// <summary>
        /// Get the essential container parameters
        /// </summary>
        /// <param name="_container_id"></param>
        /// <param name="container_type_id"></param>
        /// <param name="gsm_number"></param>
        /// <param name="read_pointer"></param>
        /// <param name="write_pointer"></param>
        /// <returns></returns>
        bool GetContainerData(int container_id, ref int container_type_id, ref string gsm_number, ref int read_pointer, ref int write_pointer);

        /// <summary>
        /// Get container type
        /// </summary>
        /// <param name="container_type_id"></param>
        /// <param name="container_type"></param>
        /// <returns></returns>
        bool GetContainerType(int container_type_id, ref string container_type);

        /// <summary>
        /// Update read and write pointer of a specified container
        /// </summary>
        /// <param name="container_id"></param>
        /// <param name="read_pointer"></param>
        /// <param name="write_pointer"></param>
        /// <returns></returns>
        bool UpdateReadWritePointer(int container_id, int read_pointer, int write_pointer);

        /// <summary>
        /// Update customer's last waste disposal time
        /// </summary>
        /// <param name="customer_id"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        bool UpdateCustomerLastWasteDisposal(int customer_id, DateTime date);

        /// <summary>
        /// Update operator's last data export
        /// </summary>
        /// <param name="operator_id"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        bool UpdateLastDataExport(int operator_id, DateTime date);

        /// <summary>
        /// Update last communication date for specified container
        /// </summary>
        /// <param name="container_id"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        bool UpdateContainerLastCommunication(int container_id, DateTime date);

        /// <summary>
        /// Update csutomers positive credit balance
        /// </summary>
        /// <param name="customer_id"></param>
        /// <param name="positive_credit_balance"></param>
        /// <returns></returns>
        bool UpdateCustomersPositiveCreditBalance(int customer_id, decimal positive_credit_balance);

        /// <summary>
        /// Update locations container id
        /// </summary>
        /// <param name="location_id"></param>
        /// <param name="container_id"></param>
        /// <returns></returns>
        bool UpdateLocationContainerId(int location_id, int container_id);

        /// <summary>
        /// Store container status change messages in container status table
        /// </summary>
        /// <param name="container_id"></param>
        /// <param name="location_id"></param>
        /// <param name="gsm_number"></param>
        /// <param name="code"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        bool StoreContainerStatus(int container_id, int location_id, string gsm_number, int code, DateTime date);

        /// <summary>
        /// Update recharged flag for specified customer in recharge table
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        bool StoreRechargedFlag(int conatiner_id, Customer customer);

        /// <summary>
        /// Store transaction journal entry in database
        /// </summary>
        /// <param name="container_id"></param>
        /// <param name="transaction"></param>
        /// <param name="last_trans_id"></param>
        /// <param name="gsm_number"></param>
        /// <returns></returns>
        bool StoreTransaction(int container_id, Transaction transaction, int last_trans_id, string gsm_number);

        /// <summary>
        /// Retrieve last transaction id
        /// </summary>
        /// <param name="last_transaction_id"></param>
        /// <returns></returns>
        bool GetMaxTransactionId(ref int last_transaction_id);
      
        /// <summary>
        /// Retrieve message text in specified language
        /// </summary>
        /// <param name="language_keyword"></param>
        /// <param name="language_code"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        bool GetMessageText(int language_keyword, string language_code, ref string text);


        /// <summary>
        /// Retrieve user list in case of an container erroe
        /// </summary>
        /// <param name="container_id"></param>
        /// <param name="number_of_users"></param>
        /// <param name="users"></param>
        /// <returns></returns>
        bool GetAlertingUsers(int container_id, ref int number_of_users, ref AlertingUser[] users);

        /// <summary>
        /// Retrieve user list in case of a full container
        /// </summary>
        /// <param name="container_id"></param>
        /// <param name="number_of_users"></param>
        /// <param name="users"></param>
        /// <returns></returns>
        bool GetFullContainerUsers(int container_id, ref int number_of_users, ref AlertingUser[] users);

        /// <summary>
        /// Calculate actual weight of container due
        /// </summary>
        /// <param name="container_id"></param>
        /// <param name="location_id"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        bool CalculateContainerWeight(int container_id, int location_id, ref float weight);

        /// <summary>
        /// Increment container transaction counter
        /// </summary>
        /// <param name="container_id"></param>
        /// <returns></returns>
        bool IncrementContainerTransactionCounter(int container_id);
    }

    class WIPLocalDataAccess : IWIPDataAccess
    {
        #region Members

        private string _connection_string;
        private static Mutex mutex = new Mutex();

        #endregion

        #region Constructor

        public WIPLocalDataAccess(string connection_string)
        {
            _connection_string = connection_string;
        }

        #endregion

        #region IWIPDataAccess Members

        public bool AddNewCustomer(int operator_id, CustomerParams customer)
        {
            bool retval = true;
            int max_address_id = 0;
            int max_customer_id = 0;

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    // get max address id
                    string sqlStatement = "SELECT MAX(ADDRESS_ID) AS MAX_ADDRESS_ID FROM ADDRESS";

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                max_address_id = (int)reader[0];
                            }
                        }
                    }

                    // store new address in database
                    sqlStatement = "INSERT INTO ADDRESS ([ADDRESS_ID], [COUNTRY_ID], [STREET], [ZIP_CODE], [CITY]) VALUES (@AddressId, @CountryId, @Street, @ZipCode, @City)";

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter p_AddressId = new SqlParameter("@AddressId", SqlDbType.Int);
                        p_AddressId.Value = ++max_address_id;
                        cmd.Parameters.Add(p_AddressId);

                        SqlParameter p_countryId = new SqlParameter("@CountryId", SqlDbType.Int);
                        p_countryId.Value = 4;  // fixed with italy at the moment only for Brixen
                        cmd.Parameters.Add(p_countryId);

                        SqlParameter p_street = new SqlParameter("@Street", SqlDbType.VarChar);
                        p_street.Value = customer.Street;
                        cmd.Parameters.Add(p_street);

                        SqlParameter p_zip = new SqlParameter("@ZipCode", SqlDbType.VarChar);
                        p_zip.Value = customer.ZIPCode;
                        cmd.Parameters.Add(p_zip);

                        SqlParameter p_city = new SqlParameter("@City", SqlDbType.VarChar);
                        p_city.Value = customer.City;
                        cmd.Parameters.Add(p_city);

                        // store it
                        if (cmd.ExecuteNonQuery() != 1)
                        {
                            LogFile.WriteErrorToLogFile("Error while excuting sql statement: {0}", sqlStatement);
                            retval = false;
                        }
                    }

                    // get max customer id
                    sqlStatement = "SELECT MAX(CUSTOMER_ID) AS MAX_CUSTOMER_ID FROM CUSTOMER";

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                max_customer_id = (int)reader[0];
                            }
                        }
                    }

                    // get location language code
                    sqlStatement = "SELECT LANGUAGE_CODE FROM LOCATION_LANGUAGE WHERE LOCATION_LANGUAGE_ID=@LocationLanguageId";
                    if (customer.LanguageCode == "") customer.LanguageCode = "DE";

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter p1 = new SqlParameter("@LocationLanguageId", SqlDbType.Int);
                        p1.Value = customer.LanguageId;
                        cmd.Parameters.Add(p1);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                customer.LanguageCode = (string)reader[0];

                                if (customer.LanguageCode == "GB")
                                    customer.LanguageCode = "EN";
                            }
                        }
                    }

                    // store new customer
                    sqlStatement = "INSERT INTO CUSTOMER ([CUSTOMER_ID], [OPERATOR_ID], [SALUTATION_ID], [CARD_TYPE_ID], [ADDRESS_ID], [CUSTOMER_NUMBER], [TITLE], [FIRST_NAME], [LAST_NAME], [COMPANY_NAME], [PHONE], [EMAIL], [LOGIN], [PASSWORD], [CARD_ID], [POSITIVE_CREDIT_BALANCE], [CARD_DATE], [PRICE_HUNDRED_KILO], [CARD_EXTENSION], [LANGUAGE_CODE], [MOBIL_PHONE]) VALUES(@CustomerId, @OperatorId, @SalutationId, @CardTypeId, @AddressId, @CustomerNumber, @Title, @FirstName, @LastName, @CompanyName, @Phone, @Email, @Login, @Password, @CardId, @PositiveCreditBalance, @CardDate, @PriceHundredKilo, @CardExtension, @LanguageCode, @MobilePhone)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter p1 = new SqlParameter("@CustomerId", SqlDbType.Int);
                        p1.Value = ++max_customer_id;
                        cmd.Parameters.Add(p1);

                        SqlParameter p2 = new SqlParameter("@OperatorId", SqlDbType.Int);
                        p2.Value = operator_id;
                        cmd.Parameters.Add(p2);

                        SqlParameter p3 = new SqlParameter("@SalutationId", SqlDbType.Int);
                        p3.Value = customer.SalutationId;
                        cmd.Parameters.Add(p3);

                        SqlParameter p4 = new SqlParameter("@CardTypeId", SqlDbType.Int);
                        p4.Value = customer.CardTypeId;
                        cmd.Parameters.Add(p4);

                        SqlParameter p5 = new SqlParameter("@AddressId", SqlDbType.Int);
                        p5.Value = max_address_id;
                        cmd.Parameters.Add(p5);

                        SqlParameter p6 = new SqlParameter("@CustomerNumber", SqlDbType.VarChar);
                        p6.Value = customer.CustomerNumber;
                        cmd.Parameters.Add(p6);

                        SqlParameter p7 = new SqlParameter("@Title", SqlDbType.VarChar);
                        p7.Value = customer.Title;
                        cmd.Parameters.Add(p7);

                        SqlParameter p8 = new SqlParameter("@FirstName", SqlDbType.VarChar);
                        p8.Value = customer.FirstName;
                        cmd.Parameters.Add(p8);

                        SqlParameter p9 = new SqlParameter("@LastName", SqlDbType.VarChar);
                        p9.Value = customer.LastName;
                        cmd.Parameters.Add(p9);

                        SqlParameter p10 = new SqlParameter("@CompanyName", SqlDbType.VarChar);
                        p10.Value = customer.CompanyName;
                        cmd.Parameters.Add(p10);

                        SqlParameter p11 = new SqlParameter("@Phone", SqlDbType.VarChar);
                        p11.Value = customer.PhoneNumber;
                        cmd.Parameters.Add(p11);

                        SqlParameter p12 = new SqlParameter("@Email", SqlDbType.VarChar);
                        p12.Value = customer.Email;
                        cmd.Parameters.Add(p12);

                        SqlParameter p13 = new SqlParameter("@Login", SqlDbType.VarChar);
                        p13.Value = customer.LogIn;
                        cmd.Parameters.Add(p13);

                        SqlParameter p14 = new SqlParameter("@Password", SqlDbType.VarChar);
                        p14.Value = customer.Password;
                        cmd.Parameters.Add(p14);

                        SqlParameter p15 = new SqlParameter("@CardId", SqlDbType.Int);
                        if (customer.CardId == -1)
                            p15.Value = DBNull.Value;
                        else
                            p15.Value = customer.CardId;

                        cmd.Parameters.Add(p15);

                        SqlParameter p16 = new SqlParameter("@PositiveCreditBalance", SqlDbType.Decimal);
                        p16.Value = customer.PositiveCreditBalance;
                        cmd.Parameters.Add(p16);

                        SqlParameter p17 = new SqlParameter("@CardDate", SqlDbType.DateTime);
                        p17.Value = customer.CardReleaseDate;
                        cmd.Parameters.Add(p17);

                        SqlParameter p18 = new SqlParameter("@PriceHundredKilo", SqlDbType.Int);
                        p18.Value = customer.PriceHundredKilo;
                        cmd.Parameters.Add(p18);

                        SqlParameter p19 = new SqlParameter("@CardExtension", SqlDbType.Int);
                        if (customer.CardReleaseNumber == -1)
                            p19.Value = DBNull.Value;
                        else
                            p19.Value = customer.CardReleaseNumber;
                        cmd.Parameters.Add(p19);

                        SqlParameter p20 = new SqlParameter("@LanguageCode", SqlDbType.VarChar);
                        p20.Value = customer.LanguageCode;
                        cmd.Parameters.Add(p20);

                        SqlParameter p21 = new SqlParameter("@MobilePhone", SqlDbType.VarChar);
                        p21.Value = customer.MobilePhone;
                        cmd.Parameters.Add(p21);

                        // store it
                        if (cmd.ExecuteNonQuery() != 1)
                        {
                            LogFile.WriteErrorToLogFile("Error while excuting sql statement: {0}", sqlStatement);
                            retval = false;
                        }
                    }

                    // store new payment range
                    sqlStatement = "INSERT INTO PAYMENT_RANGE ([PAYMENT_ID], [FROM_AMOUNT], [TO_AMOUNT], [CUSTOMER_ID]) VALUES (@PaymentId, @FromAmount, @ToAmount, @CustomerId)";

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter p1 = new SqlParameter("@PaymentId", SqlDbType.Int);
                        p1.Value = 2;
                        cmd.Parameters.Add(p1);

                        SqlParameter p2 = new SqlParameter("@FromAmount", SqlDbType.Int);
                        p2.Value = customer.FromAmount;
                        cmd.Parameters.Add(p2);

                        SqlParameter p3 = new SqlParameter("@ToAmount", SqlDbType.VarChar);
                        p3.Value = customer.ToAmount.ToString();
                        cmd.Parameters.Add(p3);

                        SqlParameter p4 = new SqlParameter("@CustomerId", SqlDbType.Int);
                        p4.Value = max_customer_id;
                        cmd.Parameters.Add(p4);

                        // store it
                        if (cmd.ExecuteNonQuery() != 1)
                        {
                            LogFile.WriteErrorToLogFile("Error while excuting sql statement: {0}", sqlStatement);
                            retval = false;
                        }
                    }

                    // store new customer group
                    sqlStatement = "INSERT INTO GROUP_OF_CUSTOMER ([GROUPS_ID], [CUSTOMER_ID]) VALUES (@GroupsId, @CustomerId)";

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter p1 = new SqlParameter("@GroupsId", SqlDbType.Int);
                        p1.Value = 4;
                        cmd.Parameters.Add(p1);

                        SqlParameter p2 = new SqlParameter("@CustomerId", SqlDbType.Int);
                        p2.Value = max_customer_id;
                        cmd.Parameters.Add(p2);

                        // store it
                        if (cmd.ExecuteNonQuery() != 1)
                        {
                            LogFile.WriteErrorToLogFile("Error while excuting sql statement: {0}", sqlStatement);
                            retval = false;
                        }
                    }

                    // store location group membership
                    ArrayList location_groups = new ArrayList();
                    // get new location group ids
                    for (UInt32 i = 0; i < 32; i++)
                    {
                        UInt32 bitmask = (UInt32)(1 << (int)i);

                        if (((UInt32)customer.LocationGroupMask & bitmask) == bitmask)
                        {
                            // get location group id
                            sqlStatement = "SELECT LOCATION_GROUP_ID FROM LOCATION_GROUP WHERE OPERATOR_ID=@OperatorId AND OPERATOR_LOCATION_GROUP_BIT=@LocationGroupBit AND ACTIVE = 0";
                            using (SqlCommand cmd1 = new SqlCommand(sqlStatement, sqlConnection))
                            {
                                SqlParameter p1 = new SqlParameter("@OperatorId", SqlDbType.Int);
                                p1.Value = operator_id;
                                cmd1.Parameters.Add(p1);

                                SqlParameter p2 = new SqlParameter("@LocationGroupBit", SqlDbType.Int);
                                p2.Value = i + 1;
                                cmd1.Parameters.Add(p2);

                                using (SqlDataReader reader = cmd1.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        int location_group_id = (int)reader[0];

                                        location_groups.Add(location_group_id);
                                    }
                                }
                            }
                        }
                    }

                    // add location groups to customer loaction group
                    sqlStatement = "INSERT INTO CUSTOMER_LOCATION_GROUP ([CUSTOMER_ID], [LOCATION_GROUP_ID]) VALUES (@CustomerId, @LocationGroupId)";

                    foreach (int location_group_id in location_groups)
                    {
                        using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                        {
                            SqlParameter p1 = new SqlParameter("@CustomerId", SqlDbType.Int);
                            p1.Value = max_customer_id;
                            cmd.Parameters.Add(p1);

                            SqlParameter p2 = new SqlParameter("@LocationGroupId", SqlDbType.Int);
                            p2.Value = location_group_id;
                            cmd.Parameters.Add(p2);

                            // store it
                            if (cmd.ExecuteNonQuery() != 1)
                            {
                                LogFile.WriteErrorToLogFile("Error while excuting sql statement: {0}", sqlStatement);
                                retval = false;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("Exception: {0} in \'AddNewCustomer\' appeared. Customer ID/Number: {1}/{2}", e.Message, customer.CustomerId, customer.CustomerNumber);
                retval = false;
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool GetConfiguredContainers(ref ArrayList containers)
        {
            //string sqlStatement = "SELECT CONTAINER_ID FROM CONTAINER WHERE SPS_RELEASE_ID >= 3 AND COMMUNICATION_START < @Now AND (COMMUNICATION_END IS NULL OR COMMUNICATION_END > @Now)";
            string sqlStatement = "SELECT CONTAINER_ID, COMMUNICATION_START, COMMUNICATION_END, OPERATOR_ID, DEVICE_NUMBER, GSM_NUMBER, LAST_COMMUNICATION FROM CONTAINER WHERE SPS_RELEASE_ID >= 3";

            bool retval = true;

#if TRACE_PERFORMANCE
            DateTime tStart = DateTime.Now;
#endif
            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        //                        SqlParameter p_date = new SqlParameter("@Now", SqlDbType.DateTime);
                        //                        p_date.Value = DateTime.Now;

                        //                        cmd.Parameters.Add(p_date);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DateTime start, end;

                                Client cl = new Client();
                                cl.ContainerID = (int)reader[0];

                                // check if communication start and end are in range
                                if (reader[1].GetType() == typeof(System.DBNull))
                                    start = new DateTime();
                                else
                                    start = (DateTime)reader[1];

                                if (start > DateTime.Now)
                                    cl.IsDisabled = true;
                                else
                                {
                                    if (reader[2].GetType() == typeof(System.DBNull))
                                        end = DateTime.Now.Add(new TimeSpan(1, 0, 0, 0, 0));
                                    else
                                        end = (DateTime)reader[2];

                                    if (end < DateTime.Now)
                                        cl.IsDisabled = true;
                                }

                                int operator_id = 0;
                                int currency_id = 0;
                                string operator_name = "";
                                string language_code = "";

                                if (reader[3].GetType() != typeof(System.DBNull))
                                {
                                    operator_id = (int)reader[3];
                                    if (GetOperatorParams(operator_id, ref operator_name, ref currency_id, ref language_code))
                                        cl.OperatorName = operator_name;
                                }

                                if (reader[4].GetType() != typeof(System.DBNull))
                                    cl.DeviceNumber = (string)reader[4];

                                if (reader[5].GetType() != typeof(System.DBNull))
                                    cl.GSMNumber = (string)reader[5];

                                if (reader[6].GetType() != typeof(System.DBNull))
                                    cl.TimeLastCommunication = (DateTime)reader[6];
                                else
                                    cl.TimeLastCommunication = DateTime.Now;

                                containers.Add(cl);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("{0} in \'GetConfiguredContainers\' appeared.", e.Message);
                retval = false;
            }
            finally
            {
                mutex.ReleaseMutex();
            }

#if TRACE_PERFORMANCE
            LogFile.WriteMessageToLogFile("GetConfiguredContainers took : {0} ms", DateTime.Now.Subtract(tStart));
#endif
            return retval;
        }

        public bool GetContainerId(string gsm_number, ref int container_id)
        {
            string sqlStatement = "SELECT CONTAINER_ID FROM CONTAINER WHERE GSM_NUMBER=@GSM_Number";
            bool retval = true;

#if TRACE_PERFORMANCE
            DateTime tStart = DateTime.Now;
#endif
            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter param_gsm_number = new SqlParameter("@GSM_Number", SqlDbType.VarChar);
                        param_gsm_number.Value = gsm_number;

                        cmd.Parameters.Add(param_gsm_number);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                container_id = (int)reader[0];
                            }
                            else
                            {
//                                LogFile.WriteErrorToLogFile("Error while excuting sql statement: {0}", sqlStatement);
                                retval = false;
                            }

                            reader.Close();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("{0} in \'GetContainerId\' appeared.", e.Message);
                retval = false;
            }
            finally
            {
                mutex.ReleaseMutex();
            }

#if TRACE_PERFORMANCE
            LogFile.WriteMessageToLogFile("GetConfiguredContainers took : {0} ms", DateTime.Now.Subtract(tStart));
#endif
            return retval;
        }

        public bool GetContainerData(int container_id, ref int container_type_id, ref string gsm_number, ref int read_pointer, ref int write_pointer)
        {
            string sqlStatement = "SELECT CONTAINER_TYPE_ID, GSM_NUMBER, READ_POINTER, WRITE_POINTER FROM CONTAINER WHERE CONTAINER_ID=@Container_Id";
            bool retval = true;

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter param_contid = new SqlParameter("@Container_Id", SqlDbType.Int);
                        param_contid.Value = container_id;

                        cmd.Parameters.Add(param_contid);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                container_type_id = (int)reader[0];
                                gsm_number = (string)reader[1];
                                read_pointer = (int)reader[2];
                                write_pointer = (int)reader[3];
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
                LogFile.WriteErrorToLogFile("{0} in \'GetContainerData\' appeared.", e.Message);
                retval = false;
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool GetOperatorsCustomers(int operator_id, ref ArrayList customers)
        {
            string sqlStatement = "SELECT CUSTOMER_ID, SALUTATION_ID, CARD_TYPE_ID, ADDRESS_ID, CUSTOMER_NUMBER, TITLE, FIRST_NAME, LAST_NAME, COMPANY_NAME, PHONE, EMAIL, LOGIN, PASSWORD, CARD_ID, POSITIVE_CREDIT_BALANCE, LAST_WASTE_DISPOSAL, CARD_DATE, PRICE_HUNDRED_KILO, CARD_EXTENSION, LANGUAGE_CODE, MOBIL_PHONE FROM CUSTOMER WHERE OPERATOR_ID=@Operator_Id ORDER BY CUSTOMER_NUMBER";
            bool retval = true;

#if TRACE_PERFORMANCE
            DateTime tStart = DateTime.Now;
#endif
            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter param_opid = new SqlParameter("@Operator_Id", SqlDbType.Int);
                        param_opid.Value = operator_id;

                        cmd.Parameters.Add(param_opid);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CustomerParams cust_param = new CustomerParams();

                                cust_param.CustomerId = (int)reader[0];

                                if (reader[1].GetType() == typeof(System.DBNull))
                                    cust_param.SalutationId = -1;
                                else
                                    cust_param.SalutationId = (int)reader[1];

                                if (reader[2].GetType() == typeof(System.DBNull))
                                    cust_param.CardTypeId = -1;
                                else
                                    cust_param.CardTypeId = (int)reader[2];

                                if (reader[3].GetType() == typeof(System.DBNull))
                                    cust_param.AddressId = -1;
                                else
                                    cust_param.AddressId = (int)reader[3];

                                if (reader[4].GetType() == typeof(System.DBNull))
                                    cust_param.CustomerNumber = "";
                                else
                                    cust_param.CustomerNumber = (string)reader[4];

                                if (reader[5].GetType() == typeof(System.DBNull))
                                    cust_param.Title = "";
                                else
                                    cust_param.Title = (string)reader[5];

                                if (reader[6].GetType() == typeof(System.DBNull))
                                    cust_param.FirstName = "";
                                else
                                    cust_param.FirstName = (string)reader[6];

                                if (reader[7].GetType() == typeof(System.DBNull))
                                    cust_param.LastName = "";
                                else
                                    cust_param.LastName = (string)reader[7];

                                if (reader[8].GetType() == typeof(System.DBNull))
                                    cust_param.CompanyName = "";
                                else
                                    cust_param.CompanyName = (string)reader[8];

                                if (reader[9].GetType() == typeof(System.DBNull))
                                    cust_param.PhoneNumber = "";
                                else
                                    cust_param.PhoneNumber = (string)reader[9];

                                if (reader[10].GetType() == typeof(System.DBNull))
                                    cust_param.Email = "";
                                else
                                    cust_param.Email = (string)reader[10];

                                if (reader[11].GetType() == typeof(System.DBNull))
                                    cust_param.LogIn = "";
                                else
                                    cust_param.LogIn = (string)reader[11];

                                if (reader[12].GetType() == typeof(System.DBNull))
                                    cust_param.Password = "";
                                else
                                    cust_param.Password = (string)reader[12];

                                if (reader[13].GetType() == typeof(System.DBNull))
                                    cust_param.CardId = -1;
                                else
                                    cust_param.CardId = (int)reader[13];

                                if (reader[14].GetType() == typeof(System.DBNull))
                                    cust_param.PositiveCreditBalance = -1;
                                else
                                    cust_param.PositiveCreditBalance = (decimal)reader[14];

                                if (reader[15].GetType() != typeof(System.DBNull))
                                    cust_param.LastTransaction = (DateTime)reader[15];

                                if (reader[16].GetType() != typeof(System.DBNull))
                                    cust_param.CardReleaseDate = (DateTime)reader[16];

                                if (reader[17].GetType() == typeof(System.DBNull))
                                    cust_param.PriceHundredKilo = -1;
                                else
                                    cust_param.PriceHundredKilo = (int)reader[17];

                                if (reader[18].GetType() == typeof(System.DBNull))
                                    cust_param.CardReleaseNumber = -1;
                                else
                                    cust_param.CardReleaseNumber = (int)reader[18];

                                if (reader[19].GetType() == typeof(System.DBNull))
                                    cust_param.LanguageCode = "";
                                else
                                    cust_param.LanguageCode = (string)reader[19];

                                if (reader[20].GetType() == typeof(System.DBNull))
                                    cust_param.MobilePhone = "";
                                else
                                    cust_param.MobilePhone = (string)reader[20];

                                customers.Add(cust_param);
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
                mutex.ReleaseMutex();
            }

#if TRACE_PERFORMANCE
            LogFile.WriteMessageToLogFile("GetConfiguredContainers took : {0} ms", DateTime.Now.Subtract(tStart));
#endif
            return retval;
        }

        public bool GetCustomerData(string customer_number, int operator_id, ref int customer_id, ref eCARD_TYPE card_type_id, ref string language_code)
        {
            string sqlStatement = "SELECT CUSTOMER_ID, CARD_TYPE_ID, LANGUAGE_CODE FROM CUSTOMER WHERE CUSTOMER_NUMBER=@CustomerNumber AND OPERATOR_ID=@OperatorId";
            bool retval = true;

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter p_cutomerNumber = new SqlParameter("@CustomerNumber", SqlDbType.VarChar);
                        p_cutomerNumber.Value = customer_number;

                        cmd.Parameters.Add(p_cutomerNumber);

                        SqlParameter p_operatorId = new SqlParameter("@OperatorId", SqlDbType.Int);
                        p_operatorId.Value = operator_id;

                        cmd.Parameters.Add(p_operatorId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                customer_id = (int)reader[0];
                                card_type_id = (eCARD_TYPE)reader[1];
                                language_code = (string)reader[2];
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("{0} in \'GetCustomerData\' appeared.", e.Message);
                retval = false;
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool GetCustomerDataEx(string customer_number, int operator_id, ref CustomerParams customer)
        {
            string sqlStatement = "SELECT CUSTOMER_ID, SALUTATION_ID, CARD_TYPE_ID, ADDRESS_ID, CUSTOMER_NUMBER, TITLE, FIRST_NAME, LAST_NAME, COMPANY_NAME, PHONE, EMAIL, LOGIN, PASSWORD, CARD_ID, POSITIVE_CREDIT_BALANCE, LAST_WASTE_DISPOSAL, CARD_DATE, PRICE_HUNDRED_KILO, CARD_EXTENSION, LANGUAGE_CODE, MOBIL_PHONE FROM CUSTOMER WHERE CUSTOMER_NUMBER=@CustomerNumber AND OPERATOR_ID=@OperatorId";
            bool retval = false;

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter p_cutomerNumber = new SqlParameter("@CustomerNumber", SqlDbType.VarChar);
                        p_cutomerNumber.Value = customer_number;

                        cmd.Parameters.Add(p_cutomerNumber);

                        SqlParameter p_operatorId = new SqlParameter("@OperatorId", SqlDbType.Int);
                        p_operatorId.Value = operator_id;

                        cmd.Parameters.Add(p_operatorId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                customer.CustomerId = (int)reader[0];

                                if (reader[1].GetType() == typeof(System.DBNull))
                                    customer.SalutationId = -1;
                                else
                                    customer.SalutationId = (int)reader[1];

                                if (reader[2].GetType() == typeof(System.DBNull))
                                    customer.CardTypeId = -1;
                                else
                                    customer.CardTypeId = (int)reader[2];

                                if (reader[3].GetType() == typeof(System.DBNull))
                                    customer.AddressId = -1;
                                else
                                    customer.AddressId = (int)reader[3];

                                if (reader[4].GetType() == typeof(System.DBNull))
                                    customer.CustomerNumber = "";
                                else
                                    customer.CustomerNumber = (string)reader[4];

                                if (reader[5].GetType() == typeof(System.DBNull))
                                    customer.Title = "";
                                else
                                    customer.Title = (string)reader[5];

                                if (reader[6].GetType() == typeof(System.DBNull))
                                    customer.FirstName = "";
                                else
                                    customer.FirstName = (string)reader[6];

                                if (reader[7].GetType() == typeof(System.DBNull))
                                    customer.LastName = "";
                                else
                                    customer.LastName = (string)reader[7];

                                if (reader[8].GetType() == typeof(System.DBNull))
                                    customer.CompanyName = "";
                                else
                                    customer.CompanyName = (string)reader[8];

                                if (reader[9].GetType() == typeof(System.DBNull))
                                    customer.PhoneNumber = "";
                                else
                                    customer.PhoneNumber = (string)reader[9];

                                if (reader[10].GetType() == typeof(System.DBNull))
                                    customer.Email = "";
                                else
                                    customer.Email = (string)reader[10];

                                if (reader[11].GetType() == typeof(System.DBNull))
                                    customer.LogIn = "";
                                else
                                    customer.LogIn = (string)reader[11];

                                if (reader[12].GetType() == typeof(System.DBNull))
                                    customer.Password = "";
                                else
                                    customer.Password = (string)reader[12];

                                if (reader[13].GetType() == typeof(System.DBNull))
                                    customer.CardId = -1;
                                else
                                    customer.CardId = (int)reader[13];

                                if (reader[14].GetType() == typeof(System.DBNull))
                                    customer.PositiveCreditBalance = 0;
                                else
                                    customer.PositiveCreditBalance = (decimal)reader[14];

                                if (reader[15].GetType() == typeof(System.DBNull))
                                    customer.LastTransaction = new DateTime();
                                else
                                    customer.LastTransaction = (DateTime)reader[15];

                                if (reader[16].GetType() == typeof(System.DBNull))
                                    customer.CardReleaseDate = new DateTime();
                                else
                                    customer.CardReleaseDate = (DateTime)reader[16];

                                if (reader[17].GetType() == typeof(System.DBNull))
                                    customer.PriceHundredKilo = -1;
                                else
                                    customer.PriceHundredKilo = (int)reader[17];

                                if (reader[18].GetType() == typeof(System.DBNull))
                                    customer.CardReleaseNumber = -1;
                                else
                                    customer.CardReleaseNumber = (int)reader[18];

                                if (reader[19].GetType() == typeof(System.DBNull))
                                    customer.LanguageCode = "";
                                else
                                {
                                    customer.LanguageCode = (string)reader[19];

                                    GetLocationLanguageId(customer.LanguageCode, ref customer.LanguageId);
                                }

                                if (reader[20].GetType() == typeof(System.DBNull))
                                    customer.MobilePhone = "";
                                else
                                    customer.MobilePhone = (string)reader[20];

                                retval = true;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                //                LogFile.WriteErrorToLogFile("{0} in \'GetCustomerData\' appeared.", e.Message);
                retval = false;
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool ModifyCustomerData(int operator_id, CustomerParams existing_customer, CustomerParams imported_customer)
        {
            string sqlStatement = "";
            bool retval = false;

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    // check if address has to be changed
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        sqlStatement = "UPDATE ADDRESS SET ";

                        if (imported_customer.Street != "" && imported_customer.Street != existing_customer.Street)
                        {
                            sqlStatement += "STREET= @Street,";

                            SqlParameter param = new SqlParameter("@Street", SqlDbType.VarChar);
                            param.Value = imported_customer.Street;
                            cmd.Parameters.Add(param);
                        }

                        if (imported_customer.ZIPCode != "" && imported_customer.ZIPCode != existing_customer.ZIPCode)
                        {
                            sqlStatement += "ZIP_CODE= @ZipCode,";

                            SqlParameter param = new SqlParameter("@ZipCode", SqlDbType.VarChar);
                            param.Value = imported_customer.ZIPCode;
                            cmd.Parameters.Add(param);
                        }

                        if (imported_customer.City != "" && imported_customer.City != existing_customer.City)
                        {
                            sqlStatement += "CITY= @City,";

                            SqlParameter param = new SqlParameter("@City", SqlDbType.VarChar);
                            param.Value = imported_customer.City;
                            cmd.Parameters.Add(param);
                        }

                        sqlStatement = sqlStatement.TrimEnd(new char[] { ',' });
                        sqlStatement += " WHERE ADDRESS_ID=@AddressId";
                        cmd.CommandText = sqlStatement;

                        if (cmd.Parameters.Count > 0)
                        {
                            SqlParameter p_AddressId = new SqlParameter("@AddressId", SqlDbType.Int);
                            p_AddressId.Value = existing_customer.AddressId;
                            cmd.Parameters.Add(p_AddressId);

                            retval = true;

                            if (cmd.ExecuteNonQuery() != 1)
                            {
                                LogFile.WriteErrorToLogFile("Error while trying to modify customers address");
                            }
                        }
                    }

                    // if payment id has change we have to delete the record first
                    if (existing_customer.PaymentId != -1 && imported_customer.PaymentId != -1 && imported_customer.PaymentId != existing_customer.PaymentId)
                    {
                        sqlStatement = "DELETE PAYMENT_RANGE WHERE CUSTOMER_ID=@CustomerId";

                        using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                        {
                            SqlParameter p_CustomerId = new SqlParameter("@CustomerId", SqlDbType.Int);
                            p_CustomerId.Value = existing_customer.CustomerId;
                            cmd.Parameters.Add(p_CustomerId);

                            if (cmd.ExecuteNonQuery() != 1)
                            {
                                LogFile.WriteErrorToLogFile("Error while trying to delete customers payment data");
                            }
                            else
                            {
                                existing_customer.PaymentId = -1;

                                retval = true;
                            }
                        }
                    }

                    // check if payment data has to be created
                    if (existing_customer.PaymentId == -1)
                    {
                        // store new payment range
                        sqlStatement = "INSERT INTO PAYMENT_RANGE ([PAYMENT_ID], [FROM_AMOUNT], [TO_AMOUNT], [CUSTOMER_ID]) VALUES (@PaymentId, @FromAmount, @ToAmount, @CustomerId)";

                        using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                        {
                            SqlParameter p1 = new SqlParameter("@PaymentId", SqlDbType.Int);
                            p1.Value = imported_customer.PaymentId;
                            cmd.Parameters.Add(p1);

                            SqlParameter p2 = new SqlParameter("@FromAmount", SqlDbType.Int);
                            p2.Value = imported_customer.FromAmount;
                            cmd.Parameters.Add(p2);

                            SqlParameter p3 = new SqlParameter("@ToAmount", SqlDbType.VarChar);
                            p3.Value = imported_customer.ToAmount.ToString();
                            cmd.Parameters.Add(p3);

                            SqlParameter p4 = new SqlParameter("@CustomerId", SqlDbType.Int);
                            p4.Value = existing_customer.CustomerId;
                            cmd.Parameters.Add(p4);

                            // store it
                            if (cmd.ExecuteNonQuery() != 1)
                            {
                                LogFile.WriteErrorToLogFile("Error while trying to create payment data for existing customer");
                            }
                            else
                                retval = true;

                            existing_customer.PaymentId = imported_customer.PaymentId;
                            existing_customer.FromAmount = imported_customer.FromAmount;
                            existing_customer.ToAmount = imported_customer.ToAmount;
                        }
                    }

                    // check if payment data has to be changed
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        sqlStatement = "UPDATE PAYMENT_RANGE SET ";

                        if (imported_customer.FromAmount != -1 && imported_customer.FromAmount != existing_customer.FromAmount)
                        {
                            sqlStatement += "FROM_AMOUNT= @FromAmount,";

                            SqlParameter param = new SqlParameter("@FromAmount", SqlDbType.Int);
                            param.Value = imported_customer.FromAmount;
                            cmd.Parameters.Add(param);
                        }

                        if (imported_customer.ToAmount != -1 && imported_customer.ToAmount != existing_customer.ToAmount)
                        {
                            sqlStatement += "TO_AMOUNT= @ToAmount,";

                            SqlParameter param = new SqlParameter("@ToAmount", SqlDbType.VarChar);
                            param.Value = imported_customer.ToAmount.ToString();
                            cmd.Parameters.Add(param);
                        }

                        sqlStatement = sqlStatement.TrimEnd(new char[] { ',' });
                        sqlStatement += " WHERE CUSTOMER_ID=@CustomerId";
                        cmd.CommandText = sqlStatement;

                        if (cmd.Parameters.Count > 0)
                        {
                            SqlParameter p_CustomerId = new SqlParameter("@CustomerId", SqlDbType.Int);
                            p_CustomerId.Value = existing_customer.CustomerId;
                            cmd.Parameters.Add(p_CustomerId);

                            if (cmd.ExecuteNonQuery() != 1)
                            {
                                LogFile.WriteErrorToLogFile("Error while trying to modify customers payment data");
                            }
                            else
                                retval = true;
                        }
                    }

                    // check if customer data has to be changed
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        sqlStatement = "UPDATE CUSTOMER SET ";

                        if (imported_customer.SalutationId != -1 && imported_customer.SalutationId != existing_customer.SalutationId)
                        {
                            sqlStatement += "SALUTATION_ID= @SalutationID,";

                            SqlParameter param = new SqlParameter("@SalutationId", SqlDbType.Int);
                            param.Value = imported_customer.SalutationId;
                            cmd.Parameters.Add(param);
                        }

                        if (imported_customer.CardTypeId != -1 && imported_customer.CardTypeId != existing_customer.CardTypeId)
                        {
                            sqlStatement += "CARD_TYPE_ID= @CardTypeId,";

                            SqlParameter param = new SqlParameter("@CardTypeId", SqlDbType.Int);
                            param.Value = imported_customer.CardTypeId;
                            cmd.Parameters.Add(param);
                        }

                        if (imported_customer.Title != "" && imported_customer.Title != existing_customer.Title)
                        {
                            sqlStatement += "TITLE= @Title,";

                            SqlParameter param = new SqlParameter("@Title", SqlDbType.VarChar);
                            param.Value = imported_customer.Title;
                            cmd.Parameters.Add(param);
                        }

                        if (imported_customer.FirstName != "" && imported_customer.FirstName != existing_customer.FirstName)
                        {
                            sqlStatement += "FIRST_NAME= @Firstname,";

                            SqlParameter param = new SqlParameter("@Firstname", SqlDbType.VarChar);
                            param.Value = imported_customer.FirstName;
                            cmd.Parameters.Add(param);
                        }

                        if (imported_customer.LastName != "" && imported_customer.LastName != existing_customer.LastName)
                        {
                            sqlStatement += "LAST_NAME= @Lastname,";

                            SqlParameter param = new SqlParameter("@Lastname", SqlDbType.VarChar);
                            param.Value = imported_customer.LastName;
                            cmd.Parameters.Add(param);
                        }

                        if (imported_customer.CompanyName != "" && imported_customer.CompanyName != existing_customer.CompanyName)
                        {
                            sqlStatement += "COMPANY_NAME= @Companyname,";

                            SqlParameter param = new SqlParameter("@Companyname", SqlDbType.VarChar);
                            param.Value = imported_customer.CompanyName;
                            cmd.Parameters.Add(param);
                        }

                        if (imported_customer.PhoneNumber != "" && imported_customer.PhoneNumber != existing_customer.PhoneNumber)
                        {
                            sqlStatement += "PHONE= @Phone,";

                            SqlParameter param = new SqlParameter("@Phone", SqlDbType.VarChar);
                            param.Value = imported_customer.PhoneNumber;
                            cmd.Parameters.Add(param);
                        }

                        if (imported_customer.Email != "" && imported_customer.Email != existing_customer.Email)
                        {
                            sqlStatement += "EMAIL= @Email,";

                            SqlParameter param = new SqlParameter("@Email", SqlDbType.VarChar);
                            param.Value = imported_customer.Email;
                            cmd.Parameters.Add(param);
                        }

                        if (imported_customer.LogIn != "" && imported_customer.LogIn != existing_customer.LogIn)
                        {
                            sqlStatement += "LOGIN= @Login,";

                            SqlParameter param = new SqlParameter("@Login", SqlDbType.VarChar);
                            param.Value = imported_customer.LogIn;
                            cmd.Parameters.Add(param);
                        }

                        if (imported_customer.Password != "" && imported_customer.Password != existing_customer.Password)
                        {
                            sqlStatement += "PASSWORD= @Password,";

                            SqlParameter param = new SqlParameter("@Password", SqlDbType.VarChar);
                            param.Value = imported_customer.Password;
                            cmd.Parameters.Add(param);
                        }

                        if (imported_customer.CardId != -1 && imported_customer.CardId != existing_customer.CardId)
                        {
                            sqlStatement += "CARD_ID= @CardId,";

                            SqlParameter param = new SqlParameter("@CardId", SqlDbType.Int);
                            param.Value = imported_customer.CardId;
                            cmd.Parameters.Add(param);
                        }

                        if (imported_customer.PositiveCreditBalance != -1 && existing_customer.PositiveCreditBalance == -1)
                        {
                            sqlStatement += "POSITIVE_CREDIT_BALANE= @Credit,";

                            SqlParameter param = new SqlParameter("@Credit", SqlDbType.Decimal);
                            param.Value = imported_customer.PositiveCreditBalance;
                            cmd.Parameters.Add(param);
                        }

                        if (imported_customer.LastTransaction != new DateTime() && imported_customer.LastTransaction.Date != existing_customer.LastTransaction.Date)
                        {
                            sqlStatement += "LAST_WASTE_DISPOSAL= @LastWasteDisposal,";

                            SqlParameter param = new SqlParameter("@LastWasteDisposal", SqlDbType.DateTime);
                            param.Value = imported_customer.LastTransaction;
                            cmd.Parameters.Add(param);
                        }

                        if (imported_customer.CardReleaseDate != new DateTime() && imported_customer.CardReleaseDate.Date != existing_customer.CardReleaseDate.Date)
                        {
                            sqlStatement += "CARD_DATE= @CardDate,";

                            SqlParameter param = new SqlParameter("@CardDate", SqlDbType.DateTime);
                            param.Value = imported_customer.CardReleaseDate;
                            cmd.Parameters.Add(param);
                        }

                        if (imported_customer.PriceHundredKilo != -1 && imported_customer.PriceHundredKilo != existing_customer.PriceHundredKilo)
                        {
                            sqlStatement += "PRICE_HUNDRED_KILO= @PriceHundredKilo,";

                            SqlParameter param = new SqlParameter("@PriceHundredKilo", SqlDbType.Int);
                            param.Value = imported_customer.PriceHundredKilo;
                            cmd.Parameters.Add(param);
                        }

                        if (imported_customer.CardReleaseNumber != -1 && imported_customer.CardReleaseNumber != existing_customer.CardReleaseNumber)
                        {
                            sqlStatement += "CARD_EXTENSION= @CardExtension,";

                            SqlParameter param = new SqlParameter("@CardExtension", SqlDbType.Int);
                            param.Value = imported_customer.CardReleaseNumber;
                            cmd.Parameters.Add(param);
                        }

                        if (imported_customer.LanguageId != -1 && imported_customer.LanguageId != existing_customer.LanguageId)
                        {
                            // get location language code
                            string sqlStatement1 = "SELECT LANGUAGE_CODE FROM LOCATION_LANGUAGE WHERE LOCATION_LANGUAGE_ID=@LocationLanguageId";

                            using (SqlCommand cmd1 = new SqlCommand(sqlStatement1, sqlConnection))
                            {
                                SqlParameter p1 = new SqlParameter("@LocationLanguageId", SqlDbType.Int);
                                p1.Value = imported_customer.LanguageId;
                                cmd1.Parameters.Add(p1);

                                using (SqlDataReader reader = cmd1.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        imported_customer.LanguageCode = (string)reader[0];

                                        if (imported_customer.LanguageCode == "GB")
                                            imported_customer.LanguageCode = "EN";
                                    }
                                }
                            }

                            sqlStatement += "LANGUAGE_CODE= @LanguageCode,";

                            SqlParameter param = new SqlParameter("@LanguageCode", SqlDbType.VarChar);
                            param.Value = imported_customer.LanguageCode;
                            cmd.Parameters.Add(param);
                        }

                        if (imported_customer.MobilePhone != "" && imported_customer.MobilePhone != existing_customer.MobilePhone)
                        {
                            sqlStatement += "MOBIL_PHONE= @MobilePhone,";

                            SqlParameter param = new SqlParameter("@MobilePhone", SqlDbType.VarChar);
                            param.Value = imported_customer.MobilePhone;
                            cmd.Parameters.Add(param);
                        }

#if false
                        if (imported_customer.PaymentId != "" && imported_customer.PaymentId != existing_customer.PaymentId)
                        {
                            sqlStatement += "PAYMENT_ID= @PaymentId,";

                            SqlParameter param = new SqlParameter("@Firstname", SqlDbType.Int);
                            param.Value = imported_customer.PaymentId;
                            cmd.Parameters.Add(param);
                        }
#endif
                        sqlStatement = sqlStatement.TrimEnd(new char[] { ',' });
                        sqlStatement += " WHERE CUSTOMER_ID=@CustomerId";
                        cmd.CommandText = sqlStatement;

                        if (cmd.Parameters.Count > 0)
                        {
                            SqlParameter p_customerId = new SqlParameter("@CustomerId", SqlDbType.Int);
                            p_customerId.Value = existing_customer.CustomerId;
                            cmd.Parameters.Add(p_customerId);

                            if (cmd.ExecuteNonQuery() != 1)
                            {
                                LogFile.WriteErrorToLogFile("Error while trying to modify customers data");
                            }
                            else
                                retval = true;
                        }

                        // check location groups
                        if (imported_customer.LocationGroupMask != existing_customer.LocationGroupMask)
                        {
                            ArrayList location_groups_new = new ArrayList();
                            string[] toks = existing_customer.LocationGroup.Split(new char[] { ',' });

                            // remove location groups for existing customer
                            sqlStatement = "DELETE CUSTOMER_LOCATION_GROUP WHERE CUSTOMER_ID=@CustomerId";

                            using (SqlCommand cmd1 = new SqlCommand(sqlStatement, sqlConnection))
                            {
                                SqlParameter p2 = new SqlParameter("@CustomerId", SqlDbType.Int);
                                p2.Value = existing_customer.CustomerId;
                                cmd1.Parameters.Add(p2);

                                if (cmd1.ExecuteNonQuery() < 1)
                                {
                                    LogFile.WriteErrorToLogFile("Could not remove location groups from customer (id={1})", existing_customer.CustomerId);
                                }
                                else
                                    retval = true;
                            }

                            // get new location group ids
                            for (UInt32 i = 0; i < 32; i++)
                            {
                                UInt32 bitmask = (UInt32)(1 << (int)i);

                                if (((UInt32)imported_customer.LocationGroupMask & bitmask) == bitmask)
                                {
                                    // get location group id
                                    sqlStatement = "SELECT LOCATION_GROUP_ID FROM LOCATION_GROUP WHERE OPERATOR_ID=@OperatorId AND OPERATOR_LOCATION_GROUP_BIT=@LocationGroupBit AND ACTIVE = 0";
                                    using (SqlCommand cmd1 = new SqlCommand(sqlStatement, sqlConnection))
                                    {
                                        SqlParameter p1 = new SqlParameter("@OperatorId", SqlDbType.Int);
                                        p1.Value = operator_id;
                                        cmd1.Parameters.Add(p1);

                                        SqlParameter p2 = new SqlParameter("@LocationGroupBit", SqlDbType.Int);
                                        p2.Value = i + 1;
                                        cmd1.Parameters.Add(p2);

                                        using (SqlDataReader reader = cmd1.ExecuteReader())
                                        {
                                            if (reader.Read())
                                            {
                                                int location_group_id = (int)reader[0];

                                                location_groups_new.Add(location_group_id);
                                            }
                                        }
                                    }
                                }
                            }

                            // add location groups to customer loaction group
                            sqlStatement = "INSERT INTO CUSTOMER_LOCATION_GROUP ([CUSTOMER_ID], [LOCATION_GROUP_ID]) VALUES (@CustomerId, @LocationGroupId)";

                            foreach (int location_group_id in location_groups_new)
                            {
                                using (SqlCommand cmd1 = new SqlCommand(sqlStatement, sqlConnection))
                                {
                                    SqlParameter p1 = new SqlParameter("@CustomerId", SqlDbType.Int);
                                    p1.Value = existing_customer.CustomerId;
                                    cmd1.Parameters.Add(p1);

                                    SqlParameter p2 = new SqlParameter("@LocationGroupId", SqlDbType.Int);
                                    p2.Value = location_group_id;
                                    cmd1.Parameters.Add(p2);

                                    // store it
                                    if (cmd1.ExecuteNonQuery() != 1)
                                    {
                                        LogFile.WriteErrorToLogFile("Error while excuting sql statement: {0}", sqlStatement);
                                    }
                                    else
                                        retval = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("Exception: {0} in \'ModifyCustomerData\' appeared.", e.Message);
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool GetCustomerRechargeCredit(int customer_id, ref int new_credit)
        {
            string sqlStatement = "SELECT AMOUNT FROM RECHARGE WHERE CUSTOMER_ID=@CustomerId AND RECHARGED=1";
            bool retval = true;

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter p_customerId = new SqlParameter("@CustomerId", SqlDbType.Int);
                        p_customerId.Value = customer_id;

                        cmd.Parameters.Add(p_customerId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                new_credit += (int)reader[0];
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("{0} in \'GetCustomerRechargeCredit\' appeared.", e.Message);
                retval = false;
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool GetCustomerLocationgroupMask(int customer_id, ref int customer_locationgroup_mask)
        {
            string sqlStatement = "SELECT OPERATOR_LOCATION_GROUP_BIT FROM CUSTOMER_LOCATION_GROUP, LOCATION_GROUP WHERE CUSTOMER_LOCATION_GROUP.LOCATION_GROUP_ID = LOCATION_GROUP.LOCATION_GROUP_ID AND CUSTOMER_LOCATION_GROUP.CUSTOMER_ID=@CustomerId";
            bool retval = true;

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter p_customerId = new SqlParameter("@CustomerId", SqlDbType.Int);
                        p_customerId.Value = customer_id;

                        cmd.Parameters.Add(p_customerId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int operator_location_group_bit_number = (int)reader[0];

                                customer_locationgroup_mask |= (1 << (operator_location_group_bit_number - 1));
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("{0} in \'GetCustomerLocationgroupMask\' appeared.", e.Message);
                retval = false;
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool GetCustomerLocationGroups(ref CustomerParams customer)
        {
            string sqlStatement = "SELECT LOCATION_GROUP FROM CUSTOMER_LOCATION_GROUP, LOCATION_GROUP WHERE CUSTOMER_LOCATION_GROUP.LOCATION_GROUP_ID = LOCATION_GROUP.LOCATION_GROUP_ID AND CUSTOMER_LOCATION_GROUP.CUSTOMER_ID=@CustomerId";
            bool retval = true;

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter p_customerId = new SqlParameter("@CustomerId", SqlDbType.Int);
                        p_customerId.Value = customer.CustomerId;

                        cmd.Parameters.Add(p_customerId);

                        customer.LocationGroup = "";

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                customer.LocationGroup += (string)reader[0] + ",";
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("{0} in \'GetCustomerLocationGroups\' appeared.", e.Message);
                retval = false;
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool GetCustomerMinMaxAmount(int customer_id, ref int payment_id, ref int min_amount, ref int max_amount)
        {
            string sqlStatement = "SELECT PAYMENT_ID, FROM_AMOUNT, TO_AMOUNT FROM PAYMENT_RANGE WHERE CUSTOMER_ID=@CustomerId";
            bool retval = true;

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter p_customerId = new SqlParameter("@CustomerId", SqlDbType.Int);
                        p_customerId.Value = customer_id;

                        cmd.Parameters.Add(p_customerId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (reader[0].GetType() == typeof(System.DBNull))
                                    payment_id = 0;
                                else
                                    payment_id = (int)reader[0];

                                if (reader[1].GetType() != typeof(System.DBNull))
                                    min_amount = (int)reader[1];

                                if (reader[2].GetType() != typeof(System.DBNull))
                                {
                                    max_amount = (int)reader[2];
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("{0} in \'GetCustomerMinMaxAmount\' appeared.", e.Message);
                retval = false;
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool GetCustomerAddress(ref CustomerParams customer)
        {
            string sqlStatement = "SELECT STREET, ZIP_CODE, CITY FROM ADDRESS WHERE ADDRESS_ID=@Address_id";
            bool retval = true;

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter param_addrid = new SqlParameter("@Address_Id", SqlDbType.Int);
                        param_addrid.Value = customer.AddressId;

                        cmd.Parameters.Add(param_addrid);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (reader[0].GetType() != typeof(DBNull))
                                    customer.Street = (string)reader[0];
                                if (reader[1].GetType() != typeof(DBNull))
                                    customer.ZIPCode = (string)reader[1];
                                if (reader[2].GetType() != typeof(DBNull))
                                    customer.City = (string)reader[2];
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("{0} in \'GetCustomerAddress\' appeared.", e.Message);
                retval = false;
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool GetContainerType(int container_type_id, ref string container_type)
        {
            string sqlStatement = "SELECT CONTAINER_TYPE FROM CONTAINER_TYPE WHERE CONTAINER_TYPE_ID=@Container_Type_Id";
            bool retval = true;

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter param_conttypeid = new SqlParameter("@Container_Type_Id", SqlDbType.Int);
                        param_conttypeid.Value = container_type_id;

                        cmd.Parameters.Add(param_conttypeid);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                container_type = (string)reader[0];
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
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool GetLocationParams(int location_id, ref string location, ref DateTime nightlock_start, ref DateTime nightlock_stop,
                ref int gate_limit, ref int container_id, ref int price_hundred_kilo, ref bool bEmptying)
        {
            string sqlStatement = "SELECT LOCATION, NIGHT_LOCK_START, NIGHT_LOCK_STOP, GATE_LIMIT, CONTAINER_ID, PRICE_HUNDRED_KILO, ENTLEERUNG_GEPFLEGT FROM LOCATION WHERE LOCATION_ID=@Location_Id";
            bool retval = true;

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter param_locid = new SqlParameter("@Location_Id", SqlDbType.Int);
                        param_locid.Value = location_id;

                        cmd.Parameters.Add(param_locid);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                location = (string)reader[0];
                                if (reader[1].GetType() == typeof(System.DBNull))
                                    nightlock_start = new DateTime();
                                else
                                    nightlock_start = (DateTime)reader[1];

                                if (reader[2].GetType() == typeof(System.DBNull))
                                    nightlock_stop = new DateTime();
                                else
                                    nightlock_stop = (DateTime)reader[2];

                                if (nightlock_stop < nightlock_start)
                                    nightlock_stop = nightlock_stop.AddDays(1);
                                gate_limit = (int)reader[3];
                                container_id = (int)reader[4];
                                price_hundred_kilo = (int)reader[5];

                                bEmptying = reader["ENTLEERUNG_GEPFLEGT"] as bool? ?? false;
//                                LogFile.WriteMessageToLogFile("Entleerung: {0}", bEmptying ? "ja" : "nein");
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
                LogFile.WriteErrorToLogFile("{0} in \'GetLocationParams\' appeared.", e.Message);
                retval = false;
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool GetLocationLanguageId(string language_code, ref int language_id)
        {
            string sqlStatement = "SELECT LOCATION_LANGUAGE_ID FROM LOCATION_LANGUAGE WHERE LANGUAGE_CODE=@LanguageCode";
            bool retval = true;

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter param_languagecode = new SqlParameter("@LanguageCode", SqlDbType.VarChar);

                        param_languagecode.Value = language_code;

                        cmd.Parameters.Add(param_languagecode);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                language_id = (int)reader[0];
                            }
                            else
                            {
                                LogFile.WriteErrorToLogFile("\'GetLocationLanguageId\': Error: Languagecode: {0} not found", language_code);
                                retval = false;
                            }

                            reader.Close();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("{0} in \'GetLocationLanguageId\' appeared.", e.Message);
                retval = false;
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool GetLocationTimeZone(int location_id, ref string strTimeZone)
        {
            // get timezone id
            string sqlStatement = "SELECT LOCATION_GROUP_ID FROM LOCATION WHERE LOCATION_ID=@LocationId";
            bool retval = false;
            int timezone_id = -1;
            int location_group_id = -1;

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter p1 = new SqlParameter("@LocationId", SqlDbType.Int);
                        p1.Value = location_id;
                        cmd.Parameters.Add(p1);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                location_group_id = (int)reader[0];
                            }
                        }
                    }

                    if (location_group_id == -1)
                    {
                        LogFile.WriteErrorToLogFile("Could not retrieve location group id!");
                        retval = false;
                    }
                    else
                    {
                        sqlStatement = "SELECT TIMEZONE_ID FROM LOCATION_GROUP WHERE LOCATION_GROUP_ID=@LocationGroupId";

                        using (SqlCommand cmd1 = new SqlCommand(sqlStatement, sqlConnection))
                        {
                            SqlParameter p1 = new SqlParameter("@LocationGroupId", SqlDbType.Int);
                            p1.Value = location_group_id;
                            cmd1.Parameters.Add(p1);

                            using (SqlDataReader reader = cmd1.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    string strTimezone_id = (string)reader[0];
                                    timezone_id = int.Parse(strTimezone_id);
                                }
                            }
                        }

                        if (timezone_id == -1)
                        {
                            LogFile.WriteErrorToLogFile("Could not retrieve timezone id!");
                            retval = false;
                        }
                        else
                        {

                            // get time zone name
                            sqlStatement = "SELECT NAME_INTERN FROM TIMEZONE WHERE TIMEZONE_ID=@TimeZoneId";

                            using (SqlCommand cmd2 = new SqlCommand(sqlStatement, sqlConnection))
                            {
                                SqlParameter p2 = new SqlParameter("@TimeZoneId", SqlDbType.Int);
                                p2.Value = timezone_id;
                                cmd2.Parameters.Add(p2);

                                using (SqlDataReader reader1 = cmd2.ExecuteReader())
                                {
                                    if (reader1.Read())
                                    {
                                        strTimeZone = (string)reader1[0];
                                        retval = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
//                LogFile.WriteErrorToLogFile("{0} in \'GetLocationTimeZone\' appeared.", e.Message);
                retval = false;
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool GetLocationLanguages(int location_id, ref int language_code)
        {
            string sqlStatement = "SELECT LOCATION_LANGUAGE_ID FROM LANGUAGE_LOCATION WHERE LOCATION_ID=@LocationId";
            bool retval = true;
            int index = 0;
            int[] language = new int[2];

            language[0] = language[1] = 0;

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter param_locid = new SqlParameter("@LocationId", SqlDbType.Int);
                        param_locid.Value = location_id;

                        cmd.Parameters.Add(param_locid);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                language[index] = (int)reader[0];

                                if (index++ >= 2)
                                {
                                    LogFile.WriteErrorToLogFile("Error to many languages found! sql statement was: {0}", sqlStatement);
                                    break;
                                }
                            }

                            reader.Close();

                            if (language[1] == 0)   // nur eine Sprache ausgewählt
                                language_code = language[0] * 20;
                            else
                                language_code = language[0] * 20 + language[1];

//                            if (language[0] > language[1])
//                                language_code = language[1] * 20 + language[0];
//                            else
//                                language_code = language[0] * 20 + language[1];

                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("{0} in \'GetLocationLanguages\' appeared.", e.Message);
                retval = false;
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool GetOperatorParams(int operator_id, ref string operator_name, ref int currency_id, ref string language_code)
        {
            string sqlStatement = "SELECT OPERATOR_NAME1, CURRENCY_ID, LANGUAGE_CODE FROM OPERATOR WHERE OPERATOR_ID=@Operator_Id";
            bool retval = true;

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter param_opid = new SqlParameter("@Operator_Id", SqlDbType.Int);
                        param_opid.Value = operator_id;

                        cmd.Parameters.Add(param_opid);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                operator_name = (string)reader[0];
                                currency_id = (int)reader[1];
                                language_code = (string)reader[2];
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
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool GetDataExportParams(int operator_id, ref DataExportParams parameters)
        {
            string sqlStatement = "SELECT DATA_EXPORT_FREQUENCY_ID, LAST_DATA_EXPORT, FTPSERVER, FTPUSER, FTPPASSWORD, OPERATOR_NAME1 FROM OPERATOR WHERE OPERATOR_ID=@Operator_Id";
            bool retval = true;

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter param_opid = new SqlParameter("@Operator_Id", SqlDbType.Int);
                        param_opid.Value = operator_id;

                        cmd.Parameters.Add(param_opid);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                parameters.ExportFrequency = (int)reader[0];

                                if (parameters.ExportFrequency == 4)
                                    parameters.ExportFrequency = 7;
                                else if (parameters.ExportFrequency == 5)
                                    parameters.ExportFrequency = 15;
                                else if (parameters.ExportFrequency == 6)
                                    parameters.ExportFrequency = 30;

                                if (reader[1].GetType() == typeof(System.DBNull))
                                    parameters.LastDataExport = new DateTime();
                                else
                                    parameters.LastDataExport = (DateTime)reader[1];

                                if (reader[2].GetType() == typeof(System.DBNull))
                                    parameters.FtpServer = null;
                                else
                                    parameters.FtpServer = (string)reader[2];

                                if (reader[3].GetType() == typeof(System.DBNull))
                                    parameters.FtpUser = null;
                                else
                                    parameters.FtpUser = (string)reader[3];

                                if (reader[4].GetType() == typeof(System.DBNull))
                                    parameters.FtpPassword = null;
                                else
                                    parameters.FtpPassword = (string)reader[4];

                                if (reader[5].GetType() == typeof(System.DBNull))
                                    parameters.Operator = "";
                                else
                                    parameters.Operator = (string)reader[5];
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
                LogFile.WriteErrorToLogFile("{0} in \'GetDataExportParams\' appeared.", e.Message);
                retval = false;
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool UpdateReadWritePointer(int container_id, int read_pointer, int write_pointer)
        {
            bool retval = true;

            try
            {
                // store read/write pointer in database
                String sqlStatement = "UPDATE CONTAINER SET READ_POINTER = @ReadPointer, WRITE_POINTER = @WritePointer WHERE CONTAINER_ID=@ContainerId";

                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter p_readPointer = new SqlParameter("@ReadPointer", SqlDbType.Int);
                        p_readPointer.Value = read_pointer;
                        cmd.Parameters.Add(p_readPointer);

                        SqlParameter p_writePointer = new SqlParameter("@WritePointer", SqlDbType.Int);
                        p_writePointer.Value = write_pointer;
                        cmd.Parameters.Add(p_writePointer);

                        SqlParameter p_containerId = new SqlParameter("@ContainerId", SqlDbType.Int);
                        p_containerId.Value = container_id;
                        cmd.Parameters.Add(p_containerId);

                        if (cmd.ExecuteNonQuery() != 1)
                        {
                            LogFile.WriteErrorToLogFile("ContainerID {0}: Error while trying to store read write pointers", container_id);
                            retval = false;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("ContainerID {0}: {1} in \'StoreReadPointer\' appeared.", container_id, e.Message);
                retval = false;
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool UpdateCustomerLastWasteDisposal(int customer_id, DateTime date)
        {
            string sqlStatement = "UPDATE CUSTOMER SET LAST_WASTE_DISPOSAL = @DateLastDisposal WHERE CUSTOMER_ID=@CustomerId";
            bool retval = true;

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter p_date = new SqlParameter("@DateLastDisposal", SqlDbType.DateTime);
                        p_date.Value = date;
                        cmd.Parameters.Add(p_date);

                        SqlParameter p_customerId = new SqlParameter("@CustomerId", SqlDbType.Int);
                        p_customerId.Value = customer_id;
                        cmd.Parameters.Add(p_customerId);

                        if (cmd.ExecuteNonQuery() != 1)
                        {
                            LogFile.WriteErrorToLogFile("Error while trying to store customers (id={0}), last waste disposal time stamp {1}", customer_id, date);
                            retval = false;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("Exception: {0} in \'UpdateCustomerLastWasteDisposal\' appeared.", e.Message);
                retval = false;
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool UpdateLastDataExport(int operator_id, DateTime date)
        {
            string sqlStatement = "UPDATE OPERATOR SET LAST_DATA_EXPORT = @DateLastExport WHERE OPERATOR_ID=@OperatorId";
            bool retval = true;

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter p_date = new SqlParameter("@DateLastExport", SqlDbType.DateTime);
                        p_date.Value = date;
                        cmd.Parameters.Add(p_date);

                        SqlParameter p_operatorId = new SqlParameter("@OperatorId", SqlDbType.Int);
                        p_operatorId.Value = operator_id;
                        cmd.Parameters.Add(p_operatorId);

                        if (cmd.ExecuteNonQuery() != 1)
                        {
                            LogFile.WriteErrorToLogFile("Error while trying to store operators (id={0}), last data export time stamp {1}", operator_id, date);
                            retval = false;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("Exception: {0} in \'UpdateLastDataExport\' appeared.", e.Message);
                retval = false;
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool GetContainerLastCommunication(int container_id, ref DateTime date)
        {
            string sqlStatement = "SELECT LAST_COMMUNICATION FROM CONTAINER WHERE CONTAINER_ID=@ContainerId";
            bool retval = true;

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter p_containerId = new SqlParameter("@ContainerId", SqlDbType.Int);
                        p_containerId.Value = container_id;
                        cmd.Parameters.Add(p_containerId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                date = (DateTime)reader[0];
                            }
                            else
                            {
                                LogFile.WriteErrorToLogFile("Error while trying to read container's last communication Time");
                                retval = false;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("Exception: {0} in \'GetContainerLastCommunication\' appeared.", e.Message);
                retval = false;
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool UpdateContainerLastCommunication(int container_id, DateTime date)
        {
            string sqlStatement = "UPDATE CONTAINER SET LAST_COMMUNICATION = @LastCommunication WHERE CONTAINER_ID=@ContainerId";
            bool retval = true;

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter p_lastCommunication = new SqlParameter("@LastCommunication", SqlDbType.DateTime);
                        p_lastCommunication.Value = date;
                        cmd.Parameters.Add(p_lastCommunication);

                        SqlParameter p_containerId = new SqlParameter("@ContainerId", SqlDbType.Int);
                        p_containerId.Value = container_id;
                        cmd.Parameters.Add(p_containerId);

                        if (cmd.ExecuteNonQuery() != 1)
                        {
                            LogFile.WriteErrorToLogFile("Error while trying to store last communication date for container (id={0})", container_id);
                            retval = false;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("Exception: {0} in \'UpdateContainerLastCommunication\' appeared.", e.Message);
                retval = false;
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool UpdateLocationContainerId(int location_id, int container_id)
        {
            string sqlStatement = "UPDATE LOCATION SET CONTAINER_ID = @ContainerId WHERE LOCATION_ID=@LocationId";
            bool retval = true;

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter p_containerId = new SqlParameter("@ContainerId", SqlDbType.Int);
                        p_containerId.Value = container_id;
                        cmd.Parameters.Add(p_containerId);

                        SqlParameter p_locationId = new SqlParameter("@LocationId", SqlDbType.Int);
                        p_locationId.Value = location_id;
                        cmd.Parameters.Add(p_locationId);

                        if (cmd.ExecuteNonQuery() != 1)
                        {
                            LogFile.WriteErrorToLogFile("Error while trying to update container id ({0}) at location ({1})", container_id, location_id);
                            retval = false;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("Exception: {0} in \'UpdateLocationContainerId\' appeared.", e.Message);
                retval = false;
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool UpdateCustomersPositiveCreditBalance(int customer_id, decimal positive_credit_balance)
        {
            string sqlStatement = "UPDATE CUSTOMER SET POSITIVE_CREDIT_BALANCE = @PositiveCreditBalance WHERE CUSTOMER_ID=@CustomerId";
            bool retval = true;

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter p_posCredBalance = new SqlParameter("@PositiveCreditBalance", SqlDbType.Decimal);
                        p_posCredBalance.Value = positive_credit_balance;
                        cmd.Parameters.Add(p_posCredBalance);

                        SqlParameter p_customerId = new SqlParameter("@CustomerId", SqlDbType.Int);
                        p_customerId.Value = customer_id;
                        cmd.Parameters.Add(p_customerId);

                        if (cmd.ExecuteNonQuery() != 1)
                        {
                            LogFile.WriteErrorToLogFile("Error while trying to store customers (id={0}) positive credit balance", customer_id);
                            retval = false;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("Exception: {0} in \'UpdateCustomersPositiveCreditBalance\' appeared.", e.Message);
                retval = false;
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool StoreContainerStatus(int container_id, int location_id, string gsm_number, int code, DateTime date)
        {
            bool retval = true;

            try
            {
                int status_group_id = (code < 1000) ? code : (code / 1000) * 1000;
                int last_container_status_id = 0;

                // first get max container status id
                string sqlStatement = "SELECT MAX(CONTAINER_STATUS_ID) AS MAX_CONTAINERSTATUS_ID FROM CONTAINER_STATUS";

                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                last_container_status_id = (int)reader[0];
                            }
                        }
                    }

                    sqlStatement = "INSERT INTO CONTAINER_STATUS ([CONTAINER_STATUS_ID], [LOCATION_ID], [CONTAINER_ID], [STATUS_GROUP_ID], [STATUS_MESSAGE_ID], [DATE], [GSM_NUMBER]) VALUES (@ContainerStatusId, @LocationId, @ContainerId, @StatusGroupId, @StatusMessageId, @Date, @GSMNumber)";

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter p_containerStatusId = new SqlParameter("@ContainerStatusId", SqlDbType.Int);
                        p_containerStatusId.Value = ++last_container_status_id;
                        cmd.Parameters.Add(p_containerStatusId);

                        SqlParameter p_locationId = new SqlParameter("@LocationId", SqlDbType.Int);
                        p_locationId.Value = location_id;
                        cmd.Parameters.Add(p_locationId);

                        SqlParameter p_containerId = new SqlParameter("@ContainerId", SqlDbType.Int);
                        p_containerId.Value = container_id;
                        cmd.Parameters.Add(p_containerId);

                        SqlParameter p_statusGroupId = new SqlParameter("@StatusGroupId", SqlDbType.Int);
                        p_statusGroupId.Value = status_group_id;
                        cmd.Parameters.Add(p_statusGroupId);

                        SqlParameter p_statusMessageId = new SqlParameter("@StatusMessageId", SqlDbType.Int);
                        p_statusMessageId.Value = code;
                        cmd.Parameters.Add(p_statusMessageId);

                        SqlParameter p_date = new SqlParameter("@Date", SqlDbType.DateTime);
                        p_date.Value = date;
                        cmd.Parameters.Add(p_date);

                        SqlParameter p_gsmNumber = new SqlParameter("@GSMNumber", SqlDbType.VarChar);
                        p_gsmNumber.Value = gsm_number;
                        cmd.Parameters.Add(p_gsmNumber);

                        // store it
                        if (cmd.ExecuteNonQuery() != 1)
                        {
                            LogFile.WriteErrorToLogFile("ContainerID {0}: Error while excuting sql statement: {1}", container_id, sqlStatement);
                            retval = false;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("ContainerID {0}: Exception: {1} while trying to store container status item.", container_id, e.Message);
                retval = false;
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool StoreRechargedFlag(int container_id, Customer customer)
        {
            int numberOfFields = 0;
            bool retval = true;

            try
            {
                string sqlStatement = "UPDATE RECHARGE SET RECHARGED=@Recharged WHERE CUSTOMER_ID=@CustomerId";

                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter p_customerId = new SqlParameter("@CustomerId", SqlDbType.Int);
                        p_customerId.Value = customer._customer_id;

                        cmd.Parameters.Add(p_customerId);

                        SqlParameter p_recharged = new SqlParameter("@Recharged", SqlDbType.Bit);
                        p_recharged.Value = false;

                        cmd.Parameters.Add(p_recharged);

                        if ((numberOfFields = cmd.ExecuteNonQuery()) == 0)
                        {
                            LogFile.WriteErrorToLogFile("ContainerID {0}: Store Recharged Flag: No fields where found to change", container_id);
                            retval = false;
                        }
                        else
                            LogFile.WriteMessageToLogFile("ContainerID {0}: Store Recharged Flag: {1} fields where changed", container_id, numberOfFields);

                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("ContainerID {0}: Exception: {1} in \'StoreRechargedFlag\' appeared.", container_id, e.Message);
                retval = false;
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool StoreTransaction(int container_id, Transaction transaction, int last_trans_id, string gsm_number)
        {   
            bool retval = true;

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    // first get max transaction id
                    string sqlStatement = "SELECT MAX(TRANSACTION_ID) AS MAX_TRANSACTION_ID FROM TRANSACTIONS";

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                last_trans_id = (int)reader[0];
                            }
                        }
                    }

                    // store transactions in database
                    sqlStatement = "INSERT INTO TRANSACTIONS ([TRANSACTION_ID], [CUSTOMER_ID], [TRANSACTION_STATUS_ID], [LOCATION_ID], [DATE], [WEIGHT], [DURATION], [AMOUNT], [CONTAINER_ID], [GSM_NUMBER], [POSITIVE_CREDIT_BALANCE]) VALUES (@TransactionId, @CustomerId, @TransactionStatusId, @LocationId, @Date, @Weight, @Duration, @Amount, @ContainerId, @GSMNumber, @PositiveCreditBalance)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter p_transId = new SqlParameter("@TransactionId", SqlDbType.Int);
                        p_transId.Value = ++last_trans_id;
                        cmd.Parameters.Add(p_transId);

                        SqlParameter p_customerId = new SqlParameter("@CustomerId", SqlDbType.Int);
                        p_customerId.Value = transaction._customer_id;
                        cmd.Parameters.Add(p_customerId);

                        SqlParameter p_transactionStatusId = new SqlParameter("@TransactionStatusId", SqlDbType.Int);
                        p_transactionStatusId.Value = transaction._transaction_status_id;
                        cmd.Parameters.Add(p_transactionStatusId);

                        SqlParameter p_locationId = new SqlParameter("@LocationId", SqlDbType.Int);
                        p_locationId.Value = transaction._location_id;
                        cmd.Parameters.Add(p_locationId);

                        SqlParameter p_date = new SqlParameter("@Date", SqlDbType.DateTime);
                        p_date.Value = transaction._date;
                        cmd.Parameters.Add(p_date);

                        SqlParameter p_weight = new SqlParameter("@Weight", SqlDbType.Int);
                        p_weight.Value = transaction._weight;
                        cmd.Parameters.Add(p_weight);

                        SqlParameter p_duration = new SqlParameter("@Duration", SqlDbType.Int);
                        p_duration.Value = transaction._duration;
                        cmd.Parameters.Add(p_duration);

                        SqlParameter p_amount = new SqlParameter("@Amount", SqlDbType.Int);
                        p_amount.Value = transaction._amount;
                        cmd.Parameters.Add(p_amount);

                        SqlParameter p_containerId = new SqlParameter("@ContainerId", SqlDbType.Int);
                        p_containerId.Value = container_id;
                        cmd.Parameters.Add(p_containerId);

                        SqlParameter p_gsmNumber = new SqlParameter("@GSMNumber", SqlDbType.VarChar);
                        p_gsmNumber.Value = gsm_number;
                        cmd.Parameters.Add(p_gsmNumber);

                        SqlParameter p_positiveCreditBalance = new SqlParameter("@PositiveCreditBalance", SqlDbType.Int);
                        p_positiveCreditBalance.Value = transaction._positive_credit_balance;
                        cmd.Parameters.Add(p_positiveCreditBalance);

                        // store it
                        if (cmd.ExecuteNonQuery() != 1)
                        {
                            LogFile.WriteErrorToLogFile("Error while excuting sql statement: {0}", sqlStatement);
                            retval = false;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("Exception: {0} in \'StoreTransaction\' appeared.", e.Message);
                retval = false;
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool GetMaxTransactionId(ref int last_transaction_id)
        {
            // first get max transaction id
            string sqlStatement = "SELECT MAX(TRANSACTION_ID) AS MAX_TRANSACTION_ID FROM TRANSACTIONS";
            bool retval = true;

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                last_transaction_id = (int)reader[0];
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("Exception: {0} in \'GetMaxTransactionId\' appeared.", e.Message);
                retval = false;
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool GetMessageText(int language_keyword, string language_code, ref string text)
        {
            // first get max transaction id
            string sqlStatement = "SELECT SHORT_TEXT FROM TRANSLATION WHERE KEYWORD_ID=@Keyword_Id AND LANGUAGE_CODE=@Language_code";
            bool retval = true;

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter param_keyword = new SqlParameter("@Keyword_Id", SqlDbType.Int);
                        param_keyword.Value = language_keyword;

                        cmd.Parameters.Add(param_keyword);

                        SqlParameter param_langcode = new SqlParameter("@Language_code", SqlDbType.VarChar);
                        param_langcode.Value = language_code;

                        cmd.Parameters.Add(param_langcode);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                text = (string)reader[0];
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("Exception: {0} in \'GetMessageText\' appeared.", e.Message);
                retval = false;
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool GetAlertingUsers(int location_id, ref int number_of_users, ref AlertingUser[] users)
        {
            string sqlStatement = "SELECT REMOTE_CONTROL.REMOTE_CONTROL_ID, REMOTE_CONTROL_TYPE_ID, MEMO, CONTACT FROM REMOTE_CONTROL INNER JOIN ERROR ON REMOTE_CONTROL.REMOTE_CONTROL_ID=ERROR.REMOTE_CONTROL_ID WHERE ERROR.LOCATION_ID=@LocationId";

            bool retval = true;
            int i = 0;

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
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
                                users[i].remote_control_id = (int)reader[0];
                                users[i].remote_control_type = (int)reader[1];
                                if (reader[2].GetType() == typeof(System.DBNull))
                                    users[i].memo = "";
                                else
                                    users[i].memo = (string)reader[2];
                                users[i++].contact = (string)reader[3];
                            }
                        }
                    }
                }
                number_of_users = i;
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("{0} in \'GetAlertingUsers\' appeared.", e.Message);
                retval = false;
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool GetFullContainerUsers(int location_id, ref int number_of_users, ref AlertingUser[] users)
        {
            string sqlStatement = "SELECT REMOTE_CONTROL.REMOTE_CONTROL_ID, REMOTE_CONTROL_TYPE_ID, MEMO, CONTACT FROM REMOTE_CONTROL INNER JOIN FULL_CONTAINER ON REMOTE_CONTROL.REMOTE_CONTROL_ID=FULL_CONTAINER.REMOTE_CONTROL_ID WHERE FULL_CONTAINER.LOCATION_ID=@LocationId";

            bool retval = true;
            int i = 0;

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
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
                                users[i].remote_control_id = (int)reader[0];
                                users[i].remote_control_type = (int)reader[1];
                                users[i].memo = (string)reader[2];
                                users[i++].contact = (string)reader[3];
                            }
                        }
                    }
                }

                number_of_users = i;
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("{0} in \'GetFullContainerUsers\' appeared.", e.Message);
                retval = false;
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool CalculateContainerWeight(int container_id, int location_id, ref float weight)
        {
            string sqlStatement = "SELECT TOP 1 DATE FROM CONTAINER_STATUS WHERE LOCATION_ID=@LocationId AND CONTAINER_ID=@ContainerId AND STATUS_GROUP_ID=10 ORDER BY DATE DESC";
            bool retval = true;
            DateTime lastEmptying = new DateTime();

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter p_locationId = new SqlParameter("@LocationId", SqlDbType.Int);
                        p_locationId.Value = location_id;
                        cmd.Parameters.Add(p_locationId);

                        SqlParameter p_containerId = new SqlParameter("@ContainerId", SqlDbType.Int);
                        p_containerId.Value = container_id;
                        cmd.Parameters.Add(p_containerId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lastEmptying = (DateTime)reader[0];
                                LogFile.WriteMessageToLogFile("CalculateActualWeight: Last emptying date found: {0}", lastEmptying.ToLongDateString());
                            }
                            else
                                LogFile.WriteMessageToLogFile("CalculateActualWeight: No emptying date found");

                            // if no emptying event since the last 5 months the query would takt to much time!
                            if (lastEmptying < DateTime.Now.Subtract(new TimeSpan(150, 0, 0, 0, 0)))
                            {
                                LogFile.WriteMessageToLogFile("CalculateActualWeight: Emptyingdate to far in the past!");
                                weight = 0.0f;
                                return true;
                            }
                        }
                    }

                    using (SqlCommand cmd1 = new SqlCommand("SELECT SUM(TRANSACTIONS.WEIGHT) FROM TRANSACTIONS WHERE CONTAINER_ID=@ContainerId AND LOCATION_ID=@LocationId AND TRANSACTIONS.DATE > @LastEmptying", sqlConnection))
                    {
                        SqlParameter p_locationId = new SqlParameter("@LocationId", SqlDbType.Int);
                        p_locationId.Value = location_id;
                        cmd1.Parameters.Add(p_locationId);

                        SqlParameter p_containerId = new SqlParameter("@ContainerId", SqlDbType.Int);
                        p_containerId.Value = container_id;
                        cmd1.Parameters.Add(p_containerId);

                        SqlParameter p_lastEmptying = new SqlParameter("@LastEmptying", SqlDbType.DateTime);
                        p_lastEmptying.Value = lastEmptying;
                        cmd1.Parameters.Add(p_lastEmptying);

                        using (SqlDataReader reader = cmd1.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int w = (int)reader[0];
                                weight = Convert.ToSingle(w) / 10.0f;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("Exception: {0} in \'CalculateContainerWeight\' appeared.", e.Message);
                retval = false;
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool IncrementContainerTransactionCounter(int container_id)
        {
            string sqlStatement = "SELECT TRANSACTION_COUNT FROM CONTAINER WHERE CONTAINER_ID=@ContainerId";
            bool retval = true;
            int transaction_counter = 0;

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter p_containerId = new SqlParameter("@ContainerId", SqlDbType.Int);
                        p_containerId.Value = container_id;
                        cmd.Parameters.Add(p_containerId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                transaction_counter = (int)reader[0];
                            }
                            else
                            {
                                LogFile.WriteErrorToLogFile("ContainerID {0}: IncrementTransactionCounter, Cannot access field TRANSACTION_COUNT", container_id);
                            }
                        }
                    }

                    using (SqlCommand cmd_update = new SqlCommand("UPDATE CONTAINER SET TRANSACTION_COUNT=@TransactionCount WHERE CONTAINER_ID=@ContainerId", sqlConnection))
                    {
                        SqlParameter p_trans_count = new SqlParameter("@TransactionCount", SqlDbType.Int);
                        p_trans_count.Value = ++transaction_counter;

                        SqlParameter p_contID = new SqlParameter("@ContainerId", SqlDbType.Int);
                        p_contID.Value = container_id;

                        cmd_update.Parameters.Add(p_trans_count);
                        cmd_update.Parameters.Add(p_contID);

                        if (cmd_update.ExecuteNonQuery() != 1)
                        {
                            LogFile.WriteErrorToLogFile("ContainerID {0}: Error while trying to update transaction counter", container_id);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("Exception: {0} in \'GetMaxTransactionId\' appeared.", e.Message);
                retval = false;
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        #endregion
    }
}
