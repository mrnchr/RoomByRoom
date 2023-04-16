using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace RoomByRoom
{
	public class DelTimerSystem<T> : IEcsRunSystem
	where T : struct, ITimerable
	{
		private readonly EcsFilterInject<Inc<T>> _timers = default;

		public void Run(IEcsSystems systems)
		{
			foreach (int index in _timers.Value)
			{
				if(_timers.Pools.Inc1.Get(index).TimeLeft <= 0)
					_timers.Pools.Inc1.Del(index);
			}
		}
	}
}