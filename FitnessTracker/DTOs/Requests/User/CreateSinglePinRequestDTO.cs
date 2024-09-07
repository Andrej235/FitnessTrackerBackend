using FitnessTracker.DTOs.Enums;

namespace FitnessTracker.DTOs.Requests.User
{
    public class CreateSinglePinRequestDTO
    {
        public Guid Id { get; set; }
        public PinType Type { get; set; }
    }
}
