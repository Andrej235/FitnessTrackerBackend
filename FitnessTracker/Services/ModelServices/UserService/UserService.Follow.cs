using FitnessTracker.Exceptions;

namespace FitnessTracker.Services.ModelServices.UserService
{
    public partial class UserService
    {
        public async Task Follow(Guid userId, string usernameToFollow)
        {
            if (userId == default)
                throw new UnauthorizedException();

            if (string.IsNullOrWhiteSpace(usernameToFollow))
                throw new InvalidArgumentException();

            var userToFollowId = await readSingleSelectedService.Get(
                    x => new { x.Id },
                    x => x.Username == usernameToFollow
                ) ?? throw new NotFoundException($"User '{usernameToFollow}' not found");

            if (userId == userToFollowId.Id)
                throw new BadRequestException("You cannot follow yourself");

            _ = await followCreateService.Add(new()
            {
                FollowerId = userId,
                FolloweeId = userToFollowId.Id
            });
        }

        public async Task Unfollow(Guid userId, string usernameToFollow)
        {
            if (userId == default)
                throw new UnauthorizedException();

            if (string.IsNullOrWhiteSpace(usernameToFollow))
                throw new InvalidArgumentException();

            var userToFollowId = await readSingleSelectedService.Get(
                    x => new { x.Id },
                    x => x.Username == usernameToFollow
                ) ?? throw new NotFoundException($"User '{usernameToFollow}' not found");

            if (userId == userToFollowId.Id)
                throw new BadRequestException("You cannot follow yourself");

            await followDeleteService.Delete(x => x.FollowerId == userId && x.FolloweeId == userToFollowId.Id);
        }
    }
}
