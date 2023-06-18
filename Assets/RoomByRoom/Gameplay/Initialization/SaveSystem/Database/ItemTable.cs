using LinqToDB.Mapping;

namespace RoomByRoom.Database
{
  [Table("item")]
  public class ItemTable : IComponentTable<BoundComponent<ItemInfo>>
  {
    [PrimaryKey]
    public int Id { get; set; }
    
    [Column("profile_name"), PrimaryKey]
    public string ProfileName { get; set; } 
    
    [Column("item_type")]
    public int ItemType { get; set; }

    public BoundComponent<ItemInfo> GetComponent() =>
      new BoundComponent<ItemInfo>
      {
        Entity = Id,
        ComponentInfo = new ItemInfo
        {
          Type = (ItemType)ItemType
        }
      };

    public void SetComponent(BoundComponent<ItemInfo> comp, string profileName)
    {
      Id = comp.Entity;
      ProfileName = profileName;
      ItemType = (int)comp.ComponentInfo.Type;
    }
  }
}