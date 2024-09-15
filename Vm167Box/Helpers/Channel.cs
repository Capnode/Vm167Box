namespace Vm167Box.Helpers
{
    public class Channel
    {
        public string? Name { get; set; }
        public string? Unit { get; set; }
        public int MinSignal { get; set; }
        public int MaxSignal { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }

        public double ToValue(int signal)
        {
            return MinValue + (MaxValue - MinValue) * (signal - MinSignal) / (MaxSignal - MinSignal);
        }

        public int ToSignal(double value)
        {
            return MinSignal + (int)((value - MinValue) * (MaxSignal - MinSignal) / (MaxValue - MinValue));
        }
    }
}
