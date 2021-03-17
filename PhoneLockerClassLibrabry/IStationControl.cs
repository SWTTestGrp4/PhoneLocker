using System;
using System.Collections.Generic;
using System.Text;
using PhoneLockerClassLibrary;

namespace PhoneLocker
{
    public interface IStationControl
    {
        public Enum PhoneLockerState();
        public event EventHandler<DoorOpenedEventArgs> DoorOpenedEvent;
        //public event EventHandler<RFIDDetectedEventArgs> RfidDetectedEvent;
        public IDoor Door { get; set; }
        public IRFIDReader RfidReader { get; set; }

        public bool DoorOpened();
        public bool RFIDDetected(int ID);
        public bool CheckId(int OldId, int Id);
        //HandleDoorOpenedEvent

        //Skal denne metode impl. her jævnfør Marc/Mads, eller i IDoor, jævnfør vores diagram?
        //protected virtual void OnDoorOpened(DoorOpenedEventArgs e)
        //{
        //    //DoorOpenedEvent
        //}


    }

    
}
