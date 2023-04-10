using Leopotam.EcsLite;
using RoomByRoom.Utility;

namespace RoomByRoom
{
	public class CharacteristicService
	{
		private readonly EcsWorld _world;

		public CharacteristicService(EcsWorld world)
		{
			_world = world;
		}

		public void Calculate(int unit)
		{
			CalcPhysProtection(unit);
		}

		private void CalcPhysProtection(int unit)
		{
			float totalProtection = 0;
			foreach (int index in _world.GetComponent<Equipment>(unit).ItemList)
			{
				if (!_world.HasComponent<ItemPhysicalProtection>(index))
					continue;

				totalProtection += _world.GetComponent<ItemPhysicalProtection>(index).Point;
			}

			// UnityEngine.Debug.Log($"Entity: {unit}, protection: {totalProtection}");
			_world.GetComponent<UnitPhysicalProtection>(unit)
				.Assign(x =>
				{
					x.MaxPoint = totalProtection;
					return x;
				});
		}
	}
}