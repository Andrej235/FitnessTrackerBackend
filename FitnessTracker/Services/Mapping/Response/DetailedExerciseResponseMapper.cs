using FitnessTracker.DTOs.Responses.Equipment;
using FitnessTracker.DTOs.Responses.Exercises;
using FitnessTracker.DTOs.Responses.Muscle;
using FitnessTracker.DTOs.Responses.MuscleGroup;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response
{
    public class DetailedExerciseResponseMapper(IResponseMapper<Equipment, SimpleEquipmentResponseDTO> equipmentResponseMapper,
                                                IResponseMapper<MuscleGroup, SimpleMuscleGroupResponseDTO> muscleGroupResponseMapper,
                                                IResponseMapper<Muscle, SimpleMuscleResponseDTO> muscleResponseMapper) : IResponseMapper<Exercise, DetailedExerciseResponseDTO>
    {
        private readonly IResponseMapper<Equipment, SimpleEquipmentResponseDTO> equipmentResponseMapper = equipmentResponseMapper;
        private readonly IResponseMapper<MuscleGroup, SimpleMuscleGroupResponseDTO> muscleGroupResponseMapper = muscleGroupResponseMapper;
        private readonly IResponseMapper<Muscle, SimpleMuscleResponseDTO> muscleResponseMapper = muscleResponseMapper;

        public DetailedExerciseResponseDTO Map(Exercise from)
        {
            return new()
            {
                Name = from.Name,
                Description = from.Description,
                Image = from.Image,
                Equipment = from.Equipment.Select(equipmentResponseMapper.Map),
                PrimaryMuscleGroups = from.PrimaryMuscleGroups.Select(muscleGroupResponseMapper.Map),
                PrimaryMuscles = from.PrimaryMuscles.Select(muscleResponseMapper.Map),
                SecondaryMuscleGroups = from.SecondaryMuscleGroups.Select(muscleGroupResponseMapper.Map),
                SecondaryMuscles = from.SecondaryMuscles.Select(muscleResponseMapper.Map),
            };
        }
    }
}
