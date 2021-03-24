using System;
using System.Runtime.CompilerServices;
using PhoneLocker;
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
            PhoneLockerState state = PhoneLockerState.Available;
            IChargeControl chargeControl = new ChargeControl(usbCharger);
            
            IStationControl stationControl = new StationControl(state, door, rfidReader, chargeControl, logging, display);
            #endregion


            bool finish = false;
            do
            {
                string input;
                System.Console.WriteLine("Indtast E, O, C, R: ");
                input = Console.ReadLine();
                if (string.IsNullOrEmpty(input)) continue;


                switch (input[0])
                {
                    case 'E':
                        finish = true;
                        break;

                    case 'O':
                        display.DisplayText("Døren er åben");
                        break;

                    case 'C':
                        display.DisplayText("Døren er lukket");
                        display.DisplayText("Scan venligst RFID");
                        break;

                    case 'R':
                        display.DisplayText("Indtast RFID id: ");
                        string idString = System.Console.ReadLine();

                        int id = Convert.ToInt32(idString);
                        rfidReader.ReadRFID(id);

                        stationControl.RfidDetected();

                        break;

                    default:
                        break;
                }

            } while (!finish);
        }
    }
}
