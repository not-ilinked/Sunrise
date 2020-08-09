using System;

namespace Sunrise
{
    // Structure
    // 1 byte (type)
    // n bytes (data)
    public class SunriseValue : SunriseToken
    {
        public byte[] Data { get; set; }

        public SunriseValue(byte[] data) : base(SunriseType.Value)
        {
            Data = data;
        }

        public override byte[] Serialize()
        {
            SunriseBuffer buffer = new SunriseBuffer(new byte[1 + Data.Length]);
            buffer.WriteByte((byte)Type);
            buffer.WriteBytes(Data);

            return buffer.Data;
        }

        internal static SunriseValue Deserialize(SunriseBuffer buffer, int endIndex)
        {
            return new SunriseValue(buffer.ReadBytes(endIndex - buffer.Offset));
        }
    }
}
