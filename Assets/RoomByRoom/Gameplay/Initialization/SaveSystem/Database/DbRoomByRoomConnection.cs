using LinqToDB;
using LinqToDB.Data;

namespace RoomByRoom.Database
{
  public class DbRoomByRoomConnection : DataConnection
  {
    public DbRoomByRoomConnection(DataOptions dataOptions) : base(dataOptions) {}

    public ITable<ProfileTable> TProfile => this.GetTable<ProfileTable>();
    public ITable<ArmorTable> TArmor => this.GetTable<ArmorTable>();
    public ITable<EquippedTable> TEquipped => this.GetTable<EquippedTable>();
    public ITable<ItemTable> TItem => this.GetTable<ItemTable>();
    public ITable<ShapeTable> TShape => this.GetTable<ShapeTable>();
    public ITable<WeaponTable> TWeapon => this.GetTable<WeaponTable>();
    public ITable<PhysDamageTable> TPhysDamage => this.GetTable<PhysDamageTable>();
    public ITable<PhysProtectionTable> TPhysProtection => this.GetTable<PhysProtectionTable>();
  }
}