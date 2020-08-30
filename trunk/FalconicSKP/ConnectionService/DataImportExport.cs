using ConnectionService;
using Luthien;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;

namespace SKP.CustomerImportExport
{
    [Serializable]
    public class DataImportExport : MarshalByRefObject
    {
        #region members

        ConnectionControl _controller;
        DataExportParams _params = new DataExportParams();
        ArrayList _customers = null;
        DateTime _lastFtpCheckTime = new DateTime();
        private int _operator_id;
        private string _working_path = "c:\\tmp";
        private string _export_format = "";
        private AutoResetEvent _stop_event = new AutoResetEvent(false);
        private int _added_customers = 0;
        private int _modified_customers = 0;
        private Encoding _encoding = Encoding.UTF8;

        #endregion

        #region properties

        public Encoding Encoding
        {
            get { return _encoding; }
            set { _encoding = value; }
        }

        #endregion

        #region constructor

        public DataImportExport(ConnectionControl controller, int operator_id, string export_format)
        {
            _controller = controller;
            _operator_id = operator_id;
            _export_format = export_format;
        }

        #endregion

        #region private methods

        private bool do_data_export()
        {
            bool retval = false;
            int index = 0;

            _customers = new ArrayList();
 
            try
            {
                string file_name_csv;
                string file_name = file_name_csv = String.Format("{0}\\{1}\\Export", _working_path, _operator_id);

                if (!Directory.Exists(file_name))
                    Directory.CreateDirectory(file_name);

                file_name += "\\customers.xml";
                file_name_csv += "\\customers.csv";
 
                LogFile.WriteMessageToLogFile("{0}: Begin customer data export, retrieving operators customers ...", _params.Operator);

                // get all customer data of specified operator
                _controller.DataAccess.GetOperatorsCustomers(_operator_id, ref _customers);

                LogFile.WriteMessageToLogFile("{0}: Retrieving address, language, amount and location group informations ...", _params.Operator);

                for (int i = 0; i < _customers.Count; i++)
                {
                    CustomerParams customer = (CustomerParams)_customers[i];

                    // get address information, location language, min max amount and location group information
                    _controller.DataAccess.GetCustomerAddress(ref customer);
                    if (customer.LanguageCode != "")
                        _controller.DataAccess.GetLocationLanguageId(customer.LanguageCode, ref customer.LanguageId);

                    _controller.DataAccess.GetCustomerMinMaxAmount(customer.CustomerId, ref customer.PaymentId, ref customer.FromAmount, ref customer.ToAmount);
                    _controller.DataAccess.GetCustomerLocationGroups(ref customer);
                    _controller.DataAccess.GetCustomerLocationgroupMask(customer.CustomerId, ref customer.LocationGroupMask);

                    if (_stop_event.WaitOne(1, false))
                        return false;
                }

                LogFile.WriteMessageToLogFile("{0}: Writing informations to xml file ...", _params.Operator);

                // write customer data to xml and csv file
                StreamWriter txtWriter = new StreamWriter(file_name_csv, false, _encoding); 
                XmlTextWriter writer = new XmlTextWriter(file_name, null);

                string startLine = "CUSTOMER_NUMBER;SALUTATION_ID;TITLE;FIRST_NAME;LAST_NAME;LOGIN;PASSWORD;CARD_ID;CARD_RELEASE_NUMBER;CARD_TYPE_ID;PAYMENT_ID;FROM_AMOUNT;TO_AMOUNT;POSITIVE_CREDIT_BALANCE;CARD_RELEASE_DATE;LAST_TRANSACTION;COMPANY_NAME;STREET;ZIP_CODE;CITY;PHONE;MOBILE_PHONE;EMAIL;LANGUAGE_ID;PRICE_HUNDRED_KILO;LOCATION_CODE";

                txtWriter.WriteLine(startLine);

                // Use automatic indentation for readability.
                writer.Formatting = Formatting.Indented;
                writer.WriteStartDocument();

                writer.WriteStartElement("table");
                writer.WriteAttributeString("name", "customer");

                // write structure element
                writer.WriteStartElement("structure");

                // Customer number
                writer.WriteStartElement("column");
                writer.WriteAttributeString("name", "CUSTOMER_NUMBER");
                writer.WriteAttributeString("description", "Kundennummer");
                writer.WriteEndElement();

                // Salutation
                writer.WriteStartElement("column");
                writer.WriteAttributeString("name", "SALUTATION_ID");
                writer.WriteAttributeString("description", "Anrede");
                writer.WriteEndElement();

                // Title
                writer.WriteStartElement("column");
                writer.WriteAttributeString("name", "TITLE");
                writer.WriteAttributeString("description", "Titel");
                writer.WriteEndElement();

                // First name
                writer.WriteStartElement("column");
                writer.WriteAttributeString("name", "FIRST_NAME");
                writer.WriteAttributeString("description", "Vorname");
                writer.WriteEndElement();

                // Last name
                writer.WriteStartElement("column");
                writer.WriteAttributeString("name", "LAST_NAME");
                writer.WriteAttributeString("description", "Nachname");
                writer.WriteEndElement();

                // Login
                writer.WriteStartElement("column");
                writer.WriteAttributeString("name", "LOGIN");
                writer.WriteAttributeString("description", "Login");
                writer.WriteEndElement();

                // Password
                writer.WriteStartElement("column");
                writer.WriteAttributeString("name", "PASSWORD");
                writer.WriteAttributeString("description", "Passwort");
                writer.WriteEndElement();

                // CardID
                writer.WriteStartElement("column");
                writer.WriteAttributeString("name", "CARD_ID");
                writer.WriteAttributeString("description", "Karten ID");
                writer.WriteEndElement();

                // Card release number
                writer.WriteStartElement("column");
                writer.WriteAttributeString("name", "CARD_RELEASE_NUMBER");
                writer.WriteAttributeString("description", "Kartenausgabenummer");
                writer.WriteEndElement();

                // Card type
                writer.WriteStartElement("column");
                writer.WriteAttributeString("name", "CARD_TYPE_ID");
                writer.WriteAttributeString("description", "Karten type");
                writer.WriteEndElement();


                // Payment type
                writer.WriteStartElement("column");
                writer.WriteAttributeString("name", "PAYMENT_ID");
                writer.WriteAttributeString("description", "Zahlungsart");
                writer.WriteEndElement();

                // Min amount
                writer.WriteStartElement("column");
                writer.WriteAttributeString("name", "FROM_AMOUNT");
                writer.WriteAttributeString("description", "Minimale Zahlung");
                writer.WriteEndElement();

                // Max amount
                writer.WriteStartElement("column");
                writer.WriteAttributeString("name", "TO_AMOUNT");
                writer.WriteAttributeString("description", "Maximale Zahlung");
                writer.WriteEndElement();

                // Positive credit balance
                writer.WriteStartElement("column");
                writer.WriteAttributeString("name", "POSITIVE_CREDIT_BALANCE");
                writer.WriteAttributeString("description", "Guthaben");
                writer.WriteEndElement();

                // Card release date
                writer.WriteStartElement("column");
                writer.WriteAttributeString("name", "CARD_RELEASE_DATE");
                writer.WriteAttributeString("description", "Kartenausgabedatum");
                writer.WriteEndElement();

                // Last transaction
                writer.WriteStartElement("column");
                writer.WriteAttributeString("name", "LAST_TRANSACTION");
                writer.WriteAttributeString("description", "Letzte Transaktion");
                writer.WriteEndElement();

                // Company
                writer.WriteStartElement("column");
                writer.WriteAttributeString("name", "COMPANY");
                writer.WriteAttributeString("description", "Firma");
                writer.WriteEndElement();

                // Street
                writer.WriteStartElement("column");
                writer.WriteAttributeString("name", "STREET");
                writer.WriteAttributeString("description", "Strasse");
                writer.WriteEndElement();

                // ZIP code
                writer.WriteStartElement("column");
                writer.WriteAttributeString("name", "ZIP_CODE");
                writer.WriteAttributeString("description", "PLZ");
                writer.WriteEndElement();

                // City
                writer.WriteStartElement("column");
                writer.WriteAttributeString("name", "CITY");
                writer.WriteAttributeString("description", "Stadt");
                writer.WriteEndElement();

                // Phone
                writer.WriteStartElement("column");
                writer.WriteAttributeString("name", "PHONE");
                writer.WriteAttributeString("description", "Telefon");
                writer.WriteEndElement();

                // Mobile phone
                writer.WriteStartElement("column");
                writer.WriteAttributeString("name", "MOBILE_PHONE");
                writer.WriteAttributeString("description", "Mobiltelefon");
                writer.WriteEndElement();

                // Email
                writer.WriteStartElement("column");
                writer.WriteAttributeString("name", "EMAIL");
                writer.WriteAttributeString("description", "Email");
                writer.WriteEndElement();

                // Language ID
                writer.WriteStartElement("column");
                writer.WriteAttributeString("name", "LANGUAGE_ID");
                writer.WriteAttributeString("description", "Sprach-ID");
                writer.WriteEndElement();

                // Price per hundred kilo
                writer.WriteStartElement("column");
                writer.WriteAttributeString("name", "PRICE_HUNDRED_KILO");
                writer.WriteAttributeString("description", "Preis pro hundert Kilogramm");
                writer.WriteEndElement();

                // Location group
//                writer.WriteStartElement("column");
//                writer.WriteAttributeString("name", "LOCATION_GROUP");
//                writer.WriteAttributeString("description", "Standortgruppe");
//                writer.WriteEndElement();

                // Location groupmask
                writer.WriteStartElement("column");
                writer.WriteAttributeString("name", "LOCATION_CODE");
                writer.WriteAttributeString("description", "Standortgruppenmaske");
                writer.WriteEndElement();

                writer.WriteEndElement();

                // write the data element
                writer.WriteStartElement("data");

                foreach (CustomerParams customer in _customers)
                {
                    // start a row element
                    writer.WriteStartElement("row");
                    writer.WriteAttributeString("number", String.Format("{0}", ++index));
                    
                    // write the customer's data
                    writer.WriteElementString("CUSTOMER_NUMBER", customer.CustomerNumber);
                    
                    if (customer.SalutationId == -1)
                        writer.WriteElementString("SALUTATION_ID", "");
                    else
                        writer.WriteElementString("SALUTATION_ID", customer.SalutationId.ToString());

                    writer.WriteElementString("TITLE", customer.Title); 
                    writer.WriteElementString("FIRST_NAME", customer.FirstName);
                    writer.WriteElementString("LAST_NAME", customer.LastName);
                    writer.WriteElementString("LOGIN", customer.LogIn);
                    writer.WriteElementString("PASSWORD", customer.Password);
                    
                    if(customer.CardId == -1)
                        writer.WriteElementString("CARD_ID", "");
                    else
                        writer.WriteElementString("CARD_ID", customer.CardId.ToString());

                    if (customer.CardReleaseNumber == -1)
                        writer.WriteElementString("CARD_RELEASE_NUMBER", "");
                    else
                        writer.WriteElementString("CARD_RELEASE_NUMBER", customer.CardReleaseNumber.ToString());

                    if (customer.CardTypeId == -1)
                        writer.WriteElementString("CARD_TYPE_ID", "");
                    else
                        writer.WriteElementString("CARD_TYPE_ID", customer.CardTypeId.ToString());

                    if (customer.PaymentId == -1)
                        writer.WriteElementString("PAYMENT_ID", "");
                    else
                        writer.WriteElementString("PAYMENT_ID", customer.PaymentId.ToString());

                    if (customer.FromAmount == -1)
                        writer.WriteElementString("FROM_AMOUNT", "");
                    else
                        writer.WriteElementString("FROM_AMOUNT", customer.FromAmount.ToString());

                    if (customer.ToAmount == -1)
                        writer.WriteElementString("TO_AMOUNT", "");
                    else
                        writer.WriteElementString("TO_AMOUNT", customer.ToAmount.ToString());

                    if (customer.PositiveCreditBalance == -1)
                        writer.WriteElementString("POSITIVE_CREDIT_BALANCE", "");
                    else
                        writer.WriteElementString("POSITIVE_CREDIT_BALANCE", customer.PositiveCreditBalance.ToString());


                    if (customer.CardReleaseDate < DateTime.Parse("01.01.1900"))
                        writer.WriteElementString("CARD_RELEASE_DATE", "");
                    else
                        writer.WriteElementString("CARD_RELEASE_DATE", customer.CardReleaseDate.ToShortDateString());

                    if (customer.LastTransaction < DateTime.Parse("01.01.1900"))
                        writer.WriteElementString("LAST_TRANSACTION", "");
                    else    
                        writer.WriteElementString("LAST_TRANSACTION", customer.LastTransaction.ToShortDateString());

                    writer.WriteElementString("COMPANY_NAME", customer.CompanyName);
                    writer.WriteElementString("STREET", customer.Street);
                    writer.WriteElementString("ZIP_CODE", customer.ZIPCode);
                    writer.WriteElementString("CITY", customer.City);
                    writer.WriteElementString("PHONE", customer.PhoneNumber);
                    writer.WriteElementString("MOBILE_PHONE", customer.MobilePhone);
                    writer.WriteElementString("EMAIL", customer.Email);

                    if (customer.LanguageId == -1)
                        writer.WriteElementString("LANGUAGE_ID", "");
                    else
                        writer.WriteElementString("LANGUAGE_ID", customer.LanguageId.ToString());

                    if (customer.PriceHundredKilo == -1)
                        writer.WriteElementString("PRICE_HUNDRED_KILO", "");
                    else
                        writer.WriteElementString("PRICE_HUNDRED_KILO", customer.PriceHundredKilo.ToString());

//                    writer.WriteElementString("LOCATION_GROUP", customer.LocationGroup);

                    if (customer.LocationGroupMask == 0)
                        writer.WriteElementString("LOCATION_CODE", "");
                    else
                        writer.WriteElementString("LOCATION_CODE", customer.LocationGroupMask.ToString());

                    // end the row element
                    writer.WriteEndElement();

                    // write to csv file
                    string txtLine = "";
                    string val = "";


                    txtLine += customer.CustomerNumber.ToString() + ";";
                    if (customer.SalutationId == -1)
                        val = "";
                    else
                        val = customer.SalutationId.ToString();
                    txtLine += val + ";";

                    txtLine += customer.Title + ";";
                    txtLine += customer.FirstName + ";";
                    txtLine += customer.LastName + ";";
                    txtLine += customer.LogIn + ";";
                    txtLine += customer.Password + ";";

                    if (customer.CardId == -1)
                        val = "";
                    else
                        val = customer.CardId.ToString();
                    txtLine += val + ";";

                    if (customer.CardReleaseNumber == -1)
                        val = "";
                    else
                        val = customer.CardReleaseNumber.ToString();
                    txtLine += val + ";";

                    if (customer.CardTypeId == -1)
                        val = "";
                    else
                        val = customer.CardTypeId.ToString();
                    txtLine += val + ";";

                    if (customer.PaymentId == -1)
                        val = "";
                    else
                        val = customer.PaymentId.ToString();
                    txtLine += val + ";";

                    if (customer.FromAmount == -1)
                        val = "";
                    else
                        val = customer.FromAmount.ToString();
                    txtLine += val + ";";

                    if (customer.ToAmount == -1)
                        val = "";
                    else
                        val = customer.ToAmount.ToString();
                    txtLine += val + ";";

                    if (customer.PositiveCreditBalance == -1)
                        val = "";
                    else
                        val = customer.PositiveCreditBalance.ToString();
                    txtLine += val + ";";

                    if (customer.CardReleaseDate < DateTime.Parse("01.01.1900"))
                        val = "";
                    else
                        val = customer.CardReleaseDate.ToShortDateString();
                    txtLine += val + ";";

                    if (customer.LastTransaction < DateTime.Parse("01.01.1900"))
                        val = "";
                    else
                        val = customer.LastTransaction.ToShortDateString();
                    txtLine += val + ";";

                    txtLine += customer.CompanyName + ";";
                    txtLine += customer.Street + ";";
                    txtLine += customer.ZIPCode + ";";
                    txtLine += customer.City + ";";
                    txtLine += customer.PhoneNumber + ";";
                    txtLine += customer.MobilePhone + ";";
                    txtLine += customer.Email + ";";

                    if (customer.LanguageId == -1)
                        val = "";
                    else
                        val = customer.LanguageId.ToString();
                    txtLine += val + ";";

                    if (customer.PriceHundredKilo == -1)
                        val = "";
                    else
                        val = customer.PriceHundredKilo.ToString();
                    txtLine += val + ";";

//                    txtLine += customer.LocationGroup + ";";
                    if (customer.LocationGroupMask == 0)
                        val = "";
                    else 
                        val = customer.LocationGroupMask.ToString();
                    txtLine += val + ";";
#if old
                    string txtLine = String.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15};{16};{17};{18};{19};{20};{21};{22};{23};{24};{25}",
                        customer.CustomerNumber, customer.SalutationId, customer.Title, customer.FirstName,
                        customer.LastName, customer.LogIn, customer.Password, customer.CardId,
                        customer.CardReleaseNumber, customer.CardTypeId, customer.PaymentId, customer.FromAmount,
                        customer.ToAmount, customer.PositiveCreditBalance, customer.CardReleaseDate, customer.LastTransaction,
                        customer.CompanyName, customer.Street, customer.ZIPCode, customer.City,
                        customer.PhoneNumber, customer.MobilePhone, customer.Email, customer.LanguageId,
                        customer.PriceHundredKilo, customer.LocationGroup);
#endif
                    txtWriter.WriteLine(txtLine);

                    if (_stop_event.WaitOne(1, false))
                        return false;
                }

                // end the data element
                writer.WriteEndElement();

                // end the table element
                writer.WriteEndElement();
                writer.Close();

                txtWriter.Close();
                txtWriter.Dispose();

                LogFile.WriteMessageToLogFile("{0}: Uploading customer data file to ftp server: {1}", _params.Operator, _params.FtpServer);

                if (_export_format.ToLower().IndexOf(".xml") != -1)
                {
                    // upload xml export file to ftp server
                    FileInfo fi = new FileInfo(file_name);
                    string file_name_server = String.Format("{0}/Export/{1}", _operator_id, fi.Name);

                    if (ftp_upload_file(file_name, file_name_server, _params.FtpServer, _params.FtpUser, _params.FtpPassword))
                    {
                        retval = true;
                    }
                }

                if (_export_format.ToLower().IndexOf(".csv") != -1)
                {
                    FileInfo fi = new FileInfo(file_name_csv);
                    string file_name_server = String.Format("{0}/Export/{1}", _operator_id, fi.Name);
                    // upload csv export file to ftp server
                    if (ftp_upload_file(file_name_csv, file_name_server, _params.FtpServer, _params.FtpUser, _params.FtpPassword))
                    {
                        retval = true;
                    }
                }
            }
            catch (Exception excp)
            {
                LogFile.WriteErrorToLogFile("{0}: Exception {1} while trying to export data!", _params.Operator, excp.Message);
            }

            return retval;
        }


        private bool ftp_check_directoy(string dir, string ftp_server, string username, string password)
        {
            bool retval = false;

            try
            {
                string uri = string.Format("ftp://{0}/{1}", ftp_server, dir);

                // create the ftp request
                WebRequest request = FtpWebRequest.Create(uri);
                request.Credentials = new NetworkCredential(username, password);

                // state that we want to check if directory exists
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Timeout = 10000;
                ((FtpWebRequest)request).KeepAlive = false;
                ((FtpWebRequest)request).EnableSsl = true;

                ServicePointManager.ServerCertificateValidationCallback =
                          (s, certificate, chain, sslPolicyErrors) => true;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                if (response.StatusCode == FtpStatusCode.DataAlreadyOpen)
                {
                    retval = true;
                }

                response.Close();

            }
            catch (Exception excp)
            {
                // print error only if not 'no such directory' exception
                if (excp.Message.IndexOf("550") == -1)
                    LogFile.WriteErrorToLogFile("{0}: Exception {1} while trying to check directory!", _params.Operator, excp.Message);
            }

            return retval;
        }

        private bool ftp_make_directory(string dir, string ftp_server, string username, string password)
        {
            bool retval = false;

            try
            {
                string uri = string.Format("ftp://{0}/{1}", ftp_server, dir);

                // create the ftp request
                WebRequest request = FtpWebRequest.Create(uri);
                request.Credentials = new NetworkCredential(username, password);

                // state that we want to check if directory exists
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                request.Timeout = 10000;
                ((FtpWebRequest)request).KeepAlive = false;
                ((FtpWebRequest)request).EnableSsl = true;

                ServicePointManager.ServerCertificateValidationCallback =
                          (s, certificate, chain, sslPolicyErrors) => true;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                if (response.StatusCode == FtpStatusCode.PathnameCreated)
                {
                    LogFile.WriteMessageToLogFile("{0}: Ftp Directory: {1} successfully created", _params.Operator, dir);
                    retval = true;
                }

                response.Close();

            }
            catch (Exception excp)
            {
                LogFile.WriteErrorToLogFile("{0}: Exception {1} while trying to check directory!", _params.Operator, excp.Message);
            }

            return retval;
        }

        private bool ftp_upload_file(string local_file_name, string server_file_name, string ftp_server, string username, string password)
        {
            bool retval = false;

            try
            {                
                string uri = string.Format("ftp://{0}/{1}", ftp_server, server_file_name);

                // create the ftp request
                WebRequest request = FtpWebRequest.Create(uri);
                request.Credentials = new NetworkCredential(username, password);

                // state that we uploading the file
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Timeout = 600000;
                ((FtpWebRequest)request).KeepAlive = false;
                ((FtpWebRequest)request).EnableSsl = true;

                ServicePointManager.ServerCertificateValidationCallback =
                          (s, certificate, chain, sslPolicyErrors) => true;

                using (Stream ftpStream = request.GetRequestStream())
                using (Stream fileStream = new System.IO.FileStream(local_file_name, System.IO.FileMode.Open))
                {
                    fileStream.CopyTo(ftpStream);
                }

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                if (response.StatusCode == FtpStatusCode.ClosingData)
                {
                    retval = true;
                    LogFile.WriteMessageToLogFile("{0}: Ftp upload of file: {1} was ok.", _params.Operator, server_file_name);
                }
                else
                    LogFile.WriteErrorToLogFile("{0}: Ftp upload error. Status: {1}", _params.Operator, response.StatusDescription);

                response.Close();

            }
            catch (Exception excp)
            {
                LogFile.WriteErrorToLogFile("{0}: Exception {1} while trying to upload file!", _params.Operator, excp.Message);
            }

            return retval;
        }

        private bool modify_or_create_customer(CustomerParams imported_customer)
        {
            bool retval = false;

            try
            {
                CustomerParams existing_customer = new CustomerParams();

                if (_controller.DataAccess.GetCustomerDataEx(imported_customer.CustomerNumber, _operator_id, ref existing_customer))
                {
                    // get additional data for existing customer
                    _controller.DataAccess.GetCustomerAddress(ref existing_customer);
                    _controller.DataAccess.GetLocationLanguageId(existing_customer.LanguageCode, ref existing_customer.LanguageId);
                    _controller.DataAccess.GetCustomerMinMaxAmount(existing_customer.CustomerId, ref existing_customer.PaymentId, ref existing_customer.FromAmount, ref existing_customer.ToAmount);
                    _controller.DataAccess.GetCustomerLocationGroups(ref existing_customer);
                    _controller.DataAccess.GetCustomerLocationgroupMask(existing_customer.CustomerId, ref existing_customer.LocationGroupMask);


                    // check for default values
                    if (imported_customer.SalutationId == -1 && existing_customer.SalutationId == -1)
                        imported_customer.SalutationId = 1;

                    if (imported_customer.FirstName == "" && existing_customer.FirstName == "")
                        imported_customer.FirstName = "Max";

                    if (imported_customer.LastName == "" && existing_customer.LastName == "")
                        imported_customer.LastName = "Muster";

                    if (imported_customer.LogIn == "" && existing_customer.LogIn == "")
                        imported_customer.LogIn = string.Format("{0}-{1}", _operator_id, existing_customer.CustomerNumber);

                    if (imported_customer.Password == "" && existing_customer.Password == "")
                    {
                        Random rnd = new Random();
                        imported_customer.Password = string.Format("{0}-{1:D5}", _operator_id, rnd.Next(0, 99999));
                    }

                    if (imported_customer.CardTypeId == -1 && existing_customer.CardTypeId == -1)
                        imported_customer.CardTypeId = 2;

                    if (imported_customer.PaymentId == -1 && existing_customer.PaymentId == -1)
                        imported_customer.PaymentId = 1;

                    if (existing_customer.CardTypeId == 1 && imported_customer.FromAmount == -1 && existing_customer.FromAmount == -1)
                        imported_customer.FromAmount = 5;

                    if (existing_customer.CardTypeId == 1 && imported_customer.ToAmount == -1 && existing_customer.ToAmount == -1)
                        imported_customer.ToAmount = 100;

                    if (imported_customer.Street == "" && existing_customer.Street == "")
                        imported_customer.Street = "Industriestrasse 30";

                    if (imported_customer.ZIPCode == "" && existing_customer.ZIPCode == "")
                        imported_customer.ZIPCode = "4710";

                    if (imported_customer.City == "" && existing_customer.City == "")
                        imported_customer.City = "Grieskirchen";

                    if (imported_customer.LanguageId == -1 && existing_customer.LanguageId == -1)
                        imported_customer.LanguageId = 2;

                    if (imported_customer.PriceHundredKilo == -1 && existing_customer.PriceHundredKilo == -1)
                        imported_customer.PriceHundredKilo = 0;

                    if ((imported_customer.CardId != -1 && existing_customer.CardId != imported_customer.CardId) ||
                        (imported_customer.CardTypeId != -1 && existing_customer.CardTypeId != imported_customer.CardTypeId) ||
                        (imported_customer.City != "" && existing_customer.City != imported_customer.City) ||
                        (imported_customer.CompanyName != "" && existing_customer.CompanyName != imported_customer.CompanyName) ||
                        (imported_customer.Title != "" && existing_customer.Title != imported_customer.Title) ||
                        (imported_customer.FirstName != "" && existing_customer.FirstName != imported_customer.FirstName) ||
                        (imported_customer.FromAmount != -1 && existing_customer.FromAmount != imported_customer.FromAmount) ||
                        (imported_customer.LastName != "" && existing_customer.LastName != imported_customer.LastName) ||
//                        (imported_customer.LocationGroup != "" && existing_customer.LocationGroup != imported_customer.LocationGroup) ||
                        (existing_customer.LocationGroupMask != imported_customer.LocationGroupMask) ||
                        (imported_customer.LanguageId != -1 && existing_customer.LanguageId != imported_customer.LanguageId) ||
                        (imported_customer.LogIn != "" && existing_customer.LogIn != imported_customer.LogIn) ||
                        (imported_customer.Password != "" && existing_customer.Password != imported_customer.Password) ||
                        (imported_customer.PaymentId != -1 && existing_customer.PaymentId != imported_customer.PaymentId) ||
                        (imported_customer.PriceHundredKilo != -1 && existing_customer.PriceHundredKilo != imported_customer.PriceHundredKilo) ||
                        (imported_customer.SalutationId != -1 && existing_customer.SalutationId != imported_customer.SalutationId) ||
                        (imported_customer.Street != "" && existing_customer.Street != imported_customer.Street) ||
                        (imported_customer.ToAmount != -1 && existing_customer.ToAmount != imported_customer.ToAmount) ||
                        (imported_customer.ZIPCode != "" && existing_customer.ZIPCode != imported_customer.ZIPCode) ||
                        (imported_customer.CardReleaseDate != null && existing_customer.CardReleaseDate.Date != imported_customer.CardReleaseDate.Date) ||
                        (imported_customer.PositiveCreditBalance != -1 && imported_customer.PositiveCreditBalance != existing_customer.PositiveCreditBalance))
                    {
                        if ((retval = _controller.DataAccess.ModifyCustomerData(_operator_id, existing_customer, imported_customer)) == true)
                        {
                            _modified_customers++;
                            retval = true;
                        }
                    }
                }
                else
                {
                    // check for default values
                    if (imported_customer.SalutationId == -1)
                        imported_customer.SalutationId = 1;

                    if (imported_customer.FirstName == "")
                        imported_customer.FirstName = "Max";

                    if (imported_customer.LastName == "")
                        imported_customer.LastName = "Muster";

                    if (imported_customer.LogIn == "")
                        imported_customer.LogIn = string.Format("{0}-{1}", _operator_id, imported_customer.CustomerNumber);

                    if (imported_customer.Password == "")
                    {
                        Random rnd = new Random();
                        imported_customer.Password = string.Format("{0}-{1:D5}", _operator_id, rnd.Next(0, 99999));
                    }

                    if (imported_customer.LastTransaction == new DateTime())
                        imported_customer.LastTransaction = DateTime.Parse("01.01.1900");

                    if (imported_customer.CardReleaseDate == new DateTime())
                        imported_customer.CardReleaseDate = DateTime.Now;

                    if (imported_customer.CardTypeId == -1)
                        imported_customer.CardTypeId = 2;

                    if (imported_customer.PaymentId == -1)
                        imported_customer.PaymentId = 1;

                    if (imported_customer.FromAmount == -1)
                        imported_customer.FromAmount = 5;

                    if (imported_customer.ToAmount == -1)
                        imported_customer.ToAmount = 100;

                    if (imported_customer.Street == "")
                        imported_customer.Street = "Industriestrasse 30";

                    if (imported_customer.ZIPCode == "")
                        imported_customer.ZIPCode = "4710";

                    if (imported_customer.City == "")
                        imported_customer.City = "Grieskirchen";

                    if (imported_customer.LanguageId == -1)
                        imported_customer.LanguageId = 2;

                    if (imported_customer.PriceHundredKilo == -1)
                        imported_customer.PriceHundredKilo = 0;

                    if ((retval = _controller.DataAccess.AddNewCustomer(_operator_id, imported_customer)) == true)
                        _added_customers++;
                }
            }
            catch (Exception excp)
            {
                LogFile.WriteErrorToLogFile("{0}: Exception {1} while trying to import ", _params.Operator, excp.Message);
                retval = false;
            }

            return retval;
        }

        private bool import_new_customers_from_csv(string file_name)
        {
            bool retval = false;

            StreamReader reader = null;
            CustomerParams customer = null;
            string line = "";
            string headline = "";
            string[] struct_toks;
            string[] line_toks;

            int i = 0;

            try
            {
                // Load the reader with the customer data file
                reader = new StreamReader(file_name);

                // get structure line
                headline = reader.ReadLine();

                struct_toks = headline.Split(new char[] { ';' });

                // Parse the file 
                while ((line = reader.ReadLine()) != null)
                {
                    line_toks = line.Split(new char[] { ';' });
                    customer = new CustomerParams();

                    for (i = 0; i < struct_toks.GetLength(0); i++)
                    {
                        if (struct_toks[i] == "CUSTOMER_NUMBER")
                            customer.CustomerNumber = line_toks[i];
                        else if (struct_toks[i] == "SALUTATION_ID")
                        {
                            if (line_toks[i] != null)
                                customer.SalutationId = Convert.ToInt32(line_toks[i]);
                        }
                        else if (struct_toks[i] == "TITLE")
                            customer.Title = line_toks[i];
                        else if (struct_toks[i] == "FIRST_NAME")
                            customer.FirstName = line_toks[i];
                        else if (struct_toks[i] == "LAST_NAME")
                            customer.LastName = line_toks[i];
                        else if (struct_toks[i] == "LOGIN")
                            customer.LogIn = line_toks[i];
                        else if (struct_toks[i] == "PASSWORD")
                            customer.Password = line_toks[i];
                        else if (struct_toks[i] == "CARD_ID")
                        {
                            if (line_toks[i] != "")
                                customer.CardId = Convert.ToInt32(line_toks[i]);
                        }
                        else if (struct_toks[i] == "CARD_RELEASE_NUMBER")
                        {
                            if (line_toks[i] != "")
                                customer.CardReleaseNumber = Convert.ToInt32(line_toks[i]);
                        }
                        else if (struct_toks[i] == "CARD_TYPE_ID")
                        {
                            if (line_toks[i] != "")
                                customer.CardTypeId = Convert.ToInt32(line_toks[i]);
                        }
                        else if (struct_toks[i] == "PAYMENT_ID")
                        {
                            if (line_toks[i] != "")
                                customer.PaymentId = Convert.ToInt32(line_toks[i]);
                        }
                        else if (struct_toks[i] == "FROM_AMOUNT")
                        {
                            if (line_toks[i] != "")
                                customer.FromAmount = Convert.ToInt32(line_toks[i]);
                        }
                        else if (struct_toks[i] == "TO_AMOUNT")
                        {
                            if (line_toks[i] != "")
                                customer.ToAmount = Convert.ToInt32(line_toks[i]);
                        }
                        else if (struct_toks[i] == "POSITIVE_CREDIT_BALANCE")
                        {
                            if (line_toks[i] != "")
                                customer.PositiveCreditBalance = Convert.ToDecimal(line_toks[i]);
                        }
                        else if (struct_toks[i] == "CARD_RELEASE_DATE")
                        {
                            if (line_toks[i] != "")
                                customer.CardReleaseDate = Convert.ToDateTime(line_toks[i]);
                        }
                        else if (struct_toks[i] == "LAST_TRANSACTION")
                        {
                            if (line_toks[i] != "")
                                customer.LastTransaction = Convert.ToDateTime(line_toks[i]);
                        }
                        else if (struct_toks[i].IndexOf("COMPANY") != -1)
                            customer.CompanyName = line_toks[i];
                        else if (struct_toks[i] == "STREET")
                            customer.Street = line_toks[i];
                        else if (struct_toks[i] == "ZIP_CODE")
                            customer.ZIPCode = line_toks[i];
                        else if (struct_toks[i] == "CITY")
                            customer.City = line_toks[i];
                        else if (struct_toks[i] == "PHONE")
                            customer.PhoneNumber = line_toks[i];
                        else if (struct_toks[i] == "MOBILE_PHONE")
                            customer.MobilePhone = line_toks[i];
                        else if (struct_toks[i] == "EMAIL")
                            customer.Email = line_toks[i];
                        else if (struct_toks[i] == "LANGUAGE_ID")
                        {
                            if (line_toks[i] != "")
                            {
                                customer.LanguageId = Convert.ToInt32(line_toks[i]);

                            }
                        }
                        else if (struct_toks[i] == "PRICE_HUNDRED_KILO")
                        {
                            if (line_toks[i] != "")
                                customer.PriceHundredKilo = Convert.ToInt32(line_toks[i]);
                        }
                        else if (struct_toks[i] == "LOCATION_GROUP")
                            customer.LocationGroup = line_toks[i];
                        else if (struct_toks[i] == "LOCATION_CODE")
                        {
                            if (line_toks[i] != "")
                                customer.LocationGroupMask = Convert.ToInt32(line_toks[i]);
                            else
                                customer.LocationGroupMask = 0;
                        }
                    }

                    modify_or_create_customer(customer);

                    if (_stop_event.WaitOne(1, false))
                        return false;
                }
            }
            catch (Exception excp)
            {
                LogFile.WriteErrorToLogFile("Exception {1} while trying to import customer data from csv file", _params.Operator, excp.Message);
                LogFile.WriteMessageToLogFile("Index was: {0}", i);
                LogFile.WriteMessageToLogFile("Headline was: {0}", headline);
                LogFile.WriteMessageToLogFile("line was: {0}", line);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return retval;
        }

        private bool import_new_customers_from_xml(string file_name)
        {
            bool retval = false;

            XmlTextReader reader = null;
            CustomerParams customer = null;
            string element_name = "";

            try
            {
                // Load the reader with the customer data file and ignore all white space nodes.         
                reader = new XmlTextReader(file_name);
                reader.WhitespaceHandling = WhitespaceHandling.None;

                // Parse the file and display each of the nodes.
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (reader.Name == "row")
                            {
                                customer = new CustomerParams();
                            }
                            else
                                element_name = reader.Name;
                            break;

                        case XmlNodeType.Text:
                            if (element_name == "CUSTOMER_NUMBER")
                                customer.CustomerNumber = reader.Value;
                            else if (element_name == "SALUTATION_ID")
                                customer.SalutationId = Convert.ToInt32(reader.Value);
                            else if (element_name == "TITLE")
                                customer.Title = reader.Value;
                            else if (element_name == "FIRST_NAME")
                                customer.FirstName = reader.Value;
                            else if (element_name == "LAST_NAME")
                                customer.LastName = reader.Value;
                            else if (element_name == "LOGIN")
                                customer.LogIn = reader.Value;
                            else if (element_name == "PASSWORD")
                                customer.Password = reader.Value;
                            else if (element_name == "CARD_ID")
                                customer.CardId = Convert.ToInt32(reader.Value);
                            else if (element_name == "CARD_RELEASE_NUMBER")
                                customer.CardReleaseNumber = Convert.ToInt32(reader.Value);
                            else if (element_name == "CARD_TYPE_ID")
                                customer.CardTypeId = Convert.ToInt32(reader.Value);
                            else if (element_name == "PAYMENT_ID")
                                customer.PaymentId = Convert.ToInt32(reader.Value);
                            else if (element_name == "FROM_AMOUNT")
                                customer.FromAmount = Convert.ToInt32(reader.Value);
                            else if (element_name == "TO_AMOUNT")
                                customer.ToAmount = Convert.ToInt32(reader.Value);
                            else if (element_name == "POSITIVE_CREDIT_BALANCE")
                                customer.PositiveCreditBalance = Convert.ToDecimal(reader.Value);
                            else if (element_name == "CARD_RELEASE_DATE")
                                customer.CardReleaseDate = Convert.ToDateTime(reader.Value);
                            else if (element_name == "LAST_TRANSACTION")
                                customer.LastTransaction = Convert.ToDateTime(reader.Value);
                            else if (element_name == "COMPANY_NAME")
                                customer.CompanyName = reader.Value;
                            else if (element_name == "STREET")
                                customer.Street = reader.Value;
                            else if (element_name == "ZIP_CODE")
                                customer.ZIPCode = reader.Value;
                            else if (element_name == "CITY")
                                customer.City = reader.Value;
                            else if (element_name == "PHONE")
                                customer.PhoneNumber = reader.Value;
                            else if (element_name == "MOBILE_PHONE")
                                customer.MobilePhone = reader.Value;
                            else if (element_name == "EMAIL")
                                customer.Email = reader.Value;
                            else if (element_name == "LANGUAGE_ID")
                                customer.LanguageId = Convert.ToInt32(reader.Value);
                            else if (element_name == "PRICE_HUNDRED_KILO")
                                customer.PriceHundredKilo = Convert.ToInt32(reader.Value);
                            else if (element_name == "LOCATION_GROUP")
                                customer.LocationGroup = reader.Value;
                            else if (element_name == "LOCATION_CODE")
                                customer.LocationGroupMask = Convert.ToInt32(reader.Value);
                            break;
                        case XmlNodeType.EndElement:
                            if (reader.Name == "row")
                            {
                                modify_or_create_customer(customer);
                                
                                if (_stop_event.WaitOne(1, false))
                                    return false;
                            }
                            break;
                    }
                }
            }
            catch (Exception excp)
            {
                LogFile.WriteErrorToLogFile("Exception {1} while trying to import customer data from xml file", _params.Operator, excp.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return retval;
        }

        private bool ftp_download_file_if_present()
        {
            bool retval = false;
            ArrayList directory = new ArrayList();

            try
            {
//                string uri = string.Format("ftp://{0}/Ents/{1}/Import/", _params.FtpServer, _operator_id); 
                string uri = string.Format("ftp://{0}/{1}/Export/", _params.FtpServer, _operator_id);  // Brixen copy import data to export folder as well :-o
                string path_name = String.Format("{0}\\{1}\\Import", _working_path, _operator_id);

                if (!Directory.Exists(path_name))
                    Directory.CreateDirectory(path_name);

                try
                {
                    // create the ftp request
                    FtpWebRequest request = FtpWebRequest.Create(uri) as FtpWebRequest;
                    request.Credentials = new NetworkCredential("skp", "RlNz2M5xv4dSW1PfnZvT");

                    request.Method = WebRequestMethods.Ftp.ListDirectory;
                    request.KeepAlive = false;
                    request.EnableSsl = true;

                    ServicePointManager.ServerCertificateValidationCallback =
                              (s, certificate, chain, sslPolicyErrors) => true;

                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                    Stream responseStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);

                    while (!reader.EndOfStream)
                    {
                        string filename = reader.ReadLine();

//                       if (filename.IndexOf(".xml") != -1 || filename.IndexOf(".csv") != -1)
                        if (filename.IndexOf("siu2poettinger.csv") != -1)
                        {
                            directory.Add(filename);
                            LogFile.WriteMessageToLogFile("{0}: Found file {1} on ftp server.", _params.Operator, filename);
                        }
                    }
                    response.Close();
                    responseStream.Close();
                    reader.Close();
                }
                catch (Exception excp)
                {
                    LogFile.WriteErrorToLogFile("{0}: Exception {1} while trying list directory: {2} on ftp server", _params.Operator, excp.Message, uri);
                }

                for (int i = 0; i < directory.Count; i++)
                {
                    string uri_file = string.Format("ftp://{0}/{1}/Export/{2}", _params.FtpServer, _operator_id, (string)directory[i]);
                    string output_filename = String.Format("{0}\\{1}", path_name, (string)directory[i]);

                    _added_customers = _modified_customers = 0;

                    LogFile.WriteMessageToLogFile("{0}: Found import file on ftp server: {1}. Download it to: {2} ...", _params.Operator, _params.FtpServer, output_filename);

                    try
                    {
                        FtpWebRequest request1 = FtpWebRequest.Create(new Uri(uri_file)) as FtpWebRequest;
                        request1.Credentials = new NetworkCredential("skp", "RlNz2M5xv4dSW1PfnZvT");
                        request1.EnableSsl = true;

                        ServicePointManager.ServerCertificateValidationCallback =
                                  (s, certificate, chain, sslPolicyErrors) => true;

                        request1.Method = WebRequestMethods.Ftp.DownloadFile;

                        using (Stream ftpStream = request1.GetResponse().GetResponseStream())
                        using (Stream fileStream = File.Create(output_filename))
                        {
                            ftpStream.CopyTo(fileStream);
                        }
                    }
                    catch (Exception excp)
                    {
                        LogFile.WriteErrorToLogFile("{0}: Exception {1} occured while trying to import data from file: {2}!", _params.Operator, excp.Message, uri_file);
                        return false;
                    }

                    // process file
                    try
                    {
                        LogFile.WriteMessageToLogFile("{0}: Process new import file ...", _params.Operator);

                        if (Path.GetExtension(output_filename).ToLower() == ".xml")
                            import_new_customers_from_xml(output_filename);
                        else if (Path.GetExtension(output_filename).ToLower() == ".csv")
                            import_new_customers_from_csv(output_filename);
                        else
                            LogFile.WriteErrorToLogFile("{0}: Error wrong file extension: {1} for customer data import file: ({2})", _params.Operator, Path.GetExtension(output_filename).ToLower(), output_filename);
                    }
                    catch (Exception excp)
                    {
                        LogFile.WriteErrorToLogFile("{0}: Exception {1} occured while trying to import data from file: {2}!", _params.Operator, excp.Message, uri_file);
                        return false;
                    }

                    LogFile.WriteMessageToLogFile("{0}: Customers added: {1}, modified: {2}!", _params.Operator, _added_customers, _modified_customers);

                    try
                    {
                        LogFile.WriteMessageToLogFile("{0}: Delete file {1} on ftp server: {2}", _params.Operator, uri_file, _params.FtpServer);

                        // delete file on ftp server
                        FtpWebRequest request2 = FtpWebRequest.Create(uri_file) as FtpWebRequest;
                        request2.Method = WebRequestMethods.Ftp.DeleteFile;
                        request2.Credentials = new NetworkCredential("skp", "RlNz2M5xv4dSW1PfnZvT");
                        request2.UsePassive = true;
                        request2.UseBinary = true;
                        request2.KeepAlive = false; //close the connection when done
                        request2.EnableSsl = true;

                        FtpWebResponse response1 = (FtpWebResponse)request2.GetResponse();

                        // save file in backup directory
                        CultureInfo cuInfo = new CultureInfo("de-DE");
                        string dateString = DateTime.Now.ToString("d", cuInfo);

                        string server_file_name = String.Format("Sicherung/{0}_{1}_{2}_{3}_{4}", _operator_id, dateString,
                            DateTime.Now.Hour, DateTime.Now.Minute, directory[i]);

                        // upload with advanced ftp user
                        if (ftp_upload_file(output_filename, server_file_name, _params.FtpServer, "skp", "RlNz2M5xv4dSW1PfnZvT"))
                        {
                            LogFile.WriteMessageToLogFile("{0}: Delete file: {1}.", _params.Operator, output_filename);

                            File.Delete(output_filename);
                            LogFile.WriteMessageToLogFile("{0}: Data import ended successfully!", _params.Operator);
                        }
                        else
                            LogFile.WriteErrorToLogFile("{0}: Error while trying to upload file: {1} to ftp-server: {2}", _params.Operator, server_file_name, _params.FtpServer);
                    }
                    catch (Exception excp)
                    {
                        LogFile.WriteErrorToLogFile("{0}: Exception {1} occured while trying to delete file on server!", _params.Operator, excp.Message);
                        return false;
                    }
                }

            }
            catch (Exception excp)
            {
                LogFile.WriteErrorToLogFile("{0}: Exception {1} while trying to download file!", _params.Operator, excp.Message);
            }

            return retval;
        }

        #endregion

        #region public methods

        /// <summary>
        /// Do the import export module's job
        /// </summary>
        public void DoWork()
        {
            bool bTest = false; 

            try
            {
                // first get data export params of operator
                _controller.DataAccess.GetDataExportParams(_operator_id, ref _params);

                // override ftp server since database field is to short for the new name
                _params.FtpServer = "poettinger-wip-ftp.westeurope.cloudapp.azure.com";

                LogFile.WriteMessageToLogFile("{0}: data import export service started", _params.Operator, _params.Operator);
                LogFile.WriteMessageToLogFile("{0}: parameters are: last export date: {1}, export interval: {2}", _params.Operator, _params.LastDataExport, _params.ExportFrequency);
                if (_params.FtpServer.IndexOf("/") != -1)
                {
                    int pos = _params.FtpServer.IndexOf("/");
                    _params.FtpServer = _params.FtpServer.Substring(0, pos);
                }

                do
                {
                    // data export overdue ?
                    if (DateTime.Now > _params.LastDataExport.AddDays(_params.ExportFrequency) || bTest)
                    {
                        int retry = 0;
                        bTest = false;
                        while (!do_data_export() && retry++ < 5)
                            Thread.Sleep(60000);

                        // update last export date
                        _params.LastDataExport = DateTime.Now;
                        _controller.DataAccess.UpdateLastDataExport(_operator_id, _params.LastDataExport);
                    }

                    // check for new import file every 5 minutes
                    if (DateTime.Now > _lastFtpCheckTime.AddMinutes(5) || bTest)
                    {
                        bTest = false;

                        // refresh export parameters
                        _controller.DataAccess.GetDataExportParams(_operator_id, ref _params);
                        _params.FtpServer = "poettinger-wip-ftp.westeurope.cloudapp.azure.com";
                        if (_params.FtpServer.IndexOf("/") != -1)
                        {
                            int pos = _params.FtpServer.IndexOf("/");
                            _params.FtpServer = _params.FtpServer.Substring(0, pos);
                        }

                        LogFile.WriteMessageToLogFile("{0}: Check for data imports.", _params.Operator);
                        
                        ftp_download_file_if_present();

                        _lastFtpCheckTime = DateTime.Now;
                    }

                    Thread.Sleep(10);

                } while (true);
            }
            catch (Exception)
            {
                LogFile.WriteMessageToLogFile("{0}: data import export service stopped", _params.Operator);
            }

            _stop_event.Set();
        }

        /// <summary>
        /// Do the export
        /// </summary>
        public void ExportCustomerData()
        {
            int retry = 0;

            try
            {
                // first get data export params of operator
                _controller.DataAccess.GetDataExportParams(_operator_id, ref _params);

                LogFile.WriteMessageToLogFile("{0}: customer export service started", _params.Operator, _params.Operator);
                LogFile.WriteMessageToLogFile("{0}: parameters are: last export date: {1}, export interval: {2}", _params.Operator, _params.LastDataExport, _params.ExportFrequency);

                if (_params.FtpServer == null)
                {
                    _params.FtpServer = "ftp.poettinger.at";
                    _params.FtpUser = "erleand";
                    _params.FtpPassword = "Erler963";
                }

                LogFile.WriteMessageToLogFile("{0}: ftp parameters are: server: {1}, user: {2}", _params.Operator, _params.FtpServer, _params.FtpUser);

                if (_params.FtpServer.IndexOf("/") != -1)
                {
                    int pos = _params.FtpServer.IndexOf("/");
                    _params.FtpServer = _params.FtpServer.Substring(0, pos);
                }

                // check if directory and server exists
                string dir = String.Format("Ents/{0}/Export", _operator_id);

                if (!ftp_check_directoy(dir, _params.FtpServer, _params.FtpUser, _params.FtpPassword))
                {
                    ftp_make_directory(String.Format("Ents/{0}", _operator_id), _params.FtpServer, _params.FtpUser, _params.FtpPassword);
                    ftp_make_directory(String.Format("Ents/{0}/Export", _operator_id), _params.FtpServer, _params.FtpUser, _params.FtpPassword);
                }


                while (!do_data_export() && retry++ < 5)
                    Thread.Sleep(60000);

                if (retry < 5)
                {
                    // update last export date
                    _params.LastDataExport = DateTime.Now;
                    _controller.DataAccess.UpdateLastDataExport(_operator_id, _params.LastDataExport);
                }

            }
            catch (Exception excp)
            {
                LogFile.WriteMessageToLogFile("{0}: Exception: {1} while trying to export customer data.", excp.Message, _params.Operator);
            }

            LogFile.WriteMessageToLogFile("{0}: data export service ready", _params.Operator);
        }


        #endregion
    }
}
