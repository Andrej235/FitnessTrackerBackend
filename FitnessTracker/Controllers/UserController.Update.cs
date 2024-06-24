using FitnessTracker.DTOs.Requests.User;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class UserController
    {
        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody] UpdatePasswordUserRequestDTO request)
        {
            try
            {
                if (request.NewPassword.Length < 8)
                    return BadRequest("Password must be at least 8 characters long");

                if (User.Identity is not ClaimsIdentity claimsIdentity
                    || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                    || !Guid.TryParse(userIdString, out var userId))
                    return Unauthorized();

                var user = await readSingleService.Get(x => x.Id == userId, "none");
                if (user is null)
                    return Unauthorized();

                var hash = request.OldPassword.ToHash(user.Salt);
                if (!user.PasswordHash.SequenceEqual(hash))
                    return Unauthorized();

                user.PasswordHash = request.NewPassword.ToHash(user.Salt);
                await updateService.Update(user);

                return Ok();
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest(ex.GetErrorMessage());
            }
        }
    }
}
