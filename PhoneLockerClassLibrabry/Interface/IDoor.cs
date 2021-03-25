using System;

namespace PhoneLockerClassLibrary
{
    public interface IDoor
    {
       public event EventHandler<DoorLockedEventArgs> DoorLockedEvent;
        public void LockDoor();

        public void UnlockDoor();

        public void OnDoorOpened(DoorLockedEventArgs e);



    }
}