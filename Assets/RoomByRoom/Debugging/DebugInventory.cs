using System;
using System.Collections.Generic;
using System.Linq;
using Leopotam.EcsLite;
using RoomByRoom.Utility;
using UnityEngine;
using Packed = Leopotam.EcsLite.EcsPackedEntity;

namespace RoomByRoom.Debugging
{
	public class DebugInventory : MonoBehaviour
	{
		public List<int> RawInventory;
		public List<int> RawEquipment;
		public List<int> RawBackpack;
		
		private EcsWorld _world;
		public List<Packed> Inventory;
		public List<Packed> Equipment;
		public List<Packed> Backpack;

		public void Construct(EcsWorld world)
		{
			_world = world;
		}

		private void Update()
		{
			RawInventory = Inventory?.Select(_world.Unpack).ToList();
			RawEquipment = Equipment?.Select(_world.Unpack).ToList();
			RawBackpack = Backpack?.Select(_world.Unpack).ToList();
		}
	}
}