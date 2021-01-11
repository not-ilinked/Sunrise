using System;
using System.Collections.Generic;
using System.Linq;

namespace Sunrise
{
    // Structure
    // Header
    // 1 byte (type)
    // Items
    // 1 byte (name length) - length of 'name'
    // 4 byte (length) - length of the contents
    // n bytes (name) - length of the child's name. indicated by 'flags'.
    // n bytes (contents) - serialized children
    public class SunriseObject : SunriseToken
    {
        private static readonly int MetaLength = 1 + 4;

        public Dictionary<string, SunriseToken> Children { get; set; }

        public SunriseObject(Dictionary<string, SunriseToken> children) : base(SunriseType.Object)
        {
            Children = children;
        }

        public SunriseObject() : this(new Dictionary<string, SunriseToken>())
        { }

        public SunriseToken this[int index]
        {
            get { return Children.Values.ElementAt(index); }
            set { Children[Children.Keys.ElementAt(index)] = value; }
        }

        public SunriseToken this[string name]
        {
            get { return Children[name]; }
            set { Children[name] = value; }
        }

        public override byte[] Serialize()
        {
            List<byte[]> serializedChildren = new List<byte[]>();

            foreach (var child in Children)
            {
                byte[] serializedContents = child.Value.Serialize();

                SunriseBuffer buffer = new SunriseBuffer(new byte[MetaLength + child.Key.Length + serializedContents.Length]);
                buffer.WriteByte((byte)child.Key.Length);
                buffer.WriteInt(serializedContents.Length);
                buffer.WriteString(child.Key);
                buffer.WriteBytes(serializedContents);

                serializedChildren.Add(buffer.Data);
            }

            int combinedLength = 0;

            foreach (var child in serializedChildren)
                combinedLength += child.Length;

            SunriseBuffer result = new SunriseBuffer(new byte[1 + combinedLength]);
            result.WriteByte((byte)Type);

            foreach (var child in serializedChildren)
                result.WriteBytes(child);

            return result.Data;
        }


        internal static SunriseObject Deserialize(SunriseBuffer buffer, int endIndex)
        {
            SunriseObject obj = new SunriseObject();

            while (buffer.Offset < endIndex)
            {
                byte nameLength = buffer.ReadByte();
                int contentLength = buffer.ReadInt();

                string name = buffer.ReadString(nameLength);

                obj.Children[name] = SunriseSerializer.Deserialize(buffer, contentLength);
            }

            return obj;
        }
    }
}
