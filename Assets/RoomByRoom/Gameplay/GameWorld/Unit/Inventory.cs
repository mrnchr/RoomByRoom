﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace RoomByRoom
{
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