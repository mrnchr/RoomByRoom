using UnityEngine;
using Zenject;

namespace RoomByRoom.Debugging
{
  public class DebugSave : MonoBehaviour
  {
    [Inject] public ProgressData ProgressData;
  }
}