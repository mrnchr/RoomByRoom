using UnityEngine;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;

namespace RoomByRoom
{
	public class CreateEquipmentViewSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<Equipped>, Exc<ItemViewRef>> _equipments = default;
		private readonly EcsCustomInject<PackedPrefabData> _prefabData = default;
		private EcsWorld _world;

		public void Run(IEcsSystems systems)
		{
			_world = systems.GetWorld();

			foreach (var index in _equipments.Value)
			{
				ItemType type = _world.GetComponent<ItemInfo>(index).Type;

				// TODO: create view for all types of items
				if (type == ItemType.Artifact)
					continue;

				ItemView itemView = Object.Instantiate(GetPrefab(index, type));
				itemView.Entity = index;

				_world.AddComponent<ItemViewRef>(index)
					.Value = itemView;

				Utils.PutItemInPlace(itemView.transform, GetPlace(index, type));

				if (type == ItemType.Weapon)
					itemView.gameObject.SetActive(_world.HasComponent<InHands>(index));
			}
		}

		private ItemView GetPrefab(int entity, ItemType itemType)
		{
			int prefabIndex = _world.GetComponent<Shape>(entity).PrefabIndex;

			int typeNumber =
				itemType == ItemType.Weapon
					? (int)_world.GetComponent<WeaponInfo>(entity).Type
					: (int)_world.GetComponent<ArmorInfo>(entity).Type;

			// Debug.Log($"itemType: {itemType}, typeNumber: {typeNumber}, prefabIndex: {prefabIndex}");
			return _prefabData.Value.GetItem(itemType, typeNumber, prefabIndex);
		}

		private ItemPlace GetPlace(int entity, ItemType itemType)
		{
			HumanoidView humanoid = GetHumanoidUnit(entity);

			return itemType == ItemType.Weapon
				? humanoid.GetWeaponPlace(_world.GetComponent<WeaponInfo>(entity).Type)
				: humanoid.GetArmorPlace(_world.GetComponent<ArmorInfo>(entity).Type);
		}

		private HumanoidView GetHumanoidUnit(int index)
		{
			return (HumanoidView)_world.GetComponent<UnitViewRef>(
					_world.GetComponent<Owned>(index).Owner)
				.Value;
		}
	}
}