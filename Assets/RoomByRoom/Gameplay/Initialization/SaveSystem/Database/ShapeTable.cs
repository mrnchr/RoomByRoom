using LinqToDB.Mapping;

namespace RoomByRoom.Database
{
  [Table("shape")]
  public class ShapeTable : IComponentTable<BoundComponent<ShapeInfo>>
  {
    [PrimaryKey, Column("id")]
    public int Id { get; set; }
    
    [PrimaryKey, Column("profile_name")]
    public string ProfileName { get; set; } 
    
    [Column("pref_index")]
    public int PrefabIndex { get; set; }

    public BoundComponent<ShapeInfo> GetComponent() =>
      new BoundComponent<ShapeInfo>
      {
        BoundEntity = Id,
        ComponentInfo = new ShapeInfo
        {
          PrefabIndex = PrefabIndex
        }
      };

    public void SetComponent(BoundComponent<ShapeInfo> comp, string profileName)
    {
      Id = comp.BoundEntity;
      ProfileName = profileName;
      PrefabIndex = comp.ComponentInfo.PrefabIndex;
    }
  }
}