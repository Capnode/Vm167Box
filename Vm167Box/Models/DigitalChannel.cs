namespace Vm167Box.Models
{
    public class DigitalChannel
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

        private bool _value;
        public bool Value
        {
            get => _value;
            set
            {
                if (value == _value) return;
                _value = value;
                Changed = true;
            }
        }

        public void Copy(DigitalChannel channel)
        {
            _name = channel.Name;
            _value = channel.Value;
        }
    }
}
