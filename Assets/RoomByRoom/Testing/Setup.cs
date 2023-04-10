using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using RoomByRoom.Utility;

public static class Setup
{
	public static IEcsSystems Systems<TSystem>(IEcsSystems systems, EcsWorld oneMoreWorld, TSystem testSystem)
		where TSystem : IEcsSystem
	{
		return systems
			.AddWorld(oneMoreWorld, Idents.Worlds.MessageWorld)
			.Add(testSystem)
			.Inject();
	}
}