using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services.Create;
using ProjectGym.Services.Delete;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;
using ProjectGym.Services.Update;
using ProjectGym.Utilities;
using System.Diagnostics;
using System.Security.Claims;
using System.Text.RegularExpressions;
using static ProjectGym.Controllers.UserController;

namespace ProjectGym.Controllers
{
    [Route("api/user")]
    [ApiController]
    public partial class UserController(IReadService<User> readService,
                          IEntityMapper<User, UserDTO> mapper,
                          ICreateService<User> createService,
                          IDeleteService<User> deleteService) : ControllerBase, ICreateController<User, RegisterDTO>, IDeleteController<User>
    {
        public IReadService<User> ReadService { get; } = readService;
        public IEntityMapper<User, UserDTO> Mapper { get; } = mapper;
        public ICreateService<User> CreateService { get; } = createService;
        public IDeleteService<User> DeleteService { get; } = deleteService;

        [HttpPost("register")]
        public async Task<IActionResult> Create([FromBody] RegisterDTO userDTO)
        {
            if (!ValidEmailRegex().IsMatch(userDTO.Email))
                return BadRequest("Invalid email address");

            if (userDTO.Password.Length < 8)
                return BadRequest("Password must be at least 8 characters long");

            byte[] salt = HashingService.GenerateSalt();
            User user = new()
            {
                Name = userDTO.Name,
                Email = userDTO.Email,
                Salt = salt,
                PasswordHash = userDTO.Password.HashPassword(salt)
            };

            var newEntityId = await CreateService.Add(user);
            if (newEntityId == default)
                return BadRequest("User already exists");

            return Ok(Program.CreateJWT(user));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO userDTO)
        {
            try
            {
                //This is just so the server doesn't waste time on checking for an invalid email
                if (!ValidEmailRegex().IsMatch(userDTO.Email) || userDTO.Password.Length < 8)
                    return BadRequest("Incorrect email or password");

                var user = await ReadService.Get(x => x.Email == userDTO.Email, "none");

                var hash = userDTO.Password.HashPassword(user.Salt);
                if (!user.PasswordHash.SequenceEqual(hash))
                    return BadRequest("Incorrect email or password");

                return Ok(Program.CreateJWT(user));
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest(ex.GetErrorMessage(false));
            }
        }

        [Authorize]
        [HttpGet("authenticate")]
        public async Task<IActionResult> Authenticate()
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity)
                return Unauthorized();

            var userIdClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim is null)
                return Unauthorized();

            try
            {
                return Ok(Mapper.Map(await ReadService.Get(userIdClaim, "all")));
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest(ex.GetErrorMessage(false));
            }
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity)
                return Unauthorized();

            var userIdClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim is null)
                return Unauthorized();

            try
            {
                await DeleteService.Delete(userIdClaim);
                return Ok();
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest(ex.GetErrorMessage(false));
            }
        }

        public class RegisterDTO
        {
            public string Name { get; set; } = null!;
            public string Email { get; set; } = null!;
            public string Password { get; set; } = null!;
        }

        public class LoginDTO
        {
            public string Email { get; set; } = null!;
            public string Password { get; set; } = null!;
        }

        [GeneratedRegex(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$")]
        private static partial Regex ValidEmailRegex();

        public Task<IActionResult> Delete(string primaryKey)
        {
            throw new NotImplementedException();
        }
    }
}
