using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;

namespace RoomByRoom
{
	public class PickPlayerMainWeaponSystem : IEcsInitSystem
	{
		private readonly EcsFilterInject<Inc<WeaponInfo, Equipped>> _weapons = default;
		private EcsWorld _world;

		public void Init(IEcsSystems systems)
		{
			_world = systems.GetWorld();

			foreach (int index in _weapons.Value)
			{
				if (IsBow(index)) 
					continue;
				
				_world.Add<InHands>(index);
				AddMainWeapon(index);
			}
		}

		private bool IsBow(int item) => _world.Get<WeaponInfo>(item).Type == WeaponType.Bow;

		private void AddMainWeapon(int item) =>
			_world.Add<MainWeapon>(Utils.GetOwner(_world, item))
				.Entity = item;
	}
}