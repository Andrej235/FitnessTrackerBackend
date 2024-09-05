namespace FitnessTracker.DTOs.Requests.User
{
    public class DeletePinsRequestDTO
    {
        public IEnumerable<Guid> DeletedPinIds { get; set; } = null!;
    }
}
