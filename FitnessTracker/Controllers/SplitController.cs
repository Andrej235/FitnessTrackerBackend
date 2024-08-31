using FitnessTracker.DTOs.Requests.Split;
using FitnessTracker.DTOs.Responses.Split;
using FitnessTracker.Models;
using FitnessTracker.Services.Create;
using FitnessTracker.Services.Delete;
using FitnessTracker.Services.Mapping.Request;
using FitnessTracker.Services.Mapping.Response;
using FitnessTracker.Services.Read.Full;
using FitnessTracker.Services.Update;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    [ApiController]
    [Route("api/split")]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public partial class SplitController(ICreateService<Split> createService,
                                         ICreateService<SplitComment> commentCreateService,
                                         ICreateService<SplitCommentLike> commentLikeCreateService,
                                         ICreateService<SplitLike> likeCreateService,
                                         ICreateService<FavoriteSplit> favoriteCreateService,
                                         IFullReadService<Split> readSingleService,
                                         IFullReadRangeService<Split> readRangeService,
                                         IFullReadRangeService<SplitComment> commentReadRangeService,
                                         IFullReadRangeService<Workout> workoutReadRangeService,
                                         IUpdateService<Split> updateService,
                                         IUpdateService<SplitWorkout> splitWorkoutUpdateService,
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
        private readonly IFullReadService<Split> readSingleService = readSingleService;
        private readonly IFullReadRangeService<Split> readRangeService = readRangeService;
        private readonly IFullReadRangeService<SplitComment> commentReadRangeService = commentReadRangeService;
        private readonly IFullReadRangeService<Workout> workoutReadRangeService = workoutReadRangeService;
        private readonly IUpdateService<Split> updateService = updateService;
        private readonly IUpdateService<SplitWorkout> splitWorkoutUpdateService = splitWorkoutUpdateService;
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
    }
}
