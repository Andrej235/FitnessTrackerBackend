using FitnessTracker.DTOs.Requests.Split;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Request.SplitMappers
{
    public class CreateSplitWorkoutRequestMapper : IRequestMapper<CreateSplitWorkoutRequestDTO, SplitWorkout>
    {
        public SplitWorkout Map(CreateSplitWorkoutRequestDTO from)
        {
            return new()
            {
                WorkoutId = from.WorkoutId,
                Day = from.Day,
            };
        }
    }
}
