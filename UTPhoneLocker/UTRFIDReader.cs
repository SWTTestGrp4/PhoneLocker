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

        [TestCase(-12),
         TestCase(0),
         TestCase(12)]
        public void OnRFIDDetected_ReadRFIDIDid_EventRFIDValueEqualsid(int id)
        {
            _uut.RFIDDetectedEvent += (o, args) => { _receivedArgs = args; };
            _uut.ReadRFID(id);
            Assert.That(_receivedArgs.RFID, Is.EqualTo(id));
        }

    }
}