using FitnessTracker.DTOs.Enums;

namespace FitnessTracker.DTOs.Requests.Pins
{
    public class UpdateSinglePinRequestDTO
    {
        public Guid Id { get; set; }
        public PinType Type { get; set; }
        public int NewOrder { get; set; }
    }
}
