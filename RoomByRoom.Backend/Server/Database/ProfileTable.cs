using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Database
{
    [Table("profile")]
    public class ProfileTable
    {
        public int Id { get; init; }

        [Column("user_id")]
        public int UserId { get; init; }
        public UserTable User { get; init; }
        
        public string? Progress { get; set; }
    }
}