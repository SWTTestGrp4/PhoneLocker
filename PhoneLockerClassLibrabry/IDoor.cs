using System;
using PhoneLocker;

namespace PhoneLockerClassLibrary
{
    public interface IDoor
    {
        public event EventHandler<DoorOpenedEventArgs> doorOpenedEvent;
        public void LockDoor();

        public void UnlockDoor();

        //Skal denne metode impl. her jævnfør vores diagram, eller i IStationControl jævnfør Marc/Mads?
        protected virtual void OnDoorOpened(DoorOpenedEventArgs e)
        {
            //DoorOpenedEvent
        }
    }
}