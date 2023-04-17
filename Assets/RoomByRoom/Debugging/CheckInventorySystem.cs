using System.Linq;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom.Debugging
{
	internal class CheckInventorySystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<ControllerByPlayer>> _player = default;
		private EcsWorld _world;

		public void Run(IEcsSystems systems)
		{
			_world = systems.GetWorld();
			foreach (int index in _player.Value)
			{
				var inv = _world.GetComponent<Inventory>(index).ItemList;
				var eq = _world.GetComponent<Equipment>(index).ItemList;
				var bp = _world.GetComponent<Backpack>(index).ItemList;

				if (!inv.All(x => eq.Union(bp).Contains(x)))
					Debug.LogError("Inventory is not synchronized with equipment and backpack");
			}
		}
	}
}