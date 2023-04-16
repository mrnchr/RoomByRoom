using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom
{
	public class SpawnBonusSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<SpawnCommand>, Exc<BonusViewRef>> _bonuses = default;
		private readonly EcsCustomInject<PackedPrefabData> _prefabData = default;
		private EcsWorld _world;

		public void Run(IEcsSystems systems)
		{
			_world = systems.GetWorld();

			foreach (int index in _bonuses.Value)
			{
				PutItemInBonus(SpawnItem(index), SpawnBonus(index));
			}
		}

		private void PutItemInBonus(ItemView itemView, BonusView bonusView)
		{
			Transform transform = itemView.transform;
			Renderer renderer = itemView.GetComponent<Renderer>();
			ItemPlace place = bonusView.ItemHolder;
			
			transform.SetParent(place.Parent);
			transform.position = place.Point.position - renderer.bounds.center;
			transform.rotation = place.Point.rotation;
		}

		private BonusView SpawnBonus(int index)
		{
			BonusView bonusView = Object.Instantiate(_prefabData.Value.Prefabs.Bonus);
			bonusView.transform.position = _world.GetComponent<SpawnCommand>(index).Coords;
			_world.AddComponent<BonusViewRef>(index)
				.Value = bonusView;
			return bonusView;
		}

		private ItemView SpawnItem(int entity)
		{
			int item = _world.GetComponent<Bonus>(entity).Item;
			ItemView itemView = Object.Instantiate(GetItemPrefab(item));
			_world.AddComponent<ItemViewRef>(item)
				.Value = itemView;
			return itemView;
		}

		private ItemView GetItemPrefab(int item)
		{
			ItemType type = _world.GetComponent<ItemInfo>(item).Type;

			// TODO: add artifact type
			int eqType = GetEquipmentType(type, item);
			int shape = _world.GetComponent<Shape>(item).PrefabIndex;
			return _prefabData.Value.GetItem(type, eqType, shape);
		}

		private int GetEquipmentType(ItemType type, int entity)
		{
			return type == ItemType.Armor
				? (int)_world.GetComponent<ArmorInfo>(entity).Type
				: (int)_world.GetComponent<WeaponInfo>(entity).Type;
		}
	}
}