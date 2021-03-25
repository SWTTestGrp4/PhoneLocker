using System;

namespace PhoneLockerClassLibrary
{
    public class Display : IDisplay
    {
        public void DisplayText(string message)
        {
            Console.WriteLine(message);
        }

        public void DisplayCharge(string message)
        {
            Console.WriteLine("\t\t\t"+message);
        }
    }
}
