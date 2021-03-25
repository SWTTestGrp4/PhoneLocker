using NSubstitute;
using NUnit.Framework;
using PhoneLockerClassLibrary;

namespace UsbSimulator.Test
{
    [TestFixture]
    public class UTDisplay
    {
        private ILogging fakeLogging;
        private IStationControl fakeStationControl;
        private IDoor fakeDoor;
        private IRFIDReader fakeRfidReader;
        private IChargeControl fakeChargeControl;
        private IDisplay UUT;

        [SetUp]
        public void Setup()
        {
            fakeLogging = Substitute.For<ILogging>();
            fakeDoor = Substitute.For<IDoor>();
            fakeRfidReader = Substitute.For<IRFIDReader>();
            fakeChargeControl = Substitute.For<IChargeControl>();
            UUT = Substitute.For<IDisplay>();
        }

        [TestCase("Tilslut telefon")]
        public void DisplayText_WhenPhoneLockerIsAvailable_DisplayTextWritesConnectPhone(string message)
        {
            //ARRANGE
            PhoneLockerState teststate = PhoneLockerState.Available;
            fakeStationControl = new StationControl(teststate, fakeDoor, fakeRfidReader, fakeChargeControl, fakeLogging, UUT);

            //ACT
            fakeStationControl.RfidDetected();

            //ASSERT
            UUT.Received(1).DisplayText(message);
        }

        [TestCase("Brug RFID til at låse skab op.")]
        public void DisplayText_WhenPhoneLockerIsAvailableAndChargerConnectedRfidDetected_DisplayTextWritesUnlockMessage(string message)
        {
            //ARRANGE
            PhoneLockerState teststate = PhoneLockerState.Available;
            fakeChargeControl.Connected = true;
            fakeStationControl = new StationControl(teststate, fakeDoor, fakeRfidReader, fakeChargeControl, fakeLogging, UUT);

            //ACT
            fakeStationControl.RfidDetected();

            //ASSERT
            UUT.Received(1).DisplayText(message);
        }

        [TestCase("Din telefon er ikke ordentlig tilsluttet. Prøv igen.")]
        public void DisplayText_WhenPhoneLockerIsAvailableAndChargerNotConnectedRfidDetected_DisplayTextWritesFailMessage(string message)
        {
            //ARRANGE
            PhoneLockerState teststate = PhoneLockerState.Available;
            fakeChargeControl.Connected = false;
            fakeStationControl = new StationControl(teststate, fakeDoor, fakeRfidReader, fakeChargeControl, fakeLogging, UUT);

            //ACT
            fakeStationControl.RfidDetected();

            //ASSERT
            UUT.Received(1).DisplayText(message);
        }
        [TestCase("Forkert RFID tag",3)]
        public void DisplayText_WhenPhoneLockerIsLockedAndChargerConnectedRfidDetectedButIsWrong_DisplayErrorMessage(string message, int rfid)
        {
            //ARRANGE
            PhoneLockerState teststate = PhoneLockerState.Locked;
            fakeChargeControl.Connected = true;
            fakeStationControl = new StationControl(teststate, fakeDoor, fakeRfidReader, fakeChargeControl, fakeLogging, UUT);
            fakeStationControl.Rfid = rfid; //id fra event ved rfid scanning
            fakeStationControl._oldId = 99; //gammelt id fra da man låste skabet

            //ACT
            fakeStationControl.RfidDetected();

            //ASSERT
            UUT.Received(1).DisplayText(message);
        }

        [TestCase("Skabet er nu optaget og opladning påbegyndes.")]
        public void DisplayCharge_WhenPhoneLockerIsAvailableAndChargerConnectedRfidDetected_DisplayChargeStartedAndLockerOccupied(string message)
        {
            //ARRANGE
            PhoneLockerState teststate = PhoneLockerState.Available;
            fakeChargeControl.Connected = true;
            fakeStationControl = new StationControl(teststate, fakeDoor, fakeRfidReader, fakeChargeControl, fakeLogging, UUT);

            //ACT
            fakeStationControl.RfidDetected();

            //ASSERT
            UUT.Received(1).DisplayCharge(message);
        }


        [Test]
        public void DisplayCharge_WhenPhoneLockerIsAvailableAndChargerNotConnectedRfidDetected_DisplayChargeDoesNotReceiveCall()
        {
            //ARRANGE
            PhoneLockerState teststate = PhoneLockerState.Available;
            fakeChargeControl.Connected = false;
            fakeStationControl = new StationControl(teststate, fakeDoor, fakeRfidReader, fakeChargeControl, fakeLogging, UUT);

            //ACT
            fakeStationControl.RfidDetected();

            //ASSERT
            UUT.Received(0).DisplayCharge("");
        }

    }
}