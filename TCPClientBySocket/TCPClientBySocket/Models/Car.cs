using System.Xml.Serialization;
using TCPClientBySocket.Helpers;

namespace TCPClientBySocket.Models
{
    public class Car
    {
        [XmlIgnore]
        public Guid Id { get; set; }
        public string Model { get; set; }
        public ushort Year { get; set; }
        public float EngineCapacity { get; set; }
        public ushort DoorsCount { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (obj is Car)
            {
                var car = obj as Car;
                return car.Model.Equals(Model)
                    && car.Year == Year
                    && car.EngineCapacity == EngineCapacity
                    && car.DoorsCount == DoorsCount;
            }
            return false;
        }

        public override string ToString()
        {
            return $"Модель: {Model};" +
                   $"Год: {Year};" +
                   $"Объем: {EngineCapacity};" +
                   $"Кол-во дверей: {DoorsCount};";
        }

        public static Car ConvertToModel(List<byte> bytes)
            => BinarySerializer.Deserialize(bytes);

    }
}
