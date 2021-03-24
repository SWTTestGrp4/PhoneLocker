using System;
using System.Collections.Generic;
using System.Text;
using PhoneLockerClassLibrary;

namespace PhoneLocker
{
    public interface IStationControl
    {
        public enum PhoneLockerState { }
        public event EventHandler<DoorLockedEventArgs> DoorLockedEvent;
        public event EventHandler<RFIDDetectedEventArgs> RfidDetectedEvent;
        public bool DoorLocked { get; set; }
        public IRFIDReader RfidReader { get; set; }
    }


}
