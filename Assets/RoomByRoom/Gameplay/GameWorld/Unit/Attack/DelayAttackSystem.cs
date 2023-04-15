using System.Collections;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom
{
	public class DelayAttackSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<DelayAttackMessage>> _delayMsgs = Idents.Worlds.MessageWorld;
		private readonly EcsCustomInject<EnemyData> _enemyCfg = default;
		private readonly EcsCustomInject<CoroutineStarter> _coroutineStarter = default;
		private EcsWorld _world;
		private EcsWorld _message;

		public void Run(IEcsSystems systems)
		{
			_world = systems.GetWorld();
			_message = systems.GetWorld(Idents.Worlds.MessageWorld);
			
			foreach (int index in _delayMsgs.Value)
			{
				_coroutineStarter.Value.PerformCoroutine(WaitToAttack(_message.GetComponent<DelayAttackMessage>(index).Unit));
				_message.DelEntity(index);
			}
		}

		private IEnumerator WaitToAttack(int unit)
		{
			yield return new WaitForSeconds(_enemyCfg.Value.DelayAttackTime);
			_world.DelComponent<CantAttack>(unit);
		}
	}
}