using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using NUnit.Framework;
using RoomByRoom.Utility;

namespace RoomByRoom.Testing
{
	public static class Setup
	{
		public static IEcsSystems Systems(IEcsSystems systems, EcsWorld oneMoreWorld = null, params IEcsSystem[] system)
		{
			if (oneMoreWorld != null)
				systems.AddWorld(oneMoreWorld, Idents.Worlds.MessageWorld);
			
			foreach (var sys in system)
				systems.Add(sys);
			
			return systems
					.Inject();
		}
	}
}