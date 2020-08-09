using System;

namespace Sunrise
{
    public static class SunriseDeserializer
    {
        internal static SunriseToken Deserialize(SunriseBuffer buffer, int count)
        {
            int endIndex = buffer.Offset + count;

            byte rawType = buffer.ReadByte();

            switch ((SunriseType)rawType)
            {
                case SunriseType.Value:
                    return SunriseValue.Deserialize(buffer, endIndex);
                case SunriseType.Object:
                    return SunriseObject.Deserialize(buffer, endIndex);
                case SunriseType.Array:
                    return SunriseArray.Deserialize(buffer, endIndex);
                default:
                    throw new NotSupportedException($"Unsupported type found at index {buffer.Offset}: {rawType}");
            }
        }

        public static SunriseToken Deserialize(byte[] buffer)
        {
            return Deserialize(new SunriseBuffer(buffer), buffer.Length);
        }
    }
}
