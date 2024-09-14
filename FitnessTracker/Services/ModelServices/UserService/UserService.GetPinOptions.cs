using FitnessTracker.DTOs.Enums;
using FitnessTracker.DTOs.Responses.Pins;
using FitnessTracker.Exceptions;

namespace FitnessTracker.Services.ModelServices.UserService
{
    public partial class UserService
    {
        public async Task<IEnumerable<PinResponseDTO>> GetPinOptions(Guid userId)
        {
            if (userId == default)
                throw new UnauthorizedException();

            List<PinResponseDTO>? pins = await readSingleSelectedService.Get(
                select: x => x.CreatedWorkouts.Where(x => x.IsPublic).Select(x => new PinResponseDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    LikeCount = x.Likes.Count,
                    Type = PinType.Workout,
                    Description = x.Description ?? "",
                    Order = -1,
                })
                .Union(x.CreatedSplits.Where(x => x.IsPublic).Select(x => new PinResponseDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    LikeCount = x.Likes.Count,
                    Type = PinType.Split,
                    Description = x.Description ?? "",
                    Order = -1,
                }))
                .OrderByDescending(x => x.LikeCount)
                .ToList(),
                criteria: x => x.Id == userId)
                ?? throw new UnauthorizedException();

            return pins;
        }
    }
}
