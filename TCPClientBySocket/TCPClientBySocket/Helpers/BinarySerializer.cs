using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TCPClientBySocket.Models;

namespace TCPClientBySocket.Helpers
{
    public static class BinarySerializer
    {
        // List<byte> (0x02 0x03 0x09 0x06 0x4E 0x69 0x73 0x73 0x61 0x6E 0x12 0x07 0xD8 0x13 0x3F 0xCC 0xCC 0xCD)
        public static Car Deserialize(List<byte> bytes)
        {
            Car car = new();
            
            while (bytes.Count > 0)
            {
                if(bytes.Count > 2)
                {
                    bytes.RemoveAt(0);
                    bytes.RemoveAt(0);
                }

                switch (bytes[0])
                {
                    //TODO
                    case 0x09:
                        bytes.RemoveAt(0);
                        GetString(text, bytes);
                        break;
                    case ushort uint16:
                        WriteUInt16(uint16, bytes);
                        break;
                    case float single:
                        WriteSingle(single, bytes);
                        break;
                    default:
                        break;
                }
            }
            return car;
        }

        private static void GetString(string text, List<byte> result)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            result.Add(0x09);
            result.Add((byte)bytes.Length);
            result.AddRange(bytes);
        }

        private static void WriteSingle(float number, List<byte> result)
        {
            result.Add(0x13);
            byte[] buffer = new byte[sizeof(float)];
            BinaryPrimitives.WriteSingleBigEndian(buffer, number);
            result.AddRange(buffer);
        }

        private static void WriteUInt16(ushort number, List<byte> result)
        {
            result.Add(0x12);
            byte[] buffer = new byte[sizeof(ushort)];
            BinaryPrimitives.WriteUInt16BigEndian(buffer, number);
            result.AddRange(buffer);
        }
    }
}
