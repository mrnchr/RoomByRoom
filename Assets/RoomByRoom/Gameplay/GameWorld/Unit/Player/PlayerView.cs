using UnityEngine;
using UnityEngine.Serialization;

namespace RoomByRoom
{
	public class PlayerView : HumanoidView
	{
		public Transform Character;
		public Transform CameraHolder;
		public Transform Camera;
		public float CameraDistance;
		[FormerlySerializedAs("Wall")] public LayerMask WallMask;

		protected override void OnAwake()
		{
			// Rb = Character.GetComponent<Rigidbody>();
		}
	}
}