using RoomByRoom.UI.Game.HUD;
using UnityEngine;
using UnityEngine.Serialization;

namespace RoomByRoom
{
	public class SceneInfo : MonoBehaviour
	{
		public Camera MainCamera;
		public OverlayBar HealthBar;
		public OverlayBar ArmorBar;
		public GameInfo CurrentGame;
		public Saving CurrentSave;
	}
}