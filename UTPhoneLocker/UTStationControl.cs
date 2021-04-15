using System;
using NSubstitute;
using NUnit.Framework;
using PhoneLockerClassLibrary;

namespace UTPhoneLocker
{
    [TestFixture]
    public class UTStationControl
    {
        private ILogging fakeLogging;
        private IStationControl UUT;
        private IDoor fakeDoor;
        private IRFIDReader fakeRfidReader;
        private IChargeControl fakeChargeControl;
        private IDisplay fakeDisplay;

        [SetUp]
        public void Setup()
        {
            fakeLogging = Substitute.For<ILogging>();
            fakeDoor = Substitute.For<IDoor>();
            fakeRfidReader = Substitute.For<IRFIDReader>();
            fakeChargeControl = Substitute.For<IChargeControl>();
            fakeDisplay = Substitute.For<IDisplay>();
            fakeRfidReader = Substitute.For<RFIDReader>();
        }

        [TestCase("Brug RFID til at låse skab op.")]
        public void DisplayText_WhenPhoneLockerIsAvailableAndChargerConnectedRfidDetected_DisplayTextWritesUnlockMessage(string message)
        {
            //ARRANGE
            PhoneLockerState teststate = PhoneLockerState.Available;
            fakeChargeControl.Connected = true;
            UUT = new StationControl(teststate, fakeDoor, fakeRfidReader, fakeChargeControl, fakeLogging, fakeDisplay);

            //ACT
            fakeRfidReader.ReadRFID(0);

            //ASSERT
            fakeDisplay.Received(1).DisplayText(message);
        }

        [TestCase("Din telefon er ikke ordentlig tilsluttet. Prøv igen.")]
        public void DisplayText_WhenPhoneLockerIsAvailableAndChargerNotConnectedRfidDetected_DisplayTextWritesFailMessage(string message)
        {
            //ARRANGE
            PhoneLockerState teststate = PhoneLockerState.Available;
            fakeChargeControl.Connected = false;
            UUT = new StationControl(teststate, fakeDoor, fakeRfidReader, fakeChargeControl, fakeLogging, fakeDisplay);

            //ACT
            fakeRfidReader.ReadRFID(0);

            //ASSERT
            fakeDisplay.Received(1).DisplayText(message);
        }
        [TestCase("Forkert RFID tag", 3)]
        public void DisplayText_WhenPhoneLockerIsLockedAndChargerConnectedRfidDetectedButIsWrong_DisplayErrorMessage(string message, int rfid)
        {
            //ARRANGE
            PhoneLockerState teststate = PhoneLockerState.Locked;
            fakeChargeControl.Connected = true;
            UUT = new StationControl(teststate, fakeDoor, fakeRfidReader, fakeChargeControl, fakeLogging, fakeDisplay);
            UUT.Rfid = rfid; //id fra event ved rfid scanning
            UUT._oldId = 99; //gammelt id fra da man låste skabet

            //ACT
            fakeRfidReader.ReadRFID(0);

            //ASSERT
            fakeDisplay.Received(1).DisplayText(message);
        }

        [TestCase("Skabet er nu optaget og opladning påbegyndes.")]
        public void DisplayCharge_WhenPhoneLockerIsAvailableAndChargerConnectedRfidDetected_DisplayChargeStartedAndLockerOccupied(string message)
        {
            //ARRANGE
            PhoneLockerState teststate = PhoneLockerState.Available;
            fakeChargeControl.Connected = true;
            UUT = new StationControl(teststate, fakeDoor, fakeRfidReader, fakeChargeControl, fakeLogging, fakeDisplay);

            //ACT
            fakeRfidReader.ReadRFID(0);

            //ASSERT
            fakeDisplay.Received(1).DisplayCharge(message);
        }


        [Test]
        public void DisplayCharge_WhenPhoneLockerIsAvailableAndChargerNotConnectedRfidDetected_DisplayChargeDoesNotReceiveCall()
        {
            //ARRANGE
            PhoneLockerState teststate = PhoneLockerState.Available;
            fakeChargeControl.Connected = false;
            UUT = new StationControl(teststate, fakeDoor, fakeRfidReader, fakeChargeControl, fakeLogging, fakeDisplay);

            //ACT
            fakeRfidReader.ReadRFID(0);

            //ASSERT
            fakeDisplay.Received(0).DisplayCharge("");
        }

        [Test]
        public void StartCharge_WhenPhoneLockerIsAvailableChargerConnectedRfidDetected_CallReceived()
        {
            //ARRANGE
            PhoneLockerState teststate = PhoneLockerState.Available;
            UUT = new StationControl(teststate, fakeDoor, fakeRfidReader, fakeChargeControl, fakeLogging, fakeDisplay);
            fakeChargeControl.Connected = true;

            //ACT
            fakeRfidReader.ReadRFID(0);

            //ASSERT
            fakeChargeControl.Received(1).StartCharge();
        }

        [TestCase(1)]
        public void StopCharge_WhenPhoneLockerIsLockedRfidDetected_RFIDIsCorrect_CallReceived(int rfid)
        {
            //ARRANGE
            PhoneLockerState teststate = PhoneLockerState.Locked;
            UUT = new StationControl(teststate, fakeDoor, fakeRfidReader, fakeChargeControl, fakeLogging, fakeDisplay);
            UUT.Rfid = rfid; //id fra event
            UUT._oldId = rfid; //gammelt id fra da man låste skabet

            //ACT
            fakeRfidReader.ReadRFID(rfid);

            //ASSERT
            fakeChargeControl.Received(1).StopCharge();
        }
        [TestCase(1)]
        public void StopCharge_WhenPhoneLockerIsLockedRfidDetected_RFIDIsWrong_CallNotReceived(int rfid)
        {
            //ARRANGE
            PhoneLockerState teststate = PhoneLockerState.Locked;
            UUT = new StationControl(teststate, fakeDoor, fakeRfidReader, fakeChargeControl, fakeLogging, fakeDisplay);
            UUT.Rfid = rfid; //forkert id fra event
            UUT._oldId = 99; //korrekt id fra vedkommende der låste skabet

            //ACT
            fakeRfidReader.ReadRFID(0);

            //ASSERT
            fakeChargeControl.DidNotReceive().StopCharge();
        }

        [TestCase(12)]
        [TestCase(0)]
        public void Write_WhenPhoneLockerIsAvailableChargerConnectedRfidDetected_TimeAndRFIDCorrectLogged(int rfid)
        {
            //ARRANGE
            PhoneLockerState teststate = PhoneLockerState.Available;
            fakeChargeControl.Connected = true;
            UUT = new StationControl(teststate, fakeDoor, fakeRfidReader, fakeChargeControl, fakeLogging, fakeDisplay);
            string message2write = DateTime.Now.ToString("HH:mm:ss") + ": Skab laast med RFID: " + rfid;
            UUT.Rfid = rfid;

            //ACT
            fakeRfidReader.ReadRFID(rfid);

            //ASSERT
            fakeLogging.Received(1).Write(message2write);
        }
        [Test]
        public void LockDoor_WhenPhoneLockerIsAvailableChargerConnectedRfidDetected_CallReceived()
        {
            //ARRANGE
            PhoneLockerState teststate = PhoneLockerState.Available;
            UUT = new StationControl(teststate, fakeDoor, fakeRfidReader, fakeChargeControl, fakeLogging, fakeDisplay);
            fakeChargeControl.Connected = true;

            //ACT
            fakeRfidReader.ReadRFID(0);

            //ASSERT
            fakeDoor.Received(1).LockDoor();
        }
        [Test]
        public void LockDoor_WhenPhoneLockerIsAvailableAndChargerNotConnectedRfidDetected_CallNotReceived()
        {
            //ARRANGE
            PhoneLockerState teststate = PhoneLockerState.Available;
            UUT = new StationControl(teststate, fakeDoor, fakeRfidReader, fakeChargeControl, fakeLogging, fakeDisplay);
            fakeChargeControl.Connected = false;

            //ACT
            fakeRfidReader.ReadRFID(0);

            //ASSERT
            fakeDoor.DidNotReceive().LockDoor();
        }

        [TestCase(12)]
        [TestCase(1)]
        public void Write_WhenPhoneLockerIsLockedChargerConnectedRfidDetected_TimeAndRFIDCorrectLogged(int rfid)
        {
            //ARRANGE
            PhoneLockerState teststate = PhoneLockerState.Locked;
            fakeChargeControl.Connected = true;
            UUT = new StationControl(teststate, fakeDoor, fakeRfidReader, fakeChargeControl, fakeLogging, fakeDisplay);
            UUT.Rfid = rfid; //id fra event
            UUT._oldId = rfid; //gammelt id fra da man låste skabet
            string message2write = DateTime.Now.ToString("HH:mm:ss") + ": Skab laast op med RFID: " + rfid;


            //ACT
            fakeRfidReader.ReadRFID(rfid);

            //ASSERT
            fakeLogging.Received(1).Write(message2write);
        }

        [TestCase(1)]
        public void UnlockDoor_RFIDIsCorrect_WhenPhoneLockerIsLockedChargerConnectedRfidDetected_CallReceived(int rfid)
        {
            //ARRANGE
            PhoneLockerState teststate = PhoneLockerState.Locked;
            UUT = new StationControl(teststate, fakeDoor, fakeRfidReader, fakeChargeControl, fakeLogging, fakeDisplay);
            UUT.Rfid = rfid; //id fra event
            UUT._oldId = rfid; //gammelt id fra da man låste skabet

            //ACT
            fakeRfidReader.ReadRFID(rfid);
            //ASSERT
            fakeDoor.Received(1).UnlockDoor();
        }

        [TestCase(1)]
        public void UnlockDoor_WhenRFIDIsNotCorrect_PhoneLockerIsLockedChargerConnectedRfidDetected_CallNotReceived(int rfid)
        {
            //ARRANGE
            PhoneLockerState teststate = PhoneLockerState.Locked;
            UUT = new StationControl(teststate, fakeDoor, fakeRfidReader, fakeChargeControl, fakeLogging, fakeDisplay);
            UUT.Rfid = rfid; //id fra event
            UUT._oldId = 99; //gammelt id fra da man låste skabet

            //ACT
            fakeRfidReader.ReadRFID(0);
            //ASSERT
            fakeDoor.DidNotReceive().UnlockDoor();
        }

        [Test]
        public void PhoneLockerStateIsLocked_AfterRfidDetectedAndPhoneConnectedChargingStarted()
        {
            //ARRANGE
            PhoneLockerState teststate = PhoneLockerState.Available;
            UUT = new StationControl(teststate, fakeDoor, fakeRfidReader, fakeChargeControl, fakeLogging, fakeDisplay);
            fakeChargeControl.Connected = true;

            //ACT
            fakeRfidReader.ReadRFID(0);

            PhoneLockerState desiredState = PhoneLockerState.Locked;
            //ASSERT
            Assert.That(UUT._state, Is.EqualTo(desiredState));
        }

        [TestCase(1)]
        public void PhoneLockerStateIsAvailable_AfterRfidDetected_IDMatchesOldID(int rfid)
        {
            //ARRANGE
            PhoneLockerState teststate = PhoneLockerState.Locked;
            UUT = new StationControl(teststate, fakeDoor, fakeRfidReader, fakeChargeControl, fakeLogging, fakeDisplay);
            fakeChargeControl.Connected = true;
            UUT.Rfid = rfid; //id fra event
            UUT._oldId = rfid; //gammelt id fra da man låste skabet

            //ACT
            fakeRfidReader.ReadRFID(rfid);

            PhoneLockerState desiredState = PhoneLockerState.Available;
            //ASSERT
            Assert.That(UUT._state, Is.EqualTo(desiredState));
        }
        [TestCase(-10),
         TestCase(0),
         TestCase(1),
         TestCase(200)]
        public void HandleRfidDetectedEvent_AfterRfidDetected_IDMatchesOldID(int rfid)
        {
            //ARRANGE
            PhoneLockerState teststate = PhoneLockerState.Locked;
            UUT = new StationControl(teststate, fakeDoor, fakeRfidReader, fakeChargeControl, fakeLogging, fakeDisplay);

            //ACT
            //fakeRfidReader.RFIDDetectedEvent += Raise.EventWith(new RFIDDetectedEventArgs() { RFID = rfid });
            fakeRfidReader.ReadRFID(rfid);

            //ASSERT
            Assert.That(UUT.Rfid, Is.EqualTo(rfid));
        }

        [TestCase(true),
         TestCase(false)]
        public void HandleDoorLockedEvent_DoorOpenAndClosedInserted_DoorStatusInUUTIsSet(bool door)
        {
            //ARRANGE
            PhoneLockerState teststate = PhoneLockerState.Available;
            UUT = new StationControl(teststate, fakeDoor, fakeRfidReader, fakeChargeControl, fakeLogging, fakeDisplay);

            //ACT
            fakeDoor.DoorLockedEvent += Raise.EventWith(new DoorLockedEventArgs() { DoorLocked = door });

            //ASSERT
            Assert.That(UUT.DoorLocked, Is.EqualTo(door));
        }
    }
}