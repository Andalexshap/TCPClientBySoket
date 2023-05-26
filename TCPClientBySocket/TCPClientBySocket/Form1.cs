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

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            var state = _tcpService.Connect().GetAwaiter().GetResult();

            ChangedStatusLabel(state);
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            _tcpService.Disconnect();
        }

        private async void buttonResponse_Click(object sender, EventArgs e)
        {
            var key = Guid.Empty;
            Guid.TryParse(textBoxRequest.Text, out key);
            var response = "TODO";
            if (!response.Any())
            {
                textBoxResponse.Text
            }

        }
        private void ChangedStatusLabel(StateEnum state)
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
    }
}