using System.Collections;
using FluentAssertions;
using UnityEngine;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using RoomByRoom.Utility;

namespace RoomByRoom
{
	public class DamageSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<GetDamageMessage>> _messages = Idents.Worlds.MessageWorld;
		private EcsWorld _world;

		public void Run(IEcsSystems systems)
		{
			_world = systems.GetWorld();

			foreach (int index in _messages.Value)
			{
				ref GetDamageMessage message = ref _messages.Pools.Inc1.Get(index);
				ref UnitPhysicalProtection physProtection = ref _world.GetComponent<UnitPhysicalProtection>(message.Damaged);
				float physDamage = _world.GetComponent<ItemPhysicalDamage>(message.Weapon).Point;

				// UnityEngine.Debug.Log($"Damage: {physDamage.Point}, protection: {protection}, health: {health.Point}");

				UpdateCantRestoreTime(index, physProtection.CantRestoreTime);
				physProtection.CurrentPoint -= physDamage;
				if (physProtection.CurrentPoint < 0)
				{
					ref Health health = ref _world.GetComponent<Health>(message.Damaged);

					health.CurrentPoint += physProtection.CurrentPoint;
					health.CurrentPoint.Clamp(min: 0);
					
					physProtection.CurrentPoint = 0;
					// UnityEngine.Debug.Log($"Health after damage: {health.Point}");
				}

				// is sended from the service so is not deleted by DelHere
				_messages.Pools.Inc1.Del(index);
			}
		}

		private void UpdateCantRestoreTime(int entity, float maxTime)
		{
			(_world.HasComponent<CantRestore>(entity)
				? ref _world.GetComponent<CantRestore>(entity)
				: ref _world.AddComponent<CantRestore>(entity))
					.TimeLeft = maxTime;
		}
	}
}