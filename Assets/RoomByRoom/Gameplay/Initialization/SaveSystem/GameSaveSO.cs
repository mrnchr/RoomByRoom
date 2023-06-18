using UnityEngine;
using UnityEngine.Serialization;

namespace RoomByRoom
{
  [CreateAssetMenu(menuName = "RoomByRoom/Data/InitializeData")]
  public class GameSaveSO : ScriptableObject
  {
    [FormerlySerializedAs("Save")] public GameSave Value;
  }
}