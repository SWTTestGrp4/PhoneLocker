using NSubstitute;
using NUnit.Framework;
using PhoneLockerClassLibrary;

namespace UsbSimulator.Test
{
    [TestFixture]
    public class UTPhoneLockerState
    {
        private ILogging fakeLogging;
        private IStationControl StationControl;
        private IDoor fakeDoor;
        private IRFIDReader fakeRfidReader;
        private IChargeControl fakeChargeControl;
        private IDisplay fakeDisplay;
        private PhoneLockerState UUT;

        [SetUp]
        public void Setup()
        {
            fakeLogging = Substitute.For<ILogging>();
            fakeDoor = Substitute.For<IDoor>();
            fakeRfidReader = Substitute.For<IRFIDReader>();
            fakeChargeControl = Substitute.For<IChargeControl>();
            fakeDisplay = Substitute.For<IDisplay>();
        }

        [Test]
        public void PhoneLockerStateIsLocked_AfterRfidDetectedAndPhoneConnectedChargingStarted()
        {
            //ARRANGE
            PhoneLockerState teststate = PhoneLockerState.Available;
            StationControl = new StationControl(teststate, fakeDoor, fakeRfidReader, fakeChargeControl, fakeLogging, fakeDisplay);
            fakeChargeControl.Connected = true;

            //ACT
            StationControl.RfidDetected();

            PhoneLockerState desiredState = PhoneLockerState.Locked;
            //ASSERT
            Assert.That(StationControl._state, Is.EqualTo(desiredState));
        }

        [TestCase(1)]
        public void PhoneLockerStateIsAvailable_AfterRfidDetected_IDMatchesOldID(int rfid)
        {
            //ARRANGE
            PhoneLockerState teststate = PhoneLockerState.Locked;
            StationControl = new StationControl(teststate, fakeDoor, fakeRfidReader, fakeChargeControl, fakeLogging, fakeDisplay);
            fakeChargeControl.Connected = true;
            StationControl.Rfid = rfid; //id fra event
            StationControl._oldId = rfid; //gammelt id fra da man låste skabet

            //ACT
            StationControl.RfidDetected();

            PhoneLockerState desiredState = PhoneLockerState.Available;
            //ASSERT
            Assert.That(StationControl._state, Is.EqualTo(desiredState));
        }
    }
}