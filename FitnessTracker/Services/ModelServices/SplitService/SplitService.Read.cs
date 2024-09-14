using FitnessTracker.DTOs.Responses.Split;

namespace FitnessTracker.Services.ModelServices.SplitService
{
    public partial class SplitService
    {
        public Task<IEnumerable<SimpleSplitResponseDTO>> GetAllPublic(string? name) => throw new NotImplementedException();
        public Task<IEnumerable<SimpleSplitResponseDTO>> GetAllPublicBy(string username, string? name) => throw new NotImplementedException();
        public Task<IEnumerable<SimpleSplitResponseDTO>> GetAllSimplePersonal(Guid userId, string? name) => throw new NotImplementedException();
        public Task<DetailedSplitResponseDTO> GetSingleDetailed(Guid splitId, Guid? userId) => throw new NotImplementedException();
        public Task<DetailedUserSplitResponseDTO> GetDetailedUsedBy(string username) => throw new NotImplementedException();
    }
}
