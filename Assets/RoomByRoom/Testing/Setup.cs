using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using NUnit.Framework;
using RoomByRoom.Utility;

namespace RoomByRoom.Testing
{
	public static class Setup
	{
		public static IEcsSystems Systems<TSystem>(IEcsSystems systems, TSystem testSystem, EcsWorld oneMoreWorld = null)
			where TSystem : IEcsSystem
		{
			if (oneMoreWorld != null)
				systems.AddWorld(oneMoreWorld, Idents.Worlds.MessageWorld);
			return systems
				.Add(testSystem)
				.Inject();
		}
	}
}