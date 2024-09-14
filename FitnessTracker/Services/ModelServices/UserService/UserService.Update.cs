using FitnessTracker.DTOs.Requests.User;
using FitnessTracker.Exceptions;
using FitnessTracker.Models;
using FitnessTracker.Utilities;

namespace FitnessTracker.Services.ModelServices.UserService
{
    public partial class UserService
    {
        public async Task Update(Guid userId, UpdatePasswordUserRequestDTO request)
        {
            if (request.NewPassword.Length < 8)
                throw new BadRequestException("Password must be at least 8 characters long");

            User user = await readSingleService.Get(x => x.Id == userId) ?? throw new UnauthorizedException();

            byte[] hash = request.OldPassword.ToHash(user.Salt);
            if (!user.PasswordHash.SequenceEqual(hash))
                throw new BadRequestException("Incorrect old password");

            user.PasswordHash = request.NewPassword.ToHash(user.Salt);
            await updateService.Update(user);

            await tokenManager.InvalidateAllTokensForUser(user.Id);
        }

        public async Task Update(Guid userId, UpdateUserEmailRequestDTO request)
        {
            if (!ValidEmailRegex().IsMatch(request.NewEmail.Trim()))
                throw new BadRequestException("Invalid email address");

            if (request.NewEmail == request.OldEmail)
                throw new BadRequestException("New email cannot be the same as old email");

            User user = await readSingleService.Get(x => x.Id == userId) ?? throw new UnauthorizedException();

            if (user.Email != request.OldEmail)
                return;

            user.Email = request.NewEmail.Trim();
            user.Role = Role.Unverified;
            user.EmailConfirmed = false;

            await updateService.Update(user);

            await tokenManager.InvalidateAllTokensForUser(user.Id);
            await emailConfirmationSender.SendEmailConfirmation(user.Email, user.Id);
        }

        public async Task Update(Guid userId, UpdateSplitUserRequestDTO request)
        {
            User user = await readSingleService.Get(x => x.Id == userId) ?? throw new UnauthorizedException();
            user.SplitId = request.SplitId;
            await updateService.Update(user);
        }

        public Task Update(Guid userId, UpdateUserNameRequestDTO request) => executeUpdateService.Execute(x => x.Id == userId, x => x.SetProperty(x => x.Name, request.NewName));

        public Task Update(Guid userId, UpdateUserBioRequestDTO request) => executeUpdateService.Execute(x => x.Id == userId, x => x.SetProperty(x => x.Bio, request.NewBio));

        public Task Update(Guid userId, UpdateUserImageRequestDTO request) => executeUpdateService.Execute(x => x.Id == userId, x => x.SetProperty(x => x.ProfilePic, request.NewImage));
    }
}
