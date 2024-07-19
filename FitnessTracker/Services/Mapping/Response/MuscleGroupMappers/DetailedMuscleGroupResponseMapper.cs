using FitnessTracker.DTOs.Responses.Muscle;
using FitnessTracker.DTOs.Responses.MuscleGroup;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response.MuscleGroupMappers
{
    public class DetailedMuscleGroupResponseMapper(IResponseMapper<Muscle, SimpleMuscleResponseDTO> muscleResponseMapper) : IResponseMapper<MuscleGroup, DetailedMuscleGroupResponseDTO>
    {
        public DetailedMuscleGroupResponseDTO Map(MuscleGroup from) => new()
        {
            Id = from.Id,
            Name = from.Name,
            Muscles = from.Muscles.Select(muscleResponseMapper.Map).ToList()
        };
    }
}
