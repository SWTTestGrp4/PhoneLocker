using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace PhoneLockerClassLibrary
{
    public class LogFileDAL : ILogging
    {
       public void Write(string message)
        {
            //try
            //{
                using (var stream = new FileStream(
                    "PhoneLockerLog.txt", FileMode.Create, FileAccess.Write, FileShare.Write, 4096))
                {
                    var bytes = Encoding.UTF8.GetBytes(message);
                    stream.Write(bytes, 0, bytes.Length);
                }
                Console.WriteLine("Log has been saved to App Base Directory");
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("Woops serializing failed. Try again");
            //    throw e;
            //}
        }
    }
}
