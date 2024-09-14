using FitnessTracker.DTOs.Responses.Pins;

namespace FitnessTracker.Services.ModelServices.UserService
{
    public partial class UserService
    {
        public Task<IEnumerable<PinResponseDTO>> GetPins(Guid userId) => throw new NotImplementedException();
        public Task<IEnumerable<PinResponseDTO>> GetPinsFor(string username) => throw new NotImplementedException();
    }
}
