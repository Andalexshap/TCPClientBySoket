using TCPClientBySocket.Models;
using TCPClientBySocket.Services;

namespace TCPClientBySocket
{
    public partial class Client : Form
    {
        private readonly TcpService _tcpService;
        private readonly string _host = "127.0.0.1";
        private readonly XMLService _xmlService;

        private Cars _cars = new Cars();

        public Client()
        {
            InitializeComponent();
            _tcpService = new TcpService(_host);
            _xmlService = new XMLService();

            //инициализаци€ таблицы
            InitDataGrid();
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
            DataGridClear();

            var id = textBoxRequest.Text;
            if (!string.IsNullOrEmpty(id))
            {
                var car = _tcpService.GetCarById(id).GetAwaiter().GetResult();

                SetColumnGrid(car);
                _cars = new Cars { ListCars = new List<Car> { car } };
                return;
            }

            var cars = _tcpService.GetAllCars().GetAwaiter().GetResult();

            _cars = cars;

            SetColumnsGrid(cars);

            buttonSave.Visible = true;
        }

        private void DataGridClear()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            InitDataGrid();
        }

        private void SetColumnsGrid(Cars cars)
        {
            foreach (var car in cars.ListCars)
            {
                SetColumnGrid(car);
            }
        }

        private void SetColumnGrid(Car car)
        {
            DataGridViewCell model = new DataGridViewTextBoxCell();
            DataGridViewCell year = new DataGridViewTextBoxCell();
            DataGridViewCell engineCapacity = new DataGridViewTextBoxCell();
            DataGridViewCell doorsCount = new DataGridViewTextBoxCell();

            model.Value = car.Model;
            year.Value = car.Year;
            engineCapacity.Value = car.EngineCapacity;
            doorsCount.Value = car.DoorsCount;

            DataGridViewRow row = new DataGridViewRow();
            //добавление €чеек в строку
            row.Cells.AddRange(model, year, engineCapacity, doorsCount);
            //добавление строки в таблицу
            dataGridView1.Rows.Add(row);
        }

        private void InitDataGrid()
        {
            //1 столбец, текстовый
            DataGridViewTextBoxColumn column0 = new DataGridViewTextBoxColumn();
            column0.Name = "Model";
            column0.HeaderText = "Model";
            //2 столбец,
            DataGridViewTextBoxColumn column1 = new DataGridViewTextBoxColumn();
            column1.Name = "Year";
            column1.HeaderText = "Year";
            DataGridViewTextBoxColumn column2 = new DataGridViewTextBoxColumn();
            column2.Name = "EngineCapacity";
            column2.HeaderText = "EngineCapacity";
            DataGridViewTextBoxColumn column3 = new DataGridViewTextBoxColumn();
            column3.Name = "DoorsCount";
            column3.HeaderText = "DoorsCount";
            //добавл€ем столбцы
            dataGridView1.Columns.AddRange(column0, column1, column2, column3);
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

        private void buttonSave_Click(object sender, EventArgs e)
        {
            _xmlService.CreateXmlFromModel(_cars);
        }
    }
}
