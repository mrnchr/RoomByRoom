using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom
{
	public class LoadPlayerHandSystem : IEcsInitSystem
	{
		private readonly EcsFilterInject<Inc<WeaponInfo>> _weapons = default;
		private EcsWorld _world;

		public void Init(IEcsSystems systems)
		{
			_world = systems.GetWorld();
			
			foreach (int index in _weapons.Value)
			{
				if (_world.Get<WeaponInfo>(index).Type != WeaponType.None || _world.Has<Equipped>(index)) continue;
				Debug.Log("Add not visible");
				_world.Get<Owned>(index);
				_world.Add<NotVisible>(index);
				int player = Utils.GetPlayerEntity(_world);
				RemoveFromList(_world.Get<Backpack>(player).ItemList, index);
				RemoveFromList(_world.Get<Inventory>(player).ItemList, index);
			}
		}

		private void RemoveFromList(List<EcsPackedEntity> list, int item) => list.Remove(_world.PackEntity(item));
	}
}