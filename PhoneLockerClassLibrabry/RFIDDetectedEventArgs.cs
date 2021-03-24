using System;

namespace PhoneLockerClassLibrary
{
    public class RFIDDetectedEventArgs : EventArgs
    {
        public int RFID { get; set; }
    }
}