using Sunrise.Mapping;
using System;

namespace Sunrise
{
    public static class SunriseSerializer
    {
        public static byte[] Serialize(object data)
        {
            if (typeof(SunriseToken).IsAssignableFrom(data.GetType()))
                return ((SunriseToken)data).Serialize();
            else
                return SunriseMapper.Convert(data).Serialize();
        }

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

        public static T Deserialize<T>(byte[] buffer)
        {
            return (T)SunriseMapper.Map(Deserialize(new SunriseBuffer(buffer), buffer.Length), typeof(T));
        }
    }
}
