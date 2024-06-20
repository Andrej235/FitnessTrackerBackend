using FitnessTracker.DTOs.Requests.Set;
using FitnessTracker.DTOs.Requests.Workout;
using FitnessTracker.DTOs.Responses.Workout;
using FitnessTracker.Models;
using FitnessTracker.Services.Create;
using FitnessTracker.Services.Delete;
using FitnessTracker.Services.Mapping.Request;
using FitnessTracker.Services.Mapping.Response;
using FitnessTracker.Services.Read;
using FitnessTracker.Services.Update;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    [ApiController]
    [Route("api/workout")]
    public class WorkoutController(ICreateService<Workout> createService,
                                   IReadService<Workout> readService,
                                   IUpdateService<Workout> updateService,
                                   IDeleteService<Workout> deleteService,
                                   IRequestMapper<CreateWorkoutRequestDTO, Workout> createRequestMapper,
                                   IResponseMapper<Workout, SimpleWorkoutResponseDTO> simpleResponseMapper,
                                   IResponseMapper<Workout, DetailedWorkoutResponseDTO> detailedResponseMapper) : ControllerBase
    {
        private readonly ICreateService<Workout> createService = createService;
        private readonly IReadService<Workout> readService = readService;
        private readonly IUpdateService<Workout> updateService = updateService;
        private readonly IDeleteService<Workout> deleteService = deleteService;
        private readonly IRequestMapper<CreateWorkoutRequestDTO, Workout> createRequestMapper = createRequestMapper;
        private readonly IResponseMapper<Workout, SimpleWorkoutResponseDTO> simpleResponseMapper = simpleResponseMapper;
        private readonly IResponseMapper<Workout, DetailedWorkoutResponseDTO> detailedResponseMapper = detailedResponseMapper;

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpGet("personal")]
        public async Task<IActionResult> GetPersonal()
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            var usersWorkouts = await readService.Get(x => x.CreatorId == userId, 0, 10, "none");
            return Ok(usersWorkouts.Select(simpleResponseMapper.Map));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSimple()
        {
            var workouts = await readService.Get(x => true, 0, 10, "creator");
            return Ok(workouts.Select(simpleResponseMapper.Map));
        }

        [HttpGet("detailed")]
        public async Task<IActionResult> GetAllDetailed()
        {
            var workouts = await readService.Get(x => true, 0, 10, "sets,creator,likes,favorites,comments");
            return Ok(workouts.Select(detailedResponseMapper.Map));
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateWorkoutRequestDTO request)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            var workout = createRequestMapper.Map(request);
            workout.CreatorId = userId;
            var newId = await createService.Add(workout);
            if (newId == default)
                return BadRequest("Failed to create workout");

            return Ok();
        }
    }
}
