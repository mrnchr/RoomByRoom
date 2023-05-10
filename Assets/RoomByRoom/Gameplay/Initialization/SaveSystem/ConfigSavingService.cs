using System.IO;
using System.Xml.Serialization;
using RoomByRoom.Control;
using RoomByRoom.Utility;

namespace RoomByRoom
{
  public class ConfigSavingService
  {
    private const string _fileName = Idents.FilePaths.ConfigFileName;
    private readonly XmlSerializer _xmlSerializer = new XmlSerializer(typeof(Configuration));

    public bool LoadData(ref Configuration config)
    {
      try
      {
        using var fs = new FileStream(_fileName, FileMode.Open);
        config = (Configuration)_xmlSerializer.Deserialize(fs);

        return true;
      }
      catch (IOException)
      {
        return false;
      }
    }

    public void SaveData(Configuration config)
    {
      using var fs = new FileStream(_fileName, FileMode.Create);
      _xmlSerializer.Serialize(fs, config);
    }
  }
}