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
    }
}