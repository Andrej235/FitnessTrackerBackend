using FitnessTracker.Auth;
using FitnessTracker.DTOs;
using FitnessTracker.DTOs.Requests.User;
using FitnessTracker.Models;
using FitnessTracker.Services.Create;
using FitnessTracker.Services.Mapping.Request;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace FitnessTracker.Controllers
{
    [Route("api/v2/user")]
    [ApiController]
    public partial class UserControllerV2(IRequestMapper<RegisterUserRequestDTO, User> registrationMapper, ICreateService<User> createService, ITokenManager tokenManager) : ControllerBase
    {
        private readonly IRequestMapper<RegisterUserRequestDTO, User> registrationMapper = registrationMapper;
        private readonly ICreateService<User> createService = createService;
        private readonly ITokenManager tokenManager = tokenManager;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequestDTO request)
        {
            if (request.Name.Length < 3 || !ValidEmailRegex().IsMatch(request.Email.Trim()) || request.Password.Length < 8)
                return BadRequest("Invalid registration details");

            var user = registrationMapper.Map(request);
            var newUserId = await createService.Add(user);
            if (newUserId == default)
                return BadRequest("User already exists");

            var jwt = await tokenManager.CreateJWTAndRefreshToken(user, Response.Cookies);
            return Ok(jwt);
        }

        [GeneratedRegex(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$")]
        private static partial Regex ValidEmailRegex();
    }
}
