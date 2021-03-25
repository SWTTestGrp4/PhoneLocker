using System;
using System.Collections.Generic;
using System.Text;
using PhoneLockerClassLibrary;

namespace PhoneLocker
{
    public interface IChargeControl
    {
        public bool Connected { get; set; }
        public double CurrentCurrent { get; set; }
        public void StartCharge();
        public void StopCharge();
        public void HandleCurrentChangedEvent(object sender, CurrentChangedEventArgs e);
        public IUsbCharger _charger { get; set; }
        public IDisplay _display { get; set; }
    }
}
