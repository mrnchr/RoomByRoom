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
				PutItemInBonus(SpawnItem(index), SpawnBonus(index));
		}

		private void PutItemInBonus(ItemView itemView, BonusView bonusView)
		{
			Transform transform = itemView.transform;
			var renderer = itemView.GetComponent<Renderer>();
			ItemPlace place = bonusView.ItemHolder;

			transform.SetParent(place.Parent);
			transform.position = place.Point.position - renderer.bounds.center;
			transform.rotation = place.Point.rotation;
		}

		private BonusView SpawnBonus(int bonus)
		{
			BonusView bonusView = Object.Instantiate(_prefabData.Value.Prefabs.Bonus);
			bonusView.transform.position = _world.Get<SpawnCommand>(bonus).Coords;
			bonusView.Entity = bonus;
			_world.Add<BonusViewRef>(bonus)
				.Value = bonusView;
			return bonusView;
		}

		private ItemView SpawnItem(int bonus)
		{
			int item = _world.Get<Bonus>(bonus).Item;
			ItemView itemView = Object.Instantiate(GetItemPrefab(item));
			itemView.Entity = item;
			_world.Add<ItemViewRef>(item)
				.Value = itemView;
			return itemView;
		}

		private ItemView GetItemPrefab(int item)
		{
			ItemType type = _world.Get<ItemInfo>(item).Type;

			// TODO: add artifact type
			int eqType = GetEquipmentType(type, item);
			int shape = _world.Get<Shape>(item).PrefabIndex;
			return _prefabData.Value.GetItem(type, eqType, shape);
		}

		private int GetEquipmentType(ItemType type, int entity) =>
			type == ItemType.Armor
				? (int)_world.Get<ArmorInfo>(entity).Type
				: (int)_world.Get<WeaponInfo>(entity).Type;
	}
}