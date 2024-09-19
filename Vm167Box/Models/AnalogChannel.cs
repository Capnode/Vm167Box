namespace Vm167Box.Models
{
    public class AnalogChannel
    {
        public bool Changed { get; set; }

        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set
            {
                if (value == _name) return;
                _name = value ?? string.Empty;
                Changed = true;
            }
        }

        private string _unit = string.Empty;
        public string Unit
        {
            get => _unit;
            set
            {
                if (value == _unit) return;
                _unit = value ?? string.Empty;
                Changed = true;
            }
        }

        private int _decimals;
        public int Decimals
        {
            get => _decimals;
            set
            {
                if (value == _decimals) return;
                _decimals = value;
                Changed = true;
            }
        }

        private int _minSignal;
        public int MinSignal
        {
            get => _minSignal;
            set
            {
                if (value == _minSignal) return;
                _minSignal = value;
                Changed = true;
            }
        }

        private int _maxSignal;
        public int MaxSignal
        {
            get => _maxSignal;
            set
            {
                if (value == _maxSignal) return;
                _maxSignal = value;
                Changed = true;
            }
        }

        private double _minValue;
        public double MinValue
        {
            get => _minValue;
            set
            {
                if (value == _minValue) return;
                _minValue = value;
                Changed = true;
            }
        }
        private double _maxValue;
        public double MaxValue
        {
            get => _maxValue;
            set
            {
                if (value == _maxValue) return;
                _maxValue = value;
                Changed = true;
            }
        }

        private int _signal;
        public int Signal
        {
            get => _signal;
            set
            {
                if (value == _signal) return;
                _signal = value;
                _value = ToValue(_signal);
                Changed = true;
            }
        }

        private double _value;
        public double Value
        {
            get => _value;
            set
            {
                if (value == _value) return;
                _value = value;
                _signal = ToSignal(_value);
                Changed = true;
            }
        }

        public void Copy(AnalogChannel channel)
        {
            _name = channel.Name;
            _unit = channel.Unit;
            _decimals = channel.Decimals;
            _minSignal = channel.MinSignal;
            _maxSignal = channel.MaxSignal;
            _minValue = channel.MinValue;
            _maxValue = channel.MaxValue;
            _signal = channel.Signal;
            _value = channel.Value;
        }

        private double ToValue(int signal)
        {
            return MinValue + (MaxValue - MinValue) * (signal - MinSignal) / (MaxSignal - MinSignal);
        }

        private int ToSignal(double value)
        {
            return MinSignal + (int)((value - MinValue) * (MaxSignal - MinSignal) / (MaxValue - MinValue));
        }
    }
}
