using System;
using PhoneLocker;

namespace PhoneLockerClassLibrary
{
    public class RFIDReader : IRFIDReader
    {
        public event EventHandler<RFIDDetectedEventArgs> RFIDDetectedEvent;
        public void ReadRFID(int id)
        {
            OnRFIDDetected(new RFIDDetectedEventArgs(){RFID = id});
        }

        public void OnRFIDDetected(RFIDDetectedEventArgs e)
        {
            RFIDDetectedEvent?.Invoke(this, e);
        }
    }
}