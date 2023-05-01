using System;
using UnityEngine;

namespace RoomByRoom
{
	[Serializable]
	public struct Controller
	{
		[HideInInspector] public KeyCode ForwardCode;
		[HideInInspector] public KeyCode BackCode;
		[HideInInspector] public KeyCode LeftCode;
		[HideInInspector] public KeyCode RightCode;
		[HideInInspector] public KeyCode JumpCode;
		public KeyCode AttackCode;
		public KeyCode TakeCode;
		public KeyCode OpenDoorCode;
		public KeyCode InventoryCode;
		public KeyCode PauseCode;
		[HideInInspector] public KeyCode CameraUpCode;
		[HideInInspector] public KeyCode CameraDownCode;
		[HideInInspector] public KeyCode CameraLeftCode;
		[HideInInspector] public KeyCode CameraRightCode;
	}
}