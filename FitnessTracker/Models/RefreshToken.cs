using ProjectGym.Models;
using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Models
{
    public class RefreshToken
    {
        [Key]
        public Guid Token { get; set; }
        public string JwtId { get; set; } = null!;
        public DateTime ExpiryDate { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
