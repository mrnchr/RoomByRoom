using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom
{
	public class InputSystem : IEcsInitSystem, IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<ControllerByPlayer>> _player = default;
		private readonly EcsCustomInject<Configuration> _config = default;
		private Controller _controller;
		private EcsWorld _message;
		private EcsWorld _world;

		public void Init(IEcsSystems systems)
		{
			_world = systems.GetWorld();
			_message = systems.GetWorld(Idents.Worlds.MessageWorld);
			_controller = _config.Value.Controller;
		}

		public void Run(IEcsSystems systems)
		{
			foreach (int index in _player.Value)
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
			if (Input.GetKeyDown(_controller.TakeCode)) 
				_world.Add<TakeCommand>(entity);
		}

		private void RotateCamera()
		{
			_message.Add<RotateCameraMessage>(_message.NewEntity())
				.RotateDirection = GetRotationInput();
		}

		private static Vector2 GetRotationInput() =>
			new Vector2((int)Input.GetAxisRaw("Mouse X"), (int)Input.GetAxisRaw("Mouse Y"));

		private void Jump(int entity)
		{
			if (Input.GetAxisRaw("Jump") > 0)
				_world.Add<JumpCommand>(entity);
		}

		private void Move(int entity)
		{
			Vector3 dir = GetMovementInput();
			_world.Add<MoveCommand>(entity)
				.MoveDirection = dir;
			_world.Add<RotateCommand>(entity)
				.RotateDirection = dir;
		}

		private static Vector3 GetMovementInput() =>
			new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

		private void OpenDoor()
		{
			if (Input.GetKeyDown(_controller.OpenDoorCode))
				_message.Add<OpenDoorMessage>(_message.NewEntity());
		}

		private void Attack(int entity)
		{
			if (Input.GetKeyDown(_controller.AttackCode))
				_world.Add<AttackCommand>(entity);
		}
	}
}