using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom.Testing
{
	public static class Setup
	{
		public static IEcsSystems Systems(IEcsSystems systems, EcsWorld oneMoreWorld = null, params IEcsSystem[] system)
		{
			if (oneMoreWorld != null)
				systems.AddWorld(oneMoreWorld, Idents.Worlds.MessageWorld);

			foreach (IEcsSystem sys in system)
				systems.Add(sys);

			return systems
				.Inject();
		}

		public static WeaponView WeaponViewF(WeaponView view)
		{
			view.AttackTriggers = new Collider[] { view.gameObject.AddComponent<BoxCollider>() };
			return view;
		}
	}
}