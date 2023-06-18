using RoomByRoom.Control;
using UnityEngine;
using UnityEngine.Serialization;

namespace RoomByRoom
{
  public class OuterData : MonoBehaviour
  {
    [HideInInspector] public string ProfileName;
    [HideInInspector] public Configuration Config;
  }
}