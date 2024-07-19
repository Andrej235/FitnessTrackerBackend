using FitnessTracker.DTOs.Responses.Split;
using FitnessTracker.DTOs.Responses.Workout;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response.SplitMappers
{
    public class SimpleSplitWorkoutResponseMapper(IResponseMapper<Workout, SimpleWorkoutResponseDTO> workoutResponseMapper) : IResponseMapper<SplitWorkout, SimpleSplitWorkoutResponseDTO>
    {
        private readonly IResponseMapper<Workout, SimpleWorkoutResponseDTO> workoutResponseMapper = workoutResponseMapper;

        public SimpleSplitWorkoutResponseDTO Map(SplitWorkout from) => new()
        {
            Workout = workoutResponseMapper.Map(from.Workout),
            Day = from.Day,
        };
    }
}
