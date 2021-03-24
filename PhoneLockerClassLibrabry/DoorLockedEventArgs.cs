using System;

namespace PhoneLocker
{
    public class DoorLockedEventArgs: EventArgs
    {
        public bool DoorLocked { get; set; } 
    }
}