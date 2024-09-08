namespace Vm167Box.Helpers
{
    public class Signal
    {
        public static double Duty(double x, double period, int dutyCycle)
        {
            var half = period * dutyCycle / 100d;
            if (x < half)
            {
                return x * period / (2d * half);
            }

            return period * (1 + (x - half)/(period-half)) / 2;
        }
    }
}