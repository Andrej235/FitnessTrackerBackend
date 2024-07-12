using FitnessTracker.DTOs.Requests.Set;
using FitnessTracker.DTOs.Requests.Workout;
using FitnessTracker.DTOs.Responses.Workout;
using FitnessTracker.Models;
using FitnessTracker.Services.Create;
using FitnessTracker.Services.Delete;
using FitnessTracker.Services.Mapping.Request;
using FitnessTracker.Services.Mapping.Response;
using FitnessTracker.Services.Read.ExpressionBased;
using FitnessTracker.Services.Update;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    [ApiController]
    [Route("api/workout")]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public partial class WorkoutController(ICreateService<Workout> createService,
                                           ICreateService<WorkoutComment> commentCreateService,
                                           ICreateService<WorkoutCommentLike> commentLikeCreateService,
                                           ICreateService<WorkoutLike> likeCreateService,
                                           ICreateService<FavoriteWorkout> favoriteCreateService,
                                           IReadSingleService<Workout> readSingleService,
                                           IReadRangeService<CompletedWorkout> completedWorkoutReadSingleService,
                                           IReadRangeService<Workout> readRangeService,
                                           IReadRangeService<WorkoutComment> commentReadRangeService,
                                           IUpdateService<Workout> updateService,
                                           IUpdateService<Set> setUpdateService,
                                           IDeleteService<WorkoutComment> commentDeleteService,
                                           IDeleteService<WorkoutCommentLike> commentLikeDeleteService,
                                           IDeleteService<WorkoutLike> likeDeleteService,
                                           IDeleteService<FavoriteWorkout> favoriteDeleteService,
                                           IDeleteRangeService<WorkoutComment> commentDeleteRangeService,
                                           IRequestMapper<CreateWorkoutRequestDTO, Workout> createRequestMapper,
                                           IRequestMapper<CreateSetRequestDTO, Set> setCreateRequestMapper,
                                           IRequestMapper<CreateWorkoutCommentRequestDTO, WorkoutComment> commentCreateRequestMapper,
                                           IResponseMapper<Workout, NewWorkoutResponseDTO> newWorkoutResponseMapper,
                                           IResponseMapper<Workout, SimpleWorkoutResponseDTO> simpleResponseMapper,
                                           IResponseMapper<Workout, DetailedWorkoutResponseDTO> detailedResponseMapper,
                                           IResponseMapper<WorkoutComment, SimpleWorkoutCommentResponseDTO> simpleCommentResponseMapper) : ControllerBase
    {
        private readonly ICreateService<Workout> createService = createService;
        private readonly ICreateService<WorkoutComment> commentCreateService = commentCreateService;
        private readonly ICreateService<WorkoutCommentLike> commentLikeCreateService = commentLikeCreateService;
        private readonly ICreateService<WorkoutLike> likeCreateService = likeCreateService;
        private readonly ICreateService<FavoriteWorkout> favoriteCreateService = favoriteCreateService;
        private readonly IReadSingleService<Workout> readSingleService = readSingleService;
        private readonly IReadRangeService<CompletedWorkout> completedWorkoutReadSingleService = completedWorkoutReadSingleService;
        private readonly IReadRangeService<Workout> readRangeService = readRangeService;
        private readonly IReadRangeService<WorkoutComment> commentReadRangeService = commentReadRangeService;
        private readonly IUpdateService<Workout> updateService = updateService;
        private readonly IUpdateService<Set> setUpdateService = setUpdateService;
        private readonly IDeleteService<WorkoutComment> commentDeleteService = commentDeleteService;
        private readonly IDeleteService<WorkoutCommentLike> commentLikeDeleteService = commentLikeDeleteService;
        private readonly IDeleteService<WorkoutLike> likeDeleteService = likeDeleteService;
        private readonly IDeleteService<FavoriteWorkout> favoriteDeleteService = favoriteDeleteService;
        private readonly IDeleteRangeService<WorkoutComment> commentDeleteRangeService = commentDeleteRangeService;
        private readonly IRequestMapper<CreateWorkoutRequestDTO, Workout> createRequestMapper = createRequestMapper;
        private readonly IRequestMapper<CreateSetRequestDTO, Set> setCreateRequestMapper = setCreateRequestMapper;
        private readonly IRequestMapper<CreateWorkoutCommentRequestDTO, WorkoutComment> commentCreateRequestMapper = commentCreateRequestMapper;
        private readonly IResponseMapper<Workout, SimpleWorkoutResponseDTO> simpleResponseMapper = simpleResponseMapper;
        private readonly IResponseMapper<Workout, DetailedWorkoutResponseDTO> detailedResponseMapper = detailedResponseMapper;
        private readonly IResponseMapper<WorkoutComment, SimpleWorkoutCommentResponseDTO> simpleCommentResponseMapper = simpleCommentResponseMapper;
    }
}
