﻿using FitnessTracker.DTOs;
using FitnessTracker.Models;
using FitnessTracker.Services.Read;

namespace FitnessTracker.Services.Mapping
{
    public class ExerciseMapper(IReadService<Equipment> equipmentReadService,
                                IReadService<MuscleGroup> muscleGroupReadService,
                                IReadService<Muscle> muscleReadService) : IEntityMapperAsync<Exercise, ExerciseDTO>
    {
        public ExerciseDTO Map(Exercise entity) => new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Image = entity.EncodedImage,
            EquipmentIds = entity.Equipment.Select(x => x.Id),
            PrimaryMuscleGroupIds = entity.PrimaryMuscleGroups.Select(x => x.Id),
            SecondaryMuscleGroupIds = entity.SecondaryMuscleGroups.Select(x => x.Id),
            PrimaryMuscleIds = entity.PrimaryMuscles.Select(x => x.Id),
            SecondaryMuscleIds = entity.SecondaryMuscles.Select(x => x.Id),
        };

        public async Task<Exercise> MapAsync(ExerciseDTO dto) => new()
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            EncodedImage = dto.Image,
            Equipment = await equipmentReadService.Get(x => dto.EquipmentIds.Contains(x.Id), 0, -1, "none"),
            PrimaryMuscleGroups = await muscleGroupReadService.Get(x => dto.PrimaryMuscleGroupIds.Contains(x.Id), 0, -1, "none"),
            SecondaryMuscleGroups = await muscleGroupReadService.Get(x => dto.SecondaryMuscleGroupIds.Contains(x.Id), 0, -1, "none"),
            PrimaryMuscles = await muscleReadService.Get(x => dto.PrimaryMuscleIds.Contains(x.Id), 0, -1, "none"),
            SecondaryMuscles = await muscleReadService.Get(x => dto.SecondaryMuscleIds.Contains(x.Id), 0, -1, "none"),
        };

        Exercise IEntityMapper<Exercise, ExerciseDTO>.Map(ExerciseDTO dto) => new()
        {
            Id = dto.Id,
            Name = dto.Name,
        };
    }
}
