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

namespace FitnessTracker.Services.ModelServices.WorkoutService
{
    public partial class WorkoutService(ICreateService<Workout> createService,
                                        ICreateService<WorkoutComment> commentCreateService,
                                        ICreateService<WorkoutCommentLike> commentLikeCreateService,
                                        ICreateService<WorkoutLike> likeCreateService,
                                        ICreateService<FavoriteWorkout> favoriteCreateService,
                                        IReadSingleService<Workout> readSingleService,
                                        IReadSingleSelectedService<Workout> readSingleSelectedService,
                                        IReadSingleSelectedService<User> userReadSingleSelectedService,
                                        IReadSingleSelectedService<WorkoutComment> commentReadSingleSelectedService,
                                        IReadRangeService<Workout> readRangeService,
                                        IReadRangeService<CompletedWorkout> completedWorkoutReadRangeService,
                                        IReadRangeSelectedService<WorkoutComment> commentSelectService,
                                        IReadRangeService<WorkoutLike> likeReadRangeService,
                                        IReadRangeService<FavoriteWorkout> favoriteReadRangeService,
                                        IUpdateSingleService<Workout> updateService,
                                        IDeleteService<Workout> deleteService,
                                        IDeleteService<WorkoutComment> commentDeleteService,
                                        IDeleteService<WorkoutCommentLike> commentLikeDeleteService,
                                        IDeleteService<WorkoutLike> likeDeleteService,
                                        IDeleteService<FavoriteWorkout> favoriteDeleteService,
                                        IRequestMapper<CreateWorkoutRequestDTO, Workout> createRequestMapper,
                                        IRequestMapper<CreateSetRequestDTO, Set> setCreateRequestMapper,
                                        IRequestMapper<CreateWorkoutCommentRequestDTO, WorkoutComment> commentCreateRequestMapper,
                                        IResponseMapper<Workout, NewWorkoutResponseDTO> newWorkoutResponseMapper,
                                        IResponseMapper<Workout, SimpleWorkoutResponseDTO> simpleResponseMapper,
                                        IResponseMapper<Workout, DetailedWorkoutResponseDTO> detailedResponseMapper) : IWorkoutService
    {
        private readonly ICreateService<Workout> createService = createService;
        private readonly ICreateService<WorkoutComment> commentCreateService = commentCreateService;
        private readonly ICreateService<WorkoutCommentLike> commentLikeCreateService = commentLikeCreateService;
        private readonly ICreateService<WorkoutLike> likeCreateService = likeCreateService;
        private readonly ICreateService<FavoriteWorkout> favoriteCreateService = favoriteCreateService;
        private readonly IReadSingleService<Workout> readSingleService = readSingleService;
        private readonly IReadSingleSelectedService<Workout> readSingleSelectedService = readSingleSelectedService;
        private readonly IReadSingleSelectedService<User> userReadSingleSelectedService = userReadSingleSelectedService;
        private readonly IReadSingleSelectedService<WorkoutComment> commentReadSingleSelectedService = commentReadSingleSelectedService;
        private readonly IReadRangeService<Workout> readRangeService = readRangeService;
        private readonly IReadRangeService<CompletedWorkout> completedWorkoutReadRangeService = completedWorkoutReadRangeService;
        private readonly IReadRangeSelectedService<WorkoutComment> commentSelectService = commentSelectService;
        private readonly IReadRangeService<WorkoutLike> likeReadRangeService = likeReadRangeService;
        private readonly IReadRangeService<FavoriteWorkout> favoriteReadRangeService = favoriteReadRangeService;
        private readonly IUpdateSingleService<Workout> updateService = updateService;
        private readonly IDeleteService<Workout> deleteService = deleteService;
        private readonly IDeleteService<WorkoutComment> commentDeleteService = commentDeleteService;
        private readonly IDeleteService<WorkoutCommentLike> commentLikeDeleteService = commentLikeDeleteService;
        private readonly IDeleteService<WorkoutLike> likeDeleteService = likeDeleteService;
        private readonly IDeleteService<FavoriteWorkout> favoriteDeleteService = favoriteDeleteService;
        private readonly IRequestMapper<CreateWorkoutRequestDTO, Workout> createRequestMapper = createRequestMapper;
        private readonly IRequestMapper<CreateSetRequestDTO, Set> setCreateRequestMapper = setCreateRequestMapper;
        private readonly IRequestMapper<CreateWorkoutCommentRequestDTO, WorkoutComment> commentCreateRequestMapper = commentCreateRequestMapper;
        private readonly IResponseMapper<Workout, NewWorkoutResponseDTO> newWorkoutResponseMapper = newWorkoutResponseMapper;
        private readonly IResponseMapper<Workout, SimpleWorkoutResponseDTO> simpleResponseMapper = simpleResponseMapper;
        private readonly IResponseMapper<Workout, DetailedWorkoutResponseDTO> detailedResponseMapper = detailedResponseMapper;
    }
}
