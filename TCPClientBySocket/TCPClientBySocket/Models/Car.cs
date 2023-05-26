namespace TCPClientBySocket.Models
{
    public class Car
    {
        public Guid Id { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal EngineCapacity { get; set; }
        public int DoorsCount { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (obj is Car)
            {
                var car = obj as Car;
                return car.Model.Equals(Model)
                    && car.Manufacturer.Equals(Manufacturer)
                    && car.Year == Year
                    && car.EngineCapacity == EngineCapacity
                    && car.DoorsCount == DoorsCount;
            }
            return false;
        }

        public override string ToString()
        {
            return $"Производитель: {Manufacturer};" +
                   $"Модель: {Model};" +
                   $"Год: {Year};" +
                   $"Объем: {EngineCapacity};" +
                   $"Кол-во дверей: {DoorsCount};";
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
