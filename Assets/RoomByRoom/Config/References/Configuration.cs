using UnityEngine;

namespace RoomByRoom
{
  [CreateAssetMenu(menuName = "RoomByRoom/Config/Configuration")]
  public class Configuration : ScriptableObject
  {
    public Controller Controller;
    public Vector2 MouseSensitivity;
    public string DefaultSaveName;
    public bool IsNewGame;
    public bool SaveInFile;
  }
}