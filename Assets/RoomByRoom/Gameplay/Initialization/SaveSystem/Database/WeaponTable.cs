using LinqToDB.Mapping;

namespace RoomByRoom.Database
{
  [Table("weapon")]
  public class WeaponTable : IComponentTable<BoundComponent<WeaponInfo>>
  {
    [PrimaryKey, Column("id")]
    public int Id { get; set; }
    
    [PrimaryKey, Column("profile_name")]
    public string ProfileName { get; set; } 
    
    [Column("weapon_type")]
    public int WeaponType { get; set; }

    public BoundComponent<WeaponInfo> GetComponent() =>
      new BoundComponent<WeaponInfo>
      {
        BoundEntity = Id,
        ComponentInfo = new WeaponInfo
        {
          Type = (WeaponType) WeaponType
        }
      };

    public void SetComponent(BoundComponent<WeaponInfo> comp, string profileName)
    {
      Id = comp.BoundEntity;
      ProfileName = profileName;
      WeaponType = (int)comp.ComponentInfo.Type;
    }
  }
}