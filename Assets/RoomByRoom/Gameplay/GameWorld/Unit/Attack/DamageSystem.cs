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
				physProtection.CurrentPoint -= physDamage;
				if (physProtection.CurrentPoint < 0)
				{
					ref Health health = ref _world.GetComponent<Health>(message.Damaged);

					health.CurrentPoint += physProtection.CurrentPoint;
					if (health.CurrentPoint < 0)
						health.CurrentPoint = 0;

					physProtection.CurrentPoint = 0;
					// UnityEngine.Debug.Log($"Health after damage: {health.Point}");
				}

				// To be created from the service because not to be deleted by DelHere
				_messages.Pools.Inc1.Del(index);
			}
		}
	}
}