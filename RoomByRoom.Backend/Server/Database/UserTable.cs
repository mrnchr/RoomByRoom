using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Server.Database
{
    [Table("user")]
    public class UserTable
    {
        public int Id { get; init; }
        public string Name { get; init; } = "";
        public string Password { get; init; } = "";
        public string Salt { get; init; } = "";
    }
}