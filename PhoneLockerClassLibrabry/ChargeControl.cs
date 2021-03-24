using System;
using System.Collections.Generic;
using System.Text;
using PhoneLocker;

namespace PhoneLockerClassLibrary
{
    public class ChargeControl : IChargeControl
    {
        public bool Connected { get; set; }
        public bool isConnected()
        {
            throw new NotImplementedException();
        }

        public void StartCharge()
        {
            throw new NotImplementedException();
        }

        public void StopCharge()
        {
            throw new NotImplementedException();
        }
    }
}
