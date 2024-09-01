using FitnessTracker.DTOs.Requests.Set;
using FitnessTracker.DTOs.Requests.Workout;
using FitnessTracker.DTOs.Responses.Workout;
using FitnessTracker.Models;
using FitnessTracker.Services.Count;
using FitnessTracker.Services.Create;
using FitnessTracker.Services.Delete;
using FitnessTracker.Services.Mapping.Request;
using FitnessTracker.Services.Mapping.Response;
using FitnessTracker.Services.Read;
using FitnessTracker.Services.ReadSelected;
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
                                           IReadRangeService<Workout> readRangeService,
                                           IReadSingleService<WorkoutLike> likeReadSingleService,
                                           IReadSingleService<FavoriteWorkout> favoriteReadSingleService,
                                           IReadRangeService<CompletedWorkout> completedWorkoutReadSingleService,
                                           IReadRangeService<WorkoutComment> commentReadRangeService,
                                           IReadRangeSelectedService<WorkoutComment> commentSelectService,
                                           IReadRangeService<WorkoutLike> likeReadRangeService,
                                           IReadRangeService<FavoriteWorkout> favoriteReadRangeService,
                                           ICountService<WorkoutLike> likeCountService,
                                           ICountService<FavoriteWorkout> favoriteCountService,
                                           ICountService<WorkoutComment> commentCountService,
                                           IUpdateService<Workout> updateService,
                                           IDeleteService<Workout> deleteService,
                                           IDeleteService<WorkoutComment> commentDeleteService,
                                           IDeleteService<WorkoutCommentLike> commentLikeDeleteService,
                                           IDeleteService<WorkoutLike> likeDeleteService,
                                           IDeleteService<FavoriteWorkout> favoriteDeleteService,
                                           IDeleteRangeService<WorkoutComment> commentDeleteRangeService,
                                           IDeleteRangeService<WorkoutCommentLike> commentLikeDeleteRangeService,
                                           IDeleteRangeService<WorkoutLike> likeDeleteRangeService,
                                           IDeleteRangeService<FavoriteWorkout> favoriteDeleteRangeService,
                                           IRequestMapper<CreateWorkoutRequestDTO, Workout> createRequestMapper,
                                           IRequestMapper<CreateSetRequestDTO, Set> setCreateRequestMapper,
                                           IRequestMapper<CreateWorkoutCommentRequestDTO, WorkoutComment> commentCreateRequestMapper,
                                           IResponseMapper<Workout, NewWorkoutResponseDTO> newWorkoutResponseMapper,
                                           IResponseMapper<Workout, SimpleWorkoutResponseDTO> simpleResponseMapper,
                                           IResponseMapper<Workout, DetailedWorkoutResponseDTO> detailedResponseMapper) : ControllerBase
    {
        private readonly ICreateService<Workout> createService = createService;
        private readonly ICreateService<WorkoutComment> commentCreateService = commentCreateService;
        private readonly ICreateService<WorkoutCommentLike> commentLikeCreateService = commentLikeCreateService;
        private readonly ICreateService<WorkoutLike> likeCreateService = likeCreateService;
        private readonly ICreateService<FavoriteWorkout> favoriteCreateService = favoriteCreateService;
        private readonly IReadSingleService<Workout> readSingleService = readSingleService;
        private readonly IReadRangeService<Workout> readRangeService = readRangeService;
        private readonly IReadSingleService<WorkoutLike> likeReadSingleService = likeReadSingleService;
        private readonly IReadSingleService<FavoriteWorkout> favoriteReadSingleService = favoriteReadSingleService;
        private readonly IReadRangeService<CompletedWorkout> completedWorkoutReadSingleService = completedWorkoutReadSingleService;
        private readonly IReadRangeService<WorkoutComment> commentReadRangeService = commentReadRangeService;
        private readonly IReadRangeSelectedService<WorkoutComment> commentSelectService = commentSelectService;
        private readonly IReadRangeService<WorkoutLike> likeReadRangeService = likeReadRangeService;
        private readonly IReadRangeService<FavoriteWorkout> favoriteReadRangeService = favoriteReadRangeService;
        private readonly ICountService<WorkoutLike> likeCountService = likeCountService;
        private readonly ICountService<FavoriteWorkout> favoriteCountService = favoriteCountService;
        private readonly ICountService<WorkoutComment> commentCountService = commentCountService;
        private readonly IUpdateService<Workout> updateService = updateService;
        private readonly IDeleteService<Workout> deleteService = deleteService;
        private readonly IDeleteService<WorkoutComment> commentDeleteService = commentDeleteService;
        private readonly IDeleteService<WorkoutCommentLike> commentLikeDeleteService = commentLikeDeleteService;
        private readonly IDeleteService<WorkoutLike> likeDeleteService = likeDeleteService;
        private readonly IDeleteService<FavoriteWorkout> favoriteDeleteService = favoriteDeleteService;
        private readonly IDeleteRangeService<WorkoutComment> commentDeleteRangeService = commentDeleteRangeService;
        private readonly IDeleteRangeService<WorkoutCommentLike> commentLikeDeleteRangeService = commentLikeDeleteRangeService;
        private readonly IDeleteRangeService<WorkoutLike> likeDeleteRangeService = likeDeleteRangeService;
        private readonly IDeleteRangeService<FavoriteWorkout> favoriteDeleteRangeService = favoriteDeleteRangeService;
        private readonly IRequestMapper<CreateWorkoutRequestDTO, Workout> createRequestMapper = createRequestMapper;
        private readonly IRequestMapper<CreateSetRequestDTO, Set> setCreateRequestMapper = setCreateRequestMapper;
        private readonly IRequestMapper<CreateWorkoutCommentRequestDTO, WorkoutComment> commentCreateRequestMapper = commentCreateRequestMapper;
        private readonly IResponseMapper<Workout, NewWorkoutResponseDTO> newWorkoutResponseMapper = newWorkoutResponseMapper;
        private readonly IResponseMapper<Workout, SimpleWorkoutResponseDTO> simpleResponseMapper = simpleResponseMapper;
        private readonly IResponseMapper<Workout, DetailedWorkoutResponseDTO> detailedResponseMapper = detailedResponseMapper;
    }
}
