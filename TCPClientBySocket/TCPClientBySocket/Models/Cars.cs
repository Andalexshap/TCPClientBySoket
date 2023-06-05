namespace TCPClientBySocket.Models
{
    public class Cars
    {
        public List<Car> ListCars { get; set; }

        public static Cars ConvertToModel(List<byte> bytes)
        {
            var cars = new Cars { ListCars = new List<Car>() };
            while (bytes.Count > 0)
            {
            if (bytes[0] == 0x02)
                {
                    List<byte> car = new();
                    car.Add(bytes[0]);
                    bytes.RemoveAt(0);

                    while (bytes[0] != 0x02 || bytes.Count > 0)
                    {
                        car.Add(bytes[0]);
                        bytes.RemoveAt(0);
                    }
                }

            cars.ListCars.Add(Car.ConvertToModel(bytes));
            }

            return cars;
        }
    }
}
