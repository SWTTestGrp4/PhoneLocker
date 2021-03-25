using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneLocker
{
    public interface IDisplay
    {
        public void DisplayText(string message);
        public void DisplayCharge(string message);
    }
}
