using System;
using System.Text;

namespace Sunrise
{
    internal class SunriseBuffer
    {
        public byte[] Data { get; private set; }
        public int Offset { get; set; }

        public SunriseBuffer(byte[] data)
        {
            Data = data;
        }

        public byte[] ReadBytes(int count)
        {
            byte[] subBuffer = new byte[count];
            Buffer.BlockCopy(Data, Offset, subBuffer, 0, count);
            Offset += count;
            return subBuffer;
        }

        public void WriteBytes(byte[] buffer, int offset, int count)
        {
            Buffer.BlockCopy(buffer, offset, Data, Offset, count);
            Offset += count;
        }

        public void WriteBytes(byte[] buffer, int offset)
        {
            WriteBytes(buffer, offset, buffer.Length);
        }

        public void WriteBytes(byte[] buffer)
        {
            WriteBytes(buffer, 0);
        }


        public byte ReadByte()
        {
            return ReadBytes(1)[0];
        }

        public int ReadInt()
        {
            return BitConverter.ToInt32(ReadBytes(sizeof(int)), 0);
        }

        public string ReadString(int count)
        {
            return Encoding.UTF8.GetString(ReadBytes(count));
        }


        public void WriteByte(byte b)
        {
            WriteBytes(new byte[] { b });
        }

        public void WriteInt(int i)
        {
            WriteBytes(BitConverter.GetBytes(i));
        }

        public void WriteString(string str)
        {
            byte[] asBuffer = new byte[str.Length];

            for (int i = 0; i < str.Length; i++)
                asBuffer[i] = (byte)str[i];

            WriteBytes(asBuffer);
        }
    }
}
