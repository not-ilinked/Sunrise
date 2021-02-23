namespace Sunrise
{
    public abstract class SunriseToken
    {
        public SunriseType Type { get; private set; }

        public SunriseToken(SunriseType type)
        {
            Type = type;
        }

        internal abstract byte[] Serialize();
    }
}
