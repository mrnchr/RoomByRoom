using UnityEngine;

namespace RoomByRoom.Control
{
  [CreateAssetMenu(menuName = "RoomByRoom/Config/Configuration")]
  public class ConfigurationSO : ScriptableObject
  {
    public Configuration Value;
  }
}