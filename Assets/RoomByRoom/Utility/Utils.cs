using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

namespace RoomByRoom.Utility
{
	public static class Utils
	{
		public static string ListToString<T>(List<T> list)
		{
			string s = "{ ";
			for (int i = 0; i < list.Count; i++)
			{
				s += list[i].ToString();
				if (i < list.Count - 1)
					s += ", ";
			}

			s += " }";
			return s;
		}

		public static string Print<T>(this List<T> list)
		{
			string s = "{ ";
			for (int i = 0; i < list.Count; i++)
			{
				s += list[i].ToString();
				if (i < list.Count - 1)
					s += ", ";
			}

			s += " }";
			return s;
		}
		
		public static void SetTransform(Transform from, Transform to)
		{
			from.position = to.position;
			from.rotation = to.rotation;
		}

		public static int GetEnumLength<T>()
			where T : Enum => Enum.GetNames(typeof(T)).Length;

		public static bool IsPlayer(EcsWorld world, int index)
		{
			return world.GetComponent<UnitInfo>(index).Type == UnitType.Player;
		}

		public static void AddItemToList(List<int> list, int item)
		{
			if (list.Contains(item))
				throw new ArgumentException("You try to add an item which is in the item list");
			list.Add(item);
		}
	}
}