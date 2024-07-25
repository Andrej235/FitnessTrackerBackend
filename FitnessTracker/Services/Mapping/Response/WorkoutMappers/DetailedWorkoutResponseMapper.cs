using FitnessTracker.DTOs.Responses.Exercises;
using FitnessTracker.DTOs.Responses.Set;
using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.DTOs.Responses.Workout;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response.WorkoutMappers
{
    public class DetailedWorkoutResponseMapper(IResponseMapper<User, SimpleUserResponseDTO> userResponseMapper,
                                               IResponseMapper<Set, DetailedSetResponseDTO> setResponseMapper,
                                               IResponseMapper<Exercise, SimpleExerciseResponseDTO> exerciseResponseMapper) : IResponseMapper<Workout, DetailedWorkoutResponseDTO>
    {
        private readonly IResponseMapper<User, SimpleUserResponseDTO> userResponseMapper = userResponseMapper;
        private readonly IResponseMapper<Set, DetailedSetResponseDTO> setResponseMapper = setResponseMapper;
        private readonly IResponseMapper<Exercise, SimpleExerciseResponseDTO> exerciseResponseMapper = exerciseResponseMapper;

        public DetailedWorkoutResponseDTO Map(Workout from) => new()
        {
            Id = from.Id,
            Name = from.Name,
            Description = from.Description,
            IsPublic = from.IsPublic,
            Creator = userResponseMapper.Map(from.Creator),
            Sets = from.Sets.Select(setResponseMapper.Map),
            Exercises = from.Sets.DistinctBy(x => x.ExerciseId).Select(x => exerciseResponseMapper.Map(x.Exercise)),
        };
    }
}
