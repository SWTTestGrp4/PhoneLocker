using System;
using System.Collections.Generic;
using System.Text;
using PhoneLockerClassLibrary;

namespace PhoneLocker
{
    public interface IStationControl
    {
      public bool DoorLocked { get; set; }

      public void RfidDetected();
    }


}
