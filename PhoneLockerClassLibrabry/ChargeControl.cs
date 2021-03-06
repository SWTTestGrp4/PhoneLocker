﻿namespace PhoneLockerClassLibrary
{
    public class ChargeControl : IChargeControl
    {
        public bool Connected
        {
            get { return _charger.Connected; }
            set { _charger.SimulateConnected(value); }
        }

        public double CurrentCurrent { get; set; }
        public IUsbCharger _charger { get; set; }
        public IDisplay _display { get; set; }

        public ChargeControl(IUsbCharger charger)
        {
            _charger = charger;
            _display = new Display();
            _charger.CurrentEventArgs += HandleCurrentChangedEvent;
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

            if (CurrentCurrent == 0)
            {
                //Der er ingen forbindelse til en telefon, eller ladning er ikke startet. Displayet viser ikke noget om ladning
            }
            else if (CurrentCurrent > 0 && CurrentCurrent <= 5)
            {
                StopCharge();
                _display.DisplayCharge("Telefonen er fuldt opladet");
            }
            else if (CurrentCurrent > 5 && CurrentCurrent <= 500)
            {
                StartCharge();
                _display.DisplayCharge("Opladning igang");
            }
            else
            {
                StopCharge();
                _display.DisplayCharge("Der er noget galt! Afbryder straks opladning");
            }
        }

    }
}

