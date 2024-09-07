using FitnessTracker.DTOs.Enums;

namespace FitnessTracker.DTOs.Requests.Pins
{
    public class DeleteSinglePinRequestDTO
    {
        public Guid Id { get; set; }
        public PinType Type { get; set; }
    }
}
