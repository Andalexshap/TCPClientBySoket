namespace TCPClientBySocket
{
    public partial class Client : Form
    {
        public Client()
        {
            InitializeComponent();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {

        }

        private void buttonStop_Click(object sender, EventArgs e)
        {

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