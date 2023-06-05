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

            while ((bytesRead = _stream.ReadByte()) != 0x00)
            {
                // добавляем в буфер
                response.Add((byte)bytesRead);
            }

            var cars = Cars.ConvertToModel(response);

            return cars;
        }

        public async Task<Car> GetCarById(string id)
        {
            var response = new List<byte>();
            int bytesRead = 50; // для считывания байтов из потока

            byte[] data = Encoding.UTF8.GetBytes($"car:{id}" + '\n');

            await _stream.WriteAsync(data);

            while ((bytesRead = _stream.ReadByte()) != 0x00)
            {
                // добавляем в буфер
                response.Add((byte)bytesRead);
            }

            var car = Car.ConvertToModel(response);

            return car;
        }
    }
}