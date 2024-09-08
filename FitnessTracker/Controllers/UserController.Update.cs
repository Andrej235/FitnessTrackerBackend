using FitnessTracker.DTOs.Requests.User;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class UserController
    {
        [Authorize]
        [HttpPatch("me/password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateUser([FromBody] UpdatePasswordUserRequestDTO request)
        {
            try
            {
                if (request.NewPassword.Length < 8)
                    return BadRequest("Password must be at least 8 characters long");

                if (User.Identity is not ClaimsIdentity claimsIdentity
                    || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                    || !Guid.TryParse(userIdString, out Guid userId))
                    return Unauthorized();

                Models.User? user = await readSingleService.Get(x => x.Id == userId);
                if (user is null)
                    return Unauthorized();

                byte[] hash = request.OldPassword.ToHash(user.Salt);
                if (!user.PasswordHash.SequenceEqual(hash))
                    return Unauthorized();

                user.PasswordHash = request.NewPassword.ToHash(user.Salt);
                await updateService.Update(user);

                await tokenManager.InvalidateAllTokensForUser(user.Id);

                return NoContent();
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest(ex.GetErrorMessage());
            }
        }

        [Authorize]
        [HttpPatch("me/email")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserEmailRequestDTO request)
        {
            try
            {
                if (!ValidEmailRegex().IsMatch(request.NewEmail.Trim()))
                    return BadRequest("Invalid email address");

                if (request.NewEmail == request.OldEmail)
                    return BadRequest("New email cannot be the same as old email");

                if (User.Identity is not ClaimsIdentity claimsIdentity
                    || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                    || !Guid.TryParse(userIdString, out Guid userId))
                    return Unauthorized();

                Models.User? user = await readSingleService.Get(x => x.Id == userId);
                if (user is null)
                    return Unauthorized();

                if (user.Email != request.OldEmail)
                    return NoContent();

                user.Email = request.NewEmail.Trim();
                user.Role = Role.Unverified;
                user.EmailConfirmed = false;

                await updateService.Update(user);

                await tokenManager.InvalidateAllTokensForUser(user.Id);
                await emailConfirmationSender.SendEmailConfirmation(user.Email, user.Id);

                return NoContent();
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest(ex.GetErrorMessage());
            }
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpPatch("me/split")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateSplitUserRequestDTO request)
        {
            try
            {
                if (User.Identity is not ClaimsIdentity claimsIdentity
                    || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                    || !Guid.TryParse(userIdString, out Guid userId))
                    return Unauthorized();

                Models.User? user = await readSingleService.Get(x => x.Id == userId);
                if (user is null)
                    return Unauthorized();

                user.SplitId = request.SplitId;
                await updateService.Update(user);
                return NoContent();
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest("Failed to update split");
            }
        }

        [Authorize]
        [HttpPatch("me/name")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserNameRequestDTO request)
        {
            try
            {
                if (User.Identity is not ClaimsIdentity claimsIdentity
                    || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                    || !Guid.TryParse(userIdString, out Guid userId))
                    return Unauthorized();

                await executeUpdateService.Execute(x => x.Id == userId, x => x.SetProperty(x => x.Name, request.NewName));
                return NoContent();
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest();
            }
        }

        [Authorize]
        [HttpPatch("me/bio")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserBioRequestDTO request)
        {
            try
            {
                if (User.Identity is not ClaimsIdentity claimsIdentity
                    || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                    || !Guid.TryParse(userIdString, out Guid userId))
                    return Unauthorized();

                await executeUpdateService.Execute(x => x.Id == userId, x => x.SetProperty(x => x.Bio, request.NewBio));
                return NoContent();
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest();
            }
        }

        [Authorize]
        [HttpPatch("me/image")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserImageRequestDTO request)
        {
            try
            {
                if (User.Identity is not ClaimsIdentity claimsIdentity
                    || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                    || !Guid.TryParse(userIdString, out Guid userId))
                    return Unauthorized();

                await executeUpdateService.Execute(x => x.Id == userId, x => x.SetProperty(x => x.ProfilePic, request.NewImage));
                return NoContent();
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest();
            }
        }
    }
}
