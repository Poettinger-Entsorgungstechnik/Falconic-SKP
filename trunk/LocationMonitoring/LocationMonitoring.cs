using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LocationMonitoring
{
    class LocationMonitoring
    {
        #region members

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private DateTime _tLastLocationCheck = DateTime.Now.Subtract(new TimeSpan(0, 1, 0, 0, 0));
        private Hashtable _lastMonitoringMessage = new Hashtable();

        private string DB_CONNECTION_STRING = "Server=tcp:poettingerwip.database.windows.net,1433;Initial Catalog=WIP;Persist Security Info=False;User ID=wip;Password=9xWQyoatmZ3eH64ef5iS;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";


        #endregion


        public class Location
        {
            int _locationId;
            int _watchdogDuration;
            string _name;

            public int LocationId { get => _locationId; set => _locationId = value; }
            public int WatchdogDuration { get => _watchdogDuration; set => _watchdogDuration = value; }
            public string Name { get => _name; set => _name = value; }
        }

        public class AlertingUser
        {
            public int remote_control_id;
            public int remote_control_type;
            public string memo;
            public string contact;
        }

        public bool GetAlertingUsers(int location_id, ref int number_of_users, ref AlertingUser[] users)
        {
            string sqlStatement = "SELECT REMOTE_CONTROL.REMOTE_CONTROL_ID, REMOTE_CONTROL_TYPE_ID, MEMO, CONTACT FROM REMOTE_CONTROL INNER JOIN ERROR ON REMOTE_CONTROL.REMOTE_CONTROL_ID=ERROR.REMOTE_CONTROL_ID WHERE ERROR.LOCATION_ID=@LocationId";

            bool retval = true;
            int i = 0;

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(DB_CONNECTION_STRING))
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

                                if (reader[3].GetType() == typeof(System.DBNull))
                                    users[i].contact = "";
                                else
                                    users[i++].contact = (string)reader[3];
                            }
                        }
                    }
                }
                number_of_users = i;
            }
            catch (Exception e)
            {
                Logger.Error(e, "Exception in GetAlertingUsers appeared.");
                retval = false;
            }

            return retval;
        }


        private void DoAlerting(int locationId, string smsMessage, string subject)
        {
            AlertingUser[] alertingUsers = new AlertingUser[32];
            int numUsers = 0;

            for (int i = 0; i < 32; i++)
            {
                alertingUsers[i] = new AlertingUser();
            }

            GetAlertingUsers(locationId, ref numUsers, ref alertingUsers);

            ArrayList users = new ArrayList();

            if (smsMessage.Length > 160)
                smsMessage = smsMessage.Substring(0, 160);


            for (int i = 0; i < numUsers; i++)
            {
                FieldAreaNetwork.AlertingUser alertUser = new FieldAreaNetwork.AlertingUser();

                Logger.Info("Alert User: {0}, with Telnumber: {1}, Msg: {2}", alertingUsers[i].memo, alertingUsers[i].contact, smsMessage);

                alertUser.ClientName = "SKP";
                alertUser.TelephoneNumber = alertingUsers[i].contact;
                alertUser.Flags = (int)FieldAreaNetwork.ALERTING_FLAGS.SMS_ENABLED;
                alertUser.EmailAddress = "";
                alertUser.Name = "Unknown";
                users.Add(alertUser);
            }

            try
            {
                FieldAreaNetwork.AlertingControl.AddAlarm("alarm.wallner-automation.com", users, "WIP", smsMessage, false, 0);
            }
            catch (Exception excp)
            {
                Logger.Fatal(excp, "Exception while tring send sms!");
            }
        }

        /// <summary>
        /// Do the service's job
        /// </summary>
        public void DoWork()
        {
            Logger.Info("Location Monitoring Service Started!");

            try
            {
                do
                {
                    TimeSpan ts = DateTime.Now.Subtract(_tLastLocationCheck);
                    List<Location> locations = new List<Location>();

                    if (ts.TotalMinutes >= 30)
                    {
                        _tLastLocationCheck = DateTime.Now;

                        string sqlStatement = "SELECT * FROM LOCATION WHERE MONITORING_ACTIVE=1";

                        Logger.Info("Start check ...");

                        try
                        {
                            using (SqlConnection sqlConnection = new SqlConnection(DB_CONNECTION_STRING))
                            {
                                sqlConnection.Open();

                                using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                                {
                                    using (SqlDataReader reader = cmd.ExecuteReader())
                                    {
                                        while (reader.Read())
                                        {
                                            DateTime dtFrom = new DateTime();
                                            DateTime dtTo = new DateTime();
                                            TimeSpan tsSinceStart = DateTime.Today - new DateTime(1900, 1, 1);
                                            int duration = 0;
                                            int containerId = 0;
                                            int locationId = 0;

                                            if (reader["MONITOR_FROM"].GetType() != typeof(System.DBNull))
                                            {
                                                dtFrom = (DateTime)reader["MONITOR_FROM"];
                                                dtFrom = dtFrom.Add(tsSinceStart);
                                            }

                                            if (reader["MONITOR_TO"].GetType() != typeof(System.DBNull))
                                            {
                                                dtTo = (DateTime)reader["MONITOR_TO"];
                                                dtTo = dtTo.Add(tsSinceStart);
                                            }

                                            if (reader["MONITOR_DURATION"].GetType() != typeof(System.DBNull))
                                            {
                                                duration = (int)reader["MONITOR_DURATION"];
                                            }

                                            if (reader["CONTAINER_ID"].GetType() != typeof(System.DBNull))
                                            {
                                                containerId = (int)reader["CONTAINER_ID"];
                                            }

                                            if (reader["LOCATION_ID"].GetType() != typeof(System.DBNull))
                                            {
                                                locationId = (int)reader["LOCATION_ID"];
                                            }

                                            if (dtTo <= dtFrom)
                                                dtTo = dtTo.AddDays(1);

                                            if (DateTime.Now >= dtFrom && DateTime.Now < dtTo)
                                            {
                                                // store this loaction for monitoring
                                                Location loc = new Location();

                                                loc.LocationId = locationId;
                                                loc.WatchdogDuration = duration;
                                                loc.Name = (string)reader["LOCATION"];

                                                Logger.Info("LocationId: {0}, ContainerId: {1}, From: {2}, To: {3}, Duration: {4}", locationId, containerId,
                                                    dtFrom, dtTo, duration);

                                                locations.Add(loc);
                                            }
                                        }

                                        reader.Close();
                                    }
                                }

                                foreach (Location loc in locations)
                                {
                                    sqlStatement = "SELECT TRANSACTION_ID FROM TRANSACTIONS WHERE LOCATION_ID=@LocationId AND DATE > @Date";

                                    using (SqlCommand cmd = new SqlCommand(sqlStatement, sqlConnection))
                                    {
                                        SqlParameter locId = new SqlParameter("@LocationId", SqlDbType.Int);

                                        locId.Value = loc.LocationId;
                                        cmd.Parameters.Add(locId);

                                        DateTime dtMin = DateTime.Now.AddMinutes(-loc.WatchdogDuration);
                                        SqlParameter minDate = new SqlParameter("@Date", SqlDbType.DateTime);

                                        minDate.Value = dtMin;
                                        cmd.Parameters.Add(minDate);

                                        Logger.Info("Check LocationId: {0}, Starttime: {1}", loc.LocationId, dtMin);

                                        using (SqlDataReader reader = cmd.ExecuteReader())
                                        {
                                            if (!reader.Read())
                                            {
                                                // there was no transactions in specified time
                                                // so do alerting when necessary
                                                bool bDoAlerting = true;

                                                Logger.Info("LocationId: {0}, No Transactions found", loc.LocationId);

                                                if (_lastMonitoringMessage[loc.LocationId] != null)
                                                {
                                                    TimeSpan ts1 = DateTime.Now.Subtract((DateTime)_lastMonitoringMessage[loc.LocationId]);

                                                    if (ts1.TotalHours < 24)
                                                    {
                                                        Logger.Info("Skip alerting since time is not right");
                                                        bDoAlerting = false;
                                                    }
                                                }

                                                if (bDoAlerting)
                                                {
                                                    string smsMessage = String.Format("{0}: Warning: No Transactions since: {1} minutes", loc.Name, loc.WatchdogDuration);
                                                    string subject = "WIP - LocationMonitoring";

                                                    DoAlerting(loc.LocationId, smsMessage, subject);
                                                    _lastMonitoringMessage[loc.LocationId] = DateTime.Now;
                                                }
                                            }
                                            else
                                            {
                                                _lastMonitoringMessage[loc.LocationId] = null;
                                            }
                                        }
                                    }
                                }

                                sqlConnection.Close();
                            }
                        }
                        catch (Exception e)
                        {
                            Logger.Fatal(e, "Exception in LocationMonitoring appeared.");
                        }

                        Logger.Info("End check ...");
                    }

                    Thread.Sleep(1000);

                } while (true);
            }
            catch (Exception excp)
            {
                Logger.Error(excp, "Exception: in DoWork");
            }

            Logger.Info("Location Monitoring Service Stopped!");
        }
    }
}
