using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;

namespace RoomByRoom.Debugging
{
	public class AddInventoryInfoSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<UnitViewRef>, Exc<DebugInfo>> _units = default;
		private EcsWorld _world;

		public void Run(IEcsSystems systems)
		{
			_world = systems.GetWorld();

			foreach (int index in _units.Value)
			{
				var inv = _world.GetComponent<UnitViewRef>(index)
					.Value.gameObject.AddComponent<DebugInventory>();

				inv.Equipment = _world.GetComponent<Equipment>(index).ItemList;

				if (Utils.IsUnitOf(_world, index, UnitType.Player))
				{
					inv.Backpack = _world.GetComponent<Backpack>(index).ItemList;
					inv.Inventory = _world.GetComponent<Inventory>(index).ItemList;
				}

				_world.AddComponent<DebugInfo>(index).Inv = inv;
			}
		}
	}
}