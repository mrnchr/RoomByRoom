using LinqToDB.Mapping;

namespace RoomByRoom.Database
{
  [Table("phys_damage")]
  public class PhysDamageTable : IComponentTable<BoundComponent<ItemPhysicalDamage>>
  {
    [PrimaryKey, Column("id")]
    public int Id { get; set; }
    
    [PrimaryKey, Column("profile_name")]
    public string ProfileName { get; set; } 
    
    [Column("point")]
    public float Point { get; set; }

    public BoundComponent<ItemPhysicalDamage> GetComponent() =>
      new BoundComponent<ItemPhysicalDamage>
      {
        Entity = Id,
        ComponentInfo = new ItemPhysicalDamage
        {
          Point = Point
        }
      };

    public void SetComponent(BoundComponent<ItemPhysicalDamage> comp, string profileName)
    {
      Id = comp.Entity;
      ProfileName = profileName;
      Point = comp.ComponentInfo.Point;
    }
  }
}