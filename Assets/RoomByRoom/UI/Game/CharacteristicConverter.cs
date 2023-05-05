using System;
using System.Collections.Generic;

namespace RoomByRoom.UI.Game
{
	public class CharacteristicConverter
	{
		private readonly Dictionary<Type, string> _charStrings = new Dictionary<Type, string>();

		public CharacteristicConverter(CharacteristicStrings config)
		{
			_charStrings[typeof(ItemPhysicalProtection)] = config.ItemPhysicalProtectionStr;
			_charStrings[typeof(ItemPhysicalDamage)] = config.ItemPhysicalDamageStr;
		}

		public string this[Type t] => _charStrings[t];
		public string GetString<T>() => _charStrings[typeof(T)];
	}
}