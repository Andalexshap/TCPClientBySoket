namespace TCPClientBySocket.Models
{
    public class Cars
    {
        public List<Car> ListCars { get; set; }

        public static Cars ConvertToModel(string message)
        {
            var cars = new Cars { ListCars = new List<Car>() };
            message = message.Substring(5);
            var carsArray = message.Split("0x02 ");

            cars.ListCars = carsArray.Select(x => Car.ConvertToModel(x)).ToList();

            return cars;
        }
    }
}
