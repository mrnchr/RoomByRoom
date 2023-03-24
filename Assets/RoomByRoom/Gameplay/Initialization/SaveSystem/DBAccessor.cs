using System.Data.SQLite;
using System;

namespace RoomByRoom
{
    public class DBAccessor : ISaver
    {
        private string _dbFileName = Utility.Idents.FilePaths.DatabaseFileName;
        private SQLiteConnection _conn;
        private SQLiteCommand _comm;

        public DBAccessor()
        {
            _conn = new SQLiteConnection("Data Source = " + _dbFileName);

            _conn.Open();
            _comm = new SQLiteCommand();
            _comm.Connection = _conn;
        }

        public bool LoadData(string fromProfile, ref SavedData savedData)
        {
            // Get data from db for current profile
            _comm.CommandText = $"select * from profile where name = \'{fromProfile}\';";
            SQLiteDataReader profileRow = _comm.ExecuteReader();

            if(!profileRow.Read())
            {
                return false;
            }
            int index = 1;

            savedData.GameInfo.RoomCount = profileRow.GetInt32(index++);
            savedData.Player.Race.Type = (RaceType)profileRow.GetInt32(index++);
            savedData.Player.Ground.Unit.Health.Point = profileRow.GetInt32(index++);
            savedData.Player.Ground.Unit.Moving.Speed = profileRow.GetInt32(index++);
            savedData.Player.Ground.Jumping.JumpForce = profileRow.GetInt32(index++);
            savedData.Room.Info.Type = (RoomType)profileRow.GetInt32(index++);
            savedData.Room.Race.Type = (RaceType)profileRow.GetInt32(index++);

            profileRow.Close();
            
            // Get inventory components from db
            _comm.CommandText = $"select * from item where profile_name = \'{fromProfile}\';";
            profileRow = _comm.ExecuteReader();

            while(profileRow.Read())
            {
                BoundComponent<ItemInfo> comp = new BoundComponent<ItemInfo>();
                comp.BoundEntity = profileRow.GetInt32(0);
                comp.ComponentInfo.Type = (ItemType)profileRow.GetInt32(2);
                savedData.Inventory.Item.Add(comp);
            }

            profileRow.Close();

            _comm.CommandText = $"select * from weapon where profile_name = \'{fromProfile}\';";
            profileRow = _comm.ExecuteReader();

            while(profileRow.Read())
            {
                BoundComponent<WeaponInfo> comp = new BoundComponent<WeaponInfo>();
                comp.BoundEntity = profileRow.GetInt32(0);
                comp.ComponentInfo.Type = (WeaponType)profileRow.GetInt32(2);
                savedData.Inventory.Weapon.Add(comp);
            }

            profileRow.Close();

            _comm.CommandText = $"select * from phys_damage where profile_name = \'{fromProfile}\';";
            profileRow = _comm.ExecuteReader();

            while(profileRow.Read())
            {
                BoundComponent<PhysicalDamage> comp = new BoundComponent<PhysicalDamage>();
                comp.BoundEntity = profileRow.GetInt32(0);
                comp.ComponentInfo.Point = profileRow.GetInt32(2);
                savedData.Inventory.PhysDamage.Add(comp);
            }

            profileRow.Close();

            _comm.CommandText = $"select * from equiped where profile_name = \'{fromProfile}\';";
            profileRow = _comm.ExecuteReader();

            while(profileRow.Read())
            {
                BoundComponent<Equipped> comp = new BoundComponent<Equipped>();
                comp.BoundEntity = profileRow.GetInt32(0);
                savedData.Inventory.Equipped.Add(comp);
            }

            profileRow.Close();

            _comm.CommandText = $"select * from protection where profile_name = \'{fromProfile}\';";
            profileRow = _comm.ExecuteReader();

            while(profileRow.Read())
            {
                BoundComponent<Protection> comp = new BoundComponent<Protection>();
                comp.BoundEntity = profileRow.GetInt32(0);
                comp.ComponentInfo.Point = profileRow.GetInt32(2);
                savedData.Inventory.Protection.Add(comp);
            }

            profileRow.Close();

            _comm.CommandText = $"select * from shape where profile_name = \'{fromProfile}\';";
            profileRow = _comm.ExecuteReader();

            while(profileRow.Read())
            {
                BoundComponent<Shape> comp = new BoundComponent<Shape>();
                comp.BoundEntity = profileRow.GetInt32(0);
                comp.ComponentInfo.PrefabIndex = profileRow.GetInt32(2);
                savedData.Inventory.Shape.Add(comp);
            }

            return true;
        }

        public bool SaveData(string toProfile, SavedData savedData)
        {
            // Save current profile
            _comm.CommandText = "insert or replace into profile values " +
            $"(" +
            $"\'{toProfile}\', " +
            $"{savedData.GameInfo.RoomCount}, " +
            $"{(int)savedData.Player.Race.Type}, " +
            $"{savedData.Player.Ground.Unit.Health.Point}, " +
            $"{savedData.Player.Ground.Unit.Moving.Speed}, " +
            $"{savedData.Player.Ground.Jumping.JumpForce}, " +
            $"{(int)savedData.Room.Info.Type}, " +
            $"{(int)savedData.Room.Race.Type} " +
            ");";
            _comm.ExecuteNonQuery();

            // Save current profile's inventory
            foreach(var comp in savedData.Inventory.Item)
            {
                _comm.CommandText = $"insert or replace into item values " +
                $"(" +
                $"{comp.BoundEntity}, " +
                $"\'{toProfile}\', " +
                $"{(int)comp.ComponentInfo.Type}" +
                $");";
                _comm.ExecuteNonQuery();
            }

            foreach(var comp in savedData.Inventory.Weapon)
            {
                _comm.CommandText = $"insert or replace into weapon values " +
                $"(" +
                $"{comp.BoundEntity}, " +
                $"\'{toProfile}\', " +
                $"{(int)comp.ComponentInfo.Type}" +
                $");";
                _comm.ExecuteNonQuery();
            }

            foreach(var comp in savedData.Inventory.PhysDamage)
            {
                _comm.CommandText = $"insert or replace into phys_damage values " +
                $"(" +
                $"{comp.BoundEntity}, " +
                $"\'{toProfile}\', " +
                $"{comp.ComponentInfo.Point}" +
                $");";
                _comm.ExecuteNonQuery();
            }

            foreach(var comp in savedData.Inventory.Equipped)
            {
                _comm.CommandText = $"insert or replace into equiped values " +
                $"(" +
                $"{comp.BoundEntity}, " +
                $"\'{toProfile}\' " +
                $");";
                _comm.ExecuteNonQuery();
            }

            foreach(var comp in savedData.Inventory.Protection)
            {
                _comm.CommandText = $"insert or replace into protection values " +
                $"(" +
                $"{comp.BoundEntity}, " +
                $"\'{toProfile}\', " +
                $"{comp.ComponentInfo.Point}" +
                $");";
                _comm.ExecuteNonQuery();
            }

            foreach(var comp in savedData.Inventory.Shape)
            {
                _comm.CommandText = $"insert or replace into shape values " +
                $"(" +
                $"{comp.BoundEntity}, " +
                $"\'{toProfile}\', " +
                $"{comp.ComponentInfo.PrefabIndex}" +
                $");";
                _comm.ExecuteNonQuery();
            }

            return true;
        }
    }
}