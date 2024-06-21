using FitnessTracker.DTOs.Requests.User;
using FitnessTracker.Models;
using FitnessTracker.Services.Read;
using FitnessTracker.Services.Read.ExpressionBased;
using FitnessTracker.Services.Update;
using FitnessTracker.Services.UserServices.EmailConfirmationSenderService;
using FitnessTracker.Services.UserServices.EmailConfirmationService;
using FitnessTracker.Services.UserServices.ResetPasswordSenderService;
using FitnessTracker.Services.UserServices.ResetPasswordService;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace FitnessTracker.Controllers.Admin
{
    [Route("api/unsafe/user")]
    [ApiController]
    public class UNSAFE_UserController(IEmailConfirmationSenderService emailConfirmationSender,
                                       IEmailConfirmationService emailConfirmationService,
                                       IReadSingleService<User> readSingleService,
                                       IResetPasswordEmailSenderService passwordResetEmailSender,
                                       IResetPasswordService passwordResetEmailService,
                                       IUpdateService<User> updateService) : ControllerBase
    {
        [Authorize(Roles = Role.Unverified)]
        [HttpGet("mail")]
        public async Task<IActionResult> GetMail([FromQuery] string mail)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            await emailConfirmationSender.SendEmailConfirmation(mail, userId);
            return Ok();
        }

        [Authorize(Roles = Role.Unverified)]
        [HttpGet("confirm/{code:guid}")]
        public async Task<IActionResult> Confirm(Guid code)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            var success = await emailConfirmationService.ConfirmEmail(userId, code);
            return success ? Ok("Email Confirmed") : BadRequest("Invalid code");
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpGet("roletest")]
        public IActionResult RoleTest()
        {
            return Ok();
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpGet("forgotpassword")]
        public async Task<IActionResult> SendResetPasswordEmail()
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            var user = await readSingleService.Get(x => x.Id == userId, "none");
            if (user is null)
                return Unauthorized();

            await passwordResetEmailSender.SendEmail(user.Email, userId);
            return Ok();
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpGet("forgotpassword/{code:guid}")]
        public async Task<IActionResult> ConfirmResetPassword(Guid code, [FromBody] ResetPasswordUserRequestDTO request)
        {
            try
            {
                if (request.NewPassword.Length < 8)
                    return BadRequest("Password must be at least 8 characters long");

                if (User.Identity is not ClaimsIdentity claimsIdentity
                    || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                    || !Guid.TryParse(userIdString, out var userId))
                    return Unauthorized();

                var success = await passwordResetEmailService.ResetPassword(userId, code, request.NewPassword);
                return success ? Ok("Email Confirmed") : BadRequest("Invalid code");
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest(ex.GetErrorMessage());
            }
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpPost("changepassword")]
        public async Task<IActionResult> ResetPassword([FromBody] UpdatePasswordUserRequestDTO request)
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
