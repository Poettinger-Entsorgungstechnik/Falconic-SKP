using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogFileUploader
{
    class Program
    {
        static void Main(string[] args)
        {
            LogFileUploader uploader = new LogFileUploader();

            uploader.DoWork();
        }
    }
}
