namespace RabbitCache.Extensions
{
    public static class DoubleExtension
    {
        public static double ToRadian(this double _value)
        {
            return System.Math.PI / 180 * _value;
        }
    }
}