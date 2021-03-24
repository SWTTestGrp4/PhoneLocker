using System;

namespace PhoneLockerClassLibrary
{
    public class Door: IDoor
    {
        public event EventHandler<DoorLockedEventArgs> DoorLockedEvent;

        public void LockDoor()
        {
            OnDoorOpened(new DoorLockedEventArgs { DoorLocked = true });
        }

        public void UnlockDoor()
        {
            OnDoorOpened(new DoorLockedEventArgs { DoorLocked = false });
        }
        protected virtual void OnDoorOpened(DoorLockedEventArgs e)
        {
            DoorLockedEvent?.Invoke(this, e);
        }

    }
}