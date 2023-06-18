using System.IO;
using System.Xml.Serialization;
using RoomByRoom.Control;

namespace RoomByRoom
{
  public class ConfigSaveService
  {
    private readonly XmlSerializer _xmlSerializer = new XmlSerializer(typeof(Configuration));
    private readonly string _fileName;

    public ConfigSaveService(string fileName)
    {
      _fileName = fileName;
    }
    
    public Configuration Load()
    {
      using var fs = new FileStream(_fileName, FileMode.Open);
      return (Configuration)_xmlSerializer.Deserialize(fs);
    }

    public void Save(Configuration config)
    {
      using var fs = new FileStream(_fileName, FileMode.Create);
      _xmlSerializer.Serialize(fs, config);
    }
  }
}