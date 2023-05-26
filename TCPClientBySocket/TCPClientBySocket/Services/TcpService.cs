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
            var car = ConvertToModelCar(responseServer);

            return car;

            response.Clear();
            await Task.Delay(1000);
        }

        private Car ConvertToModelCar(string message)
        {
            var a = message.Split(";");

            var car = new Car
            {
                Id = Guid.Empty,
                Manufacturer = a[0].Split(":")[1].TrimStart(),
                Model = a[1].Split(":")[1].TrimStart(),
                Year = int.TryParse(a[2].Split(":")[1].TrimStart(), out var year) ? year : 0,
                EngineCapacity = decimal.TryParse(a[3].Split(":")[1].TrimStart(), out var caparacity) ? caparacity : 0,
                DoorsCount = int.TryParse(a[4].Split(":")[1].TrimStart(), out var count) ? count : 0,

            };
            return car;
        }

        private Cars ConvertToModelCars(string message)
        {
            var cars = new Cars();
            var carsArray = message.Split("|");

            foreach (var currentCar in carsArray)
            {
                var car = ConvertToModelCar(currentCar);
                cars.ListCars.Add(car);
            }

            return cars;
        }
    }
}
