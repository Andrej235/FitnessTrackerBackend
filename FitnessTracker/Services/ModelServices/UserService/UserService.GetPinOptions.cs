using FitnessTracker.DTOs.Responses.Pins;

namespace FitnessTracker.Services.ModelServices.UserService
{
    public partial class UserService
    {
        public Task<IEnumerable<PinResponseDTO>> GetPinOptions(Guid userId) => throw new NotImplementedException();
    }
}
