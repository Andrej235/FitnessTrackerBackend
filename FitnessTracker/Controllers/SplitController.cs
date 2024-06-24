using FitnessTracker.DTOs.Requests.Split;
using FitnessTracker.DTOs.Requests.Workout;
using FitnessTracker.DTOs.Responses.Split;
using FitnessTracker.Models;
using FitnessTracker.Services.Create;
using FitnessTracker.Services.Delete;
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
                                 ICreateService<SplitComment> commentCreateService,
                                 ICreateService<SplitCommentLike> commentLikeCreateService,
                                 ICreateService<SplitLike> likeCreateService,
                                 ICreateService<FavoriteSplit> favoriteCreateService,
                                 IReadSingleService<Split> readSingleService,
                                 IReadRangeService<Split> readRangeService,
                                 IReadRangeService<SplitComment> commentReadRangeService,
                                 IDeleteService<SplitComment> commentDeleteService,
                                 IDeleteService<SplitCommentLike> commentLikeDeleteService,
                                 IDeleteService<SplitLike> likeDeleteService,
                                 IDeleteService<FavoriteSplit> favoriteDeleteService,
                                 IDeleteRangeService<SplitComment> commentDeleteRangeService,
                                 IRequestMapper<CreateSplitRequestDTO, Split> createRequestMapper,
                                 IRequestMapper<CreateSplitCommentRequestDTO, SplitComment> createCommentRequestMapper,
                                 IResponseMapper<Split, SimpleSplitResponseDTO> simpleResponseMapper,
                                 IResponseMapper<Split, DetailedSplitResponseDTO> detailedResponseMapper,
                                 IResponseMapper<SplitComment, SimpleSplitCommentResponseDTO> simpleCommentResponseMapper) : ControllerBase
    {
        private readonly ICreateService<Split> createService = createService;
        private readonly ICreateService<SplitComment> commentCreateService = commentCreateService;
        private readonly ICreateService<SplitCommentLike> commentLikeCreateService = commentLikeCreateService;
        private readonly ICreateService<SplitLike> likeCreateService = likeCreateService;
        private readonly ICreateService<FavoriteSplit> favoriteCreateService = favoriteCreateService;
        private readonly IReadSingleService<Split> readSingleService = readSingleService;
        private readonly IReadRangeService<Split> readRangeService = readRangeService;
        private readonly IReadRangeService<SplitComment> commentReadRangeService = commentReadRangeService;
        private readonly IDeleteService<SplitComment> commentDeleteService = commentDeleteService;
        private readonly IDeleteService<SplitCommentLike> commentLikeDeleteService = commentLikeDeleteService;
        private readonly IDeleteService<SplitLike> likeDeleteService = likeDeleteService;
        private readonly IDeleteService<FavoriteSplit> favoriteDeleteService = favoriteDeleteService;
        private readonly IDeleteRangeService<SplitComment> commentDeleteRangeService = commentDeleteRangeService;
        private readonly IRequestMapper<CreateSplitRequestDTO, Split> createRequestMapper = createRequestMapper;
        private readonly IRequestMapper<CreateSplitCommentRequestDTO, SplitComment> createCommentRequestMapper = createCommentRequestMapper;
        private readonly IResponseMapper<Split, SimpleSplitResponseDTO> simpleResponseMapper = simpleResponseMapper;
        private readonly IResponseMapper<Split, DetailedSplitResponseDTO> detailedResponseMapper = detailedResponseMapper;
        private readonly IResponseMapper<SplitComment, SimpleSplitCommentResponseDTO> simpleCommentResponseMapper = simpleCommentResponseMapper;

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

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpPost("{id:guid}/like")]
        public async Task<IActionResult> CreateLike(Guid id)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            try
            {
                await likeCreateService.Add(new()
                {
                    UserId = userId,
                    SplitId = id
                });
                return Ok();

            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest("Failed to like");
            }
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpDelete("{id:guid}/like")]
        public async Task<IActionResult> DeleteLike(Guid id)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            try
            {
                await likeDeleteService.Delete(x => x.UserId == userId && x.SplitId == id);
                return Ok();
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest("Failed to remove like");
            }
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpPost("{id:guid}/favorite")]
        public async Task<IActionResult> CreateFavorite(Guid id)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            try
            {
                await favoriteCreateService.Add(new()
                {
                    UserId = userId,
                    SplitId = id
                });
                return Ok();
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest("Failed to favorite");
            }
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpDelete("{id:guid}/favorite")]
        public async Task<IActionResult> DeleteFavorite(Guid id)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            try
            {
                await favoriteDeleteService.Delete(x => x.UserId == userId && x.SplitId == id);
                return Ok();
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest("Failed to remove favorite");
            }
        }

    }
}
