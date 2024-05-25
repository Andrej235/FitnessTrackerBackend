using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services.Create;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;
using ProjectGym.Services.Update;
using ProjectGym.Utilities;
using System.Diagnostics;
using System.Security.Claims;
using static ProjectGym.Controllers.UserController;

namespace ProjectGym.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController(IReadService<User> readService,
                          IEntityMapper<User, UserDTO> mapper,
                          ICreateService<User> createService) : ControllerBase, ICreateController<User, RegisterDTO>
    {
        public IReadService<User> ReadService { get; } = readService;
        public IEntityMapper<User, UserDTO> Mapper { get; } = mapper;
        public ICreateService<User> CreateService { get; } = createService;

        [HttpPost("register")]
        public async Task<IActionResult> Create([FromBody] RegisterDTO userDTO)
        {
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
                var user = await ReadService.Get(x => x.Email == userDTO.Email, "none");

                var hash = userDTO.Password.HashPassword(user.Salt);
                if (!user.PasswordHash.SequenceEqual(hash))
                    return BadRequest("Incorrect password");

                return Ok(Program.CreateJWT(user));
            }
            catch (NullReferenceException)
            {
                return NotFound("User does not exist");
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest(ex.GetErrorMessage());
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

            return Ok(Mapper.Map(await ReadService.Get(userIdClaim, "all")));
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
    }
}
