using LinqToDB.Mapping;

namespace RoomByRoom.Database
{
  [Table("equipped")]
  public class EquippedTable : IComponentTable<BoundComponent<Equipped>>
  {
    [PrimaryKey]
    public int Id { get; set; }
    
    [Column("profile_name"), PrimaryKey]
    public string ProfileName { get; set; } 
    
    public BoundComponent<Equipped> GetComponent() =>
      new BoundComponent<Equipped>
      {
        Entity = Id
      };

    public void SetComponent(BoundComponent<Equipped> comp, string profileName)
    {
      Id = comp.Entity;
      ProfileName = profileName;
    }
  }
}