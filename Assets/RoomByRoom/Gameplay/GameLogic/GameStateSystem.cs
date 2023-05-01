using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using static UnityEngine.CursorLockMode;

namespace RoomByRoom
{
	public class GameStateSystem : IEcsRunSystem
	{
		private readonly EcsCustomInject<BlockingService> _blockingSvc = default;

		public void Run(IEcsSystems systems)
		{
			Time.timeScale = _blockingSvc.Value.IsBlocking() ? 0 : 1;
			Cursor.lockState = _blockingSvc.Value.IsBlocking() ? None : Locked;
		}
	}
}