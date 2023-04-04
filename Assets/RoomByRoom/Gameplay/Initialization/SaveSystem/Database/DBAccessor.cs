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
            _comm = new SQLiteCommand();
            _comm.Connection = _conn;

            // turn on foreign references
            _comm.CommandText = "PRAGMA foreign_keys=ON";
            _comm.ExecuteNonQuery();
        }

        public bool LoadData(string profile, ref SavedData savedData)
        {
            _comm.CommandText = $"select * from profile where name = \'{profile}\';";
            SQLiteDataReader profileRow = _comm.ExecuteReader();

            if(!profileRow.Read())
            {
                profileRow.Close();
                return false;
            }

            int index = 1;

            savedData.GameSave.RoomCount = profileRow.GetInt32(index++);
            savedData.Player.Race.Type = (RaceType)profileRow.GetInt32(index++);
            savedData.Player.HealthCmp.Point = profileRow.GetFloat(index++);
            savedData.Player.MovingCmp.Speed = profileRow.GetFloat(index++);
            savedData.Player.JumpingCmp.JumpForce = profileRow.GetFloat(index++);
            savedData.Room.Info.Type = (RoomType)profileRow.GetInt32(index++);
            savedData.Room.Race.Type = (RaceType)profileRow.GetInt32(index++);

            profileRow.Close();

            List<BoundComponent<TValue>> PullComponent<T, TValue>()
            where T : ITable<BoundComponent<TValue>>, new()
            where TValue : struct
            {
                T compTable = new T();
                _comm.CommandText = $"select * from {compTable.GetTableName()} where profile_name = \'{profile}\';";
                profileRow = _comm.ExecuteReader();
                List<BoundComponent<TValue>> comps = new List<BoundComponent<TValue>>();

                while(profileRow.Read())
                {
                    comps.Add(compTable.Pull(profileRow));
                }

                profileRow.Close();
                return comps;
            }

            savedData.Inventory.Item = PullComponent<ItemTable, ItemInfo>();
            savedData.Inventory.Weapon = PullComponent<WeaponTable, WeaponInfo>();
            savedData.Inventory.Protection = PullComponent<ProtectionTable, Protection>();
            savedData.Inventory.PhysDamage = PullComponent<PhysicalDamageTable, PhysicalDamage>();
            savedData.Inventory.Equipped = PullComponent<EquippedTable, Equipped>();
            savedData.Inventory.Shape = PullComponent<ShapeTable, Shape>();

            return true;
        }

        public void DeleteData(string profile)
        {
            _comm.CommandText = $"delete from profile where name = \'{profile}\';";
            _comm.ExecuteNonQuery();

        }

        public void SaveData(string profile, SavedData savedData)
        {
            DeleteData(profile);

            // Save current profile
            _comm.CommandText = "insert or replace into profile values " +
            $"(" +
            $"\'{profile}\', " +
            $"{savedData.GameSave.RoomCount}, " +
            $"{(int)savedData.Player.Race.Type}, " +
            $"{savedData.Player.HealthCmp.Point}, " +
            $"{savedData.Player.MovingCmp.Speed}, " +
            $"{savedData.Player.JumpingCmp.JumpForce}, " +
            $"{(int)savedData.Room.Info.Type}, " +
            $"{(int)savedData.Room.Race.Type} " +
            ");";
            _comm.ExecuteNonQuery();

            // Save current profile's inventory
            void PutComponent<T, TValue>(List<BoundComponent<TValue>> comps)
            where T : ITable<BoundComponent<TValue>>, new()
            where TValue : struct
            {
                T compTable = new T();
                foreach (var comp in comps)
                {
                    _comm.CommandText = compTable.GetTextToPut(comp, profile);
                    _comm.ExecuteNonQuery();
                }
            }

            PutComponent<ItemTable, ItemInfo>(savedData.Inventory.Item);
            PutComponent<WeaponTable, WeaponInfo>(savedData.Inventory.Weapon);
            PutComponent<ProtectionTable, Protection>(savedData.Inventory.Protection);
            PutComponent<PhysicalDamageTable, PhysicalDamage>(savedData.Inventory.PhysDamage);
            PutComponent<EquippedTable, Equipped>(savedData.Inventory.Equipped);
            PutComponent<ShapeTable, Shape>(savedData.Inventory.Shape);
        }
    }
}