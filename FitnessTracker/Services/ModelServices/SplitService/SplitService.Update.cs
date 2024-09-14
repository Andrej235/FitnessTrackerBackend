using FitnessTracker.DTOs.Requests.Split;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Services.ModelServices.SplitService
{
    public partial class SplitService
    {
        public Task UpdateBasicInformation(Guid splitId, Guid userId, UpdateSplitBaseInfoRequestDTO request) => throw new NotImplementedException();
        public Task UpdateSplitWorkout(Guid splitId, Guid userId, DayOfWeek day, [FromBody] UpdateSplitWorkoutRequestDTO request) => throw new NotImplementedException();
    }
}
