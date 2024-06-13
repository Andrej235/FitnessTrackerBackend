using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessTracker.Models
{
    public class WorkoutLike
    {
        [Key, Column(Order = 0)]
        public Guid WorkoutId { get; set; }

        [Key, Column(Order = 1)]
        public Guid UserId { get; set; }
    }
}
