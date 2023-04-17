using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom
{
	public class InputSystem : IEcsInitSystem, IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<ControllerByPlayer>> _controller = default;
		private EcsWorld _message;
		private EcsWorld _world;

		public void Init(IEcsSystems systems)
		{
			_world = systems.GetWorld();
			_message = systems.GetWorld(Idents.Worlds.MessageWorld);
		}

		public void Run(IEcsSystems systems)
		{
			foreach (int index in _controller.Value)
			{
				OpenDoor();
				Move(index);
				Jump(index);
				RotateCamera();
				Attack(index);
				Take(index);
			}
		}

		private void Take(int entity)
		{
			if (Input.GetKeyDown(KeyCode.T)) _world.AddComponent<TakeCommand>(entity);
		}

		private void RotateCamera()
		{
			int entity = _message.NewEntity();
			_message.AddComponent<RotateCameraMessage>(entity)
				.Assign(x =>
				{
					x.RotateDirection = GetRotationInput();
					return x;
				});
		}

		private static Vector2 GetRotationInput() =>
			new Vector2((int)Input.GetAxisRaw("Mouse X"), (int)Input.GetAxisRaw("Mouse Y"));

		private void Jump(int entity)
		{
			if (Input.GetAxisRaw("Jump") > 0)
				_world.AddComponent<JumpCommand>(entity);
		}

		private void Move(int entity)
		{
			Vector3 dir = GetMovementInput();
			_world.AddComponent<MoveCommand>(entity)
				.Assign(x =>
				{
					x.MoveDirection = dir;
					return x;
				});
			_world.AddComponent<RotateCommand>(entity)
				.Assign(x =>
				{
					x.RotateDirection = dir;
					return x;
				});
		}

		private static Vector3 GetMovementInput() =>
			new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

		private void OpenDoor()
		{
			if (Input.GetKeyDown(KeyCode.F))
			{
				int entity = _message.NewEntity();
				_message.AddComponent<OpenDoorMessage>(entity);
			}
		}

		private void Attack(int entity)
		{
			if (Input.GetKeyDown(KeyCode.Mouse0))
				_world.AddComponent<AttackCommand>(entity);
		}
	}
}