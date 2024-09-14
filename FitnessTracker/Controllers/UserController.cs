﻿using FitnessTracker.Auth;
using FitnessTracker.DTOs.Requests.Completed;
using FitnessTracker.DTOs.Requests.User;
using FitnessTracker.DTOs.Responses.AuthTokens;
using FitnessTracker.DTOs.Responses.CompletedWorkouts;
using FitnessTracker.DTOs.Responses.Pins;
using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.DTOs.Responses.Workout;
using FitnessTracker.Models;
using FitnessTracker.Services.Count;
using FitnessTracker.Services.Create;
using FitnessTracker.Services.Delete;
using FitnessTracker.Services.Mapping.Request;
using FitnessTracker.Services.Mapping.Response;
using FitnessTracker.Services.ModelServices.UserService;
using FitnessTracker.Services.Read;
using FitnessTracker.Services.Update;
using FitnessTracker.Services.UserServices.EmailConfirmationSenderService;
using FitnessTracker.Services.UserServices.EmailConfirmationService;
using FitnessTracker.Services.UserServices.ResetPasswordSenderService;
using FitnessTracker.Services.UserServices.ResetPasswordService;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace FitnessTracker.Controllers
{
    [Route("api/user")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public partial class UserController(IUserService userService,
                                        ICreateService<User> createService,
                                        ICreateService<UserSettings> settingsCreateService,
                                        ICreateService<CompletedWorkout> completedWorkoutCreateService,
                                        ICreateService<UserFollows> followCreateService,
                                        ICreateRangeService<SplitPin> splitPinCreateRangeService,
                                        ICreateRangeService<WorkoutPin> workoutPinCreateRangeService,
                                        IReadSingleService<User> readSingleService,
                                        IReadSingleSelectedService<User> readSingleSelectedService,
                                        IReadSingleService<UserFollows> followerReadSingleService,
                                        IReadSingleService<UserSettings> settingsReadSingleService,
                                        IReadRangeService<CompletedWorkout> completedWorkoutReadRangeService,
                                        IReadRangeService<UserFollows> followerReadRangeService,
                                        ICountService<UserFollows> followerCountService,
                                        ICountService<CompletedWorkout> completedWorkoutCountService,
                                        IUpdateService<User> updateService,
                                        IExecuteUpdateService<User> executeUpdateService,
                                        IUpdateService<UserSettings> settingsUpdateService,
                                        IUpdateRangeService<WorkoutPin> workoutPinUpdateRangeService,
                                        IUpdateRangeService<SplitPin> splitPinUpdateRangeService,
                                        IDeleteService<UserFollows> followDeleteService,
                                        IDeleteService<WorkoutPin> workoutPinDeleteService,
                                        IDeleteService<SplitPin> splitPinDeleteService,
                                        ITokenManager tokenManager,
                                        IEmailConfirmationSenderService emailConfirmationSender,
                                        IEmailConfirmationService emailConfirmationService,
                                        IResetPasswordEmailSenderService passwordResetEmailSender,
                                        IResetPasswordService passwordResetEmailService,
                                        IRequestMapper<RegisterUserRequestDTO, User> registrationMapper,
                                        IRequestMapper<CreateCompletedWorkoutRequestDTO, CompletedWorkout> createCompletedWorkoutRequestMapper,
                                        IResponseMapper<User, SimpleUserResponseDTO> simpleResponseMapper,
                                        IResponseMapper<WorkoutPin, PinResponseDTO> workoutPinResponseMapper,
                                        IResponseMapper<SplitPin, PinResponseDTO> splitPinResponseMapper,
                                        IResponseMapper<User, DetailedUserResponseDTO> detailedResponseMapper,
                                        IResponseMapper<Workout, DetailedWorkoutResponseDTO> detailedWorkoutResponseMapper,
                                        IResponseMapper<User, DetailedPublicUserResponseDTO> publicUserDetailedResponseMapper,
                                        IResponseMapper<UserSettings, UserSettingsResponseDTO> settingsResponseMapper,
                                        IResponseMapper<IGrouping<DateTime, CompletedWorkout>, SimpleWeekOfCompletedWorkoutsResponseDTO> simpleWeekOfCompletedWorkoutsResponseMapper,
                                        IResponseMapper<IEnumerable<CompletedWorkout>, DetailedWeekOfCompletedWorkoutsResponseDTO> detailedWeekOfCompletedWorkoutsResponseMapper,
                                        IResponseMapper<string, SimpleJWTResponseDTO> jwtResponseMapper) : ControllerBase
    {
        private readonly IUserService userService = userService;
        private readonly ICreateService<User> createService = createService;
        private readonly ICreateService<UserSettings> settingsCreateService = settingsCreateService;
        private readonly ICreateService<CompletedWorkout> completedWorkoutCreateService = completedWorkoutCreateService;
        private readonly ICreateService<UserFollows> followCreateService = followCreateService;
        private readonly ICreateRangeService<SplitPin> splitPinCreateRangeService = splitPinCreateRangeService;
        private readonly ICreateRangeService<WorkoutPin> workoutPinCreateRangeService = workoutPinCreateRangeService;
        private readonly IReadSingleService<User> readSingleService = readSingleService;
        private readonly IReadSingleSelectedService<User> readSingleSelectedService = readSingleSelectedService;
        private readonly IReadSingleService<UserFollows> followerReadSingleService = followerReadSingleService;
        private readonly IReadSingleService<UserSettings> settingsReadSingleService = settingsReadSingleService;
        private readonly IReadRangeService<CompletedWorkout> completedWorkoutReadRangeService = completedWorkoutReadRangeService;
        private readonly IReadRangeService<UserFollows> followerReadRangeService = followerReadRangeService;
        private readonly ICountService<UserFollows> followerCountService = followerCountService;
        private readonly ICountService<CompletedWorkout> completedWorkoutCountService = completedWorkoutCountService;
        private readonly IUpdateService<User> updateService = updateService;
        private readonly IExecuteUpdateService<User> executeUpdateService = executeUpdateService;
        private readonly IUpdateService<UserSettings> settingsUpdateService = settingsUpdateService;
        private readonly IUpdateRangeService<WorkoutPin> workoutPinUpdateRangeService = workoutPinUpdateRangeService;
        private readonly IUpdateRangeService<SplitPin> splitPinUpdateRangeService = splitPinUpdateRangeService;
        private readonly IDeleteService<UserFollows> followDeleteService = followDeleteService;
        private readonly IDeleteService<WorkoutPin> workoutPinDeleteService = workoutPinDeleteService;
        private readonly IDeleteService<SplitPin> splitPinDeleteService = splitPinDeleteService;
        private readonly ITokenManager tokenManager = tokenManager;
        private readonly IEmailConfirmationSenderService emailConfirmationSender = emailConfirmationSender;
        private readonly IEmailConfirmationService emailConfirmationService = emailConfirmationService;
        private readonly IResetPasswordEmailSenderService passwordResetEmailSender = passwordResetEmailSender;
        private readonly IResetPasswordService passwordResetEmailService = passwordResetEmailService;
        private readonly IRequestMapper<RegisterUserRequestDTO, User> registrationMapper = registrationMapper;
        private readonly IRequestMapper<CreateCompletedWorkoutRequestDTO, CompletedWorkout> createCompletedWorkoutRequestMapper = createCompletedWorkoutRequestMapper;
        private readonly IResponseMapper<User, SimpleUserResponseDTO> simpleResponseMapper = simpleResponseMapper;
        private readonly IResponseMapper<WorkoutPin, PinResponseDTO> workoutPinResponseMapper = workoutPinResponseMapper;
        private readonly IResponseMapper<SplitPin, PinResponseDTO> splitPinResponseMapper = splitPinResponseMapper;
        private readonly IResponseMapper<User, DetailedUserResponseDTO> detailedResponseMapper = detailedResponseMapper;
        private readonly IResponseMapper<Workout, DetailedWorkoutResponseDTO> detailedWorkoutResponseMapper = detailedWorkoutResponseMapper;
        private readonly IResponseMapper<User, DetailedPublicUserResponseDTO> publicUserDetailedResponseMapper = publicUserDetailedResponseMapper;
        private readonly IResponseMapper<UserSettings, UserSettingsResponseDTO> settingsResponseMapper = settingsResponseMapper;
        private readonly IResponseMapper<IGrouping<DateTime, CompletedWorkout>, SimpleWeekOfCompletedWorkoutsResponseDTO> simpleWeekOfCompletedWorkoutsResponseMapper = simpleWeekOfCompletedWorkoutsResponseMapper;
        private readonly IResponseMapper<IEnumerable<CompletedWorkout>, DetailedWeekOfCompletedWorkoutsResponseDTO> detailedWeekOfCompletedWorkoutsResponseMapper = detailedWeekOfCompletedWorkoutsResponseMapper;
        private readonly IResponseMapper<string, SimpleJWTResponseDTO> jwtResponseMapper = jwtResponseMapper;

        [GeneratedRegex(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$")]
        private static partial Regex ValidEmailRegex();
    }
}
