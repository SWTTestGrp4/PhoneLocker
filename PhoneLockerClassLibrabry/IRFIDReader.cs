using System;
using System.Collections.Generic;
using System.Text;
using PhoneLockerClassLibrary;

namespace PhoneLocker
{
    public interface IRFIDReader
    {
        public event EventHandler<RFIDDetectedEventArgs> RFIDDetectedEvent;
        public int OldID { get; set; }
        public int ID { get; set; }

        public int ReadRFID();
        protected void OnRFIDDetected(RFIDDetectedEventArgs e);

    }
}
