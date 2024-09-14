using FitnessTracker.DTOs.Responses.Workout;

namespace FitnessTracker.Services.ModelServices.WorkoutService
{
    public partial class WorkoutService
    {
        public Task<IEnumerable<SimpleWorkoutCommentResponseDTO>> GetComments(Guid workoutId, int? offset, int? limit) => throw new NotImplementedException();
        public Task<IEnumerable<SimpleWorkoutCommentResponseDTO>> GetReplies(Guid workoutId, Guid commentId, int? offset, int? limit) => throw new NotImplementedException();
    }
}
