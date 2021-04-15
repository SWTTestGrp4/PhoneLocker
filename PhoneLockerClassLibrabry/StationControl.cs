﻿using System;

namespace PhoneLockerClassLibrary
{
    public class StationControl: IStationControl
    {
        #region instantiering af objekter
        public int _oldId { get; set; }
        public bool DoorLocked { get; set; }
        public int Rfid { get; set; }

        public IDoor _door { get; set; }
        public PhoneLockerState _state { get; set; }
        public IChargeControl _charger { get; set; }
        public ILogging _logging { get; set; }
        public IDisplay _display { get; set; }
        private IRFIDReader _rfidReader;

        public IRFIDReader RfidReader
        {
            set
            {
                _rfidReader = value;
            }
        }

        #endregion

        public StationControl(PhoneLockerState state, IDoor door, IRFIDReader rfidReader, IChargeControl charger, ILogging logging, IDisplay display)
        {
            _state = state;
            _charger = charger;
            _logging = logging;
            _display = display;
            _door = door;
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
            switch (_state)
            {
                case PhoneLockerState.Available:
                    //DoorLocked = false;

                    if (_charger.Connected)
                    {
                        _door.LockDoor();
                        _logging.Write(DateTime.Now.ToString("HH:mm:ss") + ": Skab laast med RFID: " + id);
                        _charger.StartCharge();
                        _oldId = id;
                        _display.DisplayText("Brug RFID til at låse skab op.");
                        _display.DisplayCharge("Skabet er nu optaget og opladning påbegyndes.");
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
                        _logging.Write(DateTime.Now.ToString("HH:mm:ss") + ": Skab laast op med RFID: " + id);

                        _display.DisplayText("Tag din telefon ud af skabet og luk døren");
                        _display.DisplayText("Skabet er nu ledigt");
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
