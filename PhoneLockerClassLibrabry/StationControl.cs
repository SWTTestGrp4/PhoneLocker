using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneLocker;
using PhoneLockerClassLibrary;

namespace PhoneLocker
{
    public class StationControl
    {
        // Enum med tilstande ("states") svarende til tilstandsdiagrammet for klassen
        public enum PhoneLockerState
        {
            Available,
            Locked,
            DoorOpen
        };
          
        #region instantiering af objekter
        private PhoneLockerState _state;
        private IChargeControl _charger;
        private int _oldId;
        private IDoor _door;
        private ILogging _logging;
        private Display _display;


        private string logFile = "logfile.txt"; // Navnet på systemets log-fil

        public event EventHandler<DoorLockedEventArgs> DoorLockedEvent;
        public event EventHandler<RFIDDetectedEventArgs> RfidDetectedEvent;
        public bool DoorLocked { get; set; }
        public IRFIDReader RfidReader { get; set; }
        public bool rfidDetected { get; set; }

        #endregion

        public StationControl(PhoneLockerState state, IDoor door, IRFIDReader rfidReader, IChargeControl charger, ILogging logging, IDisplay display)
        {
            _state = state;
            _charger = charger;
            _logging = logging;
            _display = display;
            door.DoorLockedEvent += HandleDoorLockedEvent;
            rfidReader.RFIDDetectedEvent += HandleRfidDetectedEvent;
        }


      

        private void HandleDoorLockedEvent(object sender, DoorLockedEventArgs e)
        {
            DoorLocked = e.DoorLocked;
        }


        private void HandleRfidDetectedEvent(object sender,RFIDDetectedEventArgs e)
        {
            rfidDetected = e.RFIDDetected;
        }


        private void RfidDetected(int id)
        {
            switch (_state)
            {
                case PhoneLockerState.Available:
                    // Check for ladeforbindelse
                    DoorLocked = false;
                    _display.DisplayText("Tilslut telefon");
                    if (_charger.isConnected())
                    {
                        _door.LockDoor();
                        _charger.StartCharge();
                        _oldId = id;
                        _logging.Write((DateTime.Now + ": Skab låst med RFID: {0}", id).ToString());
                        

                        _display.DisplayText("Skabet er låst og din telefon lades. Brug dit RFID tag til at låse op.");
                        _state = PhoneLockerState.Locked;
                    }
                    else
                    {
                        _display.DisplayText("Din telefon er ikke ordentlig tilsluttet. Prøv igen.");
                    }

                    break;

                case PhoneLockerState.DoorOpen:
                    // Ignore
                    break;

                case PhoneLockerState.Locked:
                    // Check for correct ID
                    if (id == _oldId)
                    {
                        _charger.StopCharge();
                        _door.UnlockDoor();
                        _logging.Write((DateTime.Now + ": Skab låst op med RFID: {0}", id).ToString());


                        _display.DisplayText("Tag din telefon ud af skabet og luk døren");
                        _state = PhoneLockerState.Available;
                    }
                    else
                    {
                        _display.DisplayText("Forkert RFID tag");
                    }

                    break;
            }
        }
    }
}
