using System;
using PhoneLocker;

namespace PhoneLockerClassLibrary
{
    public class RFIDReader : IRFIDReader
    {
        public event EventHandler<RFIDDetectedEventArgs> RFIDDetectedEvent;
        public int OldID { get; set; }
        public int ID { get; set; }
        public int ReadRFID()
        {
            
        }

        void IRFIDReader.OnRFIDDetected(RFIDDetectedEventArgs e)
        {
            RFIDDetectedEvent?.Invoke(this, e);
        }
    }
}