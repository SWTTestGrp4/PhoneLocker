namespace PhoneLockerClassLibrary
{
    public interface IStationControl
    {
      public bool DoorLocked { get; set; }
      public int _oldId { get; set; }
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
