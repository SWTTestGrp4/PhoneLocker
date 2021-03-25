using System;
using NSubstitute;
using NUnit.Framework;
using PhoneLockerClassLibrary;

namespace UsbSimulator.Test
{
    [TestFixture]
    public class UTLogFileDAL
    {
        private ILogging UUT;
        private IStationControl fakeStationControl;
        private IDoor fakeDoor;
        private IRFIDReader fakeRfidReader;
        private IChargeControl fakeChargeControl;
        private IDisplay fakeDisplay;

        [SetUp]
        public void Setup()
        {
            UUT = Substitute.For<ILogging>();
            fakeDoor = Substitute.For<IDoor>();
            fakeRfidReader = Substitute.For<IRFIDReader>();
            fakeChargeControl = Substitute.For<IChargeControl>();
            fakeDisplay = Substitute.For<IDisplay>();
        }

        [TestCase(12)]
        [TestCase(0)]
        public void Write_WhenPhoneLockerIsAvailableChargerConnectedRfidDetected_TimeAndRFIDCorrectLogged(int rfid)
        {
            //ARRANGE
            PhoneLockerState teststate = PhoneLockerState.Available;
            fakeChargeControl.Connected = true;
            fakeStationControl = new StationControl(teststate, fakeDoor, fakeRfidReader, fakeChargeControl, UUT, fakeDisplay);
            string message2write = DateTime.Now.ToString("HH:mm:ss") + ": Skab laast med RFID: " + rfid;
            fakeStationControl.Rfid = rfid;

            //ACT
            fakeStationControl.RfidDetected();

            //ASSERT
            UUT.Received(1).Write(message2write);
        }

        [TestCase(12)]
        [TestCase(1)]
        public void Write_WhenPhoneLockerIsLockedChargerConnectedRfidDetected_TimeAndRFIDCorrectLogged(int rfid)
        {
            //ARRANGE
            PhoneLockerState teststate = PhoneLockerState.Locked;
            fakeChargeControl.Connected = true;
            fakeStationControl = new StationControl(teststate, fakeDoor, fakeRfidReader, fakeChargeControl, UUT, fakeDisplay);
            fakeStationControl.Rfid = rfid; //id fra event
            fakeStationControl._oldId = rfid; //gammelt id fra da man låste skabet
            string message2write = DateTime.Now.ToString("HH:mm:ss") + ": Skab laast op med RFID: " + rfid;


            //ACT
            fakeStationControl.RfidDetected();

            //ASSERT
            UUT.Received(1).Write(message2write);
        }
    }
}