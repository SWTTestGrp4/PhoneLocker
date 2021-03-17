using System;

namespace PhoneLockerClassLibrary
{
    public class CurrentChangedEventArgs : EventArgs
    {
        public double CurrentCurrent { get; set; }
    }
}