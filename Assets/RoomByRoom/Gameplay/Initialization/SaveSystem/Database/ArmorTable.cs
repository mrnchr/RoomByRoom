using LinqToDB.Mapping;

namespace RoomByRoom.Database
{
  [Table("armor")]
  public class ArmorTable : IComponentTable<BoundComponent<ArmorInfo>>
  {
    [PrimaryKey, Column("id")] public int Id { get; set; }

    [PrimaryKey, Column("profile_name")] public string ProfileName { get; set; }

    [Column("armor_type")] public int ArmorType { get; set; }

    public BoundComponent<ArmorInfo> GetComponent() =>
      new BoundComponent<ArmorInfo>
      {
        Entity = Id,
        ComponentInfo = new ArmorInfo
        {
          Type = (ArmorType)ArmorType
        }
      };

    public void SetComponent(BoundComponent<ArmorInfo> comp, string profileName)
    {
      Id = comp.Entity;
      ProfileName = profileName;
      ArmorType = (int)comp.ComponentInfo.Type;
    }
  }
}