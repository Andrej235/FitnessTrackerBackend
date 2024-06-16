using FitnessTracker.DTOs.Requests.User;
using FitnessTracker.Models;
using FitnessTracker.Services.Read;
using FitnessTracker.Services.UserServices.EmailConfirmationSender;
using FitnessTracker.Services.UserServices.EmailConfirmationService;
using FitnessTracker.Services.UserServices.ResetPasswordSender;
using FitnessTracker.Services.UserServices.ResetPasswordService;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers.Admin
{
    [Route("api/unsafe/user")]
    [ApiController]
    public class UNSAFE_UserController(IEmailConfirmationSender emailConfirmationSender,
                                       IEmailConfirmationService emailConfirmationService,
                                       IReadService<User> readService,
                                       IResetPasswordEmailSender passwordResetEmailSender,
                                       IResetPasswordService passwordResetEmailService) : ControllerBase
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
        [HttpGet("resetpassword")]
        public async Task<IActionResult> SendResetPasswordEmail()
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            var user = await readService.Get(x => x.Id == userId, "none");
            if (user is null)
                return Unauthorized();

            await passwordResetEmailSender.SendEmail(user.Email, userId);
            return Ok();
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpGet("resetpassword/{code:guid}")]
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
                return BadRequest(ex.GetErrorMessage(false));
            }
        }
    }
}
