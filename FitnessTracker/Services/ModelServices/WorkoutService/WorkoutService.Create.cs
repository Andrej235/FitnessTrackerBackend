using FitnessTracker.DTOs.Requests.Workout;
using FitnessTracker.DTOs.Responses.Workout;
using FitnessTracker.Exceptions;
using FitnessTracker.Models;

namespace FitnessTracker.Services.ModelServices.WorkoutService
{
    public partial class WorkoutService
    {
        public async Task<NewWorkoutResponseDTO> Create(Guid userId, CreateWorkoutRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new InvalidRequestDTOException(nameof(request));

            Workout workout = createRequestMapper.Map(request);
            workout.CreatorId = userId;
            Workout newWorkout = await createService.Add(workout);

            return newWorkoutResponseMapper.Map(newWorkout);
        }
    }
}
