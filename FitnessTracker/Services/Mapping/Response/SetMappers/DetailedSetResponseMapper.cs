using FitnessTracker.DTOs.Responses.Exercises;
using FitnessTracker.DTOs.Responses.Set;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response.SetMappers
{
    public class DetailedSetResponseMapper(IResponseMapper<Exercise, SimpleExerciseResponseDTO> exerciseResponseMapper) : IResponseMapper<Set, DetailedSetResponseDTO>
    {
        private readonly IResponseMapper<Exercise, SimpleExerciseResponseDTO> exerciseResponseMapper = exerciseResponseMapper;

        public DetailedSetResponseDTO Map(Set from)
        {
            return new()
            {
                NumberOfSets = from.NumberOfSets,
                BottomRepRange = from.BottomRepRange,
                TopRepRange = from.TopRepRange,
                Exercise = exerciseResponseMapper.Map(from.Exercise),
            };
        }
    }
}
