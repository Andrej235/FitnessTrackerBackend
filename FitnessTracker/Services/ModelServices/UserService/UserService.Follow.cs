using FitnessTracker.Exceptions;

namespace FitnessTracker.Services.ModelServices.UserService
{
    public partial class UserService
    {
        public Task Follow(Guid userId, Guid userToFollowId)
        {
            if (userId == default)
                throw new UnauthorizedException();

            if (userToFollowId == default)
                throw new InvalidArgumentException();

            if (userId == userToFollowId)
                throw new BadRequestException("You cannot follow yourself");

            return followCreateService.Add(new()
            {
                FollowerId = userId,
                FolloweeId = userToFollowId
            });
        }

        public Task Unfollow(Guid userId, Guid userToFollowId)
        {
            if (userId == default)
                throw new UnauthorizedException();

            if (userToFollowId == default)
                throw new InvalidArgumentException();

            if (userId == userToFollowId)
                throw new BadRequestException("You cannot unfollow yourself");

            return followDeleteService.Delete(x => x.FollowerId == userId && x.FolloweeId == userToFollowId);
        }
    }
}
