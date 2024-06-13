﻿namespace FitnessTracker.DTOs
{
    public class EquipmentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public IEnumerable<int> UsedInExerciseIds { get; set; } = [];
    }
}
