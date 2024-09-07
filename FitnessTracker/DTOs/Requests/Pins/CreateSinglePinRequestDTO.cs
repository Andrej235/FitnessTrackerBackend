using FitnessTracker.DTOs.Enums;

namespace FitnessTracker.DTOs.Requests.Pins
{
    public class CreateSinglePinRequestDTO
    {
        public Guid Id { get; set; }
        public PinType Type { get; set; }
    }
}
