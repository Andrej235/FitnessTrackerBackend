using FitnessTracker.DTOs.Requests.Pins;
using FitnessTracker.Exceptions;
using FitnessTracker.Models;
using FitnessTracker.Services.Read;

namespace FitnessTracker.Services.ModelServices.UserService
{
    public partial class UserService
    {
        public async Task ReorderPins(Guid userId, ReorderPinsRequestDTO request)
        {
            var pins = await readSingleSelectedService.Get(
                x => new
                {
                    x.WorkoutPins,
                    x.SplitPins
                },
                x => x.Id == userId,
                x => x.Include(x => x.WorkoutPins).Include(x => x.SplitPins).AsNoTracking())
                ?? throw new UnauthorizedException();

            if (pins.WorkoutPins.Count == 0 && pins.SplitPins.Count == 0)
                throw new BadRequestException("User has no pins");

            IEnumerable<UpdateSinglePinRequestDTO> workoutRequests = request.NewOrder.Where(x => x.Type == DTOs.Enums.PinType.Workout);
            IEnumerable<UpdateSinglePinRequestDTO> splitRequests = request.NewOrder.Where(x => x.Type == DTOs.Enums.PinType.Split);

            if (workoutRequests.Count() != pins.WorkoutPins.Count || splitRequests.Count() != pins.SplitPins.Count)
                throw new BadRequestException("Invalid request");

            if (request.NewOrder.Any(x => x.NewOrder < 0))
                throw new BadRequestException("Invalid request");

            if (request.NewOrder.Any(x => x.NewOrder > pins.WorkoutPins.Count + pins.SplitPins.Count))
                throw new BadRequestException("Invalid request");

            ICollection<WorkoutPin> workoutPins = pins.WorkoutPins;
            ICollection<SplitPin> splitPins = pins.SplitPins;

            List<WorkoutPin> workoutPinsToUpdate = [];
            List<SplitPin> splitPinsToUpdate = [];

            foreach (WorkoutPin pin in workoutPins)
            {
                int newOrder = workoutRequests.First(x => x.Id == pin.WorkoutId).NewOrder;
                if (newOrder == pin.Order)
                    continue;

                pin.Order = newOrder;
                workoutPinsToUpdate.Add(pin);
            }

            foreach (SplitPin pin in splitPins)
            {
                int newOrder = splitRequests.First(x => x.Id == pin.SplitId).NewOrder;
                if (newOrder == pin.Order)
                    continue;

                pin.Order = newOrder;
                splitPinsToUpdate.Add(pin);
            }

            await workoutPinUpdateRangeService.Update(workoutPinsToUpdate);
            await splitPinUpdateRangeService.Update(splitPinsToUpdate);
        }
    }
}
