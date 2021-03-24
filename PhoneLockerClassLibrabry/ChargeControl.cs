using System;
using System.Collections.Generic;
using System.Text;
using PhoneLocker;

namespace PhoneLockerClassLibrary
{
    public class ChargeControl : IChargeControl
    {
        public bool Connected { get; set; }
        private IUsbCharger _charger;
        private IStationControl _stationControl;
        public double CurrentCurrent { get; set; }

        public ChargeControl(IUsbCharger charger, IStationControl stationControl)
        {
            _charger = charger;
            _stationControl = stationControl;
            _charger.CurrentEventArgs += HandleCurrentChangedEvent;
        }
        public bool isConnected()
        {
            Connected = _charger.Connected;
            return Connected;
        }
        public void StartCharge()
        {
            _charger.StartCharge();
        }

        public void StopCharge()
        {
            _charger.StopCharge();
        }

        public void HandleCurrentChangedEvent(object sender, CurrentChangedEventArgs e)
        {
            CurrentCurrent = e.CurrentCurrent;
            //do something with the current current??
            if (!_stationControl.DoorLocked)
            {
                if (isConnected())
                {
                    if (CurrentCurrent == 0)
                    {
                        //Der er ingen forbindelse til en telefon, eller ladning er ikke startet. Displayet viser ikke noget om ladning
                    }
                    else if (CurrentCurrent > 0 && CurrentCurrent <= 5)
                    {
                        StopCharge();
                        Console.WriteLine("Telefonen er fuldt opladet");
                    }
                    else if (CurrentCurrent >5 && CurrentCurrent <=500)
                    {
                        StartCharge();
                        Console.WriteLine("Opladning igang");
                    }
                    else
                    {
                        StopCharge();
                        Console.WriteLine("Der er noget galt! Afbryder straks opladning");
                    }
                }
            }
        }
    }
}
