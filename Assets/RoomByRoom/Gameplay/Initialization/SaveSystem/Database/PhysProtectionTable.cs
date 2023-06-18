using LinqToDB.Mapping;

namespace RoomByRoom.Database
{
  [Table("phys_protection")]
  public class PhysProtectionTable : IComponentTable<BoundComponent<ItemPhysicalProtection>>
  {
    [PrimaryKey, Column("id")]
    public int Id { get; set; }
    
    [PrimaryKey, Column("profile_name")]
    public string ProfileName { get; set; } 
    
    [Column("point")]
    public float Point { get; set; }

    public BoundComponent<ItemPhysicalProtection> GetComponent() =>
      new BoundComponent<ItemPhysicalProtection>
      {
        Entity = Id,
        ComponentInfo = new ItemPhysicalProtection
        {
          Point = Point
        }
      };

    public void SetComponent(BoundComponent<ItemPhysicalProtection> comp, string profileName)
    {
      Id = comp.Entity;
      ProfileName = profileName;
      Point = comp.ComponentInfo.Point;
    }
  }
}