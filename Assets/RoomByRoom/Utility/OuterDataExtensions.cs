using RoomByRoom.Control;

namespace RoomByRoom.Utility
{
  public static class OuterDataExtensions
  {
    public static OuterData SetConfig(this OuterData obj, Configuration config)
    {
      obj.Config = config;
      
      return obj;
    }
  }
}