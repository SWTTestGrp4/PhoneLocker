using System;

namespace PhoneLockerClassLibrary
{
    public class RFIDDetectedEventArgs : EventArgs
    {
        public bool RFIDDetected { get; set; }
    }
}