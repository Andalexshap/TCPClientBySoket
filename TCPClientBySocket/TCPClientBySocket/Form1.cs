using System.Net;
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
            _tcpService.Connect().GetAwaiter().GetResult();
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            _tcpService.Disconnect().GetAwaiter().GetResult();
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
    }
}