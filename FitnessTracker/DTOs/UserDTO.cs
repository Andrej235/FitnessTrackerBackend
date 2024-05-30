namespace ProjectGym.DTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? ProfilePic { get; set; }

        public IEnumerable<Guid> WeightIds { get; set; } = [];
        public IEnumerable<Guid> CreatedWorkoutIds { get; set; } = [];
        public IEnumerable<Guid> CreatedExerciseSetIds { get; set; } = [];

        public IEnumerable<int> BookmarkIds { get; set; } = [];
    }
}
