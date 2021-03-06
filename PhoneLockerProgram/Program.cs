﻿using System;
using PhoneLockerClassLibrary;


namespace PhoneLockerProgram
{
    class Program
    {

        static void Main(string[] args)
        {
            #region Instantiering af dependencies
            IUsbCharger usbCharger = new UsbCharger();
            IDisplay display = new Display();
            IDoor door = new Door();
            ILogging logging = new LogFileDAL();
            IRFIDReader rfidReader = new RFIDReader();

            IChargeControl chargeControl = new ChargeControl(usbCharger);

            IStationControl stationControl = new StationControl(door, rfidReader, chargeControl, logging, display);
            #endregion


            bool finish = false;
            do
            {
                string input;
                System.Console.WriteLine("Indtast E (exit), O (open), C (close), R (rfid), P (phone): ");
                input = Console.ReadLine();
                if (string.IsNullOrEmpty(input)) continue;


                switch (input[0])
                {
                    case 'E':
                    case 'e':
                        finish = true;
                        break;

                    case 'O':
                    case 'o':
                        if (stationControl.State == PhoneLockerState.DoorOpen)
                        {
                            display.DisplayText("Ladedøren er allerede åben. Frakobl eller tilslut mobil.");
                            break;
                        }
                        else
                        {
                            stationControl.State = PhoneLockerState.DoorOpen;
                            display.DisplayText("Døren er åben");
                            display.DisplayText("Tilslut telefon");
                        }
                        break;

                    case 'C':
                    case 'c':
                        if (stationControl.State != PhoneLockerState.DoorOpen)
                        {
                            break;
                        }
                        stationControl.State = PhoneLockerState.Available;
                        display.DisplayText("Døren er lukket");
                        display.DisplayText("Scan venligst RFID");
                        break;

                    case 'R':
                    case 'r':
                        Console.Clear();
                        display.DisplayText("Indtast RFID id: ");
                        string idString = System.Console.ReadLine();

                        int id = Convert.ToInt32(idString);
                        rfidReader.ReadRFID(id);

                        break;
                    case 'P':
                    case 'p':
                        if (stationControl.State != PhoneLockerState.DoorOpen)
                        {
                            display.DisplayText("Døren er lukket. Åben før der kan tilsluttes.");
                            break;
                        }
                        if (chargeControl.Connected)
                        {
                            chargeControl.Connected = false;
                            display.DisplayText("Telefon frakoblet");
                        }
                        else
                        {
                            chargeControl.Connected = true;
                            display.DisplayText("Telefon tilsluttet");
                        }
                        break;
                    default:
                        break;
                }

            } while (!finish);
        }
    }
}
