using FitnessTracker.DTOs.Requests.Pins;

namespace FitnessTracker.Services.ModelServices.UserService
{
    public partial class UserService
    {
        public async Task DeletePins(Guid userId, DeletePinsRequestDTO request)
        {
            IEnumerable<DeleteSinglePinRequestDTO> deletedWorkouts = request.DeletedPins.Where(x => x.Type == DTOs.Enums.PinType.Workout);
            IEnumerable<DeleteSinglePinRequestDTO> deletedSplits = request.DeletedPins.Where(x => x.Type == DTOs.Enums.PinType.Split);

            foreach (DeleteSinglePinRequestDTO? workout in deletedWorkouts)
                await workoutPinDeleteService.Delete(x => x.UserId == userId && x.WorkoutId == workout.Id);

            foreach (DeleteSinglePinRequestDTO? split in deletedSplits)
                await splitPinDeleteService.Delete(x => x.UserId == userId && x.SplitId == split.Id);
        }
    }
}
