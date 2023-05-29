using System.Text;
using System.Xml.Serialization;

namespace TCPClientBySocket.Models
{
    public class Car
    {
        [XmlIgnore]
        public Guid Id { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public float EngineCapacity { get; set; }
        public int DoorsCount { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (obj is Car)
            {
                var car = obj as Car;
                return car.Model.Equals(Model)
                    && car.Year == Year
                    && car.EngineCapacity == EngineCapacity
                    && car.DoorsCount == DoorsCount;
            }
            return false;
        }

        public override string ToString()
        {
            return $"Модель: {Model};" +
                   $"Год: {Year};" +
                   $"Объем: {EngineCapacity};" +
                   $"Кол-во дверей: {DoorsCount};";
        }

        public static Car ConvertToModel(string message)
        {
            var car = new Car();
            var a = message.Split(" ");

            if (message.StartsWith("0x02 "))
            {
                message = message.Substring(5);
                if (message.Length == 0)
                {
                    return car;
                }
            }

            var byteList = message.Split(" ").ToList();

            var countProperties = 0;

            int.TryParse(byteList[0].Replace("0x", ""), out countProperties);

            byteList.Remove(byteList[0]);


            for (int i = 0; i < countProperties && byteList.Count != 0; i++)
            {
                if (byteList[0] == "0x09")
                {
                    byteList.Remove(byteList[0]);
                    car.Model = ConvertHexToStringModel(byteList);
                }
                else if (byteList[0] == "0x12" && byteList.Contains("0x13"))
                {
                    byteList.Remove(byteList[0]);
                    car.Year = ConvertHexToIntYear(byteList);
                }
                else if (byteList[0] == "0x13")
                {
                    byteList.Remove(byteList[0]);
                    car.EngineCapacity = ConvertHexToFloat(byteList);
                }
                else if (byteList[0] == "0x12")
                {
                    byteList.Remove(byteList[0]);
                    car.DoorsCount = ConvertHexToIntDoors(byteList);
                }
            }
            return car;
        }

        private static string ConvertHexToStringModel(List<string> byteList)
        {
            var count = 0;
            int.TryParse(byteList[0].Replace("0x", ""), out count);
            byteList.Remove(byteList[0]);

            var stringHex = string.Empty;

            for (int i = 0; i < count; i++)
            {
                stringHex += byteList[0].Replace("0x", "");
                byteList.Remove(byteList[0]);
            }

            string[] hexBytes = new string[stringHex.Length / 2];
            for (int i = 0; i < hexBytes.Length; i++)
            {
                hexBytes[i] = stringHex.Substring(i * 2, 2);
            }
            byte[] resultBytes = hexBytes.Select(value => Convert.ToByte(value, 16)).ToArray();
            string result = Encoding.UTF8.GetString(resultBytes);

            return result;
        }

        private static int ConvertHexToIntYear(List<string> byteList)
        {
            string stringHexYear = byteList[0].Replace("0x", "") + byteList[1].Replace("0x", "");

            byteList.Remove(byteList[0]);
            byteList.Remove(byteList[0]);

            return Convert.ToInt32(stringHexYear, 16);
        }

        private static float ConvertHexToFloat(List<string> byteList)
        {
            var listByte = new List<byte>();

            for (int i = 0; i < 4; i++)
            {
                listByte.Add(Convert.ToByte(byteList[0], 16));
                byteList.Remove(byteList[0]);
            }
            var bytes = listByte.ToArray();

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes); // Convert big endian to little endian
            }

            return BitConverter.ToSingle(bytes, 0);
        }
        private static int ConvertHexToIntDoors(List<string> byteList)
        {
            string stringHexDoors = byteList[0].Replace("0x", "");

            byteList.Remove(byteList[0]);

            return Convert.ToInt32(stringHexDoors, 16);
        }
    }
}
