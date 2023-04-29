using System;
using System.Collections.Generic;
using UnityEngine;

namespace RoomByRoom
{
	public struct UnitViewRef
	{
		public UnitView Value;
	}

	[Serializable]
	public struct UnitInfo
	{
		public UnitType Type;
	}

	[Serializable]
	public struct RaceInfo
	{
		public RaceType Type;
	}

	[Serializable]
	public struct Inventory
	{
		[SerializeField] public List<int> ItemList;
	}

	[Serializable]
	public struct Equipment
	{
		[SerializeField] public List<int> ItemList;
	}

	[Serializable]
	public struct Backpack
	{
		[SerializeField] public List<int> ItemList;
	}
}