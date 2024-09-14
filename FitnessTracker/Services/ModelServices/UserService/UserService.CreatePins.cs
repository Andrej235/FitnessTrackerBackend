using FitnessTracker.DTOs.Enums;
using FitnessTracker.DTOs.Requests.Pins;
using FitnessTracker.Exceptions;
using FitnessTracker.Models;

namespace FitnessTracker.Services.ModelServices.UserService
{
    public partial class UserService
    {
        public async Task CreatePins(Guid userId, CreatePinsRequestDTO request)
        {
            if (!request.NewPins.Any())
                throw new BadRequestException("No pins provided");

            var pins = await readSingleSelectedService.Get(
                x => new
                {
                    Count = x.WorkoutPins.Count + x.SplitPins.Count,
                    HighestOrder = (x.WorkoutPins.Any() || x.SplitPins.Any())
                    ? x.WorkoutPins
                      .Select(wp => wp.Order)
                      .Union(x.SplitPins.Select(sp => sp.Order))
                      .Max()
                    : 0
                },
                x => x.Id == userId)
                ?? throw new UnauthorizedException();

            if (pins.Count + request.NewPins.Count() > 6)
                throw new BadRequestException("Cannot have more than 6 pins");

            IEnumerable<CreateSinglePinRequestDTO> workoutPinRequests = request.NewPins.Where(x => x.Type == PinType.Workout);
            IEnumerable<CreateSinglePinRequestDTO> splitPinRequests = request.NewPins.Where(x => x.Type == PinType.Split);
            int newWorkoutPinsCount = workoutPinRequests.Count();
            int newSplitPinsCount = splitPinRequests.Count();

            if (newWorkoutPinsCount > 0)
                await workoutPinCreateRangeService.Add(workoutPinRequests.Select((x, i) => new WorkoutPin()
                {
                    UserId = userId,
                    WorkoutId = x.Id,
                    Order = pins.HighestOrder + i + 1
                }));

            if (newSplitPinsCount > 0)
                await splitPinCreateRangeService.Add(splitPinRequests.Select((x, i) => new SplitPin()
                {
                    UserId = userId,
                    SplitId = x.Id,
                    Order = pins.HighestOrder + i + 1 + newWorkoutPinsCount
                }));
        }
    }
}
