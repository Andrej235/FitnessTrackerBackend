using FitnessTracker.DTOs.Requests.Split;
using FitnessTracker.DTOs.Responses.Split;
using FitnessTracker.Models;
using FitnessTracker.Services.Create;
using FitnessTracker.Services.Mapping.Request;
using FitnessTracker.Services.Mapping.Response;
using FitnessTracker.Services.Read.ExpressionBased;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    [ApiController]
    [Route("api/split")]
    public class SplitController(ICreateService<Split> createService,
                                 IReadSingleService<Split> readSingleService,
                                 IReadRangeService<Split> readRangeService,
                                 IRequestMapper<CreateSplitRequestDTO, Split> createRequestMapper,
                                 IResponseMapper<Split, SimpleSplitResponseDTO> simpleResponseMapper,
                                 IResponseMapper<Split, DetailedSplitResponseDTO> detailedResponseMapper) : ControllerBase
    {
        private readonly ICreateService<Split> createService = createService;
        private readonly IReadSingleService<Split> readSingleService = readSingleService;
        private readonly IReadRangeService<Split> readRangeService = readRangeService;
        private readonly IRequestMapper<CreateSplitRequestDTO, Split> createRequestMapper = createRequestMapper;
        private readonly IResponseMapper<Split, SimpleSplitResponseDTO> simpleResponseMapper = simpleResponseMapper;
        private readonly IResponseMapper<Split, DetailedSplitResponseDTO> detailedResponseMapper = detailedResponseMapper;

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSplitRequestDTO request)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            //TODO: If the split is public and one or more of the workouts are private, make the split private

            var mapped = createRequestMapper.Map(request);
            mapped.CreatorId = userId;

            await createService.Add(mapped);
            return Ok();
        }

        [HttpGet("public/simple")]
        public async Task<IActionResult> GetAllSimplePublic()
        {
            var workouts = await readRangeService.Get(x => x.IsPublic, 0, 10, "creator");
            return Ok(workouts.Select(simpleResponseMapper.Map));
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpGet("personal/simple")]
        public async Task<IActionResult> GetAllSimplePersonal()
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            var workouts = await readRangeService.Get(x => x.CreatorId == userId, 0, 10, "creator");
            return Ok(workouts.Select(simpleResponseMapper.Map));
        }

        [Authorize]
        [HttpGet("{id:guid}/detailed")]
        public async Task<IActionResult> GetDetailed(Guid id)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            var workout = await readSingleService.Get(x => x.Id == id, "detailed");
            if (workout is null)
                return NotFound();

            if (!workout.IsPublic && workout.CreatorId != userId)
                return Unauthorized();

            var mapped = detailedResponseMapper.Map(workout);
            mapped.IsLiked = workout.Likes.Any(x => x.Id == userId);
            mapped.IsFavorited = workout.Favorites.Any(x => x.Id == userId);
            return Ok(mapped);
        }
    }
}
