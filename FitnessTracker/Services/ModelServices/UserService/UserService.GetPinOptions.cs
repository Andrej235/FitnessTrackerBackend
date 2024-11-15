using FitnessTracker.DTOs.Enums;
using FitnessTracker.DTOs.Responses.Pins;
using FitnessTracker.Exceptions;

namespace FitnessTracker.Services.ModelServices.UserService
{
    public partial class UserService
    {
        public async Task<IEnumerable<PinResponseDTO>> GetPinOptions(Guid userId, int? offset, int? limit)
        {
            if (userId == default)
                throw new UnauthorizedException();

            int nonNullOffset = offset ?? 0;
            int nonNullLimit = limit ?? 10;

            List<PinResponseDTO>? pins = await readSingleSelectedService.Get(
                select: x => x.CreatedWorkouts.Select(x => new PinResponseDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    LikeCount = x.Likes.Count,
                    FavoriteCount = x.Favorites.Count,
                    Type = PinType.Workout,
                    Description = x.Description ?? "",
                    Order = -1,
                })
                .Union(x.CreatedSplits.Select(x => new PinResponseDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    LikeCount = x.Likes.Count,
                    FavoriteCount = x.Favorites.Count,
                    Type = PinType.Split,
                    Description = x.Description ?? "",
                    Order = -1,
                }))
                .OrderByDescending(x => x.LikeCount)
                .Skip(nonNullOffset)
                .Take(nonNullLimit)
                .ToList(),
                criteria: x => x.Id == userId)
                ?? throw new UnauthorizedException();

            return pins;
        }
    }
}
