using System;

namespace PhoneLockerClassLibrary
{
    public class StationControl: IStationControl
    {
        #region instantiering af objekter
        public int oldId { get; set; }
        public bool DoorLocked { get; set; }
        public int Rfid { get; set; }

        public IDoor Door { get; set; }
        public PhoneLockerState State { get; set; }
        public IChargeControl Charger { get; set; }
        public ILogging Logging { get; set; }
        public IDisplay Display { get; set; }
        private IRFIDReader _rfidReader;


        #endregion

        public StationControl(IDoor door, IRFIDReader rfidReader, IChargeControl charger, ILogging logging, IDisplay display)
        {
            Charger = charger;
            Logging = logging;
            Display = display;
            Door = door;
            _rfidReader = rfidReader;
            door.DoorLockedEvent += HandleDoorLockedEvent;
            rfidReader.RFIDDetectedEvent += HandleRfidDetectedEvent;
        }


      

        private void HandleDoorLockedEvent(object sender, DoorLockedEventArgs e)
        {
            DoorLocked = e.DoorLocked;
        }


        private void HandleRfidDetectedEvent(object sender,RFIDDetectedEventArgs e)
        {
            Rfid = e.RFID;
            RfidDetected();
        }


        private void RfidDetected()
        {
            int id;
            id = Rfid;
            switch (State)
            {
                case PhoneLockerState.Available:
                    //DoorLocked = false;

                    if (Charger.Connected)
                    {
                        Door.LockDoor();
                        Logging.Write(DateTime.Now.ToString("HH:mm:ss") + ": Skab laast med RFID: " + id);
                        Charger.StartCharge();
                        oldId = id;
                        Display.DisplayText("Brug RFID til at låse skab op.");
                        Display.DisplayCharge("Skabet er nu optaget og opladning påbegyndes.");
                        State = PhoneLockerState.Locked;
                    }
                    else
                    {
                        Display.DisplayText("Din telefon er ikke ordentlig tilsluttet. Prøv igen.");
                    }

                    break;

                case PhoneLockerState.DoorOpen:
                    // Ignore
                    break;

                case PhoneLockerState.Locked:
                    // Check for correct ID
                    if (id == oldId)
                    {
                        Charger.StopCharge();
                        Door.UnlockDoor();
                        Logging.Write(DateTime.Now.ToString("HH:mm:ss") + ": Skab laast op med RFID: " + id);

                        Display.DisplayText("Tag din telefon ud af skabet og luk døren");
                        Display.DisplayText("Skabet er nu ledigt");
                        State = PhoneLockerState.Available;
                    }
                    else
                    {
                        Display.DisplayText("Forkert RFID tag");
                    }

                    break;
            }
        }


    }
}
