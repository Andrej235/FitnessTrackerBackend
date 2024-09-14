using FitnessTracker.DTOs.Requests.Workout;

namespace FitnessTracker.Services.ModelServices.WorkoutService
{
    public partial class WorkoutService
    {
        public Task<string> CreateComment(Guid workoutId, CreateWorkoutCommentRequestDTO request) => throw new NotImplementedException();
        public Task<string> CreateReply(Guid workoutId, Guid commentId, CreateWorkoutCommentRequestDTO request) => throw new NotImplementedException();
    }
}
