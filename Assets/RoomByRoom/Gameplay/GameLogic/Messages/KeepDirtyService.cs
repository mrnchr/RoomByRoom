using Leopotam.EcsLite;
using RoomByRoom.UI.Game;
using RoomByRoom.Utility;

namespace RoomByRoom
{
	public class KeepDirtyService
	{
		private readonly EcsWorld _message;
		private EcsPackedEntity _entity;

		public KeepDirtyService(EcsWorld message) => _message = message;

		public void UpdateDirtyMessage(DirtyType dirty)
		{
			int entity = _message.Unpack(_entity);
			if (entity == -1 || !_message.Has<DirtyMessage>(entity))
			{
				entity = _message.NewEntity();
				_message.Add<DirtyMessage>(entity);
			}

			_message.Get<DirtyMessage>(entity)
				.DirtyFlags |= dirty;

			_entity = _message.PackEntity(entity);
		}
	}
}