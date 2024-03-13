using RoomByRoom.UI.Game.HUD;
using UnityEngine;

namespace RoomByRoom
{
  public class SceneInfo : MonoBehaviour
  {
    public Camera MainCamera;
    public OverlayBar HealthBar;
    public OverlayBar ArmorBar;
    public GlobalState CurrentState;
    public ProgressData CurrentSave;
    public Control.PlayerConfiguration Config;
    public bool DevTools;
  }
}