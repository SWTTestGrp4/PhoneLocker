﻿using System;
using System.Collections.Generic;
using System.Text;
using PhoneLockerClassLibrary;

namespace PhoneLocker
{
    public interface IStationControl
    {
      public bool DoorLocked { get; set; }
      public int Rfid { get; set; }
      
      public void RfidDetected();
      public IDoor _door { get; set; }
      public PhoneLockerState _state { get; set; }
      public IChargeControl _charger { get; set; }
      public ILogging _logging { get; set; }
      public IDisplay _display { get; set; }
      public IRFIDReader _rfidReader { get; set; }
 
      
    }


}
