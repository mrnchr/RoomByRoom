using Leopotam.EcsLite;
using UnityEngine;

namespace RoomByRoom.PlayTesting
{
	public abstract class TestEngine : MonoBehaviour
	{
		public EcsWorld World;
		public EcsWorld Message;
		public IEcsSystems Systems;
		public AttackService AttackSvc;

		public abstract void Start();

		public virtual void Update()
		{
			Systems?.Run();
		}

		public virtual void OnDestroy()
		{
			if (Systems != null)
			{
				Systems.Destroy();
				Systems = null;
			}

			if (World != null)
			{
				World.Destroy();
				World = null;
			}
		}
	}
}