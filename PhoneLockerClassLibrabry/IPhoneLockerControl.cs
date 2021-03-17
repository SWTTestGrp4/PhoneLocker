using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneLocker
{
    public interface IPhoneLockerControl
    {
        public void StartCharge();
        public void StopCharge();
    }
}
