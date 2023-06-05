using System.Buffers.Binary;
using System.Text;
using TCPClientBySocket.Models;

namespace TCPClientBySocket.Helpers
{
    public static class BinarySerializer
    {
        // List<byte> (0x02 0x03 0x09 0x06 0x4E 0x69 0x73 0x73 0x61 0x6E 0x12 0x07 0xD8 0x13 0x3F 0xCC 0xCC 0xCD)
        public static Car Deserialize(List<byte> bytes)
        {
            Car car = new();

            if (bytes.Count > 2)
            {
                bytes.RemoveAt(0);
                bytes.RemoveAt(0);
            }

            while (bytes.Count > 0 && (bytes[0] == 0x09 || bytes[0] == 0x12 || bytes[0] == 0x13 || bytes[0] == 0x14))
            {
                switch (bytes[0])
                {
                    case 0x09:
                        car.Model = WriteModel(bytes);
                        break;
                    case 0x12:
                        car.Year = WriteYear(bytes);
                        break;
                    case 0x13:
                        car.EngineCapacity = WriteEngine(bytes);
                        break;
                    case 0x14:
                        car.DoorsCount = WriteDoors(bytes);
                        break;
                    default:
                        break;
                }
            }
            return car;
        }

        private static string WriteModel(List<byte> bytes)
        {
            List<byte> byteString = new();

            while (bytes.Count > 0 && bytes[0] != 0x12 && bytes[0] != 0x13 && bytes[0] != 0x14)
            {
                if (bytes[0] == 0x09 && bytes.Count > 2)
                {
                    bytes.RemoveAt(0);
                    bytes.RemoveAt(0);
                }
                byteString.Add(bytes[0]);
                bytes.RemoveAt(0);
            }

            return Encoding.UTF8.GetString(byteString.ToArray());
        }

        private static ushort WriteYear(List<byte> bytes)
        {
            List<byte> byteYear = new();

            while (bytes.Count > 0 && bytes[0] != 0x13 && bytes[0] != 0x14)
            {
                if (bytes[0] == 0x12 && bytes.Any())
                {
                    bytes.RemoveAt(0);
                }
                byteYear.Add(bytes[0]);
                bytes.RemoveAt(0);
            }
            ushort year =
            BinaryPrimitives.ReadUInt16BigEndian(byteYear.ToArray());

            return year;
        }

        private static float WriteEngine(List<byte> bytes)
        {
            List<byte> byteFloat = new();

            while (bytes.Count > 0 && bytes[0] != 0x14)
            {
                if (bytes[0] == 0x13 && bytes.Any())
                {
                    bytes.RemoveAt(0);
                }

                byteFloat.Add(bytes[0]);
                bytes.RemoveAt(0);
            }

            return BinaryPrimitives.ReadSingleBigEndian(byteFloat.ToArray());
        }
        private static ushort WriteDoors(List<byte> bytes)
        {
            List<byte> byteDoors = new();

            while (bytes.Count > 0)
            {
                if (bytes[0] == 0x14 && bytes.Any())
                {
                    bytes.RemoveAt(0);
                }

                if (!bytes.Any()) return 0;

                byteDoors.Add(bytes[0]);
                bytes.RemoveAt(0);
            }
            ushort doors =
            BinaryPrimitives.ReadUInt16BigEndian(byteDoors.ToArray());

            return doors;
        }
    }
}
