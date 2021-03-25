using System;

namespace PhoneLockerClassLibrary
{
    public interface IRFIDReader
    {
        public event EventHandler<RFIDDetectedEventArgs> RFIDDetectedEvent;

        public void ReadRFID(int id);
        public void OnRFIDDetected(RFIDDetectedEventArgs e);

    }
}
