using System;

namespace PhoneLockerClassLibrary
{
    public class DoorLockedEventArgs : EventArgs
    {
        public bool DoorLocked { get; set; }
    }
}