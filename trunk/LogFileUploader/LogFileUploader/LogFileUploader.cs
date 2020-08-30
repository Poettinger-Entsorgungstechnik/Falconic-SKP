using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using WinSCP;

namespace LogFileUploader
{

    class LogFileUploader
    {
        static string _strFTPServer = "ftp.poettinger.at";
        static string _strUsername = "erleand";
        static string _strPassword = "Erler963";

        //        static string _strFTPServer = "poettinger-wip-ftp.westeurope.cloudapp.azure.com",
        //        static string _strUsername = "skp";
        //       static string _strPassword = "RlNz2M5xv4dSW1PfnZvT";


        private string _rootPath = @"C:\SKP\Log\";
        private long _actualFileSize = 0;
        private bool _bDoneForToday = false;

        // Setup session options
        WinSCP.SessionOptions _sessionOptions = new WinSCP.SessionOptions
        {
            Protocol = WinSCP.Protocol.Ftp,
            HostName = _strFTPServer,
            UserName = _strUsername,
            Password = _strPassword,
//            FtpSecure = FtpSecure.Implicit,
//            GiveUpSecurityAndAcceptAnyTlsHostCertificate = true,
        };


        private void SessionFileTransferProgress(object sender, WinSCP.FileTransferProgressEventArgs e)
        {
            Console.WriteLine("Synchronise file: {0}, progress: {1}", e.FileName, e.FileProgress);
        }

        void synchroniseAllFiles()
        {
            Console.WriteLine("Upload files started...");

            ServicePointManager.ServerCertificateValidationCallback =
                (s, certificate, chain, sslPolicyErrors) => true;

            try
            {
                using (WinSCP.Session session = new WinSCP.Session())
                {
                    session.FileTransferProgress += SessionFileTransferProgress;
                    // Connect
                    session.Open(_sessionOptions);
                    string localDir = _rootPath;

                    WinSCP.SynchronizationResult result = session.SynchronizeDirectories(WinSCP.SynchronizationMode.Remote, localDir, "Ents/Logs", false);

                    if (result.IsSuccess)
                    {
                        Console.WriteLine("Directories successfully synchronised!");
                    }
                    else
                    {
                        for (int i = 0; i < result.Failures.Count; i++)
                        {
                            Console.WriteLine("Error: {0}", result.Failures[i].Message);
                        }
                    }
                }
            }
            catch (Exception excp)
            {
                Console.WriteLine("Exception: {0}, {1}", excp.Message, excp.StackTrace);
            }
        }

        private void synchroniseTodaysLogfile()
        {
            try
            {
                string fileName = DateTime.Now.ToString("yyyy_MM_dd") + ".log";

                string uri = string.Format("ftp://{0}/Ents/Logs/{1}", _strFTPServer, fileName);

                Console.WriteLine("{0}: Try to synchronize logfile: {1}", DateTime.Now, uri);

                // create the ftp request
                WebRequest request = FtpWebRequest.Create(uri);
                request.Credentials = new NetworkCredential(_strUsername, _strPassword);

                // state that we uploading the file
                request.Method = WebRequestMethods.Ftp.AppendFile;
                request.Timeout = 600000;
                ((FtpWebRequest)request).KeepAlive = false;
                //                ((FtpWebRequest)request).EnableSsl = true;

                ServicePointManager.ServerCertificateValidationCallback =
                          (s, certificate, chain, sslPolicyErrors) => true;

                using (Stream ftpStream = request.GetRequestStream())
                using (FileStream fs = File.Open(_rootPath + fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    fs.Seek(_actualFileSize, SeekOrigin.Begin);
                    fs.CopyTo(ftpStream);
                    _actualFileSize = fs.Length;
                }

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                if (response.StatusCode != FtpStatusCode.ClosingData)
                    Console.WriteLine("Response: {0}, {1}", response.ToString(), response.StatusCode);
                response.Close();
            }
            catch (Exception excp)
            {
                Console.WriteLine("Exception: {0}, {1} while trying to upload file!", excp.Message, excp.StackTrace);
                if (excp.Message.IndexOf("550") != -1)
                {
                    Console.WriteLine("------ > Synchronise all files! <--------");
                    synchroniseAllFiles();
                }
            }
        }

        void t1_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (DateTime.Now.Hour == 23 && DateTime.Now.Minute == 59)
            {
                if (_bDoneForToday == false)
                {
                    synchroniseAllFiles();
                }

                _bDoneForToday = true;
            }
            else
                _bDoneForToday = false;

            synchroniseTodaysLogfile();
        }

        public void DoWork()
        {
            Timer t1 = new Timer();
            t1.Interval = (1000 * 60 * 1);
            t1.Elapsed += new ElapsedEventHandler(t1_Elapsed);
            t1.AutoReset = true;
            t1.Start();

            synchroniseAllFiles();

            // start immediately
            t1_Elapsed(null, null);

            Console.ReadLine();
        }
    }
}
