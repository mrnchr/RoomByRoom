using UnityEngine;

namespace RoomByRoom
{
	public struct RotateCameraMessage
	{
		public Vector2 RotateDirection;
	}

	public struct TakeCommand
	{
	}

	public struct RotateCommand
	{
		public Vector3 RotateDirection;
	}

	public struct MoveCommand
	{
		public Vector3 MoveDirection;
	}

	public struct JumpCommand
	{
	}

	public struct AttackCommand
	{
	}
}