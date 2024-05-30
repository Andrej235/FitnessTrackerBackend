namespace ProjectGym.DTOs
{
    public class ExerciseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string EncodedImage { get; set; } = null!;

        public IEnumerable<int> EquipmentIds { get; set; } = [];
        public IEnumerable<int> PrimaryMuscleGroupIds { get; set; } = [];
        public IEnumerable<int> SecondaryMuscleGroupIds { get; set; } = [];
        public IEnumerable<int> PrimaryMuscleIds { get; set; } = [];
        public IEnumerable<int> SecondaryMuscleIds { get; set; } = [];
        public IEnumerable<int> AliasIds { get; set; } = [];
    }
}
