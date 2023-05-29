using System.Xml.Serialization;
using TCPClientBySocket.Models;

namespace TCPClientBySocket.Services
{
    public class XMLService
    {
        public void CreateXmlFromModel(Cars cars)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Cars));

            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "xml files(*.xml)|*.xml|All files(*.*)|*.*";
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                using (FileStream fs = new FileStream(saveFile.FileName, FileMode.OpenOrCreate))
                {
                    xmlSerializer.Serialize(fs, cars);
                }

                MessageBox.Show("Файл сохранен");
            }
        }
    }
}
