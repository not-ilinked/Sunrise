using System;
using System.Collections.Generic;
using System.Text;

namespace Sunrise
{
    // Structure
    // Header:
    // 1 byte (type)
    // Item:
    // 4 bytes (length) - the item's length
    // n bytes (contents) - the item
    public class SunriseArray : SunriseToken
    {
        public List<SunriseToken> Items { get; set; }

        public SunriseArray(List<SunriseToken> items) : base(SunriseType.Array)
        {
            Items = items;
        }

        public SunriseArray() : this(new List<SunriseToken>())
        { }

        public SunriseToken this[int index]
        {
            get { return Items[index]; }
            set { Items[index] = value; }
        }

        public override byte[] Serialize()
        {
            List<byte[]> serializedChildren = new List<byte[]>();

            foreach (var item in Items)
            {
                byte[] serializedContents = item.Serialize();

                SunriseBuffer buffer = new SunriseBuffer(new byte[4 + serializedContents.Length]);
                buffer.WriteInt(serializedContents.Length);
                buffer.WriteBytes(serializedContents);

                serializedChildren.Add(buffer.Data);
            }

            int itemLength = 0;
            foreach (var child in serializedChildren)
                itemLength += child.Length;

            SunriseBuffer result = new SunriseBuffer(new byte[1 + itemLength]);

            result.WriteByte((byte)Type);

            foreach (var child in serializedChildren)
                result.WriteBytes(child);

            return result.Data;
        }


        internal static SunriseArray Deserialize(SunriseBuffer buffer, int endIndex)
        {
            SunriseArray arr = new SunriseArray();

            while (buffer.Offset < endIndex)
            {
                int length = buffer.ReadInt();

                arr.Items.Add(SunriseSerializer.Deserialize(buffer, length));
            }

            return arr;
        }
    }
}
