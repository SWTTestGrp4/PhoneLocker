using System;
using System.Collections.Generic;
using System.Text;
using PhoneLockerClassLibrary;

namespace PhoneLocker
{
    public interface IStationControl
    {
        public enum PhoneLockerState { };

        public bool DoorLocked { get; set; }

        public IRFIDReader RfidReader { get; set; }
    }


}
