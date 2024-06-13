﻿using FitnessTracker.Models;
using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Models
{
    public class RefreshToken
    {
        [Key]
        public Guid Token { get; set; }
        public Guid JwtId { get; set; }
        public DateTime ExpiryDate { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
