using UnityEngine;

namespace RoomByRoom
{
	public class PlayerView : HumanoidView
	{
		public Transform Character;
		public Transform CameraHolder;
		public Transform Camera;
		public float CameraDistance;
		public LayerMask Wall;

		protected override void OnAwake()
		{
			// Rb = Character.GetComponent<Rigidbody>();
		}
	}
}