using PoolBrackets_backend_dotnet.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace PoolBrackets_backend_dotnet.Models
{
    [Table("user")]
    public class User
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public required string Username { get; set; }
        [Column("password")]
        public required string Password { get; set; }
        [Column("email")]
        public required string Email { get; set; }
        [Column("is_active")]
        public required bool IsActive { get; set; }
        [Column("role")]
        public required UserRoleEnum Role { get; set; }
        [Column("create_at")]
        public DateTime? CreatedAt { get; set; }
        [Column("update_at")]
        public DateTime? UpdateAt { get; set; }
    }
}
 