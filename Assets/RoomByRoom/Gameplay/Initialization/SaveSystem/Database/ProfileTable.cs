using LinqToDB.Mapping;

namespace RoomByRoom.Database
{
  [Table("profile")]
  public class ProfileTable
  {
    [PrimaryKey]
    public string Name { get; set; }
    
    [Column("rm_count")]
    public int RoomCount { get; set; }
    
    [Column("pl_race")]
    public int PlayerRace { get; set; }
    
    [Column("pl_health")]
    public float PlayerHealth { get; set; }
    
    [Column("pl_max_health")]
    public float PlayerMaxHealth { get; set; }
    
    [Column("pl_speed")]
    public float PlayerSpeed { get; set; }
    
    [Column("pl_jump_force")]
    public float PlayerJumpForce { get; set; }
    
    [Column("rm_type")]
    public int RoomType { get; set; }
    
    [Column("rm_race")]
    public int RoomRace { get; set; }
  }
}