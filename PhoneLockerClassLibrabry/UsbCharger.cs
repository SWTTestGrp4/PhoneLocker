using System;
using System.Timers;

namespace PhoneLockerClassLibrary
{
    public class UsbCharger : IUsbCharger
    {
        // Constants
        private const double MaxCurrent = 500.0; // mA
        private const double FullyChargedCurrent = 2.5; // mA
        private const double OverloadCurrent = 750; // mA
        private const int ChargeTimeMinutes = 20; // minutes
        private const int CurrentTickInterval = 250; // ms

        public event EventHandler<CurrentChangedEventArgs> CurrentEventArgs;

        public double CurrentValue { get; private set; }
        public bool Connected { get; private set; }

        private bool _overload;
        private bool _charging;
        private System.Timers.Timer _timer;
        private int _ticksSinceStart;

        public UsbCharger()
        {
            CurrentValue = 0.0;
            Connected = true;
            _overload = false;

            _timer = new System.Timers.Timer();
            _timer.Enabled = false;
            _timer.Interval = CurrentTickInterval;
            _timer.Elapsed += TimerOnElapsed;
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            // Only execute if charging
            if (_charging)
            {
                _ticksSinceStart++;
                if (Connected && !_overload)
                {
                    double newValue = MaxCurrent - 
                                      _ticksSinceStart * (MaxCurrent - FullyChargedCurrent) / (ChargeTimeMinutes * 60 * 1000 / CurrentTickInterval);
                    CurrentValue = Math.Max(newValue, FullyChargedCurrent);
                }
                else if (Connected && _overload)
                {
                    CurrentValue = OverloadCurrent;
                }
                else if (!Connected)
                {
                    CurrentValue = 0.0;
                }

                OnCurrentChangedEvent(new CurrentChangedEventArgs() { CurrentCurrent = this.CurrentValue });
            }
        }

        public void SimulateConnected(bool connected)
        {
            Connected = connected;
        }

        public void SimulateOverload(bool overload)
        {
            _overload = overload;
        }

        public void StartCharge()
        {
            // Ignore if already charging
            if (!_charging)
            {
                if (Connected && !_overload)
                {
                    CurrentValue = 500;
                    //_charging = true;
                    //TODO
                }
                else if (Connected && _overload)
                {
                    CurrentValue = OverloadCurrent;
                }
                else if (!Connected)
                {
                    CurrentValue = 0.0;
                }

                OnCurrentChangedEvent(new CurrentChangedEventArgs() { CurrentCurrent = this.CurrentValue });
                _ticksSinceStart = 0;

                _charging = true;

                _timer.Start();
            }
        }

        public void StopCharge()
        {
            _timer.Stop();

            CurrentValue = 0.0;
            OnCurrentChangedEvent(new CurrentChangedEventArgs() { CurrentCurrent = this.CurrentValue });

            _charging = false;
        }


        public void OnCurrentChangedEvent(CurrentChangedEventArgs e)
        {
            CurrentEventArgs?.Invoke(this, e);
        }
    }
}
