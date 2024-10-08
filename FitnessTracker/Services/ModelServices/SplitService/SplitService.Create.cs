using FitnessTracker.DTOs.Requests.Split;
using FitnessTracker.DTOs.Responses.Split;
using FitnessTracker.Exceptions;
using FitnessTracker.Models;

namespace FitnessTracker.Services.ModelServices.SplitService
{
    public partial class SplitService
    {
        public async Task<NewSplitResponseDTO> Create(Guid userId, CreateSplitRequestDTO request)
        {
            IEnumerable<Guid> selectedWorkoutIds = request.Workouts.Select(x => x.WorkoutId);
            IEnumerable<Models.Workout> selectedWorkouts = await workoutReadRangeService.Get(x => selectedWorkoutIds.Contains(x.Id));
            if (selectedWorkoutIds.Count() != selectedWorkouts.Count())
                throw new NotFoundException("One or more selected workouts could not found");

            if (selectedWorkouts.Any(x => !x.IsPublic && x.CreatorId != userId))
                throw new AccessDeniedException("One or more selected workouts are not public and you are not the creator");

            if (request.IsPublic && selectedWorkouts.Any(x => !x.IsPublic))
                throw new BadRequestException("Attempted to create a public split with one or more private workouts");

            Split mapped = createRequestMapper.Map(request);
            mapped.CreatorId = userId;

            Split newSplit = await createService.Add(mapped);
            return new()
            {
                Id = newSplit.Id,
                IsPublic = newSplit.IsPublic,
                Name = newSplit.Name,
            };
        }
    }
}
