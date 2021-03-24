using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneLocker
{
    public interface IChargeControl
    {
        public bool Connected { get; set; }
        //Event CurrentChangedEvent

        public bool isConnected();
        public void StartCharge();
        public void StopCharge();
        //private HandleCurrentChangedEvent(string)


    }
}
