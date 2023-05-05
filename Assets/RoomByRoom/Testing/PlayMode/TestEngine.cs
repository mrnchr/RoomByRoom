using System;
using Leopotam.EcsLite;
using UnityEngine;

namespace RoomByRoom.Testing.PlayMode
{
	public abstract class TestEngine : MonoBehaviour
	{
		public AttackService AttackSvc;
		public EcsWorld Message;
		public IEcsSystems Systems;
		public EcsWorld World;

		public void Awake()
		{
			World = new EcsWorld();
			Message = new EcsWorld();
			AttackSvc = new AttackService(World, Message);
			Systems = new EcsSystems(World);
		}

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