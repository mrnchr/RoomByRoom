using System.Collections.Generic;
using System.Linq;
using LinqToDB;

namespace RoomByRoom.Database
{
  public class DBGameSaver : ISaver
  {
    private readonly DbAccessor _db;

    public DBGameSaver() => _db = new DbAccessor();

    public bool LoadData(string profile, ref Saving saving)
    {
      using var conn = _db.GetConnection();
      var profiles = from p in conn.TProfile
        where p.Name == profile
        select p;

      if (!profiles.Any()) return false;

      foreach (ProfileTable p in profiles)
      {
        saving.GameSave.RoomCount = p.RoomCount;
        saving.Player.Race.Type = (RaceType)p.PlayerRace;
        saving.Player.HealthCmp.CurrentPoint = p.PlayerHealth;
        saving.Player.HealthCmp.MaxPoint = p.PlayerMaxHealth;
        saving.Player.MovableCmp.Speed = p.PlayerSpeed;
        saving.Player.JumpableCmp.JumpForce = p.PlayerJumpForce;
        saving.Room.Info.Type = (RoomType)p.RoomType;
        saving.Room.Race.Type = (RaceType)p.RoomRace;
      }

      PullComps(conn.TArmor, saving.Inventory.Armor);
      PullComps(conn.TEquipped, saving.Inventory.Equipped);
      PullComps(conn.TItem, saving.Inventory.Item);
      PullComps(conn.TShape, saving.Inventory.Shape);
      PullComps(conn.TWeapon, saving.Inventory.Weapon);
      PullComps(conn.TPhysDamage, saving.Inventory.PhysDamage);
      PullComps(conn.TPhysProtection, saving.Inventory.PhysProtection);

      return true;

      void PullComps<T, TComp>(ITable<T> table, List<BoundComponent<TComp>> to)
        where T : IComponentTable<BoundComponent<TComp>>
        where TComp : struct =>
        to.AddRange(from c in table
                    where c.ProfileName == profile
                    select c.GetComponent());
    }

    public void SaveData(string profile, Saving saving)
    {
      DeleteData(profile);

      using var conn = _db.GetConnection();
      conn.TProfile.Insert(() => new ProfileTable
      {
        Name = profile,
        RoomCount = saving.GameSave.RoomCount,
        PlayerRace = (int)saving.Player.Race.Type,
        PlayerHealth = saving.Player.HealthCmp.CurrentPoint,
        PlayerMaxHealth = saving.Player.HealthCmp.MaxPoint,
        PlayerSpeed = saving.Player.MovableCmp.Speed,
        PlayerJumpForce = saving.Player.JumpableCmp.JumpForce,
        RoomType = (int)saving.Room.Info.Type,
        RoomRace = (int)saving.Room.Race.Type
      });

      GetTableComps<ItemTable, ItemInfo>(saving.Inventory.Item).ForEach(x => conn.Insert(x));
      GetTableComps<WeaponTable, WeaponInfo>(saving.Inventory.Weapon).ForEach(x => conn.Insert(x));
      GetTableComps<ArmorTable, ArmorInfo>(saving.Inventory.Armor).ForEach(x => conn.Insert(x));
      GetTableComps<ShapeTable, ShapeInfo>(saving.Inventory.Shape).ForEach(x => conn.Insert(x));
      GetTableComps<EquippedTable, Equipped>(saving.Inventory.Equipped).ForEach(x => conn.Insert(x));
      GetTableComps<PhysDamageTable, ItemPhysicalDamage>(saving.Inventory.PhysDamage).ForEach(x => conn.Insert(x));
      GetTableComps<PhysProtectionTable, ItemPhysicalProtection>(saving.Inventory.PhysProtection)
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