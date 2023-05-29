using System.Net.Sockets;
using System.Text;
using TCPClientBySocket.Models;

namespace TCPClientBySocket.Services
{
    public class TcpService
    {
        private readonly string _host;
        private readonly TcpClient _tcpClient;
        private NetworkStream _stream;

        public TcpService(string host)
        {
            _host = host;
            _tcpClient = new TcpClient();
        }

        public async Task<StateEnum> Connect()
        {
            try
            {
                await _tcpClient.ConnectAsync(_host, 8888);
                _stream = _tcpClient.GetStream();
                return StateEnum.Connected;
            }
            catch (Exception)
            {
                return StateEnum.Error;
            }
        }

        public async Task<StateEnum> Disconnect()
        {
            try
            {
                await _stream.WriteAsync(Encoding.UTF8.GetBytes("END\n"));
                _tcpClient.Close();
                return StateEnum.Disconnected;
            }
            catch (Exception)
            {
                return StateEnum.Error;
                throw;
            }
        }

        public async Task<Cars> GetAllCars()
        {
            // буфер для входящих данных
            var response = new List<byte>();
            int bytesRead = 10; // для считывания байтов из потока

            byte[] data = Encoding.UTF8.GetBytes("cars" + '\n');

            await _stream.WriteAsync(data);

            while ((bytesRead = _stream.ReadByte()) != '\n')
            {
                // добавляем в буфер
                response.Add((byte)bytesRead);
            }

            var responseServer = Encoding.UTF8.GetString(response.ToArray());

            var cars = ConvertToModelCars(responseServer);

            return cars;

            response.Clear();
            await Task.Delay(1000);
        }

        public async Task<Car> GetCarById(string id)
        {
            var response = new List<byte>();
            int bytesRead = 10; // для считывания байтов из потока

            byte[] data = Encoding.UTF8.GetBytes($"car:{id}" + '\n');

            await _stream.WriteAsync(data);

            while ((bytesRead = _stream.ReadByte()) != '\n')
            {
                // добавляем в буфер
                response.Add((byte)bytesRead);
            }

            var responseServer = Encoding.UTF8.GetString(response.ToArray());
            var car = ConvertToModelCar(responseServer.Trim());

            return car;

            response.Clear();
            await Task.Delay(1000);
        }

        #region HexToPropertyConvertation
        public Car ConvertToModelCar(string message)
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

        private Cars ConvertToModelCars(string message)
        {
            var cars = new Cars { ListCars = new List<Car>() };
            message = message.Substring(5);
            var carsArray = message.Split("0x02 ");

            foreach (var currentCar in carsArray)
            {
                var car = ConvertToModelCar(currentCar);
                cars.ListCars.Add(car);
            }

            return cars;
        }

        private string ConvertHexToStringModel(List<string> byteList)
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

        private int ConvertHexToIntYear(List<string> byteList)
        {
            string stringHexYear = byteList[0].Replace("0x", "") + byteList[1].Replace("0x", "");

            byteList.Remove(byteList[0]);
            byteList.Remove(byteList[0]);

            return Convert.ToInt32(stringHexYear, 16);
        }

        private float ConvertHexToFloat(List<string> byteList)
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
        private int ConvertHexToIntDoors(List<string> byteList)
        {
            string stringHexDoors = byteList[0].Replace("0x", "");

            byteList.Remove(byteList[0]);

            return Convert.ToInt32(stringHexDoors, 16);
        }
        #endregion
    }
}