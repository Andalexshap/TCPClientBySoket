using System.Xml.Serialization;
using TCPClientBySocket.Models;

namespace TCPClientBySocket.Services
{
    public class XMLService
    {
        public void CreateXmlFromModel(Cars cars)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Cars));

            using (FileStream fs = new FileStream("Cars.xml", FileMode.OpenOrCreate))
            {
                xmlSerializer.Serialize(fs, cars);
            }
        }
    }
}
