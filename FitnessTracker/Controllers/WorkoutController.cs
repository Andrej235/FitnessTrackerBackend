using FitnessTracker.DTOs.Requests.Workout;
using FitnessTracker.DTOs.Responses.Workout;
using FitnessTracker.Models;
using FitnessTracker.Services.Create;
using FitnessTracker.Services.Delete;
using FitnessTracker.Services.Mapping.Request;
using FitnessTracker.Services.Mapping.Response;
using FitnessTracker.Services.Read.ExpressionBased;
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
                                   ICreateService<WorkoutComment> commentCreateService,
                                   ICreateService<WorkoutCommentLike> commentLikeCreateService,
                                   IReadSingleService<Workout> readSingleService,
                                   IReadRangeService<Workout> readRangeService,
                                   IReadSingleService<WorkoutComment> commentReadSingleService,
                                   IReadRangeService<WorkoutComment> commentReadRangeService,
                                   IUpdateService<Workout> updateService,
                                   IDeleteService<Workout> deleteService,
                                   IDeleteService<WorkoutComment> commentDeleteService,
                                   IDeleteService<WorkoutCommentLike> commentLikeDeleteService,
                                   IDeleteRangeService<WorkoutComment> commentDeleteRangeService,
                                   ICreateService<WorkoutLike> likeCreateService,
                                   IDeleteService<WorkoutLike> likeDeleteService,
                                   ICreateService<FavoriteWorkout> favoriteCreateService,
                                   IDeleteService<FavoriteWorkout> favoriteDeleteService,
                                   IRequestMapper<CreateWorkoutRequestDTO, Workout> createRequestMapper,
                                   IRequestMapper<CreateWorkoutCommentRequestDTO, WorkoutComment> createCommentRequestMapper,
                                   IResponseMapper<Workout, SimpleWorkoutResponseDTO> simpleResponseMapper,
                                   IResponseMapper<Workout, DetailedWorkoutResponseDTO> detailedResponseMapper,
                                   IResponseMapper<WorkoutComment, SimpleWorkoutCommentResponseDTO> simpleCommentResponseMapper) : ControllerBase
    {
        private readonly ICreateService<Workout> createService = createService;
        private readonly ICreateService<WorkoutComment> commentCreateService = commentCreateService;
        private readonly ICreateService<WorkoutCommentLike> commentLikeCreateService = commentLikeCreateService;
        private readonly IReadSingleService<Workout> readSingleService = readSingleService;
        private readonly IReadRangeService<Workout> readRangeService = readRangeService;
        private readonly IReadSingleService<WorkoutComment> commentReadSingleService = commentReadSingleService;
        private readonly IReadRangeService<WorkoutComment> commentReadRangeService = commentReadRangeService;
        private readonly IUpdateService<Workout> updateService = updateService;
        private readonly IDeleteService<Workout> deleteService = deleteService;
        private readonly IDeleteService<WorkoutComment> commentDeleteService = commentDeleteService;
        private readonly IDeleteService<WorkoutCommentLike> commentLikeDeleteService = commentLikeDeleteService;
        private readonly IDeleteRangeService<WorkoutComment> commentDeleteRangeService = commentDeleteRangeService;
        private readonly ICreateService<WorkoutLike> likeCreateService = likeCreateService;
        private readonly IDeleteService<WorkoutLike> likeDeleteService = likeDeleteService;
        private readonly ICreateService<FavoriteWorkout> favoriteCreateService = favoriteCreateService;
        private readonly IDeleteService<FavoriteWorkout> favoriteDeleteService = favoriteDeleteService;
        private readonly IRequestMapper<CreateWorkoutRequestDTO, Workout> createRequestMapper = createRequestMapper;
        private readonly IRequestMapper<CreateWorkoutCommentRequestDTO, WorkoutComment> createCommentRequestMapper = createCommentRequestMapper;
        private readonly IResponseMapper<Workout, SimpleWorkoutResponseDTO> simpleResponseMapper = simpleResponseMapper;
        private readonly IResponseMapper<Workout, DetailedWorkoutResponseDTO> detailedResponseMapper = detailedResponseMapper;
        private readonly IResponseMapper<WorkoutComment, SimpleWorkoutCommentResponseDTO> simpleCommentResponseMapper = simpleCommentResponseMapper;

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpGet("personal")]
        public async Task<IActionResult> GetPersonal()
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            var usersWorkouts = await readRangeService.Get(x => x.CreatorId == userId, 0, 10, "creator");
            return Ok(usersWorkouts.Select(simpleResponseMapper.Map));
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

            if (workout.IsPublic || workout.CreatorId == userId)
            {
                var mapped = detailedResponseMapper.Map(workout);
                mapped.IsLiked = workout.Likes.Any(x => x.Id == userId);
                mapped.IsFavorited = workout.Favorites.Any(x => x.Id == userId);
                return Ok(mapped);
            }

            return Unauthorized();
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
                await likeCreateService.Add(new WorkoutLike
                {
                    UserId = userId,
                    WorkoutId = id
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
                await likeDeleteService.Delete(x => x.UserId == userId && x.WorkoutId == id);
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
        public async Task<IActionResult> Createfavorite(Guid id)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            try
            {
                await favoriteCreateService.Add(new FavoriteWorkout
                {
                    UserId = userId,
                    WorkoutId = id
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
        public async Task<IActionResult> Deletefavorite(Guid id)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            try
            {
                await favoriteDeleteService.Delete(x => x.UserId == userId && x.WorkoutId == id);
                return Ok();
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest("Failed to remove favorite");
            }
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpPost("{workoutId:guid}/comment")]
        public async Task<IActionResult> CreateComment(Guid workoutId, [FromBody] CreateWorkoutCommentRequestDTO request)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            try
            {
                var mapped = createCommentRequestMapper.Map(request);
                mapped.CreatorId = userId;
                mapped.WorkoutId = workoutId;

                var newId = await commentCreateService.Add(mapped);
                if (newId == default)
                    return BadRequest("Failed to create comment");

                return Ok();
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest("Failed to create comment");
            }
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpDelete("{workoutId:guid}/comment/{commentId:guid}")]
        public async Task<IActionResult> DeleteComment(Guid workoutId, Guid commentId)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            try
            {
                await commentDeleteService.Delete(x => x.WorkoutId == workoutId && x.CreatorId == userId && x.Id == commentId);
                await commentDeleteRangeService.Delete(x => x.WorkoutId == workoutId && x.ParentId == commentId);
                return Ok();
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest("Failed to delete comment");
            }
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpPost("{workoutId:guid}/comment/{parentId:guid}/reply")]
        public async Task<IActionResult> CreateComment(Guid workoutId, Guid parentId, [FromBody] CreateWorkoutCommentRequestDTO request)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            try
            {
                var mapped = createCommentRequestMapper.Map(request);
                mapped.CreatorId = userId;
                mapped.WorkoutId = workoutId;
                mapped.ParentId = parentId;

                var newId = await commentCreateService.Add(mapped);
                if (newId == default)
                    return BadRequest("Failed to create comment");

                return Ok();
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest("Failed to create comment");
            }
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpGet("{workoutId:guid}/comment")]
        public async Task<IActionResult> GetComments(Guid workoutId)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            var comments = await commentReadRangeService.Get(x => x.WorkoutId == workoutId && x.ParentId == null, 0, 10, "creator,likes");
            var mapped = comments.Select(x =>
            {
                var mapped = simpleCommentResponseMapper.Map(x);
                mapped.IsLiked = x.Likes.Any(x => x.Id == userId);
                return mapped;
            });
            return Ok(mapped);
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpGet("{workoutId:guid}/comment/{commentId:guid}/reply")]
        public async Task<IActionResult> GetReplies(Guid workoutId, Guid commentId)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            var comments = await commentReadRangeService.Get(x => x.WorkoutId == workoutId && x.ParentId == commentId, 0, 10, "creator,likes");
            var mapped = comments.Select(x =>
            {
                var mapped = simpleCommentResponseMapper.Map(x);
                mapped.IsLiked = x.Likes.Any(x => x.Id == userId);
                return mapped;
            });
            return Ok(mapped);
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpPost("comment/{id:guid}/like")]
        public async Task<IActionResult> CreateCommentLike(Guid id) 
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            try
            {
                await commentLikeCreateService.Add(new WorkoutCommentLike
                {
                    UserId = userId,
                    WorkoutCommentId = id
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
        [HttpDelete("comment/{id:guid}/like")]
        public async Task<IActionResult> DeleteCommentLike(Guid id)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            try
            {
                await commentLikeDeleteService.Delete(x => x.UserId == userId && x.WorkoutCommentId == id);
                return Ok();
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest("Failed to remove like");
            }
        }
    }
}
