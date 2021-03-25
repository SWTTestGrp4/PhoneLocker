using NUnit.Framework;
using PhoneLockerClassLibrary;
using NSubstitute;
namespace UTPhoneLocker
{
    [TestFixture]
    public class UTDoor
    {
        private Door _uut;
        private DoorLockedEventArgs _receivedEventArgs;
        private IStationControl _uutControl;
        private IDoor _sourceDoor;
        

        [SetUp]
        public void Setup()
        {
            _receivedEventArgs = null;
            _uut = new Door();

            //Event listener
            _uut.DoorLockedEvent +=
                (o, args) =>
                {
                    _receivedEventArgs = args;
                };

            _sourceDoor = new Door();
            _uutControl = new FakeStationControl(_sourceDoor);


        }

        [Test]
        public void LockDoor_DoorEventSetToNewValue_EventFired()
        {
           _uut.LockDoor();
           Assert.That(_receivedEventArgs, Is.Not.Null);
        }

        [Test]
        public void LockDoor_DoorEventSetToNewValue_CorrectReceiveFromEvent()
        {
            _uut.LockDoor();
            Assert.That(_receivedEventArgs.DoorLocked, Is.EqualTo(true));
        }

        [Test]
        public void UnlockDoor_DoorEventSetToNewValue_EventFired()
        {
            _uut.UnlockDoor();
            Assert.That(_receivedEventArgs, Is.Not.Null);
        }

        [Test]
        public void UnlockDoor_DoorEventSetToNewValue_CorrectReceiveFromEvent()
        {
            _uut.UnlockDoor();
            Assert.That(_receivedEventArgs.DoorLocked, Is.EqualTo(false));
        }

        [Test]
        public void SetDoorLocked_DoorLockSetToNewParameter()
        {
            _uut.DoorLocked = true;
            Assert.That(_uut.DoorLocked, Is.EqualTo(true));
        }

        [Test]
        public void GetDoorLocked_NewParameter_ParameterSetToDoorLocked()
        {
            _uut.DoorLocked = true;
            bool newVal = _uut.DoorLocked;
            Assert.That(_uut.DoorLocked, Is.EqualTo(newVal));
        }
        /*
        [Test]
        public void OnDoorOpened_DoorHandlerTest_TestSuccess()
        {
            _receivedEventArgs = new DoorLockedEventArgs() {DoorLocked = true };
            _uut.OnDoorOpened(_receivedEventArgs);
            Assert.That(_uut.DoorLocked, Is.EqualTo(true));

        }
        */
        /*
        [TestCase(true)]
        public void StationControlDoorEvent_DifferentArguments_DoorBoolIsTrue(bool locked)
        {
            DoorLockedEventArgs doorLockedEvent = new DoorLockedEventArgs() {DoorLocked = true};
            //This class tests the stationsControl by using Nsubstitute. It injects the source into the receiver to make sure that the event gets triggered on the right place
            _sourceDoor.DoorLockedEvent += Raise.EventWith(doorLockedEvent);
            _sourceDoor.OnDoorOpened(doorLockedEvent);
            Assert.That(_uutControl.DoorLocked, Is.EqualTo(locked));
        }
        
        [TestCase(false)]
        public void StationControlDoorEvent_DifferentArguments_DoorBoolIsFalse(bool locked)
        {
            //This class tests the stationsControl by using Nsubstitute. It injects the source into the receiver to make sure that the event gets triggered on the right place
            _sourceDoor.DoorLockedEvent += Raise.EventWith(new DoorLockedEventArgs() { DoorLocked = false });
            Assert.That(_uutControl.DoorLocked, Is.EqualTo(locked));
        }
        */
    }

    public class FakeStationControl: IStationControl
    {
        public bool DoorLocked { get; set; }
        public int _oldId { get; set; }
        public int Rfid { get; set; }
        public DoorLockedEventArgs DoorEventArgs { get; set; }

        public void RfidDetected()
        {
            throw new System.NotImplementedException();
        }

        public IDoor _door { get; set; }
        public PhoneLockerState _state { get; set; }
        public IChargeControl _charger { get; set; }
        public ILogging _logging { get; set; }
        public IDisplay _display { get; set; }
        private IRFIDReader _rfidReader;

        public IRFIDReader RfidReader
        {
            set
            {
                _rfidReader = value;
            }
        }

        public FakeStationControl(IDoor door)
        {
            _door = door;
        }
    }
}