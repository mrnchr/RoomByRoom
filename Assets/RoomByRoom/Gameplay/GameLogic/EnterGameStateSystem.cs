using Leopotam.EcsLite;
using UnityEngine;

namespace RoomByRoom
{
	public class EnterGameStateSystem : IEcsInitSystem
	{
		public void Init(IEcsSystems systems)
		{
			Cursor.lockState = CursorLockMode.Locked;
		}
	}
}