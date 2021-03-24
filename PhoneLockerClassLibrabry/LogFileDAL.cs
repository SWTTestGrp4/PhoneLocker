using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace PhoneLockerClassLibrary
{
    public class LogFileDAL : ILogging
    {
        private string fileName;
        private string destPath;
        private FileStream outputFileStream;
        private BinaryFormatter formatter;
        public LogFileDAL()
        {
            fileName = "PhoneLockerLog";
            destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            outputFileStream = new FileStream(destPath, FileMode.OpenOrCreate, FileAccess.Write);
            formatter = new BinaryFormatter();
        }
        public void Write(string message)
        {
            try
            {
                formatter.Serialize(outputFileStream,message);
                Console.WriteLine("Log has been saved to App Base Directory");
            }
            catch (Exception e)
            {
                Console.WriteLine("Woops serializing failed. Try again");
                throw e;
            }
        }
    }
}
