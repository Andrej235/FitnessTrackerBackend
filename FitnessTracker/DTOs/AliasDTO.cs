﻿namespace ProjectGym.DTOs
{
    public class AliasDTO
    {
        public int Id { get; set; }
        public string Alias { get; set; } = null!;
        public int ExerciseId { get; set; }
    }
}
