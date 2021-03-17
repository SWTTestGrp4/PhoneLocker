using System;
using System.Collections.Generic;
using System.Text;
using PhoneLocker;

namespace PhoneLockerClassLibrary
{
    public class Display : IDisplay
    {
        public void DisplayText(string message)
        {
            Console.WriteLine(message);
        }
    }
}
