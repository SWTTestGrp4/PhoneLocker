using System;

namespace PhoneLockerClassLibrary
{

    public interface IUsbCharger
    {
        // Require connection status of the phone
        bool Connected { get; }

        // Direct access to the current current value
        double CurrentValue { get; }

        // Event triggered on new current value
        event EventHandler<CurrentChangedEventArgs> CurrentEventArgs;

        // Start charging
        void StartCharge();
        // Stop charging
        void StopCharge();

        protected void OnCurrentChangedEvent(CurrentChangedEventArgs e);
    }
}