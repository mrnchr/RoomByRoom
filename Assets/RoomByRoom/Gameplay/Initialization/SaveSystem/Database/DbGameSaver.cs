using System;
using System.Collections.Generic;
using System.Linq;
using LinqToDB;

namespace RoomByRoom.Database
{
  public class DbGameSaver : ISaver
  {
    private readonly DbAccessor _db;

    public DbGameSaver() => _db = new DbAccessor();

    public bool ProfileExists(string profile) =>
      _db.GetConnection().TProfile
        .Any(x => x.Name == profile);

    public GameSave LoadProfile(string profile)
    {
      if (!ProfileExists(profile)) throw new ArgumentException("This profile does not exist");

      GameSave gameSave = new GameSave();
      using var conn = _db.GetConnection();
      var profiles = from p in conn.TProfile
        where p.Name == profile
        select p;


      foreach (ProfileTable p in profiles)
      {
        gameSave.Game.RoomCount = p.RoomCount;
        gameSave.Player.Race.Type = (RaceType)p.PlayerRace;
        gameSave.Player.HealthCmp.CurrentPoint = p.PlayerHealth;
        gameSave.Player.HealthCmp.MaxPoint = p.PlayerMaxHealth;
        gameSave.Player.MovableCmp.Speed = p.PlayerSpeed;
        gameSave.Player.JumpableCmp.JumpForce = p.PlayerJumpForce;
        gameSave.Room.Info.Type = (RoomType)p.RoomType;
        gameSave.Room.Race.Type = (RaceType)p.RoomRace;
      }

      PullComps(conn.TArmor, gameSave.InventorySave.Armor);
      PullComps(conn.TEquipped, gameSave.InventorySave.Equipped);
      PullComps(conn.TItem, gameSave.InventorySave.Item);
      PullComps(conn.TShape, gameSave.InventorySave.Shape);
      PullComps(conn.TWeapon, gameSave.InventorySave.Weapon);
      PullComps(conn.TPhysDamage, gameSave.InventorySave.PhysDamage);
      PullComps(conn.TPhysProtection, gameSave.InventorySave.PhysProtection);

      void PullComps<T, TComp>(ITable<T> table, List<BoundComponent<TComp>> to)
        where T : IComponentTable<BoundComponent<TComp>>
        where TComp : struct =>
        to.AddRange(from c in table
                    where c.ProfileName == profile
                    select c.GetComponent());

      return gameSave;
    }

    public void SaveProfile(string profile, GameSave gameSave)
    {
      DeleteData(profile);

      using var conn = _db.GetConnection();
      conn.TProfile.Insert(() => new ProfileTable
      {
        Name = profile,
        RoomCount = gameSave.Game.RoomCount,
        PlayerRace = (int)gameSave.Player.Race.Type,
        PlayerHealth = gameSave.Player.HealthCmp.CurrentPoint,
        PlayerMaxHealth = gameSave.Player.HealthCmp.MaxPoint,
        PlayerSpeed = gameSave.Player.MovableCmp.Speed,
        PlayerJumpForce = gameSave.Player.JumpableCmp.JumpForce,
        RoomType = (int)gameSave.Room.Info.Type,
        RoomRace = (int)gameSave.Room.Race.Type
      });

      GetTableComps<ItemTable, ItemInfo>(gameSave.InventorySave.Item).ForEach(x => conn.Insert(x));
      GetTableComps<WeaponTable, WeaponInfo>(gameSave.InventorySave.Weapon).ForEach(x => conn.Insert(x));
      GetTableComps<ArmorTable, ArmorInfo>(gameSave.InventorySave.Armor).ForEach(x => conn.Insert(x));
      GetTableComps<ShapeTable, ShapeInfo>(gameSave.InventorySave.Shape).ForEach(x => conn.Insert(x));
      GetTableComps<EquippedTable, Equipped>(gameSave.InventorySave.Equipped).ForEach(x => conn.Insert(x));
      GetTableComps<PhysDamageTable, ItemPhysicalDamage>(gameSave.InventorySave.PhysDamage).ForEach(x => conn.Insert(x));
      GetTableComps<PhysProtectionTable, ItemPhysicalProtection>(gameSave.InventorySave.PhysProtection)
        .ForEach(x => conn.Insert(x));

      List<T> GetTableComps<T, TValue>(List<BoundComponent<TValue>> comps)
        where T : IComponentTable<BoundComponent<TValue>>, new()
        where TValue : struct
      {
        List<T> tableComps = new List<T>();
        foreach (var comp in comps)
        {
          T tComp = new T();
          tComp.SetComponent(comp, profile);
          tableComps.Add(tComp);
        }

        return tableComps;
      }
    }

    private void DeleteData(string profile)
    {
      using var conn = _db.GetConnection();
      conn.TProfile
        .Where(x => x.Name == profile)
        .Delete();
    }
  }
}