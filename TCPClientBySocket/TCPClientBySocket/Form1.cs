using TCPClientBySocket.Models;
using TCPClientBySocket.Services;

namespace TCPClientBySocket
{
    public partial class Client : Form
    {
        private readonly TcpService _tcpService;
        private readonly string _host = "127.0.0.1";
        private readonly XMLService _xmlService;

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
            var id = textBoxRequest.Text;
            if (!string.IsNullOrEmpty(id))
            {
                var car = _tcpService.GetCarById(id).GetAwaiter().GetResult();

                SetColumnGrid(car);
                _xmlService.CreateXmlFromModel(new Cars { ListCars = new List<Car> { car } });
                return;
            }

            var cars = _tcpService.GetAllCars().GetAwaiter().GetResult();

            _xmlService.CreateXmlFromModel(cars);

            SetColumnsGrid(cars);
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
            DataGridViewTextBoxColumn column3 = new DataGridViewTextBoxColumn();
            column1.Name = "EngineCapacity";
            column1.HeaderText = "EngineCapacity";
            DataGridViewTextBoxColumn column4 = new DataGridViewTextBoxColumn();
            column1.Name = "DoorsCount";
            column1.HeaderText = "DoorsCount";
            //добавл€ем столбцы
            dataGridView1.Columns.AddRange(column0, column1, column3, column4);
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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
