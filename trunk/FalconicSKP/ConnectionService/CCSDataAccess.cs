using Luthien;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading;

namespace CCS
{
    public class AlertingUser
    {
        #region members

        private int _id;
        private string _firstName;
        private string _lastName;
        private string _gsmNumber;
        private string _email;

        #endregion

        #region properties

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        public string GsmNumber
        {
            get { return _gsmNumber; }
            set { _gsmNumber = value; }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        #endregion
    }

    public class APNParams
    {
        #region members

        private int _id;
        private string _name;
        private string _country;
        private string _apn;
        private string _username;
        private string _password;

        #endregion

        #region properties

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Country
        {
            get { return _country; }
            set { _country = value; }
        }

        public string APN
        {
            get { return _apn; }
            set { _apn = value; }
        }

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        #endregion
        public override string ToString()
        {
            return _name;
        }
    }

    public class CCSDataAccess
    {
        #region Members

        private string _connection_string;
        private static Mutex mutex = new Mutex();

        #endregion

        #region Constructor

        public CCSDataAccess(string connection_string)
        {
            _connection_string = connection_string;
        }

        #endregion

        #region Data access methods

        public bool GetApns(ref int number_of_apns, ref APNParams[] apnParams)
        {
            string sqlStatement = "SELECT * FROM ACCESSPOINTNAMES";

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
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader["APN_ID"].GetType() != typeof(System.DBNull))
                                    apnParams[i].Id = (int)reader["APN_ID"];

                                if (reader["COUNTRY"].GetType() != typeof(System.DBNull))
                                    apnParams[i].Country = (string)reader["COUNTRY"];

                                if (reader["NAME"].GetType() != typeof(System.DBNull))
                                    apnParams[i].Name = (string)reader["NAME"];

                                if (reader["ACCESSPOINT"].GetType() != typeof(System.DBNull))
                                    apnParams[i].APN = (string)reader["ACCESSPOINT"];

                                if (reader["USERNAME"].GetType() != typeof(System.DBNull))
                                    apnParams[i].Username = (string)reader["USERNAME"];

                                if (reader["PASSWORD"].GetType() != typeof(System.DBNull))
                                    apnParams[i].Password = (string)reader["PASSWORD"];

                                i++;
                            }
                        }
                    }
                }
                number_of_apns = i;
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("{0} in \'GetApns\' appeared.", e.Message);
                retval = false;
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool UpdateApn(ref int apn_id, string apn, string username, string password)
        {
            bool retval = true;

            int apn_id1 = -1;
            string apnname = "";
            string country = "";
            string username1 = "";
            string password1 = "";

            if (GetApnParams(apn, ref apn_id1, ref apnname, ref country, ref username1, ref password1))
            {
                if (apn_id1 != apn_id)
                {
                    // user changed to another apn
                    apn_id = apn_id1;
                    return true;
                }
                else
                {
                    // update username and password
                    String sqlStatement = "UPDATE ACCESSPOINTNAMES SET USERNAME = @Username, PASSWORD = @Password WHERE APN_ID=@APNId";
                    try
                    {
                        mutex.WaitOne();

                        using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                        {
                            sqlConnection.Open();

                            // store new operator in database
                            using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                            {
                                SqlParameter p_1 = new SqlParameter("@Username", SqlDbType.VarChar);
                                p_1.Value = username;
                                cmd.Parameters.Add(p_1);

                                SqlParameter p_2 = new SqlParameter("@Password", SqlDbType.VarChar);
                                p_2.Value = password;
                                cmd.Parameters.Add(p_2);

                                SqlParameter p_3 = new SqlParameter("@APNId", SqlDbType.Int);
                                p_3.Value = apn_id;
                                cmd.Parameters.Add(p_3);

                                // store it
                                if (cmd.ExecuteNonQuery() != 1)
                                {
                                    retval = false;
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        LogFile.WriteErrorToLogFile(String.Format("{0} in \'CreateApn\' appeared.", e.Message));
                    }
                    finally
                    {
                        mutex.ReleaseMutex();
                    }
                }
            }
            else
            {
                CreateApn(apn, username, password, "xx", "DE");
                GetApnParams(apn, ref apn_id, ref apnname, ref country, ref username, ref password);
            }


            return retval;
        }

        public bool GetApnParams(string apn, ref int apn_id, ref string apnname, ref string country, ref string username, ref string password)
        {
            string sqlStatement = "SELECT APN_ID, NAME, COUNTRY, USERNAME, PASSWORD FROM ACCESSPOINTNAMES WHERE ACCESSPOINT=@Name";
            bool retval = true;

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter param = new SqlParameter("Name", SqlDbType.VarChar);

                        param.Value = apn;

                        cmd.Parameters.Add(param);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                apn_id = (int)reader[0];
                                apnname = (string)reader[1];
                                country = (string)reader[2];
                                if (reader[3].GetType() != typeof(System.DBNull))
                                    username = (string)reader[3];
                                if (reader[4].GetType() != typeof(System.DBNull))
                                    password = (string)reader[4];
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile(String.Format("{0} in \'GetApnParams\' appeared.", e.Message));
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool GetApnParams(int apn_id, ref string apn, ref string apnname, ref string country, ref string username, ref string password)
        {
            string sqlStatement = "SELECT COUNTRY, NAME, ACCESSPOINT, USERNAME, PASSWORD FROM ACCESSPOINTNAMES WHERE APN_ID=@APNId";
            bool retval = true;

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter param = new SqlParameter("@APNId", SqlDbType.Int);

                        param.Value = apn_id;

                        cmd.Parameters.Add(param);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader[0].GetType() != typeof(System.DBNull))
                                    country = (string)reader[0];
                                apnname = (string)reader[1];
                                apn = (string)reader[2];
                                if (reader[3].GetType() != typeof(System.DBNull))
                                    username = (string)reader[3];
                                if (reader[4].GetType() != typeof(System.DBNull))
                                    password = (string)reader[4];
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile(String.Format("{0} in \'GetApnParams\' appeared.", e.Message));
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool GetContainerId(int operator_id, string container_name, ref int container_id)
        {
            string sqlStatement = "SELECT CONTAINER_ID FROM CONTAINER WHERE OPERATOR_ID=@OperatorId AND CONTAINER_NAME=@ContainerName";
            bool retval = true;

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter p_1 = new SqlParameter("@OperatorId", SqlDbType.Int);
                        p_1.Value = operator_id;

                        cmd.Parameters.Add(p_1);

                        SqlParameter p_2 = new SqlParameter("@ContainerName", SqlDbType.VarChar);
                        p_2.Value = container_name;

                        cmd.Parameters.Add(p_2);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                container_id = (int)reader[0];
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("{0} in \'GetContainerId\' appeared.", e.Message);
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool GetContainerParams(int container_id, ref int apn_id, ref string name, ref string phoneNumber, ref int blockingTime, ref int delayTime, ref string material)
        {
            string sqlStatement = "SELECT APN_ID, CONTAINER_NAME, GSM_NUMBER, BLOCKING_TIME, DELAY_TIME, MATERIAL FROM CONTAINER WHERE CONTAINER_ID=@ContainerId";
            bool retval = true;

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    // store new operator in database
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter p_1 = new SqlParameter("@ContainerId", SqlDbType.Int);
                        p_1.Value = container_id;
                        cmd.Parameters.Add(p_1);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                apn_id = (int)reader[0];
                                name = (string)reader[1];
                                phoneNumber = (string)reader[2];
                                blockingTime = (int)reader[3];
                                delayTime = (int)reader[4];
                                material = (string)reader[5];
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("{0} in \'GetContainerParams\' appeared.", e.Message);
                retval = false;
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool CreateContainer(int operator_id, string name, int apn_id, string phonenumber, int blockingTime, int delayTime, string material)
        {
            string sqlStatement = "INSERT INTO CONTAINER ([OPERATOR_ID], [APN_ID], [CONTAINER_NAME], [GSM_NUMBER], [BLOCKING_TIME], [DELAY_TIME], [MATERIAL]) VALUES (@OperatorId, @APN_Id, @Name, @GSMNumber, @BlockingTime, @DelayTime, @Material)";
            bool retval = true;

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    // store new operator in database
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter p_1 = new SqlParameter("@OperatorId", SqlDbType.Int);
                        p_1.Value = operator_id;
                        cmd.Parameters.Add(p_1);

                        SqlParameter p_2 = new SqlParameter("@APN_Id", SqlDbType.Int);
                        p_2.Value = apn_id;
                        cmd.Parameters.Add(p_2);

                        SqlParameter p_3 = new SqlParameter("@Name", SqlDbType.VarChar);
                        p_3.Value = name;
                        cmd.Parameters.Add(p_3);

                        SqlParameter p_4 = new SqlParameter("@GSMNumber", SqlDbType.VarChar);
                        p_4.Value = phonenumber;
                        cmd.Parameters.Add(p_4);

                        SqlParameter p_5 = new SqlParameter("@BlockingTime", SqlDbType.Int);
                        p_5.Value = blockingTime;
                        cmd.Parameters.Add(p_5);

                        SqlParameter p_6 = new SqlParameter("@DelayTime", SqlDbType.Int);
                        p_6.Value = delayTime;
                        cmd.Parameters.Add(p_6);

                        SqlParameter p_7 = new SqlParameter("@Material", SqlDbType.VarChar);
                        p_7.Value = material;
                        cmd.Parameters.Add(p_7);

                        // store it
                        if (cmd.ExecuteNonQuery() != 1)
                        {
                            retval = false;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("{0} in \'CreateContainer\' appeared.", e.Message);
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool UpdateContainer(int container_id, string name, int apn_id, string phonenumber, int blockingTime, int delayTime, string material)
        {
            // update username and password
            String sqlStatement = "UPDATE CONTAINER SET APN_ID = @APNId, CONTAINER_NAME = @ContainerName, GSM_NUMBER = @GSMNumber, BLOCKING_TIME = @BlockingTime, DELAY_TIME = @DelayTime, MATERIAL = @Material WHERE CONTAINER_ID=@ContainerId";
            bool retval = true;

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    // store new operator in database
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter p_1 = new SqlParameter("@APNId", SqlDbType.Int);
                        p_1.Value = apn_id;
                        cmd.Parameters.Add(p_1);

                        SqlParameter p_2 = new SqlParameter("@ContainerName", SqlDbType.VarChar);
                        p_2.Value = name;
                        cmd.Parameters.Add(p_2);

                        SqlParameter p_3 = new SqlParameter("@GSMNumber", SqlDbType.VarChar);
                        p_3.Value = phonenumber;
                        cmd.Parameters.Add(p_3);

                        SqlParameter p_4 = new SqlParameter("@BlockingTime", SqlDbType.Int);
                        p_4.Value = blockingTime;
                        cmd.Parameters.Add(p_4);

                        SqlParameter p_5 = new SqlParameter("@DelayTime", SqlDbType.Int);
                        p_5.Value = delayTime;
                        cmd.Parameters.Add(p_5);

                        SqlParameter p_6 = new SqlParameter("@Material", SqlDbType.VarChar);
                        p_6.Value = material;
                        cmd.Parameters.Add(p_6);

                        SqlParameter p_7 = new SqlParameter("@ContainerId", SqlDbType.Int);
                        p_7.Value = container_id;
                        cmd.Parameters.Add(p_7);

                        // store it
                        if (cmd.ExecuteNonQuery() != 1)
                        {
                            retval = false;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("{0} in \'UpdateContainer\' appeared.", e.Message);
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public int GetOperatorId(string operator_name)
        {
            string sqlStatement = "SELECT OPERATOR_ID FROM OPERATOR WHERE OPERATOR_NAME=@Name";
            int retval = -1;

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter param = new SqlParameter("Name", SqlDbType.VarChar);

                        param.Value = operator_name;

                        cmd.Parameters.Add(param);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                retval = (int)reader[0];
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("{0} in \'GetOperatorId\' appeared.", e.Message);
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool CreateOperator(string name, string language, string username, string password)
        {
            string sqlStatement = "INSERT INTO OPERATOR ([OPERATOR_NAME], [LANGUAGE_CODE], [LOGIN], [PASSWORD]) VALUES (@OperatorName, @LanguageCode, @Login, @Password)";
            bool retval = true;

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    // store new operator in database
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter p_1 = new SqlParameter("@OperatorName", SqlDbType.VarChar);
                        p_1.Value = name;
                        cmd.Parameters.Add(p_1);

                        SqlParameter p_2 = new SqlParameter("@LanguageCode", SqlDbType.VarChar);
                        p_2.Value = language;
                        cmd.Parameters.Add(p_2);

                        SqlParameter p_3 = new SqlParameter("@Login", SqlDbType.VarChar);
                        p_3.Value = username;
                        cmd.Parameters.Add(p_3);

                        SqlParameter p_4 = new SqlParameter("@Password", SqlDbType.VarChar);
                        p_4.Value = password;
                        cmd.Parameters.Add(p_4);

                        // store it
                        if (cmd.ExecuteNonQuery() != 1)
                        {
                            retval = false;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("{0} in \'CreateOperator\' appeared.", e.Message);
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool ModifyOperator(int operator_id, string name, string language, string username, string password)
        {
            string sqlStatement = "";
            bool retval = true;

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    // modify existing operator in database
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        sqlStatement = "UPDATE OPERATOR SET OPERATOR_NAME= @OperatorName, LANGUAGE_CODE= @LanguageCode, LOGIN= @Login, PASSWORD= @Password WHERE OPERATOR_ID=@OperatorId";

                        SqlParameter p_1 = new SqlParameter("@OperatorName", SqlDbType.VarChar);
                        p_1.Value = name;
                        cmd.Parameters.Add(p_1);

                        SqlParameter p_2 = new SqlParameter("@LanguageCode", SqlDbType.VarChar);
                        p_2.Value = language;
                        cmd.Parameters.Add(p_2);

                        SqlParameter p_3 = new SqlParameter("@Login", SqlDbType.VarChar);
                        p_3.Value = username;
                        cmd.Parameters.Add(p_3);

                        SqlParameter p_4 = new SqlParameter("@Password", SqlDbType.VarChar);
                        p_4.Value = password;
                        cmd.Parameters.Add(p_4);

                        SqlParameter p_5 = new SqlParameter("@OperatorId", SqlDbType.Int);
                        p_5.Value = operator_id;
                        cmd.Parameters.Add(p_5);

                        cmd.CommandText = sqlStatement;

                        // store it
                        if (cmd.ExecuteNonQuery() != 1)
                        {
                            retval = false;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("{0} in \'ModifyOperator\' appeared.", e.Message);
            }
            finally
            {
                mutex.ReleaseMutex(); ;
            }

            return retval;
        }

        public bool GetOperatorParams(int operator_id, ref string name, ref string language, ref string username, ref string password)
        {
            string sqlStatement = "SELECT * FROM OPERATOR WHERE OPERATOR_ID=@OperatorId";
            bool retval = true;

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    // store new operator in database
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter p_1 = new SqlParameter("@OperatorId", SqlDbType.Int);
                        p_1.Value = operator_id;
                        cmd.Parameters.Add(p_1);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                name = (string)reader["OPERATOR_NAME"];
                                language = (string)reader["LANGUAGE_CODE"];
                                username = (string)reader["LOGIN"];
                                password = (string)reader["PASSWORD"];
                            }
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



        public bool GetOperatorParams(int operator_id, ref string operator_name, ref string language_code)
        {
            string sqlStatement = "SELECT OPERATOR_NAME, LANGUAGE_CODE FROM OPERATOR WHERE OPERATOR_ID=@OperatorId";
            bool retval = true;

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter param_operator_id = new SqlParameter("@OperatorId", SqlDbType.Int);
                        param_operator_id.Value = operator_id;

                        cmd.Parameters.Add(param_operator_id);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                operator_name = (string)reader[0];
                                language_code = (string)reader[1];
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

        public bool StoreContainerData(string simcardNumber, string gsmNumber)
        {
            int numberOfFields = 0;
            bool retval = true;

            try
            {
                string sqlStatement = "UPDATE CONTAINER SET SIM_SERIALNUMBER=@SimNumber WHERE GSM_NUMBER=@GSMNumber";

                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter p_SimNumber = new SqlParameter("@SimNumber", SqlDbType.VarChar);
                        p_SimNumber.Value = simcardNumber;

                        cmd.Parameters.Add(p_SimNumber);

                        SqlParameter p_GSMNumber = new SqlParameter("@GSMNumber", SqlDbType.VarChar);
                        p_GSMNumber.Value = gsmNumber;

                        cmd.Parameters.Add(p_GSMNumber);

                        if ((numberOfFields = cmd.ExecuteNonQuery()) == 0)
                        {
                            LogFile.WriteErrorToLogFile("CCS {0}: StoreContainerData: No fields where found to change", gsmNumber);
                            retval = false;
                        }
                        else
                            LogFile.WriteMessageToLogFile("CCS {0}: StoreContainerData: successful", gsmNumber);
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("CCS {0}: Exception: {1} in \'StoreContainerData\' appeared.", gsmNumber, e.Message);
                retval = false;
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool GetContainerParams(string sim_serialnumber, ref int container_id, ref string container_name, ref int operator_id, ref int blocking_time, ref int delay_time, ref string material)
        {
            string sqlStatement = "SELECT CONTAINER_ID, CONTAINER_NAME, OPERATOR_ID, BLOCKING_TIME, DELAY_TIME, MATERIAL FROM CONTAINER WHERE SIM_SERIALNUMBER=@SIM_SerialNumber";
            bool retval = true;

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter param_sim_serialnumber = new SqlParameter("@SIM_SerialNumber", SqlDbType.VarChar);
                        param_sim_serialnumber.Value = sim_serialnumber;

                        cmd.Parameters.Add(param_sim_serialnumber);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                container_id = (int)reader[0];
                                container_name = (string)reader[1];
                                operator_id = (int)reader[2];
                                blocking_time = (int)reader[3];
                                delay_time = (int)reader[4];
                                material = (string)reader[5];
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
                LogFile.WriteErrorToLogFile("{0} in \'GetContainerParams\' appeared.", e.Message);
                retval = false;
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool GetAlertingUsers(int container_id, ref int number_of_users, ref AlertingUser[] users)
        {
            string sqlStatement = "SELECT ALERTING_USER_ID, FIRST_NAME, LAST_NAME, GSM_NUMBER, EMAIL FROM ALERTINGUSERS WHERE CONTAINER_ID=@ContainerId";

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
                        SqlParameter p_containerId = new SqlParameter("@ContainerId", SqlDbType.Int);
                        p_containerId.Value = container_id;

                        cmd.Parameters.Add(p_containerId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader[0].GetType() != typeof(System.DBNull))
                                    users[i].Id = (int)reader[0];

                                if (reader[1].GetType() != typeof(System.DBNull))
                                    users[i].FirstName = (string)reader[1];

                                if (reader[2].GetType() != typeof(System.DBNull))
                                    users[i].LastName = (string)reader[2];

                                if (reader[3].GetType() != typeof(System.DBNull))
                                    users[i].GsmNumber = (string)reader[3];

                                if (reader[4].GetType() != typeof(System.DBNull))
                                    users[i++].Email = (string)reader[4];
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

        public bool CreateAlertingUser(int container_id, string firstname, string lastname, string gsm_number, string email)
        {
            string sqlStatement = "INSERT INTO ALERTINGUSERS ([CONTAINER_ID], [FIRST_NAME], [LAST_NAME], [GSM_NUMBER], [EMAIL]) VALUES (@ContainerId, @FirstName, @LastName, @GSMNumber, @Email)";
            bool retval = true;

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    // store new operator in database
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter p_1 = new SqlParameter("@ContainerId", SqlDbType.Int);
                        p_1.Value = container_id;
                        cmd.Parameters.Add(p_1);

                        SqlParameter p_2 = new SqlParameter("@FirstName", SqlDbType.VarChar);
                        p_2.Value = firstname;
                        cmd.Parameters.Add(p_2);

                        SqlParameter p_3 = new SqlParameter("@LastName", SqlDbType.VarChar);
                        p_3.Value = lastname;
                        cmd.Parameters.Add(p_3);

                        SqlParameter p_4 = new SqlParameter("@GSMNumber", SqlDbType.VarChar);
                        p_4.Value = gsm_number;
                        cmd.Parameters.Add(p_4);

                        SqlParameter p_5 = new SqlParameter("@Email", SqlDbType.VarChar);
                        p_5.Value = email;
                        cmd.Parameters.Add(p_5);

                        // store it
                        if (cmd.ExecuteNonQuery() != 1)
                        {
                            retval = false;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("{0} in \'CreateApn\' appeared.", e.Message);
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool ModifyAlertingUser(int userId, string firstname, string lastname, string gsmnumber, string email)
        {
            bool retval = false;

            try
            {
                string sqlStatement = "UPDATE ALERTINGUSERS SET FIRST_NAME=@FirstName, LAST_NAME=@LastName, EMAIL=@Email, GSM_NUMBER=@GsmNumber WHERE ALERTING_USER_ID=@AlertingUserId";

                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter p_1 = new SqlParameter("@AlertingUserId", SqlDbType.Int);
                        p_1.Value = userId;

                        cmd.Parameters.Add(p_1);

                        SqlParameter p_2 = new SqlParameter("@FirstName", SqlDbType.VarChar);
                        p_2.Value = firstname;

                        cmd.Parameters.Add(p_2);

                        SqlParameter p_3 = new SqlParameter("@LastName", SqlDbType.VarChar);
                        p_3.Value = lastname;

                        cmd.Parameters.Add(p_3);

                        SqlParameter p_4 = new SqlParameter("@Email", SqlDbType.VarChar);
                        p_4.Value = email;

                        cmd.Parameters.Add(p_4);

                        SqlParameter p_5 = new SqlParameter("@GsmNumber", SqlDbType.VarChar);
                        p_5.Value = gsmnumber;

                        cmd.Parameters.Add(p_5);

                        if (cmd.ExecuteNonQuery() == 0)
                        {
                            LogFile.WriteErrorToLogFile("CCS: Could not modify user id: {0}", userId);
                            retval = false;
                        }
                        else
                            retval = true;
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("CCS: Exception: {0} in \'ModifyAlertingUser\' appeared.", e.Message);
                retval = false;
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool DeleteAPN(int apnId)
        {
            bool retval = false;

            try
            {
                string sqlStatement = "DELETE ACCESSPOINTNAMES WHERE APN_ID=@ApnId";

                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter p_1 = new SqlParameter("@ApnId", SqlDbType.Int);
                        p_1.Value = apnId;

                        cmd.Parameters.Add(p_1);

                        if (cmd.ExecuteNonQuery() == 0)
                        {
                            LogFile.WriteErrorToLogFile("CCS: Could not delete apn id: {0}", apnId);
                            retval = false;
                        }
                        else
                            retval = true;
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("CCS: Exception: {0} in \'DeleteAPN\' appeared.", e.Message);
                retval = false;
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool DeleteContainer(int containerId)
        {
            bool retval = false;

            try
            {
                string sqlStatement = "DELETE CONTAINER WHERE CONTAINER_ID=@ContainerId";

                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter p_1 = new SqlParameter("@ContainerId", SqlDbType.Int);
                        p_1.Value = containerId;

                        cmd.Parameters.Add(p_1);

                        if (cmd.ExecuteNonQuery() == 0)
                        {
                            LogFile.WriteErrorToLogFile("CCS: Could not delete container id: {0}", containerId);
                            retval = false;
                        }
                        else
                            retval = true;
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("CCS: Exception: {0} in \'DeleteContainer\' appeared.", e.Message);
                retval = false;
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool DeleteAlertingUser(int alertingUserId)
        {
            bool retval = false;

            try
            {
                string sqlStatement = "DELETE ALERTINGUSERS WHERE ALERTING_USER_ID=@AlertingUserId";

                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter p_1 = new SqlParameter("@AlertingUserId", SqlDbType.Int);
                        p_1.Value = alertingUserId;

                        cmd.Parameters.Add(p_1);

                        if (cmd.ExecuteNonQuery() == 0)
                        {
                            LogFile.WriteErrorToLogFile("CCS: Could not delete user id: {0}", alertingUserId);
                            retval = false;
                        }
                        else
                            retval = true;
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("CCS: Exception: {0} in \'DeleteAlertingUser\' appeared.", e.Message);
                retval = false;
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return retval;
        }

        public bool CreateApn(string apn, string username, string password, string name, string country)
        {
            string sqlStatement = "INSERT INTO ACCESSPOINTNAMES ([COUNTRY], [NAME], [ACCESSPOINT], [USERNAME], [PASSWORD]) VALUES (@Country, @Name, @AccessPoint, @Username, @Password)";
            bool retval = true;

            try
            {
                mutex.WaitOne();

                using (SqlConnection sqlConnection = new SqlConnection(_connection_string))
                {
                    sqlConnection.Open();

                    // store new operator in database
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        SqlParameter p_1 = new SqlParameter("@Country", SqlDbType.NChar);
                        p_1.Value = country;
                        cmd.Parameters.Add(p_1);

                        SqlParameter p_2 = new SqlParameter("@Name", SqlDbType.VarChar);
                        p_2.Value = name;
                        cmd.Parameters.Add(p_2);

                        SqlParameter p_3 = new SqlParameter("@AccessPoint", SqlDbType.VarChar);
                        p_3.Value = apn;
                        cmd.Parameters.Add(p_3);

                        SqlParameter p_4 = new SqlParameter("@Username", SqlDbType.VarChar);
                        p_4.Value = username;
                        cmd.Parameters.Add(p_4);

                        SqlParameter p_5 = new SqlParameter("@Password", SqlDbType.VarChar);
                        p_5.Value = password;
                        cmd.Parameters.Add(p_5);

                        // store it
                        if (cmd.ExecuteNonQuery() != 1)
                        {
                            retval = false;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteErrorToLogFile("{0} in \'CreateApn\' appeared.", e.Message);
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
