using NUnit.Framework;
using PhoneLocker;
using PhoneLockerClassLibrary;

namespace UsbSimulator.Test
{
    [TestFixture]
    public class UTRFIDReader
    {
        private IRFIDReader _uut;
        private RFIDDetectedEventArgs _receivedArgs;

        [SetUp]
        public void Setup()
        {
            _uut = new RFIDReader();
        }

        [Test]
        public void OnRFIDDetected_ReceiveID12_InvokeEventWithValue12()
        {
            _uut.RFIDDetectedEvent += (o, args) => { _receivedArgs = args; };
        }
    }
}