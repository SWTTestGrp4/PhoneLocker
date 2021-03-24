using System;
using System.Collections.Generic;
using System.Text;
using PhoneLockerClassLibrary;

namespace PhoneLocker
{
    public interface IRFIDReader
    {
        public event EventHandler<RFIDDetectedEventArgs> RFIDDetectedEvent;

        public void ReadRFID(int id);
        public void OnRFIDDetected(RFIDDetectedEventArgs e);

    }
}
