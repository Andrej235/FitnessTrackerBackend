using FitnessTracker.Models;

namespace FitnessTracker.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
