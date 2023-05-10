using System;
using UnityEngine;

namespace RoomByRoom.Control
{
  [Serializable]
  public class Configuration
  {
    public Controller Controller;
    public Vector2 MouseSensitivity;
    public bool IsNewGame;
    public bool SaveInFile;
  }
}