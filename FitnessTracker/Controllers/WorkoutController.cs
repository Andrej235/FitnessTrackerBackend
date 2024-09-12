﻿using FitnessTracker.DTOs.Requests.Set;
using FitnessTracker.DTOs.Requests.Workout;
using FitnessTracker.DTOs.Responses.Workout;
using FitnessTracker.Models;
using FitnessTracker.Services.Create;
using FitnessTracker.Services.Delete;
using FitnessTracker.Services.Mapping.Request;
using FitnessTracker.Services.Mapping.Response;
using FitnessTracker.Services.Read;
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
                                           IReadSingleSelectedService<Workout> readSingleSelectedService,
                                           IReadRangeService<Workout> readRangeService,
                                           IReadRangeService<CompletedWorkout> completedWorkoutReadRangeService,
                                           IReadRangeSelectedService<WorkoutComment> commentSelectService,
                                           IReadRangeService<WorkoutLike> likeReadRangeService,
                                           IReadRangeService<FavoriteWorkout> favoriteReadRangeService,
                                           IUpdateService<Workout> updateService,
                                           IExecuteDeleteService<Workout> deleteService,
                                           IExecuteDeleteService<WorkoutComment> commentDeleteService,
                                           IExecuteDeleteService<WorkoutCommentLike> commentLikeDeleteService,
                                           IExecuteDeleteService<WorkoutLike> likeDeleteService,
                                           IExecuteDeleteService<FavoriteWorkout> favoriteDeleteService,
                                           IExecuteDeleteService<WorkoutComment> commentExecuteDeleteService,
                                           IExecuteDeleteService<WorkoutCommentLike> commentLikeExecuteDeleteService,
                                           IExecuteDeleteService<WorkoutLike> likeExecuteDeleteService,
                                           IExecuteDeleteService<FavoriteWorkout> favoriteExecuteDeleteService,
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
        private readonly IReadSingleSelectedService<Workout> readSingleSelectedService = readSingleSelectedService;
        private readonly IReadRangeService<Workout> readRangeService = readRangeService;
        private readonly IReadRangeService<CompletedWorkout> completedWorkoutReadRangeService = completedWorkoutReadRangeService;
        private readonly IReadRangeSelectedService<WorkoutComment> commentSelectService = commentSelectService;
        private readonly IReadRangeService<WorkoutLike> likeReadRangeService = likeReadRangeService;
        private readonly IReadRangeService<FavoriteWorkout> favoriteReadRangeService = favoriteReadRangeService;
        private readonly IUpdateService<Workout> updateService = updateService;
        private readonly IExecuteDeleteService<Workout> deleteService = deleteService;
        private readonly IExecuteDeleteService<WorkoutComment> commentDeleteService = commentDeleteService;
        private readonly IExecuteDeleteService<WorkoutCommentLike> commentLikeDeleteService = commentLikeDeleteService;
        private readonly IExecuteDeleteService<WorkoutLike> likeDeleteService = likeDeleteService;
        private readonly IExecuteDeleteService<FavoriteWorkout> favoriteDeleteService = favoriteDeleteService;
        private readonly IExecuteDeleteService<WorkoutComment> commentExecuteDeleteService = commentExecuteDeleteService;
        private readonly IExecuteDeleteService<WorkoutCommentLike> commentLikeExecuteDeleteService = commentLikeExecuteDeleteService;
        private readonly IExecuteDeleteService<WorkoutLike> likeExecuteDeleteService = likeExecuteDeleteService;
        private readonly IExecuteDeleteService<FavoriteWorkout> favoriteExecuteDeleteService = favoriteExecuteDeleteService;
        private readonly IRequestMapper<CreateWorkoutRequestDTO, Workout> createRequestMapper = createRequestMapper;
        private readonly IRequestMapper<CreateSetRequestDTO, Set> setCreateRequestMapper = setCreateRequestMapper;
        private readonly IRequestMapper<CreateWorkoutCommentRequestDTO, WorkoutComment> commentCreateRequestMapper = commentCreateRequestMapper;
        private readonly IResponseMapper<Workout, NewWorkoutResponseDTO> newWorkoutResponseMapper = newWorkoutResponseMapper;
        private readonly IResponseMapper<Workout, SimpleWorkoutResponseDTO> simpleResponseMapper = simpleResponseMapper;
        private readonly IResponseMapper<Workout, DetailedWorkoutResponseDTO> detailedResponseMapper = detailedResponseMapper;
    }
}
