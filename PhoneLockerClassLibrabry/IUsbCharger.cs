using System;

namespace PhoneLockerClassLibrary
{

    public interface IUsbCharger
    {
        bool Connected { get; }

        double CurrentValue { get; }

        event EventHandler<CurrentChangedEventArgs> CurrentEventArgs;

        
        void StartCharge();
        
        void StopCharge();

        void OnCurrentChangedEvent(CurrentChangedEventArgs e);
    }
}