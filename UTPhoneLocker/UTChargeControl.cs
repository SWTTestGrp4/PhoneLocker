using NSubstitute;
using NUnit.Framework;
using PhoneLockerClassLibrary;

namespace UsbSimulator.Test
{
    [TestFixture]
    public class UTChargeControl
    {
        private IUsbCharger fakeCharger;
        private IChargeControl _uut;
        [SetUp]
        public void Setup()
        {
            fakeCharger = Substitute.For<IUsbCharger>();

            _uut = new ChargeControl(fakeCharger);
            _uut._display = Substitute.For<IDisplay>();

        }


        [TestCase(-10),
         TestCase(0), 
         TestCase(1),
         TestCase(200)]
        public void HandleCurrentChangedEvent_CurrentInserted_CurrentInUUTIsSet(int current)
        {
            fakeCharger.CurrentEventArgs += Raise.EventWith(new CurrentChangedEventArgs() {CurrentCurrent = current});
            Assert.That(_uut.CurrentCurrent, Is.EqualTo(current));
        }


        [TestCase(-1),
         TestCase(0),
         TestCase(1),
         TestCase(4),
         TestCase(6),
         TestCase(499),
         TestCase(501)]
        public void HandleCurrentChangedEvent_CurrentInserted_StartChargeCalledRightly(int current)
        {
            fakeCharger.CurrentEventArgs += Raise.EventWith(new CurrentChangedEventArgs() { CurrentCurrent = current });
            Assert.That(_uut.CurrentCurrent, Is.EqualTo(current));

            if (current == 0)
            {
                fakeCharger.Received(0).StartCharge();
                _uut._display.Received(0).DisplayCharge("");
                //Der er ingen forbindelse til en telefon, eller ladning er ikke startet. Displayet viser ikke noget om ladning
            }
            else if (current > 0 && current <= 5)
            {
                fakeCharger.Received(0).StartCharge();
                _uut._display.Received(1).DisplayCharge("Telefonen er fuldt opladet");
            }
            else if (current > 5 && current <= 500)
            {
                fakeCharger.Received(1).StartCharge();
                _uut._display.Received(1).DisplayCharge("Opladning igang");
            }
            else
            {
                fakeCharger.Received(0).StartCharge();
                _uut._display.Received(1).DisplayCharge("Der er noget galt! Afbryder straks opladning");
            }
        }

        [TestCase(-1),
         TestCase(0),
         TestCase(1),
         TestCase(4),
         TestCase(6),
         TestCase(499),
         TestCase(501)]
        public void HandleCurrentChangedEvent_CurrentInserted_StopChargeCalledRightly(int current)
        {
            fakeCharger.CurrentEventArgs += Raise.EventWith(new CurrentChangedEventArgs() { CurrentCurrent = current });
            Assert.That(_uut.CurrentCurrent, Is.EqualTo(current));

            if (current == 0)
            {
                fakeCharger.Received(0).StopCharge();
                _uut._display.Received(0).DisplayCharge("");
                //Der er ingen forbindelse til en telefon, eller ladning er ikke startet. Displayet viser ikke noget om ladning
            }
            else if (current > 0 && current <= 5)
            {
                fakeCharger.Received(1).StopCharge();
                _uut._display.Received(1).DisplayCharge("Telefonen er fuldt opladet");
            }
            else if (current > 5 && current <= 500)
            {
                fakeCharger.Received(0).StopCharge();
                _uut._display.Received(1).DisplayCharge("Opladning igang");
            }
            else
            {
                fakeCharger.Received(1).StopCharge();
                _uut._display.Received(1).DisplayCharge("Der er noget galt! Afbryder straks opladning");
            }
        }


        [TestCase(false),
         TestCase(true)]
        public void ConnectedGet_SimulateConnection_ReturnExpected(bool expected)
        {
            //Arrange
            _uut._charger.Connected.Returns(expected);

            //act
            var result = _uut.Connected;

            //Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(false)]
        public void ConnectedSet_SimulateConnection_ReturnExpected(bool expected)
        {
            //Arrange

            //act
            _uut.Connected = expected;

            //Assert
            Assert.That(_uut._charger.Connected, Is.EqualTo(expected));
        }

    }
}