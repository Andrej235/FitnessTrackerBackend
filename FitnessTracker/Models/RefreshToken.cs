using ProjectGym.Models;
using ProjectGym.Services.DatabaseSerialization;
using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Models
{
    public class RefreshToken
    {
        [Key]
        public Guid Token { get; set; }
        public Guid JwtId { get; set; }
        public DateTime ExpiryDate { get; set; }

        [ModelReference("User")]
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
