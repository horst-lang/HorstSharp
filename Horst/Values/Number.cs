namespace Horst.Values
{
    public class Number: Value
    {
        public double Value { get; }

        public Number(double number)
        {
            Value = number;
        }
    }
}