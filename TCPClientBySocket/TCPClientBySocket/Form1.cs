using TCPClientBySocket.Models;
using TCPClientBySocket.Services;

namespace TCPClientBySocket
{
    public partial class Client : Form
    {
        private readonly TcpService _tcpService;
        private readonly string _host = "127.0.0.1";
        public Client()
        {
            InitializeComponent();
            _tcpService = new TcpService(_host);
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private async void buttonConnect_Click(object sender, EventArgs e)
        {
            var state = await _tcpService.Connect();

            ChangedStatusLabel(state);
        }

        private async void buttonStop_Click(object sender, EventArgs e)
        {
            var state = await _tcpService.Disconnect();

            ChangedStatusLabel(state);
        }

        private async void buttonResponse_Click(object sender, EventArgs e)
        {
            var id = textBoxRequest.Text;
            if (!string.IsNullOrEmpty(id))
            {
                var car = _tcpService.GetCarById(id).GetAwaiter().GetResult();
                //Отправка id
            }

            var cars = _tcpService.GetAllCars().GetAwaiter().GetResult();

            var response = "TODO";
            if (!response.Any())
            {
                //textBoxResponse.Text
            }
        }
        private async void ChangedStatusLabel(StateEnum state)
        {
            switch (state)
            {
                case StateEnum.Connected:
                    labelStatusChanging.Text = "Connected";
                    labelStatusChanging.ForeColor = Color.Green;

                    buttonConnect.Visible = false;
                    buttonStop.Visible = true;

                    buttonRequest.Visible = true;
                    textBoxRequest.Visible = true;
                    break;
                case StateEnum.Disconnected:
                    labelStatusChanging.Text = "Not connected";
                    labelStatusChanging.ForeColor = Color.Red;

                    buttonConnect.Visible = true;
                    buttonStop.Visible = false;

                    buttonRequest.Visible = false;
                    textBoxRequest.Visible = false;
                    break;
                case StateEnum.Error:
                    labelStatusChanging.Text = "Error";
                    labelStatusChanging.ForeColor = Color.Red;

                    buttonConnect.Visible = true;
                    buttonStop.Visible = false;

                    buttonRequest.Visible = false;
                    textBoxRequest.Visible = false;
                    break;
                default:
                    break;
            }
        }
        /*
                private void button1_Click(object sender, EventArgs e)
                {
                    var car = new Car
                    {
                        Id = Guid.NewGuid(),
                        Manufacturer = "NISSAN",
                        Model = "nISSAN",
                        EngineCapacity = 1.8M,
                        DoorsCount = 0,
                        Year = 2050
                    };

                    var stringCar = car.ToString();

                    _tcpService.ConvertToModelCar(stringCar);
                }
        */
    }
}
