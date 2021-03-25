using System;

namespace PhoneLockerClassLibrary
{

    public interface IUsbCharger
    {
        bool Connected { get; }

        double CurrentValue { get; }

        public void SimulateConnected(bool connected);

        event EventHandler<CurrentChangedEventArgs> CurrentEventArgs;

        
        void StartCharge();
        
        void StopCharge();

        void OnCurrentChangedEvent(CurrentChangedEventArgs e);
    }
}