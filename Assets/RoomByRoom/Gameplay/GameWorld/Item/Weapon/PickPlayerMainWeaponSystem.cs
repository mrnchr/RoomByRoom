using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;

namespace RoomByRoom
{
	public class PickPlayerMainWeaponSystem : IEcsInitSystem
	{
		private EcsFilterInject<Inc<WeaponInfo, Equipped>> _weapons = default;

		public void Init(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();

			foreach (int index in _weapons.Value)
			{
				if (world.GetComponent<WeaponInfo>(index).Type != WeaponType.Bow)
				{
					world.AddComponent<InHands>(index);
					world.AddComponent<MainWeapon>(world.GetComponent<Owned>(index).Owner)
						.Assign(x =>
						{
							x.Entity = index;
							return x;
						});
				}
			}
		}
	}
}