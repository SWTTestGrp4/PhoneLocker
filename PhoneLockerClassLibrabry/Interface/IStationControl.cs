namespace PhoneLockerClassLibrary
{
    public interface IStationControl
    {
      bool DoorLocked { get; set; }
      public int oldId { get; set; }
      public int Rfid { get; set; }
      public IDoor Door { get; set; }
      public PhoneLockerState State { get; set; }
      public IChargeControl Charger { get; set; }
      public ILogging Logging { get; set; }
      public IDisplay Display { get; set; }

    }


}
