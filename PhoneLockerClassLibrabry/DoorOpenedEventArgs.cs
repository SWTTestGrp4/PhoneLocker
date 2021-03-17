using System;

namespace PhoneLocker
{
    public class DoorOpenedEventArgs: EventArgs
    {
        public bool DoorOpen { get; set; } 
    }
}