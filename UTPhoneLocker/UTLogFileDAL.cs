using System;
using NSubstitute;
using NUnit.Framework;
using PhoneLocker;
using PhoneLockerClassLibrary;

namespace UsbSimulator.Test
{
    [TestFixture]
    public class UTLogFileDAL
    {
        private ILogging UUT;
        private DateTime TestDateTime;
        private IStationControl testStationControl;
        private IDoor testdoor;
        private IRFIDReader testRfidReader;
        private IChargeControl testChargeControl;
        private IDisplay testDisplay;

        [SetUp]
        public void Setup()
        {
            UUT = Substitute.For<ILogging>();
            //TestDateTime = new DateTime(2021, 12, 31, 12, 0, 0);
            testdoor = Substitute.For<IDoor>();
            testRfidReader = Substitute.For<IRFIDReader>();
            testChargeControl = Substitute.For<IChargeControl>();
            testDisplay = Substitute.For<IDisplay>();
        }

        [TestCase(12)]
        [TestCase(0)]
        public void Write_WhenPhoneLockerIsAvailableChargerConnected_TimeAndRFIDCorrectLogged(int rfid)
        {
            //ARRANGE
            PhoneLockerState teststate = PhoneLockerState.Available;
            testChargeControl.Connected = true;
            testStationControl = new StationControl(teststate, testdoor, testRfidReader, testChargeControl, UUT, testDisplay);
            string message2write = DateTime.Now.ToString("HH:mm:ss") + ": Skab laast med RFID: " + rfid;
            testStationControl.Rfid = rfid;

            //ACT
            testStationControl.RfidDetected();

            //ASSERT
            UUT.Received(1).Write(message2write);
        }

        [TestCase(12)]
        [TestCase(1)]
        public void Write_WhenPhoneLockerIsLockedChargerConnected_TimeAndRFIDCorrectLogged(int rfid)
        {
            //ARRANGE
            PhoneLockerState teststate = PhoneLockerState.Locked;
            testChargeControl.Connected = true;
            testStationControl = new StationControl(teststate, testdoor, testRfidReader, testChargeControl, UUT, testDisplay);
            testStationControl.Rfid = rfid; //id fra event
            testStationControl._oldId = rfid; //gammelt id fra da man låste skabet
            string message2write = DateTime.Now.ToString("HH:mm:ss") + ": Skab laast op med RFID: " + rfid;


            //ACT
            testStationControl.RfidDetected();

            //ASSERT
            UUT.Received(1).Write(message2write);
        }
    }
}