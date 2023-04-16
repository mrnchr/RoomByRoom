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
		private EcsWorld _world;
		private EcsWorld _message;

		public void Run(IEcsSystems systems)
		{
			_world = systems.GetWorld();
			_message = systems.GetWorld(Idents.Worlds.MessageWorld);

			foreach (int index in _delayMsgs.Value)
			{
				int unit = _message.GetComponent<DelayAttackMessage>(index).Unit;
				Utils.UpdateTimer<CantAttack>(_world, unit, _enemyCfg.Value.DelayAttackTime);
				_message.DelEntity(index);
			}
		}
	}
}