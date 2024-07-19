using FitnessTracker.DTOs.Requests.Workout;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Request.WorkoutMappers
{
    public class CreateWorkoutCommentRequestMapper : IRequestMapper<CreateWorkoutCommentRequestDTO, WorkoutComment>
    {
        public WorkoutComment Map(CreateWorkoutCommentRequestDTO from) => new()
        {
            Text = from.Comment,
            CreatedAt = DateTime.Now,
        };
    }
}
