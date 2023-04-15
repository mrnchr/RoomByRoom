using System.Data.SQLite;
using System.Collections.Generic;

namespace RoomByRoom.Database
{
	public class DBAccessor : ISaver
	{
		protected string _dbFileName = Utility.Idents.FilePaths.DatabaseFileName;
		protected SQLiteConnection _conn;
		protected SQLiteCommand _comm;

		public DBAccessor()
		{
			_conn = new SQLiteConnection("Data Source = " + _dbFileName);

			_conn.Open();
			_comm = new SQLiteCommand
			{
				Connection = _conn,
				// turn on foreign references
				CommandText = "PRAGMA foreign_keys=ON"
			};
			_comm.ExecuteNonQuery();
		}

		public bool LoadData(string profile, ref Saving saving)
		{
			_comm.CommandText = $"select * from profile where name = \'{profile}\';";
			SQLiteDataReader profileRow = _comm.ExecuteReader();

			if (!profileRow.Read())
			{
				profileRow.Close();
				return false;
			}

			int index = 1;

			saving.GameSave.RoomCount = profileRow.GetInt32(index++);
			saving.Player.Race.Type = (RaceType)profileRow.GetInt32(index++);
			saving.Player.HealthCmp.MaxPoint = profileRow.GetFloat(index++);
			saving.Player.MovableCmp.Speed = profileRow.GetFloat(index++);
			saving.Player.JumpableCmp.JumpForce = profileRow.GetFloat(index++);
			saving.Room.Info.Type = (RoomType)profileRow.GetInt32(index++);
			saving.Room.Race.Type = (RaceType)profileRow.GetInt32(index++);

			profileRow.Close();

			List<BoundComponent<TValue>> PullComponent<T, TValue>()
				where T : ITable<BoundComponent<TValue>>, new()
				where TValue : struct
			{
				T compTable = new();
				_comm.CommandText = $"select * from {compTable.GetTableName()} where profile_name = \'{profile}\';";
				profileRow = _comm.ExecuteReader();
				List<BoundComponent<TValue>> comps = new();

				while (profileRow.Read())
				{
					comps.Add(compTable.Pull(profileRow));
				}

				profileRow.Close();
				return comps;
			}

			saving.Inventory.Item = PullComponent<ItemTable, ItemInfo>();
			saving.Inventory.Weapon = PullComponent<WeaponTable, WeaponInfo>();
			saving.Inventory.Armor = PullComponent<ArmorTable, ArmorInfo>();
			saving.Inventory.PhysProtection = PullComponent<ProtectionTable, ItemPhysicalProtection>();
			saving.Inventory.PhysDamage = PullComponent<PhysicalDamageTable, ItemPhysicalDamage>();
			saving.Inventory.Equipped = PullComponent<EquippedTable, Equipped>();
			saving.Inventory.Shape = PullComponent<ShapeTable, Shape>();

			return true;
		}

		public void DeleteData(string profile)
		{
			_comm.CommandText = $"delete from profile where name = \'{profile}\';";
			_comm.ExecuteNonQuery();
		}

		public void SaveData(string profile, Saving saving)
		{
			DeleteData(profile);

			// Save current profile
			_comm.CommandText = "insert or replace into profile values " +
			                    $"(" +
			                    $"\'{profile}\', " +
			                    $"{saving.GameSave.RoomCount}, " +
			                    $"{(int)saving.Player.Race.Type}, " +
			                    $"{saving.Player.HealthCmp.MaxPoint}, " +
			                    $"{saving.Player.MovableCmp.Speed}, " +
			                    $"{saving.Player.JumpableCmp.JumpForce}, " +
			                    $"{(int)saving.Room.Info.Type}, " +
			                    $"{(int)saving.Room.Race.Type} " +
			                    ");";
			_comm.ExecuteNonQuery();

			// Save current profile's inventory
			void PutComponent<T, TValue>(List<BoundComponent<TValue>> comps)
				where T : ITable<BoundComponent<TValue>>, new()
				where TValue : struct
			{
				T compTable = new();
				foreach (var comp in comps)
				{
					_comm.CommandText = compTable.GetTextToPut(comp, profile);
					_comm.ExecuteNonQuery();
				}
			}

			PutComponent<ItemTable, ItemInfo>(saving.Inventory.Item);
			PutComponent<WeaponTable, WeaponInfo>(saving.Inventory.Weapon);
			PutComponent<ArmorTable, ArmorInfo>(saving.Inventory.Armor);
			PutComponent<ProtectionTable, ItemPhysicalProtection>(saving.Inventory.PhysProtection);
			PutComponent<PhysicalDamageTable, ItemPhysicalDamage>(saving.Inventory.PhysDamage);
			PutComponent<EquippedTable, Equipped>(saving.Inventory.Equipped);
			PutComponent<ShapeTable, Shape>(saving.Inventory.Shape);
		}
	}
}