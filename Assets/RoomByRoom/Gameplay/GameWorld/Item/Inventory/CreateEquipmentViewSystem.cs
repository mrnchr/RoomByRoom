using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;
using UnityEngine;

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

			foreach (int index in _equipments.Value)
			{
				ItemType type = _world.Get<ItemInfo>(index).Type;

				// TODO: create view for all types of items
				if (type == ItemType.Artifact)
					continue;

				ItemView itemView = Object.Instantiate(GetPrefab(index, type));
				itemView.Entity = index;

				_world.Add<ItemViewRef>(index)
					.Value = itemView;

				Utils.PutItemInPlace(itemView.transform, GetPlace(index, type));

				if (type == ItemType.Weapon)
					itemView.gameObject.SetActive(_world.Has<InHands>(index));
			}
		}

		private ItemView GetPrefab(int entity, ItemType itemType)
		{
			int prefabIndex = _world.Get<Shape>(entity).PrefabIndex;
			int typeNumber = itemType == ItemType.Weapon
				? (int)_world.Get<WeaponInfo>(entity).Type
				: (int)_world.Get<ArmorInfo>(entity).Type;

			// Debug.Log($"itemType: {itemType}, typeNumber: {typeNumber}, prefabIndex: {prefabIndex}");
			return _prefabData.Value.GetItem(itemType, typeNumber, prefabIndex);
		}

		private ItemPlace GetPlace(int entity, ItemType itemType)
		{
			HumanoidView humanoid = GetHumanoidUnit(entity);

			return itemType == ItemType.Weapon
				? humanoid.GetWeaponPlace(_world.Get<WeaponInfo>(entity).Type)
				: humanoid.GetArmorPlace(_world.Get<ArmorInfo>(entity).Type);
		}

		private HumanoidView GetHumanoidUnit(int index) =>
			(HumanoidView)_world.Get<UnitViewRef>(Utils.GetOwner(_world, index)).Value;
	}
}