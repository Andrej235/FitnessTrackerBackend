using FitnessTracker.DTOs.Requests.Split;
using FitnessTracker.DTOs.Responses.Split;
using FitnessTracker.DTOs.Responses.Workout;
using FitnessTracker.Models;
using FitnessTracker.Services.Create;
using FitnessTracker.Services.Delete;
using FitnessTracker.Services.Mapping.Request;
using FitnessTracker.Services.Mapping.Response;
using FitnessTracker.Services.Read;
using FitnessTracker.Services.Update;

namespace FitnessTracker.Services.ModelServices.SplitService
{
    public partial class SplitService(ICreateService<Split> createService,
                                      ICreateService<SplitComment> commentCreateService,
                                      ICreateService<SplitCommentLike> commentLikeCreateService,
                                      ICreateService<SplitLike> likeCreateService,
                                      ICreateService<FavoriteSplit> favoriteCreateService,
                                      IReadSingleSelectedService<User> userReadSingleService,
                                      IReadSingleService<Split> readSingleService,
                                      IReadSingleSelectedService<Split> readSingleSelectedService,
                                      IReadRangeService<Split> readRangeService,
                                      IReadRangeService<FavoriteSplit> favoriteReadRangeService,
                                      IReadRangeService<SplitLike> likeReadRangeService,
                                      IReadRangeSelectedService<SplitComment> commentReadSelectedRangeService,
                                      IReadRangeService<Workout> workoutReadRangeService,
                                      IUpdateSingleService<Split> updateService,
                                      IUpdateSingleService<SplitWorkout> splitWorkoutUpdateService,
                                      IDeleteService<SplitComment> commentDeleteService,
                                      IDeleteService<SplitCommentLike> commentLikeDeleteService,
                                      IDeleteService<SplitLike> likeDeleteService,
                                      IDeleteService<FavoriteSplit> favoriteDeleteService,
                                      IRequestMapper<CreateSplitRequestDTO, Split> createRequestMapper,
                                      IRequestMapper<CreateSplitCommentRequestDTO, SplitComment> createCommentRequestMapper,
                                      IResponseMapper<Split, SimpleSplitResponseDTO> simpleResponseMapper,
                                      IResponseMapper<Split, DetailedUserSplitResponseDTO> detailedUserSplitResponseMapper,
                                      IResponseMapper<Split, DetailedSplitResponseDTO> detailedResponseMapper,
                                      IResponseMapper<SplitComment, SimpleSplitCommentResponseDTO> simpleCommentResponseMapper,
                                      IResponseMapper<Workout, SimpleWorkoutOptionResponseDTO> simpleWorkoutOptionResponseMapper,
                                      IReadRangeSelectedService<Workout> workoutReadSelectedRangeService) : ISplitService
    {
        private readonly ICreateService<Split> createService = createService;
        private readonly ICreateService<SplitComment> commentCreateService = commentCreateService;
        private readonly ICreateService<SplitCommentLike> commentLikeCreateService = commentLikeCreateService;
        private readonly ICreateService<SplitLike> likeCreateService = likeCreateService;
        private readonly ICreateService<FavoriteSplit> favoriteCreateService = favoriteCreateService;
        private readonly IReadSingleSelectedService<User> userReadSingleService = userReadSingleService;
        private readonly IReadSingleService<Split> readSingleService = readSingleService;
        private readonly IReadSingleSelectedService<Split> readSingleSelectedService = readSingleSelectedService;
        private readonly IReadRangeService<Split> readRangeService = readRangeService;
        private readonly IReadRangeSelectedService<SplitComment> commentReadSelectedRangeService = commentReadSelectedRangeService;
        private readonly IReadRangeService<Workout> workoutReadRangeService = workoutReadRangeService;
        private readonly IReadRangeSelectedService<Workout> workoutReadSelectedRangeService = workoutReadSelectedRangeService;
        private readonly IUpdateSingleService<Split> updateService = updateService;
        private readonly IUpdateSingleService<SplitWorkout> splitWorkoutUpdateService = splitWorkoutUpdateService;
        private readonly IDeleteService<SplitComment> commentDeleteService = commentDeleteService;
        private readonly IDeleteService<SplitCommentLike> commentLikeDeleteService = commentLikeDeleteService;
        private readonly IDeleteService<SplitLike> likeDeleteService = likeDeleteService;
        private readonly IDeleteService<FavoriteSplit> favoriteDeleteService = favoriteDeleteService;
        private readonly IRequestMapper<CreateSplitRequestDTO, Split> createRequestMapper = createRequestMapper;
        private readonly IRequestMapper<CreateSplitCommentRequestDTO, SplitComment> createCommentRequestMapper = createCommentRequestMapper;
        private readonly IResponseMapper<Split, SimpleSplitResponseDTO> simpleResponseMapper = simpleResponseMapper;
        private readonly IResponseMapper<Workout, SimpleWorkoutOptionResponseDTO> simpleWorkoutOptionResponseMapper = simpleWorkoutOptionResponseMapper;
        private readonly IResponseMapper<Split, DetailedUserSplitResponseDTO> detailedUserSplitResponseMapper = detailedUserSplitResponseMapper;
        private readonly IResponseMapper<Split, DetailedSplitResponseDTO> detailedResponseMapper = detailedResponseMapper;
        private readonly IResponseMapper<SplitComment, SimpleSplitCommentResponseDTO> simpleCommentResponseMapper = simpleCommentResponseMapper;
    }
}
